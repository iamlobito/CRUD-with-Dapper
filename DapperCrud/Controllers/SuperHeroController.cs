using Dapper;
using DapperCrud.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SuperHeroController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperheroes()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var heroes = await connection.QueryAsync<SuperHero>("select *from superheroes");
            return Ok(heroes);
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> GetHeroById(int heroId)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var heroes = await connection.QueryFirstAsync<SuperHero>("select *from superheroes where id = @id", new {id = heroId });
            return Ok(heroes);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> CreateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
            return Ok(await SelectAllSuperHeroes(connection));
        }

        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update superheroes set name = @Name, firstName = @FirstName, lastname = @LastName, place = @Place where id = @Id", hero);
            return Ok(await SelectAllSuperHeroes(connection));
        }

        [HttpDelete("{heroId}")]
        public async Task<ActionResult<SuperHero>> DeleteHero(int heroId)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from superheroes where id = @Id",  new { Id = heroId });
            return Ok(await SelectAllSuperHeroes(connection));
        }

        private static async Task<IEnumerable<SuperHero>> SelectAllSuperHeroes(SqlConnection connection)
        {
            return await connection.QueryAsync<SuperHero>("select *from superheroes");
        }
    }
}
