using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BuffaloWingsWebSite.Models
{
    [DataContract]
    public class FBBasic
    {
        [DataMember]
        public string id { set; get; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public FBAvatar picture { get; set; }
    }
}