using System;
using System.Threading.Tasks;
using CommonLibrary.Cache.Interfaces;
using CommonLibrary.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CommonLibrary.Cache.Implementations
{
    public class BaseCache : IBaseCache
    {
        private readonly ILogger<BaseCache> _logger;

        private readonly IDatabase _redisDb;
        private readonly string _prefix;
        public BaseCache(IOptions<RedisCacheConfiguration> redisCfgOptions, ILogger<BaseCache> logger)
        {
            _logger = logger;
            try
            {
                var connection = ConnectionMultiplexer.Connect(redisCfgOptions.Value.ConnectionString);
                _redisDb = connection.GetDatabase();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "cannot connect to Redis");
            }
            _prefix = $"{redisCfgOptions.Value.Environment ?? ""}:{redisCfgOptions.Value.ApiName ?? ""}";
        }

        public async Task<T> GetObjectFromCache<T>(string cacheKey) where T : class, new()
        {
            try
            {
                var jsonObj = await _redisDb.StringGetAsync($"{_prefix}:" + cacheKey);
                if(!String.IsNullOrWhiteSpace(jsonObj))
                    return JsonConvert.DeserializeObject<T>(jsonObj);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"error in getting {cacheKey}");
            }

            return null;
        }

        public async Task<bool> SetObjectToCache<T>(string cacheKey, T value, TimeSpan? expiry) where T : class, new()
        {
            try
            {
                var jsonObj = JsonConvert.SerializeObject(value);
                
                return await _redisDb.StringSetAsync($"{_prefix}:" + cacheKey, jsonObj, expiry);

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"error in setting {cacheKey}");
            }

            return false;
        }

        public async Task<string> GetStringFromCache(string cacheKey)
        {
            try
            {
                var value = await _redisDb.StringGetAsync("prefix:" + cacheKey);

                return value;

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"error in getting {cacheKey}");
            }

            return null;
        }

        public async Task<bool> SetStringToCache(string cacheKey, string value, TimeSpan? expiry)
        {
            try
            {
                return await _redisDb.StringSetAsync($"{_prefix}:" + cacheKey, value, expiry);

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"error in setting {cacheKey}");
            }

            return false;
        }
    }
}