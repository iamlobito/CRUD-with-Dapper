using DapperCrud.Model;

namespace DapperCrud.Repositories
{
    public interface ISuperHeroRepository
    {
        Task<IEnumerable<SuperHero>> GetAll();
        Task<SuperHero> Get(int id);
        Task<IEnumerable<SuperHero>> Create(SuperHero entity);
        Task<IEnumerable<SuperHero>> Update(SuperHero entity);
        Task<IEnumerable<SuperHero>> Delete(int id);
    }
}
