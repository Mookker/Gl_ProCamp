using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Repositories.Interfaces;
using FixturesApi.Models;
using FixturesApi.Models.Search;

namespace FixturesApi.Repositories.Interfaces
{
    /// <summary>
    /// Repo for fixtures
    /// </summary>
    public interface IFixturesRepository :  IBaseRepository<Fixture, FixturesSearchOptions>
    {
        /// <summary>
        /// Gets 
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<NearestFixture>> GetNearestFixtures(double longitude, double latitude, int offset = 0, int limit = 10);
    }
}