using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class CheckResult
    {
        public CheckResult()
        {
            Notices = new HashSet<Notice>();
            NotifyRecodes = new HashSet<NotifyRecode>();
        }

        public string CheckId { get; set; }
        public int Status { get; set; }
        public string MessageInfo { get; set; }
        public int MessageLevel { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long CostMillisecond { get; set; }
        public string ClientId { get; set; }
        public int? ProcessId { get; set; }
        public string TaskId { get; set; }

        public virtual TaskInfo TaskInfo { get; set; }
        public virtual ICollection<Notice> Notices { get; set; }
        public virtual ICollection<NotifyRecode> NotifyRecodes { get; set; }
    }
}
