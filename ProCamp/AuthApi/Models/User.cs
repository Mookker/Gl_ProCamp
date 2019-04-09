using CommonLibrary.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthApi.Models
{
    /// <summary>
    /// User model
    /// </summary>
    public class User : BaseModelWithId
    {
        /// <summary>
        /// Login
        /// </summary>
        [BsonElement("login")]
        public string Login { get; set; }
        
        /// <summary>
        /// Password hash
        /// </summary>
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }
    }
}