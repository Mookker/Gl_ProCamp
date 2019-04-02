using MongoDB.Bson.Serialization.Attributes;

namespace CommonLibrary.Models
{
    public class BaseModelWithId
    {
        [BsonElement("_id")]
        public string Id { get; set; }
    }
}