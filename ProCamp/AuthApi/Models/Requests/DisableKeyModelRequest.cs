using System.Runtime.Serialization;

namespace AuthApi.Models.Requests
{
    [DataContract]
    public class DisableKeyModelRequest
    {
        [DataMember]
        public string Key { get; set; }
    }
}