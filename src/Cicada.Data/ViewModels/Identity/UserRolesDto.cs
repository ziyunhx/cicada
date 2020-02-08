using Cicada.ViewModels.Identity.Base;
using System.Collections.Generic;

namespace Cicada.ViewModels.Identity
{
    public class UserRolesDto : BaseUserRolesDto<string, string>
    {
        public UserRolesDto()
        {
           Roles = new List<RoleDto>(); 
        }
       

        public List<RoleDto> Roles { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }
}
