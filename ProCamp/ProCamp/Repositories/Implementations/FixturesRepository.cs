using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using ProCamp.Models;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Repositories.Implementations
{
    /// <inheritdoc cref="IFixturesRepository" />
    public class FixturesRepository : InMemoryRepository<Fixture, FixturesSearchOptions>, IFixturesRepository
    {
        private readonly ILogger<FixturesRepository> _logger;

        /// <summary>
        /// Ctor
        /// </summary>
        public FixturesRepository(ILogger<FixturesRepository> logger)
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
                result = (fixture => fixture.AwayTeamName == searchOptions.HomeTeamName && previousResult(fixture));
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
    }
}