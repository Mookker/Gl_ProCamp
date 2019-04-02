using System;
using CommonLibrary.Config;
using CommonLibrary.Repositories.Implementations;
using CommonLibrary.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProCamp.Models;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Repositories.Implementations
{
    /// <inheritdoc cref="IFixturesRepository" />
    public class MongoDbFixturesRepository : BaseMongoDbRepository<Fixture, FixturesSearchOptions>, IFixturesRepository
    {
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client"></param>    
        public MongoDbFixturesRepository(IOptions<MongoConfiguration> configuration, IMongoClient client)
        {
            _db = client.GetDatabase(configuration.Value.DbName);
        }

        /// <inheritdoc />
        protected override IMongoCollection<Fixture> Collection => _db.GetCollection<Fixture>("fixtures");

        /// <inheritdoc />
        protected override FilterDefinition<Fixture> GetMultipleItemsQuery(FixturesSearchOptions searchOptions)
        {
            var query = FilterDefinition<Fixture>.Empty;
            if (searchOptions != null)
            {
                if (!String.IsNullOrEmpty(searchOptions.Id))
                    query &= Builders<Fixture>.Filter.Eq(c => c.Id, searchOptions.Id);
                if (!String.IsNullOrEmpty(searchOptions.AwayTeamName))
                    query &= Builders<Fixture>.Filter.Eq(c => c.AwayTeamName, searchOptions.AwayTeamName);
                if (!String.IsNullOrEmpty(searchOptions.HomeTeamName))
                    query &= Builders<Fixture>.Filter.Eq(c => c.AwayTeamName, searchOptions.HomeTeamName);
                if (searchOptions.DateTo.HasValue)
                    query &= Builders<Fixture>.Filter.Lte(c => c.Date, searchOptions.DateTo.Value);
                if (searchOptions.DateFrom.HasValue)
                    query &= Builders<Fixture>.Filter.Gte(c => c.Date, searchOptions.DateFrom.Value);
            }

            return query;
        }
    }
}