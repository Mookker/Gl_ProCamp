using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FixturesApi.Models.Responses
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FixturesResponse
    {
        /// <summary>
        /// Id of fixture
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }
        
        /// <summary>
        /// Date of game
        /// </summary>
        [JsonProperty]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Name of home team
        /// </summary>
        [JsonProperty]
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Name of away team
        /// </summary>
        [JsonProperty]
        public string AwayTeamName { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        [JsonProperty]
        public List<double> Location { get; set; }
    }
}