using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStatisticsGenerator.Models
{
    public class ActivityResult
    {

        public int Total { get; set; }

        public int TotalLike { get; set; }

        public int TotalShares { get; set; }

        public int TotalStatusUpdates { get; set; }

        public int EngagementMadeToOthers { get; set; }

        public int EngagementFromOthers { get; set; }

    }
}
