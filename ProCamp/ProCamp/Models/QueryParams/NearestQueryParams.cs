using System;
using Newtonsoft.Json;

namespace ProCamp.Models.QueryParams
{
    /// <summary>
    /// Nearest 
    /// </summary>
    [Serializable]
    public class NearestQueryParams
    {
        /// <summary>
        /// Lng
        /// </summary>
        [JsonProperty]
        public double Longitude { get; set; }
        
        /// <summary>
        /// Ltd
        /// </summary>
        [JsonProperty]
        public double Latitude { get; set; }

        /// <summary>
        /// Offset
        /// </summary>
        [JsonProperty]
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Limit
        /// </summary>
        [JsonProperty]
        public int Limit { get; set; } = 10;
    }
}