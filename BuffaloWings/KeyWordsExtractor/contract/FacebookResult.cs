using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{
    [DataContract]
    public class FacebookResult
    {
        [DataMember]
        public FacebookPost posts { get; set; }
    }
}