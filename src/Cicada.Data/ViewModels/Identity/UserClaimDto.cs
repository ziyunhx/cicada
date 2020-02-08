using System.ComponentModel.DataAnnotations;
using Cicada.ViewModels.Identity.Base;

namespace Cicada.ViewModels.Identity
{
    public class UserClaimDto : BaseUserClaimDto<string, int>
    {
        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }
    }
}