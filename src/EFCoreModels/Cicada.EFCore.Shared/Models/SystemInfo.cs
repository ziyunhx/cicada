using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class SystemInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
