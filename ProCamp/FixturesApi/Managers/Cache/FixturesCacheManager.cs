using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Cache.Interfaces;
using FixturesApi.Managers.Cache.Interfaces;
using FixturesApi.Models;

namespace FixturesApi.Managers.Cache
{
    class FixturesCacheManager : IFixturesCacheManager
    {
        private readonly IBaseCache _baseCache;

        public FixturesCacheManager(IBaseCache baseCache)
        {
            _baseCache = baseCache;
        }
        
        public Task<bool> AddFixture(Fixture fixture)
        {
            return _baseCache.SetObjectToCache(GetFixtureCacheKey(fixture.Id), fixture, TimeSpan.FromMinutes(5));
        }

        public Task<Fixture> GetFixture(string id)
        {
            return _baseCache.GetObjectFromCache<Fixture>(GetFixtureCacheKey(id));
        }

        private string GetFixtureCacheKey(string fixtureId)
        {
            return $"fixture:{fixtureId}";
        }
    }
}