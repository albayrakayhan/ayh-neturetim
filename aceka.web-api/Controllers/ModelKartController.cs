using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
using aceka.web_api.Models.StokkartModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Model Kart ile ilgili service
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ModelKartController : ApiController
    {
        #region Değişkenler
        private StokkartRepository stokKartRepository = null;
        private stokkart_rapor_parametre stok_rapor = null;
        private stokkart_ekler stok_ekler = null;
        private stokkart_fiyat stok_fiyat = null;
        private stokkart_kontrol stokkartkontrol = null;
        private parametre_beden parametrebeden = null;
        private ParametreRepository parametreRepository = null;
        private string errorMessage = "";
        #endregion

        public ModelKartController()
        {


        }

        /// <summary>
        /// Model Kart Arama Metodu
        /// </summary>
        /// <param name="stok_kodu"></param>
        /// <param name="stok_adi"></param>
        /// <param name="stokkart_id"></param>
        /// <param name="stokkart_tur_id"></param>
        /// <param name="stokkart_tipi_id"></param>
        /// <param name="stokkartturu">Modelkart için değer = 0 olmalı. Özellikle belirtilmez ise varsayılan değer "0" (sıfır) dır.</param>
        ///   /// <param name="orjinal_stok_kodu"></param>
        /// <returns>
        /// [
        ///   {
        ///     "stokkart_id": 4515,
        ///     "stok_adi": "evoSPEED Statement Indoor Shirt",
        ///     "stok_kodu": "701605 01",
        ///     "stokkart_tipi": "Mamul Model Kartı",
        ///     "stokkart_turu": "",
        ///     "statu": true
        ///   },
        ///   {
        ///     "stokkart_id": 4516,
        ///     "stok_adi": "evoSPEED Statement Indoor Shirt",
        ///     "stok_kodu": "701605 02",
        ///     "stokkart_tipi": "Mamul Model Kartı",
        ///     "stokkart_turu": "",
        ///     "statu": true
        ///   }
        /// ] 
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart")]
        [Route("api/modelkart/arama")]
        public HttpResponseMessage StokkartAra(string stok_adi = "", short stokkart_tur_id = 0, int stokkart_tipi_id = 1, string stok_kodu = "", byte stokkartturu = 0, string orjinal_stok_kodu = "")
        {
            stokKartRepository = new StokkartRepository();

            var stokkartlar = stokKartRepository.Bul(stok_adi, stokkart_tur_id, stokkart_tipi_id, stok_kodu, stokkartturu, orjinal_stok_kodu);
            if (stokkartlar != null && stokkartlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, stokkartlar.Select(sk => new
                {
                    sk.stokkart_id,
                    sk.stok_adi,
                    sk.stok_kodu,
                    stokkart_tipi = sk.stokkart_tipi_id > 0 ? sk.stokkarttipi.tanim : "",
                    stokkart_turu = sk.stokkart_tur_id >= 0 ? sk.stokkartturu.tanim : "",
                    sk.statu,
                    sk.stokkart_ozel.orjinal_stok_kodu,
                    sk.stokkart_ozel.stok_adi_uzun,
                    sk.stokkart_ozel.orjinal_stok_adi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record'" });
            }
        }

        /// <summary>
        /// Model kart stok koda göre arama yapan metod. (Autocomplate için kullanıldı)
        /// </summary>
        /// <param name="stok_kodu"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart")]
        [Route("api/modelkart/stok-kod-arama/{stok_kodu}")]
        public HttpResponseMessage StokKodArama(string stok_kodu)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul(stok_kodu, 0);
            if (stokkartlar != null && stokkartlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, stokkartlar.Select(sk => new
                {
                    sk.stokkart_id,
                    sk.stok_adi,
                    sk.stok_kodu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                //return Request.CreateResponse(HttpStatusCode.NotFound, new { });
            }

        }

        #region model kart genel Üst Kısım. 
        /// <summary>
        /// Model Kartta Fasoncu Listesini veren Method. carikart_tipi_id=14 olanlar. AA.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/fasoncular")]
        [Route("api/modelkart/fasoncular")]
        public HttpResponseMessage CarikartFasonListesi()
        {
            stokKartRepository = new StokkartRepository();
            var fasoncular = stokKartRepository.FasoncuGetir();

            if (fasoncular != null && fasoncular.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fasoncular.Select(p => new
                {
                    p.carikart_id,
                    p.cari_unvan
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Model Kart Genel Üst kısmı veren metod
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        ///   {
        ///  "stokkart_id": 1,
        ///  "birim_id_1": 2,
        ///  "birim_id_2": 4,
        ///  "birim_id_2_zorunlu": true,
        ///  "birim_id_3": 3,
        ///  "birim_id_3_zorunlu": false,
        ///  "kdv_alis_id": 18,
        ///  "kdv_satis_id": 18,
        ///  "stok_kodu": "Albayrak",
        ///  "stokkart_tipi_id": 1,
        ///  "stokkart_turu_id":0,
        ///  "statu": true,
        ///  "stok_adi": "selam",
        ///  "degistiren_carikart_id": 0,
        ///  "stokkart_onay": {
        ///    "genel_onay": false
        ///  },
        ///  "stokkart_ozel": {
        ///    "orjinal_renk_kodu": "",
        ///    "orjinal_renk_adi": "",
        ///    "stok_adi_uzun": "Kerem",
        ///    "orjinal_stok_kodu": "000001",
        ///    "orjinal_stok_adi": "orjinal ad yok"
        ///  }
        ///}
        /// </returns>        
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart")]
        [Route("api/modelkart/{stokkart_id}")]
        public HttpResponseMessage Getir(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            parametreRepository = new ParametreRepository();
            var stokKartTipi = parametreRepository.StokkartTip(stokkart_id);
            var stokKart = stokKartRepository.Getir(stokkart_id, stokKartTipi.stokkartturu);
            if (stokKart != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    stokKart.stokkart_id,
                    stokKart.birim_id_1,
                    stokKart.birim_id_2,
                    stokKart.birim_id_2_zorunlu,
                    stokKart.birim_id_3,
                    stokKart.birim_id_3_zorunlu,
                    stokKart.kdv_alis_id,
                    stokKart.kdv_satis_id,
                    stokKart.stok_kodu,
                    stokKart.stokkart_tipi_id,
                    stokKart.stokkart_tur_id,
                    stokKart.statu,
                    stokKart.stok_adi,
                    stokKart.degistiren_carikart_id,
                    stokKart.anastokkart_id,

                    stokkart_onay = new
                    {
                        genel_onay = (stokKart.stokkartonay != null && stokKart.stokkartonay.genel_onay ? true : false)
                    },

                    //talimat = (stokKart.talimat != null ?
                    //new
                    //{
                    //    stokKart.talimat.kod,
                    //    stokKart.talimat.talimatturu_id
                    //} : null),
                    //stok_talimat = (stokKart.stokkart_talimat != null ?
                    //new
                    //{
                    //    stokKart.stokkart_talimat.aciklama
                    //} : null),
                    stokkart_ozel = (stokKart.stokkart_ozel != null ?
                    new
                    {
                        stokKart.stokkart_ozel.orjinal_renk_kodu,
                        stokKart.stokkart_ozel.orjinal_renk_adi,
                        stokKart.stokkart_ozel.stok_adi_uzun,
                        stokKart.stokkart_ozel.orjinal_stok_kodu,
                        stokKart.stokkart_ozel.orjinal_stok_adi,
                        //stokKart.stokkart_ozel.tek_varyant
                    } : null)
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "Not Found!" });
            }
        }

        /// <summary>
        /// POST - > Model kart insert method.
        /// NOT: stokkart_id boş gönderilmeli
        /// </summary>
        /// <param name="stokkart"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/modelkart")]
        [Route("api/modelkart")]
        public HttpResponseMessage Post(Stokkart stokkart)
        {
            /*
             stokkodtipi bilgisi gönderildiğinde, model içerisinde stokkartturu de gönderilmeli
             bu alanlar tabloda stokkart_tipi_id ve stokkart_tur_id alanlarına eşitlenecek.
             Eğer stokkartturu gönderilmez ise giz_sabit_stokkarttipi tablosunda stokkartturu getirilecek!
             */
            AcekaResult acekaResult = null;

            if (stokkart != null)
            {
                parametreRepository = new ParametreRepository();
                var stokKartTipi = parametreRepository.StokkartTipi(stokkart.stokkart_tipi_id);
                if (stokKartTipi != null)
                    stokkart.stokkart_tur_id = stokKartTipi.stokkartturu; //stokkart_tur_id formdan gelmiyor. Bu sebeple giz_sabit_stokkarttipi tablosundan bu bilgiyi çektik.

                stokkart.degistiren_tarih = DateTime.Now;
                stokkart.degistiren_carikart_id = Tools.PersonelId;

                acekaResult = CrudRepository<Stokkart>.Insert(stokkart, "stokkart", new string[] { "stokkart_id" });
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    long stokkartId = acekaResult.RetVal.acekaToLong();

                    #region stok_talimat -> Gelen talimatturu_id ye göre "stok_talimat" kaydediliyor
                    //if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0
                    //    && stokkart.stok_talimat != null && !string.IsNullOrEmpty(stokkart.stok_talimat.aciklama.Trim()))
                    //{
                    //    stokkart_talimat stokkartTalimat = new stokkart_talimat
                    //    {
                    //        stokkart_id = stokkartId,
                    //        sira_id = 1,
                    //        aciklama = stokkart.stok_talimat.aciklama.Trim(),
                    //        talimatturu_id = stokkart.talimat.talimatturu_id,
                    //        degistiren_carikart_id = stokkart.degistiren_carikart_id,
                    //        degistiren_tarih = DateTime.Now
                    //    };
                    //    var talimatRetVal = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                    //}
                    #endregion

                    #region stokkart_ozel
                    if (stokkart.stokkart_ozel != null)
                    {
                        stokkart.stokkart_ozel.stokkart_id = stokkartId;
                        stokkart.stokkart_ozel.degistiren_tarih = stokkart.degistiren_tarih;
                        stokkart.stokkart_ozel.degistiren_carikart_id = Tools.PersonelId;
                        stokkart.stokkart_ozel.orjinal_renk_kodu = stokkart.stokkart_ozel.orjinal_renk_kodu;
                        stokkart.stokkart_ozel.orjinal_renk_adi = stokkart.stokkart_ozel.orjinal_renk_adi;

                        var stokkartOzelRetVal = CrudRepository<Stokkart_Ozel>.Insert(stokkart.stokkart_ozel, "stokkart_ozel", new string[] { "tek_varyant" });
                    }
                    #endregion

                    #region stokkart_onay ve Onay log
                    //stokkart_onay
                    //if (stokkart.stokkart_onay != null && stokkart.stokkart_onay.stokkart_id > 0)
                    //{
                    try
                    {
                        stokkart.stokkart_onay.stokkart_id = stokkartId;
                        stokkart.stokkart_onay.genel_onay = stokkart.stokkart_onay.genel_onay;
                        var stokkartOnayRetVal = CrudRepository<Stokkart_onay>.Insert(stokkart.stokkart_onay, "stokkart_onay");
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    //}
                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = stokkartId.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }

        /// <summary>
        ///  PUT - > Model kart update method.
        /// </summary>
        /// <param name="stokkart"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/modelkart")]
        [Route("api/modelkart")]
        public HttpResponseMessage Put(StokkartUpdate stokkart)
        {
            AcekaResult acekaResult = null;
            /*
             stokkodtipi bilgisi gönderildiğinde, model içerisinde stokkartturu de gnderilmeli
             bu alanlar tabloda stokkart_tipi_id ve stokkart_tur_id alanlarına eşitlenecek.
             Eğer stokkartturu gönderilmez ise giz_sabit_stokkarttipi tablosunda stokkartturu getirilecek!
             */
            if (stokkart != null && stokkart.stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var detay = stokKartRepository.Getir(stokkart.stokkart_id);
                if (detay != null)
                {
                    detay.degistiren_carikart_id = Tools.PersonelId;
                    detay.degistiren_tarih = DateTime.Now;
                    detay.birim_id_1 = stokkart.birim_id_1;
                    detay.birim_id_2 = stokkart.birim_id_2;
                    detay.birim_id_2_zorunlu = stokkart.birim_id_2_zorunlu;
                    detay.birim_id_3 = stokkart.birim_id_3;
                    detay.birim_id_3_zorunlu = stokkart.birim_id_3_zorunlu;
                    detay.stok_kodu = stokkart.stok_kodu;
                    detay.stok_adi = stokkart.stok_adi;
                    detay.kdv_alis_id = stokkart.kdv_alis_id;
                    detay.kdv_satis_id = stokkart.kdv_satis_id;
                    detay.stokkart_tur_id = stokkart.stokkart_tur_id;

                    if (detay.stokkart_tipi_id != stokkart.stokkart_tipi_id)
                    {
                        parametreRepository = new ParametreRepository();
                        var stokKartTipi = parametreRepository.StokkartTipi(stokkart.stokkart_tipi_id);
                        if (stokKartTipi != null)
                        {
                            detay.stokkart_tur_id = stokKartTipi.stokkartturu; //stokkart_tur_id formdan gelmiyor. Bu sebeple giz_sabit_stokkarttipi tablosundan bu bilgiyi çektik.
                            detay.stokkart_tipi_id = stokKartTipi.stokkarttipi; //Ayhan
                        }
                    }

                    detay.statu = stokkart.statu;

                    acekaResult = CrudRepository<stokkart>.Update(detay, "stokkart_id");

                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        #region stok_talimat
                        //var stokkartTalimat = stokKartRepository.StokkartTalimatDetay(stokkart.stokkart_id, 1);
                        //if (stokkartTalimat != null)
                        //{
                        //    //update
                        //    if (stokkart.stok_talimat != null)
                        //    {
                        //        stokkartTalimat.aciklama = stokkart.stok_talimat.aciklama;
                        //        stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                        //        stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                        //        stokkartTalimat.degistiren_tarih = DateTime.Now;

                        //        var stokTalimatRet = acekaResult = CrudRepository<stokkart_talimat>.Update(stokkartTalimat, new string[] { "stokkart_id", "sira_id" }, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                        //    }
                        //    else if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0
                        //&& stokkart.stok_talimat != null && !string.IsNullOrEmpty(stokkart.stok_talimat.aciklama.Trim()))
                        //    {
                        //        //insert
                        //        stokkartTalimat = new stokkart_talimat();
                        //        stokkartTalimat.stokkart_id = stokkart.stokkart_id;
                        //        stokkartTalimat.sira_id = 1;
                        //        stokkartTalimat.aciklama = stokkart.stok_talimat.aciklama.Trim();
                        //        stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                        //        stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                        //        stokkartTalimat.degistiren_tarih = DateTime.Now;
                        //        var stokTalimatRet = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                        //    }

                        //}
                        //else if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0)
                        //{
                        //    //insert. Ayhan
                        //    stokkartTalimat = new stokkart_talimat();
                        //    stokkartTalimat.stokkart_id = stokkart.stokkart_id;
                        //    stokkartTalimat.sira_id = 1;
                        //    stokkartTalimat.aciklama = stokkart.stok_talimat != null ? stokkart.stok_talimat.aciklama.Trim() : "";
                        //    stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                        //    stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                        //    stokkartTalimat.degistiren_tarih = DateTime.Now;
                        //    var stokTalimatRet = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                        //}
                        #endregion

                        #region stokkart_ozel
                        var stokkartOzel = stokKartRepository.StokkartOzelDetay(stokkart.stokkart_id);
                        if (stokkartOzel != null)
                        {
                            if (stokkart.stokkart_ozel != null)
                            {
                                // Update
                                stokkartOzel.degistiren_carikart_id = Tools.PersonelId;
                                stokkartOzel.degistiren_tarih = DateTime.Now;
                                stokkartOzel.stok_adi_uzun = stokkart.stokkart_ozel.stok_adi_uzun;
                                stokkartOzel.orjinal_stok_kodu = stokkart.stokkart_ozel.orjinal_stok_kodu;
                                stokkartOzel.orjinal_stok_adi = stokkart.stokkart_ozel.orjinal_stok_adi;
                                //stokkartOzel.tek_varyant = stokkart.stokkart_ozel.tek_varyant;
                                stokkartOzel.orjinal_renk_kodu = stokkart.stokkart_ozel.orjinal_renk_kodu;
                                stokkartOzel.orjinal_renk_adi = stokkart.stokkart_ozel.orjinal_renk_adi;


                                var stokkartOzelRetVal = CrudRepository<stokkart_ozel>.Update(stokkartOzel, "stokkart_id");
                            }

                        }
                        else if (stokkart.stokkart_ozel != null)
                        {
                            //insert
                            stokkart.stokkart_ozel.stokkart_id = stokkart.stokkart_id;
                            stokkart.stokkart_ozel.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                            stokkart.stokkart_ozel.degistiren_tarih = DateTime.Now;

                            var stokkartOzelRetVal = CrudRepository<Stokkart_Ozel>.Insert(stokkart.stokkart_ozel, "stokkart_ozel");
                        }
                        #endregion

                        #region stokkart_onay
                        //stokkart_onay
                        if (stokkart.stokkart_onay != null)
                        {
                            stokkart.stokkart_onay_log = new Stokkart_onay_log();
                            var onayKontrol = stokKartRepository.StokkartOnayLog(stokkart.stokkart_id);

                            if (onayKontrol != null)
                            {
                                //Update
                                Dictionary<string, object> fields = new Dictionary<string, object>();
                                fields.Add("onay_alan_adi", "genel_onay"); //onaylog.onay_alan_adi
                                fields.Add("iptal_carikart_id", Tools.PersonelId);
                                fields.Add("iptal_tarihi", DateTime.Now);
                                // fields.Add("stokkart_id", stokkart.stokkart_onay_log.stokkart_id);//onaylog.stokkart_id

                                //stokkart.stokkart_onay_log.stokkart_id = stokkart.stokkart_id;
                                ////stokkart.stokkart_onay_log.onay_alan_adi = "genel_onay"; //stokkart.stokkart_onay.genel_onay.ToString();
                                ////stokkart.stokkart_onay_log.onay_tarihi = onayKontrol.onay_tarihi;
                                //stokkart.stokkart_onay_log.onay_carikart_id = onayKontrol.onay_carikart_id;
                                //stokkart.stokkart_onay_log.iptal_tarihi = DateTime.Now;
                                //stokkart.stokkart_onay_log.iptal_carikart_id = Tools.PersonelId;
                                // var stokkartOzelRetVal = CrudRepository<Stokkart_onay_log>.Update(stokkart.stokkart_onay_log, new string[] { "stokkart_id" });

                                acekaResult = CrudRepository.Update("Stokkart_onay_log", "stokkart_id = " + stokkart.stokkart_id + " AND onay_alan_adi = 'genel_onay' AND  iptal_tarihi is null", fields, true);

                            }
                            else
                            {
                                //Insert
                                stokkart.stokkart_onay_log.stokkart_id = stokkart.stokkart_id;
                                stokkart.stokkart_onay_log.onay_alan_adi = "genel_onay"; //stokkart.stokkart_onay.genel_onay.ToString();
                                stokkart.stokkart_onay_log.onay_tarihi = DateTime.Now;
                                stokkart.stokkart_onay_log.onay_carikart_id = Tools.PersonelId;

                                var stokkartOzelRetVal = CrudRepository<Stokkart_onay_log>.Insert(stokkart.stokkart_onay_log, "stokkart_onay_log");
                            }
                        }
                        #endregion

                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = stokkart.stokkart_id.ToString() });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// DELETE - > Model kart delete method.
        /// Not: stokkart model içerisinde sadece "stokkart_id" ve "degistiren_carikart_id" değişkenlerinin dolu gelmesi yeterlidir! 
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/modelkart/onay-loglari")]
        [Route("api/modelkart/{stokkart_id}")]
        public HttpResponseMessage Delete(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                AcekaResult acekaResult = null;
                stokKartRepository = new StokkartRepository();

                //parametreRepository = new ParametreRepository();
                //var stokKartTipi = parametreRepository.StokkartTipi(stokkart.stokkart_tipi_id);
                //if (stokKartTipi != null)
                //    stokkart.stokkart_tur_id = stokKartTipi.stokkartturu; //stokkart_tur_id formdan gelmiyor. Bu sebeple giz_sabit_stokkarttipi tablosundan bu bilgiyi çektik.
                var detay = stokKartRepository.Getir(stokkart_id);
                if (detay != null)
                {
                    detay.statu = false; //stokkart.statu; // Ayhan. 23.02.2017
                    detay.degistiren_tarih = DateTime.Now;
                    detay.degistiren_carikart_id = Tools.PersonelId;
                    detay.kayit_silindi = true;

                    acekaResult = CrudRepository<stokkart>.Update(detay, "stokkart_id");
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                    }
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


        /// <summary>
        /// Model Kart Onay Geçmişi. 
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///         stokkart_id: 29660,
        ///         onay_tarihi: "2017-02-13T15:23:00",
        ///         onaylayan_cari: "Ayhan ALBAYRAK",
        ///         iptal_tarihi: "2017-02-20T13:01:00",
        ///         iptal_eden_cari: "Ayhan ALBAYRAK"
        ///     },
        ///     {
        ///         stokkart_id: 29660,
        ///         onay_tarihi: "2017-02-20T13:25:00",
        ///         onaylayan_cari: "Ayhan ALBAYRAK",
        ///         iptal_tarihi: null,
        ///         iptal_eden_cari: "Ayhan ALBAYRAK"
        ///     }
        /// ]
        /// </returns>
        //[HttpGet]
        //[CustAuthFilter(ApiUrl = "api/modelkart/onay-loglari")]
        //[Route("api/modelkart/onay-loglari/{stokkart_id}")]
        //public HttpResponseMessage StokkartOnayLoglari(long stokkart_id)
        //{
        //    string errorMessage = "";
        //    stokKartRepository = new StokkartRepository();
        //    var loglar = stokKartRepository.StokkartOnayLoglari(stokkart_id, CustomEnums.OnayLogTipi.genel_onay, ref errorMessage);
        //    if (loglar != null && loglar.Count > 0 && string.IsNullOrEmpty(errorMessage))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, loglar.Select(lg => new
        //        {
        //            lg.stokkart_id,
        //            lg.onay_tarihi,
        //            onaylayan_cari = lg.onaylayan_carikart.cari_unvan,
        //            lg.iptal_tarihi,
        //            iptal_eden_cari = lg.iptal_eden_carikart.cari_unvan
        //        }));
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message="No Record!"});
        //    }
        //}

        #endregion

        #region tab içerikleri

        #region genel tabı

        /// <summary>
        /// Model kart -> Genel Tab -> Rapor parametreleri GET
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        ///     {
        ///     satici_carikart_id: null,
        ///     stokalan_id_1: null,
        ///     stokalan_id_2: null,
        ///     stokalan_id_3: null,
        ///     stokalan_id_4: null,
        ///     stokalan_id_5: null,
        ///     stokalan_id_6: null,
        ///     stokalan_id_7: 0,
        ///     stokalan_id_8: 0,
        ///     stokalan_id_9: null,
        ///     stokalan_id_10: null,
        ///     stokalan_id_11: null,
        ///     stokalan_id_12: null,
        ///     stokalan_id_13: null,
        ///     stokalan_id_14: null,
        ///     stokalan_id_15: null,
        ///     stokalan_id_16: null,
        ///     stokalan_id_17: null,
        ///     stokalan_id_18: null,
        ///     stokalan_id_19: null,
        ///     stokalan_id_20: null,
        ///     uretimyeri_id:0
        ///     }
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/rapor-parametreler")]
        [Route("api/modelkart/rapor-parametreler/{stokkart_id}")]
        public HttpResponseMessage StokkartParametreler(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var parametreler = stokKartRepository.Stokkart_Genel_Parametreler(stokkart_id);

            if (parametreler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    parametreler.satici_carikart_id,
                    parametreler.satici_cari_unvan,
                    parametreler.stokkart_id,
                    parametreler.stokkart.stokkart_tipi_id,
                    parametreler.stokalan_id_1,
                    parametreler.stokalan_id_2,
                    parametreler.stokalan_id_3,
                    parametreler.stokalan_id_4,
                    parametreler.stokalan_id_5,
                    parametreler.stokalan_id_6,
                    parametreler.stokalan_id_7,
                    parametreler.stokalan_id_8,
                    parametreler.stokalan_id_9,
                    parametreler.stokalan_id_10,
                    parametreler.stokalan_id_11,
                    parametreler.stokalan_id_12,
                    parametreler.stokalan_id_13,
                    parametreler.stokalan_id_14,
                    parametreler.stokalan_id_15,
                    parametreler.stokalan_id_16,
                    parametreler.stokalan_id_17,
                    parametreler.stokalan_id_18,
                    parametreler.stokalan_id_19,
                    parametreler.stokalan_id_20,
                    parametreler.stokkart.uretimyeri_id
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "Kayıt Bulunamadı!" });
            }
        }

        /// <summary>
        /// Model kart -> Genel Tab -> Rapor parametreleri POST ve PUT  işlemi
        /// </summary>
        /// <param name="stokkartRaporParametreler"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/modelkart/rapor-parametreler")]
        [Route("api/modelkart/rapor-parametreler")]
        public HttpResponseMessage StokkartParametrelerPut(StokkartRaporParametreler stokkartRaporParametreler)
        {
            AcekaResult acekaResult = null;

            if (stokkartRaporParametreler != null)
            {
                if (stokkartRaporParametreler.satici_carikart_id != null && stokkartRaporParametreler.satici_carikart_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("stokkart_id", stokkartRaporParametreler.stokkart_id);
                    fields.Add("satici_carikart_id", stokkartRaporParametreler.satici_carikart_id);
                    var retVal = CrudRepository.Update("stokkart", "stokkart_id", fields);
                }

                if (stokkartRaporParametreler.uretimyeri_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("stokkart_id", stokkartRaporParametreler.stokkart_id);
                    fields.Add("uretimyeri_id", stokkartRaporParametreler.uretimyeri_id);
                    var retVal = CrudRepository.Update("stokkart", "stokkart_id", fields);
                }

                stokKartRepository = new StokkartRepository();
                stokkart_rapor_parametre parametre = stokKartRepository.Stokkart_Genel_Parametreler(stokkartRaporParametreler.stokkart_id);
                if (parametre != null)
                {
                    #region Update
                    // Update
                    parametre.degistiren_carikart_id = Tools.PersonelId;
                    parametre.degistiren_tarih = DateTime.Now;
                    parametre.stokalan_id_1 = stokkartRaporParametreler.stokalan_id_1;
                    parametre.stokalan_id_2 = stokkartRaporParametreler.stokalan_id_2;
                    parametre.stokalan_id_3 = stokkartRaporParametreler.stokalan_id_3;
                    parametre.stokalan_id_4 = stokkartRaporParametreler.stokalan_id_4;
                    parametre.stokalan_id_5 = stokkartRaporParametreler.stokalan_id_5;
                    parametre.stokalan_id_6 = stokkartRaporParametreler.stokalan_id_6;
                    parametre.stokalan_id_7 = stokkartRaporParametreler.stokalan_id_7;
                    parametre.stokalan_id_8 = stokkartRaporParametreler.stokalan_id_8;
                    parametre.stokalan_id_9 = stokkartRaporParametreler.stokalan_id_9;
                    parametre.stokalan_id_10 = stokkartRaporParametreler.stokalan_id_10;
                    parametre.stokalan_id_11 = stokkartRaporParametreler.stokalan_id_11;
                    parametre.stokalan_id_12 = stokkartRaporParametreler.stokalan_id_12;
                    parametre.stokalan_id_13 = stokkartRaporParametreler.stokalan_id_13;
                    parametre.stokalan_id_14 = stokkartRaporParametreler.stokalan_id_14;
                    parametre.stokalan_id_15 = stokkartRaporParametreler.stokalan_id_15;
                    parametre.stokalan_id_16 = stokkartRaporParametreler.stokalan_id_16;
                    parametre.stokalan_id_17 = stokkartRaporParametreler.stokalan_id_17;
                    parametre.stokalan_id_18 = stokkartRaporParametreler.stokalan_id_18;
                    parametre.stokalan_id_19 = stokkartRaporParametreler.stokalan_id_19;
                    parametre.stokalan_id_20 = stokkartRaporParametreler.stokalan_id_20;

                    acekaResult = CrudRepository<stokkart_rapor_parametre>.Update(parametre, "stokkart_id", new string[] { "satici_carikart_id", "satici_cari_unvan" });


                    #endregion
                }
                else
                {
                    #region Insert
                    // Insert
                    stokkartRaporParametreler.degistiren_tarih = DateTime.Now;
                    acekaResult = CrudRepository<StokkartRaporParametreler>.Insert(stokkartRaporParametreler, "stokkart_rapor_parametre", new string[] { "satici_carikart_id", "uretimyeri_id", "satici_cari_unvan" });
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    #endregion
                }
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }

        /// <summary>
        /// Model kart -> Genel tabındaki dosya eklerinin bilgileri döndürür        
        /// </summary>
        /// /// <param name="stokkart_id">stokkart_ekler tablosunda stokkart_id alanı</param>
        /// <returns>
        /// [
        ///    {
        ///  "stokkart_id": 29660,
        ///  "ek_id": 8758,
        ///  "ekadi": "germany-win-2.jpg",
        ///  "aciklama": "",
        ///  "filepath": "http://92.45.23.86:9597/content/files/",
        ///  "filename": "germany-win-2.jpg",
        ///  "ekturu_id": 1
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/genel-ekler")]
        [Route("api/modelkart/genel-ekler/{stokkart_id}")]
        public HttpResponseMessage StokkartEkler(long stokkart_id)
        {

            stokKartRepository = new StokkartRepository();
            if (stokkart_id > 0)
            {
                var stokkartEkler = stokKartRepository.Stokkart_Ekler(stokkart_id);
                if (stokkartEkler != null && stokkartEkler.Count > 0)
                {
                    parametreRepository = new ParametreRepository();

                    int[] ekIds = new int[stokkartEkler.Count];
                    for (int i = 0; i < stokkartEkler.Count; i++)
                    {
                        ekIds[i] = stokkartEkler[i].ek_id;
                    }
                    var ekler = parametreRepository.EkleriGetir(ekIds);
                    if (ekler != null && ekler.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ekler.Select(e => new
                        {
                            stokkart_id,
                            e.ek_id,
                            e.ekadi,
                            e.aciklama,
                            filepath = System.Configuration.ConfigurationManager.AppSettings["filePath"] + e.filepath,
                            e.filename,
                            e.ekturu_id
                        }));
                    }
                    else
                    {
                        //return Request.CreateResponse(HttpStatusCode.NotFound, new List<string>());
                        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                    }
                }
                else
                {
                    //return Request.CreateResponse(HttpStatusCode.NoContent, new List<string>());
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                //return Request.CreateResponse(HttpStatusCode.NotFound, new List<string>());
            }
        }

        /// <summary>
        ///Model kart -> Genel tabındaki dosya ekleri POST metodu
        /// </summary>
        /// <param name="ekler"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/modelkart/genel-ekler")]
        [Route("api/modelkart/genel-ekler")]
        public HttpResponseMessage StokkartEklerPost(List<StokkartEkler> ekler)
        {
            AcekaResult acekaResult = null;
            if (ekler != null && ekler.Count > 0)
            {
                Models.ParametreModel.Ekler ek = null;

                // "Ekler" tablosuna ek eklenip ek_id alınıyor ve bu id ile "stokkart_ekler" tablosuna kayıt yapılıyor!
                foreach (var item in ekler)
                {
                    ek = new Models.ParametreModel.Ekler();
                    ek.degistiren_carikart_id = Tools.PersonelId;
                    ek.degistiren_tarih = DateTime.Now;
                    ek.ekturu_id = item.ekturu_id;
                    ek.ekadi = item.ekadi;
                    ek.aciklama = item.aciklama;
                    ek.filepath = item.filepath;
                    ek.filename = item.filename;

                    var retVal = CrudRepository<Models.ParametreModel.Ekler>.Insert(ek, "ekler", new string[] { "ek_id" });
                    if (retVal != null && retVal.ErrorInfo == null && retVal.RetVal != null)
                    {
                        int ek_id = Convert.ToInt32(retVal.RetVal);
                        stokkart_ekler stokkartEk = new stokkart_ekler
                        {
                            stokkart_id = item.stokkart_id,
                            ek_id = ek_id,
                            degistiren_carikart_id = item.degistiren_carikart_id,
                            degistiren_tarih = ek.degistiren_tarih
                        };

                        var stokkartEkVal = CrudRepository<stokkart_ekler>.Insert(stokkartEk);
                        if (stokkartEkVal != null && stokkartEkVal.ErrorInfo == null)
                        {
                            stokkartEk = null;
                        }
                        else
                        {
                            acekaResult = new AcekaResult();
                            acekaResult = stokkartEkVal;
                            break;
                        }
                    }
                    else
                    {
                        acekaResult = new AcekaResult();
                        acekaResult = retVal;
                        break;
                    }

                }

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }

        /// <summary>
        /// Model kart -> Genel tabındaki dosya ekleri DELETE metodu
        /// Not: "stokkart_id", "ek_id" ve "filename" gönderilmelidir.
        /// </summary>
        /// <param name="ekler"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/modelkart/genel-ekler")]
        [Route("api/modelkart/genel-ekler")]
        public HttpResponseMessage StokkartEklerDelete(List<StokkartEkler> ekler)
        {
            AcekaResult acekaResult = null;
            if (ekler != null && ekler.Count > 0)
            {
                foreach (var ek in ekler)
                {
                    if (ek != null && ek.stokkart_id > 0 && ek.ek_id > 0)
                    {
                        //Stokkart_ekler tablosundan siliniyor
                        Dictionary<string, object> fields = new Dictionary<string, object>();
                        fields.Add("stokkart_id", ek.stokkart_id);
                        fields.Add("ek_id", ek.ek_id);

                        acekaResult = CrudRepository.Delete("stokkart_ekler", new string[] { "stokkart_id", "ek_id" }, fields);
                        if (acekaResult != null && acekaResult.ErrorInfo != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                        }
                        else
                        {
                            fields.Remove("stokkart_id");
                            //Ekler tablosundan siliniyor
                            acekaResult = CrudRepository.Delete("ekler", new string[] { "ek_id" }, fields);

                            if (acekaResult != null && acekaResult.ErrorInfo != null)
                            {
                                break;
                            }
                            else
                            {
                                var serverUploadFolder = System.Web.Hosting.HostingEnvironment.MapPath("/content/files/");
                                if (System.IO.File.Exists(serverUploadFolder + ek.filename))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(serverUploadFolder + ek.filename);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }
                }
                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// Model kart -> Genel tabındaki Varsayılan Satıcı Listesi Auto complate. AA
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "carikart_id": 100000000952,
        ///   "cari_unvan": "Avery Dennison Tekstil Ürünleri San. ve Tic. Ltd. Şti.",
        ///   "carikart_tipi_adi": "Alıcı/Satıcı",
        ///   "kayit_silindi": false
        /// },
        /// {
        ///   "carikart_id": 100000000953,
        ///   "cari_unvan": "VETAŞ PLASTİK SANAYİ VE TİC.LTD.ŞTİ",
        ///   "carikart_tipi_adi": "Alıcı/Satıcı",
        ///   "kayit_silindi": false
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/varsayilan_satici")]
        [Route("api/modelkart/varsayilan_satici")]
        public HttpResponseMessage VarsayilanSaticiListesi()
        {
            stokKartRepository = new StokkartRepository();
            var saticilar = stokKartRepository.VarsayilanSaticiListesi();
            if (saticilar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, saticilar.Select(satici => new
                {
                    satici.carikart_id,
                    satici.cari_unvan,
                    satici.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    satici.kayit_silindi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        #endregion
        #region Stokkart(Modelkart) Varyant Sekmesi: parametre_beden tablosu

        ///<summary>
        /// Model kart  -> Beden Grublarını veren Metod. AA. Grubu ait bedenlerin listesi api/parametre/bedenler ' de.
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///    "bedengrubu": "A"
        ///  },
        ///  {
        ///    "bedengrubu": "B"
        ///  },
        ///  {
        ///    "bedengrubu": "C"
        ///  },
        ///  {
        ///    "bedengrubu": "D"
        ///  }
        /// ]
        ///</returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/parametre-beden")]
        [Route("api/modelkart/parametre-beden")]
        public HttpResponseMessage bedenGetir()
        {
            stokKartRepository = new StokkartRepository();
            var bedenlist = stokKartRepository.BedenGetir();
            if (bedenlist != null)
            {
                var bedenler = bedenlist.GroupBy(n => new { n.bedengrubu })
                    .Select(g => new { g.Key.bedengrubu }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, bedenler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        ///<summary>
        /// Model kart  -> Varyant Stokkarta ait  bedenleri Listesini veren Metod. AA.
        /// </summary>
        /// <returns>
        ///     {
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "0001-01-01T00:00:00",
        ///  "kayit_silindi": false,
        ///  "statu": true,
        ///  "sku_no": "123",
        ///  "stokkart_id": 1,
        ///  "renk_id": 70655,
        ///  "beden_id": 2,
        ///  "asorti": null,
        ///  "asorti_miktar": null,
        ///  "bedengrubu": "A",
        ///  "beden_tanimi": "4XL"
        ///}
        ///</returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/beden")]
        [Route("api/modelkart/beden/{stokkart_id}")]
        public HttpResponseMessage BedenGet(long stokkart_id)
        {
            //giz_setup_varyant_otovtablosundan gelecek.
            stokKartRepository = new StokkartRepository();
            var bedenler = stokKartRepository.BedenListesi(stokkart_id);
            if (bedenler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bedenler.Select(beden => new
                {
                    beden.sku_id,
                    //beden.degistiren_carikart_id,
                    //beden.degistiren_tarih,
                    //beden.kayit_silindi,
                    beden.statu,
                    beden.sku_no,
                    beden.stokkart_id,
                    beden.renk_id,
                    beden.beden_id,
                    beden.asorti,
                    beden.asorti_miktar,
                    //beden.parametre_beden.beden,
                    beden.parametre_beden.bedengrubu,
                    beden.parametre_beden.beden_tanimi,

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });

            }
        }

        /// <summary>
        /// Model kart  -> Varyant Bedenler-> POST.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[CustAuthFilter]
        [Route("api/modelkart/beden")]
        public IHttpActionResult bedenPost(stokkart_sku stokkartbeden)
        {
            AcekaResult acekaResult = null;
            stokKartRepository = new StokkartRepository();

            stokkart_sku skk = new stokkart_sku();
            var bedenler = stokKartRepository.BedenListesi(stokkartbeden.stokkart_id);

            List<int> varOlanBedenler = bedenler != null
                ? bedenler.Select(x => x.beden_id).ToList()
                : new List<int>();

            var insertList = stokkartbeden.parametre_bedenler
                .Where(x => !varOlanBedenler.Contains(x.beden_id)).ToList();

            foreach (var insertItem in insertList)
            {
                //skk.asorti = stokkartbeden.asorti;
                //skk.asorti_miktar = stokkartbeden.asorti_miktar;
                skk.degistiren_carikart_id = Tools.PersonelId;
                skk.degistiren_tarih = DateTime.Now;
                skk.kayit_silindi = false;
                //skk.renk_id = stokkartbeden.renk_id;
                skk.sku_no = !String.IsNullOrWhiteSpace(stokkartbeden.sku_no) ? stokkartbeden.sku_no : "0";
                skk.statu = true;
                skk.stokkart_id = stokkartbeden.stokkart_id;
                skk.beden_id = insertItem.beden_id;

                acekaResult = CrudRepository<stokkart_sku>.Insert(skk, new string[] { "sku_id" });

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                }
            }

            return Ok(acekaResult);
        }

        /// <summary>
        /// Model kart  -> Bedenler-> PUT  Güncelleme.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        //[CustAuthFilter]
        [Route("api/modelkart/beden")]
        public HttpResponseMessage bedenPut(stokkart_sku beden)
        {
            AcekaResult acekaResult = null;
            if (beden != null && beden.beden_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var bedenliste = stokKartRepository.VaryantDetay(beden.sku_id);

                if (bedenliste != null)
                {
                    stokkart_sku sku = new stokkart_sku();

                    //bedenliste.parametre_bedenler = new List<parametre_beden>();
                    //foreach (var item in bedenliste.parametre_beden)
                    //{
                    sku.sku_id = beden.sku_id;
                    sku.asorti = beden.asorti;
                    sku.asorti_miktar = beden.asorti_miktar;
                    sku.beden_id = beden.beden_id;
                    sku.degistiren_carikart_id = beden.degistiren_carikart_id;
                    sku.degistiren_tarih = DateTime.Now;
                    sku.kayit_silindi = beden.kayit_silindi;
                    sku.renk_id = beden.renk_id;
                    sku.sku_no = beden.sku_no;
                    sku.statu = beden.statu;
                    sku.stokkart_id = beden.stokkart_id;
                    sku.beden_id = beden.beden_id;
                    CrudRepository<stokkart_sku>.Update(sku, "sku_id");
                    //}

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = beden.stokkart_id.ToString() });
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

        /// <summary>
        /// Model kart  -> Varyant Bedenler-> DELETE Silme.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        //[CustAuthFilter]
        [Route("api/modelkart/beden")]
        public HttpResponseMessage bedenDelete(stokkart_sku beden)
        {
            AcekaResult acekaResult = null;
            if (beden != null)//&& beden.st.stokkart_id > 0
            {
                stokKartRepository = new StokkartRepository();
                var bedenliste = stokKartRepository.VaryantDetay(beden.sku_id);

                if (bedenliste != null)
                {
                    stokkart_sku sku = new stokkart_sku();
                    sku.sku_id = beden.sku_id;
                    sku.degistiren_carikart_id = Tools.PersonelId;
                    sku.degistiren_tarih = DateTime.Now;
                    sku.kayit_silindi = beden.kayit_silindi;
                    sku.sku_no = beden.sku_no;
                    CrudRepository<stokkart_sku>.Update(sku, "sku_id", new string[] { "R.kayit_silindi" });

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = beden.stokkart_id.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion
        #region İlk Madde Kumaş, İlk Madde Aksesuar, İlk Madde İplik

        /// <summary>
        /// Modelkart - > Tab - > İlk madde modelleri (Kumaş = 20, Aksesuar = 21, İplik = 22)
        /// </summary>
        /// <param name="stokkart_id"></param>
        ///  <param name="stokkart_tipi_id">1-> Mamül Model Kart</param>
        /// <returns>
        /// [
        ///  {
        ///    "stokkart_id": 32445,
        ///    "sira_id": 1,
        ///    "beden_id": 1,
        ///    "talimatturu_id": 1,
        ///    "modelyeri": "Test Data yı update ediyorum",
        ///    "alt_stokkart_id": 32444,
        ///    "renk_id": 9,
        ///    "ana_kayit": 1,
        ///    "aciklama": "Bu açıklama bizi yoracak gibi",
        ///    "birim_id": 1,
        ///    "birim_id3": 1,
        ///    "miktar": 1,
        ///    "miktar3": 1
        ///  },
        ///  {
        ///    "stokkart_id": 32445,
        ///    "sira_id": 2,
        ///    "beden_id": 1,
        ///    "talimatturu_id": 1,
        ///    "modelyeri": "Test Data",
        ///    "alt_stokkart_id": 32444,
        ///    "renk_id": 9,
        ///    "ana_kayit": 1,
        ///    "aciklama": "Bu açıklama bizi yoracak gibi",
        ///    "birim_id": 1,
        ///    "birim_id3": 1,
        ///    "miktar": 1,
        ///    "miktar3": 1
        ///  }
        /// ]
        /// 
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/ilk-madde-modeller")]
        [Route("api/modelkart/ilk-madde-modeller/{stokkart_id},{stokkart_tipi_id}")]
        public HttpResponseMessage StokkartModelListesiniGetir(long stokkart_id, byte stokkart_tipi_id)
        {
            stokKartRepository = new StokkartRepository();
            var modeler = stokKartRepository.StokkartModelListesiniGetir(stokkart_id, stokkart_tipi_id, ref errorMessage);
            if (modeler != null && string.IsNullOrEmpty(errorMessage))
            {
                return Request.CreateResponse(HttpStatusCode.OK, modeler.Select(model => new
                {
                    model.stokkart_id,
                    model.sira_id,
                    model.beden_id,
                    model.talimatturu_id,
                    model.modelyeri,
                    model.alt_stokkart_id,
                    model.renk_id,
                    model.ana_kayit,
                    model.aciklama,
                    model.birim_id,
                    model.birim_id3,
                    model.miktar,
                    model.miktar3,
                }));
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                }

            }
        }
        /// <summary>
        /// Modelkart - > Tab - > İlk madde modelleri pivot liste (Kumaş = 20, Aksesuar = 21, İplik = 22)
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="stokkart_tipi_id"></param>
        /// <returns>
        /// [
        ///   {
        ///     stokkart_id: 1,
        ///     sira_id: 4,
        ///     modelyeri: "Bu bir İplik",
        ///     aciklama: "Stokkart Tipi 22",
        ///     stok_adi: "jakarlı interlok",
        ///     renk_id: 9,
        ///     renk_adi: "Navy",
        ///     talimatturu_id: 1,
        ///     talimat_tanim: "KESİM",
        ///     alt_stokkart_id: 30066,
        ///     birim: {
        ///     birim_adi: "Kilogram",
        ///     birim_id: 5
        ///     },
        ///     birim3: {
        ///     birim_adi: "Metre",
        ///     birim_id: 23
        ///     },
        ///     pivotMatrixData: [
        ///     {
        ///     id : 34
        ///     name: "94 OSFA",
        ///     value: 1.2
        ///     },
        ///     {
        ///     id:95
        ///     name: "95 38/32",
        ///     value: 0     
        ///     },
        ///     {
        ///     id:96
        ///     name: "96 40/34",
        ///     value: 0
        ///     }
        ///     ]
        ///     }
        ///  ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/ilk-madde-modeller")]
        [Route("api/modelkart/ilk-madde-modeller-pivot/{stokkart_id},{stokkart_tipi_id}")]
        public IHttpActionResult StokkartPivotModelListesiniGetir(long stokkart_id, byte stokkart_tipi_id)
        {
            stokKartRepository = new StokkartRepository();
            var modeler = stokKartRepository.StokkartPivotModelListesiniGetir(stokkart_id, stokkart_tipi_id, ref errorMessage);

            if (modeler != null && modeler.Rows.Count > 0 && string.IsNullOrEmpty(errorMessage))
            {
                List<StokkartIlkmaddePivotModel> pivot = new List<StokkartIlkmaddePivotModel>();
                StokkartIlkmaddePivotModel item = null;

                for (int i = 0; i < modeler.Rows.Count; i++)
                {
                    item = new StokkartIlkmaddePivotModel
                    {
                        stokkart_id = modeler.Rows[i]["stokkart_id"].acekaToLong(),
                        sira_id = modeler.Rows[i]["sira_id"].acekaToShort(),
                        sira = modeler.Rows[i]["sira"].acekaToByte(),
                        modelyeri = modeler.Rows[i]["modelyeri"].acekaToString(),
                        talimatturu_id = modeler.Rows[i]["talimatturu_id"].acekaToByte(),
                        talimat_tanim = modeler.Rows[i]["talimat_tanim"].acekaToString(),
                        alt_stokkart_id = modeler.Rows[i]["alt_stokkart_id"].acekaToLongWithNullable(),
                        stok_adi = modeler.Rows[i]["stok_adi"].acekaToString(),
                        renk_id = modeler.Rows[i]["renk_id"].acekaToIntWithNullable(),
                        renk_adi = modeler.Rows[i]["renk_adi"].acekaToString(),
                        aciklama = modeler.Rows[i]["aciklama"].acekaToString(),
                        birim_id = modeler.Rows[i]["birim_id"].acekaToByteWithNullable(),
                        birim_adi = modeler.Rows[i]["birim_adi"].acekaToString(),
                        birim_id3 = modeler.Rows[i]["birim_id3"].acekaToByteWithNullable(),
                        birim_adi3 = modeler.Rows[i]["birim_adi3"].acekaToString(),
                    };

                    item.pivotMatrixData = new List<StokkartIlkmaddePivotMatrix>();

                    bool isGenel = true;

                    for (int x = 14; x < modeler.Columns.Count; x++)
                    {
                        if (modeler.Columns[x].ColumnName == "stokkart_id" || modeler.Columns[x].ColumnName == "sira_id" || modeler.Columns[x].ColumnName == "miktar")
                            continue;

                        float? value = modeler.Rows[i][modeler.Columns[x].ColumnName].acekaToFloatWithNullable();
                        var pivotNameData = modeler.Columns[x].ColumnName.Split(' ');

                        item.pivotMatrixData.Add(new StokkartIlkmaddePivotMatrix
                        {
                            id = pivotNameData[0].acekaToShort(),
                            name = modeler.Columns[x].ColumnName,
                            value = value
                        });

                        if ((value.HasValue && value.Value > 0) && isGenel == true)
                        {
                            isGenel = false;
                        }
                    }

                    if (isGenel)
                        item.genel = modeler.Rows[i]["miktar"].acekaToFloat();

                    pivot.Add(item);
                    item = null;
                }

                return Ok(pivot);
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return InternalServerError(new Exception(errorMessage));
                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
        }

        /// <summary>
        /// Modelkart -> Tab -> İlk madde ortak PUT metodu 
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/modelkart/ilk-madde-modeller")]
        [Route("api/modelkart/ilk-madde-modeller")]
        public HttpResponseMessage ModelkartIlkMaddePOST(StokkartIlkmaddePivotModel modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;

            if (modelKartIlkMadde == null || modelKartIlkMadde.stokkart_id <= 0 || modelKartIlkMadde.alt_stokkart_id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new Models.AnonymousModels.NotFound { message = "Lütfen POST model içerisindeki verileri kontrol ediniz!" });
            }

            stokKartRepository = new StokkartRepository();


            short maxSiraId = stokKartRepository.StokkartModelEnBuyukSiraNo(modelKartIlkMadde.stokkart_id, ref errorMessage);
            maxSiraId++;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Sıra numarası alırken bir hata oluştu!" });
            }

            if (modelKartIlkMadde.genel.HasValue && modelKartIlkMadde.genel.Value > 0)
            {
                ModelKartIlkMadde model = new ModelKartIlkMadde
                {
                    beden_id = 0,
                    miktar = modelKartIlkMadde.genel,
                    aciklama = modelKartIlkMadde.aciklama,
                    modelyeri = modelKartIlkMadde.modelyeri,
                    alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                    stokkart_id = modelKartIlkMadde.stokkart_id,
                    ana_kayit = 0,
                    sira = modelKartIlkMadde.sira,
                    sira_id = maxSiraId,
                    degistiren_carikart_id = Tools.PersonelId,
                    degistiren_tarih = DateTime.Now,
                    birim_id = modelKartIlkMadde.birim_id.Value,
                    renk_id = modelKartIlkMadde.renk_id.Value
                };

                acekaResult = CrudRepository<ModelKartIlkMadde>.Insert(model, "stokkart_model", new string[] { "old_sira_id" });
            }
            else
            {
                for (int i = 0; i < modelKartIlkMadde.pivotMatrixData.Count; i++)
                {
                    ModelKartIlkMadde model = new ModelKartIlkMadde
                    {
                        beden_id = modelKartIlkMadde.pivotMatrixData[i].id,
                        miktar = modelKartIlkMadde.pivotMatrixData[i].value,
                        aciklama = modelKartIlkMadde.aciklama,
                        modelyeri = modelKartIlkMadde.modelyeri,
                        alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                        stokkart_id = modelKartIlkMadde.stokkart_id,
                        ana_kayit = 0,
                        sira = modelKartIlkMadde.sira,
                        sira_id = maxSiraId,
                        degistiren_carikart_id = Tools.PersonelId,
                        degistiren_tarih = DateTime.Now,
                        birim_id = modelKartIlkMadde.birim_id.Value,
                        renk_id = modelKartIlkMadde.renk_id.Value,

                        //birim_id3 = "",
                        //miktar3 = "",
                        //talimatturu_id = "",
                    };

                    acekaResult = CrudRepository<ModelKartIlkMadde>.Insert(model, "stokkart_model", new string[] { "old_sira_id" });
                }
            }

            if (acekaResult != null && acekaResult.ErrorInfo != null)
            {
                var databaseError = acekaResult.ErrorInfo.Message;
                if (databaseError.ToLower().Contains("duplicate"))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.Successful { message = "Duplicate record!" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.Successful { message = acekaResult.ErrorInfo.Message });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
        }

        /// <summary>
        /// Modelkart -> Tab -> İlk madde ortak PUT metodu 
        /// Not: PUT metodu için  "stokkart_id", "sira_id", "alt_stokkart_id" bilgisi muhakkak gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/modelkart/ilk-madde-modeller")]
        [Route("api/modelkart/ilk-madde-modeller")]
        public IHttpActionResult ModelkartIlkMaddePUT(StokkartIlkmaddePivotModel modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;
            if (
                modelKartIlkMadde == null ||
                modelKartIlkMadde.stokkart_id <= 0 ||
                modelKartIlkMadde.alt_stokkart_id < 1 ||
                modelKartIlkMadde.sira_id < 0
                )
            {
                return BadRequest();
            }

            stokKartRepository = new StokkartRepository();

            bool isGenel = true;
            modelKartIlkMadde.pivotMatrixData.ForEach(x =>
            {
                if (isGenel && (x.value.HasValue && x.value.Value > 0))
                    isGenel = false;
            });

            if (isGenel)
            {
                var deleteData = new stokkart_model
                {
                    stokkart_id = modelKartIlkMadde.stokkart_id,
                    sira_id = modelKartIlkMadde.sira_id,
                };

                acekaResult = CrudRepository<stokkart_model>.Delete(deleteData, "stokkart_model", new string[] { "stokkart_id", "sira_id" }, new string[] { "stokkart_id", "sira_id" });

                var insertModel = new stokkart_model();

                insertModel.alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id;
                insertModel.degistiren_tarih = DateTime.Now;
                insertModel.degistiren_carikart_id = Tools.PersonelId;
                insertModel.talimatturu_id = modelKartIlkMadde.talimatturu_id;
                insertModel.modelyeri = modelKartIlkMadde.modelyeri;
                insertModel.renk_id = modelKartIlkMadde.renk_id;
                insertModel.sira = modelKartIlkMadde.sira;
                insertModel.ana_kayit = modelKartIlkMadde.ana_kayit;
                insertModel.aciklama = modelKartIlkMadde.aciklama;
                insertModel.birim_id = modelKartIlkMadde.birim_id;
                insertModel.birim_id3 = modelKartIlkMadde.birim_id3;
                insertModel.beden_id = 0;
                insertModel.sira_id = modelKartIlkMadde.sira_id;
                insertModel.stokkart_id = modelKartIlkMadde.stokkart_id;

                insertModel.miktar = modelKartIlkMadde.genel;
                insertModel.miktar3 = modelKartIlkMadde.miktar3;
                acekaResult = CrudRepository<stokkart_model>.Insert(insertModel);

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                }
            }
            else
            {
                var deleteData = new stokkart_model
                {
                    stokkart_id = modelKartIlkMadde.stokkart_id,
                    sira_id = modelKartIlkMadde.sira_id
                };

                acekaResult = CrudRepository<stokkart_model>.Delete(deleteData, "stokkart_model", new string[] { "stokkart_id", "sira_id" }, new string[] { "stokkart_id", "sira_id" });

                for (int i = 0; i < modelKartIlkMadde.pivotMatrixData.Count; i++)
                {

                    var model = new stokkart_model();
                    model.alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id;
                    model.degistiren_tarih = DateTime.Now;
                    model.degistiren_carikart_id = Tools.PersonelId;
                    model.talimatturu_id = modelKartIlkMadde.talimatturu_id;
                    model.modelyeri = modelKartIlkMadde.modelyeri;
                    model.renk_id = modelKartIlkMadde.renk_id;
                    model.sira = modelKartIlkMadde.sira;
                    model.ana_kayit = modelKartIlkMadde.ana_kayit;
                    model.aciklama = modelKartIlkMadde.aciklama;
                    model.birim_id = modelKartIlkMadde.birim_id;
                    model.birim_id3 = modelKartIlkMadde.birim_id3;
                    model.beden_id = modelKartIlkMadde.pivotMatrixData[i].id;
                    model.sira_id = modelKartIlkMadde.sira_id;
                    model.stokkart_id = modelKartIlkMadde.stokkart_id;

                    model.miktar = modelKartIlkMadde.pivotMatrixData[i].value; //modelKartIlkMadde.miktar;
                    model.miktar3 = modelKartIlkMadde.miktar3;
                    acekaResult = CrudRepository<stokkart_model>.Insert(model);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                    }
                }
            }

            return Ok(acekaResult);
        }

        /// <summary>
        /// Modelkart -> Tab -> İlk madde ortak DELETE metodu 
        /// Not: DELETE metodu için  sadece "stokkart_id", "sira_id" gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/modelkart/ilk-madde-modeller")]
        [Route("api/modelkart/ilk-madde-modeller")]
        public HttpResponseMessage ModelkartIlkMaddeDelete(ModelKartIlkMadde modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;
            if (
                modelKartIlkMadde != null &&
                modelKartIlkMadde.stokkart_id > 0 &&
                modelKartIlkMadde.sira_id > 0
                )
            {
                stokKartRepository = new StokkartRepository();
                var model = stokKartRepository.StokkartModelDetayiniGetir(modelKartIlkMadde.stokkart_id, modelKartIlkMadde.sira_id, ref errorMessage);

                if (model != null && string.IsNullOrEmpty(errorMessage))
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("stokkart_id", modelKartIlkMadde.stokkart_id);
                    fields.Add("sira_id", modelKartIlkMadde.sira_id);

                    acekaResult = CrudRepository.Delete("stokkart_model", new string[] { "stokkart_id", "sira_id" }, fields);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.Successful { message = acekaResult.ErrorInfo.Message });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Model karta ait kayıt bulunamadı!" });
                    }

                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Lütfen POST model içerisindeki verileri kontrol ediniz!" });
            }
        }
        #endregion
        #region Talimatlar

        /// <summary>
        /// Modelkart - > Tab - > Talimatlar GET Metod (Detay).
        /// </summary>
        /// <param name="stokkart_id">Zorunlu</param>
        /// <param name="sira_id">Zorunlu</param>
        /// <returns>
        ///   {
        ///     "stokkart_id": 1,
        ///     "sira_id": 2,
        ///     "eski_sira_id" : 2,
        ///     "talimatturu_id": 2,
        ///     "fasoncu_carikart_id": null,
        ///     "aciklama": "2. talimat Bol bol yıka",
        ///     "irstalimat": "",
        ///     "islem_sayisi": null
        ///   }
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/modelkart/talimatlar")]
        [Route("api/modelkart/talimatlar/{stokkart_id},{sira_id}")]
        [HttpGet]
        public HttpResponseMessage TalimatlarGet(long stokkart_id, short sira_id)
        {
            if (stokkart_id > 0 && sira_id > 0)
            {
                stokKartRepository = new StokkartRepository();

                var talimat = stokKartRepository.StokkartTalimatDetay(stokkart_id, sira_id);
                if (talimat != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        talimat.stokkart_id,
                        talimat.sira_id,
                        eski_sira_id = talimat.sira_id,
                        talimat.talimatturu_id,
                        talimat.fasoncu_carikart_id,
                        talimat.aciklama,
                        talimat.irstalimat,
                        talimat.islem_sayisi,
                    });
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
        /// <summary>
        /// Modelkart - > Tab - > Talimatlar GET Metod (List).
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///   {
        ///     "stokkart_id": 1,
        ///     "sira_id": 1,
        ///     "eski_sira_id" : 1,
        ///     "talimatturu_id": 3,
        ///     "fasoncu_carikart_id": null,
        ///     "aciklama": "Bol bol yıka",
        ///     "irstalimat": "",
        ///     "islem_sayisi": null
        ///     "cari_unvan": ""
        ///   },
        ///   {
        ///     "stokkart_id": 1,
        ///     "sira_id": 2,
        ///     "talimatturu_id": 2,
        ///     "fasoncu_carikart_id": null,
        ///     "aciklama": "2. talimat Bol bol yıka",
        ///     "irstalimat": "",
        ///     "islem_sayisi": null
        ///     "cari_unvan": ""
        ///   }
        /// ]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/modelkart/talimatlar")]
        [Route("api/modelkart/talimatlar/{stokkart_id}")]
        [HttpGet]
        public HttpResponseMessage TalimatlarGet(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();

                var talimatlar = stokKartRepository.StokkartTalimatlar(stokkart_id, ref errorMessage);
                if (talimatlar != null && string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, talimatlar.Select(talimat => new
                    {
                        talimat.stokkart_id,
                        talimat.sira_id,
                        eski_sira_id = talimat.sira_id,
                        talimat.talimatturu_id,
                        talimat.fasoncu_carikart_id,
                        talimat.aciklama,
                        talimat.talimat_adi,
                        talimat.irstalimat,
                        talimat.islem_sayisi,
                        talimat.cari_kart.cari_unvan
                    }));
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                    }

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Modelkart - > Tab - > Talimatlar POST Metod     
        /// NOT 1 : sira_id gönderilmez ise sistem sira_id yi otomatik oluşturacak.
        /// NOT 2: Bir model karta ait birden fazla aynı talimatturu_id verlemez. Herkayıt için farklı telimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="stokkartTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/modelkart/talimatlar")]
        [Route("api/modelkart/talimatlar")]
        [HttpPost]
        public HttpResponseMessage TalimatlarPost(StokkartTalimat stokkartTalimat)
        {
            AcekaResult acekaResult = null;

            if (stokkartTalimat != null && stokkartTalimat.stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();

                short yeniSiraId = 0;

                //Sira_id elle değiştirildiğinde aynı stokkart_id ye ait var olan başka bir sira_id var mı diye kontrol ediyoruz
                if (stokkartTalimat.sira_id > 0)
                {
                    var siraIDRetVal = stokKartRepository.StokkartTalimatSiraID_Kontrol(stokkartTalimat.stokkart_id, stokkartTalimat.sira_id, ref errorMessage);
                    if (siraIDRetVal > 0 && string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Bu Talimata ait zaten bir 'sira_id' mevcut!<br/> Lütfen farklı bir Sıra No giriniz!!" });
                    }
                    else
                    {
                        yeniSiraId = stokkartTalimat.sira_id;
                    }
                }
                else
                {
                    var enBuyukSiraNo = stokKartRepository.StokkartTalimatEnBuyukSiraNo(stokkartTalimat.stokkart_id, ref errorMessage);
                    yeniSiraId = Convert.ToInt16(enBuyukSiraNo + 1);
                }

                stokkartTalimat.sira_id = yeniSiraId;
                stokkartTalimat.degistiren_carikart_id = Tools.PersonelId;
                stokkartTalimat.degistiren_tarih = DateTime.Now;

                acekaResult = CrudRepository<StokkartTalimat>.Insert(stokkartTalimat, "stokkart_talimat", new string[] { "eski_sira_id" });
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful", ret_val = acekaResult.RetVal.ToString() });
                }
                else
                {
                    if (acekaResult.ErrorInfo != null)
                    {
                        if (acekaResult.ErrorInfo.Message.ToLower().Contains("duplicate"))
                        {
                            acekaResult.ErrorInfo.Message = "Modelkart a aynı talimat türü 2.kez tanımlanamaz!";
                        }

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process. Please check input areas!" });
            }
        }

        /// <summary>
        /// Modelkart - > Tab - > Talimatlar PUT Metod. 
        /// NOT 1: eski_sira_id ve sira_id dolu gönderilmeli! sira_id değişti ise API eski_sira_id kaydını kullanarak kaydı buluyor ve yenisi ile güncelliyor.
        /// NOT 2: Bir model karta ait birden fazla aynı talimatturu_id verlemez. Herkayıt için farklı telimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="stokkartTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/modelkart/talimatlar")]
        [Route("api/modelkart/talimatlar")]
        [HttpPut]
        public HttpResponseMessage TalimatlarPut(StokkartTalimat stokkartTalimat)
        {
            AcekaResult acekaResult = null;

            if (stokkartTalimat != null && stokkartTalimat.stokkart_id > 0 && stokkartTalimat.sira_id > 0 && stokkartTalimat.eski_sira_id > 0)
            {
                stokKartRepository = new StokkartRepository();

                stokkart_talimat talimatDetay = stokKartRepository.StokkartTalimatDetay(stokkartTalimat.stokkart_id, stokkartTalimat.eski_sira_id);

                bool islemDevam = false;
                //Sira_id elle değiştirildiğinde aynı stokkart_id ye ait var olan başka bir sira_id var mı diye kontrol ediyoruz
                if (stokkartTalimat.sira_id != stokkartTalimat.eski_sira_id)
                {
                    var siraIDRetVal = stokKartRepository.StokkartTalimatSiraID_Kontrol(stokkartTalimat.stokkart_id, stokkartTalimat.sira_id, ref errorMessage);
                    if (siraIDRetVal > 0 && string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Modelkarta ait zaten bir 'sira_id' mevcut!" });
                    }
                    else
                    {
                        islemDevam = true;
                    }
                }
                else
                {
                    islemDevam = true;
                }

                if (islemDevam)
                {

                    if (talimatDetay != null)
                    {
                        Dictionary<string, object> fields = new Dictionary<string, object>();
                        fields.Add("sira_id", stokkartTalimat.sira_id);
                        fields.Add("degistiren_carikart_id", Tools.PersonelId);
                        fields.Add("degistiren_tarih", DateTime.Now);
                        fields.Add("talimatturu_id", stokkartTalimat.talimatturu_id);
                        fields.Add("fasoncu_carikart_id", stokkartTalimat.fasoncu_carikart_id);
                        fields.Add("aciklama", stokkartTalimat.aciklama);
                        fields.Add("irstalimat", stokkartTalimat.irstalimat);
                        fields.Add("islem_sayisi", stokkartTalimat.islem_sayisi);

                        acekaResult = CrudRepository.Update("stokkart_talimat", "stokkart_id = " + stokkartTalimat.stokkart_id + " AND sira_id = " + stokkartTalimat.eski_sira_id, fields, true);

                        if (acekaResult != null && acekaResult.ErrorInfo == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful", ret_val = acekaResult.RetVal.ToString() });
                        }
                        else
                        {
                            if (acekaResult.ErrorInfo != null)
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "No Record!" });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process. Please check input areas!" });
            }
        }

        /// <summary>
        /// Modelkart - > Tab - > Talimatlar DELETE Metod.
        /// </summary>
        /// <param name="stokkartTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/modelkart/talimatlar")]
        [Route("api/modelkart/talimatlar")]
        [HttpDelete]
        public HttpResponseMessage TalimatlarDelete(StokkartTalimat stokkartTalimat)
        {
            AcekaResult acekaResult = null;

            if (stokkartTalimat != null && stokkartTalimat.stokkart_id > 0 && stokkartTalimat.sira_id > 0)
            {
                stokKartRepository = new StokkartRepository();

                stokkart_talimat talimatDetay = stokKartRepository.StokkartTalimatDetay(stokkartTalimat.stokkart_id, stokkartTalimat.sira_id);

                if (talimatDetay != null)
                {

                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("sira_id", stokkartTalimat.sira_id);
                    fields.Add("stokkart_id", stokkartTalimat.stokkart_id);

                    acekaResult = CrudRepository.Delete("stokkart_talimat", new string[] { "sira_id", "stokkart_id" }, fields);

                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful", ret_val = acekaResult.RetVal.ToString() });
                    }
                    else
                    {
                        if (acekaResult.ErrorInfo != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process. Please check input areas!" });
            }
        }
        #endregion
        #region Ölçüler

        /// <summary>
        /// Modelkart -> Tab -> Ölçüler - tanımlı beden listesi (Varyant ile ilişkili) GET metod 
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///         beden_id: 12,
        ///         beden: "S",
        ///         bedengrubu: "A"
        ///     },
        ///     {
        ///         beden_id: 9,
        ///         beden: "M",
        ///         bedengrubu: "A"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer/bedenler")]
        [Route("api/modelkart/olculer/bedenler/{stokkart_id}")]
        public HttpResponseMessage OlculerBedenlerGet(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var olcuBedenleri = stokKartRepository.StokkartModelOlcuBedenleri(stokkart_id, ref errorMessage);
                if (olcuBedenleri != null && olcuBedenleri.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        olcuBedenleri.Select(ob => new
                        {
                            ob.beden_id,
                            ob.beden,
                            ob.bedengrubu,
                            ob.sira
                        })

                        );
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Model ya da stokkart a ait bedenlere ait ölçülerin getirildiği pivot tablew
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer")]
        [Route("api/modelkart/olculer-pivot/{stokkart_id}")]
        public IHttpActionResult StokkartPivotOlcuListesiniGetir(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var dt = stokKartRepository.StokkartPivotOlcuListesiniGetir(stokkart_id);
            var result = new List<StokkartOlcu>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var item = new StokkartOlcu();
                item.beden_id = dt.Rows[i]["beden_id"].acekaToShort();
                item.beden_adi = dt.Rows[i]["beden_tanimi"].acekaToString();
                item.birim_id = dt.Rows[i]["birim_id"].acekaToByte();
                item.birim_adi = dt.Rows[i]["birim_adi"].acekaToString();
                item.olcuyeri = dt.Rows[i]["olcuyeri"].ToString();
                item.olcu_id = dt.Rows[i]["olcu_id"].acekaToShort();
                item.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                item.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                item.deger = dt.Rows[i]["deger"].acekaToFloat();
                item.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                result.Add(item);
            }

            List<StokkartOlculerPivotModel> pivot = new List<StokkartOlculerPivotModel>();
            var notExistData = result.Where(x => x.olcu_id == 0).ToList();

            foreach (var item in result)
            {
                if (pivot.Any(x => x.stokkart_id == item.stokkart_id && x.olcuyeri == item.olcuyeri))
                {
                    pivot.Where(x => x.stokkart_id == item.stokkart_id && x.olcuyeri == item.olcuyeri).First().pivotMatrixData.Add(
                    new StokkartIlkmaddePivotMatrix
                    {
                        id = item.beden_id,
                        olcu_id = item.olcu_id,
                        name = item.beden_adi,
                        value = item.deger
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.olcuyeri))
                    {

                        var pivotItems = new List<StokkartIlkmaddePivotMatrix>
                        {
                            new StokkartIlkmaddePivotMatrix
                            {
                                id = item.beden_id,
                                olcu_id = item.olcu_id,
                                name = item.beden_id.ToString(),
                                value = item.deger
                            }
                        };
                        // öceden var olmayan bir beden eklenmişse datası oluşturuluyor update tarafında değeri dbye eklenemez çok küçük bir ihtimal olduğu için dikkaate alınmıyor.
                        if (notExistData.Count > 0)
                        {
                            notExistData.ForEach(x =>
                            {
                                pivotItems.Add(new StokkartIlkmaddePivotMatrix
                                {
                                    id = x.beden_id,
                                    olcu_id = x.olcu_id,
                                    name = x.beden_id.ToString(),
                                    value = x.deger
                                });
                            });
                        }

                        pivot.Add(new StokkartOlculerPivotModel
                        {
                            birim_adi = item.birim_adi,
                            birim_id = item.birim_id,
                            olcuyeri = item.olcuyeri,
                            stokkart_id = item.stokkart_id,
                            pivotMatrixData = pivotItems
                        });
                    }
                }
            }

            return Ok(pivot);

            /*
            if (modeler != null && modeler.Rows.Count > 0 && string.IsNullOrEmpty(errorMessage))
            {
                List<StokkartOlculerPivotModel> pivot = new List<StokkartOlculerPivotModel>();
                StokkartOlculerPivotModel item = null;

                for (int i = 0; i < modeler.Rows.Count; i++)
                {
                    item = new StokkartOlculerPivotModel
                    {
                        //olcu_id = modeler.Rows[i]["olcu_id"].acekaToInt(),
                        stokkart_id = modeler.Rows[i]["stokkart_id"].acekaToLong(),
                        olcuyeri = modeler.Rows[i]["olcuyeri"].acekaToString(),
                        birim_adi = modeler.Rows[i]["birim_adi"].acekaToString(),
                        birim_id = modeler.Rows[i]["birim_id"].acekaToByte()
                    };

                    item.pivotMatrixData = new List<StokkartIlkmaddePivotMatrix>();
                    //sabit kolon sayısı 3 olduğundan geri kalan dinamik kolanlar için döngü 4 ten başlıyor
                    for (int x = 3; x < modeler.Columns.Count; x++)
                    {
                        var pivotNameData = modeler.Columns[x].ColumnName.Split(' ');

                        if (modeler.Columns[x].ColumnName == "birim_adi")
                            continue;

                        item.pivotMatrixData.Add(new StokkartIlkmaddePivotMatrix
                        {
                            id = pivotNameData[0].acekaToShort(),
                            name = modeler.Columns[x].ColumnName,
                            value = modeler.Rows[i][modeler.Columns[x].ColumnName].acekaToFloat()
                        });
                    }
                    pivot.Add(item);
                    item = null;
                }
                var tt = modeler.Columns.Count;
                return Request.CreateResponse(HttpStatusCode.OK, pivot);
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                }

            }*/

        }


        /// <summary>
        /// Modelkart -> Tab -> Ölçüler  GET metod (Liste)
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///         stokkart_id: 1,
        ///         olcuyeri: "atolye",
        ///         beden_id: 1,
        ///         deger: "1",
        ///         birim: "1",
        ///         "sira_id": 1 //eski beden_id ye ulaşmak çin eklendi.
        ///     },
        ///     {
        ///         stokkart_id: 1,
        ///         olcuyeri: "atolye",
        ///         beden_id: 2,
        ///         deger: "1",
        ///         birim: "1",
        ///         "sira_id": 2
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer")]
        [Route("api/modelkart/olculer/{stokkart_id}")]
        public HttpResponseMessage OlculerGet(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var olculer = stokKartRepository.StokkartModelOlcuListesi(stokkart_id, ref errorMessage);
                if (olculer != null && olculer.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        olculer.Select(olcu => new
                        {
                            olcu.olcu_id,
                            olcu.stokkart_id,
                            olcu.olcuyeri,
                            olcu.beden_id,
                            olcu.deger,
                            olcu.birim_id,
                            sira_id = olcu.beden_id,
                            olcu.parametrebeden.beden_tanimi,
                        })
                        );
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Modelkart -> Tab -> Ölçüler  POST metod
        /// NOT: Objedeki bütün alanlar dolu gelmelidir
        /// </summary>
        /// <param name="stokkartOlcu"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer")]
        [Route("api/modelkart/olculer")]
        public HttpResponseMessage OlculerPost(StokkartOlculerPivotModel stokkartOlcu)
        {
            AcekaResult acekaResult = null;

            if (stokkartOlcu != null
                && stokkartOlcu.stokkart_id > 0
                && !string.IsNullOrEmpty(stokkartOlcu.olcuyeri.Trim())
                && stokkartOlcu.birim_id > 0
                )
            {
                for (int i = 0; i < stokkartOlcu.pivotMatrixData.Count; i++)
                {
                    var model = new stokkart_olcu
                    {
                        stokkart_id = stokkartOlcu.stokkart_id,
                        beden_id = stokkartOlcu.pivotMatrixData[i].id.acekaToShort(),
                        olcuyeri = stokkartOlcu.olcuyeri,
                        birim_id = stokkartOlcu.birim_id,
                        deger = stokkartOlcu.pivotMatrixData[i].value.acekaToFloat(),
                        degistiren_carikart_id = Tools.PersonelId,
                        degistiren_tarih = DateTime.Now,
                    };

                    acekaResult = CrudRepository<stokkart_olcu>.Insert(model, new string[] { "olcu_id" });

                    if (acekaResult == null || acekaResult.ErrorInfo != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Eksik ya da hatalı alanlar var! Lütfen kontrol edip yeniden kayıt işlemini deneyin!" });
            }
        }

        /// <summary>
        /// Modelkart -> Tab -> Ölçüler  PUT metod
        /// NOT: Bu metod henüz yapılmadı!
        /// </summary>
        /// <param name="stokkartOlcu"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer")]
        [Route("api/modelkart/olculer")]
        public HttpResponseMessage OlculerPut(StokkartOlculerPivotModel stokkartOlcu)
        {
            AcekaResult acekaResult = null;

            if (stokkartOlcu != null
                && stokkartOlcu.stokkart_id > 0
                && !string.IsNullOrEmpty(stokkartOlcu.olcuyeri.Trim())
                )
            {
                stokKartRepository = new StokkartRepository();

                for (int i = 0; i < stokkartOlcu.pivotMatrixData.Count; i++)
                {
                    var model = new stokkart_olcu
                    {
                        olcu_id = stokkartOlcu.pivotMatrixData[i].olcu_id.acekaToInt(),
                        stokkart_id = stokkartOlcu.stokkart_id,
                        beden_id = stokkartOlcu.pivotMatrixData[i].id.acekaToShort(),
                        olcuyeri = stokkartOlcu.olcuyeri,
                        birim_id = stokkartOlcu.birim_id,
                        deger = stokkartOlcu.pivotMatrixData[i].value.acekaToFloat(),
                        degistiren_carikart_id = Tools.PersonelId,
                        degistiren_tarih = DateTime.Now,
                    };

                    acekaResult = CrudRepository<stokkart_olcu>.Update(model, new string[] { "olcu_id" }, new string[] { "stokkart_id", "beden_id" });

                    if (acekaResult == null || acekaResult.ErrorInfo != null)
                    {
                        errorMessage = "A problem has been occurred during the process.";
                        if (!string.IsNullOrEmpty(acekaResult.ErrorInfo.Message))
                        {
                            errorMessage = acekaResult.ErrorInfo.Message;

                            if (errorMessage.Contains("duplicate"))
                            {
                                errorMessage = "Ölçü eklenirken hata oluştu!";
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Eksik ya da hatalı alanlar var! Lütfen kontrol edip yeniden kayıt işlemini deneyin!" });
            }
        }

        /// <summary>
        /// Modelkart -> Tab -> Ölçüler DELETE metod
        /// NOT : silmeişlemi için "stokkart_id", "beden_id" ve "olcuyeri" alanları gönderilmelidir!
        /// </summary>
        /// <param name="stokkartOlcu"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/modelkart/olculer")]
        [Route("api/modelkart/olculer")]
        public HttpResponseMessage OlculerDelete(StokkartOlculerPivotModel stokkartOlcu)
        {
            AcekaResult acekaResult = null;

            if (stokkartOlcu != null
                && stokkartOlcu.stokkart_id > 0
                && !string.IsNullOrEmpty(stokkartOlcu.olcuyeri.Trim())
                )
            {
                var model = new stokkart_olcu
                {
                    stokkart_id = stokkartOlcu.stokkart_id,
                    olcuyeri = stokkartOlcu.olcuyeri,
                };

                acekaResult = CrudRepository<stokkart_olcu>.Delete(model, "stokkart_olcu", new string[] { "stokkart_id", "olcuyeri" }, new string[] { "stokkart_id", "olcuyeri" });
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
                }
                else
                {
                    errorMessage = "A problem has been occurred during the process.";
                    if (!string.IsNullOrEmpty(acekaResult.ErrorInfo.Message))
                    {
                        errorMessage = acekaResult.ErrorInfo.Message;
                    }

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Eksik ya da hatalı alanlar var! Lütfen kontrol edip yeniden kayıt işlemini deneyin!" });
            }
        }
        #endregion
        #region Fiyat
        /// <summary>
        ///  Modelkart - > Tab - > Fiyatlar -> Üst Fiyat tablosu GET metod. 
        ///  Not: stokkart fiyat talimat ve talimat tablolarından gelen kayıtlar.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///     stokkart_id: 4,
        ///     tarih: "2017-03-27T00:00:00",
        ///     talimat: "KESİM+YIKAMA+DİKİM",
        ///     fiyat: 4.5,
        ///     pb: 1,
        ///     pb_kodu: "$"
        ///     },
        ///     {
        ///     stokkart_id: 4,
        ///     tarih: "2017-03-27T00:00:00",
        ///     talimat: "KESİM+YIKAMA+DİKİM",
        ///     fiyat: 4.5,
        ///     pb: 1,
        ///     pb_kodu: "$"
        ///     }
        /// ] 
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/fiyat-talimatlar")]
        [Route("api/modelkart/fiyat-talimatlar/{stokkart_id}")]
        public HttpResponseMessage FiyatTalimatlarGet(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var talimatlar = stokKartRepository.StokkartFiyatTalimatlariGetir(stokkart_id, ref errorMessage);

                if (talimatlar != null && talimatlar.Count > 0 && string.IsNullOrEmpty(errorMessage))
                {

                    var groupByTalimatlar = talimatlar.GroupBy(c => new { c.stokkart_id, c.tarih, c.pb, c.parabirimi.pb_kodu, c.fiyat })
                                    .Select(g => new
                                    {
                                        g.Key.stokkart_id,
                                        tarih = g.Key.tarih,
                                        talimat = string.Join("+", g.Select(t => t.talimat).ToList().Select(t => t.tanim)),
                                        g.Key.fiyat,
                                        g.Key.pb,
                                        g.Key.pb_kodu
                                    })
                                    .OrderBy(c => c.tarih);


                    return Request.CreateResponse(HttpStatusCode.OK, groupByTalimatlar);

                    //return Request.CreateResponse(HttpStatusCode.OK,
                    //    talimatlar.Select(skTalimat => new
                    //    {
                    //        skTalimat.stokkart_id,
                    //        skTalimat.talimat.talimatturu_id,
                    //        skTalimat.tarih,
                    //        skTalimat.carikart_id,
                    //        skTalimat.fiyat,
                    //        skTalimat.pb,
                    //        skTalimat.parabirimi.pb_kodu,
                    //        skTalimat.talimat.kod,
                    //        skTalimat.talimat.varsayilan,
                    //        skTalimat.talimat.tanim,
                    //        skTalimat.talimat.sira,
                    //        skTalimat.talimat.renk_rgb,
                    //        skTalimat.talimat.kesim,
                    //        skTalimat.talimat.dikim,
                    //        skTalimat.talimat.parca,
                    //        skTalimat.talimat.model,
                    //        skTalimat.talimat.stokkart_tipi_id,
                    //        skTalimat.talimat.onayoto,
                    //        skTalimat.talimat.parcamodel_giris,
                    //        skTalimat.talimat.parcamodel_cikis,
                    //        skTalimat.talimat.model_zorunlu,
                    //        skTalimat.talimat.varsayilan_fasoncu,
                    //        skTalimat.talimat.kdv_tevkifat
                    //    })
                    //    );
                }
                else
                {

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessage = "No Recors!";
                    }

                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = errorMessage });

                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "stokkart_id can't be blank!" });
            }

        }

        /// <summary>
        /// Modelkart -> Tab ->  Fiyatlar -> Alt Fiyat tablosu GET metod.
        /// Sadece GET Metodu olacak.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///     stokkart_id: 1,
        ///      fiyattipi: "AF",
        ///      tarih: "2017-03-25T00:00:00",
        ///      fiyat: 200,
        ///      fiyattipi_adi: "Sabit Alış Fiyatı",
        ///      pb_kodu: "TL"
        ///  },
        ///  {
        ///    "stokkart_id": 1,
        ///    fiyattipi: "SF",
        ///    tarih: "2017-02-28T00:00:00",
        ///     fiyat: 68,
        ///     fiyattipi_adi: "Sabit Satış Fiyatı",
        ///     pb_kodu: "TL"
        ///  }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/modelkart/fiyat")]
        [Route("api/modelkart/fiyat/{stokkart_id}")]
        public HttpResponseMessage FiyatGet(long stokkart_id)
        {
            if (stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var stokfiyat = stokKartRepository.StokkartFiyat(stokkart_id);
                if (stokfiyat != null && stokfiyat.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        stokfiyat.Select(sf => new
                        {
                            sf.stokkart_id,
                            sf.fiyattipi,
                            sf.tarih,
                            sf.fiyat,
                            sf.fiyattipi_adi,
                            sf.pb_kodu
                        })

                        );
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }
        #endregion
        #endregion
    }
}