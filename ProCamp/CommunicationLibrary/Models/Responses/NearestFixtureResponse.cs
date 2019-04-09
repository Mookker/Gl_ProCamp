using Newtonsoft.Json;

namespace CommunicationLibrary.Models.Responses
{
    /// <summary>
    /// Nearest fixture
    /// </summary>
    public class NearestFixtureResponse : FixturesResponse
    {
        /// <summary>
        /// Distance
        /// </summary>
        [JsonProperty]
        public double Distance { get; set; }
    }
}