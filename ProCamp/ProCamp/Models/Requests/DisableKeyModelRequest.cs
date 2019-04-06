using System.Runtime.Serialization;

namespace ProCamp.Models.Requests
{
    [DataContract]
    public class DisableKeyModelRequest
    {
        [DataMember]
        public string Key { get; set; }
    }
}