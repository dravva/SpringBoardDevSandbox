using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class InterestCommonsExtractor
    {
        public IEnumerable<SocialRelationship> ExtractCommons(FacebookUser me, IEnumerable<FacebookUser> friends)
        {
            var friendsWithSameInterests = new List<SocialRelationship>();

            var myInterests = ExtractInterests(me.Interests);
            var myBooks = ExtractInterests(me.Books);
            var myMusics = ExtractInterests(me.Music);
            var myMovies = ExtractInterests(me.Movies);

            foreach (var friend in friends)
            {
                var friendInterests = ExtractInterests(friend.Interests);
                var friendBooks = ExtractInterests(friend.Books);
                var friendMusics = ExtractInterests(friend.Music);
                var friendMovies = ExtractInterests(friend.Movies);

                var commonInterests = FindCommons(myInterests, friendInterests);
                var commonBooks = FindCommons(myBooks, friendBooks);
                var commonMusics = FindCommons(myMusics, friendMusics);
                var commonMovies = FindCommons(myMovies, friendMovies);

                var score = 2 * commonInterests.Length + 2 * commonBooks.Length + 1 * commonMusics.Length + 1 * commonMovies.Length;

                var tags = new Dictionary<string, int>();
                tags.Add("Books", commonBooks.Length);
                tags.Add("Musics", commonMusics.Length);
                tags.Add("Movies", commonMovies.Length);

                var commonTags = tags.Keys.Where(s => tags[s] > 0).OrderByDescending(s => tags[s]);
                var caption = string.Join(", ", commonTags);
                

                if (score > 0)
                {
                    var commonBasedRelation = new SocialRelationship()
                    {
                        With = friend,
                        Type = "Interest",
                        Weight = score,
                        Title = string.Join(", ", commonInterests.Union(commonBooks).Union(commonMovies).Union(commonMusics)),
                        Caption = caption
                    };

                    friendsWithSameInterests.Add(commonBasedRelation);
                }
            }

            return friendsWithSameInterests.OrderByDescending(s => s.Weight);
        }

        private static string[] ExtractInterests(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string[0];
            }

            return text.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string[] FindCommons(string[] mine, string[] ofFriend)
        {
            if (mine == null || ofFriend == null)
            {
                return new string[0];
            }

            return mine.Intersect(ofFriend, StringComparer.InvariantCultureIgnoreCase).ToArray();
        }
    }
}
