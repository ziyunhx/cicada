﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Models
{
    public class Permission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Type { get; set; }
        public string PluginId { get; set; }
        public string Descript { get; set; }
        public int Status { get; set; }
    }
}
