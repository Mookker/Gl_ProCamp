using System.Net.Http;
using System.Threading.Tasks;
using CommonLibrary.Constants;
using CommonLibrary.Helpers;
using CommunicationLibrary.Extensions;
using CommunicationLibrary.Models.Responses;
using CommunicationLibrary.Services.Interfaces;

namespace CommunicationLibrary.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(ServiceNames.Auth);
        }
        
        public async Task<string> GetApiKeyJwt(string apiKey)
        {
            _httpClient.DefaultRequestHeaders.Remove(AuthConstants.ApiKeyHeaderName);
            _httpClient.DefaultRequestHeaders.Add(AuthConstants.ApiKeyHeaderName, apiKey);
            var response = await _httpClient.GetJsonResponse<ApiKeyJwtResponse>("api/v1/auth/jwt");

            return response?.Token;
        }
    }
}