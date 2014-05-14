using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class SocialRelationship
    {
        public FacebookUser From { get; set; }

        public FacebookUser With { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Caption { get; set; }

        public double Weight { get; set; }
    }
}
