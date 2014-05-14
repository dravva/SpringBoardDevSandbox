using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.Dldw.BuffaloWings.LinkedIn
{
    public class LinkedInUser
    {
        public String Name { get; set; }

        public IList<String> Skills
        {
            get;
            set;
        }

        [DataMember]
        public IList<ProfessionJob> ProfessionalJobs { get; set; }

        [DataMember]
        public String ProfileHeadline { get; set; }  


        public LinkedInUser()
        {
            Skills= new List<string>();

            ProfessionalJobs= new List<ProfessionJob>();
        }
    }

    [DataContract]
    public class ProfessionJob
    {
        [DataMember]
        public String Designation { get; set; }

        [DataMember]
        public String Company { get; set; } 

        [DataMember]
        public bool IsCurrent { get; set; } 


    }
}
