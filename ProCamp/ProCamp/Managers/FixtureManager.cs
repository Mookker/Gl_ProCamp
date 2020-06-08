using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Managers
{
    class FixtureManager : IFixtureManager
    {
        private readonly IFixturesRepository _fixturesRepository;
        private readonly IFixturesCacheManager _fixturesCacheManager;
        private static List<Fixture> _fixtures = new List<Fixture>
        {
            new Fixture
            {
                Id = "1",
                AwayTeamName = "ManCity",
                HomeTeamName = "Fulham",
                Date = new DateTime(2019, 3, 30, 12, 30, 0)
            },
            new Fixture
            {
                Id = Guid.NewGuid().ToString("N"),
                AwayTeamName = "Cardiff City",
                HomeTeamName = "ManCity",
                Date = new DateTime(2019, 4, 3, 19, 45, 0)
            },
            
            new Fixture
            {
                Id = Guid.NewGuid().ToString("N"),
                AwayTeamName = "ManCity",
                HomeTeamName = "Brighton",
                Date = new DateTime(2019, 4, 6, 17, 30, 0)
            },
            
        };
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

        public async Task<bool> CreateFixture(Fixture fixture)
        {
            if (!string.IsNullOrWhiteSpace(fixture.Id))
            {
                var result = await _fixturesRepository.GetById(fixture.Id);
                if (result != null)
                {
                    throw new ArgumentException("Fixture already exist");
                }
            }
            else
            {
                fixture.Id = Guid.NewGuid().ToString("N");
            }

            var success = await _fixturesRepository.Create(fixture);
            if (success)
            {
                await _fixturesCacheManager.AddFixture(fixture);
            }

            return success;
        }

        public async Task<bool> ReplaceFixture(Fixture fixture)
        {
            var existing = await _fixturesRepository.GetById(fixture.Id);
            if (existing != null)
            {
                await _fixturesRepository.Replace(fixture);
            }
            else
            {
                await _fixturesRepository.Create(fixture);
            }

            await _fixturesCacheManager.AddFixture(fixture);

            return true;
        }

        public async Task<bool> RemoveFixture(string id)
        {
            await _fixturesCacheManager.RemoveFixture(id);
            var result = await _fixturesRepository.Remove(id);

            return result;
        }

        public async Task Seed()
        {
            foreach (var fixture in _fixtures)
            {
                if (await _fixturesRepository.Exists(fixture.Id))
                {
                    await _fixturesRepository.Replace(fixture);
                }
                else
                {
                    await _fixturesRepository.Create(fixture);
                }
            }
        }

        public Task<List<Fixture>> GetMultiple(FixturesSearchOptions fixturesSearchOptions)
        {
            return _fixturesRepository.GetMultiple(fixturesSearchOptions);
        }
    }
}