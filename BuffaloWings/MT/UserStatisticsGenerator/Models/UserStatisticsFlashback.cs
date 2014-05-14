using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dldw.BuffaloWings.Facebook;

namespace UserStatisticsGenerator
{
    [DataContract]
    public class UserStatisticsFlashback
    {
        public FacebookPhoto FacebookPhoto { get; set; }

        public FacebookPost FacebookPost { get; set; }

    }
}
