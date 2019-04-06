using System;
using Newtonsoft.Json;

namespace ProCamp.Models.Responses
{
    /// <summary>
    /// Login response
    /// </summary>
    [Serializable]
    public class LoginResponse
    {
        /// <summary>
        /// Auth token for further requests
        /// </summary>
        [JsonProperty]
        public string AuthToken { get; set; }
        
        /// <summary>
        /// Used to refresh login token
        /// </summary>
        [JsonProperty]
        public string RefreshToken { get; set; }
    }
}