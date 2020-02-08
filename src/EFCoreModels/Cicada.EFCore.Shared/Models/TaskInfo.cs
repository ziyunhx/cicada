using Cicada.EFCore.Shared.Enums;
using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class TaskInfo
    {
        public TaskInfo()
        {
            CheckResults = new HashSet<CheckResult>();
            Notices = new HashSet<Notice>();
            NotifyRecodes = new HashSet<NotifyRecode>();
        }

        public string TaskId { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public string Params { get; set; }
        public TaskTypeEnum Type { get; set; }
        public int? SupportPlatforms { get; set; }
        public string SupportClientIds { get; set; }
        public string Owner { get; set; }
        public int Status { get; set; }
        public RuleTypeEnum RuleType { get; set; }
        public string Schedule { get; set; }
        public int Retries { get; set; }
        public long Timeout { get; set; }        
        public DateTime? EndTime { get; set; }
        public string WorkDays { get; set; }
        public string SleepDays { get; set; }
        public string WorkTimes { get; set; }
        public string SleepTimes { get; set; }
        public string WorkWeeks { get; set; }
        public string SleepWeeks { get; set; }
        public string WorkCalendar { get; set; }
        public string SleepCalendar { get; set; }

        public bool SleepFirst { get; set; }
        public int? LastCheckStatus { get; set; }
        public DateTime? NextRunTime { get; set; }

        public int MaxRuningThread { get; set; }
        public int MaxThreadInOneClient { get; set; }

        public DateTime RowVersion { get; set; }

        public virtual ICollection<CheckResult> CheckResults { get; set; }
        public virtual ICollection<Notice> Notices { get; set; }
        public virtual ICollection<NotifyRecode> NotifyRecodes { get; set; }
    }
}
