using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Models
{
    public class UserPermission
    {
        public string UserId { get; set; }
        public string PermissionId { get; set; }
        public int Type { get; set; }
    }
}
