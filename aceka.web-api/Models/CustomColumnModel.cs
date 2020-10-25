using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models
{
    public class BaseColumnModel
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class SiparisPivotColumnModel : BaseColumnModel
    {
    }
}