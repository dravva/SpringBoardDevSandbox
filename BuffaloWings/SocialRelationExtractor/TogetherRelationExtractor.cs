using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class TogetherRelationExtractor
    {

        public IEnumerable<SocialRelationship> ExtractTogetherRelations(FacebookUser subject, IDictionary<string, IEnumerable<FacebookPost>> postsOfFriends)
        {

            var myGathers = new Dictionary<string, SocialGather>();

            foreach (var postsOfFriend in postsOfFriends)
            {
                foreach (var gather in this.ExtractGathers(postsOfFriend.Value))
                {
                    if (!myGathers.ContainsKey(gather.Topic) && gather.Attendees.Any(a => a.Id == subject.Id))
                    {
                        myGathers[gather.Topic] = gather;
                    }
                }
            }

            var gatherWiths = new Dictionary<string, int>();

            foreach (var socialGather in myGathers)
            {
                var withs = socialGather.Value.Attendees.Select(a => a.Id).Where(a => a != subject.Id).Distinct();
                foreach (var with in withs)
                {
                    gatherWiths[with] = gatherWiths.ContainsKey(with) ? gatherWiths[with] + 1 : 1;
                }
            }

            var sortedGatherWiths = gatherWiths.Keys.OrderByDescending(k => gatherWiths[k]);

            var gatherTogethers = new List<SocialRelationship>();

            foreach (var sortedGatherWith in sortedGatherWiths)
            {
                var relation = new SocialRelationship()
                {
                    With = new FacebookUser(){Id = sortedGatherWith},
                    Caption = string.Format(CultureInfo.InvariantCulture, "Gathered {0} times", gatherWiths[sortedGatherWith]),
                    Title = string.Format(CultureInfo.InvariantCulture, "Gathered {0} times", gatherWiths[sortedGatherWith]),
                    Type = "GatherTogether",
                    Weight = gatherWiths[sortedGatherWith]
                };

                gatherTogethers.Add(relation);
            }
            return gatherTogethers;
        }

        private IEnumerable<SocialGather> ExtractGathers(IEnumerable<FacebookPost> posts)
        {
            var gathers = new List<SocialGather>();

            foreach (var facebookPost in posts)
            {
                var author = facebookPost.From;
                var withs = facebookPost.WithTags == null ? new List<FacebookUser>() : facebookPost.WithTags.Data.ToList();
                withs.Add(author);

                if (withs.Count > 1)
                {
                    var gather = new SocialGather();
                    gather.Topic = facebookPost.Id;
                    gather.Time = facebookPost.CreateTime;
                    gather.Attendees = withs;
                    gathers.Add(gather);
                }
            }

            return gathers;
        } 
    }
}
