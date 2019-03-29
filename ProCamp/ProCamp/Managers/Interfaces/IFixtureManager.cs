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
    }
}