using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace KeyWordsExtractor.contract
{
    [DataContract]
    public class WeiboResult
    {

        [DataMember]
        public IList<WeiboStatuses> statuses { set; get; }

        [DataMember]
        public int total_number { get; set; }

    }
}