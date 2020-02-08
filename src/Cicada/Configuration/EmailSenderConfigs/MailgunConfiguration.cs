using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.Configuration.EmailSenderConfigs
{
    public class MailgunConfiguration
    {
        public string Domain { get; set; }
        public string SourceEmail { get; set; }
        public string SourceName { get; set; }
        public string ApiKey { get; set; }
    }
}
