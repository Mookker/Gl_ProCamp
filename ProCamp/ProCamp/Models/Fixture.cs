using System;

namespace ProCamp.Models
{
    /// <summary>
    /// Simple fixture
    /// </summary>
    public class Fixture
    {
        /// <summary>
        /// Id of fixture
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Date of game
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Name of home team
        /// </summary>
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Name of away team
        /// </summary>
        public string AwayTeamName { get; set; }
    }
}