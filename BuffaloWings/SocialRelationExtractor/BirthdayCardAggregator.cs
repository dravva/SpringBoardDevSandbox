using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace Microsoft.Dldw.BuffaloWings.SocialRelation
{
    public class BirthdayCardAggregator
    {
        public IEnumerable<BirthdayCard> GenerateBirthdayCards(FacebookDataCrawler crawler)
        {
            var me = crawler.GetUserProfile("me");

            var birthdayOfMyFriends = crawler.GetBirthdayOfMyFriends();

            var recentBirthdays = FindRecentBirthdays(birthdayOfMyFriends);

            var myPhotos = crawler.GetPhotosOfMe().ToList();

            var myStatueses = crawler.GetUserFeeds(me.Id, 200).ToList();

            var birthdayCards = new List<BirthdayCard>();

            foreach (var birthday in recentBirthdays)
            {

                var hisStatuses = crawler.GetUserFeeds(birthday.Key, 200);

                var topMoments = FindTopMomentBetweenMeAnd(crawler, me.Id, birthday.Key, myPhotos, myStatueses,
                    hisStatuses);

                var birthdayCard = new BirthdayCard()
                {
                    Birthday = birthday.Value,
                    Person = new FacebookUser(){Id = birthday.Key},
                    Memories = topMoments.ToList()
                };

                birthdayCards.Add(birthdayCard);
            }

            birthdayCards = birthdayCards.Where(c => c.Memories.Count > 0).OrderBy(b => b.Birthday.Ticks).ToList();

            var ids = birthdayCards.Select(b => b.Person.Id);

            var profiles = crawler.GetUserProfiles(ids);
            foreach (var birthdayCard in birthdayCards)
            {
                if (profiles.ContainsKey(birthdayCard.Person.Id))
                {
                    birthdayCard.Person = profiles[birthdayCard.Person.Id];
                }
            }

            return birthdayCards;
        }

        private IDictionary<string, DateTime> FindRecentBirthdays(IDictionary<string, DateTime> birthdayOfMyFriends)
        {
            var recents = new Dictionary<string, DateTime>();

            foreach (var birthdayOfMyFriend in birthdayOfMyFriends)
            {
                if (birthdayOfMyFriend.Value.Month == 2 && birthdayOfMyFriend.Value.Day == 29 &&
                    !DateTime.IsLeapYear(DateTime.Now.Year))
                {
                    continue;
                }

                var birthdayOfThisYear = new DateTime(DateTime.Now.Year, birthdayOfMyFriend.Value.Month,
                    birthdayOfMyFriend.Value.Day, 23, 59, 59);

                if (birthdayOfThisYear.Ticks <= DateTime.Now.Ticks)
                {
                    continue;
                }

                var timeToIt = birthdayOfThisYear.Subtract(DateTime.Now);

                if (timeToIt.TotalDays > 15)
                {
                    continue;
                }

                recents[birthdayOfMyFriend.Key] = birthdayOfThisYear;
            }

            return recents;
        }

        private IEnumerable<SweetMemory> FindTopMomentBetweenMeAnd(FacebookDataCrawler crawler, string myId,
            string friendId, IEnumerable<FacebookPost> myPhotos, IEnumerable<FacebookPost> myStatuses,
            IEnumerable<FacebookPost> friendStatuses)
        {
            var topMoments = new List<SweetMemory>();

            var statusSet = new HashSet<string>();

            topMoments.AddRange(this.FindTopPhotosBetweenMeAnd(myId, friendId, myPhotos, statusSet));

            topMoments.AddRange(this.FindTopWithPosts(myId, friendId, myStatuses, friendStatuses, statusSet));

            topMoments.AddRange(this.FindTos(myId, friendId, myStatuses, friendStatuses, statusSet));

            return topMoments;
        }

        private IEnumerable<SweetMemory> FindTopPhotosBetweenMeAnd(string myId, string friendId,
            IEnumerable<FacebookPost> myPhotos, HashSet<string> statusSet)
        {

            var photosHavingBoth = new List<SweetMemory>();

            foreach (var photo in myPhotos)
            {
                if (photo.Tags == null || photo.Tags.Data == null)
                {
                    continue;
                }

                if (photo.From != null && photo.From.Id.Equals(myId, StringComparison.OrdinalIgnoreCase))
                {
                    if (
                        photo.Tags.Data.Any(
                            tagged =>
                                tagged.Id != null && tagged.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "You are both in this photo by {0}: {1}",
                            photo.From.FullName, photo.Name);
                        var memory = new SweetMemory() { Author = photo.From, Id = photo.Id, Picture = photo.Picture, Time = photo.CreateTime, Caption = caption, OriginalPage = photo.Link };
                        photosHavingBoth.Add(memory);
                        statusSet.Add(photo.Id);
                    }
                }
                else if (photo.From != null && photo.From.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase))
                {
                    if (
                        photo.Tags.Data.Any(
                            tagged => tagged.Id != null && tagged.Id.Equals(myId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "You are both in this photo by {0}: {1}",
                            photo.From.FullName, photo.Name);
                        var memory = new SweetMemory() { Author = photo.From, Id = photo.Id, Picture = photo.Picture, Time = photo.CreateTime, Caption = caption, OriginalPage = photo.Link };
                        photosHavingBoth.Add(memory);
                        statusSet.Add(photo.Id);
                    }
                }
                else
                {
                    if (
                        photo.Tags.Data.Any(
                            tagged =>
                                tagged.Id != null && tagged.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)) &&
                        photo.Tags.Data.Any(
                            tagged => tagged.Id != null && tagged.Id.Equals(myId, StringComparison.OrdinalIgnoreCase)))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "You are both in this photo by {0}: {1}",
                            photo.From.FullName, photo.Name);
                        var memory = new SweetMemory() { Author = photo.From, Id = photo.Id, Picture = photo.Picture, Time = photo.CreateTime, Caption = caption, OriginalPage = photo.Link };
                        photosHavingBoth.Add(memory);
                        statusSet.Add(photo.Id);
                    }
                }
            }

            return photosHavingBoth;
        }

        private IEnumerable<SweetMemory> FindTopWithPosts(string myId, string friendId,
            IEnumerable<FacebookPost> myStatus, IEnumerable<FacebookPost> friendStatus, HashSet<string> statusSet)
        {
            var statusWithTheFriends = new List<SweetMemory>();

            foreach (var facebookPost in myStatus)
            {
                if (facebookPost.WithTags != null &&
                    facebookPost.WithTags.Data.Any(
                        t => t.Id != null && t.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!statusSet.Contains(facebookPost.Id))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "You are both in {0}'s message: {1}",
                            facebookPost.From.FullName, facebookPost.Caption);

                        var memory = new SweetMemory() {Id = facebookPost.Id, Author = facebookPost.From, Caption = caption, Picture = facebookPost.Picture, OriginalPage = facebookPost.Link, Time = facebookPost.CreateTime};

                        statusWithTheFriends.Add(memory);
                        statusSet.Add(facebookPost.Id);
                    }
                }
            }

            foreach (var facebookPost in friendStatus)
            {
                if (facebookPost.WithTags != null &&
                    facebookPost.WithTags.Data.Any(
                        t => t.Id != null && t.Id.Equals(myId, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!statusSet.Contains(facebookPost.Id))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "You are both in {0}'s message: {1}",
                            facebookPost.From.FullName, facebookPost.Message);

                        var memory = new SweetMemory() { Id = facebookPost.Id, Author = facebookPost.From, Caption = caption, Picture = facebookPost.Picture, OriginalPage = facebookPost.Link, Time = facebookPost.CreateTime};

                        statusWithTheFriends.Add(memory); 
                        statusSet.Add(facebookPost.Id);
                    }
                }
            }

            return statusWithTheFriends;
        }

        private IEnumerable<SweetMemory> FindTos(string myId, string friendId,
            IEnumerable<FacebookPost> myStatus, IEnumerable<FacebookPost> friendStatus, HashSet<string> statusSet)
        {
            var statuses = new List<SweetMemory>();

            foreach (var post in friendStatus.Where( p => p.From != null && p.From.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)))
            {
                if (post.To != null &&
                    post.To.Data.Any(t => t.Id != null && t.Id.Equals(myId, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!statusSet.Contains(post.Id))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "{0} sent you an update: {1}",
                            post.From.FullName, ExtractCaption(post));

                        var memory = new SweetMemory() { Id = post.Id, Author = post.From, Caption = caption, Picture = post.Picture, OriginalPage = post.Link, Time = post.CreateTime};

                        statuses.Add(memory);
                        statusSet.Add(post.Id);
                    }
                }
            }

            foreach (var post in myStatus.Where(p => p.From != null && p.From.Id.Equals(myId, StringComparison.OrdinalIgnoreCase)))
            {
                if (post.To != null &&
                    post.To.Data.Any(t => t.Id != null && t.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!statusSet.Contains(post.Id))
                    {
                        var caption = string.Format(CultureInfo.InvariantCulture, "you sent {0} an update: {1}",
                            post.To.Data.Where(t => t.Id != null && t.Id.Equals(friendId, StringComparison.OrdinalIgnoreCase)).First().Name, ExtractCaption(post));

                        var memory = new SweetMemory() { Id = post.Id, Author = post.From, Caption = caption, Picture = post.Picture, OriginalPage = post.Link, Time = post.CreateTime};

                        statuses.Add(memory);
                        statusSet.Add(post.Id);
                    }
                }
            }

            return statuses;
        }

        private string ExtractCaption(FacebookPost post)
        {
            if (!string.IsNullOrEmpty(post.Message))
            {
                return post.Message;
            }

            if (!string.IsNullOrEmpty(post.Name))
            {
                return post.Name;
            }

            return post.Story;
        }
    }
}
