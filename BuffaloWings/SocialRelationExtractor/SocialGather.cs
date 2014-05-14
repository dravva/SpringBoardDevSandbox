using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class SocialGather
    {

        public string Topic { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<FacebookUser> Attendees { get; set; } 
    }
}
