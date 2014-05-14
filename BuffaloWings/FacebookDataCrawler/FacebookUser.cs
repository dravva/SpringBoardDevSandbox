using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class FacebookUser
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "about")]
        public string About { get; set; }

        [DataMember(Name = "bio")]
        public string Bio { get; set; }

        [DataMember(Name = "birthday")]
        public DateTime? Birthday { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "middle_name")]
        public string MiddleName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "name")]
        public string FullName { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "is_verified")]
        public bool IsManuallyVerified { get; set; }

        [DataMember(Name = "verified")]
        public bool IsVerified { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "relationship_status")]
        public string RelationshipStatus { get; set; }

        [DataMember(Name = "significant_other")]
        public FacebookUser SignificantOther { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "pic")]
        public string Picture { get; set; }

        [DataMember(Name = "uid")]
        public string Uid { get; set; }

        [DataMember(Name = "education")]
        public Education[] EducationHistory { get; set; }

        [DataMember(Name = "books")]
        public string Books { get; set; }

        [DataMember(Name = "interests")]
        public string Interests { get; set; }

        [DataMember(Name = "movies")]
        public string Movies { get; set; }

        [DataMember(Name = "music")]
        public string Music { get; set; }

        [DataMember(Name = "sports")]
        public Profile[] Sports { get; set; }

        [DataMember(Name = "tv")]
        public string Tv { get; set; }

        [DataMember(Name = "work")]
        public Company[] Works { get; set; }

        [DataMember(Name = "location")]
        public Profile Location { get; set; }



    }

    public class Education
    {
        [DataMember(Name = "school")]
        public Profile School { get; set; }

        [DataMember(Name = "year")]
        public Profile Year { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "concentration")]
        public Profile[] Concentration { get; set; }
    }

    public class Company
    {
        [DataMember(Name = "employer")]
        public Profile Employer { get; set; }

        [DataMember(Name = "location")]
        public Profile Location { get; set; }

        [DataMember(Name = "position")]
        public Profile Position { get; set; }

        [DataMember(Name = "start_date")]
        public string StartDate { get; set; }
    }

    public class Profile
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

}
