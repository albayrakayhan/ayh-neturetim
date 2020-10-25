using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
using aceka.web_api.Models.StokkartModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace aceka.web_api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class APITestController : ApiController
    {


        public IEnumerable<string> Get()
        {
            return new string[] { "current_Date", DateTime.Now.ToShortDateString() };
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]dataTest model)
        {

            return Request.CreateResponse(HttpStatusCode.OK);

        }

    }

    public class dataTest
    {
        public DateTime date { get; set; }
    }
}

