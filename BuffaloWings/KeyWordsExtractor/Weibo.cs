using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using KeyWordsExtractor.contract;
using Newtonsoft.Json;

namespace KeyWordsExtractor
{
    public static class Weibo
    {
        public static async Task<IList<string>> GetArticles(string id, string token)
        {
            var articles = new List<string>();
            var url = "https://api.weibo.com/2/statuses/user_timeline.json?uid=" + id + "&access_token=" + token +
                      "&page=";
            int pos = 0;
            var loop = 0;
            while (true)
            {
                var task1 = GetResult(url + (++pos));
                var task2 = GetResult(url + (++pos));
                var task3 = GetResult(url + (++pos));

                var result1 = await task1;
                if (result1 != null && result1.statuses != null)
                {
                    foreach (var status in result1.statuses)
                    {
                        articles.Add(ParseResult(status));
                    }
                }

                var result2 = await task2;
                if (result2 != null && result2.statuses != null)
                {
                    foreach (var status in result2.statuses)
                    {
                        articles.Add(ParseResult(status));
                    }
                }

                var result3 = await task3;
                if (result3 != null && result3.statuses != null)
                {
                    foreach (var status in result3.statuses)
                    {
                        articles.Add(ParseResult(status));
                    }
                }

                if (result3 == null || result3.statuses == null || result3.statuses.Count < 5 || ++loop > 5) break;
            }

            return articles;
        }

        private static async Task<WeiboResult> GetResult(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<WeiboResult>(result);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<WeiboComments> GetComment(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    return JsonConvert.DeserializeObject<WeiboComments>(result);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string ParseResult(WeiboStatuses status)
        {
            var ret = "";
            if (status != null && status.retweeted_status != null && !string.IsNullOrWhiteSpace(status.retweeted_status.text))
            {
                ret = status.retweeted_status.text;
            }
            else if (status != null && !string.IsNullOrWhiteSpace(status.text))
            {
                ret = status.text;
            }
            return ret;
        }

        public static async Task<IList<string>> GetComments(string id, string token)
        {
            var comments = new List<string>();
            var url = "https://api.weibo.com/2/comments/by_me.json?uid=" + id + "&access_token=" + token + "&page=";
            int pos = 0;
            while (true)
            {
                var task1 = GetComment(url + (++pos));
                var task2 = GetComment(url + (++pos));
                var task3 = GetComment(url + (++pos));

                var result1 = await task1;
                if (result1 != null && result1.comments != null)
                {
                    foreach (var comment in result1.comments)
                    {
                        if (comment.status != null)
                        {
                            comments.Add(ParseResult(comment.status));
                        }
                    }
                }

                var result2 = await task2;
                if (result2 != null && result2.comments != null)
                {
                    foreach (var comment in result2.comments)
                    {
                        if (comment.status != null)
                        {
                            comments.Add(ParseResult(comment.status));
                        }
                    }
                }

                var result3 = await task3;
                if (result3 != null && result3.comments != null)
                {
                    foreach (var comment in result3.comments)
                    {
                        if (comment.status != null)
                        {
                            comments.Add(ParseResult(comment.status));
                        }
                    }
                }

                if (result3 == null || result3.comments == null || result3.comments.Count < 5) break;
            }

            return comments;
        }

    }
}