using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using KeyWordsExtractor.contract;

namespace KeyWordsExtractor.contract
{
    [DataContract]
    public class FacebookPost
    {
        [DataMember]
        public IList<FacebookData> data { get; set; }

        [DataMember]
        public FacebookPaging paging { get; set; }
    }
}