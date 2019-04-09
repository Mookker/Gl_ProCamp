using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibrary.Repositories.Implementations;
using FixturesApi.Models;
using FixturesApi.Models.Search;
using FixturesApi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace FixturesApi.Repositories.Implementations
{
    /// <inheritdoc cref="IFixturesRepository" />
    public class InMemoryFixturesRepository : BaseInMemoryRepository<Fixture, FixturesSearchOptions>, IFixturesRepository
    {
        private readonly ILogger<InMemoryFixturesRepository> _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        public InMemoryFixturesRepository(ILogger<InMemoryFixturesRepository> logger)
        {
            _logger = logger;
            Data = new List<Fixture>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override Func<Fixture, bool> GetSearchDefinition(FixturesSearchOptions searchOptions)
        {
            Predicate<Fixture> result = fixture => true;


            if (!String.IsNullOrWhiteSpace(searchOptions.Id))
            {
                result = fixture => fixture.Id == searchOptions.Id;
            }
            
            if (!String.IsNullOrWhiteSpace(searchOptions.HomeTeamName))
            {
                var previousResult = result;
                result = (fixture => fixture.HomeTeamName == searchOptions.HomeTeamName && previousResult(fixture));
            }

            if (!String.IsNullOrWhiteSpace(searchOptions.AwayTeamName))
            {
                var previousResult = result;
                result = (fixture => fixture.AwayTeamName == searchOptions.AwayTeamName && previousResult(fixture));
            }

            if (searchOptions.DateTo.HasValue)
            {
                var previousResult = result;
                result = (fixture => fixture.Date <= searchOptions.DateTo.Value && previousResult(fixture));
            }

            if (searchOptions.DateFrom.HasValue)
            {
                var previousResult = result;
                result = (fixture => fixture.Date >= searchOptions.DateFrom.Value && previousResult(fixture));
            }

            return result.Invoke;
        }

        /// <inheritdoc />
        public Task<List<NearestFixture>> GetNearestFixtures(double longitude, double latitude, int offset = 0,
            int limit = 10)
        {
            throw new NotImplementedException();
        }
    }
}