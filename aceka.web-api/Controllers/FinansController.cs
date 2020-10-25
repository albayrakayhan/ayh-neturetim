using aceka.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Muhasebe işlemleri metordları
    /// </summary>
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FinansController : ApiController
    {
        #region Degiskenler
        private FinansRepository finansRepository = null;
        #endregion

        /// <summary>
        /// Finans Tanımları -> Ödeme Planları Listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///         {
        ///             odeme_plani_id: 1,
        ///             odeme_plani_adi: "Test 1"
        ///        },
        ///        {
        ///             odeme_plani_id: 2,
        ///             odeme_plani_adi: "Test 2"
        ///         }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/finans/odeme-planlari")]
        public HttpResponseMessage FinansTanimOdemeplanlari()
        {
            finansRepository = new FinansRepository();
            var odemePlanlari = finansRepository.FinansTanimOdemeplanlari();
            if (odemePlanlari != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, odemePlanlari.Select(op => new
                {
                    op.odeme_plani_id,
                    op.odeme_plani_kodu,
                    op.odeme_plani_adi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

    }
}
