using System;
using System.Collections.Generic;
using CommonLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;

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
        [BsonElement("date")]
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Name of home team
        /// </summary>
        [BsonElement("homeTeamName")]
        public string HomeTeamName { get; set; }
        
        /// <summary>
        /// Name of away team
        /// </summary>
        [BsonElement("awayTeamName")]
        public string AwayTeamName { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        [BsonElement("location")]
        public List<double> Location { get; set; }
    }
}