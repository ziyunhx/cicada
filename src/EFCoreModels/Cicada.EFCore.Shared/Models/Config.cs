using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Models
{
    public class Config
    {
        public string Key { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public int Status { get; set; }
        public string Descript { get; set; }
        public sbyte EnableView { get; set; }
        public string EffectModel { get; set; }
        public string VaildRule { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Order { get; set; }
    }
}
