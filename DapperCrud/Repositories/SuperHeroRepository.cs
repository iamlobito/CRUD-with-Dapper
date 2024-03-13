using DapperCrud.Model;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
namespace DapperCrud.Repositories
{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;


        public SuperHeroRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<SuperHero>> Create(SuperHero hero)
        {
            await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();
            await sqlConnection.ExecuteAsync("insert into superheroes (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
            return await SelectAllSuperHeroes(sqlConnection);
        }

        public async Task<IEnumerable<SuperHero>> Delete(int heroId)
        {
            await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();
            await sqlConnection.ExecuteAsync("delete from superheroes where id = @Id", new { Id = heroId });
            return await SelectAllSuperHeroes(sqlConnection);
        }

        public async Task<SuperHero> Get(int heroId)
        {
            await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();
            return await sqlConnection.QueryFirstAsync<SuperHero>("select *from superheroes where id = @id", new { id = heroId });
        }

        public async Task<IEnumerable<SuperHero>> GetAll()
        {
            await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();
            return await sqlConnection.QueryAsync<SuperHero>("select *from superheroes");
        }

        public async Task<IEnumerable<SuperHero>> Update(SuperHero hero)
        {
            await using SqlConnection sqlConnection = _sqlConnectionFactory.CreateSqlConnection();
            var result = await sqlConnection.ExecuteAsync("update superheroes set name = @Name, firstName = @FirstName, lastname = @LastName, place = @Place where id = @Id", hero);
            return await SelectAllSuperHeroes(sqlConnection);
        }

        private static async Task<IEnumerable<SuperHero>> SelectAllSuperHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("select *from superheroes");
        }
    }
}
