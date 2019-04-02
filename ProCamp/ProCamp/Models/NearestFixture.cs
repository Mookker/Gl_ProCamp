using MongoDB.Bson.Serialization.Attributes;

namespace ProCamp.Models
{
    /// <summary>
    /// Nearest fixture
    /// </summary>
    public class NearestFixture : Fixture
    {
        /// <summary>
        /// Distance
        /// </summary>
        [BsonElement("distance")]
        public double Distance { get; set; }
    }
}