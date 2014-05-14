using System.Runtime.Serialization;
namespace KeyWordsExtractor.contract
{

    [DataContract]
    public class FacebookData
    {
        [DataMember]
        public string caption { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string message { get; set; }

        [DataMember]
        public string story { get; set; }
    }
}