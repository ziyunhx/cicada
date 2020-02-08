using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagParties = new HashSet<TagParty>();
            UserTags = new HashSet<MemberTag>();
        }

        public string TagId { get; set; }
        public string TagName { get; set; }
        public long Order { get; set; }
        public string Remark { get; set; }
        public string ExtendId { get; set; }
        public string FromSource { get; set; }

        public virtual ICollection<TagParty> TagParties { get; set; }
        public virtual ICollection<MemberTag> UserTags { get; set; }
    }
}
