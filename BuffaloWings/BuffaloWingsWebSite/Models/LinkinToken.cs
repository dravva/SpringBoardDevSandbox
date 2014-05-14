using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BuffaloWingsWebSite.Models
{
    [DataContract]
    public class LinkinToken
    {
        [DataMember]
        public string access_token { set; get; }

        [DataMember]
        public long expires_in { get; set; }

    }
}