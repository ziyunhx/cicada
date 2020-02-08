using System.ComponentModel.DataAnnotations;
using Cicada.ViewModels.Identity.Base;

namespace Cicada.ViewModels.Identity
{
    public class UserChangePasswordDto : BaseUserChangePasswordDto<string>
    {        
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
