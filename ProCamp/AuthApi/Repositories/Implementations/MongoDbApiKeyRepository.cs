using System;
using System.Threading.Tasks;
using AuthApi.Models;
using AuthApi.Repositories.Interfaces;
using CommonLibrary.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Repositories.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoDbApiKeyRepository : IApiKeyRepository
    {
        private readonly IMongoDatabase _db;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="client"></param>    
        public MongoDbApiKeyRepository(IOptions<MongoConfiguration> configuration, IMongoClient client)
        {
            _db = client.GetDatabase(configuration.Value.DbName);
        }

        /// <summary>
        /// Key collection
        /// </summary>
        protected IMongoCollection<ApiKeyModel> KeyCollection => _db.GetCollection<ApiKeyModel>("apiKeys");
        
        /// <inheritdoc />
        public async Task<ApiKeyModel> AddKey(string key)
        {
            var apiKeyModel = new ApiKeyModel {Id = Guid.NewGuid().ToString("N"), IsValid = true, Key = key};
            await KeyCollection.InsertOneAsync(apiKeyModel);

            return apiKeyModel;
        }

        /// <inheritdoc />
        public async Task<bool> IsKeyValid(string key)
        {
            var existingKey = await KeyCollection.Find(k => k.Key == key).FirstOrDefaultAsync();

            return existingKey?.IsValid ?? false;
        }

        /// <inheritdoc />
        public async Task<bool> InvalidateKey(string key)
        {
            var result = await KeyCollection.UpdateOneAsync(k => k.Key == key, Builders<ApiKeyModel>.Update.Set(k => k.IsValid, false));

            return result.ModifiedCount > 0;
        }
    }
}