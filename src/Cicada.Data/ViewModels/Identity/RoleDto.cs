using System.ComponentModel.DataAnnotations;
using Cicada.ViewModels.Identity.Base;

namespace Cicada.ViewModels.Identity
{
    public class RoleDto : BaseRoleDto<string>
    {      
        [Required]
        public string Name { get; set; }
    }
}