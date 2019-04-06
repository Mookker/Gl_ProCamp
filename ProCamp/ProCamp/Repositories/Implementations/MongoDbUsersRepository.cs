using System;
using CommonLibrary.Config;
using CommonLibrary.Repositories.Implementations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProCamp.Models;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Repositories.Implementations
{
    class MongoDbUsersRepository : BaseMongoDbRepository<User, UserSearchOptions>, IUsersRepository
    {
        private readonly IMongoDatabase _db;

        public MongoDbUsersRepository(IOptions<MongoConfiguration> configuration, IMongoClient client)
        {
            _db = client.GetDatabase(configuration.Value.DbName);
        }

        protected override IMongoCollection<User> Collection => _db.GetCollection<User>("users");
        protected override FilterDefinition<User> GetMultipleItemsQuery(UserSearchOptions searchOptions)
        {
            var query = FilterDefinition<User>.Empty;
            if (searchOptions != null)
            {
                if (!String.IsNullOrEmpty(searchOptions.Id))
                    query &= Builders<User>.Filter.Eq(c => c.Id, searchOptions.Id);
                if (!String.IsNullOrEmpty(searchOptions.Login))
                    query &= Builders<User>.Filter.Regex(c => c.Login, $"/{searchOptions.Login}/i");
            }

            return query;
        }
    }
}