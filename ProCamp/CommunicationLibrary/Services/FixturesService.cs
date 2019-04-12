using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary.Constants;
using CommunicationLibrary.Extensions;
using CommunicationLibrary.Models.Responses;
using CommunicationLibrary.Services.Interfaces;

namespace CommunicationLibrary.Services
{
    public class FixturesService : IFixtureService
    {
        private readonly HttpClient _httpClient;

        public FixturesService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(ServiceNames.Fixtures);
        }

        public Task<FixturesResponse> GetFixture(string id)
        {
            return _httpClient.GetJsonResponse<FixturesResponse>($"api/v1/fixtures/{id}");
        }

        public void Authorize(string jwt)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }
    }
}