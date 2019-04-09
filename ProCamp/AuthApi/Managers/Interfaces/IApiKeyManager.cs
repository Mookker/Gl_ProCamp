using System.Threading.Tasks;

namespace AuthApi.Managers.Interfaces
{
    public interface IApiKeyManager
    {
        /// <summary>
        /// Creates new key
        /// </summary>
        /// <returns></returns>
        Task<string> GenerateKey();
        
        /// <summary>
        /// Checks key is valid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> IsKeyValid(string key);
        
        /// <summary>
        /// Marks key as invalid
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> InvalidateKey(string key);
    }
}