using System.Threading.Tasks;
using AuthApi.Models;

namespace AuthApi.Repositories.Interfaces
{
    public interface IApiKeyRepository
    {
        /// <summary>
        /// Saves key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Success or not</returns>
        Task<ApiKeyModel> AddKey(string key);
        
        /// <summary>
        /// Checks key is valid
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Valid or not</returns>
        Task<bool> IsKeyValid(string key);
        
        /// <summary>
        /// Marks key as invalid
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Success or not</returns>
        Task<bool> InvalidateKey(string key);
    }
}