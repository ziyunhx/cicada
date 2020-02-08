using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core.Models
{
    public class NextRunTask : IComparable<NextRunTask>
    {
        public string TaskId { set; get; }
        public DateTime NextRunTime { set; get; }

        public int CompareTo(NextRunTask other)
        {
            if (this.NextRunTime == other.NextRunTime)
                return 0;
            else if (this.NextRunTime > other.NextRunTime)
                return 1;
            else
                return -1;
        }
    }
}
