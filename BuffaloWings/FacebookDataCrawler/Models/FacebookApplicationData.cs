using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    
    [DataContract]
    public class FacebookApplicationData
    {
        [DataMember(Name = "id")]
        public String Id { get; set; }

        [DataMember(Name = "application")]
        public Application Application { get; set; }
        
        [DataMember(Name = "data")]
        public IDictionary<String, ApplicationDataItem> ItemsList { get; set; }

    }

    [DataContract]
    public class Application
    {

        [DataMember(Name = "id")]
        public String Id { get; set; }
            
        [DataMember(Name = "name")]
        public String Name { get; set; }
    }
    public class ApplicationDataItem
    {

        [DataMember(Name = "id")]
        public String Id { get; set; }
            
        [DataMember(Name = "url")]
        public String Url { get; set; }
            
        [DataMember(Name = "title")]
        public String Title { get; set; }
    }
        
     
}
