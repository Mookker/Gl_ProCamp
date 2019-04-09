using System.Collections.Generic;
using System.Threading.Tasks;
using FixturesApi.Managers.Cache.Interfaces;
using FixturesApi.Managers.Interfaces;
using FixturesApi.Models;
using FixturesApi.Repositories.Interfaces;

namespace FixturesApi.Managers
{
    class FixtureManager : IFixtureManager
    {
        private readonly IFixturesRepository _fixturesRepository;
        private readonly IFixturesCacheManager _fixturesCacheManager;

        public FixtureManager(IFixturesRepository fixturesRepository, IFixturesCacheManager fixturesCacheManagerManager)
        {
            _fixturesRepository = fixturesRepository;
            _fixturesCacheManager = fixturesCacheManagerManager;
        }

        public async Task<Fixture> GetFixture(string fixtureId)
        {
            var cached = await _fixturesCacheManager.GetFixture(fixtureId);
            if (cached == null)
            {
                var result = await _fixturesRepository.GetById(fixtureId);
                if (result != null)
                {
                    await _fixturesCacheManager.AddFixture(result);
                }
                
                return result;
            }

            return cached;
        }

        
        public Task<List<NearestFixture>> GetNearestFixtures(double longitude, double latitude, int offset, int limit)
        {
            return _fixturesRepository.GetNearestFixtures(longitude, latitude, offset, limit);
        }
    }
}