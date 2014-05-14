using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Microsoft.Dldw.BuffaloWings.MT.UserInterestAggregator
{
    using Microsoft.Dldw.BuffaloWings.LinkedIn;

    [DataContract]
    public class UserProfile
    {
        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public String MiddleName { get; set; }

        [DataMember]
        public String Birthday { get; set; }
      
        [DataMember]
        public String Location { get; set; }  // City, Country

        [DataMember]
        public IDictionary<String, UserInterest> UserInterests { get; set; }  // Movies, Videso, Sports

        [DataMember]
        public LinkedInUser LinkedInProfile { get; set; }  
        
    }

    [DataContract]
    public class UserInterest
    {
        [DataMember]
        public String Category { get; set; }  // Games

        [DataMember]
        public IDictionary<String, UserInterestSubCategoryItems> UserSubcategoryList { get; set; }  // C#, .Net


        public UserInterest()
        {
         UserSubcategoryList= new Dictionary<string, UserInterestSubCategoryItems>();   
        }
    }
    [DataContract]
    public class UserInterestSubCategoryItems
    {
        [DataMember]
        public String SubCategory { get; set; }  // Games

        [DataMember]
        public IList<UserInterestItem> InterestItemList { get; set; }

        public UserInterestSubCategoryItems()
        {
            InterestItemList = new List<UserInterestItem>();
        }
    }
    [DataContract]
    public class UserInterestItem
    {
        [DataMember]
        public String Title { get; set; }  // Movie Name, optional

         [DataMember]
        public String Url { get; set; }  // www.something.com, optional
    }


    //[DataContract]
    //public class LinkedInProfile
    //{
    //    [DataMember]
    //    public IList<ProfessionJob> ProfessionalJobs { get; set; }

    //    [DataMember]
    //    public String ProfileHeadline { get; set; }  // Custom Fields

    //    public LinkedInProfile()
    //    {
    //        ProfessionalJobs= new List<ProfessionJob>();
    //    }
    //}

   
}