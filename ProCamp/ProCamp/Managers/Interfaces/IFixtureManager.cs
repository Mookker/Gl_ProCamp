using System.Collections.Generic;
using System.Threading.Tasks;
using ProCamp.Models;
using ProCamp.Models.Search;

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
        /// Creates fixture
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        Task<Fixture> CreateFixture(Fixture fixture);

        /// <summary>
        /// Replaces or create new fixture
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        Task<bool> ReplaceFixture(Fixture fixture);

        /// <summary>
        /// Removes fixture
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveFixture(string id);

        /// <summary>
        /// Adds fake data
        /// </summary>
        /// <returns></returns>
        Task Seed();

        /// <summary>
        /// Gets multiple fixtures
        /// </summary>
        /// <param name="fixturesSearchOptions"></param>
        /// <returns></returns>
        Task<List<Fixture>> GetMultiple(FixturesSearchOptions fixturesSearchOptions);
    }
}