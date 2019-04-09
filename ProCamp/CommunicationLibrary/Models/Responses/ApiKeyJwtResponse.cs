using Newtonsoft.Json;

namespace CommunicationLibrary.Models.Responses
{
    /// <summary>
    /// Reponse
    /// </summary>
    public class ApiKeyJwtResponse
    {
        /// <summary>
        /// Token
        /// </summary>
        [JsonProperty]
        public string Token { get; set; }
    }
}