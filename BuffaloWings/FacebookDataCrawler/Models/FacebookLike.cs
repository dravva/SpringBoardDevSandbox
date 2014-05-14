using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class FacebookLike
    {  
        [DataMember(Name = "category")]
        public String Category { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "created_time")]
        public DateTime CreatedTime { get; set; }

        [DataMember(Name = "id")]
        public String Id { get; set; }

    }
}
