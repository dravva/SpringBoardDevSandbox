using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace KeyWordsExtractor
{
    public static class Linkedin
    {
        private const int Batch = 20;

        public static async Task<IList<string>> GetArticles(string token)
        {
            var articles = new List<string>();
            var baseurl =
                "https://api.linkedin.com/v1/people/~/network/updates?oauth2_access_token=" + token + "&count=" + Batch +
                "&type=SHAR&start=";

            int start = 0;
            bool loop = true;
            var loopCount = 0;
            while (loop)
            {
                var url = baseurl + start;
                loop = false;
                var result = await GetPost(url);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    var tp = ParseResult(result);
                    foreach (var s in tp)
                    {
                        articles.Add(s);
                    }

                    if (result.Length < 150) break;
                    else
                    {
                        loop = true;
                        start += Batch;
                    }
                }
                if (++loopCount > 5) break;
            }

            return articles;
        }

        private static async Task<string> GetPost(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static IEnumerable<string> ParseResult(string s)
        {
            var ret = new List<string>();
            const string start = "<comment>";
            const string end = "</comment>";

            var pos1 = s.IndexOf(start, StringComparison.Ordinal);
            while (pos1 > 0)
            {
                pos1 += start.Length;
                var pos2 = s.IndexOf(end, pos1, StringComparison.Ordinal);
                if (pos2 < 0) break;
                ret.Add(s.Substring(pos1, pos2 - pos1));
                pos1 = s.IndexOf(start, pos2, StringComparison.Ordinal);
            }

            return ret;
        }
    }
}