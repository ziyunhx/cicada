using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.ViewModels.Tasks
{
    public class TaskNoticationDto
    {
        public string NotifyRecodeId { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string ReplyMsg { get; set; }
        public DateTime ReplyTime { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public int CheckStatus { get; set; }
        public string MessageInfo { get; set; }
        public int MessageLevel { get; set; }
    }
}
