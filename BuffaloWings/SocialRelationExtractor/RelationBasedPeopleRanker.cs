using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class RelationBasedPeopleRanker
    {
        public void AddRelations(IEnumerable<SocialRelationship> relations)
        {
            foreach (var socialRelationship in relations)
            {
                if (socialRelationship.With == null)
                {
                    continue;
                }

                if (!this.weightedRelations.ContainsKey(socialRelationship.With.Id))
                {
                    this.weightedRelations[socialRelationship.With.Id] = new SocialRelationship(){With = socialRelationship.With, Type = "Aggregated"};
                }

                var weight = GetRelationWeight(socialRelationship);
                this.weightedRelations[socialRelationship.With.Id].Weight += weight;
            }
        }

        public IEnumerable<SocialRelationship> TopRelations()
        {
            return this.weightedRelations.Values.OrderByDescending(s => s.Weight);
        }

        private double GetRelationWeight(SocialRelationship relationship)
        {
            switch (relationship.Type)
            {
                case "Family":
                    return 1;
                case "EngagedMost":
                    return relationship.Weight;
                case "EngagingMost":
                    return relationship.Weight*1.5;
            }

            return 0;
        }

        private IDictionary<string, SocialRelationship> weightedRelations = new Dictionary<string, SocialRelationship>(); 
    }
}
