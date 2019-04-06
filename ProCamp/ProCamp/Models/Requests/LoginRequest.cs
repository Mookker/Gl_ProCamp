using System;
using Newtonsoft.Json;

namespace ProCamp.Models.Requests
{
    [Serializable]
    public class LoginRequest
    {
        /// <summary>
        /// Login
        /// </summary>
        [JsonProperty]
        public string Login { get; set; }
        
        /// <summary>
        /// Password 
        /// </summary>
        [JsonProperty]
        public string Password { get; set; }
    }
}