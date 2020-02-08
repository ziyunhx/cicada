using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core.Models
{
    public class EmailSettings
    {
        public string ApiKey { get; set; }
        public string ApiBaseUri { get; set; }
        public string RequestUri { get; set; }
        public string From { get; set; }
        public string DomainName { get; set; }
    }
}
