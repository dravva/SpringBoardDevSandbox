using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BuffaloWingsWebSite.Models;
using Newtonsoft.Json;
using SqlDataProvider;

namespace BuffaloWingsWebSite.Controllers
{
    public class HomeController : Controller
    {
        private const string WeiboClientId = "528708455";

        private const string WeiboClientSecret = "7fe03d90ec14ba2449c4d44b0ca2a963";

        private const string FacebookClientId = "619387381484849";

        private const string FacebookClientSecret = "1f22ecd5cfa27759fbf126531994531c";

        private const string LinkedinClientId = "75t7rrb3xk9g17";

        private const string LinkedinClientSecret = "jIjUyNJHBqIqT8CL";

        public ActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(TryGetCookie("weibo")))
            {
                ViewBag.WeiboToken = TryGetCookie("weibo");
                ViewBag.WeiboId = TryGetCookie("weiboid");
            }
            if (!string.IsNullOrWhiteSpace(TryGetCookie("fb")))
            {
                ViewBag.FBToken = TryGetCookie("fb");
                ViewBag.FBId = TryGetCookie("fbid");
            }
            if (!string.IsNullOrWhiteSpace(TryGetCookie("linkin")))
            {
                ViewBag.LinkinToken = TryGetCookie("linkin");
                ViewBag.LinkinId = TryGetCookie("linkinid");
            }
            return View();
        }

        public ActionResult FeedBack()
        {
            var groupedResult = FeedbackProvider.GetGroupedResult();
            return View(groupedResult);
        }

        public ActionResult FeedBackDetail(string category,string item)
        {
            var groupeDetail = FeedbackProvider.GetGroupDetail(category,item);
            return View(groupeDetail);
        }

        public async Task<ActionResult> FromWeibo([FromUri] string code)
        {
            string weiboToken = "", weiboId = "";
            if (!string.IsNullOrWhiteSpace(code))
            {
                string url =
                    "https://api.weibo.com/oauth2/access_token?client_id=" + WeiboClientId + "&client_secret=" +
                    WeiboClientSecret +
                    "&grant_type=authorization_code&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromweibo&code=" +
                    code;
                using (var client = new HttpClient())
                {
                    var result = await client.PostAsync(url, null);
                    var token = JsonConvert.DeserializeObject<WeiboToken>(await result.Content.ReadAsStringAsync());
                    weiboToken = token.access_token;
                    weiboId = token.uid;
                    LogOptin(weiboId,weiboToken,"weibo");
                    Response.SetCookie(new HttpCookie("weibo", weiboToken));
                    Response.SetCookie(new HttpCookie("weiboid", weiboId));
                }
            }

            if (string.IsNullOrWhiteSpace(weiboToken) && !string.IsNullOrWhiteSpace(TryGetCookie("weibo")))
            {
                weiboToken = TryGetCookie("weibo");
                weiboId = TryGetCookie("weiboid");
            }

            //fill the other field from cookie
            if (!string.IsNullOrWhiteSpace(TryGetCookie("fb")))
            {
                ViewBag.FBToken = TryGetCookie("fb");
                ViewBag.FBId = TryGetCookie("fbid");
            }
            if (!string.IsNullOrWhiteSpace(TryGetCookie("linkin")))
            {
                ViewBag.LinkinToken = TryGetCookie("linkin");
                ViewBag.LinkinId = TryGetCookie("linkinid");
            }

            ViewBag.WeiboToken = weiboToken;
            ViewBag.WeiboId = weiboId;
            return View("afterlogin");
        }

        private void LogOptin(string user, string token, string from)
        {
            try
            {
                OptinProvider.WriteOptin(user,token,from);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {}
        }

        public async Task<ActionResult> FromFaceBook([FromUri] string code)
        {
            string fbToken = "", fbId = "", fbAvatar = "";

            if (!string.IsNullOrWhiteSpace(code))
            {
                string url =
                    "https://graph.facebook.com/oauth/access_token?client_id=" + FacebookClientId +
                    "&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromfacebook&client_secret=" +
                    FacebookClientSecret + "&code=" +
                    code;
                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    fbToken = GetString(result, "access_token=", "&expires");
                    //exchange for long term access_token
                    url =
                        "https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" +
                        FacebookClientId + "&client_secret=" + FacebookClientSecret + "&fb_exchange_token=" +
                        fbToken;
                    result = await client.GetStringAsync(url);
                    fbToken = GetString(result, "access_token=", "&expires");

                    url = "https://graph.facebook.com/me?access_token=" + fbToken + "&fields=id,name,picture";
                    result = await client.GetStringAsync(url);
                    var basic = JsonConvert.DeserializeObject<FBBasic>(result);
                    fbId = basic.name;
                    if (basic.picture != null && basic.picture.data != null && basic.picture.data.url != null)
                    {
                        fbAvatar = basic.picture.data.url;
                    }

                    LogOptin(basic.id,fbToken,"facebook");

                    Response.SetCookie(new HttpCookie("fbid", fbId));
                    Response.SetCookie(new HttpCookie("fb", fbToken));
                    Response.SetCookie(new HttpCookie("fbavatar", fbAvatar));

                }
            }
            if (string.IsNullOrWhiteSpace(fbToken) && !string.IsNullOrWhiteSpace(TryGetCookie("fb")))
            {
                fbToken = TryGetCookie("fb");
                fbId = TryGetCookie("fbid");
            }

            //fill the other field from cookie
            if (!string.IsNullOrWhiteSpace(TryGetCookie("weibo")))
            {
                ViewBag.WeiboToken = TryGetCookie("weibo");
                ViewBag.WeiboId = TryGetCookie("weiboid");
            }
            if (!string.IsNullOrWhiteSpace(TryGetCookie("linkin")))
            {
                ViewBag.LinkinToken = TryGetCookie("linkin");
                ViewBag.LinkinId = TryGetCookie("linkinid");
            }

            ViewBag.FBToken = fbToken;
            ViewBag.FBId = fbId;
            return View("afterlogin");
        }

        public ActionResult Clear()
        {
            Response.SetCookie(new HttpCookie("weibo", ""));
            Response.SetCookie(new HttpCookie("weiboid", ""));
            Response.SetCookie(new HttpCookie("linkin", ""));
            Response.SetCookie(new HttpCookie("linkinid", ""));
            Response.SetCookie(new HttpCookie("fb", ""));
            Response.SetCookie(new HttpCookie("fbid", ""));
            return View("Index");
        }

        public async Task<ActionResult> FromLinkIn([FromUri] string code)
        {
            string linkToken = "", linkId = "";

            if (!string.IsNullOrWhiteSpace(code))
            {
                string url = "https://www.linkedin.com/uas/oauth2/accessToken?grant_type=authorization_code&code=" +
                             code +
                             "&redirect_uri=http%3A%2F%2Fdwlc.cloudapp.net%2Fhome%2Ffromlinkin&client_id=" +
                             LinkedinClientId + "&client_secret=" + LinkedinClientSecret;
                using (var client = new HttpClient())
                {
                    var result = await client.PostAsync(url, null);
                    var token = JsonConvert.DeserializeObject<LinkinToken>(await result.Content.ReadAsStringAsync());
                    linkToken = token.access_token;

                    url =
                        "https://api.linkedin.com/v1/people/~?oauth2_access_token=" + linkToken;
                    var resultString = await client.GetStringAsync(url);
                    linkId = GetString(resultString, "<first-name>", "</first-name>") + ' ' +
                             GetString(resultString, "<last-name>", "</last-name>");

                    LogOptin(GetString(resultString, "view?id=", "&amp"),linkToken,"linkedin");

                    Response.SetCookie(new HttpCookie("linkin", linkToken));
                    Response.SetCookie(new HttpCookie("linkinid", linkId));
                }
            }
            if (string.IsNullOrWhiteSpace(linkToken) && !string.IsNullOrWhiteSpace(TryGetCookie("linkin")))
            {
                linkToken = TryGetCookie("linkin");
                linkId = TryGetCookie("linkinid");
            }

            //fill the other field from cookie
            if (!string.IsNullOrWhiteSpace(TryGetCookie("weibo")))
            {
                ViewBag.WeiboToken = TryGetCookie("weibo");
                ViewBag.WeiboId = TryGetCookie("weiboid");
            }
            if (!string.IsNullOrWhiteSpace(TryGetCookie("fb")))
            {
                ViewBag.FBToken = TryGetCookie("fb");
                ViewBag.FBId = TryGetCookie("fbid");
            }

            ViewBag.LinkinToken = linkToken;
            ViewBag.LinkinId = linkId;
            return View("afterlogin");
        }

        public ActionResult Mainpage([FromUri]string id)
        {
            return View();
        }

        private static string GetString(string s, string start, string end)
        {
            int pos1 = s.IndexOf(start, StringComparison.Ordinal) + start.Length;
            int pos2 = s.IndexOf(end, pos1, StringComparison.Ordinal);
            if (pos1 < 0 || pos2 < 0 || pos2 <= pos1) return string.Empty;
            return s.Substring(pos1, pos2 - pos1);
        }

        private string TryGetCookie(string key)
        {
            if (Request != null && Request.Cookies != null && Request.Cookies[key] != null)
            {
                return Request.Cookies[key].Value;
            }
            return string.Empty;
        }

    }
}
