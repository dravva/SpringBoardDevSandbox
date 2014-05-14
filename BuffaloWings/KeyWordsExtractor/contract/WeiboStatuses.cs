using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{

    [DataContract]
    public class WeiboStatuses
    {
        [DataMember]
        public string text { set; get; }

        [DataMember]
        public WeiboRetweet retweeted_status { get; set; }
    }
}