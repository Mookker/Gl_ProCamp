using CommonLibrary.Repositories.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Search;

namespace ProCamp.Repositories.Interfaces
{
    public interface IFixturesRepository :  IBaseRepository<Fixture, FixturesSearchOptions>
    {
        
    }
}