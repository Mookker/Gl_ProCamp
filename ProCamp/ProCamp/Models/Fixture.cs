using System;
using CommonLibrary.Models;

namespace ProCamp.Models
{
    /// <summary>
    /// Simple fixture
    /// </summary>
    public class Fixture : BaseModelWithId
    {
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