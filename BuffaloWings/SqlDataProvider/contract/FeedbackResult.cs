using System.Runtime.Serialization;

namespace SqlDataProvider.contract
{
    [DataContract]
    public class FeedbackResult
    {
        [DataMember]
        public string status { get; set; }

        [DataMember]
        public int total { get; set; }

        [DataMember]
        public int up { get; set; }

        [DataMember]
        public int down { get; set; }

    }
}