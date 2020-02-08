using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class NotifyRecode
    {
        public string NotifyRecodeId { get; set; }
        public string MemberId { get; set; }
        public string CheckId { get; set; }
        public string TaskId { get; set; }
        public string Msg { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string ReplyMsg { get; set; }
        public DateTime ReplyTime { get; set; }

        public virtual CheckResult CheckResult { get; set; }
        public virtual TaskInfo TaskInfo { get; set; }
    }
}
