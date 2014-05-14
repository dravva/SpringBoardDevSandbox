using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;
using Microsoft.Dldw.BuffaloWings.SocialRelation;

namespace UserStatisticsGenerator
{
    public class EngagementCalculator
    {
        public int GetEngagementFromOthers(IEnumerable<FacebookPost> timeline)
        {
            const int engagementFromOthers = 0;

            if (timeline == null)
            {
                return engagementFromOthers;
            }

            var profiles = new Dictionary<string, FacebookUser>();
            var scores = new Dictionary<string, int>();

            foreach (var post in timeline)
            {
                if (post.Likes != null)
                {
                    foreach (var like in post.Likes.Data)
                    {
                        Update(like, 1, profiles, scores);
                    }
                }

                if (post.Comments != null)
                {
                    foreach (var comment in post.Comments.Data)
                    {
                        Update(comment.From, 2, profiles, scores);
                    }
                }

                if (post.WithTags != null)
                {
                    foreach (var user in post.WithTags.Data)
                    {
                        Update(user, 2, profiles, scores);
                    }
                }
            }

            return scores.Sum(score => score.Value);
        }

        public int GetEngageMadeToOthers(IDictionary<string, string> likedObjects, IEnumerable<FacebookUser> friends  )
        {
            const int engagementMadeToOtherOthers = 0;

            if (likedObjects == null)
            {
                return engagementMadeToOtherOthers;
            }

            var friendIndex = friends.ToDictionary(f => f.Id);

            var profiles = new Dictionary<string, FacebookUser>();
            var scores = new Dictionary<string, int>();

            foreach (var likedObject in likedObjects)
            {
                var user = friendIndex.ContainsKey(likedObject.Value)
                    ? friendIndex[likedObject.Value]
                    : new FacebookUser() {Id = likedObject.Value};
                Update(user, 1, profiles, scores);
            }

            return scores.Sum(score => score.Value);
            ;
        }

        private void Update(FacebookUser user, int score, Dictionary<string, FacebookUser> profiles, Dictionary<string, int> scores)
        {
            if (!profiles.ContainsKey(user.Id))
            {
                profiles[user.Id] = user;
            }

            if(!scores.ContainsKey(user.Id))
            {
                scores[user.Id] = 0;
            }

            scores[user.Id] += score;
        }
    }
    
}
