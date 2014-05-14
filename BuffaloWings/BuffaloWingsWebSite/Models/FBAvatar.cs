using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BuffaloWingsWebSite.Models
{
    [DataContract]
    public class FBAvatar
    {
        [DataMember]
        public FbAvatarUrl data { get; set; }
    }
}