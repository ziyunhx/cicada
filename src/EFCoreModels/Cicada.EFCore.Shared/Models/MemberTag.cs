using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class MemberTag
    {
        public string MemberId { get; set; }
        public string TagId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Member Member { get; set; }
    }
}
