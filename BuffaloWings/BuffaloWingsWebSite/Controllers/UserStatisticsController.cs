using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuffaloWingsWebSite.Controllers
{
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Dldw.BuffaloWings.Facebook;
    using Microsoft.Dldw.BuffaloWings.SocialRelation;

    using Newtonsoft.Json;

    using UserStatisticsGenerator;

    public class UserStatisticsController : Controller
    {
        //
        // GET: /UserStatistics/



        public ActionResult Index()
        {
            return View("Index");
        }
        public async Task<ActionResult> Flashback(string accessToken)
        {
            var userStatisticsGenerator = new UserStatisticsGenerator(accessToken);

            var post = await userStatisticsGenerator.GetFlashback();

            ViewData["flashback"] = post;

            ViewData["section"] = "Sweet Memories a Year Ago";

            return View("Flashback");
        }

        public async Task<ActionResult> FBStats(string accessToken)
        {
            var userStatisticsGenerator = new UserStatisticsGenerator(accessToken);

            var stats = await userStatisticsGenerator.GetMyStats();

            ViewData["stats"] = stats;

            ViewData["section"] = "Facebook Stats";

            return View("Flashback");
        }

        public async Task<ActionResult> JsonP(string ask, string accessToken, string callback)
        {
            var content = "Test";

            switch (ask)
            {
                case "Flashback":
                    {
                        await this.Flashback(accessToken);
                        content = RenderViewToString(this.ControllerContext, "Flashback", this.ViewData, false);
                        break;
                    }
                case "FBStats":
                    {
                        await this.FBStats(accessToken);
                        content = RenderViewToString(this.ControllerContext, "UserStatistics", this.ViewData, false);
                        break;
                    }
                
            }

            //content = HttpUtility.HtmlEncode(content);
            var result = new JavaScriptResult();
            result.Script = string.Format(CultureInfo.InvariantCulture, "{0}({{content:{1}}});", callback, JsonConvert.SerializeObject(content));
            return result;
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

    }
}
