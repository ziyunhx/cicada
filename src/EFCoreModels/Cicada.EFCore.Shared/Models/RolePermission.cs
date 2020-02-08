using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Models
{
    public class RolePermission
    {
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
        public int Type { get; set; }
    }
}
