using System.Collections.Generic;
using System.Threading.Tasks;
using FixturesApi.Models;

namespace FixturesApi.Managers.Cache.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFixturesCacheManager
    {
        /// <summary>
        /// Adds to cache
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        Task<bool> AddFixture(Fixture fixture);

        /// <summary>
        /// Gets from cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Fixture> GetFixture(string id);
    }
}