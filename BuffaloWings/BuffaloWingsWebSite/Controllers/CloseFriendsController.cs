using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Dldw.BuffaloWings.Facebook;
using Microsoft.Dldw.BuffaloWings.SocialRelation;
using Newtonsoft.Json;

namespace BuffaloWingsWebSite.Controllers
{
    public class CloseFriendsController : Controller
    {
        //
        // GET: /CloseFriends/

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult MostEngaged(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);
            var me = facebookClient.GetUserProfile("me");
            var feeds = facebookClient.GetUserFeeds("me");
            var engagementExtractor = new EngagementRelationExtractor();
            var engagedRelations = engagementExtractor.Extract(feeds).Where(s => s.With.Id != me.Id).Take(15).ToList();

            FillWithProfiles(facebookClient, engagedRelations);

            ViewData["relations"] = engagedRelations;
            ViewData["category"] = "People engage me the most";

            return View("CloseFriends");
        }

        public ActionResult MostEngaging(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);
            var me = facebookClient.GetUserProfile("me");
            var engagementExtractor = new EngagementRelationExtractor();
            var friends = facebookClient.GetMyFriends(1500);
            var likedObjects = facebookClient.GetObjectsLikedByMe();
            var engagingRelations = engagementExtractor.Extract(likedObjects, friends).Where(s => s.With.Id != me.Id).Take(15).ToList();

            FillWithProfiles(facebookClient, engagingRelations);

            ViewData["relations"] = engagingRelations;
            ViewData["category"] = "People I engage the most";

            return View("CloseFriends");
        }

        public ActionResult CommonEducation(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);
            var me = facebookClient.GetUserProfile("me");
            var friends = facebookClient.GetMyFriends(1500);
            var interestsOfFriends = facebookClient.GetUserInterests(friends.Select(f => f.Id));
            var interestsOfMe = facebookClient.GetUserInterests(new String[] { me.Id }).Values.First();
            var educationCommonsExtractor = new EducationCommonsExtractor();
            var educationCommons = educationCommonsExtractor.ExtractCommons(interestsOfMe, interestsOfFriends.Values).Take(15).ToList();

            FillWithProfiles(facebookClient, educationCommons);
            ViewData["relations"] = educationCommons;
            ViewData["category"] = "People went to the same school";

            return View("CloseFriends");
        }

        public ActionResult CommonInterest(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);
            var me = facebookClient.GetUserProfile("me");
            var friends = facebookClient.GetMyFriends(1500);
            var interestsOfFriends = facebookClient.GetUserInterests(friends.Select(f => f.Id));
            var interestsOfMe = facebookClient.GetUserInterests(new String[] { me.Id }).Values.First();
            var interestCommonsExtractor = new InterestCommonsExtractor();
            var interestCommons = interestCommonsExtractor.ExtractCommons(interestsOfMe, interestsOfFriends.Values).Take(15).ToList();

            FillWithProfiles(facebookClient, interestCommons);
            ViewData["relations"] = interestCommons;
            ViewData["category"] = "People with common interests";

            return View("CloseFriends");
        }

        public ActionResult GatherTogether(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);

            var togetherRelationExtractor = new TogetherRelationExtractor();
            var me = facebookClient.GetUserProfile("me");
            var friends = facebookClient.GetMyFriends(1500);
            var likedObjects = facebookClient.GetObjectsLikedByMe();
            var engagementExtractor = new EngagementRelationExtractor();
            var engagingRelations = engagementExtractor.Extract(likedObjects, friends).Where(s => s.With.Id != me.Id).ToList();

            var feeds = facebookClient.GetUserFeeds("me");
            var engagedRelations = engagementExtractor.Extract(feeds).Where(s => s.With.Id != me.Id).ToList();

            var postsOfFriends = new Dictionary<string, IEnumerable<FacebookPost>>();

            foreach (var facebookUser in engagingRelations.Union(engagedRelations).Select(s => s.With.Id).Distinct())
            {
                postsOfFriends[facebookUser] = facebookClient.GetUserFeeds(facebookUser, 100);
            }

            var relations = togetherRelationExtractor.ExtractTogetherRelations(me, postsOfFriends).ToList();


            FillWithProfiles(facebookClient, relations);
            ViewData["relations"] = relations;
            ViewData["category"] = "People I hang out with most";

            return View("CloseFriends");
        }

        public ActionResult BirthdayCard(string accessToken)
        {
            var facebookClient = new FacebookDataCrawler(accessToken);
            var birthdayCardDataAggregator = new BirthdayCardAggregator();
            var birthdayCards = birthdayCardDataAggregator.GenerateBirthdayCards(facebookClient);

            ViewData["birthdayCards"] = birthdayCards;

            return View("BirthdayCard");
        }

        public ActionResult JsonP(string ask, string accessToken, string callback)
        {
            var content = String.Empty;

            switch (ask)
            {
                case "MostEngaged":
                {
                    this.MostEngaged(accessToken);
                    content = RenderViewToString(this.ControllerContext, "CloseFriends", this.ViewData, false);
                    break;
                }
                case "MostEngaging":
                {
                    this.MostEngaging(accessToken);
                    content = RenderViewToString(this.ControllerContext, "CloseFriends", this.ViewData, false);
                    break;
                }
                case "SameSchool":
                {
                    this.CommonEducation(accessToken);
                    content = RenderViewToString(this.ControllerContext, "CloseFriends", this.ViewData, false);
                    break;
                }
                case "CommonInterests":
                {
                    this.CommonInterest(accessToken);
                    content = RenderViewToString(this.ControllerContext, "CloseFriends", this.ViewData, false);
                    break;
                }
                case "GatherTogether":
                {
                    this.GatherTogether(accessToken);
                    content = RenderViewToString(this.ControllerContext, "CloseFriends", this.ViewData, false);
                    break;
                }
                case "BirthdayCard":
                {
                    this.BirthdayCard(accessToken);
                    content = RenderViewToString(this.ControllerContext, "BirthdayCard", this.ViewData, false);
                    break;
                }
            }

            //content = HttpUtility.HtmlEncode(content);
            var result = new JavaScriptResult();
            result.Script = string.Format(CultureInfo.InvariantCulture, "{0}({{content:{1}}});", callback, JsonConvert.SerializeObject(content));
            return result;
        }

        private void FillWithProfiles(FacebookDataCrawler client, List<SocialRelationship> relations)
        {
            var ids = relations.Select(s => s.With.Id).Where(id => !string.IsNullOrEmpty(id));

            var profiles = client.GetUserProfiles(ids);

            foreach (var socialRelationship in relations)
            {
                if (profiles.ContainsKey(socialRelationship.With.Id))
                {
                    socialRelationship.With = profiles[socialRelationship.With.Id];
                }
            }
        }

        private static string RenderViewToString(ControllerContext context,
                                    string viewPath,
                                    object model = null,
                                    bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}
