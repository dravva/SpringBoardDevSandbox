using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft.Json;

namespace Microsoft.Dldw.BuffaloWings.MT.UserInterestAggregator
{
    [DataContract]
    public class UserInterestRequest
    {
        [DataMember]
        public IEnumerable<UserInterestRequestItem> UserInterestRequestItems { get; set; }
    }

    [DataContract]
    public class UserInterestRequestItem
    {
        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string AuthToken { get; set; }

    }

}