using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class BirthdayCard
    {
        public FacebookUser Person { get; set; }

        public DateTime Birthday { get; set; }

        public SocialRelationship Relationship { get; set; }

        public IList<SweetMemory> Memories { get; set; } 
    }

    public class SweetMemory
    {
        
        public string Id { get; set; }

        public DateTime Time { get; set; }

        public FacebookUser Author { get; set; }

        public string Caption { get; set; }

        public string Picture { get; set; }

        public string OriginalPage { get; set; }
    }
}
