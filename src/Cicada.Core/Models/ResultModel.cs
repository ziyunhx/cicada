using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicada.Core.Models
{
    public class ResultModel<T>
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public T Data { get; set; }
    }

    public class ResultModel
    {
        public int Code { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
