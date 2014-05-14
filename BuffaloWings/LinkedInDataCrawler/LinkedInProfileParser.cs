using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Microsoft.Dldw.BuffaloWings.LinkedIn
{
    public static class LinkedInProfileParser
    {

        public static LinkedInUser ExtractProfile(String profileXml)
        {

            var user = new LinkedInUser();

            var doc = new XmlDocument();

            doc.LoadXml(profileXml);

            XmlNodeList nodeList = doc.SelectNodes("//person/skills/skill/skill/name/text()");

            if (nodeList != null)
                foreach (XmlNode node in nodeList)
                {
                
                    user.Skills.Add(node.Value);
                }


            XmlNode headline = doc.SelectSingleNode("//person/headline/text()");

            if (headline != null) user.ProfileHeadline = headline.Value;


            XmlNodeList jobsNodeList = doc.SelectNodes("//person/positions/position/title/text()");

            XmlNodeList companyNodeList = doc.SelectNodes("//person/positions/position/company/name/text()");

            if (jobsNodeList != null && companyNodeList!=null)
            {

                var index = 0;
                foreach (XmlNode node in jobsNodeList)
                {
                    var professionalJob = new ProfessionJob();


                    if (node != null)
                    {
                        professionalJob.Designation = node.Value;
                        professionalJob.IsCurrent = (index==0);

                    }

                    if (companyNodeList[index] != null)
                    {

                        professionalJob.Company = companyNodeList[index].Value;

                    }
                    index++;

                    user.ProfessionalJobs.Add(professionalJob);
                }
            }

            return user;

        }
    }
}
