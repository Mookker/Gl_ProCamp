using System.Threading.Tasks;
using CommonLibrary.Cqrs;
using ProCamp.Managers.Cache.Interfaces;
using ProCamp.Models;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Queries.Fixtures
{
    public class GetFixtureByIdQuery : IQuery<string, Fixture>
    {
        
        private readonly IFixturesRepository _fixturesRepository;
        private readonly IFixturesCacheManager _fixturesCacheManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fixturesRepository"></param>
        /// <param name="fixturesCacheManager"></param>
        public GetFixtureByIdQuery(IFixturesRepository fixturesRepository, IFixturesCacheManager fixturesCacheManager)
        {
            _fixturesRepository = fixturesRepository;
            _fixturesCacheManager = fixturesCacheManager;
        }

        /// <inheritdoc />
        public async Task<Fixture> ExecuteAsync(string fixtureId)
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
    }
}