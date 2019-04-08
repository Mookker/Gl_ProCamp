using System;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProCamp.Managers.Interfaces;
using ProCamp.Repositories.Interfaces;

namespace ProCamp.Managers
{
    /// <summary>
    /// Api key manager
    /// </summary>
    public class ApiKeyManager : IApiKeyManager
    {
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IApiKeyRepository _apiKeyRepository;
        private readonly IOptions<ApiKeyManagerOptions> _apiKeyManagerOptions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="passwordHasher"></param>
        /// <param name="apiKeyRepository"></param>
        /// <param name="apiKeyManagerOptions"></param>
        public ApiKeyManager(IPasswordHasher<string> passwordHasher, 
            IApiKeyRepository apiKeyRepository,
            IOptions<ApiKeyManagerOptions> apiKeyManagerOptions)
        {
            _passwordHasher = passwordHasher;
            _apiKeyRepository = apiKeyRepository;
            _apiKeyManagerOptions = apiKeyManagerOptions;
        }

        /// <inheritdoc />
        public async Task<string> GenerateKey()
        {
            var key = GenerateApiKey();
            var result = await _apiKeyRepository.AddKey(key);

            return result.Key;
        }

        /// <inheritdoc />
        public async Task<bool> IsKeyValid(string key)
        {
            return ValidateApiKey(key) && await _apiKeyRepository.IsKeyValid(key);
        }

        /// <inheritdoc />
        public async Task<bool> InvalidateKey(string key)
        {
            return await _apiKeyRepository.InvalidateKey(key);
        }

        private string GenerateApiKey()
        {
            var guid = Guid.NewGuid().ToString("N");
            var key = _passwordHasher.HashPassword(guid, _apiKeyManagerOptions.Value.Password);
            var apiKey = $"{guid}.{key}";
            var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey));
            
            return encodedKey;
        }

        private bool ValidateApiKey(string apiKey)
        {
            var decodedKeyBytes = Convert.FromBase64String(apiKey);
            var decodedKey = Encoding.UTF8.GetString(decodedKeyBytes);
            var parts = decodedKey?.Split('.');
            if (parts == null || parts.Length != 2)
                return false;

            var result = _passwordHasher.VerifyHashedPassword(parts[0], parts[1], _apiKeyManagerOptions.Value.Password);

            return result == PasswordVerificationResult.Success;
        }
    }
}