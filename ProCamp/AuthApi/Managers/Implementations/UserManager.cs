using System;
using System.Threading.Tasks;
using AuthApi.Managers.Interfaces;
using AuthApi.Models;
using AuthApi.Models.Requests;
using AuthApi.Models.Search;
using AuthApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuthApi.Managers.Implementations
{
    class UserManager : IUserManager
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserManager(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher)
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> CheckLoginValid(string login, string password)
        {
            var user = await GetUserByLogin(login);
            if (user == null)
                return false;
            
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            
            return result == PasswordVerificationResult.Success;
        }

        public Task<User> GetUserByLogin(string login)
        {
            return _usersRepository.GetFirstOrDefault(new UserSearchOptions {Login = login});
        }

        public Task<User> CreateUser(LoginRequest loginRequest)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString("N"),
                Login = loginRequest.Login,
                PasswordHash = loginRequest.Password
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, loginRequest.Password);

            return _usersRepository.Create(newUser);

        }
    }
}