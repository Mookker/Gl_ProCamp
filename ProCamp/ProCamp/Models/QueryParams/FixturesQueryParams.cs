using System;
using Newtonsoft.Json;

namespace ProCamp.Models.QueryParams
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FixturesQueryParams
    {
        /// <summary>
        /// Id
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// Home team
        /// </summary>
        [JsonProperty]
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Away team 
        /// </summary>
        [JsonProperty]
        public string AwayTeamName { get; set; }
        
        
        /// <summary>
        /// Minimum time
        /// </summary>
        [JsonProperty]
        public DateTime? DateFrom { get; set; }
        
        /// <summary>
        /// Max time
        /// </summary>
        [JsonProperty]
        public DateTime? DateTo { get; set; }
        
    }
}