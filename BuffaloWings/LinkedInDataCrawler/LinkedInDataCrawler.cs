using System.Net.Http;
using System.Threading.Tasks;
namespace Microsoft.Dldw.BuffaloWings.LinkedIn
{
    public class LinkedInDataCrawler
    {
        private const string ProfileFields = "(first-name,last-name,headline,skills,positions)";

        private const string ProfilePage = "https://api.linkedin.com/v1/people/~:" + ProfileFields + "?oauth2_access_token=";

        private readonly string accessToken;

        public LinkedInDataCrawler(string accessToken)
        {
            this.accessToken = accessToken;
        }

        public async Task<LinkedInUser> GetProfileData()
        {
            LinkedInUser linkedInUser;
           
            using (var client = new HttpClient())
            {
                var url = ProfilePage + this.accessToken;

                var resultString = await client.GetStringAsync(url);

                linkedInUser = LinkedInProfileParser.ExtractProfile(resultString);

            }
           
            return linkedInUser;
        }
    }
}
