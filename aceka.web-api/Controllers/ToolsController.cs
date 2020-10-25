using aceka.infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace aceka.web_api.Controllers
{
    public class ToolsController : ApiController
    {
        // GET: api/Tools
        [HttpGet]
        [Route("api/test/encrypt/{key}")]
        public IEnumerable<string> Encrypt(string key)
        {
            var encrypt = Tools.Encrypt(key);
            return new string[] { "value", encrypt };
        }

      
    }
}
