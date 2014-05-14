using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.SocialRelation;
using UserStatisticsGenerator.Models;

namespace UserStatisticsGenerator
{
    using System.Globalization;

    using Microsoft.Dldw.BuffaloWings.Facebook;

    public class UserStatisticsGenerator
    {

        public UserStatisticsGenerator(String accessToken)
        {
            this.accessToken = accessToken;
        }

        public async Task<ActivityResult> GetMyStats()
        {
            var activityResult = new ActivityResult();


            var twoWeeksAgo = DateTime.UtcNow.AddDays(-14);


            var facebookClient = new FacebookDataCrawler(this.accessToken);

            int limit = 50;

            var parametersList = new List<KeyValuePair<string, string>>();

            parametersList.Add(new KeyValuePair<string, string>("limit", limit.ToString(CultureInfo.InvariantCulture)));


            var feeds = await facebookClient.GetFacebookObjectsUsingGraphApiAsync<FacebookPost>("/me/feed", parametersList);

            foreach (var post in feeds.Data)
            {
                if (post.CreateTime.Date >= twoWeeksAgo.Date)
                {
                    if (post.Type == "status")
                    {
                        activityResult.TotalStatusUpdates++;
                    }
                    else if (post.Type == "photo")
                    {
                        activityResult.TotalShares++;
                    }
                    else if (post.Type == "link")
                    {
                        activityResult.TotalShares++;
                    }
                    else if (post.Type == "video")
                    {
                        activityResult.TotalShares++;
                    }
                    activityResult.Total++;
                }

            }

            var facebookLikes = await facebookClient.GetFacebookObjectsUsingGraphApiAsync<FacebookLike>("me/likes");

            foreach (var item in facebookLikes.Data)
            {
                if (item.CreatedTime.Date >= twoWeeksAgo.Date)
                {
                    activityResult.TotalLike++;
                    activityResult.Total++;
                }
               
            }


            var me = facebookClient.GetUserProfile("me");
            var timeline = facebookClient.GetUserFeeds("me");
            var engagementCalculator = new EngagementCalculator();
            int engagementFromOthers = engagementCalculator.GetEngagementFromOthers(timeline);


            var friends = facebookClient.GetMyFriends(1500);
            var likedObjects = facebookClient.GetObjectsLikedByMe();

            int engagementMadeToOthers = engagementCalculator.GetEngageMadeToOthers(likedObjects,friends);

            //parametersList.Add(new KeyValuePair<string, string>("limit", limit.ToString(CultureInfo.InvariantCulture)));

            //var feeds = await facebookClient.GetFacebookObjectsUsingGraphApiAsync<FacebookPost>("/me/feed", parametersList);


            activityResult.EngagementFromOthers = engagementFromOthers;

            activityResult.EngagementMadeToOthers = engagementMadeToOthers;

            return activityResult;

        }


        public async Task<UserStatisticsFlashback> GetFlashback()
        {
            var flashback = new UserStatisticsFlashback();

            var oneYearAgo = DateTime.UtcNow.AddYears( -1 );
            
            var oneYearStartOfWeek = oneYearAgo.AddDays( -3 );

            var oneYearEndOfWeek = oneYearAgo.AddDays(3);

            var facebookPhoto = GetFlashbackPhotos();

            const int limit = 100;

            var facebookClient = new FacebookDataCrawler(this.accessToken);

            var parametersList = new List<KeyValuePair<string, string>>();

            parametersList.Add( new KeyValuePair<string, string>("limit",limit.ToString( CultureInfo.InvariantCulture )) );

            var feeds = await facebookClient.GetFacebookObjectsUsingGraphApiAsync<FacebookPost>( "/me/feed", parametersList );

            while (feeds.Data.Last().CreateTime >= oneYearEndOfWeek)
            {
                
                parametersList.Clear();
                var nextPageId = GetNextPageId( feeds.PagingInfo.Next );
                if( nextPageId == null ) break;
                else
                {
                 parametersList.Add( new KeyValuePair<string, string>("until",nextPageId) );
                 parametersList.Add(new KeyValuePair<string, string>("limit", limit.ToString(CultureInfo.InvariantCulture)));
                }
                feeds = await facebookClient.GetFacebookObjectsUsingGraphApiAsync<FacebookPost>("/me/feed", parametersList);
            }

            FacebookPost flashBackPost = null;
            
            var flashbackWeekPosts = new Dictionary<string,FacebookPost>();
            var flashbackDayPosts = new Dictionary<string, FacebookPost>();
            foreach (var post in feeds.Data)
            {
               if( post.CreateTime.Date >= oneYearStartOfWeek.Date && post.CreateTime.Date <= oneYearEndOfWeek.Date )
               {
                   if( post.CreateTime.Date == oneYearAgo.Date )
                   {
                       flashbackDayPosts.Add( post.Id,post );
                   }
                   flashbackWeekPosts.Add(post.Id, post);
                  
               }
            }
            
            if( flashbackDayPosts.Any() )
            {
                if (flashbackDayPosts.Count==1)
                {
                    flashBackPost = flashbackDayPosts.First().Value;
                }
                else
                {
                    flashBackPost = GetImportantPost(flashbackDayPosts);
                }
            }
            else if (flashbackWeekPosts.Any())
            {
                if (flashbackWeekPosts.Count == 1)
                {
                    flashBackPost = flashbackWeekPosts.First().Value;
                }
                else
                {
                    flashBackPost = GetImportantPost(flashbackWeekPosts);
                }
            }

            flashback.FacebookPhoto = facebookPhoto;
            flashback.FacebookPost = flashBackPost;
            return flashback;

        }

