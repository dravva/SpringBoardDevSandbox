using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using KeyWordsExtractor;
using Microsoft.Dldw.BuffaloWings.MT.UserInterestAggregator;

namespace BuffaloWingsWebSite.Controllers
{

    public class ValuesController : ApiController
    {
        public UserInterestAggregator UserDataGenerator = new UserInterestAggregator();

        [HttpGet]
        public async Task<IList<KeyValuePair<string, int>>> Keywords(string wid, string ftoken, string ltoken)
        {
            var result = await WordParse.GetWords(wid, ftoken, ltoken);
            var ret = new List<KeyValuePair<string, int>>();
            for (var i = 0; i < result.Count && i < 5; i++) ret.Add(result[i]);
            return ret;
        }

        [HttpPost]
        public async Task<UserProfile> Interest([FromBody]UserInterestRequest userInterestRequest)
        {
            return await UserDataGenerator.GetUserInterestData(userInterestRequest);
        }

    }
}