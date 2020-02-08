using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.EFCore.Shared.Enums
{
    public enum RuleTypeEnum
    {
        /// <summary>
        /// ISO8601 Interval Notation.
        /// </summary>
        ISO8601 = 1,
        /// <summary>
        /// Cron
        /// </summary>
        Cron = 2,
        /// <summary>
        /// Interval second
        /// </summary>
        Interval = 3,
        /// <summary>
        /// Fixed time
        /// </summary>
        Fixed = 4
    }
}
