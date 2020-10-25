using aceka.infrastructure.Models;
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
    public class MuhasebeController : ApiController
    {
        #region Degiskenler
        private MuhasebeRepository muhasebeRepository = null;
        #endregion

        /// <summary>
        /// Muhasebe Tanımları -> Masraf Merkezleri Listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///         {
        ///             masraf_merkezi_id: 1,
        ///             masraf_merkezi_adi: "Merkez"
        ///        },
        ///        {
        ///             masraf_merkezi_id: 2,
        ///             masraf_merkezi_adi: "Merkez Depo"
        ///         }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/muhasebe/masraf-merkezleri")]
        public HttpResponseMessage MuhasebeTanimMasrafMerkezleri()
        {
            muhasebeRepository = new MuhasebeRepository();
            var masrafMerkezleri = muhasebeRepository.MuhasebeTanimMasrafMerkezleri();
            if (masrafMerkezleri != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, masrafMerkezleri.Select(mtmd => new
                {
                    mtmd.masraf_merkezi_id,
                    mtmd.masraf_merkezi_adi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
    }
}
