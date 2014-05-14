using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataProvider.contract
{
    [DataContract]
    public class FeedbackGroupDetail
    {
        [DataMember]
        public string user { set; get; }

        [DataMember]
        public string time { set; get; }

        [DataMember]
        public string feedback { set; get; }
    }
}
