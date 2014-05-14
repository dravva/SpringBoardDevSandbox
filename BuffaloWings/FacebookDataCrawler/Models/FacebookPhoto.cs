using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class FacebookPhoto
    {

        [DataMember(Name = "object_id")]
        public string Id { get; set; }

        [DataMember(Name = "pid")]
        public string PId { get; set; }
        
        [DataMember(Name = "aid")]
        public string AId { get; set; }

        [DataMember(Name = "created")]
        public long CreatedTimestamp { get; set; }

        [DataMember(Name = "src_big")]
        public string NormalUrl { get; set; }

        [DataMember(Name = "src_small")]
        public string SmallUrl { get; set; }

        [DataMember(Name = "caption")]
        public string Caption { get; set; }

        [DataMember(Name = "comment_info")]
        public PhotoCommentsInfo PhotoCommentsInfo { get; set; }

        [DataMember(Name = "like_info")]
        public PhotoLikesInfo PhotoLikesInfo { get; set; }

        [DataMember(Name = "caption_tags")]
        public IDictionary<string,ArrayData<TagInfo>> Tags { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

    }

    [DataContract]
    public class PhotoCommentsInfo
    {
        [DataMember(Name = "comment_count")]
        public int CommentsCount { get; set; }
    }

    [DataContract]
    public class PhotoLikesInfo
    {
        [DataMember(Name = "like_count")]
        public int LikesCount { get; set; }
    }
    [DataContract]
    public class TagInfo
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

}
