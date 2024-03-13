using Dapper;
using DapperCrud.Model;
using DapperCrud.Repositories;
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
        private readonly ISuperHeroRepository _superHeroRepository;

        public SuperHeroController(IConfiguration configuration, ISuperHeroRepository superHeroRepository)
        {
            _configuration = configuration;
            _superHeroRepository = superHeroRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperheroes()
        {
            var heroes = await _superHeroRepository.GetAll();
            return Ok(heroes);
        }

        [HttpGet("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> GetHeroById(int heroId)
        {
            var heroes = await _superHeroRepository.Get(heroId);
            return Ok(heroes);
        }

        [HttpPost]
        public async Task<ActionResult<SuperHero>> CreateHero(SuperHero hero)
        {
            var heroes = await _superHeroRepository.Create(hero);
            return Ok(heroes);
        }

        [HttpPut]
        public async Task<ActionResult<SuperHero>> UpdateHero(SuperHero hero)
        {
            var heroes = await _superHeroRepository.Update(hero);
            return Ok(heroes);
        }

        [HttpDelete("{heroId}")]
        public async Task<ActionResult<SuperHero>> DeleteHero(int heroId)
        {
            var heroes = await _superHeroRepository.Delete(heroId);
            return Ok(heroes);
        }
    }
}
