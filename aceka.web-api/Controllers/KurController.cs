using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using aceka.infrastructure.Repositories;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;
using aceka.web_api.Models.KurModels;
using System.Web.Http.Cors;
using aceka.web_api.Models;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Döviz kurları ile ilgili metodlar.
    /// </summary>
    /// 

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KurController : ApiController
    {
        #region Degiskenler
        private List<Kur> kurlar = null;
        private KurRepository kurRepository = null;
        #endregion

        // <summary>
        // Exchange Rate List
        // </summary>
        // <param name="parameters"></param>
        // <returns></returns>
        //[HttpPost]
        //public IList<Kur> Get(KurGetParameters parameters)
        //{
        //    kurRepository = new KurRepository();
        //    kurlar = kurRepository.Getir(parameters.tarih, parameters.pb);
        //    return kurlar;
        //}

        /// <summary>
        /// Kur listesini getirir.
        /// </summary>
        /// <param name="sene">exchange rate date parameter</param>
        /// <param name="ay">exchange rate month parameter</param>
        /// <param name="pb">exchange rate currency parameter</param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/kur")]
        //[Route("api/kur/{tarih}/{pb}")]
        [Route("api/kur/{sene}/{ay}/{pb}")]
        public IList<Kur> Get(string sene, string ay,int pb)
        {
            kurRepository = new KurRepository();

            kurlar = kurRepository.Getir(sene, ay,pb);
            return kurlar;
        }

        /// <summary>
        /// Exchange Rate Year List.
        /// </summary>
        /// <returns>
        /// [
        ///     {
        ///       year: "2007"
        ///     },
        ///     {
        ///       year: "2008"
        ///     },
        ///     {
        ///       year: "2009"
        ///     },
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/kur")]
        [Route("api/kur/seneGetir")]
        public HttpResponseMessage seneGetir()
        {
            kurRepository = new KurRepository();
            var seneler = kurRepository.seneGetir();
            if (seneler != null)
            {
                var ozet = seneler.Select(s => new
                {
                    sene = s.sene_adi
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }


        /// <summary>
        /// Kur listesindeki Yılların Listesini getirir.
        /// </summary>
        /// <returns>
        /// [
        ///     {
        ///       month: "01"
        ///     },
        ///     {
        ///        month: "02"
        ///     },
        ///     {
        ///        month: "03"
        ///     },
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/kur")]
        [Route("api/kur/ayGetir")]
        public HttpResponseMessage ayGetir()
        {
            kurRepository = new KurRepository();
            var seneler = kurRepository.ayGetir();
            if (seneler != null)
            {
                var ozet = seneler.Select(s => new
                {
                    ay_adi = s.ay_adi,
                    ay_no=s.ay_no
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }



        //[HttpPost]
        //[Route("api/kur/GetWithPost")]
        //public HttpResponseMessage GetWithPost(string tarih, string currency)
        //{
        //    //kurRepository = new KurRepository();
        //    //kurlar = kurRepository.Getir(parameters.Tarih, parameters.Currency);
        //    return Request.CreateResponse(HttpStatusCode.NoContent);
        //}

    }
}
