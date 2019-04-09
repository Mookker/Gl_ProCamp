using System.Threading.Tasks;
using AuthApi.Models;
using AuthApi.Models.Requests;

namespace AuthApi.Managers.Interfaces
{
    /// <summary>
    /// User manager
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Checks if login valid
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckLoginValid(string login, string password);

        /// <summary>
        /// Gets user by login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<User> GetUserByLogin(string login);

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<User> CreateUser(LoginRequest loginRequest);
    }
}