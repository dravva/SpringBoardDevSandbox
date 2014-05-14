using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BuffaloWingsWebSite.Models
{
    [DataContract]
    public class WeiboToken
    {
        [DataMember]
        public string access_token { set; get; }

        [DataMember]
        public string remind_in { get; set; }

        [DataMember]
        public string expires_in { get; set; }

        [DataMember]
        public string uid { get; set; }
    }
}