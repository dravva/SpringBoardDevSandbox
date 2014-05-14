using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PanGu;

namespace KeyWordsExtractor
{
    public static class WordParse
    {
        public static async Task<IList<KeyValuePair<string, int>>> GetWords(string wid, string ftoken, string ltoken)
        {
            const string wtoken = "2.00YluiFDEx2E5B70cbbe23bei2Og2C";
            var ret = new List<string>();
            Segment.Init();

            var task1 = GetWordsFromWeibo(wid, wtoken);
            //var task2 = GetWordsFromWeibo2(wid,wtoken);
            var task3 = GetWordsFromFacebook(ftoken);
            var task4 = GetWordsFromLinkedin(ltoken);

            ret.AddRange((await task1));
            //ret.AddRange((await task2));
            ret.AddRange((await task3));
            ret.AddRange((await task4));

            //get word rank
            var map = new Dictionary<string, int>();
            foreach (var word in ret)
            {
                if (!string.IsNullOrWhiteSpace(word) && word.Length > 1 && !StopWords.English.Contains(word.ToLowerInvariant()) && !StopWords.Chinese.Contains(word.ToLowerInvariant()))
                {
                    if (IsNumber(word)) continue;
                    var wordLower = word.ToLowerInvariant();
                    int times;
                    if (map.TryGetValue(wordLower, out times))
                    {
                        map[wordLower] = times + 1;
                    }
                    else map[wordLower] = 1;
                }
            }
            var myList = map.ToList();
            myList.Sort((firstPair, nextPair) => nextPair.Value.CompareTo(firstPair.Value));
            return myList;
        }

        private static bool IsNumber(string s)
        {
            long value;
            return long.TryParse(s, out value);
        }

        private static async Task<IEnumerable<string>> GetWordsFromWeibo(string wid, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return new List<string>();
            var result = await Weibo.GetArticles(wid, token);
            var seg = new Segment();
            return (from article in result where !string.IsNullOrWhiteSpace(article) from wordInfo in seg.DoSegment(article) select wordInfo.Word).ToList();
        }

        private static async Task<IEnumerable<string>> GetWordsFromWeibo2(string wid, string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return new List<string>();
            var result = await Weibo.GetComments(wid, token);
            var seg = new Segment();
            return (from article in result where !string.IsNullOrWhiteSpace(article) from wordInfo in seg.DoSegment(article) select wordInfo.Word).ToList();
        }

        private static async Task<IEnumerable<string>> GetWordsFromFacebook(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return new List<string>();
            var result = await Facebook.GetArticles(token);
            var seg = new Segment();
            return (from article in result where !string.IsNullOrWhiteSpace(article) from wordInfo in seg.DoSegment(article) select wordInfo.Word).ToList();
        }

        private static async Task<IEnumerable<string>> GetWordsFromLinkedin(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return new List<string>();
            var result = await Linkedin.GetArticles(token);
            var seg = new Segment();
            return (from article in result where !string.IsNullOrWhiteSpace(article) from wordInfo in seg.DoSegment(article) select wordInfo.Word).ToList();
        }
    }
}
