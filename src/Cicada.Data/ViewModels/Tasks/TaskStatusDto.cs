using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.ViewModels.Tasks
{
    public class TaskStatusDto
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public int Status { get; set; }
        public int LastCheckStatus { get; set; }
        public string MessageInfo { get; set; }
        public int MessageLevel { get; set; }
        public DateTime LastCheckDateTime { get; set; }
        public double CostMillisecond { get; set; }
    }
}
