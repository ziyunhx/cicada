using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class Member
    {
        public Member()
        {
            MemberParties = new HashSet<MemberParty>();
            MemberTags = new HashSet<MemberTag>();
        }

        public string MemberId { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Status { get; set; }
        public string ExtendId { get; set; }
        public string FromSource { get; set; }

        public virtual ICollection<MemberParty> MemberParties { get; set; }
        public virtual ICollection<MemberTag> MemberTags { get; set; }
    }
}
