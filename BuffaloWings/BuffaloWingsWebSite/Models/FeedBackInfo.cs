using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuffaloWingsWebSite.Models
{
    public class FeedBackInfo
    {
        public string user { get; set; }
        public string category { get; set; }
        public string item { get; set; }
        public string token { get; set; }
        public string from { get; set; }
        public string value { get; set; }
        public string feedback { get; set; }
    }
}