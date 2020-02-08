using System;
using System.Collections.Generic;

namespace Cicada.EFCore.Shared.Models
{
    public partial class Notice
    {
        public Notice()
        {
            NotifyRecodes = new HashSet<NotifyRecode>();
        }

        public string NoticeId { get; set; }
        public string EffectLevels { get; set; }
        public string IgnoreTime { get; set; }
        public string ToMembers { get; set; }
        public string ToParties { get; set; }
        public string ToTags { get; set; }
        public string TaskId { get; set; }

        public virtual TaskInfo TaskInfo { get; set; }
        public virtual ICollection<NotifyRecode> NotifyRecodes { get; set; }
    }
}
