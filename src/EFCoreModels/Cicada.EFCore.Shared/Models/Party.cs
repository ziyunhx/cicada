using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class Party
    {
        public Party()
        {
            TagParties = new HashSet<TagParty>();
            MemberParties = new HashSet<MemberParty>();
        }

        public string PartyId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public long Order { get; set; }
        public string ParentPartyId { get; set; }
        public string ExtendId { get; set; }
        public string FromSource { get; set; }

        public virtual ICollection<TagParty> TagParties { get; set; }
        public virtual ICollection<MemberParty> MemberParties { get; set; }
    }
}
