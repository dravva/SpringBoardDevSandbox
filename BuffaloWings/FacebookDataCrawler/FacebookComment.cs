using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class FacebookComment
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "comment_count")]
        public int CommentCount { get; set; }

        [DataMember(Name = "like_count")]
        public int LikeCount { get; set; }

        [DataMember(Name = "created_time")]
        public DateTime CreateTime { get; set; }

        [DataMember(Name = "from")]
        public FacebookUser From { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "message_tags")]
        public FacebookUser[] MessageTags { get; set; }

        [DataMember(Name = "user_likes")]
        public bool LikedByMe { get; set; }
    }
}
