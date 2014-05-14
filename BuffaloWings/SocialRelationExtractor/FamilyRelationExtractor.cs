using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class FamilyRelationExtractor
    {
        public IEnumerable<SocialRelationship> Extract(FacebookUser userProfile)
        {
            var relations = new List<SocialRelationship>();

            if (userProfile == null || string.IsNullOrEmpty(userProfile.RelationshipStatus))
            {
                relations.Add(new SocialRelationship(){From = userProfile, Title = "Mother and Father", Type = "Family", Weight = 1.0});
                return relations;
            }

            switch (userProfile.RelationshipStatus.ToUpperInvariant())
            {
                case "SINGLE":
                    relations.Add(new SocialRelationship() { From = userProfile, Title = "Mother and Father", Type = "Family", Weight = 1.0 });
                    break;
                case "IN A RELATIONSHIP":
                    relations.Add(new SocialRelationship() { From = userProfile, With = userProfile.SignificantOther, Title = "Girlfriend/Boyfriend", Type = "Family", Weight = 1.0 });
                    break;
                case "MARRIED":
                    relations.Add(new SocialRelationship() { From = userProfile, With = userProfile.SignificantOther, Title = "Wife/Husbund", Type = "Family", Weight = 1.0 });
                    break;
            }

            return relations;
        } 
    }
}
