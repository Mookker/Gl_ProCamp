using System;
using System.Threading.Tasks;

namespace CommonLibrary.Cache.Interfaces
{
    public interface IBaseCache
    {
        Task<T> GetObjectFromCache<T>(string cacheKey) where T: class, new();
        Task<bool> SetObjectToCache<T>(string cacheKey, T value, TimeSpan? expiry) where T: class, new();

        Task<string> GetStringFromCache(string cacheKey);
        Task<bool> SetStringToCache(string cacheKey, string value, TimeSpan? expiry);
    }
}