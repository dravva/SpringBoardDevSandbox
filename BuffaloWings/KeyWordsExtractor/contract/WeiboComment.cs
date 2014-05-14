using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{
    [DataContract]
    public class WeiboComment
    {

        [DataMember]
        public WeiboStatuses status { set; get; }

    }
}