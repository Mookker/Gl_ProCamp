using System;
using System.Net.Http;
using System.Threading.Tasks;
using CommunicationLibrary.Exceptions;
using Newtonsoft.Json;

namespace CommunicationLibrary.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonResponse<T>(this HttpClient client, string uri) where T : class, new()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<T>(responseBody);

                return responseObj;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServiceException(ex);
            }
        }
    }
}