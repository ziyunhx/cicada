using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core.Models
{
    public class TaskCheckModel
    {
        public string CheckId { set; get; }
        public string TaskId { set; get; }
        public DateTime StartTime { set; get; }
        public string ClientId { set; get; }
        public string ProcessId { set; get; }
        public int Status { set; get; }
        public DateTime? LastModifyTime { set; get; }
        public StringBuilder Message { set; get; }

        public int MessageLevel { set; get; }
    }
}
