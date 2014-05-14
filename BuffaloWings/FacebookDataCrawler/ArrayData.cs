using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Dldw.BuffaloWings.Facebook
{
    [DataContract]
    public class ArrayData<T>
    {
        [DataMember(Name = "data")]
        public T[] Data { get; set; }

        [DataMember(Name = "paging")]
        public Paging PagingInfo { get; set; }
    }

    public class Paging
    {
        [DataMember(Name = "next")]
        public string Next { get; set; }

        [DataMember(Name = "cursors")]
        public Cursors Cursors { get; set; }
    }

    public class Cursors
    {
        [DataMember(Name = "before")]
        public string Before { get; set; }

        [DataMember(Name = "after")]
        public string After { get; set; }
    }
}
