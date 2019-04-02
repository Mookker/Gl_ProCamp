using System.Collections.Generic;
using System.Threading.Tasks;
using ProCamp.Models;

namespace ProCamp.Managers.Interfaces
{
    /// <summary>
    /// Fixture manager
    /// </summary>
    public interface IFixtureManager
    {
        /// <summary>
        /// Gets fixture by id
        /// </summary>
        /// <param name="fixtureId"></param>
        /// <returns></returns>
        Task<Fixture> GetFixture(string fixtureId);

        /// <summary>
        /// Gets nearest fixtures
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<NearestFixture>> GetNearestFixtures(double longitude, double latitude, int offset, int limit);
    }
}