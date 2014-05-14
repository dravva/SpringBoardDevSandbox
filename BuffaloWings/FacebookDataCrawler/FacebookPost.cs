using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class FacebookPost
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "story")]
        public string Story { get; set; }

        [DataMember(Name = "created_time")]
        public DateTime CreateTime { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "from")]
        public FacebookUser From { get; set; }

        [DataMember(Name = "to")]
        public ArrayData<Profile> To { get; set; }

        [DataMember(Name = "with_tags")]
        public ArrayData<FacebookUser> WithTags { get; set; }

        [DataMember(Name = "tags")]
        public ArrayData<Profile> Tags { get; set; } 

        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "is_hidden")]
        public bool IsHidden { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "object_id")]
        public string ObjectId { get; set; }

        [DataMember(Name = "picture")]
        public string Picture { get; set; }

        [DataMember(Name = "source")]
        public string OriginalPicture { get; set; }

        [DataMember(Name = "likes")]
        public ArrayData<FacebookUser> Likes { get; set; }

        [DataMember(Name = "comments")]
        public ArrayData<FacebookComment> Comments { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
