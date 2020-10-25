using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Core
{
    public class ErrorInfo
    {
        public string Source { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string Location { get; set; }
    }
}
