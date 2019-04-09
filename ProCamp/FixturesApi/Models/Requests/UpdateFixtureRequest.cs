using System;
using Newtonsoft.Json;

namespace FixturesApi.Models.Requests
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class UpdateFixtureRequest
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }
        /// <summary>
        /// Date of game
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Name of home team
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Name of away team
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string AwayTeamName { get; set; }
    }
}