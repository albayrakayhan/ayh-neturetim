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
using System.Web.Http;
using System.Web.Http.Cors;
using static aceka.web_api.Models.CarikartModels.DepokartPostModel;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Depo işlemlerine ait Metodlar
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DepoController : ApiController
    {
        #region Degiskenler
        private cari_kart cari = null;
        private Muhasebe muhasebe = null;
        private List<cari_kart> carikartlar = null;
        private List<giz_sirket> sirketler = null;
        private DepokartRepository depoRepository = null;
        private ParametreRepository parametreRepository = null;
        private CarikartRepository carikartRepository = null;
        #endregion

        /// <summary>
        /// Depo Kartı arama metodu. 
        /// Not: Parametrelerden en az birtanesi dolu dolu olarak gönderilmelidir!
        /// </summary>
        /// <param name="carikart_id">Opsiyonel</param>
        /// <param name="cari_unvan">Opsiyonel</param>
        /// <param name="ozel_kod">Opsiyonel</param>
        /// <param name="carikart_tipi_id">Opsiyonel</param>
        /// <returns>   
        /// "carikodu": 100000001360,
        ///"unvan": "AJARA DEPO",
        ///"ozel_kod": "",
        ///"giz_kodu": 0,
        ///"statu": true,
        ///"cari_tipi": 3,
        ///"cari_tipi_adi": "Üretim Depo",
        ///"cari_turu": 3,
        ///"cari_turu_adi": "Lokasyon",
        ///"fiyattipi": "",
        ///"adres": "BOBOKVATI VILLAGE BATUMI KOBULETI",
        ///"telefon": "",
        ///"email": "",
        ///"websitesi": "",
        ///"para_birimi": null,
        ///"odeme_sekli_id": 0,
        ///"ilgili_sube_id": 0,
        ///"finans_sorumlu_carikart_id": 0,
        ///"satin_alma_sorumlu_carikart_id": 100000000953,
        ///"satis_sorumlu_carikart_id": 100000000955,
        ///"ana_cari_unvan": "",
        ///"ana_carikart_id": 0
        ///</returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl ="api/depo/depo-bul")]
        [Route("api/depo/depo-bul")]
        public HttpResponseMessage DepoAra(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", byte carikart_tipi_id = 0)
        {
            depoRepository = new DepokartRepository();
            carikartlar = depoRepository.DepoBul(carikart_id, cari_unvan, ozel_kod, carikart_tipi_id);
            if (carikartlar != null)
            {
                var depoOzet = carikartlar.Select(d => new
                {
                    carikodu = d.carikart_id,
                    unvan = d.cari_unvan,
                    d.ozel_kod,
                    giz_kodu = d.giz_yazilim_kodu,
                    statu = d.statu,
                    cari_tipi = d.giz_sabit_carikart_tipi.carikart_tipi_id,
                    cari_tipi_adi = d.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    cari_turu = d.giz_sabit_carikart_turu.carikart_turu_id,
                    cari_turu_adi = d.giz_sabit_carikart_turu.carikart_turu_adi,
                    d.fiyattipi,
                    adres = d.carikart_genel_adres[0].adres,
                    telefon = d.carikart_genel_adres[0].tel1,
                    email = d.carikart_genel_adres[0].email,
                    websitesi = d.carikart_genel_adres[0].websitesi,
                    para_birimi = d.carikart_finans.pb,
                    //25.01.2017 ilave edildi. AA
                    //parametre_cari_odeme_sekli tablosunda "cari_odeme_sekli_id" olan field bu tabloda "odeme_tipi" olarak tutuluyor                    
                    odeme_sekli_id = d.carikart_finans.odeme_tipi,

                    //İlgili Şube Carikart
                    ilgili_sube_id = d.carikart_finans.ilgili_sube_carikart_id,

                    //Finans Sorumlu
                    finans_sorumlu_carikart_id = d.carikart_finans.finans_sorumlu_carikart_id,

                    //Satın Alma ve Satış Sorumluları
                    satin_alma_sorumlu_carikart_id = d.carikart_firma_ozel.satin_alma_sorumlu_carikart_id,
                    satis_sorumlu_carikart_id = d.carikart_firma_ozel.satis_sorumlu_carikart_id,
                    ana_cari_unvan = d.ana_cari_unvan,
                    ana_carikart_id = d.ana_carikart_id
                });


                return Request.CreateResponse(HttpStatusCode.OK, depoOzet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        /// <summary>
        /// Depo kart arama metodu (özet bilgileri getirir)
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        ///{
        ///"carikart_id":100120000028,
        ///"statu": true,
        ///"cari_unvan": "AYHAN SATIN ALMA DEPOcu",
        ///"sirket_adi": "Giz Yazılım",
        ///"sirket_id": 1,
        ///"carikart_turu_id": 3,
        ///"stok_yeri_tip_adi": "Depo",
        ///"carikart_tipi_id": 2,
        ///"transfer_depo_id": 0,
        ///"giz_yazilim_kodu": 0,
        ///"ozel_kod": null,
        ///"earsiv_seri": "awe",
        ///"efatura_seri": "asew",
        ///"muh_kod": "399",
        ///"masraf_merkezi": "",
        ///"masraf_merkezi_id": 0,
        ///"acilis_tarihi": null,
        ///"kapanis_tarihi": null,
        ///"stokyeri_yeri_kapandi": false,
        ///"bagli_stokyeri_id": 0,
        ///"baglistokyeri_unvan": ""
        ///}
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/depo/genel/{carikart_id}")]
        public HttpResponseMessage Genel(long carikart_id)
        {
            depoRepository = new DepokartRepository();
            var depo = depoRepository.Getir(carikart_id);
            if (depo != null)
            {
                var depoOzet = new
                {
                    carikart_id = depo.carikart_id,

                    statu = depo.statu,
                    cari_unvan = depo.cari_unvan, //Stok yeri adına gelecek.
                    sirket = depo.giz_sirket.sirket_adi,
                    sirket_id = depo.giz_sirket.sirket_id, // depo yeri değiştiğinde almak için eklendi.
                    //stok_yeri_adi = depo.giz_sirket.sirket_adi,
                    carikart_turu_id = depo.giz_sabit_carikart_turu.carikart_turu_id,
                    stok_yeri_tip_adi = depo.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    //depo.giz_sabit_carikart_tipi.carikart_tipi_adi,
                    carikart_tipi_id = depo.giz_sabit_carikart_tipi.carikart_tipi_id,
                    transfer_depo_id = depo.transfer_depo_id,
                    giz_yazilim_kodu = depo.giz_yazilim_kodu,
                    ozel_kod = depo.ozel_kod,
                    earsiv_seri = depo.carikart_earsiv.earsiv_seri,
                    efatura_seri = depo.carikart_efatura.efatura_seri,
                    muh_kod = depo.carikart_muhasebe.muh_kod,
                    masraf_merkezi = depo.muhasebe_tanim_masrafmerkezleri.masraf_merkezi_adi,
                    masraf_merkezi_id = depo.muhasebe_tanim_masrafmerkezleri.masraf_merkezi_id,
                    acilis_tarihi = depo.carikart_stokyeri.acilis_tarihi,
                    kapanis_tarihi = depo.carikart_stokyeri.kapanis_tarihi,
                    //eski halistokyeri_yeri_kapandi = String.IsNullOrWhiteSpace(depo.carikart_stokyeri.kapanis_tarihi.ToString()) ? true : false,
                    kapali = depo.carikart_stokyeri.kapali,
                    ana_carikart_id = depo.ana_carikart_id,
                    baglistokyeri_unvan = depo.ana_cari_unvan,
                    kayit_silindi = depo.kayit_silindi,
                    transfer_depo_kullan=depo.carikart_stokyeri.transfer_depo_kullan,

                };
                return Request.CreateResponse(HttpStatusCode.OK, depoOzet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// carikart_id boş ise insert, dolu ise update işlemi çalışır!
        /// </summary>
        /// <param name="depokart"></param>
        /// <returns>
        /// {
        ///  "carikart_id":100120000028,
        ///  "statu": true,
        ///  "cari_unvan": "AYHAN SATIN ALMA DEPOcu",
        ///  "sirket_adi": "Giz Yazılım",
        ///  "sirket_id": 1,
        ///  "carikart_turu_id": 3,
        ///  "stok_yeri_tip_adi": "Depo",
        ///  "carikart_tipi_id": 2,
        ///  "transfer_depo_id": 0,
        ///  "giz_yazilim_kodu": 0,
        ///  "ozel_kod": null,
        ///  "earsiv_seri": "awe",
        ///  "efatura_seri": "asew",
        ///  "muh_kod": "399",
        ///  "masraf_merkezi": "",
        ///  "masraf_merkezi_id": 0,
        ///  "acilis_tarihi": null,
        ///  "kapanis_tarihi": null,
        ///  "stokyeri_yeri_kapandi": false,
        ///  "bagli_stokyeri_id": 0,
        ///  "baglistokyeri_unvan": ""
        ///}
        /// </returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/depo/genel")]
        public HttpResponseMessage Post(Depokart depokart)
        {
            AcekaResult acekaResult = null;
            if (depokart != null)
            {
                carikartRepository = new CarikartRepository();
                depokart.degistiren_tarih = DateTime.Now;


                if (depokart.carikart_id > 0)
                {
                    //update
                    #region Fields Carikart
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", depokart.carikart_id);
                    fields.Add("cari_unvan", depokart.cari_unvan);
                    fields.Add("degistiren_tarih", depokart.degistiren_tarih);
                    fields.Add("degistiren_carikart_id", depokart.degistiren_carikart_id);
                    fields.Add("statu", depokart.statu);
                    fields.Add("transfer_depo_id", depokart.transfer_depo_id);
                    fields.Add("giz_yazilim_kodu", depokart.giz_yazilim_kodu);
                    fields.Add("ozel_kod", depokart.ozel_kod);
                    fields.Add("ana_carikart_id", depokart.ana_carikart_id);
                    fields.Add("carikart_tipi_id", depokart.carikart_tipi_id);
                    fields.Add("carikart_turu_id", depokart.carikart_turu_id);

                    //fields.Add("acilis_tarihi", depokart.acilis_tarihi);
                    //fields.Add("kayit_silindi", depokart.kapanis_tarihi);


                    #endregion
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                }
                else
                {
                    //insert
                    acekaResult = CrudRepository<Depokart>.Insert(depokart, "carikart", new string[] { "carikart_id", "sirket", "stok_yeri_adi", "stok_yeri_tipi_adi", "earsiv_seri", "efatura_seri", "carikart_muhasebe", "masraf_merkezi", "masraf_merkezi_id", "muh_kod", "carikart_stokyeri", "acilis_tarihi", "kapanis_tarihi", "sirket_id", "sirket_adi", "sirket_unvan", "kapali", "transfer_depo_kullan" });
                    depokart.carikart_id = Convert.ToInt64(acekaResult.RetVal);

                }
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    #region carikart_earsiv
                    carikart_earsiv carikartEarsiv = carikartRepository.CarikartEarsivBilgileri(depokart.carikart_id, depokart.earsiv_seri);
                    if (carikartEarsiv != null)
                    {
                        //Update
                        carikartEarsiv.carikart_id = depokart.carikart_id;
                        carikartEarsiv.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartEarsiv.degistiren_tarih = DateTime.Now;
                        carikartEarsiv.sene = DateTime.Now.Year;
                        carikartEarsiv.earsiv_seri = depokart.earsiv_seri;

                        var earsivRetval = CrudRepository<carikart_earsiv>.Update(carikartEarsiv, "carikart_id");
                    }
                    else
                    {
                        //insert earsiv_seri,degistiren_carikart_id ve degistiren_tarih alanları boş geçilemez.
                        carikartEarsiv = new carikart_earsiv();
                        carikartEarsiv.carikart_id = depokart.carikart_id;
                        carikartEarsiv.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartEarsiv.degistiren_tarih = DateTime.Now;
                        carikartEarsiv.sene = DateTime.Now.Year;
                        carikartEarsiv.earsiv_seri = depokart.earsiv_seri;

                        var earsivRetval = CrudRepository<carikart_earsiv>.Insert(carikartEarsiv, new string[] { "masraf_merkezi_adi" });

                    }
                    #endregion
                    #region carikart_efatura
                    carikart_efatura carikartEfatura = carikartRepository.CarikartEfaturaBilgileri(depokart.carikart_id);
                    if (carikartEfatura != null)
                    {
                        //Update
                        carikartEfatura.carikart_id = depokart.carikart_id;
                        carikartEfatura.degistiren_carikart_id = Tools.PersonelId;
                        carikartEfatura.degistiren_tarih = DateTime.Now;
                        carikartEfatura.efatura_seri = depokart.efatura_seri;

                        var earsivRetval = CrudRepository<carikart_efatura>.Update(carikartEfatura, "carikart_id");
                    }
                    else
                    {
                        //insert earsiv_seri,degistiren_carikart_id ve degistiren_tarih alanları boş geçilemez.
                        carikartEfatura = new carikart_efatura();
                        carikartEfatura.carikart_id = depokart.carikart_id;
                        carikartEfatura.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartEfatura.degistiren_tarih = DateTime.Now;
                        carikartEfatura.efatura_seri = depokart.efatura_seri;

                        var efaturaRetval = CrudRepository<carikart_efatura>.Insert(carikartEfatura, new string[] { });

                    }
                    #endregion

                    #region carikart_muhasebe
                    carikart_muhasebe carikartMuhasebe = carikartRepository.CarikartMuhasebeBilgileri(depokart.carikart_id);
                    if (carikartMuhasebe != null)
                    {
                        //Upate
                        carikartMuhasebe.carikart_id = depokart.carikart_id;
                        carikartMuhasebe.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartMuhasebe.degistiren_tarih = DateTime.Now;
                        carikartMuhasebe.sirket_id = depokart.sirket_id;
                        carikartMuhasebe.sene = DateTime.Now.Year;
                        carikartMuhasebe.muh_kod = depokart.muh_kod;
                        carikartMuhasebe.masraf_merkezi_id = depokart.masraf_merkezi_id;

                        var muhasebeRetval = CrudRepository<carikart_muhasebe>.Update(carikartMuhasebe, "carikart_id", new string[] { "masraf_merkezi_adi" });
                    }
                    else
                    {
                        //insert
                        carikartMuhasebe = new carikart_muhasebe();
                        carikartMuhasebe.carikart_id = depokart.carikart_id;
                        carikartMuhasebe.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartMuhasebe.degistiren_tarih = DateTime.Now;
                        carikartMuhasebe.sirket_id = depokart.sirket_id;
                        carikartMuhasebe.sene = DateTime.Now.Year;
                        carikartMuhasebe.muh_kod = depokart.muh_kod;
                        carikartMuhasebe.masraf_merkezi_id = depokart.masraf_merkezi_id;

                        var muhasebeRetval = CrudRepository<carikart_muhasebe>.Insert(carikartMuhasebe, new string[] { "masraf_merkezi_adi" });
                    }
                    #endregion

                    #region muhasebe_tanim_masrafmerkezleri
                    //Burayı yapmaya gerek yok.
                    //MuhasebeRepository muhasebeRepository = new MuhasebeRepository();
                    //muhasebe_tanim_masrafmerkezleri muhasebe = muhasebeRepository.MuhasebeMasrafMerkezBilgileri(depokart.carikart_id);
                    //if (carikartEarsiv != null)
                    //{
                    //    //Update
                    //    muhasebe.degistiren_carikart_id = depokart.degistiren_carikart_id;
                    //    muhasebe.degistiren_tarih = DateTime.Now;
                    //    muhasebe.ana_masraf_merkezi_id = 0;
                    //    muhasebe.grup_adi = null;
                    //    muhasebe.grup_kodu = null;
                    //    muhasebe.kayit_silindi = false;
                    //    muhasebe.masraf_merkezi_adi = null;
                    //    muhasebe.masraf_merkezi_id = 0;
                    //    muhasebe.masraf_merkezi_kodu = null;
                    //    muhasebe.muhkod_ek = null;
                    //    muhasebe.personel_carikart_id_1 = 0;
                    //    muhasebe.sirket_id = 0;
                    //    muhasebe.statu = true;
                    //    muhasebe.stokyeri_carikart_id = depokart.carikart_id;
                    //    var muhasebeRetval = CrudRepository<muhasebe_tanim_masrafmerkezleri>.Update(muhasebe, "carikart_id");
                    //}
                    //else
                    //{
                    //    //insert
                    //    muhasebe = new muhasebe_tanim_masrafmerkezleri();
                    //    muhasebe.degistiren_carikart_id = depokart.degistiren_carikart_id;
                    //    muhasebe.degistiren_tarih = DateTime.Now;
                    //    muhasebe.ana_masraf_merkezi_id = 0;
                    //    muhasebe.grup_adi = null;
                    //    muhasebe.grup_kodu = null;
                    //    muhasebe.kayit_silindi = false;
                    //    muhasebe.masraf_merkezi_adi = null;
                    //    muhasebe.masraf_merkezi_id = 0;
                    //    muhasebe.masraf_merkezi_kodu = null;
                    //    muhasebe.muhkod_ek = null;
                    //    muhasebe.personel_carikart_id_1 = 0;
                    //    muhasebe.sirket_id = 0;
                    //    muhasebe.statu = true;
                    //    muhasebe.stokyeri_carikart_id = depokart.carikart_id;

                    //    var muhasebeRetval = CrudRepository<muhasebe_tanim_masrafmerkezleri>.Insert(muhasebe, new string[] { "masraf_merkezi_adi" });

                    //}
                    #endregion
                    #region carikart_stokyeri
                    carikart_stokyeri carikartStokyeri = carikartRepository.CarikartStokYerilgileri(depokart.carikart_id);
                    if (carikartStokyeri != null)
                    {
                        //Update
                        carikartStokyeri.carikart_id = depokart.carikart_id;
                        carikartStokyeri.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartStokyeri.transfer_depo_kullan = Convert.ToBoolean(depokart.transfer_depo_kullan);
                        carikartStokyeri.degistiren_tarih = DateTime.Now;

                        carikartStokyeri.acilis_tarihi = depokart.acilis_tarihi;
                        //carikartStokyeri.kapali = depokart;
                        carikartStokyeri.kapali = Convert.ToBoolean(depokart.kapali);
                        //if (depokart.kapali == true)
                        //{
                        //    carikartStokyeri.kapanis_tarihi = DateTime.Now;
                        //    carikartStokyeri.kapali = true;
                        //}
                        carikartStokyeri.kapanis_tarihi = depokart.kapanis_tarihi;
                        var earsivRetval = CrudRepository<carikart_stokyeri>.Update(carikartStokyeri, "carikart_id");
                    }
                    else
                    {
                        //insert
                        carikartStokyeri = new carikart_stokyeri();
                        carikartStokyeri.carikart_id = depokart.carikart_id;
                        carikartStokyeri.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartStokyeri.degistiren_tarih = DateTime.Now;
                        carikartStokyeri.kapanis_tarihi = depokart.kapanis_tarihi;
                        carikartStokyeri.acilis_tarihi = depokart.acilis_tarihi;
                        carikartStokyeri.kapali = depokart.kapali;
                        carikartStokyeri.transfer_depo_kullan = depokart.transfer_depo_kullan;

                        var earsivRetval = CrudRepository<carikart_stokyeri>.Insert(carikartStokyeri);

                    }
                    #endregion


                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = depokart.carikart_id.ToString() });
                    //return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
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
            // return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
        }

        /// <summary>
        /// update işlemi çalışıyor!
        /// </summary>
        /// <param name="depokart"></param>
        /// <returns>
        /// {
        ///  "carikart_id":100120000028,
        ///  "statu": true,
        ///  "cari_unvan": "AYHAN SATIN ALMA DEPOcu",
        ///  "sirket_adi": "Giz Yazılım",
        ///  "sirket_id": 1,
        ///  "carikart_turu_id": 3,
        ///  "stok_yeri_tip_adi": "Depo",
        ///  "carikart_tipi_id": 2,
        ///  "transfer_depo_id": 0,
        ///  "giz_yazilim_kodu": 0,
        ///  "ozel_kod": null,
        ///  "earsiv_seri": "awe",
        ///  "efatura_seri": "asew",
        ///  "muh_kod": "399",
        ///  "masraf_merkezi": "",
        ///  "masraf_merkezi_id": 0,
        ///  "acilis_tarihi": null,
        ///  "kapanis_tarihi": null,
        ///  "stokyeri_yeri_kapandi": false,
        ///  "bagli_stokyeri_id": 0,
        ///  "baglistokyeri_unvan": ""
        ///}
        /// </returns>
        [HttpPut]
        [CustAuthFilter]
        [Route("api/depo/genel")]
        public HttpResponseMessage Put(Depokart depokart)
        {
            AcekaResult acekaResult = null;
            if (depokart != null)
            {
                carikartRepository = new CarikartRepository();
                depokart.degistiren_tarih = DateTime.Now;
                if (depokart.carikart_id > 0)
                {
                    //update
                    #region Fields Carikart
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", depokart.carikart_id);
                    fields.Add("cari_unvan", depokart.cari_unvan);
                    fields.Add("degistiren_tarih", depokart.degistiren_tarih);
                    fields.Add("degistiren_carikart_id", depokart.degistiren_carikart_id);
                    fields.Add("statu", depokart.statu);
                    fields.Add("transfer_depo_id", depokart.transfer_depo_id);
                    fields.Add("giz_yazilim_kodu", depokart.giz_yazilim_kodu);
                    fields.Add("ozel_kod", depokart.ozel_kod);
                    fields.Add("ana_carikart_id", depokart.ana_carikart_id);
                    fields.Add("carikart_tipi_id", depokart.carikart_tipi_id);
                    fields.Add("carikart_turu_id", depokart.carikart_turu_id);

                    //fields.Add("acilis_tarihi", depokart.acilis_tarihi);
                    //fields.Add("kayit_silindi", depokart.kapanis_tarihi);


                    #endregion
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                }
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    #region carikart_earsiv
                    carikart_earsiv carikartEarsiv = carikartRepository.CarikartEarsivBilgileri(depokart.carikart_id, depokart.earsiv_seri);
                    if (carikartEarsiv != null)
                    {
                        //Update
                        carikartEarsiv.carikart_id = depokart.carikart_id;
                        carikartEarsiv.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartEarsiv.degistiren_tarih = DateTime.Now;
                        carikartEarsiv.sene = DateTime.Now.Year;
                        carikartEarsiv.earsiv_seri = depokart.earsiv_seri;

                        var earsivRetval = CrudRepository<carikart_earsiv>.Update(carikartEarsiv, "carikart_id");
                    }

                    #endregion
                    #region carikart_efatura
                    carikart_efatura carikartEfatura = carikartRepository.CarikartEfaturaBilgileri(depokart.carikart_id);
                    if (carikartEfatura != null)
                    {
                        //Update
                        carikartEfatura.carikart_id = depokart.carikart_id;
                        carikartEfatura.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartEfatura.degistiren_tarih = DateTime.Now;
                        carikartEfatura.efatura_seri = depokart.efatura_seri;

                        var earsivRetval = CrudRepository<carikart_efatura>.Update(carikartEfatura, "carikart_id");
                    }

                    #endregion
                    #region carikart_muhasebe
                    carikart_muhasebe carikartMuhasebe = carikartRepository.CarikartMuhasebeBilgileri(depokart.carikart_id);
                    if (carikartMuhasebe != null)
                    {
                        //Upate
                        carikartMuhasebe.carikart_id = depokart.carikart_id;
                        carikartMuhasebe.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartMuhasebe.degistiren_tarih = DateTime.Now;
                        carikartMuhasebe.sirket_id = depokart.sirket_id;
                        carikartMuhasebe.sene = DateTime.Now.Year;
                        carikartMuhasebe.muh_kod = depokart.muh_kod;
                        carikartMuhasebe.masraf_merkezi_id = depokart.masraf_merkezi_id;

                        var muhasebeRetval = CrudRepository<carikart_muhasebe>.Update(carikartMuhasebe, "carikart_id", new string[] { "masraf_merkezi_adi" });
                    }

                    #endregion
                    #region carikart_stokyeri
                    carikart_stokyeri carikartStokyeri = carikartRepository.CarikartStokYerilgileri(depokart.carikart_id);
                    if (carikartStokyeri != null)
                    {
                        //Update
                        carikartStokyeri.carikart_id = depokart.carikart_id;
                        carikartStokyeri.degistiren_carikart_id = depokart.degistiren_carikart_id;
                        carikartStokyeri.degistiren_tarih = DateTime.Now;

                        carikartStokyeri.acilis_tarihi = depokart.acilis_tarihi;
                        //carikartStokyeri.kapali = depokart;
                        if (depokart.kapali == true)
                        {
                            carikartStokyeri.kapanis_tarihi = DateTime.Now;
                            carikartStokyeri.kapali = true;
                        }
                        carikartStokyeri.kapanis_tarihi = depokart.kapanis_tarihi;
                        carikartStokyeri.transfer_depo_kullan = depokart.transfer_depo_kullan;

                       var earsivRetval = CrudRepository<carikart_stokyeri>.Update(carikartStokyeri, "carikart_id");
                    }

                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = depokart.carikart_id.ToString() });
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
            // return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
        }

        /// <summary>
        /// Depo İletişim Sekmesi.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        ///[  
        ///{
        ///"yetkili_ad_soyad": "",
        ///"yetkili_tel": "", ->Cep Telefonu
        ///"email": "",
        ///"websitesi": "",
        ///"ulke_id": 995,
        ///"sehir_id": 0,
        ///"ilce_id": 0,
        ///"semt_id": 0,
        ///"postakodu": "",
        ///"fax": "",
        ///"tel1": "",
        ///"tel2": "",
        ///"adres": " BATUMI ",
        ///"kayit_silindi": false,
        ///"carikart_id": 100000001950
        /// }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/depo/iletisim-bilgileri/{carikart_id}")]
        public HttpResponseMessage DepokartIletisimGetir(long carikart_id)
        {
            depoRepository = new DepokartRepository();
            var adresler = depoRepository.DepokartAdresleriniGetir(carikart_id);
            if (adresler != null)
            {
                var iletisim = new
                {
                    adresunvan = adresler.adresunvan,
                    yetkili_ad_soyad = adresler.yetkili_ad_soyad,
                    yetkili_tel = adresler.yetkili_tel,
                    email = adresler.email,
                    ulke_id = adresler.ulke_id,
                    sehir_id = adresler.sehir_id,
                    ilce_id = adresler.ilce_id,
                    semt_id = adresler.semt_id,
                    postakodu = adresler.postakodu,
                    fax = adresler.fax,
                    tel1 = adresler.tel1,
                    tel2 = adresler.tel2,
                    adres = adresler.adres,
                    kayit_silindi = adresler.kayit_silindi,
                    carikart_id = adresler.carikart_id,
                    carikart_adres_id = adresler.carikart_adres_id,
                    websitesi = adresler.websitesi,
                };
                return Request.CreateResponse(HttpStatusCode.OK, iletisim);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Depo Adresleri için POST metodu. (Depo Kart İletişim Sekmesi.)
        /// </summary>
        /// <param name="depoiletisim"></param>
        /// <returns>
        ///[  
        ///{
        ///"yetkili_ad_soyad": "",
        ///"yetkili_tel": "", ->Cep Telefonu
        ///"email": "",
        ///"websitesi": "",
        ///"ulke_id": 995,
        ///"sehir_id": 0,
        ///"ilce_id": 0,
        ///"semt_id": 0,
        ///"postakodu": "",
        ///"fax": "",
        ///"tel1": "",
        ///"tel2": "",
        ///"adres": " BATUMI ",
        ///"kayit_silindi": false,
        ///"carikart_id": 100000001950
        /// }
        ///]
        /// </returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/depo/iletisim-bilgileri")]
        public HttpResponseMessage DepokartIletisimGetir(carikartgenel_adres depoiletisim)
        {
            depoRepository = new DepokartRepository();
            carikartgenel_adres adres = new carikartgenel_adres();
            AcekaResult acekaResult = null;
            var adresler = depoRepository.DepokartAdresleriniGetir(depoiletisim.carikart_adres_id);
            if (adresler.carikart_id == 0)
            {
                //insert
                adres.degistiren_carikart_id = -1;
                adres.carikart_id = depoiletisim.carikart_id;
                adres.degistiren_tarih = DateTime.Now;
                adres.yetkili_ad_soyad = depoiletisim.yetkili_ad_soyad;
                adres.yetkili_tel = depoiletisim.yetkili_tel;
                adres.email = depoiletisim.email;
                adres.websitesi = depoiletisim.websitesi;
                adres.ulke_id = depoiletisim.ulke_id;
                adres.sehir_id = depoiletisim.sehir_id;
                adres.ilce_id = depoiletisim.ilce_id;
                adres.semt_id = depoiletisim.semt_id;
                adres.postakodu = depoiletisim.postakodu;
                adres.fax = depoiletisim.fax;
                adres.tel1 = depoiletisim.tel1;
                adres.tel2 = depoiletisim.tel2;
                adres.adres = depoiletisim.adres;
                adres.adresunvan = depoiletisim.adresunvan; //Resmi Ünvan?
                adres.kayit_silindi = depoiletisim.kayit_silindi;
                adres.adrestanim = "Depo Kart İletişim";
                adres.adres_tipi_id = "DI";
                acekaResult = CrudRepository<carikartgenel_adres>.Insert(adres, "carikart_genel_adres", new string[] { "carikart_adres_id" });//"carikart_adres_id", "kayit_silindi", "faturaadresi" 

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = adres.carikart_id.ToString() });
            }

            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }


        /// <summary>
        /// Depo Adresleri için PUT metodu. (Depo Kart İletişim Sekmesi.)
        /// </summary>
        /// <param name="depoiletisim"></param>
        /// <returns>
        ///[  
        ///{
        ///"yetkili_ad_soyad": "",
        ///"yetkili_tel": "", ->Cep Telefonu
        ///"email": "",
        ///"websitesi": "",
        ///"ulke_id": 995,
        ///"sehir_id": 0,
        ///"ilce_id": 0,
        ///"semt_id": 0,
        ///"postakodu": "",
        ///"fax": "",
        ///"tel1": "",
        ///"tel2": "",
        ///"adres": " BATUMI ",
        ///"kayit_silindi": false,
        ///"carikart_id": 100000001950
        /// }
        ///]
        /// </returns>
        [HttpPut]
        [CustAuthFilter]
        [Route("api/depo/iletisim-bilgileri")]
        public HttpResponseMessage DepokartIletisimGetirPut(carikartgenel_adres depoiletisim)
        {
            depoRepository = new DepokartRepository();
            carikartgenel_adres adres = new carikartgenel_adres();
            AcekaResult acekaResult = null;

            #region normal update
            //var adresler = depoRepository.DepokartAdresleriniGetir(depoiletisim.carikart_id);
            //if (adresler != null && adresler.carikart_adres_id > 0)
            //{
            //    //update
            //    adresler.adresunvan = depoiletisim.adresunvan; //Resmi Ünvan?
            //    adresler.yetkili_ad_soyad = depoiletisim.yetkili_ad_soyad;
            //    adresler.yetkili_tel = depoiletisim.yetkili_tel;
            //    adresler.email = depoiletisim.email;
            //    adresler.websitesi = depoiletisim.websitesi;
            //    adresler.ulke_id = depoiletisim.ulke_id;
            //    adresler.sehir_id = depoiletisim.sehir_id;
            //    adresler.ilce_id = depoiletisim.ilce_id;
            //    adresler.semt_id = depoiletisim.semt_id;
            //    adresler.postakodu = depoiletisim.postakodu;
            //    adresler.fax = depoiletisim.fax;
            //    adresler.tel1 = depoiletisim.tel1;
            //    adresler.tel2 = depoiletisim.tel2;
            //    adresler.adres = depoiletisim.adres;
            //    adresler.kayit_silindi = depoiletisim.kayit_silindi;
            //    adresler.carikart_id = depoiletisim.carikart_id;
            //    adresler.degistiren_carikart_id = -1;
            //    adresler.degistiren_tarih = DateTime.Now;
            //    adresler.carikart_adres_id = depoiletisim.carikart_adres_id;

            //    acekaResult = CrudRepository<carikart_genel_adres>.Update(adresler, "carikart_id", new string[] { "carikart_adres_id" }); //"carikart_adres_id" 


            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.Successful { message = "NO RECORD" });
            //} 
            #endregion

            var adresler = depoRepository.DepokartAdresleriniGetir(depoiletisim.carikart_id);
            if (adresler != null && adresler.carikart_id > 0)
            {
                //update
                adresler.adresunvan             = depoiletisim.adresunvan; //Resmi Ünvan?
                adresler.yetkili_ad_soyad       = depoiletisim.yetkili_ad_soyad;
                adresler.yetkili_tel            = depoiletisim.yetkili_tel;
                adresler.email                  = depoiletisim.email;
                adresler.websitesi              = depoiletisim.websitesi;
                adresler.ulke_id                = depoiletisim.ulke_id;
                adresler.sehir_id               = depoiletisim.sehir_id;
                adresler.ilce_id                = depoiletisim.ilce_id;
                adresler.semt_id                = depoiletisim.semt_id;
                adresler.postakodu              = depoiletisim.postakodu;
                adresler.fax                    = depoiletisim.fax;
                adresler.tel1                   = depoiletisim.tel1;
                adresler.tel2                   = depoiletisim.tel2;
                adresler.adres                  = depoiletisim.adres;
                adresler.kayit_silindi          = depoiletisim.kayit_silindi;
                adresler.carikart_id            = depoiletisim.carikart_id;
                adresler.degistiren_carikart_id = Tools.PersonelId;
                adresler.degistiren_tarih       = DateTime.Now;
                adresler.carikart_adres_id      = depoiletisim.carikart_adres_id;
                adresler.adrestanim             = "Depo Kart İletişim";
                adresler.adres_tipi_id          = "DI";

                acekaResult = CrudRepository<carikart_genel_adres>.Update(adresler, "carikart_id", new string[] { "carikart_adres_id" }); //"carikart_adres_id" 
            }
            else
            {
                //insert
                adresler.degistiren_carikart_id = Tools.PersonelId;
                adresler.carikart_id            = depoiletisim.carikart_id;
                adresler.degistiren_tarih       = DateTime.Now;
                adresler.adresunvan             = depoiletisim.adresunvan; //Resmi Ünvan?
                adresler.yetkili_ad_soyad       = depoiletisim.yetkili_ad_soyad;
                adresler.yetkili_tel            = depoiletisim.yetkili_tel;
                adresler.email                  = depoiletisim.email;
                adresler.websitesi              = depoiletisim.websitesi;
                adresler.ulke_id                = depoiletisim.ulke_id;
                adresler.sehir_id               = depoiletisim.sehir_id;
                adresler.ilce_id                = depoiletisim.ilce_id;
                adresler.semt_id                = depoiletisim.semt_id;
                adresler.postakodu              = depoiletisim.postakodu;
                adresler.fax                    = depoiletisim.fax;
                adresler.tel1                   = depoiletisim.tel1;
                adresler.tel2                   = depoiletisim.tel2;
                adresler.adres                  = depoiletisim.adres;
                adresler.kayit_silindi          = depoiletisim.kayit_silindi;
                adresler.adrestanim             = "Depo Kart İletişim";
                adresler.adres_tipi_id          = "DI";

                acekaResult = CrudRepository<carikart_genel_adres>.Insert(adresler, "carikart_genel_adres", new string[] { "carikart_adres_id", "degistiren_tarih" });//"carikart_adres_id", "kayit_silindi", "faturaadresi"
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }

        /// <summary>
        /// Depo Rapor Parametreleri için POST ve PUT metodları. (Depo Kart Rapor Sekmesi.)
        /// </summary>
        /// <param name="parametre"></param>
        /// <returns>
        ///   {
        ///   "carikart_id": 100000001360,
        ///   "cari_parametre_1": 14,
        ///   "cari_parametre_2": 15,
        ///   "cari_parametre_3": 16,
        ///   "cari_parametre_4": 17,
        ///   "cari_parametre_5": 18,
        ///   "cari_parametre_6": 19,
        ///   "cari_parametre_7": 0
        ///   }
        /// </returns>
        [HttpPut]
        [CustAuthFilter]
        [Route("api/depo/rapor-parametre")]
        public HttpResponseMessage RaporParametre(carikart_rapor_parametre parametre)
        {
            depoRepository = new DepokartRepository();
            carikartRepository = new CarikartRepository();
            carikart_rapor_parametre adres = new carikart_rapor_parametre();
            AcekaResult acekaResult = null;

            var parametreler = carikartRepository.CarikartParametreleriniGetir(parametre.carikart_id);
            if (parametreler != null && parametreler.carikart_id > 0)
            {
                //update
                parametreler.carikart_id = parametre.carikart_id;
                parametreler.degistiren_tarih = DateTime.Now;
                parametreler.degistiren_carikart_id = Tools.PersonelId;
                parametreler.cari_parametre_1 = parametre.cari_parametre_1;
                parametreler.cari_parametre_2 = parametre.cari_parametre_2;
                parametreler.cari_parametre_3 = parametre.cari_parametre_3;
                parametreler.cari_parametre_4 = parametre.cari_parametre_4;
                parametreler.cari_parametre_5 = parametre.cari_parametre_5;
                parametreler.cari_parametre_6 = parametre.cari_parametre_6;
                parametreler.cari_parametre_7 = parametre.cari_parametre_7;

                acekaResult = CrudRepository<carikart_rapor_parametre>.Update(parametreler,"carikart_id");  
            }
            else
            {
                try
                {
                    //insert
                    //parametreler.carikart_id = parametre.carikart_id;
                    parametre.degistiren_tarih = DateTime.Now;
                    parametre.degistiren_carikart_id = Tools.PersonelId;
                    //parametreler.cari_parametre_1 = parametre.cari_parametre_1;
                    //parametreler.cari_parametre_2 = parametre.cari_parametre_2;
                    //parametreler.cari_parametre_3 = parametre.cari_parametre_3;
                    //parametreler.cari_parametre_4 = parametre.cari_parametre_4;
                    //parametreler.cari_parametre_5 = parametre.cari_parametre_5;
                    //parametreler.cari_parametre_6 = parametre.cari_parametre_6;
                    //parametreler.cari_parametre_7 = parametre.cari_parametre_7;

                    acekaResult = CrudRepository<carikart_rapor_parametre>.Insert(parametre, "carikart_rapor_parametre");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
               
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }


        ///// <summary>
        ///// CariController'da api/cari/cari-parametreleri-getir/{carikart_id} 'yi kullanıyoruz.
        ///// İPTAL.  parametre.depokartRaporParametreleri kullanılancak. Her bir Select için.
        ///// Depo kartının Rapor Parametrelerinin Listesini verir. (Depo Kartı Rapor Parametreleri Sekmesi)
        ///// </summary>
        ///// <returns>
        ///// Geriye döndürülen json object : 
        ///// [
        /////     {
        /////         carikart_id: 100000000001,
        /////         cari_parametre_1: 0,
        /////         cari_parametre_2: 0,
        /////         cari_parametre_3: 0,
        /////         cari_parametre_4: 0,
        /////         cari_parametre_5: 0,
        /////         cari_parametre_6: 0,
        /////         cari_parametre_7: 0
        /////     }
        ///// ]
        ///// </returns>
        //[HttpGet]
        //[CustAuthFilter]
        //[Route("api/depo/raporparametreleri")]
        //public HttpResponseMessage DepoRaporParametreleriniGetir()
        //{
        //    parametreRepository = new ParametreRepository();
        //    var parametreler = parametreRepository.StokkartRaporParametreGetir();
        //    if (parametreler != null && parametreler.Count > 0)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, parametreler.Select(p => new
        //        {
        //            p.parametre_id,
        //            p.parametre,
        //            p.tanim
        //        }));
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        //    }
        //}

        /// <summary>
        /// /// İptal oldu. Özel Alanlar Sekmesini getiren Metod
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
        [Route("api/depo/ozel-alanlar/{carikart_id}")]
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
        /// İptal oldu. Cari Özel Alanlar POST insert, update işlemleri
        /// </summary>
        /// <param name="ozelalanlar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/depo/ozel-alanlar")]
        public HttpResponseMessage OzelAlanGetir(carikart_ozelalanlar ozelalanlar)
        {
            AcekaResult acekaResult = null;
            carikartRepository = new CarikartRepository();
            carikart_firma_ozel ckozelanlar = carikartRepository.CarikartOzelalanlarGetir(ozelalanlar.carikart_id);
            //if (ozelalanlar != null && ozelalanlar.carikart_id > 0)
            //{

            if (ckozelanlar != null && ozelalanlar.carikart_id > 0)
            {
                //update
                ckozelanlar.degistiren_carikart_id = -1;
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
                ckozelanlar.degistiren_carikart_id = -1;
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

        /// <summary>
        /// Depo kartında Cari koda göre fiyat Tiplerini Verir. (Depo Kartı Fiyat Tipleri Sekmesi)
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///  {
        ///    "carikart_id": 100000001040,
        ///    "fiyattipi": "AF",
        ///    "statu": false,
        ///    "varsayilan": false,
        ///    "kayit_silindi": false,
        ///    "degistiren_carikart_id": -1
        ///  },
        ///  {
        ///    "carikart_id": 100000001040,
        ///    "fiyattipi": "FF",
        ///    "statu": true,
        ///    "varsayilan": true,
        ///    "kayit_silindi": false,
        ///    "degistiren_carikart_id": -1
        ///  },
        ///  {
        ///    "carikart_id": 100000001040,
        ///    "fiyattipi": "TSF",
        ///    "statu": false,
        ///    "varsayilan": false,
        ///    "kayit_silindi": false,
        ///    "degistiren_carikart_id": -1
        ///  }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/depo/fiyat-tipleri/{carikart_id}")]
        public HttpResponseMessage FiyatTipiGetir(long carikart_id)
        {
            depoRepository = new DepokartRepository();
            var fiyatTiplari = depoRepository.DepokartFiyatTipleri(carikart_id);
            if (fiyatTiplari != null && fiyatTiplari.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fiyatTiplari.Select(f => new
                {
                    f.carikart_id,
                    f.fiyattipi_adi,
                    f.fiyattipi,
                    f.statu,
                    f.varsayilan,
                    f.kayit_silindi,
                    f.degistiren_carikart_id,

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Depo kartının Fiyat Tipleri Listesini Verir. (Depo Kartı Fiyat Tipleri Sekmesi) Silme Olmayacak. Eklenen Kalacak.
        /// </summary>
        /// <param name="fiyatTipi"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter]
        [Route("api/depo/fiyat-tipleri")]
        public HttpResponseMessage FiyatTipiGetir(carikartfiyat_tipi fiyatTipi)
        {
            if (fiyatTipi != null && !string.IsNullOrEmpty(fiyatTipi.fiyattipi) && fiyatTipi.carikart_id > 0)
            {
                AcekaResult acekaResult = null;

                depoRepository = new DepokartRepository();
                var fiyatTipiDetay = depoRepository.DepokartFiyatTipiDetay(fiyatTipi.carikart_id, fiyatTipi.fiyattipi);

                if (fiyatTipiDetay != null )
                {
                    //update
                    fiyatTipiDetay.degistiren_carikart_id   = Tools.PersonelId;
                    fiyatTipiDetay.degistiren_tarih         = DateTime.Now;
                    fiyatTipiDetay.fiyattipi                = fiyatTipi.fiyattipi;
                    fiyatTipiDetay.statu                    = fiyatTipi.statu;
                    fiyatTipiDetay.varsayilan               = fiyatTipi.varsayilan;
                    fiyatTipiDetay.kayit_silindi            = fiyatTipi.kayit_silindi;
                    fiyatTipiDetay.carikart_id              = fiyatTipi.carikart_id;

                    acekaResult = CrudRepository<carikart_fiyat_tipi>.Update(fiyatTipiDetay, new string[] { "carikart_id", "fiyattipi" }, new string[] { "fiyattipi_adi", "statu", "varsayilan" });
                }
                else
                {
                    //insert
                    fiyatTipi.degistiren_tarih = DateTime.Now;
                    if (fiyatTipi.varsayilan == true) // Eğer varsayılan true gönderilirse, cariye ait diğer varsayılan fiyattipi değerinin varsayılanı değiştiriliyor
                    {
                        Dictionary<string, object> fields = new Dictionary<string, object>();
                        fields.Add("carikart_id", fiyatTipi.carikart_id);
                        fields.Add("varsayilan", false);
                        var retVal = CrudRepository.Update("carikart_fiyat_tipi", "carikart_id", fields);
                    }

                   acekaResult = CrudRepository<carikartfiyat_tipi>.Insert(fiyatTipi, "carikart_fiyat_tipi", new string[] { "fiyattipi_adi" });

                    //if (fiyatTipi.kayit_silindi == false)
                    //{
                    //    Dictionary<string, object> fields = new Dictionary<string, object>();
                    //    fields.Add("carikart_id", fiyatTipi.carikart_id);
                    //    fields.Add("fiyattipi", fiyatTipi.fiyattipi);
                    //    fields.Add("varsayilan", fiyatTipi.varsayilan);
                    //    fields.Add("statu", fiyatTipi.statu);
                    //    fields.Add("kayit_silindi", false);
                    //    acekaResult = CrudRepository.Update("carikart_fiyat_tipi", new string[] { "carikart_id", "fiyattipi" }, fields);

                    //    acekaResult = CrudRepository<carikart_fiyat_tipi>.Update(fiyatTipiDetay, new string[] { "carikart_id", "fiyattipi" }, new string[] { "fiyattipi_adi" });
                    //}
                    //else
                    //{
                    //    acekaResult = CrudRepository<carikart_fiyat_tipi>.Insert(fiyatTipi, "carikart_fiyat_tipi", new string[] { "fiyattipi_adi" });
                    //}

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
        /// Select için Depo yerleri listesini getiren metod. carikart_turu_id =3 ve carikart_tipi_id=3 olanlar.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter]
        [Route("api/depo/depo-liste")]
        public HttpResponseMessage DepoListesi()
        {
            depoRepository = new DepokartRepository();
            carikartlar = depoRepository.DepoListesi();

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

        //Parametre olduğu için api/parametre/depo-kart-liste kullanılacak.
        ///// <summary>
        ///// Parametre Carikart Rapor listesini veren metod.
        ///// </summary>
        ///// <returns>
        ///// [
        /////  {
        /////   "parametre_id": 1,
        /////   "degistiren_carikart_id": 0,
        /////   "degistiren_tarih": "0001-01-01T00:00:00",
        /////   "kayit_silindi": false,
        /////   "parametre": 1,
        /////   "kaynak_1_parametre_id": 0,
        /////   "kaynak_2_parametre_id": 0,
        /////   "kaynak_3_parametre_id": 0,
        /////   "kaynak_4_parametre_id": 0,
        /////   "kod": null,
        /////   "tanim": "Ana Grup 1",
        /////   "grup1": null,
        /////   "grup2": null,
        /////   "sira": null,
        /////   "parametre_grubu": 0,
        /////   "dil_1_tanim": null,
        /////   "dil_2_tanim": null,
        /////   "dil_3_tanim": null,
        /////   "dil_4_tanim": null,
        /////   "dil_5_tanim": null
        ///// }
        ///// ]
        ///// </returns>
        ///// <param name="parametre_grubu"></param>
        ///// <param name="parametre"></param>
        //[CustAuthFilter(ApiUrl = "api/admin/cari-raporlar")]
        //[Route("api/admin/cari-raporlar/{parametre_grubu}")]
        //[HttpGet]
        //public HttpResponseMessage CariRaporGetir(byte parametre_grubu,byte parametre)
        //{
        //    parametreRepository = new ParametreRepository();

        //    var crapor = parametreRepository.CariRaporGetir(parametre_grubu,parametre);
        //    if (crapor != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, crapor.Select(cr => new
        //        {
        //            cr.parametre_id,
        //            cr.degistiren_carikart_id,
        //            cr.degistiren_tarih,
        //            cr.kayit_silindi,
        //            cr.parametre,
        //            cr.kaynak_1_parametre_id,
        //            cr.kaynak_2_parametre_id,
        //            cr.kaynak_3_parametre_id,
        //            cr.kaynak_4_parametre_id,
        //            cr.kod,
        //            cr.tanim,
        //            cr.grup1,
        //            cr.grup2,
        //            cr.sira,
        //            cr.parametre_grubu,
        //            cr.dil_1_tanim,
        //            cr.dil_2_tanim,
        //            cr.dil_3_tanim,
        //            cr.dil_4_tanim,
        //            cr.dil_5_tanim,
        //        }));
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
        //    }
        //}
    }
}
