using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
using aceka.web_api.Models.CarikartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Carş işlemlerine ait metodlar
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CariController : ApiController
    {

        #region Degiskenler
        private cari_kart cari = null;
        private List<cari_kart> carikartlar = null;
        private CarikartRepository carikartRepository = null;
        private carikart_denetim_aksesuar carikartdenetimaksesuar = null;
        private List<carikart_denetim_aksesuar> denetimaksesuar = null;
        private List<carikart_denetim_aksesuar_kosullar> carikartDenetimKosul = null;
        private List<Muhasebe.muhasebe_tanim_hesapkodlari> muhkodlar;
        #endregion

        #region Müşteri-Firma/Bayi Listesi
        /// <summary>
        /// Select için kullanılacak olan, müşteriler listesi
        /// </summary>
        /// <returns>
        /// [
        ///     {
        ///     carikart_id: 100000002099,
        ///     cari_unvan: "NIKE INC",
        ///     cari_unvan_kucuk: "nıke ınc"
        ///     },
        ///     {
        ///     carikart_id: 100120000009,
        ///     cari_unvan: "Puma",
        ///     cari_unvan_kucuk: "puma"
        ///     },
        ///     {
        ///     carikart_id: 100000000002,
        ///     cari_unvan: "PUMA SE RUDOLF DASSLER SPORT , ACCOUNTING",
        ///     cari_unvan_kucuk: "puma se rudolf dassler sport , accountıng"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/musteri-listesi")]
        public HttpResponseMessage MusteriListe()
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.CariListeMusteri();

            if (carikartlar != null)
            {
                var ozet = carikartlar.Select(o => new
                {
                    carikart_id = o.carikart_id,
                    cari_unvan = o.cari_unvan,
                    cari_unvan_kucuk = o.cari_unvan.ToLower()
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Fİrma/Bayi listesi 
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///      "carikart_id": 100000000913,
        ///      "cari_unvan": "Almanya",
        ///      "cari_unvan_kucuk": "almanya"
        /// },
        /// {
        ///     "carikart_id": 100000000914,
        ///     "cari_unvan": "Amerika",
        ///     "cari_unvan_kucuk": "amerika"
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/firma-bayi-listesi")]
        public HttpResponseMessage FirmaListe()
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.FirmaBayiListesi();

            if (carikartlar != null)
            {
                var ozet = carikartlar.Select(o => new
                {
                    carikart_id = o.carikart_id,
                    cari_unvan = o.cari_unvan,
                    cari_unvan_kucuk = o.cari_unvan.ToLower()
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        #endregion

        #region Carikart detay, arama metodları

        /// <summary>
        /// Cari detayını getirir
        /// </summary>
        /// <param name="id"></param>
        /// <returns> http://prntscr.com/e2lq1l
        /// {
        ///     carikart_id: 100000000947,
        ///     cari_unvan: "ÖZDEMİRTEKS TRİKO ÖRME SAN.VE TİC.LTD.ŞTİ.",
        ///     carikart_turu_id: 1,
        ///     carikart_tipi_id: 13,
        ///     ozel_kod: "",
        ///     fiyattipi: "PSF",
        ///     giz_yazilim_kodu: 0,
        ///     transfer_depo_id: 0,
        ///     statu: true,
        ///     ana_carikart_id: 100000000952,
        ///     finans_sorumlu_carikart_id: 100000002375,
        ///     satin_alma_sorumlu_carikart_id: 100000002375,
        ///     satis_sorumlu_carikart_id: 100000002375,
        ///     sube_carikart_id: 0
        /// }
        /// </returns>
        [CustAuthFilter]
        public HttpResponseMessage Get(long id)
        {
            carikartRepository = new CarikartRepository();
            cari = carikartRepository.Getir(id);
            if (cari != null)
            {
                var cariOzet = new
                {
                    carikart_id = cari.carikart_id,
                    cari_unvan = cari.cari_unvan,
                    carikart_turu_id = cari.carikart_turu_id,
                    carikart_tipi_id = cari.carikart_tipi_id,

                    ozel_kod = cari.ozel_kod,
                    fiyattipi = cari.fiyattipi,
                    //giz_yazilim_kodu = cari.giz_yazilim_kodu,
                    transfer_depo_id = cari.transfer_depo_id,
                    statu = cari.statu,
                    sube_carikart_id = cari.sube_carikart_id,
                    sube_cari_unvan = cari.sube_cari_unvan,
                    cari.ana_carikart_id,
                    cari.ana_cari_unvan,
                    cari.carikart_finans.finans_sorumlu_carikart_id,
                    cari.carikart_finans.finans_sorumlu_cari_unvan,
                    cari.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    cari.carikart_firma_ozel.satin_alma_sorumlu_cari_unvan,
                    cari.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    cari.carikart_firma_ozel.satis_sorumlu_cari_unvan,

                };
                return Request.CreateResponse(HttpStatusCode.OK, cariOzet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }
        /// <summary>
        /// carikart_id boş ise insert, dolu ise update işlemi çalışır!
        /// </summary>
        /// <param name="carikart"></param>
        /// <returns></returns>
        /// POST : api/cari
        [CustAuthFilter]
        public HttpResponseMessage Post(Carikart carikart)
        {
            AcekaResult acekaResult = null;
            if (carikart != null)
            {
                carikart.degistiren_tarih = DateTime.Now;
                if (carikart.carikart_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", carikart.carikart_id);
                    fields.Add("degistiren_tarih", carikart.degistiren_tarih);
                    fields.Add("degistiren_carikart_id", carikart.degistiren_carikart_id);
                    fields.Add("statu", carikart.statu);
                    fields.Add("carikart_turu_id", carikart.carikart_turu_id);
                    fields.Add("carikart_tipi_id", carikart.carikart_tipi_id);
                    fields.Add("cari_unvan", carikart.cari_unvan);
                    fields.Add("ozel_kod", carikart.ozel_kod);
                    fields.Add("fiyattipi", carikart.fiyattipi);
                    fields.Add("ana_carikart_id", carikart.ana_carikart_id);
                    fields.Add("transfer_depo_id", carikart.transfer_depo_id);
                    fields.Add("sube_carikart_id", carikart.sube_carikart_id);
                    //Carikart Rapor Parametre Sekmesi Değerleri. carikart_rapor_parametre tablosuna taşındı.
                    //fields.Add("cari_parametre_1", carikart.cari_parametre_1);
                    //fields.Add("cari_parametre_2", carikart.cari_parametre_2);
                    //fields.Add("cari_parametre_3", carikart.cari_parametre_3);
                    //fields.Add("cari_parametre_4", carikart.cari_parametre_4);
                    //fields.Add("cari_parametre_5", carikart.cari_parametre_5);
                    //fields.Add("cari_parametre_6", carikart.cari_parametre_6);
                    //fields.Add("cari_parametre_7", carikart.cari_parametre_7);

                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                }
                else
                {
                    acekaResult = CrudRepository<Carikart>.Insert(carikart, "carikart", new string[] { "carikart_id", "finans_sorumlu_carikart_id", "satin_alma_sorumlu_carikart_id", "satis_sorumlu_carikart_id", "ilgili_sube_carikart_id", "cari_parametre_1", "cari_parametre_2", "cari_parametre_3", "cari_parametre_4", "cari_parametre_5", "cari_parametre_6", "cari_parametre_7" });

                    carikart.carikart_id = Convert.ToInt64(acekaResult.RetVal);


                }

                if (acekaResult.ErrorInfo == null && acekaResult.RetVal != null)
                {
                    carikartRepository = new CarikartRepository();

                    #region carikart_rapor_parametre
                    if (carikart.carikart_id > 0)
                    {
                        carikart_rapor_parametre cariRaporParametre = carikartRepository.CarikartParametreleriniGetir(carikart.carikart_id);
                        if (cariRaporParametre == null)
                        {
                            //insert
                            cariRaporParametre = new carikart_rapor_parametre();
                            cariRaporParametre.carikart_id = carikart.carikart_id;
                            cariRaporParametre.degistiren_carikart_id = -1;
                            cariRaporParametre.degistiren_tarih = DateTime.Now;
                            cariRaporParametre.cari_parametre_1 = carikart.cari_parametre_1;
                            cariRaporParametre.cari_parametre_2 = carikart.cari_parametre_2;
                            cariRaporParametre.cari_parametre_3 = carikart.cari_parametre_3;
                            cariRaporParametre.cari_parametre_4 = carikart.cari_parametre_4;
                            cariRaporParametre.cari_parametre_5 = carikart.cari_parametre_5;
                            cariRaporParametre.cari_parametre_6 = carikart.cari_parametre_6;
                            cariRaporParametre.cari_parametre_7 = carikart.cari_parametre_7;



                            var finansRet = CrudRepository<carikart_rapor_parametre>.Insert(cariRaporParametre, new string[] { "ilgili_sube_cari_unvan", "finans_sorumlu_cari_unvan", "cari_hesapta_ciksin" });
                        }
                        else
                        {
                            //update
                            //Dictionary<string, object> finansfields = new Dictionary<string, object>();
                            //finansfields.Add("carikart_id", carikart.carikart_id);
                            //finansfields.Add("degistiren_carikart_id", -1);
                            //finansfields.Add("degistiren_tarih", DateTime.Now);
                            //finansfields.Add("finans_sorumlu_carikart_id", carikart.finans_sorumlu_carikart_id);

                            //var finansRet = CrudRepository.Update("carikart_finans", "carikart_id", finansfields);
                        }
                    }
                    #endregion

                    #region carikart_finas
                    if (carikart.finans_sorumlu_carikart_id > 0)
                    {
                        carikart_finans cariFinans = carikartRepository.CarikartFinansBilgileriniGetir(carikart.carikart_id);
                        if (cariFinans == null)
                        {
                            //insert
                            cariFinans = new carikart_finans();
                            cariFinans.carikart_id = carikart.carikart_id;
                            cariFinans.degistiren_carikart_id = -1;
                            cariFinans.degistiren_tarih = DateTime.Now;
                            cariFinans.ilgili_sube_carikart_id = carikart.ilgili_sube_carikart_id;
                            //cariFinans.cari_hesapta_ciksin = true;
                            cariFinans.finans_sorumlu_carikart_id = carikart.finans_sorumlu_carikart_id;

                            var finansRet = CrudRepository<carikart_finans>.Insert(cariFinans, new string[] { "ilgili_sube_cari_unvan", "finans_sorumlu_cari_unvan", "cari_hesapta_ciksin" });
                        }
                        else
                        {
                            //update
                            Dictionary<string, object> finansfields = new Dictionary<string, object>();
                            finansfields.Add("carikart_id", carikart.carikart_id);
                            finansfields.Add("degistiren_carikart_id", -1);
                            finansfields.Add("degistiren_tarih", DateTime.Now);
                            finansfields.Add("finans_sorumlu_carikart_id", carikart.finans_sorumlu_carikart_id);

                            var finansRet = CrudRepository.Update("carikart_finans", "carikart_id", finansfields);
                        }
                    }
                    #endregion

                    #region carikart_firma_ozel Tablosu
                    if ((carikart.satin_alma_sorumlu_carikart_id != null && carikart.satin_alma_sorumlu_carikart_id > 0)
               || (carikart.satis_sorumlu_carikart_id != null && carikart.satis_sorumlu_carikart_id > 0))
                    {
                        carikart_firma_ozel firmaOzel = carikartRepository.CarikartOzelalanlarGetir(carikart.carikart_id);

                        if (firmaOzel == null)
                        {
                            //insert
                            firmaOzel = new carikart_firma_ozel();
                            firmaOzel.carikart_id = carikart.carikart_id;
                            firmaOzel.satin_alma_sorumlu_carikart_id = carikart.satin_alma_sorumlu_carikart_id;
                            firmaOzel.satis_sorumlu_carikart_id = carikart.satis_sorumlu_carikart_id;
                            firmaOzel.degistiren_carikart_id = -1;
                            firmaOzel.degistiren_tarih = DateTime.Now;
                            firmaOzel.baslamatarihi = DateTime.Now;

                            var ozelKodRet = CrudRepository<carikart_firma_ozel>.Insert(firmaOzel, new string[] { "satin_alma_sorumlu_cari_unvan", "satis_sorumlu_cari_unvan" });
                        }
                        else
                        {
                            //update

                            Dictionary<string, object> ozelKodfields = new Dictionary<string, object>();
                            ozelKodfields.Add("carikart_id", carikart.carikart_id);
                            ozelKodfields.Add("satin_alma_sorumlu_carikart_id", carikart.satin_alma_sorumlu_carikart_id);
                            ozelKodfields.Add("satis_sorumlu_carikart_id", carikart.satis_sorumlu_carikart_id);
                            ozelKodfields.Add("degistiren_carikart_id", -1);
                            ozelKodfields.Add("degistiren_tarih", DateTime.Now);
                            ozelKodfields.Add("baslamatarihi", firmaOzel.baslamatarihi);

                            var ozelKodRet = CrudRepository.Update("carikart_firma_ozel", "carikart_id", ozelKodfields);
                        }
                    }
                    #endregion

                    //return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = carikart.carikart_id.ToString() });
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
        ///  Carikart arama metodu
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <param name="cari_unvan"></param>
        /// <param name="ozel_kod"></param>
        /// <returns>
        /// [
        ///   {
        ///  "carikart_id": 100000000001,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "2017-02-09T16:14:20",
        ///  "kayit_silindi": false,
        ///  "kayit_yeri": 0,
        ///  "statu": true,
        ///  "carikart_turu_id": 1,
        ///  "carikart_tipi_id": 11,
        ///  "cari_unvan": "LOTTO SPORT ITALIA S.P.A.",
        ///  "ozel_kod": "",
        ///  "fiyattipi": "",
        ///  "fiyattipi_adi": null,
        ///  "giz_yazilim_kodu": 0,
        ///  "sube_carikart_id": 0,
        ///  "ana_carikart_id": 100000000947,
        ///  "ana_cari_unvan": null,
        ///  "transfer_depo_id": 0,
        ///  "giz_kullanici_adi": "",
        ///  "giz_kullanici_sifre": "",
        ///  "cari_parametre_1": 0,
        ///  "cari_parametre_2": 0,
        ///  "cari_parametre_3": 0,
        ///  "cari_parametre_4": 0,
        ///  "cari_parametre_5": 0,
        ///  "cari_parametre_6": 0,
        ///  "cari_parametre_7": 0,
        ///  "carikart_genel_adres": null,
        ///  "carikart_finans": null,
        ///  "carikart_firma_ozel": null,
        ///  "giz_sabit_carikart_tipi": null,
        ///  "giz_sabit_carikart_turu": null,
        ///  "carikart_earsiv": null,
        ///  "carikart_efatura": null,
        ///  "carikart_muhasebe": null,
        ///  "muhasebe_tanim_masrafmerkezleri": null,
        ///  "carikart_stokyeri": null,
        ///  "giz_sirket": null
        ///}
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-bul")]
        public IList<cari_kart> CarikartAra(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "")
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.Bul(carikart_id, cari_unvan, ozel_kod);
            return carikartlar;
        }

        /// <summary>
        ///  Carikart arama metodu (özet bilgileri getirir)
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <param name="cari_unvan"></param>
        /// <param name="ozel_kod"></param>
        /// <param name="carikart_tipi_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///    {
        ///        "carikodu": 100000000002,
        ///        "unvan": "PUMA SE RUDOLF DASSLER SPORT , ACCOUNTING",
        ///        "ozel_kod": "",
        ///        "giz_kodu": 0,
        ///        "statu": "Aktif",
        ///        "cari_tipi": 11,
        ///        "cari_tipi_adi": "Alıcı",
        ///        "cari_turu": 1,
        ///        "cari_turu_adi": "Cari",
        ///        "fiyattipi": "",
        ///        "adres": "",
        ///        "telefon": "",
        ///        "email": "",
        ///        "websitesi": "",
        ///        "para_birimi": ""
        ///    }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-bul-ozet")]
        public HttpResponseMessage CarikartAraOzet(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", byte carikart_tipi_id = 0)
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.BulOzet(carikart_id, cari_unvan, ozel_kod, carikart_tipi_id);
            if (carikartlar != null)
            {
                var ozet = carikartlar.Select(o => new
                {
                    carikart_id = o.carikart_id,
                    cari_unvan = o.cari_unvan,
                    o.ozel_kod,
                    giz_kodu = o.giz_yazilim_kodu,
                    statu = o.statu,
                    cari_tipi_id = o.giz_sabit_carikart_tipi.carikart_tipi_id,
                    cari_tipi_adi = o.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    cari_turu = o.giz_sabit_carikart_turu.carikart_turu_id,
                    cari_turu_adi = o.giz_sabit_carikart_turu.carikart_turu_adi,
                    o.fiyattipi,
                    adres = o.carikart_genel_adres[0].adres,
                    telefon = o.carikart_genel_adres[0].tel1,
                    email = o.carikart_genel_adres[0].email,
                    websitesi = o.carikart_genel_adres[0].websitesi,
                    para_birimi = o.carikart_finans.pb,
                    //25.01.2017 ilave edildi. AA
                    //parametre_cari_odeme_sekli tablosunda "cari_odeme_sekli_id" olan field bu tabloda "odeme_tipi" olarak tutuluyor                    
                    odeme_sekli_id = o.carikart_finans.odeme_tipi,

                    //İlgili Şube Carikart
                    ilgili_sube_id = o.carikart_finans.ilgili_sube_carikart_id,

                    //ilgili_sube = (o.carikart_finans.carikart_id > 0 ? new
                    //{
                    //    carikart_id = o.carikart_finans.ilgili_sube_carikart_id,
                    //    cari_unvan = o.carikart_finans.ilgili_sube_cari_unvan
                    //} : null),
                    //Finans Sorumlu
                    finans_sorumlu_carikart_id = o.carikart_finans.finans_sorumlu_carikart_id,
                    //finans_sorumlu = (o.carikart_finans.finans_sorumlu_carikart_id > 0 ? new
                    //{
                    //    carikart_id = o.carikart_finans.finans_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_finans.finans_sorumlu_cari_unvan
                    //} : null),

                    //Satın Alma ve Satış Sorumluları
                    satin_alma_sorumlu_carikart_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //satin_alma_sorumlusu = (o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id > 0 ? new
                    //{
                    //    satin_alma_sorumlu_carikart_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_firma_ozel.satin_alma_sorumlu_cari_unvan
                    //} : null),

                    satis_sorumlu_carikart_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //satis_sorumlusu = (o.carikart_firma_ozel.satis_sorumlu_carikart_id > 0 ? new
                    //{
                    //    satis_sorumlu_carikart_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_firma_ozel.satis_sorumlu_cari_unvan
                    //} : null),

                    ana_cari_unvan = o.ana_cari_unvan,
                    ana_carikart_id = o.ana_carikart_id


                    //satis_sorumlu_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //satin_alma_sorumlu_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //finans_sorumlu_id = o.carikart_finans.finans_sorumlu_carikart_id

                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Carikart arama Test metodu
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///  {
        ///    "carikodu": 100000000947,
        ///    "unvan": "ÖZDEMİRTEKS TRİKO ÖRME SAN.VE TİC.LTD.ŞTİ.",
        ///    "ozel_kod": "",
        ///    "giz_kodu": 0,
        ///    "statu": true,
        ///    "cari_tipi": 13,
        ///    "cari_tipi_adi": "Alıcı/Satıcı",
        ///    "cari_turu": 1,
        ///    "cari_turu_adi": "Cari",
        ///    "fiyattipi": "PSF",
        ///    "degistiren_carikart_id": -1,
        ///    "degistiren_tarih": "2017-01-04T18:46:19",
        ///    "kayit_silindi": false,
        ///    "kayit_yeri": 0,
        ///    "transfer_depo_id": 0,
        ///    "giz_kullanici_adi": "",
        ///    "giz_kullanici_sifre": "",
        ///    "adres": "",
        ///    "telefon": "",
        ///    "email": "",
        ///    "websitesi": "",
        ///    "para_birimi": null,
        ///    "odeme_sekli_id": 0,
        ///    "ilgili_sube_id": 0,
        ///    "finans_sorumlu_carikart_id": 100000000100,
        ///    "satin_alma_sorumlu_carikart_id": 100000001060,
        ///    "satis_sorumlu_carikart_id": 100000001060,
        ///    "ana_cari_unvan": "Avery Dennison Tekstil Ürünleri San. ve Tic. Ltd. Şti.",
        ///    "ana_carikart_id": 100000000952
        ///  }
        ///]
        /// </returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-bul-ozet-test")]
        public HttpResponseMessage CarikartAraOzetTest(AramaParameters parameters)
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.BulOzetTest(new cari_kart
            {
                cari_unvan = parameters.unvan,
                carikart_id = parameters.carikart_id,
                ozel_kod = parameters.ozel_kod
            });

            if (carikartlar != null)
            {
                var ozet = carikartlar.Select(o => new
                {
                    carikodu = o.carikart_id,
                    unvan = o.cari_unvan,
                    o.ozel_kod,
                    giz_kodu = o.giz_yazilim_kodu,
                    statu = o.statu,
                    cari_tipi = o.giz_sabit_carikart_tipi.carikart_tipi_id,
                    cari_tipi_adi = o.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    cari_turu = o.giz_sabit_carikart_turu.carikart_turu_id,
                    cari_turu_adi = o.giz_sabit_carikart_turu.carikart_turu_adi,
                    o.fiyattipi,
                    o.degistiren_carikart_id,
                    o.degistiren_tarih,
                    o.kayit_silindi,
                    o.kayit_yeri,
                    o.transfer_depo_id,
                    o.giz_kullanici_adi,
                    o.giz_kullanici_sifre,
                    adres = o.carikart_genel_adres[0].adres,
                    telefon = o.carikart_genel_adres[0].tel1,
                    email = o.carikart_genel_adres[0].email,
                    websitesi = o.carikart_genel_adres[0].websitesi,
                    para_birimi = o.carikart_finans.pb,
                    //25.01.2017 ilave edildi. AA
                    //parametre_cari_odeme_sekli tablosunda "cari_odeme_sekli_id" olan field bu tabloda "odeme_tipi" olarak tutuluyor                    
                    odeme_sekli_id = o.carikart_finans.odeme_tipi,


                    //İlgili Şube Carikart
                    ilgili_sube_id = o.carikart_finans.ilgili_sube_carikart_id,

                    //ilgili_sube = (o.carikart_finans.carikart_id > 0 ? new
                    //{
                    //    carikart_id = o.carikart_finans.ilgili_sube_carikart_id,
                    //    cari_unvan = o.carikart_finans.ilgili_sube_cari_unvan
                    //} : null),
                    //Finans Sorumlu
                    finans_sorumlu_carikart_id = o.carikart_finans.finans_sorumlu_carikart_id,
                    //finans_sorumlu = (o.carikart_finans.finans_sorumlu_carikart_id > 0 ? new
                    //{
                    //    carikart_id = o.carikart_finans.finans_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_finans.finans_sorumlu_cari_unvan
                    //} : null),

                    //Satın Alma ve Satış Sorumluları
                    satin_alma_sorumlu_carikart_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //satin_alma_sorumlusu = (o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id > 0 ? new
                    //{
                    //    satin_alma_sorumlu_carikart_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_firma_ozel.satin_alma_sorumlu_cari_unvan
                    //} : null),

                    satis_sorumlu_carikart_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //satis_sorumlusu = (o.carikart_firma_ozel.satis_sorumlu_carikart_id > 0 ? new
                    //{
                    //    satis_sorumlu_carikart_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //    cari_unvan = o.carikart_firma_ozel.satis_sorumlu_cari_unvan
                    //} : null),

                    ana_cari_unvan = o.ana_cari_unvan,
                    ana_carikart_id = o.ana_carikart_id

                    //satis_sorumlu_id = o.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    //satin_alma_sorumlu_id = o.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    //finans_sorumlu_id = o.carikart_finans.finans_sorumlu_carikart_id

                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Autocomplate için kullanılacak olan, cari türü bayi ve cari olan carikartlar listesi
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///  carikart_id: 100120000065,
        ///  cari_unvan: "",
        ///  cari_unvan_kucuk: ""
        ///  },
        ///  {
        ///  carikart_id: 100120000066,
        ///  cari_unvan: "",
        ///  cari_unvan_kucuk: ""
        ///  }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-liste-turu-bayi-ve-cari")]
        public HttpResponseMessage CariListeTurBayi_veCari()
        {
            carikartRepository = new CarikartRepository();
            carikartlar = carikartRepository.CariListeTurBayi_veCari();

            if (carikartlar != null)
            {
                var ozet = carikartlar.Where(c => c.carikart_turu_id == 1 | c.carikart_turu_id == 4).Select(o => new
                {
                    carikart_id = o.carikart_id,
                    cari_unvan = o.cari_unvan,
                    cari_unvan_kucuk = o.cari_unvan.ToLower()
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Muhasebe Kodu Autocpmplate listesi
        /// </summary>
        /// <returns>
        /// {
        ///  "carikart_id": 1,
        ///  "masraf_merkezi_adi": "1",
        ///  "masraf_merkezi_id": "Dönen Varlıklar",
        ///  "sene": 0,
        ///  "sirket_id": 0,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "0001-01-01T00:00:00"
        /// }
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/muhasebe-kodlari")]
        public HttpResponseMessage CariMuhkodGetir()
        {
            carikartRepository = new CarikartRepository();
            muhkodlar = carikartRepository.MuhasebeKodlariGetir();

            if (muhkodlar != null)
            {

                var ozet = muhkodlar.Select(o => new
                {
                    muh_kod_id = o.muh_kod_id,
                    muh_kod = o.muh_kod,
                    muh_kod_adi = o.muh_kod_adi,
                    //14.02.2017 AA Düzelti.
                    //carikart_id = o.muh_kod_id,
                    //masraf_merkezi_adi = o.muh_kod,
                    //masraf_merkezi_id = o.muh_kod_adi,
                    sene = o.sene,
                    sirket_id = o.sirket_id,
                    degistiren_carikart_id = o.degistiren_carikart_id,
                    degistiren_tarih = o.degistiren_tarih,
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }
        #endregion

        #region Carikart TAB içerikleri

        /// <summary>
        /// Cari Adreslerinin Listesini verir.
        /// Adresler "adres_tipi_id" alanına göre gruplanıyor. FA,IA,IF,II
        /// Cari kart için Fatura Adresi -> FA ve İrsali adresi -> IA alanları ile filtreleme yapıldı.
        /// </summary>
        /// <param name="carikart_id">Cari kart için Fatura Adresi -> FA ve İrsali adresi -> IA alanları ile filtreleme yapıldı.</param>
        /// <returns>
        /// [
        /// {
        /// carikart_id: 100000000947,
        /// iletisim_adres_tipi_id: "FA",
        ///fatura_adresi: {
        ///carikart_adres_id: 100000000947,
        ///degistiren_carikart_id: 0,
        ///adrestanim: "FATURA",
        ///adresunvan: "",
        ///adres: "BAKIRCILAR PİRİNÇÇİLER SAN.TİC.MANOLYA CAD.NO:4 KAT:3 İSTANBUL BEYLİKDÜZÜ",
        ///postakodu: "",
        ///ulke_id: 90,
        ///sehir_id: 34,
        ///ilce_id: 435,
        ///semt_id: 1960,
        ///vergidairesi: "BEYLİKDÜZÜ",
        ///vergino: "681 004 2053",
        ///tel1: "5365556865",
        ///tel2: "",
        ///fax: "",
        ///email: "deneme@gmail.com",
        ///websitesi: "",
        ///yetkili_ad_soyad: "Yetkili adı",
        ///yetkili_tel: "5987568888",
        ///statu: true
        ///},
        ///irsaliye_adresi: {
        ///carikart_adres_id: 100100500007,
        ///degistiren_carikart_id: 0,
        ///adrestanim: "IRSALIYE",
        ///adresunvan: "",
        ///adres: "Beylikdüzü istanbul",
        ///postakodu: "34562",
        ///ulke_id: 90,
        ///sehir_id: 34,
        ///ilce_id: 436,
        ///semt_id: 0,
        ///vergidairesi: "ISTANBUL",
        ///vergino: "7889564",
        ///tel1: null,
        ///tel2: null,
        ///fax: null,
        ///email: null,
        ///websitesi: null,
        ///yetkili_ad_soyad: null,
        ///yetkili_tel: null,
        ///statu: true
        ///}
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-iletisim-bilgileri/{carikart_id}")]
        public HttpResponseMessage CarikartAdresleriniGetir(long carikart_id)
        {

            carikartRepository = new CarikartRepository();
            string iletisim_adres_tipi_id = "";

            var adresler = carikartRepository.CarikartAdresleriniGetir(carikart_id, out iletisim_adres_tipi_id);
            if (adresler != null && adresler.Count > 0)
            {

                Carikart_IletisimBilgileri iletisimBilgileri = new Carikart_IletisimBilgileri();
                iletisimBilgileri.carikart_id = carikart_id;
                iletisimBilgileri.iletisim_adres_tipi_id = iletisim_adres_tipi_id;

                foreach (var item in adresler.Where(a => a.adres_tipi_id == "FA" || a.adres_tipi_id == "IA"))
                {
                    if (item.adres_tipi_id == "FA")
                    {
                        iletisimBilgileri.fatura_adresi = new Adres();
                        iletisimBilgileri.fatura_adresi.carikart_adres_id = item.carikart_adres_id;
                        iletisimBilgileri.fatura_adresi.adres_tipi_id = item.adres_tipi_id;
                        iletisimBilgileri.fatura_adresi.adrestanim = item.adrestanim;
                        iletisimBilgileri.fatura_adresi.adresunvan = item.adresunvan;
                        iletisimBilgileri.fatura_adresi.adres = item.adres;
                        iletisimBilgileri.fatura_adresi.postakodu = item.postakodu;
                        iletisimBilgileri.fatura_adresi.ulke_id = item.ulke_id;
                        iletisimBilgileri.fatura_adresi.sehir_id = item.sehir_id;
                        iletisimBilgileri.fatura_adresi.ilce_id = item.ilce_id;
                        iletisimBilgileri.fatura_adresi.semt_id = item.semt_id;
                        iletisimBilgileri.fatura_adresi.vergidairesi = item.vergidairesi;
                        iletisimBilgileri.fatura_adresi.vergino = item.vergino;
                        iletisimBilgileri.fatura_adresi.tel1 = item.tel1;
                        iletisimBilgileri.fatura_adresi.tel2 = item.tel2;
                        iletisimBilgileri.fatura_adresi.fax = item.fax;
                        iletisimBilgileri.fatura_adresi.email = item.email;
                        iletisimBilgileri.fatura_adresi.websitesi = item.websitesi;
                        iletisimBilgileri.fatura_adresi.yetkili_ad_soyad = item.yetkili_ad_soyad;
                        iletisimBilgileri.fatura_adresi.yetkili_tel = item.yetkili_tel;
                        iletisimBilgileri.fatura_adresi.faturaadresi = item.faturaadresi;
                        iletisimBilgileri.fatura_adresi.statu = item.statu;
                    }
                    else if (item.adres_tipi_id == "IA")
                    {
                        iletisimBilgileri.irsaliye_adresi = new Adres();
                        iletisimBilgileri.irsaliye_adresi.carikart_adres_id = item.carikart_adres_id;
                        iletisimBilgileri.irsaliye_adresi.adres_tipi_id = item.adres_tipi_id;
                        iletisimBilgileri.irsaliye_adresi.adrestanim = item.adrestanim;
                        iletisimBilgileri.irsaliye_adresi.adresunvan = item.adresunvan;
                        iletisimBilgileri.irsaliye_adresi.adres = item.adres;
                        iletisimBilgileri.irsaliye_adresi.postakodu = item.postakodu;
                        iletisimBilgileri.irsaliye_adresi.ulke_id = item.ulke_id;
                        iletisimBilgileri.irsaliye_adresi.sehir_id = item.sehir_id;
                        iletisimBilgileri.irsaliye_adresi.ilce_id = item.ilce_id;
                        iletisimBilgileri.irsaliye_adresi.semt_id = item.semt_id;
                        iletisimBilgileri.irsaliye_adresi.vergidairesi = item.vergidairesi;
                        iletisimBilgileri.irsaliye_adresi.vergino = item.vergino;
                        iletisimBilgileri.irsaliye_adresi.tel1 = item.tel1;
                        iletisimBilgileri.irsaliye_adresi.tel2 = item.tel2;
                        iletisimBilgileri.irsaliye_adresi.fax = item.fax;
                        iletisimBilgileri.irsaliye_adresi.email = item.email;
                        iletisimBilgileri.irsaliye_adresi.websitesi = item.websitesi;
                        iletisimBilgileri.irsaliye_adresi.yetkili_ad_soyad = item.yetkili_ad_soyad;
                        iletisimBilgileri.irsaliye_adresi.yetkili_tel = item.yetkili_tel;
                        iletisimBilgileri.irsaliye_adresi.faturaadresi = item.faturaadresi;
                        iletisimBilgileri.irsaliye_adresi.statu = item.statu;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, iletisimBilgileri);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Carikart iletişim bilgileri Insert, Update
        /// </summary>
        /// <param name="iletisimBilgileri"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-iletisim-bilgileri")]
        public HttpResponseMessage CarikartAdresleriniGetir(Carikart_IletisimBilgileri iletisimBilgileri)
        {
            AcekaResult acekaResult = null;
            if (iletisimBilgileri != null)
            {
                carikartRepository = new CarikartRepository();
                string iletisim_adres_tipi_id = "";
                var adresler = carikartRepository.CarikartAdresleriniGetir(iletisimBilgileri.carikart_id, out iletisim_adres_tipi_id);
                if (adresler != null && adresler.Count > 0)
                {
                    bool faControl = false;
                    bool iaControl = false;
                    #region Carikart "iletisim_adres_tipi_id" alanı güncelleniyor
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("iletisim_adres_tipi_id", iletisimBilgileri.iletisim_adres_tipi_id);
                    fields.Add("carikart_id", iletisimBilgileri.carikart_id);
                    var retCariAdresTipi = CrudRepository.Update("carikart", "carikart_id", fields);
                    #endregion
                    #region Update
                    foreach (var item in adresler)
                    {
                        item.degistiren_tarih = DateTime.Now;
                        switch (item.adres_tipi_id)
                        {
                            case "FA":
                                faControl = true;
                                #region FA Detayları
                                item.carikart_adres_id = iletisimBilgileri.fatura_adresi.carikart_adres_id;
                                item.ulke_id = iletisimBilgileri.fatura_adresi.ulke_id;
                                item.sehir_id = iletisimBilgileri.fatura_adresi.sehir_id;
                                item.ilce_id = iletisimBilgileri.fatura_adresi.ilce_id;
                                item.semt_id = iletisimBilgileri.fatura_adresi.semt_id;
                                item.degistiren_carikart_id = iletisimBilgileri.fatura_adresi.degistiren_carikart_id;
                                item.adrestanim = "FATURA";//iletisimBilgileri.fatura_adresi.adrestanim;
                                item.adresunvan = iletisimBilgileri.fatura_adresi.adresunvan;
                                item.postakodu = iletisimBilgileri.fatura_adresi.postakodu;
                                item.vergidairesi = iletisimBilgileri.fatura_adresi.vergidairesi;
                                item.vergino = iletisimBilgileri.fatura_adresi.vergino;
                                item.faturaadresi = iletisimBilgileri.fatura_adresi.faturaadresi;
                                item.statu = iletisimBilgileri.fatura_adresi.statu;
                                item.adres = iletisimBilgileri.fatura_adresi.adres;
                                item.adres_tipi_id = "FA";

                                if (iletisim_adres_tipi_id == "FA")
                                {
                                    item.tel1 = iletisimBilgileri.fatura_adresi.tel1;
                                    item.tel2 = iletisimBilgileri.fatura_adresi.tel2;
                                    item.fax = iletisimBilgileri.fatura_adresi.fax;
                                    item.email = iletisimBilgileri.fatura_adresi.email;
                                    item.websitesi = iletisimBilgileri.fatura_adresi.websitesi;
                                    item.yetkili_ad_soyad = iletisimBilgileri.fatura_adresi.yetkili_ad_soyad;
                                    item.yetkili_tel = iletisimBilgileri.fatura_adresi.yetkili_tel;
                                }

                                #endregion
                                var faResult = CrudRepository<carikart_genel_adres>.Update(item, "carikart_adres_id", new string[] { "kayit_silindi", "faturaadresi" });
                                break;
                            case "IA":
                                iaControl = true;
                                #region IA Detayları
                                item.carikart_adres_id = iletisimBilgileri.irsaliye_adresi.carikart_adres_id;
                                item.degistiren_carikart_id = iletisimBilgileri.irsaliye_adresi.degistiren_carikart_id;
                                item.adrestanim = "IRSALIYE";//iletisimBilgileri.irsaliye_adresi.adrestanim;
                                item.adresunvan = iletisimBilgileri.irsaliye_adresi.adresunvan;
                                item.adres = iletisimBilgileri.irsaliye_adresi.adres;
                                item.postakodu = iletisimBilgileri.irsaliye_adresi.postakodu;
                                item.ulke_id = iletisimBilgileri.irsaliye_adresi.ulke_id;
                                item.sehir_id = iletisimBilgileri.irsaliye_adresi.sehir_id;
                                item.ilce_id = iletisimBilgileri.irsaliye_adresi.ilce_id;
                                item.semt_id = iletisimBilgileri.irsaliye_adresi.semt_id;
                                item.vergidairesi = iletisimBilgileri.irsaliye_adresi.vergidairesi;
                                item.vergino = iletisimBilgileri.irsaliye_adresi.vergino;
                                item.faturaadresi = iletisimBilgileri.irsaliye_adresi.faturaadresi;
                                item.statu = iletisimBilgileri.irsaliye_adresi.statu;
                                item.adres_tipi_id = "IA";
                                if (iletisim_adres_tipi_id == "IA")
                                {
                                    item.tel1 = iletisimBilgileri.irsaliye_adresi.tel1;
                                    item.tel2 = iletisimBilgileri.irsaliye_adresi.tel2;
                                    item.fax = iletisimBilgileri.irsaliye_adresi.fax;
                                    item.email = iletisimBilgileri.irsaliye_adresi.email;
                                    item.websitesi = iletisimBilgileri.irsaliye_adresi.websitesi;
                                    item.yetkili_ad_soyad = iletisimBilgileri.irsaliye_adresi.yetkili_ad_soyad;
                                    item.yetkili_tel = iletisimBilgileri.irsaliye_adresi.yetkili_tel;
                                }
                                #endregion
                                var iaResult = CrudRepository<carikart_genel_adres>.Update(item, "carikart_adres_id", new string[] { "kayit_silindi", "faturaadresi" });
                                break;
                        }
                    }
                    #endregion
                    #region Insert
                    //Eğer daha önceden IA yada FA kayıtlarından biri oluşturulmamışsa yeni bir insert yaratılıyor
                    if (!iaControl || !faControl)
                    {
                        if (!faControl)
                        {
                            iletisimBilgileri.fatura_adresi.carikart_id = iletisimBilgileri.carikart_id;
                            iletisimBilgileri.fatura_adresi.degistiren_tarih = DateTime.Now;
                            iletisimBilgileri.fatura_adresi.adrestanim = "FATURA";
                            iletisimBilgileri.fatura_adresi.adres_tipi_id = "FA";
                            var faResult = CrudRepository<Adres>.Insert(iletisimBilgileri.fatura_adresi, "carikart_genel_adres", new string[] { "carikart_adres_id", "kayit_silindi", "faturaadresi" });
                        }

                        if (!iaControl)
                        {
                            iletisimBilgileri.irsaliye_adresi.carikart_id = iletisimBilgileri.carikart_id;
                            iletisimBilgileri.irsaliye_adresi.degistiren_tarih = DateTime.Now;
                            iletisimBilgileri.irsaliye_adresi.adrestanim = "IRSALIYE";
                            iletisimBilgileri.irsaliye_adresi.adres_tipi_id = "IA";

                            var iaResult = CrudRepository<Adres>.Insert(iletisimBilgileri.irsaliye_adresi, "carikart_genel_adres", new string[] { "carikart_adres_id", "kayit_silindi", "faturaadresi" });
                        }
                        //if (!iaControl)
                        //{
                        //    iletisimBilgileri.irsaliye_adresi.carikart_id = iletisimBilgileri.carikart_id;
                        //    iletisimBilgileri.irsaliye_adresi.degistiren_tarih = DateTime.Now;
                        //    iletisimBilgileri.irsaliye_adresi.adrestanim = "Finans Bilgileri İletişim";
                        //    var iaResult = CrudRepository<Adres>.Insert(iletisimBilgileri.irsaliye_adresi, "carikart_genel_adres", new string[] { "carikart_adres_id", "kayit_silindi", "faturaadresi" });
                        //}
                    }
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else
                {
                    //Burada tüm alanlar açılmalı.
                    #region Carikart "iletisim_adres_tipi_id" alanı güncelleniyor
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("iletisim_adres_tipi_id", iletisimBilgileri.iletisim_adres_tipi_id);
                    fields.Add("carikart_id", iletisimBilgileri.carikart_id);
                    var retCariAdresTipi = CrudRepository.Update("carikart", "carikart_id", fields);
                    #endregion
                    #region Insert
                    //FA -> Fatura Adresi Insert
                    iletisimBilgileri.fatura_adresi.carikart_id = iletisimBilgileri.carikart_id;
                    iletisimBilgileri.fatura_adresi.degistiren_tarih = DateTime.Now;
                    iletisimBilgileri.fatura_adresi.adres_tipi_id = "FA";
                    iletisimBilgileri.fatura_adresi.adrestanim = "FATURA";
                    var faResult = CrudRepository<Adres>.Insert(iletisimBilgileri.fatura_adresi, "carikart_genel_adres", new string[] { "carikart_adres_id", "kayit_silindi", "faturaadresi" });

                    //IA -> Irsaliye Adresi Insert
                    iletisimBilgileri.irsaliye_adresi.carikart_id = iletisimBilgileri.carikart_id;
                    iletisimBilgileri.irsaliye_adresi.degistiren_tarih = DateTime.Now;
                    iletisimBilgileri.irsaliye_adresi.adres_tipi_id = "IA";
                    iletisimBilgileri.irsaliye_adresi.adrestanim = "IRSALIYE";
                    var iaResult = CrudRepository<Adres>.Insert(iletisimBilgileri.irsaliye_adresi, "carikart_genel_adres", new string[] { "carikart_adres_id", "kayit_silindi", "faturaadresi" });
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    #endregion

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }

        /// <summary>
        /// Carikart -> Finans Bilgilerindeki -> İletişim Bilgileri sekmesinin Adres Listesini verir. Adres Tipi Id = FI
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// [
        ///     {
        ///         carikart_id: 100000000001,
        ///         carikart_adres_id : 100000000001,
        ///         email: "",
        ///         websitesi: "",
        ///         yetkili_ad_soyad: "",
        ///         yetkili_tel: ""
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-finans-iletisim/{carikart_id}")]
        public HttpResponseMessage CarikartFinansIletisimGetir(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            string iletisim_adres_tipi_id = "";
            var adresler = carikartRepository.CarikartAdresleriniGetir(carikart_id, out iletisim_adres_tipi_id).Where(a => a.adres_tipi_id == "FI").ToList();
            if (adresler != null && adresler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, adresler.Select(a => new
                {
                    a.carikart_id,
                    a.carikart_adres_id,
                    a.email,
                    a.websitesi,
                    a.yetkili_ad_soyad,
                    a.yetkili_tel,
                    a.kayit_silindi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Carikart - Finans Bilgileri - İletişim Bilgileri listesitesi veriri
        /// </summary>
        /// <param name="iletisimBilgileri"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-finans-iletisim")]
        public HttpResponseMessage CarikartFinansIletisimGetir(Carikart_Finans_IletisimBilgileri iletisimBilgileri)
        {
            if (iletisimBilgileri != null && iletisimBilgileri.carikart_id > 0)
            {
                carikart_genel_adres carikartAdres = new carikart_genel_adres();
                AcekaResult acekaResult = null;

                if (iletisimBilgileri.carikart_adres_id > 0)
                {
                    carikartRepository = new CarikartRepository();
                    string iletisim_adres_tipi_id = "";
                    var fiAdres =
                        carikartRepository.CarikartAdresleriniGetir(iletisimBilgileri.carikart_id, out iletisim_adres_tipi_id)
                        .Where(a => a.adres_tipi_id == "FI" && a.carikart_adres_id == iletisimBilgileri.carikart_adres_id).FirstOrDefault();

                    if (fiAdres != null)
                    {
                        //Update
                        //fiAdres.carikart_id = iletisimBilgileri.carikart_id;
                        //fiAdres.carikart_adres_id = iletisimBilgileri.carikart_adres_id;
                        fiAdres.degistiren_carikart_id = iletisimBilgileri.degistiren_carikart_id;
                        fiAdres.degistiren_tarih = DateTime.Now;

                        fiAdres.email = iletisimBilgileri.email;
                        fiAdres.websitesi = iletisimBilgileri.websitesi;
                        fiAdres.yetkili_ad_soyad = iletisimBilgileri.yetkili_ad_soyad;
                        fiAdres.yetkili_tel = iletisimBilgileri.yetkili_tel;
                        fiAdres.kayit_silindi = iletisimBilgileri.kayit_silindi;

                        acekaResult = CrudRepository<carikart_genel_adres>.Update(fiAdres, "carikart_adres_id", null);
                    }
                    else
                    {
                        // insert
                        carikartAdres.carikart_id = iletisimBilgileri.carikart_id;
                        carikartAdres.degistiren_carikart_id = iletisimBilgileri.degistiren_carikart_id;
                        carikartAdres.degistiren_tarih = DateTime.Now;

                        carikartAdres.email = iletisimBilgileri.email;
                        carikartAdres.websitesi = iletisimBilgileri.websitesi;
                        carikartAdres.yetkili_ad_soyad = iletisimBilgileri.yetkili_ad_soyad;
                        carikartAdres.yetkili_tel = iletisimBilgileri.yetkili_tel;
                        carikartAdres.adres_tipi_id = "FI";
                        carikartAdres.adrestanim = "Finans Bilgileri İletişim";
                        //carikartAdres.kayit_silindi = iletisimBilgileri.kayit_silindi;
                        carikartAdres.statu = true;

                        acekaResult = CrudRepository<carikart_genel_adres>.Insert(carikartAdres, new string[] { "carikart_adres_id" });

                    }
                }
                else
                {
                    // insert
                    carikartAdres.carikart_id = iletisimBilgileri.carikart_id;

                    carikartAdres.degistiren_carikart_id = Tools.PersonelId;
                    carikartAdres.degistiren_tarih = DateTime.Now;

                    carikartAdres.email = iletisimBilgileri.email;
                    carikartAdres.websitesi = iletisimBilgileri.websitesi;
                    carikartAdres.yetkili_ad_soyad = iletisimBilgileri.yetkili_ad_soyad;
                    carikartAdres.yetkili_tel = iletisimBilgileri.yetkili_tel;
                    carikartAdres.adres_tipi_id = "FI";
                    carikartAdres.adrestanim = "Finans Bilgileri İletişim";
                    //carikartAdres.kayit_silindi = iletisimBilgileri.kayit_silindi;
                    carikartAdres.statu = true;

                    acekaResult = CrudRepository<carikart_genel_adres>.Insert(carikartAdres, new string[] { "carikart_adres_id" });
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
        /// Rapor Parametrelerinin Listesini verir. parametre_carikart_rapor tablosu
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///{
        ///  "parametre_id": 1,
        ///  "parametre": 1,
        ///  "tanim": "Ana Grup 1"
        ///},
        ///{
        ///    "parametre_id": 2,
        ///    "parametre": 1,
        ///    "tanim": "Ana Grup 2"
        ///},
        ///{
        ///  "parametre_id": 3,
        ///  "parametre": 2,
        ///  "tanim": "Alt Grup 1"
        ///},
        ///{
        ///  "parametre_id": 4,
        ///  "parametre": 3,
        ///  "tanim": "TEST 1"
        ///},
        ///{
        ///  "parametre_id": 5,
        ///  "parametre": 3,
        ///  "tanim": "TEST 1"
        ///},
        ///{
        ///  "parametre_id": 6,
        ///  "parametre": 3,
        ///  "tanim": "TEST 1"
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-parametreleri-getir/{carikart_id}")]
        public HttpResponseMessage CarikartParametreleriniGetir(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var parametreler = carikartRepository.CarikartParametreleriniGetir(carikart_id);

            if (parametreler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {

                    parametreler.carikart_id,
                    parametreler.cari_parametre_1,
                    parametreler.cari_parametre_2,
                    parametreler.cari_parametre_3,
                    parametreler.cari_parametre_4,
                    parametreler.cari_parametre_5,
                    parametreler.cari_parametre_6,
                    parametreler.cari_parametre_7,

                });


            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Carikart Şirket Listesini Getirir
        /// </summary>
        /// <returns>
        /// [
        ///{
        ///sirket_id: 1,
        ///cari_unvan: "Giz Yazılım",
        ///cari_unvan_kucuk: "giz yazılım"
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/sirket-listesi")]
        public HttpResponseMessage CarikartSirketListesi()
        {
            carikartRepository = new CarikartRepository();
            var parametreler = carikartRepository.SirketListesiGetir();

            if (parametreler != null && parametreler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, parametreler.Select(p => new
                {
                    p.sirket_id,
                    p.sirket_adi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Carikart Finans Bİlgilerini Getirir
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// {
        /// carikart_id: 0,
        /// tckimlikno: "",
        /// vergi_dairesi: "MARMARA KURUMLAR",
        /// vergi_no: "609 023 0656",
        /// yabanci_uyruklu: true,
        /// diger_kod: "",
        /// pb: "",
        /// vade_alis: 0,
        /// vade_satis: 0,
        /// odemePlani: null,
        /// iskonto_alis: 0,
        /// iskonto_satis: 0,
        /// odeme_tipi: 0,
        /// kur_farki: 0,
        /// odeme_listesinde_cikmasin: 0,
        /// alacak_listesinde_cikmasin: 0,
        /// ticari_islem_grubu: 0,
        /// ilgili_sube: {
        /// carikart_id: 100000000001,
        /// cari_unvan: "LOTTO SPORT ITALIA S.P.A."
        /// },
        /// finans_sorumlu: {
        /// carikart_id: 100000000001,
        /// cari_unvan: "LOTTO SPORT ITALIA S.P.A."
        /// },
        /// swift_kodu: "",
        /// tedarik_gunu: 0,
        /// cari_hesapta_ciksin: true,
        /// sirket_id: 1,
        /// sene: 0,
        /// muh_kod: "320.99",
        /// masraf_merkezi: null
        /// }        
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/finans-bilgileri/{carikart_id}")]
        public HttpResponseMessage CarikartFinasBilgileri(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var finansBilgileri = carikartRepository.CarikartFinansBilgileriniGetir(carikart_id);
            if (finansBilgileri != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    finansBilgileri.carikart_id,
                    finansBilgileri.tckimlikno,
                    finansBilgileri.vergi_dairesi,
                    finansBilgileri.vergi_no,
                    finansBilgileri.yabanci_uyruklu,
                    finansBilgileri.diger_kod,
                    finansBilgileri.pb,
                    finansBilgileri.vade_alis,
                    finansBilgileri.vade_satis,
                    finansBilgileri.carikart.fiyattipi,
                    finansBilgileri.odeme_plani.odeme_plani_id,
                    finansBilgileri.iskonto_alis,
                    finansBilgileri.iskonto_satis,

                    //parametre_cari_odeme_sekli tablosunda "cari_odeme_sekli_id" olan field bu tabloda "odeme_tipi" olarak tutuluyor                    
                    odeme_sekli_id = finansBilgileri.odeme_tipi,

                    finansBilgileri.kur_farki,
                    finansBilgileri.odeme_listesinde_cikmasin,
                    finansBilgileri.alacak_listesinde_cikmasin,
                    finansBilgileri.ticari_islem_grubu,

                    finansBilgileri.ilgili_sube_carikart_id,
                    finansBilgileri.finans_sorumlu_carikart_id,
                    finansBilgileri.swift_kodu,
                    finansBilgileri.tedarik_gunu,
                    finansBilgileri.cari_hesapta_ciksin,
                    finansBilgileri.carikart_muhasebe.sirket_id,
                    finansBilgileri.carikart_muhasebe.sene,
                    finansBilgileri.carikart_muhasebe.muh_kod,
                    finansBilgileri.carikart_muhasebe.masraf_merkezi_id,
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Finans Bibilgilerinin POST edilerek, insert - update işlemleri yapılıyor
        /// </summary>
        /// <param name="finansBilgileri"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/finans-bilgileri")]
        public HttpResponseMessage CarikartFinasBilgileri(FinansBilgileri finansBilgileri)
        {
            if (finansBilgileri != null && finansBilgileri.carikart_id > 0)
            {
                AcekaResult acekaResult = null;
                carikartRepository = new CarikartRepository();

                carikart_finans ckFinans = carikartRepository.CarikartFinansBilgileriniGetir(finansBilgileri.carikart_id);
                if (ckFinans != null)
                {
                    // update
                    #region Fields
                    ckFinans.degistiren_carikart_id = Tools.PersonelId;
                    ckFinans.degistiren_tarih = DateTime.Now;
                    ckFinans.tckimlikno = finansBilgileri.tckimlikno;
                    ckFinans.vergi_no = finansBilgileri.vergi_no;
                    ckFinans.vergi_dairesi = finansBilgileri.vergi_dairesi;
                    ckFinans.yabanci_uyruklu = finansBilgileri.yabanci_uyruklu;
                    ckFinans.diger_kod = finansBilgileri.diger_kod;
                    ckFinans.pb = finansBilgileri.pb;
                    ckFinans.vade_alis = finansBilgileri.vade_alis;
                    ckFinans.vade_satis = finansBilgileri.vade_satis;
                    ckFinans.iskonto_alis = finansBilgileri.iskonto_alis;
                    ckFinans.iskonto_satis = finansBilgileri.iskonto_satis;
                    ckFinans.kur_farki = finansBilgileri.kur_farki;
                    ckFinans.odeme_listesinde_cikmasin = finansBilgileri.odeme_listesinde_cikmasin;
                    ckFinans.alacak_listesinde_cikmasin = finansBilgileri.alacak_listesinde_cikmasin;
                    ckFinans.ticari_islem_grubu = finansBilgileri.ticari_islem_grubu;
                    ckFinans.ilgili_sube_carikart_id = finansBilgileri.ilgili_sube_carikart_id;
                    ckFinans.finans_sorumlu_carikart_id = finansBilgileri.finans_sorumlu_carikart_id;
                    ckFinans.swift_kodu = finansBilgileri.swift_kodu;
                    ckFinans.tedarik_gunu = finansBilgileri.tedarik_gunu;
                    ckFinans.cari_hesapta_ciksin = finansBilgileri.cari_hesapta_ciksin;
                    ckFinans.odeme_plani_id = finansBilgileri.odeme_plani_id;
                    ckFinans.odeme_tipi = finansBilgileri.odeme_sekli_id;
                    ckFinans.carikart.fiyattipi = finansBilgileri.fiyattipi;
                    #endregion

                    acekaResult = CrudRepository<carikart_finans>.Update(ckFinans, "carikart_id", new string[] { "ilgili_sube_cari_unvan", "finans_sorumlu_cari_unvan" });
                }
                else
                {
                    // insert

                    #region Fields
                    ckFinans = new carikart_finans();
                    ckFinans.carikart_id = finansBilgileri.carikart_id;
                    ckFinans.degistiren_carikart_id = -1;
                    ckFinans.degistiren_tarih = DateTime.Now;
                    ckFinans.tckimlikno = finansBilgileri.tckimlikno;
                    ckFinans.vergi_no = finansBilgileri.vergi_no;
                    ckFinans.vergi_dairesi = finansBilgileri.vergi_dairesi;
                    ckFinans.yabanci_uyruklu = finansBilgileri.yabanci_uyruklu;
                    ckFinans.diger_kod = finansBilgileri.diger_kod;
                    ckFinans.pb = finansBilgileri.pb;
                    ckFinans.vade_alis = finansBilgileri.vade_alis;
                    ckFinans.vade_satis = finansBilgileri.vade_satis;
                    ckFinans.iskonto_alis = finansBilgileri.iskonto_alis;
                    ckFinans.iskonto_satis = finansBilgileri.iskonto_satis;
                    ckFinans.kur_farki = finansBilgileri.kur_farki;
                    ckFinans.odeme_listesinde_cikmasin = finansBilgileri.odeme_listesinde_cikmasin;
                    ckFinans.alacak_listesinde_cikmasin = finansBilgileri.alacak_listesinde_cikmasin;
                    ckFinans.ticari_islem_grubu = finansBilgileri.ticari_islem_grubu;
                    ckFinans.ilgili_sube_carikart_id = finansBilgileri.ilgili_sube_carikart_id;
                    ckFinans.finans_sorumlu_carikart_id = finansBilgileri.finans_sorumlu_carikart_id;
                    ckFinans.swift_kodu = finansBilgileri.swift_kodu;
                    ckFinans.tedarik_gunu = finansBilgileri.tedarik_gunu;
                    ckFinans.cari_hesapta_ciksin = finansBilgileri.cari_hesapta_ciksin;
                    ckFinans.odeme_plani_id = finansBilgileri.odeme_plani_id;
                    ckFinans.odeme_tipi = finansBilgileri.odeme_sekli_id;
                    ckFinans.carikart = new cari_kart();
                    ckFinans.carikart.fiyattipi = finansBilgileri.fiyattipi;
                    #endregion

                    acekaResult = CrudRepository<carikart_finans>.Insert(ckFinans, new string[] { "ilgili_sube_cari_unvan", "finans_sorumlu_cari_unvan" });
                }

                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {

                    #region carikart_fiyat_tipi
                    carikart_fiyat_tipi carikartFiyatTipi = carikartRepository.CarikartFiyatTipi(finansBilgileri.carikart_id);
                    if (carikartFiyatTipi != null)
                    {
                        carikartFiyatTipi.carikart_id = finansBilgileri.carikart_id;
                        carikartFiyatTipi.degistiren_carikart_id = finansBilgileri.degistiren_carikart_id;
                        carikartFiyatTipi.degistiren_tarih = DateTime.Now;
                        carikartFiyatTipi.fiyattipi = finansBilgileri.fiyattipi;

                        var fiyatTipiRetval = CrudRepository<carikart_fiyat_tipi>.Update(carikartFiyatTipi, "carikart_id", new string[] { "fiyattipi_adi" });
                        //var fiyatTipiRetval = CrudRepository<carikart_fiyat_tipi>.Update(carikartFiyatTipi, "carikart_id", new string[] { "fiyattipi_adi"} );
                    }
                    else
                    {
                        carikartFiyatTipi = new carikart_fiyat_tipi();
                        carikartFiyatTipi.carikart_id = finansBilgileri.carikart_id;
                        carikartFiyatTipi.degistiren_carikart_id = finansBilgileri.degistiren_carikart_id;
                        carikartFiyatTipi.degistiren_tarih = DateTime.Now;
                        carikartFiyatTipi.fiyattipi = finansBilgileri.fiyattipi;
                        carikartFiyatTipi.kayit_silindi = false;
                        carikartFiyatTipi.statu = true;
                        carikartFiyatTipi.varsayilan = false;

                        var fiyatTipiRetval = CrudRepository<carikart_fiyat_tipi>.Insert(carikartFiyatTipi, new string[] { "fiyattipi_adi" });

                    }
                    #endregion

                    #region carikart_muhasebe
                    carikart_muhasebe carikartMuhasebe = carikartRepository.CarikartMuhasebeBilgileri(finansBilgileri.carikart_id);
                    if (carikartMuhasebe != null)
                    {
                        carikartMuhasebe.carikart_id = finansBilgileri.carikart_id;
                        carikartMuhasebe.degistiren_carikart_id = finansBilgileri.degistiren_carikart_id;
                        carikartMuhasebe.degistiren_tarih = DateTime.Now;
                        carikartMuhasebe.sirket_id = finansBilgileri.sirket_id;
                        carikartMuhasebe.sene = finansBilgileri.sene;
                        carikartMuhasebe.muh_kod = finansBilgileri.muh_kod;
                        carikartMuhasebe.masraf_merkezi_id = finansBilgileri.masraf_merkezi_id;

                        var muhasebeRetval = CrudRepository<carikart_muhasebe>.Update(carikartMuhasebe, "carikart_id", new string[] { "masraf_merkezi_adi" });
                    }
                    else
                    {
                        carikartMuhasebe = new carikart_muhasebe();
                        carikartMuhasebe.carikart_id = finansBilgileri.carikart_id;
                        carikartMuhasebe.degistiren_carikart_id = finansBilgileri.degistiren_carikart_id;
                        carikartMuhasebe.degistiren_tarih = DateTime.Now;
                        carikartMuhasebe.sirket_id = finansBilgileri.sirket_id;
                        carikartMuhasebe.sene = finansBilgileri.sene;
                        carikartMuhasebe.muh_kod = finansBilgileri.muh_kod;
                        carikartMuhasebe.masraf_merkezi_id = finansBilgileri.masraf_merkezi_id;

                        var muhasebeRetval = CrudRepository<carikart_muhasebe>.Insert(carikartMuhasebe, new string[] { "masraf_merkezi_adi" });

                    }
                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }


        }

        /// <summary>
        /// /// Özel Alanlar Sekmesini getiren Metod
        /// </summary>
        /// <param name="carikart_id">        
        /// {
        ///   carikart_id: 100120000002,
        ///   satin_alma_sorumlu_carikart_id: 100000000100,
        ///   satin_alma_sorumlu_cari_unvan: "Hasan GÜNDOĞDU",
        ///   satis_sorumlu_carikart_id: 100000001317,
        ///   satis_sorumlu_cari_unvan: "Seçim EREN",
        ///   baslamatarihi: "2016-12-01T00:00:00",
        ///   ozel: ""
        /// }
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/ozel-alanlar/{carikart_id}")]
        public HttpResponseMessage OzelAlanGetir(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var ozelalanlar = carikartRepository.CarikartOzelalanlarGetir(carikart_id);
            if (ozelalanlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ozelalanlar.carikart_id,
                    ozelalanlar.satin_alma_sorumlu_carikart_id,
                    ozelalanlar.satin_alma_sorumlu_cari_unvan,
                    ozelalanlar.satis_sorumlu_carikart_id,
                    ozelalanlar.satis_sorumlu_cari_unvan,
                    ozelalanlar.baslamatarihi,
                    ozelalanlar.ozel
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari Özel Alanlar POST insert, update işlemleri
        /// </summary>
        /// <param name="ozelalanlar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/ozel-alanlar")]
        public HttpResponseMessage OzelAlanGetir(carikart_ozelalanlar ozelalanlar)
        {
            if (ozelalanlar != null && ozelalanlar.carikart_id > 0)
            {
                AcekaResult acekaResult = null;
                carikartRepository = new CarikartRepository();
                carikart_firma_ozel ckozelanlar = carikartRepository.CarikartOzelalanlarGetir(ozelalanlar.carikart_id);
                if (ckozelanlar != null)
                {
                    //update
                    ckozelanlar.degistiren_carikart_id = Tools.PersonelId;
                    ckozelanlar.degistiren_tarih = DateTime.Now;
                    ckozelanlar.satis_sorumlu_carikart_id = ozelalanlar.satis_sorumlu_carikart_id;
                    ckozelanlar.satin_alma_sorumlu_carikart_id = ozelalanlar.satin_alma_sorumlu_carikart_id;
                    ckozelanlar.baslamatarihi = ozelalanlar.baslamatarihi;
                    ckozelanlar.ozel = ozelalanlar.ozel;

                    acekaResult = CrudRepository<carikart_firma_ozel>.Update(ckozelanlar, "carikart_id", new string[] { "satin_alma_sorumlu_cari_unvan", "satis_sorumlu_cari_unvan" });
                }
                else
                {
                    //insert
                    ckozelanlar = new carikart_firma_ozel();
                    ckozelanlar.carikart_id = ozelalanlar.carikart_id;
                    ckozelanlar.degistiren_carikart_id = Tools.PersonelId;
                    ckozelanlar.degistiren_tarih = DateTime.Now;
                    ckozelanlar.satis_sorumlu_carikart_id = ozelalanlar.satis_sorumlu_carikart_id;
                    ckozelanlar.satin_alma_sorumlu_carikart_id = ozelalanlar.satin_alma_sorumlu_carikart_id;
                    ckozelanlar.baslamatarihi = ozelalanlar.baslamatarihi;
                    ckozelanlar.ozel = ozelalanlar.ozel;

                    acekaResult = CrudRepository<carikart_firma_ozel>.Insert(ckozelanlar, new string[] { "satin_alma_sorumlu_cari_unvan", "satis_sorumlu_cari_unvan" });
                }

                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else if (acekaResult.ErrorInfo != null)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// E-posta gruplarını veren metodu AA
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// {
        ///  "carikart_id": 0,
        ///  "babs_formu_eposta": null,
        ///  "cari_mutabakat_formu_eposta": null,
        ///  "irsaliye_eposta": null,
        ///  "odeme_hatirlatma_eposta": null,
        ///  "perakende_fatura_eposta": null,
        ///  "siparis_formu_eposta": null,
        ///  "toptan_fatura_eposta": null,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "0001-01-01T00:00:00"
        ///}
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/eposta-gruplari/{carikart_id}")]
        public HttpResponseMessage EpostaGrupGetir(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var epostagruplari = carikartRepository.EpostaGrupGetir(carikart_id);
            if (epostagruplari != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    epostagruplari.carikart_id,
                    epostagruplari.babs_formu_eposta,
                    epostagruplari.cari_mutabakat_formu_eposta,
                    epostagruplari.irsaliye_eposta,
                    epostagruplari.odeme_hatirlatma_eposta,
                    epostagruplari.perakende_fatura_eposta,
                    epostagruplari.siparis_formu_eposta,
                    epostagruplari.toptan_fatura_eposta,
                    epostagruplari.degistiren_carikart_id,
                    epostagruplari.degistiren_tarih
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// E-posta gruplarını veren Post metodu AA
        /// </summary>
        /// <param name="epostagrubu"></param>
        /// <returns>
        /// {
        ///  "carikart_id": 0,
        ///  "babs_formu_eposta": null,
        ///  "cari_mutabakat_formu_eposta": null,
        ///  "irsaliye_eposta": null,
        ///  "odeme_hatirlatma_eposta": null,
        ///  "perakende_fatura_eposta": null,
        ///  "siparis_formu_eposta": null,
        ///  "toptan_fatura_eposta": null,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "0001-01-01T00:00:00"
        ///}
        /// </returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/eposta-gruplari")]
        public HttpResponseMessage EpostaGrupGetir(carikartelektronik_bilgilendirme epostagrubu)
        {
            AcekaResult acekaResult = null;

            carikartRepository = new CarikartRepository();
            var epostagruplari = carikartRepository.EpostaGrupGetir(epostagrubu.carikart_id);
            if (epostagrubu != null)
            {
                if (epostagruplari != null && epostagruplari.carikart_id > 0)
                {
                    //update

                    epostagruplari.degistiren_carikart_id = Tools.PersonelId;
                    epostagruplari.degistiren_tarih = DateTime.Now;
                    epostagruplari.cari_mutabakat_formu_eposta = epostagrubu.cari_mutabakat_formu_eposta;
                    epostagruplari.babs_formu_eposta = epostagrubu.babs_formu_eposta;
                    epostagruplari.irsaliye_eposta = epostagrubu.irsaliye_eposta;
                    epostagruplari.odeme_hatirlatma_eposta = epostagrubu.odeme_hatirlatma_eposta;
                    epostagruplari.perakende_fatura_eposta = epostagrubu.perakende_fatura_eposta;
                    epostagruplari.siparis_formu_eposta = epostagrubu.siparis_formu_eposta;
                    epostagruplari.toptan_fatura_eposta = epostagrubu.toptan_fatura_eposta;

                    acekaResult = CrudRepository<carikart_elektronik_bilgilendirme>.Update(epostagruplari, "carikart_id");
                }
                else
                {
                    //insert
                    epostagruplari = new carikart_elektronik_bilgilendirme();
                    epostagruplari.carikart_id = epostagrubu.carikart_id;
                    epostagruplari.degistiren_carikart_id = -1;
                    epostagruplari.degistiren_tarih = DateTime.Now;
                    epostagruplari.cari_mutabakat_formu_eposta = epostagrubu.cari_mutabakat_formu_eposta;
                    epostagruplari.babs_formu_eposta = epostagrubu.babs_formu_eposta;
                    epostagruplari.irsaliye_eposta = epostagrubu.irsaliye_eposta;
                    epostagruplari.odeme_hatirlatma_eposta = epostagrubu.odeme_hatirlatma_eposta;
                    epostagruplari.perakende_fatura_eposta = epostagrubu.perakende_fatura_eposta;
                    epostagruplari.siparis_formu_eposta = epostagrubu.siparis_formu_eposta;
                    epostagruplari.toptan_fatura_eposta = epostagrubu.toptan_fatura_eposta;
                    acekaResult = CrudRepository<carikart_elektronik_bilgilendirme>.Insert(epostagruplari);

                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }

        /// <summary>
        /// Seçilen Cari ye göre şube Listesini  veren metodu AA
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// {
        ///     carikart_id: 100000001949,
        ///     ana_carikart_id: 0,
        ///     cari_unvan: "AJARA TEXTILE LTD.",
        ///     carikart_tipi_id: 4,
        ///     carikart_turu_id: 3,
        ///     carikart_tipi_adi: "Atöyle",
        ///     carikart_turu_adi: "Lokasyon"
        /// }
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-sube-listesi/{carikart_id}")]
        public HttpResponseMessage CarikartSubeGetir(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var subeler = carikartRepository.SubeListesiGetir(carikart_id);
            if (subeler != null)

            {
                return Request.CreateResponse(HttpStatusCode.OK, subeler.Select(sube => new
                {
                    sube.carikart_id,
                    sube.ana_carikart_id,
                    sube.cari_unvan,
                    sube.carikart_tipi_id,
                    sube.carikart_turu_id,
                    sube.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    sube.giz_sabit_carikart_turu.carikart_turu_adi,
                    sube.kayit_silindi
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Şube Listesini  veren metodu AA
        /// </summary>
        /// <param name="cariSube"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-sube-listesi")]
        public HttpResponseMessage CarikartSubeGetir(CariSube cariSube)
        {
            if (cariSube != null && cariSube.carikart_id > 0)
            {
                //update Kontrol
                AcekaResult acekaResult = null;
                carikartRepository = new CarikartRepository();
                cari_kart carikart = carikartRepository.Getir(cariSube.ana_carikart_id);

                if (carikart != null)
                {
                    if (cariSube.kayit_silindi == true)
                    {
                        #region Update Ana Carikat id silme
                        carikart.ana_carikart_id = 0;
                        //carikart.degistiren_tarih = DateTime.Now;
                        //carikart.degistiren_carikart_id = cariSube.degistiren_carikart_id;
                        //carikart.kayit_silindi = cariSube.kayit_silindi;

                        acekaResult = CrudRepository<cari_kart>.Update(carikart, "carikart", "carikart_id", new string[] { "sube_cari_unvan", "ana_cari_unvan", "fiyattipi_adi", "degistiren_carikart_id", "degistiren_tarih", "kayit_silindi", "kayit_yeri", "statu", "carikart_turu_id", "carikart_tipi_id", "cari_unvan", "ozel_kod", "fiyattipi", "giz_yazilim_kodu", "transfer_depo_id", "giz_kullanici_adi", "giz_kullanici_sifre", "cari_parametre_1", "cari_parametre_2", "cari_parametre_3", "cari_parametre_4", "", "cari_parametre_5", "cari_parametre_6", "cari_parametre_7", "", "iletisim_adres_tipi_id", "sube_carikart_id" });
                        if (acekaResult != null && acekaResult.ErrorInfo == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                        }
                        #endregion
                    }
                    #region Update
                    carikart.ana_carikart_id = cariSube.carikart_id;
                    //carikart.degistiren_tarih = DateTime.Now;
                    //carikart.degistiren_carikart_id = cariSube.degistiren_carikart_id;
                    //carikart.kayit_silindi = cariSube.kayit_silindi;

                    acekaResult = CrudRepository<cari_kart>.Update(carikart, "carikart", "carikart_id", new string[] { "sube_cari_unvan", "ana_cari_unvan", "fiyattipi_adi", "degistiren_carikart_id", "degistiren_tarih", "kayit_silindi", "kayit_yeri", "statu", "carikart_turu_id", "carikart_tipi_id", "cari_unvan", "ozel_kod", "fiyattipi", "giz_yazilim_kodu", "transfer_depo_id", "giz_kullanici_adi", "giz_kullanici_sifre", "cari_parametre_1", "cari_parametre_2", "cari_parametre_3", "cari_parametre_4", "", "cari_parametre_5", "cari_parametre_6", "cari_parametre_7", "", "iletisim_adres_tipi_id", "sube_carikart_id" });
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                    }
                    #endregion
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }


        /// <summary>
        ///  /// Carikart - > Finans Bilgileri -> Notlar GET Metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         carikart_not_id: 1,
        ///         carikart_id: 0,
        ///         aciklama: "genel not1",
        ///         nereden: "Tedarik Zinciri"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-notlar/{carikart_id}")]
        public HttpResponseMessage CarikartNotlar(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var notlar = carikartRepository.CarikartNotlar(carikart_id);
            if (notlar != null && notlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, notlar.Select(n => new
                {
                    n.carikart_not_id,
                    n.carikart_id,
                    n.aciklama,
                    n.nereden,
                    n.kayit_silindi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Carikart - > Finans Bilgileri -> Notlar Insert ve Update POST Metod
        /// </summary>
        /// <param name="carikart_Notlar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-notlar")]
        public HttpResponseMessage CarikartNotlar(Carikart_Notlar carikart_Notlar)
        {
            if (carikart_Notlar != null && carikart_Notlar.carikart_id > 0)
            {
                carikart_Notlar.degistiren_tarih = DateTime.Now;

                AcekaResult acekaResult = null;
                carikartRepository = new CarikartRepository();

                if (carikart_Notlar.carikart_not_id > 0)
                {
                    carikart_genel_notlar notlar = carikartRepository.CarikartNotDetay(carikart_Notlar.carikart_id, carikart_Notlar.carikart_not_id);

                    if (notlar != null)
                    {
                        //update
                        notlar.degistiren_carikart_id = Tools.PersonelId;
                        notlar.degistiren_tarih = DateTime.Now;
                        notlar.aciklama = carikart_Notlar.aciklama;
                        notlar.nereden = carikart_Notlar.nereden;
                        notlar.kayit_silindi = carikart_Notlar.kayit_silindi;

                        acekaResult = CrudRepository<carikart_genel_notlar>.Update(notlar, "carikart_not_id");
                    }
                    else
                    {
                        //insert
                        acekaResult = CrudRepository<Carikart_Notlar>.Insert(carikart_Notlar, "carikart_genel_notlar", new string[] { "carikart_not_id" });
                    }
                }
                else
                {
                    //insert
                    acekaResult = CrudRepository<Carikart_Notlar>.Insert(carikart_Notlar, "carikart_genel_notlar", new string[] { "carikart_not_id" });
                }

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// Carikart a ait banka hesaplarını getirir.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// [
        ///   {
        ///     carikart_banka_id: 1,
        ///     carikart_id: 100000000947,
        ///     ulke_id : 90,
        ///     banka_id: 10,
        ///     banka_adi: "Türkiye Cumhuriyeti Ziraat Bankası A.Ş.",
        ///     banka_sube_id: 1,
        ///     banka_sube_adi: "AKSU ISPARTA",
        ///     ibanno: "TR486513846513",
        ///     pb: "TL",
        ///     ebanka: false,
        ///     odemehesabi: false,
        ///     kredi_limiti_dbs: 0
        ///   }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-banka-hesaplari/{carikart_id}")]
        public HttpResponseMessage CarikartBankaHesaplariCarikartNotlar(long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var bankalar = carikartRepository.CarikartBankaHesaplari(carikart_id);
            if (bankalar != null && bankalar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bankalar.Select(b => new
                {
                    b.carikart_banka_id,
                    b.carikart_id,
                    ulke_id = (b.banka_id > 0 ? b.banka.ulke_id : 0),
                    b.banka_id,
                    banka_adi = (b.banka_id > 0 ? b.banka.banka_adi : ""),
                    b.banka_sube_id,
                    banka_sube_adi =
                        (b.banka_id > 0 && (b.banka.subeler != null && b.banka.subeler.Count > 0) ? b.banka.subeler[0].banka_sube_adi : ""),
                    b.ibanno,
                    b.pb,
                    b.ebanka,
                    b.odemehesabi,
                    b.kredi_limiti_dbs,
                    b.kayit_silindi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari kart banka detayını getiren metod
        /// </summary>
        /// <param name="carikart_id"></param>/// 
        /// <param name="carikart_banka_id"></param>
        /// <returns>
        /// {
        ///        carikart_banka_id: 1,
        ///        carikart_id: 100000000947,
        ///        banka_id: null,
        ///        banka_sube_id: null,
        ///        ibanno: "TR486513846513",
        ///        pb: "TL",
        ///        ebanka: false,
        ///        odemehesabi: false,
        ///        kredi_limiti_dbs: 0
        /// }
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/cari/cari-banka-hesabi/{carikart_id}/{carikart_banka_id}")]
        public HttpResponseMessage CarikartBankaHesapDetayi(long carikart_banka_id, long carikart_id)
        {
            carikartRepository = new CarikartRepository();
            var banka = carikartRepository.CarikartBankaHesapDetayi(carikart_banka_id, carikart_id);
            if (banka != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    banka.carikart_banka_id,
                    banka.carikart_id,
                    banka.banka_id,
                    banka.banka_sube_id,
                    banka.ibanno,
                    banka.pb,
                    banka.ebanka,
                    banka.odemehesabi,
                    banka.kredi_limiti_dbs
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari Banka POST edilerek, insert - update işlemleri yapılıyor
        /// </summary>
        /// <param name="cariBankaHesap"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/cari/cari-banka-hesabi")]
        public HttpResponseMessage CarikartBankaHesapDetayi(CarikartBankaHesap cariBankaHesap)
        {
            AcekaResult acekaResult = null;

            if (cariBankaHesap != null && cariBankaHesap.carikart_id > 0)
            {
                cariBankaHesap.degistiren_tarih = DateTime.Now;
                carikart_finans_banka_hesaplari bankaHesap = null;

                if (cariBankaHesap.carikart_banka_id > 0)
                {
                    carikartRepository = new CarikartRepository();

                    bankaHesap = carikartRepository.CarikartBankaHesapDetayi(cariBankaHesap.carikart_banka_id, cariBankaHesap.carikart_id);
                    if (bankaHesap != null)
                    {
                        //update
                        bankaHesap.banka = new parametre_banka();
                        bankaHesap.banka_id = cariBankaHesap.banka_id;
                        bankaHesap.degistiren_carikart_id = cariBankaHesap.degistiren_carikart_id;
                        bankaHesap.degistiren_tarih = cariBankaHesap.degistiren_tarih;
                        bankaHesap.banka_sube_id = cariBankaHesap.banka_sube_id;
                        bankaHesap.ibanno = cariBankaHesap.ibanno;
                        bankaHesap.pb = cariBankaHesap.pb;
                        bankaHesap.ebanka = cariBankaHesap.ebanka;
                        bankaHesap.odemehesabi = cariBankaHesap.odemehesabi;
                        bankaHesap.kredi_limiti_dbs = cariBankaHesap.kredi_limiti_dbs;
                        bankaHesap.kayit_silindi = cariBankaHesap.kayit_silindi;

                        acekaResult = CrudRepository<carikart_finans_banka_hesaplari>.Update(bankaHesap, "carikart_banka_id", null);
                    }
                    else
                    {
                        //insert
                        acekaResult = CrudRepository<CarikartBankaHesap>.Insert(cariBankaHesap, "carikart_finans_banka_hesaplari", new string[] { "carikart_banka_id", });
                    }
                }
                else
                {
                    //insert
                    acekaResult = CrudRepository<CarikartBankaHesap>.Insert(cariBankaHesap, "carikart_finans_banka_hesaplari", new string[] { "carikart_banka_id" });
                }

                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }
        #endregion

        #region Carikart Aksesuar Denetim
        /// <summary>
        /// Carikart Aksesuar Denetim listesi. Tip = 0 (sıfır) olacak.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <param name="tip"></param>
        /// <returns>
        /// [
        ///     {
        ///         "carikart_id": 2,
        ///         "tip": 0,
        ///         "sira": 1,
        ///         "degistiren_carikart_id": 2189,
        ///         "degistiren_tarih": "2017-11-23T12:10:07",
        ///         "aksesuarkart_id": 42592,
        ///         "renk_id": 0,
        ///         "miktar": 1,
        ///         "kosul": "s.siparisturu||=||'0'|&|isnull(g.CUSTCODE:'')||çok.içermeyen||'*USPUA*;*TWPUT*;*ECDID*'",
        ///         "stok_kodu": "Apoşt ve kart stiker",
        ///         "stok_adi": "poşet ve kart stiker",
        ///         "birim_id_1": 1,
        ///         "birim_adi": "Adet",
        ///         "orjinal_stok_adi": "",
        ///         "orjinal_stok_kodu": "poşt ve kart stiker",
        ///         "renk_adi": "Her Hangi Bir Renk"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-listesi")]
        [Route("api/cari/aksesuar-denetim-listesi/{carikart_id},{tip}")]
        public HttpResponseMessage CarikartAksesuarDenetimListesi(long carikart_id, byte tip)
        {
            carikartRepository = new CarikartRepository();
            denetimaksesuar = carikartRepository.CarikartDenetimAksesuarList(carikart_id, tip);

            if (denetimaksesuar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, denetimaksesuar.Select(b => new
                {
                    carikart_id = b.carikart_id,
                    tip = b.tip,
                    sira = b.sira,
                    degistiren_carikart_id = b.degistiren_carikart_id,
                    degistiren_tarih = b.degistiren_tarih,
                    aksesuarkart_id = b.aksesuarkart_id,
                    renk_id = b.renk_id,
                    miktar = b.miktar,
                    kosul = b.kosul,
                    stok_kodu = b.stokkart.stok_kodu,
                    stok_adi = b.stokkart.stok_adi,
                    birim_id_1 = b.stokkart.birim_id_1,
                    birim_adi = b.parametre_birim.birim_adi,
                    orjinal_stok_adi = b.stokkart_ozel.orjinal_stok_adi,
                    orjinal_stok_kodu = b.stokkart_ozel.orjinal_stok_kodu,
                    renk_adi = b.parametre_renk.renk_adi

                }).ToList());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Siparişe Aksesuar Ekleme. Dikkat carikart_id,tip ve sira alanları benzersiz olmalı. Tabloda key var.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-listesi")]
        [Route("api/cari/aksesuar-denetim-listesi")]
        public HttpResponseMessage CarikartDenetimAksesuar(carikart_denetim_aksesuar aksesuar)
        {
            AcekaResult acekaResult = null;
            if (aksesuar != null)
            {
                aksesuar.degistiren_tarih = DateTime.Now;
                aksesuar.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<carikart_denetim_aksesuar>.Insert(aksesuar, "carikart_denetim_aksesuar");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = aksesuar.carikart_id.ToString() });
        }

        /// <summary>
        /// Siparişdeki Aksesuarı Günclleme. Dikkat carikart_id,tip ve sira alanları benzersiz olmalı. Tabloda key var.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-listesi")]
        [Route("api/cari/aksesuar-denetim-listesi")]
        public HttpResponseMessage CarikartDenetimAkssuar(carikart_denetim_aksesuar aksesuar)
        {
            AcekaResult acekaResult = null;
            carikartRepository = new CarikartRepository();
            carikart_denetim_aksesuar aksdenetim = new carikart_denetim_aksesuar();
            if (aksesuar != null)
            {
                //aksesuar.degistiren_tarih = DateTime.Now;
                //aksesuar.degistiren_carikart_id = Tools.PersonelId;
                aksdenetim = carikartRepository.CarikartDenetimAksesuar(aksesuar.carikart_id, aksesuar.tip);
                if (aksdenetim != null)
                {
                    aksdenetim.carikart_id = aksesuar.carikart_id;
                    aksdenetim.degistiren_carikart_id = aksesuar.degistiren_carikart_id;
                    aksdenetim.degistiren_tarih = aksesuar.degistiren_tarih;
                    aksdenetim.kosul = aksesuar.kosul;
                    aksdenetim.miktar = aksesuar.miktar;
                    aksdenetim.renk_id = aksesuar.renk_id;
                    aksdenetim.sira = aksesuar.sira;
                    aksdenetim.tip = aksesuar.tip;
                    aksdenetim.aksesuarkart_id = aksesuar.aksesuarkart_id;
                }
                acekaResult = CrudRepository<carikart_denetim_aksesuar>.Update(aksesuar, new string[] { "carikart_id", "sira", "tip" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = aksesuar.carikart_id.ToString() });
        }

        /// <summary>
        /// Siparişdeki Aksesuarı Silme. Dikkat silme işlemi carikart_id,tip ve sira alanlarına göre yapılacak.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-listesi")]
        [Route("api/cari/aksesuar-denetim-listesi-kosul")]
        public HttpResponseMessage Delete(carikart_denetim_aksesuar aksesuar)
        {
            //AcekaResult acekaResult = null;
            carikartRepository = new CarikartRepository();
            carikart_denetim_aksesuar aksdenetim = new carikart_denetim_aksesuar();
            if (aksesuar != null)
            {
                aksdenetim = carikartRepository.CarikartDenetimAksesuar(aksesuar.carikart_id, aksesuar.tip);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("carikart_id", aksesuar.carikart_id);
                fields.Add("sira", aksesuar.sira);
                fields.Add("tip", aksesuar.tip);
                CrudRepository.Delete("carikart_denetim_aksesuar", new string[] { "carikart_id", "sira", "tip" }, fields);
                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = aksesuar.carikart_id.ToString() });

        }
        #endregion

        #region Carikart Aksesuar Koşullar
        /// <summary>
        /// Carikart Aksesuar Denetim listesi. Tip = 0 (sıfır) olacak.
        /// </summary>
        /// <returns>
        /// [
        ///     {
        ///         "carikart_id": 2,
        ///         "tip": 0,
        ///         "sira": 1,
        ///         "degistiren_carikart_id": 2189,
        ///         "degistiren_tarih": "2017-11-23T12:10:07",
        ///         "aksesuarkart_id": 42592,
        ///         "renk_id": 0,
        ///         "miktar": 1,
        ///         "kosul": "s.siparisturu||=||'0'|&|isnull(g.CUSTCODE:'')||çok.içermeyen||'*USPUA*;*TWPUT*;*ECDID*'",
        ///         "stok_kodu": "Apoşt ve kart stiker",
        ///         "stok_adi": "poşet ve kart stiker",
        ///         "birim_id_1": 1,
        ///         "birim_adi": "Adet",
        ///         "orjinal_stok_adi": "",
        ///         "orjinal_stok_kodu": "poşt ve kart stiker",
        ///         "renk_adi": "Her Hangi Bir Renk"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-listesi")]
        [Route("api/cari/aksesuar-denetim-listesi-kosullar")]
        public HttpResponseMessage CarikartAksDenetimKosulListesi()
        {
            carikartRepository = new CarikartRepository();
            carikartDenetimKosul = carikartRepository.CarikartAksesuarKosulList();

            if (carikartDenetimKosul != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, carikartDenetimKosul.Select(b => new
                {
                    grup_adi=b.grup_adi,
                    sira = b.sira,
                    param_tanim=b.param_tanim,
                    param_field_name = b.param_field_name,
                    cevap_liste_sql = b.cevap_liste_sql,
                    direkt_kosul = b.direkt_kosul,
                    operator_liste = b.operator_liste,
                    tip = b.tip,

                }).ToList());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Carikart Aksesuar Koşul. Tip = 0 (sıfır) olacak.
        /// </summary>
        /// <param name="grup_adi"></param>
        /// <param name="sira"></param>
        /// <returns>
        /// [
        ///     {
        ///         "siparisturu_id": 1,
        ///         "siparisturu_tanim": "Genel-Üretim"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-kosullar")]
        [Route("api/cari/aksesuar-denetim-kosullar/{sira},{grup_adi}")]
        public HttpResponseMessage CarikartAksesuarKosullar(byte sira,string grup_adi)
        {
            var grup = HttpUtility.HtmlDecode(grup_adi);
            carikartRepository = new CarikartRepository();
            List<string> carikartdenetimkosullari = new List<string>();
            carikartdenetimkosullari = carikartRepository.CarikartAksesuarKosullari(sira, grup);

            if (carikartdenetimkosullari != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, carikartdenetimkosullari);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Aksesuar Koşul Ekleme.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-kosullar")]
        [Route("api/cari/aksesuar-denetim-kosullar")]
        public HttpResponseMessage CarikartAksesuarKosullar(carikart_denetim_aksesuar_kosullar aksesuar)
        {
            AcekaResult acekaResult = null;
            if (aksesuar != null)
            {
                aksesuar.degistiren_tarih = DateTime.Now;
                aksesuar.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<carikart_denetim_aksesuar_kosullar>.Insert(aksesuar, "carikart_denetim_aksesuar_kosullar");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = null });
        }

        /// <summary>
        /// Aksesuar Koşul Güncelleme.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-kosullar")]
        [Route("api/cari/aksesuar-denetim-kosullar")]
        public HttpResponseMessage CarikartAksesuarKosullarPut(carikart_denetim_aksesuar_kosullar aksesuar)
        {
            AcekaResult acekaResult = null;
            carikartRepository = new CarikartRepository();
             carikart_denetim_aksesuar_kosullar aksdenetim = new carikart_denetim_aksesuar_kosullar();
            if (aksesuar != null)
            {
                carikartDenetimKosul = carikartRepository.CarikartAksesuarKosulList(aksesuar.sira, aksesuar.grup_adi);
                if (aksdenetim != null)
                {
                    aksdenetim.cevap_liste_sql = aksesuar.cevap_liste_sql;
                    aksdenetim.degistiren_carikart_id = aksesuar.degistiren_carikart_id;
                    aksdenetim.degistiren_tarih = aksesuar.degistiren_tarih;
                    aksdenetim.direkt_kosul = aksesuar.direkt_kosul;
                    aksdenetim.grup_adi = aksesuar.grup_adi;
                    aksdenetim.operator_liste = aksesuar.operator_liste;
                    aksdenetim.sira = aksesuar.sira;
                    aksdenetim.tip = aksesuar.tip;
                    aksdenetim.param_field_name = aksesuar.param_field_name;
                    aksdenetim.param_tanim = aksesuar.param_tanim;
                }
                acekaResult = CrudRepository<carikart_denetim_aksesuar_kosullar>.Update(aksdenetim, new string[] { "grup_adi", "sira" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful"});
        }

        /// <summary>
        /// Aksesuar Koşul Silme.
        /// </summary>
        /// <param name="aksesuar"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/cari/aksesuar-denetim-kosullar")]
        [Route("api/cari/aksesuar-denetim-kosullar")]
        public HttpResponseMessage CarikartAksesuarKosullarDel(carikart_denetim_aksesuar_kosullar aksesuar)
        {
            carikartRepository = new CarikartRepository();
            carikart_denetim_aksesuar_kosullar aksdenetim = new carikart_denetim_aksesuar_kosullar();
            if (aksesuar != null)
            {
                carikartDenetimKosul = carikartRepository.CarikartAksesuarKosulList(aksesuar.sira, aksesuar.grup_adi);
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("sira", aksesuar.sira);
                fields.Add("tip", aksesuar.grup_adi);
                CrudRepository.Delete("carikart_denetim_aksesuar_kosullar", new string[] { "grup_adi", "sira" }, fields);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });

        }

        #endregion
    }
}
