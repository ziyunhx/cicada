using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Models
{
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string IP { get; set; }
        public string Remark { get; set; }
        public int PlatformId { get; set; }
        public int MaxProcessNum { get; set; }
        public int ClientType { get; set; }
        public string PublicKey { get; set; }
    }
}
