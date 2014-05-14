using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{
    [DataContract]
    public class FacebookPaging
    {
        [DataMember]
        public string next { get; set; }

        [DataMember]
        public string previous { get; set; }
    }
}