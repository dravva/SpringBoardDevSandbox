using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.contract
{
    [DataContract]
    public class FeedbackGroupResult
    {
        [DataMember]
        public string category { get; set; }

        [DataMember]
        public string item { get; set; }

        [DataMember]
        public string feedback { get; set; }

        [DataMember]
        public int number { get; set; }
    }
}
