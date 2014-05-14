using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    public class FacebookDataCrawler
    {
        public FacebookDataCrawler(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentException("accessToken");
            }

            this.accessToken = accessToken;
        }

        public IEnumerable<FacebookUser> GetMyFriends(int count)
        {
            var client = this.CreateClientForGet();
            var friends = this.CallApi<ArrayData<FacebookUser>>(client, @"/me/friends");
            return friends.Data;
        }

        public IDictionary<string, string> GetObjectsLikedByMe()
        {
            var result = new Dictionary<string, string>();
            
            var likedStatuses = this.ExecuteFql<ArrayData<StatusQueryResult>>(string.Format(CultureInfo.InvariantCulture, QueryStatusOwners, QueryLikedByMe)).Data;

            foreach (var likedStatus in likedStatuses)
            {
                result[likedStatus.StatusId] = likedStatus.Owner;
            }

            var likedPhoto = this.ExecuteFql<ArrayData<PhotoQueryResult>>(string.Format(CultureInfo.InvariantCulture, QueryPhotoOwners, QueryLikedByMe)).Data;
            foreach (var photo in likedPhoto)
            {
                result[photo.ObjectId] = photo.Owner;
            }

            return result;
        }

        public async Task<ArrayData<T>> GetFacebookObjectsUsingGraphApiAsync<T>(string api, int count=1000)
        {
            var client =  this.CreateClientForGet();
            client.QueryString.Add("limit", count.ToString(CultureInfo.InvariantCulture));
            return await this.CallApiAsync<ArrayData<T>>(client, api);
        }

        public async Task<ArrayData<T>> GetFacebookObjectsUsingGraphApiAsync<T>(string api,IEnumerable<KeyValuePair<string,string>> parameters)
        {
            var client = this.CreateClientForGet();

            foreach( var parameter in parameters )
            {
                client.QueryString.Add(parameter.Key, parameter.Value);    
            }
            
            return await this.CallApiAsync<ArrayData<T>>(client, api);
        }  


        public FacebookUser GetUserProfile(string id="me")
        {
            var client = this.CreateClientForGet();
            return this.CallApi<FacebookUser>(client,
                string.Format(CultureInfo.InvariantCulture, "/{0}", id));
        }

        public IDictionary<string, FacebookUser> GetUserProfiles(IEnumerable<string> ids)
        {
            var profiles = new Dictionary<string, FacebookUser>();

            var idsPerBatch = new List<string>();
            foreach (var id in ids)
            {
                idsPerBatch.Add("\"" + id + "\"");

                if (idsPerBatch.Count >= 50)
                {
                    var query = string.Format(CultureInfo.InvariantCulture, QueryUserProfiles, string.Join(",", idsPerBatch));
                    var users = this.ExecuteFql<ArrayData<FacebookUser>>(query).Data;
                    foreach (var facebookUser in users)
                    {
                        profiles[facebookUser.Id] = facebookUser;
                    }

                    idsPerBatch.Clear();
                }
            }

            if (idsPerBatch.Count > 0)
            {
                var query = string.Format(CultureInfo.InvariantCulture, QueryUserProfiles, string.Join(",", idsPerBatch));
                var users = this.ExecuteFql<ArrayData<FacebookUser>>(query).Data;
                foreach (var facebookUser in users)
                {
                    profiles[facebookUser.Id] = facebookUser;
                }

                idsPerBatch.Clear();
            }

            return profiles;
        }

        public IDictionary<string, FacebookUser> GetUserInterests(IEnumerable<string> ids)
        {
            var profiles = new Dictionary<string, FacebookUser>();

            var idsPerBatch = new List<string>();
            foreach (var id in ids)
            {
                idsPerBatch.Add("\"" + id + "\"");

                if (idsPerBatch.Count >= 50)
                {
                    var query = string.Format(CultureInfo.InvariantCulture, QueryUserInterests, string.Join(",", idsPerBatch));
                    var users = this.ExecuteFql<ArrayData<FacebookUser>>(query).Data;
                    foreach (var facebookUser in users)
                    {
                        facebookUser.Id = facebookUser.Uid;
                        profiles[facebookUser.Uid] = facebookUser;
                    }

                    idsPerBatch.Clear();
                }
            }

            if (idsPerBatch.Count > 0)
            {
                var query = string.Format(CultureInfo.InvariantCulture, QueryUserInterests, string.Join(",", idsPerBatch));
                var users = this.ExecuteFql<ArrayData<FacebookUser>>(query).Data;
                foreach (var facebookUser in users)
                {
                    facebookUser.Id = facebookUser.Uid;
                    profiles[facebookUser.Uid] = facebookUser;
                }

                idsPerBatch.Clear();
            }

            return profiles;
        }

        public IEnumerable<FacebookPost> GetUserFeeds(string id, int count = 50)
        {
            var client = this.CreateClientForGet();
            
            client.QueryString.Add("limit", count.ToString(CultureInfo.InvariantCulture));

            return this.CallApi<ArrayData<FacebookPost>>(client,
                    string.Format(CultureInfo.InvariantCulture, "/{0}/feed", id)).Data;
        }

        public IDictionary<string, DateTime> GetBirthdayOfMyFriends()
        {
            var users = this.ExecuteFql<ArrayData<FacebookUser>>(QueryFriendBirthday).Data;
            var birthdays = new Dictionary<string, DateTime>();

            foreach (var facebookUser in users)
            {
                if (facebookUser.Birthday != null)
                {
                    birthdays[facebookUser.Uid] = facebookUser.Birthday.Value;
                }
            }

            return birthdays;
        }

        public IEnumerable<FacebookPost> GetPhotosOfMe()
        {
            var client = this.CreateClientForGet();
            return this.CallApi<ArrayData<FacebookPost>>(client, "/me/photos").Data;
        }

        private WebClient CreateClientForGet()
        {
            return this.CreateClient("GET");
        }

        private WebClient CreateClient(string method)
        {
            var client = new WebClient();
            client.BaseAddress = "https://graph.facebook.com/v1.0";

            client.QueryString.Add( "access_token", this.accessToken );

            client.QueryString.Add( "format", @"json" );
            client.QueryString.Add( "method", method );

            return client;
        }

        public T ExecuteFql<T>(string query)
        {
            var client = this.CreateClientForGet();

            client.QueryString.Add("q", HttpUtility.UrlEncode(query));

            return this.CallApi<T>(client, "/fql");
        }

        private T CallApi<T>(WebClient client, string address)
        {
            var data = client.DownloadData(address);
            var response = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(data));
            return response;
        }

        private async Task<T> CallApiAsync<T>(WebClient client, string address)
        {
          
                var data = client.DownloadDataTaskAsync(address);
                var response = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(await data));
                return response;    
          
            
        }


        private readonly string accessToken;

        private const string QueryLikedByMe = "select object_id from like where user_id=me()";

        private const string QueryStatusOwners = "select uid, status_id from status where status_id IN({0})";

        private const string QueryPhotoOwners = "select owner, object_id from photo where object_id IN({0})";

        private const string QueryUserProfiles = "select id, name, pic from profile where id in ({0})";

        private const string QueryUserInterests = "select uid, name, education, books, interests, movies, music, sports, tv, work from user where uid in ({0})";

        private const string QueryFriendBirthday = "select uid, birthday from user where uid in (select uid1 from friend where uid2 = me()) or uid in (select uid2 from friend where uid1 = me())";

    }

    [DataContract]
    class StatusQueryResult
    {
        [DataMember(Name = "uid")]
        public string Owner { get; set; }

        [DataMember(Name = "status_id")]
        public string StatusId { get; set; }
    }

    [DataContract]
    class PhotoQueryResult
    {
        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        [DataMember(Name = "object_id")]
        public string ObjectId { get; set; }
    }
}
