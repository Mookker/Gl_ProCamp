using CommonLibrary.Repositories.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Search;

namespace ProCamp.Repositories.Interfaces
{
    /// <summary>
    /// Repo for fixtures
    /// </summary>
    public interface IFixturesRepository :  IBaseRepository<Fixture, FixturesSearchOptions>
    {
        
    }
}