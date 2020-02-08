using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class MemberParty
    {
        public string MemberId { get; set; }
        public string PartyId { get; set; }

        public virtual Party Party { get; set; }
        public virtual Member Member { get; set; }
    }
}
