using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{

    [DataContract]
    public class WeiboRetweet
    {

        [DataMember]
        public string text { set; get; }

    }
}