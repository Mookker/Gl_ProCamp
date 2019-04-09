using CommonLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Models
{
    /// <summary>
    /// Api key model
    /// </summary>
    public class ApiKeyModel : BaseModelWithId
    {
        
        /// <summary>
        /// Key itself
        /// </summary>
        [BsonElement("key")]
        public string Key { get; set; }
        
        /// <summary>
        /// Is valid status
        /// </summary>
        [BsonElement("isValid")]
        public bool IsValid { get; set; }
    }
}