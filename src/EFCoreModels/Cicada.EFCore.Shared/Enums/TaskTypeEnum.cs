using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.EFCore.Shared.Enums
{
    public enum TaskTypeEnum
    {
        StandAloneTimeTask = 1,
        StandAloneKeepTask = 2,
        InheritTimeTask = 3,
        InheritKeepTask = 4
    }
}
