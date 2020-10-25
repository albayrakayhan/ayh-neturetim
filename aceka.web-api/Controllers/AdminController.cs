using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
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
    /// Admin ile ilgili service
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {

        #region Degiskenler
        private ParametreRepository parametreRepository = null;
        #endregion

        #region Parametre Birimler Sayfası
        /// <summary>
        /// Birimlerin listesini veren metod.Adet,Kilogram,Metre vb.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///  "birim_id": 10,
        ///  "birim_kod": "ad",
        ///  "birim_adi": "Adet",
        ///  "statu": true,
        ///  "kayit_silindi": false,
        ///  "ondalik": 0
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/birimliste")]
        [Route("api/admin/birimliste")]
        public HttpResponseMessage BirimGetir()
        {
            parametreRepository = new ParametreRepository();
            var birimler = parametreRepository.Birimler();
            if (birimler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, birimler.Select(brm => new
                {
                    brm.birim_id,
                    brm.birim_adi,
                    brm.birim_kod,
                    brm.statu,
                    brm.kayit_silindi,
                    brm.ondalik
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Birimler POST Metodu
        /// </summary>
        /// /// <param name="brm"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/birimliste")]
        [Route("api/admin/birimliste")]
        public HttpResponseMessage BirimlerPost(parametre_birim brm)
        {
            parametreRepository = new ParametreRepository();

            if (brm != null)
            {
                brm.degistiren_tarih = DateTime.Now;
                brm.degistiren_carikart_id = Tools.PersonelId;
                var birimRetVal = CrudRepository<parametre_birim>.Insert(brm, "parametre_birim", new string[] { "birim_id" });

                if (birimRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, birimRetVal.ErrorInfo.Message);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }


        /// <summary>
        /// Parametre Birimler PUT Metodu
        /// </summary>
        /// /// <param name="brm"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/birimliste")]
        [Route("api/admin/birimliste")]
        public HttpResponseMessage BirimlerPut(parametre_birim brm)
        {
            AcekaResult acekaResult = null;
            if (brm != null)
            {
                if (brm.birim_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("birim_id", brm.birim_id);
                    fields.Add("birim_adi", brm.birim_adi);
                    fields.Add("birim_adi_dil_1", brm.birim_adi_dil_1);
                    fields.Add("birim_adi_dil_2", brm.birim_adi_dil_2);
                    fields.Add("birim_adi_dil_3", brm.birim_adi_dil_3);
                    fields.Add("birim_adi_dil_4", brm.birim_adi_dil_4);
                    fields.Add("birim_adi_dil_5", brm.birim_adi_dil_5);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);
                    fields.Add("kayit_silindi", brm.kayit_silindi);
                    fields.Add("ondalik", brm.ondalik);
                    fields.Add("statu", brm.statu);

                    acekaResult = CrudRepository.Update("parametre_birim", "birim_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Model kart  -> Varyant Bedenler-> DELETE Silme.
        /// </summary>
        /// <param name="brm"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/birimliste")]
        [Route("api/admin/birimliste")]
        public HttpResponseMessage BirimlerDelete(parametre_birim brm)
        {
            if (brm != null)
            {
                parametreRepository = new ParametreRepository();
                // var birimListe = parametreRepository.Birimler(brm.birim_id);
                if (brm != null)
                {
                    parametre_birim prbirim = new parametre_birim();
                    prbirim.birim_id = brm.birim_id;
                    prbirim.kayit_silindi = brm.kayit_silindi;
                    prbirim.degistiren_carikart_id = Tools.PersonelId;
                    prbirim.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_birim>.Update(prbirim, "birim_id", new string[] { "birim_adi", "birim_adi_dil_1", "birim_adi_dil_2", "birim_adi_dil_3", "birim_adi_dil_4", "birim_adi_dil_5", "ondalik", "statu" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = prbirim.birim_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Zorluk Grubu Sayfası
        /// <summary>
        /// Zorluk Gruplarının listesini veren metod. Genel,ni-Re,int vb.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "zorlukgrubu_id": 1,
        ///   "sira": 0,
        ///   "tanim": "Genel",
        ///   "varsayilan": true,
        ///   "kayit_silindi": false,
        ///   "degistiren_tarih": "2016-11-28T17:31:07",
        ///   "degistiren_carikart_id": 0
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/zorlukgrubu")]
        [Route("api/admin/zorlukgrubu")]
        public HttpResponseMessage ZorlukGrubuGetir()
        {
            parametreRepository = new ParametreRepository();
            var birimler = parametreRepository.ZorlukGrubuGetir();
            if (birimler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, birimler.Select(zg => new
                {
                    zg.zorlukgrubu_id,
                    zg.sira,
                    zg.tanim,
                    zg.varsayilan,
                    zg.kayit_silindi,
                    zg.degistiren_tarih,
                    zg.degistiren_carikart_id,

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Zorlukg grubu POST Metodu
        /// </summary>
        /// /// <param name="zgrb"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/zorlukgrubu")]
        [Route("api/admin/zorlukgrubu")]
        public HttpResponseMessage ZorlukGrubuPost(parametre_zorlukgrubu zgrb)
        {
            parametreRepository = new ParametreRepository();
            if (zgrb != null)
            {
                zgrb.degistiren_tarih = DateTime.Now;
                zgrb.degistiren_carikart_id = Tools.PersonelId;
                var ZorlukRetVal = CrudRepository<parametre_zorlukgrubu>.Insert(zgrb, "parametre_zorlukgrubu", new string[] { "zorlukgrubu_id" });
                if (ZorlukRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ZorlukRetVal.ErrorInfo.Message);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }


        /// <summary>
        /// Parametre Zorlukg grubu PUT Metodu
        /// </summary>
        /// /// <param name="zgrb"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/zorlukgrubu")]
        [Route("api/admin/zorlukgrubu")]
        public HttpResponseMessage ZorlukGrubuPut(parametre_zorlukgrubu zgrb)
        {
            AcekaResult acekaResult = null;
            if (zgrb != null)
            {
                if (zgrb.zorlukgrubu_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("zorlukgrubu_id", zgrb.zorlukgrubu_id);
                    fields.Add("kayit_silindi", zgrb.kayit_silindi);
                    fields.Add("sira", zgrb.sira);
                    fields.Add("tanim", zgrb.tanim);
                    fields.Add("varsayilan", zgrb.varsayilan);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);

                    acekaResult = CrudRepository.Update("parametre_zorlukgrubu", "zorlukgrubu_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Parametre Zorlukg grubu DELETE Metodu
        /// </summary>
        /// <param name="zgrb"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/zorlukgrubu")]
        [Route("api/admin/zorlukgrubu")]
        public HttpResponseMessage ZorlukGrubuDelete(parametre_zorlukgrubu zgrb)
        {
            if (zgrb != null)
            {
                parametreRepository = new ParametreRepository();
                //ar birimListe = parametreRepository.ZorlukGrubuGetir(zgrb.zorlukgrubu_id);
                if (zgrb != null)
                {
                    parametre_zorlukgrubu prbirim = new parametre_zorlukgrubu();
                    prbirim.zorlukgrubu_id = zgrb.zorlukgrubu_id;
                    prbirim.kayit_silindi = zgrb.kayit_silindi;
                    prbirim.degistiren_carikart_id = Tools.PersonelId;
                    prbirim.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_zorlukgrubu>.Update(prbirim, "zorlukgrubu_id", new string[] { "sira", "tanim", "varsayilan" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = prbirim.zorlukgrubu_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Üretim Yeri Sayfası
        /// <summary>
        /// Üretim Yerlerinin listesini veren metod. Genel,ni-Re,int vb.
        /// </summary>
        /// <returns>
        /// [
        ///{
        ///  "uretimyeri_id": 104,
        ///  "uretimyeri_kod": "AJ",
        ///  "uretimyeri_rgb": 0,
        ///  "uretimyeri_tanim": "AJARA-Georgia",
        ///  "kayit_silindi": false,
        ///  "degistiren_tarih": "2009-02-27T18:41:11",
        ///  "degistiren_carikart_id": 100000000100
        ///}
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/uretimyeri")]
        [Route("api/admin/uretimyeri")]
        public HttpResponseMessage UretimyeriGetir()
        {
            parametreRepository = new ParametreRepository();
            var uyeri = parametreRepository.Uretimyer();
            if (uyeri != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, uyeri.Select(urt => new
                {
                    urt.uretimyeri_id,
                    urt.uretimyeri_kod,
                    urt.uretimyeri_rgb,
                    urt.uretimyeri_tanim,
                    urt.kayit_silindi,
                    urt.degistiren_tarih,
                    urt.degistiren_carikart_id,

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Üretim Yerleri POST Metodu
        /// </summary>
        /// /// <param name="uyer"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/uretimyeri")]
        [Route("api/admin/uretimyeri")]
        public HttpResponseMessage UretimyeriPost(parametre_uretimyeri uyer)
        {
            parametreRepository = new ParametreRepository();
            if (uyer != null)
            {
                uyer.degistiren_tarih = DateTime.Now;
                uyer.degistiren_carikart_id = Tools.PersonelId;
                var UretimRetVal = CrudRepository<parametre_uretimyeri>.Insert(uyer, "parametre_uretimyeri");
                if (UretimRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, UretimRetVal.ErrorInfo.Message);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }


        /// <summary>
        /// Üretim Yerleri PUT Metodu
        /// </summary>
        /// /// <param name="uyer"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/uretimyeri")]
        [Route("api/admin/uretimyeri")]
        public HttpResponseMessage UretimyeriPut(parametre_uretimyeri uyer)
        {
            AcekaResult acekaResult = null;
            if (uyer != null)
            {
                if (uyer.uretimyeri_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("uretimyeri_id", uyer.uretimyeri_id);
                    fields.Add("uretimyeri_kod", uyer.uretimyeri_kod);
                    fields.Add("uretimyeri_rgb", uyer.uretimyeri_rgb);
                    fields.Add("uretimyeri_tanim", uyer.uretimyeri_tanim);
                    fields.Add("kayit_silindi", uyer.kayit_silindi);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);
                    acekaResult = CrudRepository.Update("parametre_uretimyeri", "uretimyeri_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Üretim Yerleri DELETE Metodu
        /// </summary>
        /// <param name="uyer"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/uretimyeri")]
        [Route("api/admin/uretimyeri")]
        public HttpResponseMessage UretimyeriDelete(parametre_uretimyeri uyer)
        {
            if (uyer != null)
            {
                parametreRepository = new ParametreRepository();
                //var birimListe = parametreRepository.Uretimyer(uyer.uretimyeri_id);
                if (uyer != null)
                {
                    parametre_uretimyeri uretyer = new parametre_uretimyeri();
                    uretyer.uretimyeri_id = uyer.uretimyeri_id;
                    uretyer.kayit_silindi = uyer.kayit_silindi;
                    uretyer.degistiren_carikart_id = Tools.PersonelId;
                    uretyer.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_uretimyeri>.Update(uretyer, "uretimyeri_id", new string[] { "uretimyeri_kod", "uretimyeri_rgb", "uretimyeri_tanim" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = uretyer.uretimyeri_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Sezon Sayfası
        /// <summary>
        /// Sezonların listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///  "sezon_id": 1,
        ///  "sezon_kodu": "AW13",
        ///  "sezon_adi": "Autumn Winter 2013",
        ///  "satis": null,
        ///  "mal_kabul": null,
        ///  "mal_kabul_baslama": null,
        ///  "mal_kabul_bitis": null,
        ///  "satis_baslama": null,
        ///  "satis_bitis": null,
        ///  "kayit_silindi": false,
        ///  "degistiren_tarih": "2016-11-28T17:34:50",
        ///  "degistiren_carikart_id": 0
        ///}
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/sezon")]
        [Route("api/admin/sezon")]
        public HttpResponseMessage SezonlarGetir()
        {
            parametreRepository = new ParametreRepository();
            var uyeri = parametreRepository.Sezonlistesi();
            if (uyeri != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, uyeri.Select(szn => new
                {
                    szn.sezon_id,
                    szn.sezon_kodu,
                    szn.sezon_adi,
                    szn.satis,
                    szn.mal_kabul,
                    szn.mal_kabul_baslama,
                    szn.mal_kabul_bitis,
                    szn.satis_baslama,
                    szn.satis_bitis,
                    szn.kayit_silindi,
                    szn.degistiren_tarih,
                    szn.degistiren_carikart_id,

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Sezon POST Metodu
        /// </summary>
        /// /// <param name="szn"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/sezon")]
        [Route("api/admin/sezon")]
        public HttpResponseMessage SezonPost(parametre_sezon szn)
        {
            parametreRepository = new ParametreRepository();
            if (szn != null)
            {
                szn.degistiren_tarih = DateTime.Now;
                szn.degistiren_carikart_id = Tools.PersonelId;
                var UretimRetVal = CrudRepository<parametre_sezon>.Insert(szn, "parametre_sezon");
                if (UretimRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, UretimRetVal.ErrorInfo.Message);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }


        /// <summary>
        /// Sezon PUT Metodu
        /// </summary>
        /// /// <param name="szn"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/sezon")]
        [Route("api/admin/sezon")]
        public HttpResponseMessage SezonPut(parametre_sezon szn)
        {
            AcekaResult acekaResult = null;
            if (szn != null)
            {
                if (szn.sezon_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("sezon_id", szn.sezon_id);
                    fields.Add("sezon_adi", szn.sezon_adi);
                    fields.Add("sezon_kodu", szn.sezon_kodu);
                    fields.Add("satis_bitis", szn.satis_bitis);
                    fields.Add("satis_baslama", szn.satis_baslama);
                    fields.Add("satis", szn.satis);
                    fields.Add("mal_kabul_bitis", szn.mal_kabul_bitis);
                    fields.Add("mal_kabul_baslama", szn.mal_kabul_baslama);
                    fields.Add("mal_kabul", szn.mal_kabul);
                    fields.Add("kayit_silindi", szn.kayit_silindi);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);
                    acekaResult = CrudRepository.Update("parametre_sezon", "sezon_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Sezon DELETE Metodu
        /// </summary>
        /// <param name="szn"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/sezon")]
        [Route("api/admin/sezon")]
        public HttpResponseMessage SezonDelete(parametre_sezon szn)
        {
            if (szn != null)
            {
                parametreRepository = new ParametreRepository();
                // var birimListe = parametreRepository.Birimler(szn.sezon_id);
                if (szn != null)
                {
                    parametre_sezon sez = new parametre_sezon();
                    sez.sezon_id = szn.sezon_id;
                    sez.kayit_silindi = szn.kayit_silindi;
                    sez.degistiren_carikart_id = Tools.PersonelId;
                    sez.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_sezon>.Update(sez, "sezon_id", new string[] { "sezon_adi", "sezon_kodu", "satis_bitis", "satis_baslama", "satis", "mal_kabul_bitis", "mal_kabul_baslama", "mal_kabul" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = sez.sezon_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Bedenler
        /// <summary>
        /// Bedenleri listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "beden_id": 2,
        ///   "bedengrubu": "A",
        ///   "beden_tanimi": "4XL",
        ///    "beden": "4XL",
        ///   "degistiren_carikart_id": 0,
        ///   "degistiren_tarih": "0001-01-01T00:00:00",
        ///   "kayit_silindi": false,
        ///   "sira": 1500
        /// },
        /// {
        ///   "beden_id": 3,
        ///   "bedengrubu": "A",
        ///   "beden_tanimi": "5XL",
        ///   "beden": "5XL",
        ///   "degistiren_carikart_id": 0,
        ///   "degistiren_tarih": "0001-01-01T00:00:00",
        ///   "kayit_silindi": false,
        ///   "sira": 1510
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/beden")]
        [Route("api/admin/beden")]
        public HttpResponseMessage BedenGetir()
        {
            parametreRepository = new ParametreRepository();
            var beden = parametreRepository.BedenleriGetir();
            if (beden != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, beden.Select(bdn => new
                {
                    bdn.beden_id,
                    bdn.bedengrubu,
                    bdn.beden_tanimi,
                    bdn.beden,
                    bdn.degistiren_carikart_id,
                    bdn.degistiren_tarih,
                    bdn.kayit_silindi,
                    bdn.sira,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Bedenler POST Metodu. ÖNEMLİ!!:  Beden ve Beden grubunun da unique index var.
        /// Her beden ve Her beden grubunun dan sadece bir kayıt olabilir. Aksi takdirde hata verecektir.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="bdn"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/beden")]
        [Route("api/admin/beden")]
        public HttpResponseMessage SezonPost(parametre_beden bdn)
        {
            parametreRepository = new ParametreRepository();
            if (bdn != null)
            {
                bdn.degistiren_tarih = DateTime.Now;
                bdn.degistiren_carikart_id = Tools.PersonelId;
                var BedenRetVal = CrudRepository<parametre_beden>.Insert(bdn, "parametre_beden", new string[] { "beden_id" });
                if (BedenRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedenRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Bedenler PUT Metodu.
        /// </summary>
        /// /// <param name="bdn"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/beden")]
        [Route("api/admin/beden")]
        public HttpResponseMessage BedenPut(parametre_beden bdn)
        {
            AcekaResult acekaResult = null;
            if (bdn != null)
            {
                if (bdn.beden_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("beden_id", bdn.beden_id);
                    fields.Add("beden", bdn.beden);
                    fields.Add("bedengrubu", bdn.bedengrubu);
                    fields.Add("beden_tanimi", bdn.beden_tanimi);
                    fields.Add("sira", bdn.sira);
                    fields.Add("kayit_silindi", bdn.kayit_silindi);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);
                    acekaResult = CrudRepository.Update("parametre_beden", "beden_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        /// <summary>
        ///  Bedenler DELETE Metodu
        /// </summary>
        /// <param name="bdn"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/beden")]
        [Route("api/admin/beden")]
        public HttpResponseMessage BedenDelete(parametre_beden bdn)
        {
            if (bdn != null)
            {
                parametreRepository = new ParametreRepository();
                //var birimListe = parametreRepository.BedenleriGetir(bdn.beden_id);
                if (bdn != null)
                {
                    parametre_beden bedn = new parametre_beden();
                    bedn.beden_id = bdn.beden_id;
                    bedn.kayit_silindi = bdn.kayit_silindi;
                    bedn.degistiren_carikart_id = Tools.PersonelId;
                    bedn.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_beden>.Update(bedn, "beden_id", new string[] { "bedengrubu", "beden", "beden_tanimi", "sira" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = bedn.beden_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Stokkart Rapor
        /// <summary>
        /// parametre_stokkart_raporların listesini veren metod.
        /// parametre,parametre_grubu ve kayit_silindi boş geçilemez.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "parametre_id": 1,
        ///   "parametre": 1,
        ///   "parametre_grubu": 0,
        ///   "renk_rgb": 0,
        ///   "sira": 0,
        ///   "tanim": "Puma Alm.TS",
        ///   "deger1": 0,
        ///   "deger2": 0,
        ///   "kayit_silindi": false,
        ///   "degistiren_tarih": "2017-04-27T14:22:23.6301432+03:00",
        ///   "degistiren_carikart_id": 100000000100,
        ///   "dil_1_tanim": "",
        ///   "dil_2_tanim": "",
        ///   "dil_3_tanim": "0",
        ///   "dil_4_tanim": "",
        ///   "dil_5_tanim": "",
        ///   "kaynak_1_parametre_id": 0,
        ///   "kaynak_2_parametre_id": 0,
        ///   "kaynak_3_parametre_id": 0,
        ///   "kaynak_4_parametre_id": 0,
        ///   "kod": "ps",
        ///   "kod1": "",
        ///   "kod2": "",
        ///   "kod3": "",
        ///   "kod4": "",
        ///   "kod5": "",
        ///   "kod6": ""
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/stok-rapor")]
        [Route("api/admin/stok-rapor")]
        public HttpResponseMessage RaporGetir()
        {
            parametreRepository = new ParametreRepository();
            var rapr = parametreRepository.StokkartRaporParametreleri();
            if (rapr != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rapr.Select(rap => new
                {
                    rap.parametre_id,
                    rap.parametre,
                    rap.parametre_adi,
                    rap.parametre_grubu,
                    rap.renk_rgb,
                    rap.sira,
                    rap.tanim,
                    rap.deger1,
                    rap.deger2,
                    rap.kayit_silindi,
                    rap.degistiren_tarih,
                    rap.degistiren_carikart_id,
                    rap.dil_1_tanim,
                    rap.dil_2_tanim,
                    rap.dil_3_tanim,
                    rap.dil_4_tanim,
                    rap.dil_5_tanim,
                    rap.kaynak_1_parametre_id,
                    rap.kaynak_2_parametre_id,
                    rap.kaynak_3_parametre_id,
                    rap.kaynak_4_parametre_id,
                    rap.kod,
                    rap.kod1,
                    rap.kod2,
                    rap.kod3,
                    rap.kod4,
                    rap.kod5,
                    rap.kod6,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// parametre_stokkart_raporların POST metod.
        /// parametre, parametre_grubu ve kayit_silindi boş geçilemez.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// <param name="rpn"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/stok-rapor")]
        [Route("api/admin/stok-rapor")]
        public HttpResponseMessage raporPost(parametre_stokkart_rapor rpn)
        {
            parametreRepository = new ParametreRepository();
            if (rpn != null)
            {
                rpn.degistiren_tarih = DateTime.Now;
                rpn.degistiren_carikart_id = Tools.PersonelId;
                var UretimRetVal = CrudRepository<parametre_stokkart_rapor>.Insert(rpn, "parametre_stokkart_rapor", new string[] { "parametre_id", "parametre_adi" });
                if (UretimRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, UretimRetVal.ErrorInfo.Message);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// parametre_stokkart_raporların PUT Metodu.
        /// </summary>
        /// /// <param name="rpn"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/stok-rapor")]
        [Route("api/admin/stok-rapor")]
        public HttpResponseMessage raporPut(parametre_stokkart_rapor rpn)
        {
            AcekaResult acekaResult = null;
            if (rpn != null)
            {
                if (rpn.parametre_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("parametre_id", rpn.parametre_id);
                    fields.Add("kayit_silindi", rpn.kayit_silindi);
                    fields.Add("parametre", rpn.parametre);
                    fields.Add("parametre_grubu", rpn.parametre_grubu);
                    fields.Add("kaynak_1_parametre_id", rpn.kaynak_1_parametre_id);
                    fields.Add("kaynak_2_parametre_id", rpn.kaynak_2_parametre_id);
                    fields.Add("kaynak_3_parametre_id", rpn.kaynak_3_parametre_id);
                    fields.Add("kaynak_4_parametre_id", rpn.kaynak_4_parametre_id);
                    fields.Add("kod", rpn.kod);
                    fields.Add("tanim", rpn.tanim);
                    fields.Add("dil_1_tanim", rpn.dil_1_tanim);
                    fields.Add("dil_2_tanim", rpn.dil_2_tanim);
                    fields.Add("dil_3_tanim", rpn.dil_3_tanim);
                    fields.Add("dil_4_tanim", rpn.dil_4_tanim);
                    fields.Add("dil_5_tanim", rpn.dil_5_tanim);
                    fields.Add("sira", rpn.sira);
                    fields.Add("renk_rgb", rpn.renk_rgb);
                    fields.Add("kod1", rpn.kod1);
                    fields.Add("kod2", rpn.kod2);
                    fields.Add("kod3", rpn.kod3);
                    fields.Add("kod4", rpn.kod4);
                    fields.Add("kod5", rpn.kod5);
                    fields.Add("kod6", rpn.kod6);
                    fields.Add("deger1", rpn.deger1);
                    fields.Add("deger2", rpn.deger2);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("degistiren_tarih", DateTime.Now);
                    acekaResult = CrudRepository.Update("parametre_stokkart_rapor", "parametre_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  parametre_stokkart_rapor DELETE Metodu
        /// </summary>
        /// <param name="rpn"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/stok-rapor")]
        [Route("api/admin/stok-rapor")]
        public HttpResponseMessage raporDelete(parametre_stokkart_rapor rpn)
        {
            if (rpn != null)
            {
                parametreRepository = new ParametreRepository();
                //var rap = parametreRepository.StokkartRaporParametreleri(rpn.parametre_id);
                if (rpn != null)
                {
                    parametre_stokkart_rapor rapp = new parametre_stokkart_rapor();
                    rapp.parametre_id = rpn.parametre_id;
                    rapp.kayit_silindi = rpn.kayit_silindi;
                    rapp.degistiren_carikart_id = Tools.PersonelId;
                    rapp.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_stokkart_rapor>.Update(rapp, "parametre_id", new string[] { "parametre", "parametre_grubu", "kaynak_1_parametre_id", "kaynak_2_parametre_id", "kaynak_3_parametre_id", "kaynak_4_parametre_id", "kod", "tanim", "dil_1_tanim", "dil_2_tanim", "dil_3_tanim", "dil_4_tanim", "dil_5_tanim", "sira", "renk_rgb", "kod1", "kod2", "kod3", "kod4", "kod5", "kod6", "deger1", "deger2" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = rapp.parametre_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion

        #region Parametre Beden Carikart
        /// <summary>
        /// parametre_beden_carikart listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "beden_id": 1,
        ///   "carikart_id": 100000000100,
        ///   "bedenkodu": 0,
        ///   "kayit_silindi": false,
        ///   "degistiren_tarih": "2017-04-27T14:22:23.6301432+03:00",
        ///   "degistiren_carikart_id": 100000000100,
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/beden-cari")]
        [Route("api/admin/beden-cari")]
        public HttpResponseMessage BedenCariGetir()
        {
            parametreRepository = new ParametreRepository();
            var bcari = parametreRepository.BedenCarikartListe();
            if (bcari != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bcari.Select(bdncar => new
                {
                    bdncar.bedenkodu,
                    bdncar.beden_id,
                    bdncar.carikart_id,
                    bdncar.kayit_silindi,
                    bdncar.degistiren_tarih,
                    bdncar.degistiren_carikart_id,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        /// <summary>
        /// parametre_beden_carikart POST Metodu. ÖNEMLİ!!:  beden_id ve carikart_id grubunun da unique index var.
        /// Her beden_id ve Her carikart_id den sadece bir kayıt olabilir. Aksi takdirde hata verecektir.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="bdncari"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/beden-cari")]
        [Route("api/admin/beden-cari")]
        public HttpResponseMessage BedenCariPost(parametre_beden_carikart bdncari)
        {
            parametreRepository = new ParametreRepository();
            if (bdncari != null)
            {
                bdncari.degistiren_tarih = DateTime.Now;
                bdncari.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_beden_carikart>.Insert(bdncari, "parametre_beden_carikart");
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        /// <summary>
        /// parametre_beden_carikart PUT Metodu.
        /// </summary>
        /// /// <param name="bdncari"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/beden-cari")]
        [Route("api/admin/beden-cari")]
        public HttpResponseMessage BedenCariPut(parametre_beden_carikart bdncari)
        {
            AcekaResult acekaResult = null;
            if (bdncari.beden_id > 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("sira_id", bdncari.sira_id);
                fields.Add("beden_id", bdncari.beden_id);
                fields.Add("carikart_id", bdncari.carikart_id);
                fields.Add("kayit_silindi", bdncari.kayit_silindi);
                fields.Add("bedenkodu", bdncari.bedenkodu);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_beden_carikart", "sira_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        ///  parametre_beden_carikart DELETE Metodu. Sadece sira_id gönderilebilir.
        /// </summary>
        /// <param name="bdncari"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/beden-cari")]
        [Route("api/admin/beden-cari")]
        public HttpResponseMessage BedenCariDelete(parametre_beden_carikart bdncari)
        {
            if (bdncari != null)
            {
                parametreRepository = new ParametreRepository();
                //var birimListe = parametreRepository.BedenleriGetir(bdncari.sira_id);
                if (bdncari != null)
                {
                    parametre_beden_carikart bedn = new parametre_beden_carikart();
                    bedn.sira_id = bdncari.sira_id;
                    bedn.kayit_silindi = bdncari.kayit_silindi;
                    bedn.degistiren_carikart_id = Tools.PersonelId;
                    bedn.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_beden_carikart>.Update(bedn, "sira_id", new string[] { "beden_id", "carikart_id", "bedenkodu" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = bedn.beden_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion

        #region Parametre Kdv
        /// <summary>
        /// Kdv listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///  "kod": 8,
        ///  "oran": 8,
        ///  "tarih": "2010-01-01T00:00:00",
        ///  "giz_yazilim_kodu": null,
        ///  "degistiren_tarih": "2015-10-26T11:58:33",
        ///  "degistiren_carikart_id": 0
        ///},
        ///{
        ///  "kod": 18,
        ///  "oran": 18,
        ///  "tarih": "2010-01-01T00:00:00",
        ///  "giz_yazilim_kodu": null,
        ///  "degistiren_tarih": "2015-10-26T11:58:33",
        ///  "degistiren_carikart_id": 0
        ///}
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/kdv")]
        [Route("api/admin/kdv")]
        public HttpResponseMessage KdvGetir()
        {
            parametreRepository = new ParametreRepository();
            var kdvList = parametreRepository.KDVler();
            if (kdvList != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, kdvList.Select(klist => new
                {
                    klist.kod,
                    klist.oran,
                    klist.tarih,
                    //klist.giz_yazilim_kodu,
                    klist.degistiren_tarih,
                    klist.degistiren_carikart_id,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// KDV POST Metodu. ÖNEMLİ!!:  kod ve kod alanları unique index.
        /// Her kod ve Her kod den sadece bir kayıt olabilir. Aksi takdirde hata verecektir.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="kdv"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/kdv")]
        [Route("api/admin/kdv-post")]
        public HttpResponseMessage kdvPost(parametre_kdv kdv)
        {
            parametreRepository = new ParametreRepository();
            if (kdv != null)
            {
                kdv.degistiren_tarih = DateTime.Now;
                kdv.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_kdv>.Insert(kdv, "parametre_kdv");
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        /// <summary>
        /// KDV PUT Metodu.
        /// </summary>
        /// /// <param name="kdv"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/kdv")]
        [Route("api/admin/kdv-put")]
        public HttpResponseMessage KdvPut(parametre_kdv kdv)
        {
            AcekaResult acekaResult = null;
            if (kdv.kod > 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("kod", kdv.kod);
                fields.Add("oran", kdv.oran);
                fields.Add("tarih", kdv.tarih);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_kdv", "kod", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        /// Silme işlemi olmamalı. Kdv DELETE Metodu. 
        /// </summary>
        /// <param name="kdv"></param>
        /// <returns></returns>
        //[HttpDelete]
        //[CustAuthFilter(ApiUrl = "api/admin/kdv")]
        //[Route("api/admin/kdv-delete")]
        //public HttpResponseMessage kdvDelete(parametre_kdv kdv)
        //{
        //    if (kdv != null)
        //    {
        //        parametreRepository = new ParametreRepository();
        //        //var kdvListe = parametreRepository.KDVler(kdv.kod);
        //        if (kdvListe != null)
        //        {
        //            parametre_kdv pkdv = new parametre_kdv();
        //            pkdv.kod = kdv.kod;
        //            //pkdv.oran = kdv.oran;
        //            //pkdv.tarih = kdv.tarih;
        //            //pkdv.degistiren_carikart_id = Tools.PersonelId;
        //            //pkdv.degistiren_tarih = DateTime.Now;
        //            CrudRepository<parametre_kdv>.Delete(pkdv, "parametre_kdv", new string[] { "kod" }, new string[] { "kod" });

        //            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = pkdv.kod.ToString() });
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
        //    }
        //}


        #endregion

        #region Parametre Parabirimi
        /// <summary>
        /// Para Birimleri listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///  "pb": "0",
        ///  "pb_adi": "Türk Lirası",
        ///  "pb_kodu": null,
        ///  "kayit_silindi": false,
        ///  "kusurat_tanimi": "Krş",
        ///  "merkezbankasi_kodu": "TRY",
        ///  "sira": null,
        ///  "pr_kodu": null,
        ///  "ulke_id": null,
        ///  "degistiren_tarih": "0001-01-01T00:00:00",
        ///  "degistiren_carikart_id": 0
        ///},
        ///{
        ///  "pb": "1",
        ///  "pb_adi": "Amerikan Doları",
        ///  "pb_kodu": null,
        ///  "kayit_silindi": false,
        ///  "kusurat_tanimi": "Cent",
        ///  "merkezbankasi_kodu": "USD",
        ///  "sira": null,
        ///  "pr_kodu": null,
        ///  "ulke_id": null,
        ///  "degistiren_tarih": "0001-01-01T00:00:00",
        ///  "degistiren_carikart_id": 0
        ///}
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/para-birimi")]
        [Route("api/admin/para-birimi")]
        public HttpResponseMessage ParaBirimiGetir()
        {
            parametreRepository = new ParametreRepository();
            var pbirim = parametreRepository.ParaBirimleri();
            if (pbirim != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, pbirim.Select(brm => new
                {
                    brm.pb,
                    brm.pb_adi,
                    brm.pb_kodu,
                    brm.kayit_silindi,
                    brm.kusurat_tanimi,
                    brm.merkezbankasi_kodu,
                    brm.sira,
                    brm.pr_kodu,
                    brm.ulke_id,
                    brm.degistiren_tarih,
                    brm.degistiren_carikart_id,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        /// <summary>
        /// Para Birimleri POST Metodu. Pb kodu,pb adı boş geçilemez.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="prb"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/para-birimi")]
        [Route("api/admin/para-birimi-post")]
        public HttpResponseMessage paraBirimiPost(parametre_parabirimi prb)
        {
            parametreRepository = new ParametreRepository();
            if (prb != null)
            {
                prb.degistiren_tarih = DateTime.Now;
                prb.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_parabirimi>.Insert(prb, "parametre_parabirimi", new string[] { "pb" });
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        /// <summary>
        /// Para Birimleri PUT Metodu. Pb adi ve pb kodu bpş geçilemez.
        /// </summary>
        /// /// <param name="prb"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/para-birimi")]
        [Route("api/admin/para-birimi-put")]
        public HttpResponseMessage paraBirimiPut(parametre_parabirimi prb)
        {
            AcekaResult acekaResult = null;
            if (prb.pb >= 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("pb", prb.pb);
                fields.Add("kayit_silindi", prb.kayit_silindi);
                fields.Add("pb_kodu", prb.pb_kodu);
                fields.Add("pb_adi", prb.pb_adi);
                fields.Add("ulke_id", prb.ulke_id);
                fields.Add("merkezbankasi_kodu", prb.merkezbankasi_kodu);
                fields.Add("sira", prb.sira);
                fields.Add("kusurat_tanimi", prb.kusurat_tanimi);
                fields.Add("pr_kodu", prb.pr_kodu);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_parabirimi", "pb", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        ///  Para Birimleri DELETE Metodu. Sadece pb gönderilebilir.
        /// </summary>
        /// <param name="pbirim"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/para-birimi")]
        [Route("api/admin/para-birimi-delete")]
        public HttpResponseMessage paraBirimiDelete(parametre_parabirimi pbirim)
        {
            if (pbirim != null)
            {
                parametreRepository = new ParametreRepository();
                //var birimListe = parametreRepository.ParaBirimleri(pbirim.pb);
                if (pbirim != null)
                {
                    parametre_parabirimi brm = new parametre_parabirimi();
                    brm.kayit_silindi = true; //pbirim.kayit_silindi;
                    brm.pb = pbirim.pb;
                    brm.degistiren_carikart_id = Tools.PersonelId;
                    brm.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_parabirimi>.Update(brm, "pb", new string[] { "pb_kodu", "pb_adi", "ulke_id", "merkezbankasi_kodu", "sira", "kusurat_tanimi", "pr_kodu" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = brm.pb.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }



        #endregion

        #region Parametre Sipariş Türü
        /// <summary>
        /// Parametre Sipariş Türleri listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "genel": true,
        ///   "kalite2": false,
        ///   "kayit_silindi": false,
        ///   "numune": false,
        ///   "siparisturu_id": 1,
        ///   "siparisturu_tanim": "Genel-Üretim",
        ///   "sira": 1,
        ///   "varsayilan": true,
        ///   "degistiren_tarih": "2016-11-29T07:28:46",
        ///   "degistiren_carikart_id": 100000000100
        /// },
        /// {
        ///   "genel": false,
        ///   "kalite2": false,
        ///   "kayit_silindi": false,
        ///   "numune": false,
        ///   "siparisturu_id": 4,
        ///   "siparisturu_tanim": "Tamir",
        ///   "sira": 2,
        ///   "varsayilan": false,
        ///   "degistiren_tarih": "2017-03-31T14:22:02",
        ///   "degistiren_carikart_id": 100000000100
        /// }
        /// ]
        /// </returns>
        /// ttpGet]
        [CustAuthFilter(ApiUrl = "api/admin/siparis-turleri")]
        [Route("api/admin/siparis-turleri")]
        [HttpGet]
        public HttpResponseMessage SipTurGetir()
        {
            parametreRepository = new ParametreRepository();
            var sturu = parametreRepository.SiparisTuruGetir();
            if (sturu != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sturu.Select(str => new
                {
                    str.genel,
                    str.kalite2,
                    str.kayit_silindi,
                    str.numune,
                    str.siparisturu_id,
                    str.siparisturu_tanim,
                    str.sira,
                    str.varsayilan,
                    str.degistiren_tarih,
                    str.degistiren_carikart_id,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        /// <summary>
        /// Parametre Sipariş Türleri POST Metodu. Pb kodu,pb adı boş geçilemez.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/siparis-turleri")]
        [Route("api/admin/siparis-turleri-post")]
        public HttpResponseMessage SipTurPost(parametre_siparisturu str)
        {
            parametreRepository = new ParametreRepository();
            if (str != null)
            {
                str.degistiren_tarih = DateTime.Now;
                str.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_siparisturu>.Insert(str, "parametre_siparisturu", new string[] { "siparisturu_id" });
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        /// <summary>
        /// Parametre Sipariş Türleri  PUT Metodu. Pb adi ve pb kodu bpş geçilemez.
        /// </summary>
        /// /// <param name="str"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/siparis-turleri")]
        [Route("api/admin/siparis-turleri-put")]
        public HttpResponseMessage SipTurPut(parametre_siparisturu str)
        {
            AcekaResult acekaResult = null;
            if (str.siparisturu_id >= 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("siparisturu_id", str.siparisturu_id);
                fields.Add("kayit_silindi", str.kayit_silindi);
                fields.Add("siparisturu_tanim", str.siparisturu_tanim);
                fields.Add("varsayilan", str.varsayilan);
                fields.Add("sira", str.sira);
                fields.Add("genel", str.genel);
                fields.Add("kalite2", str.kalite2);
                fields.Add("numune", str.numune);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_siparisturu", "siparisturu_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        ///  Parametre Sipariş Türleri  DELETE Metodu. Sadece pb gönderilebilir.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/siparis-turleri")]
        [Route("api/admin/siparis-turleri-delete")]
        public HttpResponseMessage SipTurDelete(parametre_siparisturu str)
        {
            if (str != null)
            {
                parametreRepository = new ParametreRepository();
                // var sturListe = parametreRepository.SiparisTuruGetir(str.siparisturu_id);
                if (str != null)
                {
                    parametre_siparisturu stur = new parametre_siparisturu();
                    stur.kayit_silindi = str.kayit_silindi;
                    stur.siparisturu_id = str.siparisturu_id;
                    stur.degistiren_carikart_id = Tools.PersonelId;
                    stur.degistiren_tarih = DateTime.Now;
                    CrudRepository<parametre_siparisturu>.Update(stur, "siparisturu_id", new string[] { "siparisturu_tanim", "varsayilan", "sira", "genel", "kalite2", "numune" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = stur.siparisturu_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion

        #region  Parametre Carikart Rapor
        /// <summary>
        /// Parametre Carikart Rapor listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///   "parametre_id": 1,
        ///   "degistiren_carikart_id": 0,
        ///   "degistiren_tarih": "0001-01-01T00:00:00",
        ///   "kayit_silindi": false,
        ///   "parametre": 1,
        ///   "kaynak_1_parametre_id": 0,
        ///   "kaynak_2_parametre_id": 0,
        ///   "kaynak_3_parametre_id": 0,
        ///   "kaynak_4_parametre_id": 0,
        ///   "kod": null,
        ///   "tanim": "Ana Grup 1",
        ///   "grup1": null,
        ///   "grup2": null,
        ///   "sira": null,
        ///   "parametre_grubu": 0,
        ///   "dil_1_tanim": null,
        ///   "dil_2_tanim": null,
        ///   "dil_3_tanim": null,
        ///   "dil_4_tanim": null,
        ///   "dil_5_tanim": null
        /// }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/cari-raporlar")]
        [Route("api/admin/cari-raporlar")]
        [HttpGet]
        public HttpResponseMessage CariRaporGetir()
        {
            parametreRepository = new ParametreRepository();
            var crapor = parametreRepository.cariraporListe();
            if (crapor != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, crapor.Select(cr => new
                {
                    cr.parametre_id,
                    cr.degistiren_carikart_id,
                    cr.degistiren_tarih,
                    cr.kayit_silindi,
                    cr.parametre,
                    cr.kaynak_1_parametre_id,
                    cr.kaynak_2_parametre_id,
                    cr.kaynak_3_parametre_id,
                    cr.kaynak_4_parametre_id,
                    cr.kod,
                    cr.tanim,
                    cr.grup1,
                    cr.grup2,
                    cr.sira,
                    cr.parametre_grubu,
                    cr.dil_1_tanim,
                    cr.dil_2_tanim,
                    cr.dil_3_tanim,
                    cr.dil_4_tanim,
                    cr.dil_5_tanim,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        /// <summary>
        /// Parametre Carikart Rapor POST Metodu. 
        /// kod,parametre,tanim,parametre_grubu alanları boş geçilemez.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="cr"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/cari-raporlar")]
        [Route("api/admin/cari-raporlar-post")]
        public HttpResponseMessage CariRaporPost(parametre_carikart_rapor cr)
        {
            parametreRepository = new ParametreRepository();
            if (cr != null)
            {
                cr.degistiren_tarih = DateTime.Now;
                cr.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_carikart_rapor>.Insert(cr, "parametre_carikart_rapor", new string[] { "siparisturu_id" });
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Parametre Carikart Rapor PUT Metodu. 
        /// </summary>
        /// /// <param name="cr"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/cari-raporlar")]
        [Route("api/admin/cari-raporlar-put")]
        public HttpResponseMessage CariRaporPut(parametre_carikart_rapor cr)
        {
            AcekaResult acekaResult = null;
            if (cr.parametre_id >= 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("parametre_id", cr.parametre_id);
                fields.Add("kayit_silindi", cr.kayit_silindi);
                fields.Add("parametre", cr.parametre);
                fields.Add("kaynak_1_parametre_id", cr.kaynak_1_parametre_id);
                fields.Add("kaynak_2_parametre_id", cr.kaynak_2_parametre_id);
                fields.Add("kaynak_3_parametre_id", cr.kaynak_3_parametre_id);
                fields.Add("kaynak_4_parametre_id", cr.kaynak_4_parametre_id);
                fields.Add("kod", cr.kod);
                fields.Add("tanim", cr.tanim);
                fields.Add("grup1", cr.grup1);
                fields.Add("grup2", cr.grup2);
                fields.Add("sira", cr.sira);
                fields.Add("parametre_grubu", cr.parametre_grubu);
                fields.Add("dil_1_tanim", cr.dil_1_tanim);
                fields.Add("dil_2_tanim", cr.dil_2_tanim);
                fields.Add("dil_3_tanim", cr.dil_3_tanim);
                fields.Add("dil_4_tanim", cr.dil_4_tanim);
                fields.Add("dil_5_tanim", cr.dil_5_tanim);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_carikart_rapor", "parametre_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        /// Parametre Carikart Rapor DELETE Metodu. Sadece pb gönderilebilir.
        /// </summary>
        /// <param name="cr"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/cari-raporlar")]
        [Route("api/admin/cari-raporlar-delete")]
        public HttpResponseMessage CariRaporDelete(parametre_carikart_rapor cr)
        {
            if (cr != null)
            {
                parametreRepository = new ParametreRepository();
                //var sturListe = parametreRepository.CariRaporGetir(cr.parametre_id);
                if (cr != null)
                {
                    parametre_carikart_rapor stur = new parametre_carikart_rapor();
                    stur.kayit_silindi = cr.kayit_silindi;
                    stur.parametre_id = cr.parametre_id;
                    stur.degistiren_carikart_id = Tools.PersonelId;
                    stur.degistiren_tarih = DateTime.Now;

                    CrudRepository<parametre_carikart_rapor>.Update(stur, "parametre_id");

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = stur.parametre_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Üretimyeri Carikart
        /// <summary>
        /// Parametre Üretmyeri Carikart listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///   "carikart_id": 100000000001,
        ///   "degistiren_carikart_id": 0,
        ///   "degistiren_tarih": "2009-02-27T18:41:11",
        ///   "kod1": "GEO",
        ///   "kod2": "Georgia",
        ///   "kod3": "MLT847",
        ///   "made_in": "Made in Georgia",
        ///   "oncelik_sira": 0,
        ///   "uretimyeri_id": 104,
        ///   "varsayilan": false
        /// }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-uretim-carikart")]
        [Route("api/admin/parametre-uretim-carikart")]
        [HttpGet]
        public HttpResponseMessage UretimCarikartGetir()
        {
            parametreRepository = new ParametreRepository();
            var cruyer = parametreRepository.UretimyeriCarikarListe();
            if (cruyer != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cruyer.Select(uy => new
                {
                    uy.carikart_id,
                    uy.degistiren_carikart_id,
                    uy.degistiren_tarih,
                    uy.kod1,
                    uy.kod2,
                    uy.kod3,
                    uy.made_in,
                    uy.oncelik_sira,
                    uy.uretimyeri_id,
                    uy.varsayilan,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Üretimyeri Carikart POST Metodu. 
        /// uretimyeri_id, carikart_id alanları boş geçilemez. Unique alanlar.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// <param name="uycr"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/admin/parametre-uretim-carikart")]
        [Route("api/admin/parametre-uretim-carikart-post")]
        public HttpResponseMessage UretimCarikartPost(parametre_uretimyeri_carikart uycr)
        {
            parametreRepository = new ParametreRepository();
            if (uycr != null)
            {
                uycr.degistiren_tarih = DateTime.Now;
                uycr.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_uretimyeri_carikart>.Insert(uycr, "parametre_uretimyeri_carikart");
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Parametre Üretimyeri Carikart PUT Metodu. 
        /// uretimyeri_id, carikart_id alanları boş geçilemez. Unique alanlar.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// <param name="uycr"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/parametre-uretim-carikart")]
        [Route("api/admin/parametre-uretim-carikart-put")]
        public HttpResponseMessage UretimCarikartPut(parametre_uretimyeri_carikart uycr)
        {
            AcekaResult acekaResult = null;
            if (uycr.uretimyeri_id > 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("uretimyeri_id", uycr.uretimyeri_id);
                fields.Add("carikart_id", uycr.carikart_id);
                fields.Add("oncelik_sira", uycr.oncelik_sira);
                fields.Add("varsayilan", uycr.varsayilan);
                fields.Add("made_in", uycr.made_in);
                fields.Add("kod1", uycr.kod1);
                fields.Add("kod2", uycr.kod2);
                fields.Add("kod3", uycr.kod3);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);

                acekaResult = CrudRepository.Update("parametre_uretimyeri_carikart", "uretimyeri_id = " + uycr.uretimyeri_id + " AND carikart_id = " + uycr.carikart_id, fields, true);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        /// Parametre Üretimyeri Carikart PUT Metodu. 
        /// uretimyeri_id, carikart_id alanları boş geçilemez. Unique alanlar.
        /// degistiren_tarih ve degistiren_carikart_id boş gönderilecek.
        /// </summary>
        /// /// <param name="uycr"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/parametre-uretim-carikart")]
        [Route("api/admin/parametre-uretim-carikart-delete")]
        public HttpResponseMessage UretimCarikartDelete(parametre_uretimyeri_carikart uycr)
        {
            if (uycr != null)
            {
                parametreRepository = new ParametreRepository();
                parametre_uretimyeri_carikart stur = new parametre_uretimyeri_carikart();
                stur.uretimyeri_id = uycr.uretimyeri_id;
                stur.carikart_id = uycr.carikart_id;
                stur.degistiren_carikart_id = Tools.PersonelId;
                stur.degistiren_tarih = DateTime.Now;

                CrudRepository<parametre_uretimyeri_carikart>.Delete(stur, "parametre_uretimyeri_carikart", new string[] { "uretimyeri_id ", "carikart_id" }, new string[] { "uretimyeri_id", "carikart_id" });
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = stur.uretimyeri_id.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region parametre_kalite2_tur
        /// <summary>
        /// Parametre Kalite2 listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "kalite2tur_id": 1,
        ///   "degistiren_carikart_id": -1,
        ///   "degistiren_tarih": "2017-05-02T00:00:00",
        ///   "kayit_silindi": false,
        ///   "tanim": "Tanuımsızzzzz",
        ///   "sira": 1,
        ///   "numune": 0
        /// }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-kalite")]
        [Route("api/admin/parametre-kalite")]
        [HttpGet]
        public HttpResponseMessage ParametreKalite2Getir()
        {
            parametreRepository = new ParametreRepository();
            var cruyer = parametreRepository.KaliteTurListe();
            if (cruyer != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cruyer.Select(uy => new
                {
                    uy.kalite2tur_id,
                    uy.degistiren_carikart_id,
                    uy.degistiren_tarih,
                    uy.kayit_silindi,
                    uy.tanim,
                    uy.sira,
                    uy.numune,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Kalite2 POST Metodu. tanım alanı 50 karakter 'ı geçemez.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "kalite2tur_id": 1,
        ///   "degistiren_carikart_id": -1,
        ///   "degistiren_tarih": "2017-05-02T00:00:00",
        ///   "kayit_silindi": false,
        ///   "tanim": "Tanuımsızzzzz",
        ///   "sira": 1,
        ///   "numune": 0
        /// }
        /// ]
        /// </returns>
        /// <param name="kalitetur"></param>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-kalite")]
        [Route("api/admin/parametre-kalite-post")]
        [HttpPost]
        public HttpResponseMessage ParametreKalite2Post(parametre_kalite2_tur kalitetur)
        {
            parametreRepository = new ParametreRepository();
            if (kalitetur != null)
            {
                kalitetur.degistiren_tarih = DateTime.Now;
                kalitetur.degistiren_carikart_id = Tools.PersonelId;
                var BedencariRetVal = CrudRepository<parametre_kalite2_tur>.Insert(kalitetur, "parametre_kalite2_tur", new string[] { "kalite2tur_id" });
                if (BedencariRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, BedencariRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Parametre Kalite2 PUT Metodu. tanım alanı 50 karakter 'ı geçemez.
        /// </summary>
        /// <param name="kalitetur"></param>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/admin/parametre-kalite")]
        [Route("api/admin/parametre-kalite-put")]
        public HttpResponseMessage ParametreKalite2Put(parametre_kalite2_tur kalitetur)
        {
            AcekaResult acekaResult = null;
            if (kalitetur.kalite2tur_id > 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("kalite2tur_id", kalitetur.kalite2tur_id);
                fields.Add("kayit_silindi", kalitetur.kayit_silindi);
                fields.Add("tanim", kalitetur.tanim);
                fields.Add("sira", kalitetur.sira);
                fields.Add("numune", kalitetur.numune);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);

                acekaResult = CrudRepository.Update("parametre_kalite2_tur", "kalite2tur_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        /// Parametre Kalite2 DELETE Metodu.
        /// </summary>
        /// <param name="kalitetur"></param>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/admin/parametre-kalite")]
        [Route("api/admin/parametre-kalite-delete")]
        public HttpResponseMessage ParametreKalite2Delete(parametre_kalite2_tur kalitetur)
        {
            if (kalitetur != null)
            {
                parametre_kalite2_tur ktur = new parametre_kalite2_tur();
                ktur.kalite2tur_id = kalitetur.kalite2tur_id;
                ktur.degistiren_carikart_id = Tools.PersonelId;
                ktur.kayit_silindi = kalitetur.kayit_silindi;
                ktur.degistiren_tarih = DateTime.Now;
                CrudRepository<parametre_kalite2_tur>.Update(ktur, "kalite2tur_id", new string[] { "degistiren_carikart_id", "degistiren_tarih", "tanim", "sira", "numune" });
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = ktur.kalite2tur_id.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion
        #region Parametre_renk
        /// <summary>
        /// Renklerin listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "renk_adi": "%60 puma royal-puma royal-white",
        ///   "degistiren_carikart_id": -1,
        ///   "degistiren_tarih": "2017-02-10T17:43:56",
        ///   "kayit_silindi": false,
        ///   "renk_id": 60635,
        ///   "renk_kodu": "",
        ///   "renk_kodu2": "",
        ///   "renk_rgb": null,
        ///   "siparis_ozel": false,
        ///   "stokalan_id_1": 0,
        ///   "stokkart_parametre_grubu": null,
        ///   "stokkart_tipi_id": 0
        /// }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-renk")]
        [Route("api/admin/parametre-renk")]
        [HttpGet]
        public HttpResponseMessage ParametreRenkGetir()
        {
            parametreRepository = new ParametreRepository();
            var rnkler = parametreRepository.RenkListesi();
            if (rnkler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, rnkler.Select(renkler => new
                {
                    renkler.renk_adi,
                    renkler.degistiren_carikart_id,
                    renkler.degistiren_tarih,
                    renkler.kayit_silindi,
                    renkler.renk_id,
                    renkler.renk_kodu,
                    renkler.renk_kodu2,
                    renkler.renk_rgb,
                    renkler.siparis_ozel,
                    renkler.stokalan_id_1,
                    renkler.stokkart_parametre_grubu,
                    renkler.stokkart_tipi_id
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Parametre Renkler POST metod.
        /// </summary>
        /// <returns>
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-renk")]
        [Route("api/admin/parametre-renk-post")]
        [HttpPost]
        public HttpResponseMessage ParametreRenkPost(parametre_renk rnk)
        {
            AcekaResult acekaResult = null;

            parametreRepository = new ParametreRepository();
            if (rnk != null)
            {
                rnk.degistiren_tarih = DateTime.Now;
                rnk.degistiren_carikart_id = Tools.PersonelId;
                var RenkRetVal = CrudRepository<parametre_renk>.Insert(rnk, "parametre_renk", new string[] { "renk_id", "stokkart_parametre_grubu" });
                if (RenkRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, RenkRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful - renk_id =" + RenkRetVal.RetVal.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Parametre Renkler PUT metod.
        /// </summary>
        /// <returns>
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-renk")]
        [Route("api/admin/parametre-renk-put")]
        [HttpPut]
        public HttpResponseMessage ParametreRenkPut(parametre_renk rnk)
        {
            AcekaResult acekaResult = null;
            if (rnk.renk_id >= 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("renk_id", rnk.renk_id);
                fields.Add("kayit_silindi", rnk.kayit_silindi);
                fields.Add("renk_kodu", rnk.renk_kodu);
                fields.Add("renk_adi", rnk.renk_adi);
                fields.Add("renk_rgb", rnk.renk_rgb);
                fields.Add("renk_kodu2", rnk.renk_kodu2);
                fields.Add("siparis_ozel", rnk.siparis_ozel);
                fields.Add("stokkart_tipi_id", rnk.stokkart_tipi_id);
                fields.Add("stokalan_id_1", rnk.stokalan_id_1);
                fields.Add("renk_kodu_dil_1", rnk.renk_kodu_dil_1);
                fields.Add("renk_adi_dil_1", rnk.renk_adi_dil_1);
                fields.Add("renk_kodu_dil_2", rnk.renk_kodu_dil_2);
                fields.Add("renk_adi_dil_2", rnk.renk_adi_dil_2);
                fields.Add("renk_kodu_dil_3", rnk.renk_kodu_dil_3);
                fields.Add("renk_adi_dil_3", rnk.renk_adi_dil_3);
                fields.Add("renk_kodu_dil_4", rnk.renk_kodu_dil_4);
                fields.Add("renk_adi_dil_4", rnk.renk_adi_dil_4);
                fields.Add("renk_kodu_dil_5", rnk.renk_kodu_dil_5);
                fields.Add("renk_adi_dil_5", rnk.renk_adi_dil_5);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_renk", "renk_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        }

        /// <summary>
        /// Parametre Renkler PUT metod.
        /// </summary>
        /// <returns>
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-renk")]
        [Route("api/admin/parametre-renk-delete")]
        [HttpDelete]
        public HttpResponseMessage ParametreRenkDelete(parametre_renk rnk)
        {
            if (rnk != null)
            {
                parametreRepository = new ParametreRepository();
                //var sturListe = parametreRepository.RenkListesi(rnk.renk_id);
                if (rnk != null)
                {
                    parametre_renk prenk = new parametre_renk();
                    prenk.kayit_silindi = rnk.kayit_silindi;
                    prenk.renk_id = rnk.renk_id;
                    prenk.degistiren_carikart_id = Tools.PersonelId;
                    prenk.degistiren_tarih = DateTime.Now;

                    CrudRepository<parametre_renk>.Update(prenk, "renk_id", new string[] { "stokkart_parametre_grubu","degistiren_carikart_id","degistiren_tarih","renk_kodu","renk_adi","renk_rgb","renk_kodu2","siparis_ozel",
"stokkart_tipi_id","stokalan_id_1","renk_kodu_dil_1","renk_adi_dil_1","renk_kodu_dil_2","renk_adi_dil_2","renk_kodu_dil_3","renk_adi_dil_3","renk_kodu_dil_4","renk_adi_dil_4","renk_kodu_dil_5","renk_adi_dil_5"
 });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = prenk.renk_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }


        #endregion

        #region Parametre Ülke
        /// <summary>
        /// Ülkelerin listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///    "ulke_id": 1,
        ///    "ulke_adi": "ABD",
        ///    "kayit_silindi": false,
        ///    "sira": null,
        ///    "statu": false,
        ///    "ulke_plaka_kodu": "USA",
        ///    "ulke_telefon_kodu": "1",
        ///    "ulke_tipi": null,
        ///    "ulke_adi_dil_1": "USA",
        ///    "ulke_adi_dil_2": null,
        ///    "ulke_adi_dil_3": null,
        ///    "ulke_adi_dil_4": null,
        ///    "ulke_adi_dil_5": null
        ///  }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-ulke")]
        [Route("api/admin/parametre-ulke")]
        [HttpGet]
        public HttpResponseMessage ParametreUlkeGetir()
        {
            parametreRepository = new ParametreRepository();
            var ulkeler = parametreRepository.UlkeleriGetir();
            if (ulkeler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ulkeler.Select(ulk => new
                {
                    ulk.ulke_id,
                    ulk.ulke_adi,
                    ulk.kayit_silindi,
                    ulk.sira,
                    ulk.statu,
                    ulk.ulke_plaka_kodu,
                    ulk.ulke_telefon_kodu,
                    ulk.ulke_tipi,
                    ulk.ulke_adi_dil_1,
                    ulk.ulke_adi_dil_2,
                    ulk.ulke_adi_dil_3,
                    ulk.ulke_adi_dil_4,
                    ulk.ulke_adi_dil_5
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Ülkeler POST metod.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="ulk"></param>
        [CustAuthFilter(ApiUrl = "api/admin/ulke")]
        [Route("api/admin/parametre-ulke-post")]
        [HttpPost]
        public HttpResponseMessage ParametreUlkePost(parametre_ulke ulk)
        {
            parametreRepository = new ParametreRepository();
            if (ulk != null)
            {
                ulk.degistiren_tarih = DateTime.Now;
                ulk.degistiren_carikart_id = Tools.PersonelId;
                var RenkRetVal = CrudRepository<parametre_ulke>.Insert(ulk, "parametre_ulke", new string[] { "ulke_id" });
                if (RenkRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, RenkRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful - renk_id =" + RenkRetVal.RetVal.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Ülkeler PUT metod.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="ulk"></param>
        [CustAuthFilter(ApiUrl = "api/admin/ulke")]
        [Route("api/admin/parametre-ulke-put")]
        [HttpPut]
        public HttpResponseMessage ParametreUlkePut(parametre_ulke ulk)
        {
            AcekaResult acekaResult = null;
            parametreRepository = new ParametreRepository();
            if (ulk != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("ulke_id", ulk.ulke_id);
                fields.Add("kayit_silindi", ulk.kayit_silindi);
                fields.Add("ulke_adi", ulk.ulke_adi);
                fields.Add("statu", ulk.statu);
                fields.Add("ulke_dunya_kodu", ulk.ulke_dunya_kodu);
                fields.Add("ulke_plaka_kodu", ulk.ulke_plaka_kodu);
                fields.Add("ulke_telefon_kodu", ulk.ulke_telefon_kodu);
                fields.Add("ulke_tipi", ulk.ulke_tipi);
                fields.Add("ulke_adi_dil_1", ulk.ulke_adi_dil_1);
                fields.Add("ulke_adi_dil_2", ulk.ulke_adi_dil_2);
                fields.Add("ulke_adi_dil_3", ulk.ulke_adi_dil_3);
                fields.Add("ulke_adi_dil_4", ulk.ulke_adi_dil_4);
                fields.Add("ulke_adi_dil_5", ulk.ulke_adi_dil_5);
                fields.Add("sira", ulk.sira);
                fields.Add("degistiren_carikart_id", Tools.PersonelId);
                fields.Add("degistiren_tarih", DateTime.Now);
                acekaResult = CrudRepository.Update("parametre_ulke", "ulke_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful - ulke_id =" + acekaResult.RetVal.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Ülkeler DELETE metod.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="ulk"></param>
        [CustAuthFilter(ApiUrl = "api/admin/ulke")]
        [Route("api/admin/parametre-ulke-delete")]
        [HttpDelete]
        public HttpResponseMessage ParametreUlkeDelete(parametre_ulke ulk)
        {
            if (ulk != null)
            {
                parametreRepository = new ParametreRepository();
                if (ulk != null)
                {
                    parametre_ulke prulke = new parametre_ulke();
                    prulke.kayit_silindi = ulk.kayit_silindi;
                    prulke.degistiren_carikart_id = Tools.PersonelId;
                    prulke.degistiren_tarih = DateTime.Now;

                    CrudRepository<parametre_ulke>.Update(ulk, "ulke_id", new string[] {"degistiren_carikart_id","degistiren_tarih","ulke_adi","statu","ulke_dunya_kodu","ulke_plaka_kodu",
"ulke_telefon_kodu","ulke_tipi","ulke_adi_dil_1","ulke_adi_dil_2","ulke_adi_dil_3","ulke_adi_dil_4","ulke_adi_dil_5","sira" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = prulke.ulke_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Parametre Ülke Şehir
        /// <summary>
        /// Ülkeye ait Sehirlerin listesini veren metod.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///   "sehir_id": 83,
        ///   "kayit_silindi": 0,
        ///   "ulke_id": 90,
        ///   "sehir_adi": "",
        ///   "sehir_dunya_kodu": "",
        ///   "sehir_telefon_kodu": "(___)",
        ///   "sehir_plaka_kodu": "00",
        ///   "sehir_adi_dil_1": null,
        ///   "sehir_adi_dil_2": null,
        ///   "sehir_adi_dil_3": null,
        ///   "sehir_adi_dil_4": null,
        ///   "sehir_adi_dil_5": null,
        ///   "sira": null,
        ///   "ups_id": null,
        ///   "degistiren_carikart_id": 0,
        ///   "degistiren_tarih": "0001-01-01T00:00:00"
        /// }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-sehir")]
        [Route("api/admin/parametre-sehir/{ulke_id}")]
        [HttpGet]
        public HttpResponseMessage ParametreSehirGetir(short ulke_id)
        {
            parametreRepository = new ParametreRepository();
            var sehir = parametreRepository.SehirleriGetir(ulke_id);
            if (sehir != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sehir.Select(seh => new
                {
                    seh.sehir_id,
                    seh.kayit_silindi,
                    seh.ulke_id,
                    seh.sehir_adi,
                    seh.sehir_dunya_kodu,
                    seh.sehir_telefon_kodu,
                    seh.sehir_plaka_kodu,
                    seh.sehir_adi_dil_1,
                    seh.sehir_adi_dil_2,
                    seh.sehir_adi_dil_3,
                    seh.sehir_adi_dil_4,
                    seh.sehir_adi_dil_5,
                    seh.sira,
                    seh.ups_id,
                    seh.degistiren_carikart_id,
                    seh.degistiren_tarih
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Şehirlere ait POST metodu.
        /// sehir_id nin neden identity si kapalı?
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="sehir"></param>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-sehir")]
        [Route("api/admin/parametre-sehir-post")]
        [HttpPost]
        public HttpResponseMessage ParametreSehirPost(parametre_ulke_sehir sehir)
        {
            parametreRepository = new ParametreRepository();
            if (sehir != null)
            {
                sehir.degistiren_tarih = DateTime.Now;
                sehir.degistiren_carikart_id = Tools.PersonelId;
                var RenkRetVal = CrudRepository<parametre_ulke_sehir>.Insert(sehir, "parametre_ulke_sehir");
                if (RenkRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, RenkRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful - sehir_id =" + RenkRetVal.RetVal.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Şehirlere ait PUT metodu.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="sehir"></param>
        [CustAuthFilter(ApiUrl = "api/admin/parametre-sehir")]
        [Route("api/admin/parametre-sehir-put")]
        [HttpPut]
        public HttpResponseMessage ParametreSehirPut(parametre_ulke_sehir sehir)
        {
            parametreRepository = new ParametreRepository();
            if (sehir != null)
            {
                sehir.degistiren_tarih = DateTime.Now;
                sehir.degistiren_carikart_id = Tools.PersonelId;
                var RenkRetVal = CrudRepository<parametre_ulke_sehir>.Update(sehir, "sehir_id");
                if (RenkRetVal.ErrorInfo != null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, RenkRetVal.ErrorInfo.Message);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful - sehir_id =" + RenkRetVal.RetVal.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Şehirler DELETE metod.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <param name="sehir"></param>
        [CustAuthFilter(ApiUrl = "api/admin/ulke")]
        [Route("api/admin/parametre-sehir-delete")]
        [HttpDelete]
        public HttpResponseMessage ParametreSehirDelete(parametre_ulke_sehir sehir)
        {
            parametreRepository = new ParametreRepository();
            if (sehir != null)
            {
                parametre_ulke_sehir prsehir = new parametre_ulke_sehir();
                prsehir.sehir_id = sehir.sehir_id;
                prsehir.kayit_silindi = sehir.kayit_silindi;
                prsehir.degistiren_carikart_id = Tools.PersonelId;
                prsehir.degistiren_tarih = DateTime.Now;

                CrudRepository<parametre_ulke_sehir>.Update(prsehir, "sehir_id", new string[] {"ulke_id","sehir_adi","sehir_dunya_kodu","sehir_telefon_kodu","sehir_plaka_kodu","sehir_adi_dil_1","sehir_adi_dil_2",
"sehir_adi_dil_3","sehir_adi_dil_4","sehir_adi_dil_5","sira","ups_id" });

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = prsehir.sehir_id.ToString() });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion



    }
}
