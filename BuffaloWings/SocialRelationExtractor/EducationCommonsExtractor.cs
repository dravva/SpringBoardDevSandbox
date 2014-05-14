using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class EducationCommonsExtractor
    {

        public IEnumerable<SocialRelationship> ExtractCommons(FacebookUser me, IEnumerable<FacebookUser> friends)
        {
            if (me.EducationHistory == null)
            {
                return Enumerable.Empty<SocialRelationship>();
            }

            var educationHistory = me.EducationHistory;
            var schools = new HashSet<string>(educationHistory.Select(e => e.School.Name).Distinct(StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase);

            var commons = new List<SocialRelationship>();

            foreach (var facebookUser in friends)
            {
                if (facebookUser.EducationHistory == null)
                {
                    continue;
                }

                var weight = 0.0;
                var sameSchools = new List<Profile>();
                foreach (var school in facebookUser.EducationHistory.Select(e => e.School))
                {
                    if (schools.Contains(school.Name) && !sameSchools.Any(s => s.Name.Equals(school.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        weight += 1;
                        sameSchools.Add(school);
                    }
                }

                if (sameSchools.Count > 0)
                {
                    var commonOnEducation = new SocialRelationship()
                    {
                        With = facebookUser,
                        Title = string.Join(", ", sameSchools.Select(s => s.Name)),
                        Caption = string.Join(", ", sameSchools.Select(s => s.Name)),
                        Type = "Education",
                        Weight = weight
                    };

                    commons.Add(commonOnEducation);
                }
            }

            return commons.OrderByDescending(c => c.Weight);
        } 
    }
}