        public FacebookPhoto GetFlashbackPhotos()
        {
            FacebookPhoto facebookPhoto = null;

            var oneYearAgo = DateTime.UtcNow.AddYears( -1 );
            
            var oneYearStartOfWeek = oneYearAgo.AddDays( -3 );

            var oneYearEndOfWeek = oneYearAgo.AddDays(3);

            try
            {
                var facebookClient = new FacebookDataCrawler(this.accessToken);
                var photosFbData = facebookClient.ExecuteFql<ArrayData<FacebookPhoto>>(GetPhotosFromAlbum);
                
                var flashbackWeekPhotos = new Dictionary<string, FacebookPhoto>();
                var flashbackDayPhotos = new Dictionary<string, FacebookPhoto>();

                foreach (var photo in photosFbData.Data)
                {
                    var photoCreatedTime = UserStatisticsUtil.UnixTimeStampToDateTime(photo.CreatedTimestamp);

                        if (photoCreatedTime.Date >= oneYearStartOfWeek.Date && photoCreatedTime.Date <= oneYearEndOfWeek.Date)
                        {
                            if (photoCreatedTime.Date == oneYearAgo.Date)
                            {
                                flashbackDayPhotos.Add(photo.Id, photo);
                            }
                            flashbackWeekPhotos.Add(photo.Id, photo);

                        }
                }

                if (flashbackDayPhotos.Any())
                {
                    if (flashbackDayPhotos.Count == 1)
                    {
                       return flashbackDayPhotos.First().Value;
                    }
                    else
                    {
                        return GetImportantPhotos(flashbackDayPhotos);
                    }

                   
                }
                else if (flashbackWeekPhotos.Any())
                {
                    if (flashbackWeekPhotos.Count == 1)
                    {
                        return flashbackWeekPhotos.First().Value;
                    }
                    else
                    {
                        return GetImportantPhotos(flashbackWeekPhotos);
                    }
                }
              
            }
            catch (Exception exception)
            {
                
               //todo
            }
            return facebookPhoto;

        }

        //public FacebookPost GetRecentActivity(IEnumerable<FacebookPost> timeline)
        //{
        //    //FacebookPost flashBack = null;
        //    //if (timeline == null)
        //    //{
        //    //    return flashBack;
        //    //}

        //    //var twoWeeksAgo = DateTime.UtcNow.AddDays(-14);
          
        //    //List<FacebookPost> flashbackPosts = new List<FacebookPost>();
        //    //foreach (var post in timeline)
        //    //{
        //    //    if (post.CreateTime >= oneYearStartOfWeek && post.CreateTime <= oneYearEndOfWeek)
        //    //    {
        //    //        flashbackPosts.Add(post);
        //    //    }
        //    //}
        //    //return flashbackPosts.First();

        //}


        private FacebookPost GetImportantPost( IDictionary<string,FacebookPost> posts )
        {
            IDictionary<string, int> postScore = new Dictionary<string, int>();

            foreach( var facebookPost in posts )
            {
             
                if( null != facebookPost.Value.Comments )
                {
                    UpdateScore(postScore, facebookPost.Value.Comments.Data.Count(), facebookPost.Value.Id);
                }
                if (null != facebookPost.Value.Likes)
                {
                    UpdateScore(postScore, facebookPost.Value.Likes.Data.Count(), facebookPost.Value.Id);
                }
                if (null != facebookPost.Value.WithTags)
                {
                    UpdateScore(postScore, facebookPost.Value.WithTags.Data.Count(), facebookPost.Value.Id);
                }
   
            }
            IEnumerable<KeyValuePair<string, int>> result = postScore.OrderByDescending(u =>u.Value);

            return posts[result.First().Key];

        }
        private FacebookPhoto GetImportantPhotos(IDictionary<string, FacebookPhoto> posts)
        {
            IDictionary<string, int> postScore = new Dictionary<string, int>();

            foreach (var facebookPost in posts)
            {

                if (null != facebookPost.Value.PhotoCommentsInfo)
                {
                    UpdateScore(postScore, facebookPost.Value.PhotoCommentsInfo.CommentsCount, facebookPost.Value.Id);
                }
                if (null != facebookPost.Value.PhotoLikesInfo)
                {
                    UpdateScore(postScore, facebookPost.Value.PhotoLikesInfo.LikesCount, facebookPost.Value.Id);
                }
                if (null != facebookPost.Value.Tags)
                {
                    UpdateScore(postScore, facebookPost.Value.Tags.Count, facebookPost.Value.Id);
                }

            }
            IEnumerable<KeyValuePair<string, int>> result = postScore.OrderByDescending(u => u.Value);

            return posts[result.First().Key];

        }
        private void UpdateScore(IDictionary<string, int> postScore, int score, string postId)
        {
            if( !postScore.ContainsKey( postId ) )
            {
                postScore[postId] = 0;
            }

            postScore[postId] += score;
        }
        private string GetNextPageId(string url)
        {

            var parametersArray = url.Split( '&' );
            return ( from s in parametersArray
                     where s.StartsWith( "until" )
                     select s.Split( '=' )[1] ).FirstOrDefault();
        }



        private const string GetPhotosFromAlbum =
            "SELECT object_id, pid, aid, src_big, src_small, link,caption, like_info, comment_info, created, place_id FROM photo WHERE aid IN (SELECT aid FROM album WHERE owner = me() AND (created > 1362117600 OR modified > 1362117600)) AND (created > 1362117600)";

        private string accessToken; 
    }
}
