using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class TagParty
    {
        public string TagId { get; set; }
        public string PartyId { get; set; }

        public virtual Party Party { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
