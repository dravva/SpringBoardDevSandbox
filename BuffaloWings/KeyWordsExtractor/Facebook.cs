using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using KeyWordsExtractor.contract;
using Newtonsoft.Json;

namespace KeyWordsExtractor
{
    public class Facebook
    {
        public static async Task<IList<string>> GetArticles(string token)
        {
            var articles = new List<string>();
            var url =
                "https://graph.facebook.com/me?access_token=" + token + "&fields=posts.limit(200)";

            int loop = 0;
            while (!string.IsNullOrWhiteSpace(url))
            {
                var result = await GetPost(url);
                url = "";
                if (result != null && result.data != null)
                {
                    foreach (var data in result.data)
                    {
                        articles.Add(ParseResult(data));
                    }

                    if (result.paging != null)
                    {
                        url = result.paging.next;
                    }
                }
                if (++loop > 5) break;
            }

            return articles;
        }

        private static async Task<FacebookPost> GetPost(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    if (url.Contains("me?access_token="))
                    {
                        return JsonConvert.DeserializeObject<FacebookResult>(result).posts;
                    }
                    return JsonConvert.DeserializeObject<FacebookPost>(result);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ParseResult(FacebookData data)
        {
            if (!string.IsNullOrWhiteSpace(data.description)) return data.description;
            if (!string.IsNullOrWhiteSpace(data.caption)) return data.caption;
            if (!string.IsNullOrWhiteSpace(data.message)) return data.message;
            if (!string.IsNullOrWhiteSpace(data.story))
            {
                if (data.story[0] == '"' && data.story.IndexOf('"', 1) > 1)
                {
                    //story="DC is too wet this year. Local..." on his own photo.
                    return data.story.Substring(1, data.story.IndexOf('"', 1) - 1);
                }
            }
            return "";
        }

    }
}