using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProCamp.Managers.Interfaces;
using ProCamp.Models;
using ProCamp.Models.Requests;
using ProCamp.Models.Search;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Managers
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