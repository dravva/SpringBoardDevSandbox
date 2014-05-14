using System.Collections.Generic;
using System.Configuration;
using BuffaloWingsWebSite.Models;
using SqlDataProvider;
using System.Web.Http;
using SqlDataProvider.contract;


namespace BuffaloWingsWebSite.Controllers
{
    public class StaticController : ApiController
    {
        
        [HttpGet]
        public IEnumerable<string> Feedback(int start,int end)
        {
            return FeedbackProvider.ReadFeedback(start,end);
        }

        [HttpPost]
        public FeedbackResult GetFeedback([FromBody] FeedBackInfo info)
        {
            return FeedbackProvider.GetFeedback(info.user, info.category, info.item);
        }

        [HttpPost]
        public FeedbackResult Feedback([FromBody] FeedBackInfo info)
        {
            return FeedbackProvider.WriteFeedback(info.user, info.category, info.item, info.value,info.feedback);
        }

        [HttpGet]
        public int Optin(string from)
        {
            return OptinProvider.ReadOptin(from);
        }

        [HttpPost]
        public string Optin([FromBody]FeedBackInfo info)
        {
            OptinProvider.WriteOptin(info.user, info.token, info.from);
            return "success";
        }

    }
}
