using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comlib
{
    public class ApiResult
    {
        public bool ret { get; set; }
        public object RetObj { get; set; }
        public enReturnValue RetCode { get; set; }
        public string Message = "";
    }
}
