using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.MT.UserInterestAggregator
{
    using System.Globalization;

    using Microsoft.Dldw.BuffaloWings.Facebook;
    using Microsoft.Dldw.BuffaloWings.LinkedIn;

    public class UserInterestAggregator
    {

        private const string LikesApi = "/me/Likes";

        private const string BooksReadApi = "/me/book.reads";

        private const string MusicApi = "/me/music";

        private const string VideosApi = "/me/video.watches";

        public async Task<UserProfile> GetUserInterestData(UserInterestRequest userInterestRequest)
        {

            var taskList = new List<Task<Object>>();

            foreach (var item in userInterestRequest.UserInterestRequestItems)
            {
                if (item.AccountType == "Facebook")
                {
                    taskList.Add(this.GetFacebookData(item.AuthToken));
                }

                else if (item.AccountType == "LinkedIn")
                {
                   
                    taskList.Add(GetLinkedInData(item.AuthToken));
                }
                else if (item.AccountType == "SinaWeibo")
                {
                   // IDataExtractor dataExtractor = new FacabookDataExtractor();
                }
            }
            
            IList<Object> userDataList = await Task.WhenAll(taskList);

            return MergeUserData(userDataList);
        }

        private UserProfile MergeUserData(IEnumerable<Object> userDataList)
        {
            var newUser = new UserProfile();

            foreach (var user in userDataList)
            {
                if (user.GetType() == typeof(UserProfile))
                {
                    newUser.Birthday = ((UserProfile)user).Birthday;
                    newUser.FirstName = ((UserProfile)user).FirstName;
                    newUser.MiddleName = ((UserProfile)user).MiddleName;
                    newUser.LastName = ((UserProfile)user).LastName;
                    newUser.Location = ((UserProfile)user).Location;
                    newUser.UserInterests = ((UserProfile)user).UserInterests;

                }

                if (user.GetType() == typeof(LinkedInUser))
                {
                    newUser.LinkedInProfile = ((LinkedInUser)user);
                    
                }
            }
            return newUser;

        }

        public async Task<Object> GetLinkedInData(string token)
        {
            var linkedInDataCrawler = new LinkedInDataCrawler(token);

            return await linkedInDataCrawler.GetProfileData();
        }

        public async Task<Object> GetFacebookData(string token)
        {

            var user = new UserProfile();

            var facebookDataCrawler= new FacebookDataCrawler( token );

            var fbuser = facebookDataCrawler.GetUserProfile();

            if (fbuser.Birthday != null)
                user.Birthday = fbuser.Birthday.Value.ToString("MM/dd/yyyy",CultureInfo.InvariantCulture );
            user.FirstName = fbuser.FirstName;
            user.LastName = fbuser.LastName;
            user.MiddleName = fbuser.MiddleName;
            user.Location = fbuser.Location.Name;

            user.UserInterests = new Dictionary<string, UserInterest>();

            var facebookLikes = await facebookDataCrawler.GetFacebookObjectsUsingGraphApiAsync<FacebookLike>(LikesApi);

            foreach (var item in facebookLikes.Data)
            {

                FillInterest(user,item.Category,item.Name,"");
            }

            var facebookBooksDataList = await facebookDataCrawler.GetFacebookObjectsUsingGraphApiAsync<FacebookApplicationData>(BooksReadApi);

            foreach (var booksData in facebookBooksDataList.Data)
            {
                var subCategory = booksData.Application.Name;
                foreach (var item in booksData.ItemsList)
                {
                    FillInterest(user, subCategory, item.Value.Title, item.Value.Url, "Books");
                }
            }

            var musicData = await facebookDataCrawler.GetFacebookObjectsUsingGraphApiAsync<FacebookLike>(MusicApi);

            foreach (var item in musicData.Data)
            {

                FillInterest(user, item.Category, item.Name, "", "Music");
            }


            var videosData = await facebookDataCrawler.GetFacebookObjectsUsingGraphApiAsync<FacebookApplicationData>(VideosApi);

            foreach (var itemList in videosData.Data)
            {
                var subCategory = itemList.Application.Name;
                foreach (var item in itemList.ItemsList)
                {
                    FillInterest(user, subCategory, item.Value.Title, item.Value.Url, "Movies and TV Shows");
                }
            }
         
           return user;
        }

        private static void FillInterest(UserProfile user, String subCategory, String name, String url, String category = "Others")
        {

            if (InterestCategoryList.CategoryMap.ContainsKey(subCategory))
            {
                category = (string)InterestCategoryList.CategoryMap[subCategory];
            }
            UserInterest interestObject;

            user.UserInterests.TryGetValue(category, out interestObject);

            if (interestObject == null)
            {
                interestObject = new UserInterest { Category = category };

                user.UserInterests.Add(category, interestObject);
            }

            UserInterestSubCategoryItems interestItems;

            interestObject.UserSubcategoryList.TryGetValue(subCategory, out interestItems);

            if (interestItems == null)
            {
                interestItems = new UserInterestSubCategoryItems { SubCategory = subCategory };

                interestObject.UserSubcategoryList.Add(subCategory, interestItems);
            }

            var userInterestItem = new UserInterestItem { Title = name, Url = url };

            interestItems.InterestItemList.Add(userInterestItem);
        }

    }
}
