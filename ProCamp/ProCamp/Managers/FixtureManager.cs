using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Managers
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
                await _fixturesCacheManager.AddFixture(result);
                
                return result;
            }

            return cached;
        }
    }
}