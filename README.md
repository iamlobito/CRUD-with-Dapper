# DapperCrud

A cool and simple example of how to use Dapper in a .net core web api.

Here you will find the Crud method (CREATE; UPDATE; INSERT; DELETE) using a local sql server database.

In the folder SQL you will find the scripts to create the database and the table.

![image](https://github.com/iamlobito/DapperCrud/assets/162322058/5fa2da1d-aeca-4327-bd05-c4b0c33dce47)

## Redis

In this example I'm also using IDistributed Cache with Redis. This is just an example of the implementation to train and show how we can store and get values from redis cache in order to get a better performance for the get methods.

To implement this solution you MUST create a docker container with Redis. You could use Docker Container for Windows for example.

Package Required:
 - Microsoft.Extensions.Caching.StackExchangeRedis

 1 - Add the redis connection to your appsetting.json

![image](https://github.com/iamlobito/DapperCrud/assets/162322058/2b50e3dc-8c7a-40a7-9d2b-9ef9b5719832)

 2- Register the dependency in program.cs or in your startup.cs

```cs
 builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration
    .GetConnectionString("Redis");

    redisOptions.Configuration = connection;
});
```
3- In your controller or repository inject the IDistributedCache in the construtor
```cs
private readonly IDistributedCache _distributedCache;
public SuperHeroRepository(ISqlConnectionFactory sqlConnectionFactory, IDistributedCache distributedCache)
{
   _sqlConnectionFactory = sqlConnectionFactory;
   _distributedCache = distributedCache;
}
```

4- Create or logic to Get/Set the key/value in the redis cache
```cs
public async Task<SuperHero> Get(int heroId)
{
    string cacheKey = $"hero-{heroId}";

    string? cacheHero = await _distributedCache.GetStringAsync(cacheKey);

    await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();

    SuperHero? hero;
    if (String.IsNullOrEmpty(cacheHero))
    {
        
        hero = await sqlConnection.QueryFirstAsync<SuperHero>("select *from superheroes where id = @id", new { id = heroId });

        if (hero is null)
        {
            return hero;
        }

        await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(hero));

        return hero;
    }

    hero = JsonConvert.DeserializeObject<SuperHero>(cacheHero);

    return hero;
}
```

And that's it. Now, the first time that the Get method is called first check if exists in the cache. If exists, then get the value from the cache. If not goes to the database.
This cache strategy is called Cache ASide Pattern
Happy coding :)
