using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models.ParametreModel;
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
    /// Select Box larda kullanılacak liste metodları yer alır
    /// </summary>
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParametreController : ApiController
    {
        #region Degiskenler
        private ParametreRepository parametreRepository = null;
        private List<cari_parametreler> personelparametreadi = null;
        private List<giz_setup> parametregenel = null;
        private List<parametre_genel> parametre_genel = null;
        #endregion

        #region Ülke, Şehir, İlçe, Semt
        /// <summary>
        /// Ülke Listesini getirir
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///     ulke_id: 1,
        ///     ulke_adi: "ABD",
        ///     ulke_kodu: "USA",
        ///     ulke_adi_dil_1: "USA",
        ///     ulke_telefon_kodu: "1"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/ulkeler")]
        public HttpResponseMessage Ulkeler()
        {
            parametreRepository = new ParametreRepository();
            var ulkeler = parametreRepository.UlkeleriGetir();

            if (ulkeler != null && ulkeler.Count > 0)
            {
                var anonymousUlkeler = ulkeler.Select(u => new
                {
                    u.ulke_id,
                    u.ulke_adi,
                    ulke_kodu = u.ulke_plaka_kodu,
                    u.ulke_adi_dil_1,
                    u.ulke_telefon_kodu,

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, anonymousUlkeler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Şehirler listesi
        /// </summary>
        /// <param name="ulke_id">Ülke seçimine göre şehir listesi getirilir</param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         sehir_id: 1,
        ///         sehir_adi: "ADANA",
        ///         sehir_dunya_kodu: "",
        ///         sehir_telefon_kodu: "",
        ///         sehir_plaka_kodu: "01"
        ///     }
        /// ]
        /// </returns>

        [HttpGet]
        [Route("api/parametre/sehirler")]
        public HttpResponseMessage Sehirler(short ulke_id)
        {
            parametreRepository = new ParametreRepository();

            var sehirler = parametreRepository.SehirleriGetir(ulke_id);

            if (sehirler != null && sehirler.Count > 0)
            {
                var anonymousSehirler = sehirler.Select(s => new
                {
                    s.sehir_id,
                    s.sehir_adi,
                    s.sehir_dunya_kodu,
                    s.sehir_telefon_kodu,
                    s.sehir_plaka_kodu,

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, anonymousSehirler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// İlçe listesini getirir
        /// </summary>
        /// <param name="sehir_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         ilce_id: 5,
        ///         ilce_adi: "ALADAĞ(KARSANTI)",
        ///         ups_id: 12
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/ilceler")]
        public HttpResponseMessage ilceler(short sehir_id)
        {
            parametreRepository = new ParametreRepository();

            var ilceler = parametreRepository.IlceleriGetir(sehir_id);

            if (ilceler != null && ilceler.Count > 0)
            {
                var anonymousIlceler = ilceler.Select(i => new
                {
                    i.ilce_id,
                    i.ilce_adi,
                    i.ups_id

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, anonymousIlceler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Semt listesini getirir.
        /// </summary>
        /// <param name="ilce_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         ilce_id: 1922,
        ///         semt_adi: "AVCILAR",
        ///         posta_kodu: 34310
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/semtler")]
        public HttpResponseMessage semtler(short ilce_id)
        {
            parametreRepository = new ParametreRepository();

            var semtler = parametreRepository.SemtleriGetir(ilce_id);

            if (semtler != null && semtler.Count > 0)
            {
                var anonymousSemtler = semtler.Select(i => new
                {
                    i.semt_id,
                    i.semt_adi,
                    i.posta_kodu

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, anonymousSemtler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Vergi Dairelerinin listesini getirir.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         "ulke_id": 90,
        ///         "vergi_daire_no": "055110",
        ///         "vergi_daire_adi": "19 MAYIS MALMÜDÜRLÜĞÜ ",
        ///         "vergi_daire_adi_kucuk": "19 mayıs malmüdürlüğü",
        ///         "statu": true,
        ///         "kayit_silindi": false,
        ///         "degistiren_carikart_id": 0,
        ///         "degistiren_tarih": "0001-01-01T00:00:00"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/vergidaireleri")]
        public HttpResponseMessage vergiDaireleri()
        {
            parametreRepository = new ParametreRepository();

            var vergidairesi = parametreRepository.UlkeVergiDaireleri();

            if (vergidairesi != null && vergidairesi.Count > 0)
            {
                var anonymousSemtler = vergidairesi.Select(i => new
                {
                    i.ulke_id,
                    i.vergi_daire_no,
                    i.vergi_daire_adi,
                    vergi_daire_adi_kucuk = i.vergi_daire_adi.ToLower(),
                    i.statu,
                    i.kayit_silindi,
                    i.degistiren_carikart_id,
                    i.degistiren_tarih
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, anonymousSemtler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        #endregion

        #region Cari Kart Tipi,  Cari Kart Türü, Bankalar, Birim ve Genel

        /// <summary>
        /// Tüm Cari Kart Tiplerin Listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///     carikart_tipi_id: 11,
        ///     carikart_tipi_adi: "Alıcı",
        ///     aciklama: "Alıcılar"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/cari-kart-tipleri")]
        public HttpResponseMessage CariKartTipleri()
        {
            parametreRepository = new ParametreRepository();

            var turler = parametreRepository.CariKartTipleri();
            if (turler != null && turler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, turler.Select(c => new
                {
                    c.carikart_tipi_id,
                    c.carikart_tipi_adi,
                    c.aciklama
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari Kart Tip Listesi
        /// </summary>
        /// <param name="carikart_turu_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///     carikart_tipi_id: 11,
        ///     carikart_tipi_adi: "Alıcı",
        ///     aciklama: "Alıcılar"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/cari-kart-tipleri/{carikart_turu_id}")]
        public HttpResponseMessage CariKartTipleri(short carikart_turu_id)
        {
            parametreRepository = new ParametreRepository();

            var turler = parametreRepository.CariKartTipleri(carikart_turu_id);
            if (turler != null && turler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, turler.Select(c => new
                {
                    c.carikart_tipi_id,
                    c.carikart_tipi_adi,
                    c.aciklama
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari Kart Tür Listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///     carikart_turu_id: 11,
        ///     carikart_turu_adi: "Alıcı",
        ///     aciklama: "Alıcılar"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/cari-kart-turleri")]
        public HttpResponseMessage CariKartTurleri()
        {
            parametreRepository = new ParametreRepository();

            var turler = parametreRepository.CariKartTurleri();
            if (turler != null && turler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, turler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Tüm Banka Listesi
        /// </summary>
        /// <param name="ulke_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         banka_id: 100,
        ///         banka_adi: "Adabank A.Ş.",
        ///         banka_eft_kodu: "0100",
        ///         banka_swift_kodu: "ADABTRIS "
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/banka-liste")]
        public HttpResponseMessage BankaListeleri()
        {
            parametreRepository = new ParametreRepository();

            var bankalar = parametreRepository.BankaListesi();
            if (bankalar != null && bankalar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bankalar.Select(b => new
                {
                    b.banka_id,
                    b.banka_adi,
                    banka_adi_kucuk = b.banka_adi.ToLower(),
                    b.banka_eft_kodu,
                    b.banka_swift_kodu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Tüm Banka Şubelerinin Listesi.
        /// </summary>
        /// <param name="banka_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         banka_sube_id: 4391,
        ///         banka_sube_kodu: "004400",
        ///         banka_sube_adi: "adana",
        ///         sube_adi_kucuk,
        ///         statu: true
        ///     }
        /// ]       
        /// </returns>
        [HttpGet]
        [Route("api/parametre/banka-subeler/{banka_id}")]
        public HttpResponseMessage BankaSubeler(short banka_id)
        {
            parametreRepository = new ParametreRepository();

            var subeler = parametreRepository.BankaSubeListeleri(banka_id);
            if (subeler != null && subeler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, subeler.Select(b => new
                {
                    b.banka_sube_id,
                    b.banka_sube_kodu,
                    b.banka_sube_adi,
                    sube_adi_kucuk = b.banka_sube_adi.ToLower(),
                    b.statu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Bankalar Listesi
        /// </summary>
        /// <param name="ulke_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         banka_id: 100,
        ///         banka_adi: "Adabank A.Ş.",
        ///         banka_eft_kodu: "0100",
        ///         banka_swift_kodu: "ADABTRIS "
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/bankalar")]
        public HttpResponseMessage Bankalar(short ulke_id)
        {
            parametreRepository = new ParametreRepository();

            var bankalar = parametreRepository.Bankalar(ulke_id);
            if (bankalar != null && bankalar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, bankalar.Select(b => new
                {
                    b.banka_id,
                    b.banka_adi,
                    banka_adi_kucuk = b.banka_adi.ToLower(),
                    b.banka_eft_kodu,
                    b.banka_swift_kodu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Banka Şubeler Listesi. Not sehir_id bilgisi boş da gönderilebilir. sehir_id bilgisi olmasığında bankanın tüm şubeleri gelir. sehir_id bilgisi belirtilirse ilgili şehirdeki şubeler listelenir.
        /// </summary>
        /// <param name="banka_id"></param>
        /// <param name="sehir_id"></param>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         banka_sube_id: 4391,
        ///         banka_sube_kodu: "004400",
        ///         sube_adi_kucuk: "adana",
        ///         sube_adi_kucuk,
        ///         statu: true
        ///     }
        /// ]       
        /// </returns>
        [HttpGet]
        [Route("api/parametre/banka-subeler")]
        public HttpResponseMessage BankaSubeler(short banka_id, Nullable<short> sehir_id)
        {
            parametreRepository = new ParametreRepository();

            var subeler = parametreRepository.BankaSubeler(banka_id, sehir_id);
            if (subeler != null && subeler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, subeler.Select(b => new
                {
                    b.banka_sube_id,
                    b.banka_sube_kodu,
                    b.banka_sube_adi,
                    sube_adi_kucuk = b.banka_sube_adi.ToLower(),
                    b.statu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Birimler Listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         birim_id: 1,
        ///         birim_adi: "Adet",
        ///         birim_kod: "ad",
        ///         ondalik: 0,
        ///         statu: true
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/birimler")]
        public HttpResponseMessage Birimler()
        {
            parametreRepository = new ParametreRepository();

            var birimler = parametreRepository.Birimler();
            if (birimler != null && birimler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, birimler.Select(b => new
                {
                    b.birim_id,
                    b.birim_adi,
                    b.birim_kod,
                    b.ondalik
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Cari Finans Bİlgileri Ödeme Şekilleri Listesi.
        /// ÇEK, HAVALE/EFT,NAKİT,SENET
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         cari_odeme_sekli_id: 1,
        ///         cari_odeme_sekli: "ÇEK"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/cari-odeme-sekilleri")]
        public HttpResponseMessage CariOdemeSekilleri()
        {
            parametreRepository = new ParametreRepository();

            var odemeSekilleri = parametreRepository.CariOdemeSekilleri();
            if (odemeSekilleri != null && odemeSekilleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, odemeSekilleri.Select(c => new
                {
                    c.cari_odeme_sekli_id,
                    c.cari_odeme_sekli
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Para Birimleri Listesini getiri
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         pb: 0,
        ///         pb_adi: "Türk Lirası",
        ///         merkezbankasi_kodu: "TRY",
        ///         kusurat_tanimi: "Krş"
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/para-birimleri")]
        public HttpResponseMessage ParaBirimleri()
        {
            parametreRepository = new ParametreRepository();

            var paraBirimleri = parametreRepository.ParaBirimleri();
            if (paraBirimleri != null && paraBirimleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, paraBirimleri.Select(pb => new
                {
                    pb.pb,
                    pb.pb_adi,
                    pb.pb_kodu,
                    pb.merkezbankasi_kodu,
                    pb.kusurat_tanimi
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Fiyat Tipleri Listesini getiri
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         fiyattipi: "AF",
        ///         fiyattipi_adi: "Sabit Alış Fiyatı",
        ///         fiyattipi_turu: "A",
        ///         kdv_dahil: false,
        ///         kullanici_giris: false
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/fiyat-tipleri")]
        public HttpResponseMessage FiyatTipleri()
        {
            parametreRepository = new ParametreRepository();

            var fiyatTipleri = parametreRepository.FiyatTipleri();
            if (fiyatTipleri != null && fiyatTipleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fiyatTipleri.Select(ft => new
                {
                    ft.fiyattipi,
                    ft.fiyattipi_adi,
                    ft.fiyattipi_turu,
                    ft.kdv_dahil,
                    ft.kullanici_giris
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Fiyat Tipleri Listesini getiri
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///    {
        ///         "fiyattipi": "AF",
        ///         "fiyattipi_adi": "Sabit Alış Fiyatı",
        ///         "fiyattipi_turu": "A",
        ///         "kdv_dahil": false,
        ///         "kullanici_giris": false,
        ///         "varsayilan": true,
        ///         "statu": true
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/depo-fiyat-tipleri/{carikart_id}")]
        public HttpResponseMessage DepoFiyatTipleri(long carikart_id)
        {
            parametreRepository = new ParametreRepository();

            var fiyatTipleri = parametreRepository.FiyatTipleri(carikart_id);
            if (fiyatTipleri != null && fiyatTipleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fiyatTipleri.Select(ft => new
                {
                    ft.fiyattipi,
                    ft.fiyattipi_adi,
                    ft.fiyattipi_turu,
                    ft.kdv_dahil,
                    ft.kullanici_giris,
                    ft.varsayilan,
                    ft.statu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Fiyat Tipleri Listesini getiri
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///    {
        ///         "fiyattipi": "AF",
        ///         "fiyattipi_adi": "Sabit Alış Fiyatı",
        ///         "fiyattipi_turu": "A",
        ///         "kdv_dahil": false,
        ///         "kullanici_giris": false,
        ///         "varsayilan": true,
        ///         "statu": true
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/depo-fiyat-tiplers/{carikart_id}")]
        public HttpResponseMessage DepoFiyatTips(long carikart_id)
        {
            parametreRepository = new ParametreRepository();

            var fiyatTipleri = parametreRepository.FiyatTiplers(carikart_id);
            if (fiyatTipleri != null && fiyatTipleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, fiyatTipleri.Select(ft => new
                {
                    ft.fiyattipi,
                    ft.fiyattipi_adi,
                    ft.fiyattipi_turu,
                    ft.kdv_dahil,
                    ft.kullanici_giris,
                    ft.varsayilan,
                    ft.statu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        #endregion

        /// <summary>
        /// KDV listesini veren metod
        /// </summary>
        /// <returns>
        /// [
        ///     {
        ///         kod: "",
        ///         oran: ""
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/kdv-liste")]
        public HttpResponseMessage StokkartKDVgetir()
        {
            parametreRepository = new ParametreRepository();
            var kdvler = parametreRepository.KDVler();
            if (kdvler != null)

            {
                return Request.CreateResponse(HttpStatusCode.OK, kdvler.Select(kdv => new
                {
                    kdv.kod,
                    kdv.oran
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stokkart genel de kullanılan talimat listesidir
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///     talimatturu_id:"",
        ///     tanim:""
        /// },
        /// {
        ///     talimatturu_id:"",
        ///     tanim:""
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/talimat-liste")]
        public HttpResponseMessage StokkartTalimatgetir()
        {
            parametreRepository = new ParametreRepository();
            var talimatlar = parametreRepository.TalimatListesiTanim();
            if (talimatlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, talimatlar.Select(talimat => new
                {
                    talimat.talimatturu_id,
                    talimat.tanim,
                    talimat.varsayilan_fasoncu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stokkart tiplerinin listelendiği metod
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/parametre/stokkart-tipleri")]
        public HttpResponseMessage StokkartTipleri()
        {
            parametreRepository = new ParametreRepository();
            var kartTipleri = parametreRepository.StokkartTipleri();
            if (kartTipleri != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, kartTipleri.Select(tip => new
                {
                    tip.stokkarttipi,
                    tip.tanim,
                    tip.otostokkodu,
                    tip.parametre_grubu,
                    tip.stokkartturu
                }));

            }
            else
            {
                //return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stokkart türlerinin listelendiği metod
        /// </summary>
        /// <returns>
        /// [
        /// {
        /// stokkartturu: 0,
        /// tanim: "Mamül"
        /// },
        /// {
        /// stokkartturu: 1,
        /// tanim: "Yarı Mamül"
        /// },
        /// {
        /// stokkartturu: 2,
        /// tanim: "İlk Madde"
        /// },
        /// {
        /// stokkartturu: 9,
        /// tanim: "Diğer"
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/stokkart-turleri")]
        public HttpResponseMessage StokkartTurleri()
        {
            parametreRepository = new ParametreRepository();
            var kartTipleri = parametreRepository.StokkartTurleri();
            if (kartTipleri != null)

            {
                return Request.CreateResponse(HttpStatusCode.OK, kartTipleri.Select(tip => new
                {
                    tip.stokkartturu,
                    tip.tanim
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Sezonların listesini veren metod
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/parametre/sezon")]
        public HttpResponseMessage SezonListesi()
        {
            parametreRepository = new ParametreRepository();
            var sezonlar = parametreRepository.Sezonlistesi();
            if (sezonlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sezonlar.Select(s => new
                {
                    s.sezon_id,
                    s.sezon_adi,
                    s.sezon_kodu
                    
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Renk listesini veren metod.
        /// NOT: "renk_adı" belirtilmediği durumda default olarak renk adına göre 100 kayıt gelir. 
        /// Harf yada kelime yazılırsa "LIKE '%%'" metodu çalışır.
        /// </summary>
        /// <param name="renk_adi"></param>
        /// <returns>
        /// [
        ///    {
        ///   "renk_id": 60635,
        ///    "renk_kodu": "",
        ///    "renk_adi": "%60 puma royal-puma royal-white",
        ///    "renk_rgb": null
        ///   },
        ///   {
        ///     "renk_id": 74650,
        ///     "renk_kodu": "",
        ///     "renk_adi": "%80 puma royal-puma royal-white",
        ///     "renk_rgb": null
        ///   }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/renkler")]
        public HttpResponseMessage RenkListesi(string renk_adi = "")
        {
            parametreRepository = new ParametreRepository();
            var renkler = parametreRepository.RenkListesi(renk_adi);
            if (renkler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, renkler.Select(renk => new
                {
                    renk.renk_id,
                    renk.renk_kodu,
                    renk.renk_adi,
                    renk.renk_rgb
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Kumaşın renk listesini veren metod.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// [
        ///    {
        ///   "renk_id": 60635,
        ///    "renk_kodu": "",
        ///    "renk_adi": "%60 puma royal-puma royal-white",
        ///    "renk_rgb": null
        ///   },
        ///   {
        ///     "renk_id": 74650,
        ///     "renk_kodu": "",
        ///     "renk_adi": "%80 puma royal-puma royal-white",
        ///     "renk_rgb": null
        ///   }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/kumas-renk/{stokkart_id}")]
        public HttpResponseMessage KumasRenkListesi(long stokkart_id)
        {
            parametreRepository = new ParametreRepository();
            var renkler = parametreRepository.KumasRenk(stokkart_id);
            if (renkler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, renkler.Select(renk => new
                {
                    renk.renk_id,
                    renk.renk_kodu,
                    renk.renk_adi,
                    renk.renk_rgb
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Renk listesini veren metod.
        /// NOT: "renk_adı" belirtilmediği durumda default olarak renk adına göre 100 kayıt gelir. 
        /// Harf yada kelime yazılırsa "LIKE '%%'" metodu çalışır.
        /// </summary>
        /// <returns>
        /// [
        ///    {
        ///   "renk_id": 60635,
        ///    "renk_kodu": "",
        ///    "renk_adi": "%60 puma royal-puma royal-white",
        ///    "renk_rgb": null
        ///   },
        ///   {
        ///     "renk_id": 74650,
        ///     "renk_kodu": "",
        ///     "renk_adi": "%80 puma royal-puma royal-white",
        ///     "renk_rgb": null
        ///   }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/gtipListe")]
        public HttpResponseMessage gtipListesi()
        {
            //parametreRepository = new ParametreRepository();
            //var renkler = parametreRepository.gtipListesi();
            //if (renkler != null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, renkler.Select(renk => new
            //    {
            //        renk.renk_id,
            //        renk.renk_kodu,
            //        renk.renk_adi,
            //        renk.renk_rgb
            //    }));

            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            //}
            return null;
        }

        /// <summary>
        /// Parametre grubu ve parametreye göre select'lerin  liste verir.
        /// </summary>
        /// <param name="parametregrubu"></param>
        /// <param name="parametre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/parametre/stokkart-rapor-parametre/{parametregrubu},{parametre}")]
        public HttpResponseMessage StokkartRaporParametreleri(byte parametregrubu, byte parametre)
        {
            parametreRepository = new ParametreRepository();
            var stokkartParametreler = parametreRepository.StokkartRaporParametreleri(parametregrubu, parametre);
            if (stokkartParametreler != null)
            {
                List<StokkartRaporParametreler> parametreler = stokkartParametreler.Select(p => new StokkartRaporParametreler
                {

                    kod = p.kod,
                    parametre_id = p.parametre_id,
                    parametre=p.parametre,
                    parametre_grubu=p.parametre_grubu,
                    tanim = p.tanim
                }).ToList();

                //StokkartRaporParametre retParametreler = new StokkartRaporParametre
                //{
                //    parametre_adi = stokkartParametreler[0].parametre_adi,
                //    stokkartRaporParametreler = parametreler
                //};
                //return Request.CreateResponse(HttpStatusCode.OK, retParametreler);
                return Request.CreateResponse(HttpStatusCode.OK, parametreler);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new List<StokkartRaporParametreler> { });
            }
        }

        /// <summary>
        /// Stokkart  - > Genel - > Rapor parametreleri için GET liste
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="parametre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/parametre/stokkart-rapor-parametreleri/{stokkart_id},{parametre}")]
        public HttpResponseMessage StokkartRaporParametreleri(long stokkart_id, byte parametre)
        {
            parametreRepository = new ParametreRepository();
            var stokkartParametreler = parametreRepository.StokkartRaporParametreleri(stokkart_id, parametre);
            if (stokkartParametreler != null)
            {
                List<StokkartRaporParametreler> parametreler = stokkartParametreler.Select(p => new StokkartRaporParametreler
                {
                    kod = p.kod,
                    parametre_id = p.parametre_id,
                    tanim = p.tanim
                }).ToList();

                StokkartRaporParametre retParametreler = new StokkartRaporParametre
                {
                    parametre_adi = stokkartParametreler[0].parametre_adi,
                    stokkartRaporParametreler = parametreler
                };
                return Request.CreateResponse(HttpStatusCode.OK, retParametreler);

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new List<StokkartRaporParametreler> { });
            }
        }

        /// <summary>
        /// Stokkart  - > Genel - > Rapor parametreleri için GET liste. 
        /// Yeni kayıt oluşturulduğunda default değerler gelmesi için yapıldı. Kerem ayrıştıracak.
        /// </summary>
        /// <param name="parametregrubu"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/parametre/stokkart-rapor-parametgrup/{parametregrubu}")]
        public HttpResponseMessage StokkartRaporParametreleri(byte parametregrubu)
        {
            parametreRepository = new ParametreRepository();
            var stokkartParametreler = parametreRepository.StokkartRaporParametreleri(parametregrubu);
            if (stokkartParametreler != null)
            {
                List<StokkartRaporParametreler> parametreler = stokkartParametreler.Select(p => new StokkartRaporParametreler
                {
                    parametre_id = p.parametre_id,
                    parametre_adi = p.parametre_adi,
                    parametre=p.parametre,
                    parametre_grubu=p.parametre_grubu,
                    kod = p.kod,                   
                    tanim = p.tanim,
                   
                }).OrderByDescending(r=>r.parametre_adi).ToList();

                //StokkartRaporParametre retParametreler = new StokkartRaporParametre
                //{
                //    parametre_adi = stokkartParametreler[0].parametre_adi,
                //    stokkartRaporParametreler = parametreler
                //};
                return Request.CreateResponse(HttpStatusCode.OK, parametreler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message="No Record!"});
                //return Request.CreateResponse(HttpStatusCode.NoContent, new List<StokkartRaporParametreler> { });
            }
        }

        /// <summary>
        /// Resim ve dosyaları tür ve tanım listesini veren metod
        /// </summary>
        /// <returns>
        ///        [
        ///  {
        ///    "ekturu_id": 1,
        ///    "file_ext": "*.jpg, *.gif, *.tiff",
        ///    "file_types": "Jpeg (*.jpg)|*.jpg| Giff  (*.gif)|*.gif| Tiff (*.tiff)|*.tiff",
        ///    "preview": true,
        ///    "tanim": "Genel Resmi"
        ///  },
        ///  {
        ///    "ekturu_id": 2,
        ///    "file_ext": "*.jpg, *.gif, *.tiff",
        ///    "file_types": "Jpeg (*.jpg)|*.jpg| Giff  (*.gif)|*.gif| Tiff (*.tiff)|*.tiff",
        ///    "preview": true,
        ///    "tanim": "Resimler"
        ///  },
        ///  {
        ///    "ekturu_id": 3,
        ///    "file_ext": "*.pdf",
        ///    "file_types": "Pdf (*.pdf)|*.pdf",
        ///    "preview": false,
        ///    "tanim": "Dokümanları (Pdf)"
        ///  },
        ///  {
        ///    "ekturu_id": 9,
        ///    "file_ext": "*.*",
        ///    "file_types": "Tüm dosyalar (*.*)|*.*",
        ///    "preview": false,
        ///    "tanim": "Diğer Ekler"
        ///  }
        ///]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/dosyaturleri")]
        public HttpResponseMessage DosyaTurugetir()
        {
            parametreRepository = new ParametreRepository();
            var turler = parametreRepository.Dosyaturleri();
            if (turler != null)

            {
                return Request.CreateResponse(HttpStatusCode.OK, turler.Select(tur => new
                {
                    tur.ekturu_id,
                    tur.file_ext,
                    tur.file_types,
                    tur.preview,
                    tur.tanim
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Bedenlerin listesini veren metod. Model Kartta Kullanılacak.
        /// </summary>
        /// <param name="bedenGruplari">Virgül ile birleştirilerek beden grubu belirtilmelidir örnek veri bedenGruplari=A,B gibi</param>
        /// <returns>
        ///  [
        ///    {
        ///      "beden_id": 2,
        ///      "beden": "4XL",
        ///      "beden_tanimi": "4XL",
        ///      "bedengrubu": "A",
        ///      "sira": 1500
        ///    },
        ///    {
        ///      "beden_id": 3,
        ///      "beden": "5XL",
        ///      "beden_tanimi": "5XL",
        ///      "bedengrubu": "A",
        ///      "sira": 1510
        ///    }
        ///  ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/bedenler")]
        public HttpResponseMessage BedenleriGetir(string bedenGruplari)
        {
            parametreRepository = new ParametreRepository();
            var bedenler = parametreRepository.BedenleriGetir(bedenGruplari.Split(','));
            if (bedenler != null)

            {
                return Request.CreateResponse(HttpStatusCode.OK, bedenler.Select(beden => new
                {
                    beden.beden_id,
                    beden.beden,
                    beden.beden_tanimi,
                    beden.bedengrubu,
                    beden.sira
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// rapor parametreleri için label isimlerini döndüren liste 1den 7ye kadar label adları listelenir
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        ///{
        ///  "parametre": 3,
        ///  "parametre_adi": "Alt Grup 1",
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/parametre/cariparametreler")]
        public HttpResponseMessage PersonelParametreLabel()
        {
            parametreRepository = new ParametreRepository();
            personelparametreadi = parametreRepository.PersonelParametreleAdlari();
            if (personelparametreadi != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, personelparametreadi);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// takvim tablosunda tatil nedenlerinin seçilmesi için liste döner
        /// </summary>
        /// <returns>
        ///{
        ///"parametre_adi": "Haftasonu Tatili",
        ///"parametre_id": 161
        ///},
        ///{
        ///"parametre_adi": "Resmi Tatil",
        ///"parametre_id": 162
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/parametre/parametre_genel_tatilnedenleri")]
        public HttpResponseMessage Parametre_GenelTatilNedenleri()
        {
            parametreRepository = new ParametreRepository();
            parametre_genel = parametreRepository.ParametreGenel();
            if (parametre_genel != null)
            {
                var ayar = parametre_genel.Where(p => p.parametre_grup_id == "TATIL_NEDENI").Select(x => new
                {
                    x.parametre_adi,
                    x.parametre_id
                });
                return Request.CreateResponse(HttpStatusCode.OK, ayar);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// giz_setup yani sistemle ilgili önemli ayarların bulunduğu tablodur
        /// </summary>
        /// <returns>
        ///    "ayaradi": "TopluAktarimParametreAc",
        ///    "ayar": "1",
        ///    "ayaraciklama": "toplu aktarım da parametre aç 1 açma 0",
        ///    "ayar_grubu": "",
        ///    "veri_turu": "String",
        ///    "combo_degeri": null
        ///  },
        ///  {
        ///    "ayaradi": "YerelParaBirimi",
        ///    "ayar": "0",
        ///    "ayaraciklama": "Yerel Para Birimi Kodu",
        ///    "ayar_grubu": "Sistem Ayarları",
        ///    "veri_turu": "Integer",
        ///    "combo_degeri": [
        ///      {
        ///        "key": "0",
        ///        "value": "Türk Lirası"
        ///      },
        ///      {
        ///        "key": "1",
        ///        "value": "Amerikan Doları"
        ///      },
        ///      {
        ///        "key": "2",
        ///        "value": "İngiliz Sterlini"
        ///      },
        ///      {
        ///        "key": "3",
        ///        "value": "Euro"
        ///      }
        ///    ]
        ///  }
        /// </returns>
        [HttpGet]
        [Route("api/parametre/giz-setup")]
        public HttpResponseMessage ParametreGenelListe()
        {
            parametreRepository = new ParametreRepository();
            parametregenel = parametreRepository.giz_setup_parametreler();
            if (parametregenel != null)
            {
                var ayar = parametregenel.Select(x => new
                {
                    x.ayaradi,
                    x.ayar,
                    x.ayaraciklama,
                    x.ayar_grubu,
                    x.veri_turu,
                    x.combo_degeri,
                });
                return Request.CreateResponse(HttpStatusCode.OK, ayar);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// giz_setup tablosunun update bölümü
        /// </summary>
        /// <returns>
        ///{
        ///	"ayaradi": "YerelParaBirimi",
        /// "ayar": "0"
        ///}
        /// </returns>
        [HttpPut]
        [Route("api/parametre/giz-setup")]
        public HttpResponseMessage ParametreGenelListePut(giz_setup ayar)
        {
            AcekaResult acekaResult = null;
            if (ayar != null)
            {
                ayar.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("ayar", ayar.ayar);
                fields.Add("ayaradi", ayar.ayaradi);
                fields.Add("degistiren_carikart_id", ayar.degistiren_carikart_id);//sisteme giriş yapan değişikliği yapan kullanıcı bilgisi olacak
                string[] Wherefields = { "ayaradi" };
                acekaResult = CrudRepository.Update("giz_setup", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Üretim Yerlerini ve Menşei veren Get Metodu. 
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///    "uretimyeri_id": 104,
        ///    "uretimyeri_tanim": "AJARA-Georgia"
        ///  },
        ///  {
        ///    "uretimyeri_id": 106,
        ///    "uretimyeri_tanim": "MILTEKS-Turkey"
        ///  }
        ///]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/uretimyerleri")]
        public HttpResponseMessage UretimYeriGetir()
        {
            parametreRepository = new ParametreRepository();
            var uretimyerleri = parametreRepository.Uretimyer();
            if (uretimyerleri != null && uretimyerleri.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, uretimyerleri.Select(u => new
                {
                    u.uretimyeri_id,
                    u.uretimyeri_tanim,
                    u.uretimyeri_kod
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.Successful { message = "No Record!" });
            }
        }

        /// <summary>
        /// Sipariş Türü listesi 
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "siparisturu_id": 1,
        ///   "siparisturu_tanim": "Genel-Üretim",
        ///   "siparisturu_tanim_kucuk": "genel-üretim"
        /// },
        /// {
        ///   "siparisturu_id": 4,
        ///   "siparisturu_tanim": "Tamir",
        ///   "siparisturu_tanim_kucuk": "tamir"
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/siparis-turu-liste")]
        public HttpResponseMessage SiparisTuruListe()
        {
            parametreRepository = new ParametreRepository();
            var siparisler = parametreRepository.SiparisTuruGetir();

            if (siparisler != null)
            {
                var ozet = siparisler.Select(s => new
                {
                    siparisturu_id = s.siparisturu_id,
                    siparisturu_tanim = s.siparisturu_tanim,
                    siparisturu_tanim_kucuk = s.siparisturu_tanim.ToLower()
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Depoa Kart Rapor listesini veren Metodu. 
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///  parametre_id: 14,
        ///  kod: null,
        ///  tanim: "Giriş Depo"
        ///  },
        ///  {
        ///  parametre_id: 15,
        ///  kod: null,
        ///  tanim: "Çıkış Depo"
        ///  },
        ///  {
        ///  parametre_id: 16,
        ///  kod: null,
        ///  tanim: "Özel Depo"
        ///  },
        ///  {
        ///  parametre_id: 17,
        ///  kod: null,
        ///  tanim: "Kapalı Depo"
        ///  },
        ///  {
        ///  parametre_id: 18,
        ///  kod: null,
        ///  tanim: "Açık Depo"
        ///  },
        ///  {
        ///  parametre_id: 19,
        ///  kod: null,
        ///  tanim: "Depo"
        ///  }
        ///  ]
        /// </returns>
        [HttpGet]
        [Route("api/parametre/depo-kart-liste/{parametre_grubu},{parametre}")]
        public HttpResponseMessage depoListesiGetir(short parametre_grubu,short parametre)
        {
            parametreRepository = new ParametreRepository();
            var prListe = parametreRepository.CariRaporGetir(parametre_grubu,parametre);
            if (prListe != null && prListe.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, prListe.Select(u => new
                {
                    u.parametre_id,
                    u.parametre,
                    u.tanim
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.Successful { message = "No Record!" });
            }
        }

        #region zorlukgrubu metodları

        #region zorlukgrubu get metodu insert-update-delete

        /// <summary>
        /// Zorluk gruplarının listesini veren metod
        /// </summary>
        /// <returns>
        ///{
        ///  "zorlukgrubu_id": 1,
        ///  "tanim": "Genel",
        ///  "varsayilan": true,
        ///  "sira": 0,
        ///  "kayit_silindi": false
        ///},
        ///{
        ///  "zorlukgrubu_id": 3,
        ///  "tanim": "ni-Re",
        ///  "varsayilan": false,
        ///  "sira": 1,
        ///  "kayit_silindi": false
        ///},
        ///{
        ///  "zorlukgrubu_id": 6,
        ///  "tanim": "int",
        ///  "varsayilan": false,
        ///  "sira": 2,
        ///  "kayit_silindi": false
        ///},
        ///{
        ///  "zorlukgrubu_id": 7,
        ///  "tanim": "px",
        ///  "varsayilan": false,
        ///  "sira": 3,
        ///  "kayit_silindi": false
        ///},
        ///{
        ///  "zorlukgrubu_id": 8,
        ///  "tanim": "ni-Pr",
        ///  "varsayilan": false,
        ///  "sira": 4,
        ///  "kayit_silindi": false
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/parametre/zorlukgrubu")]
        public HttpResponseMessage ZorlukgrubuArma()
        {
            parametreRepository = new ParametreRepository();
            var zorluk = parametreRepository.ZorlukGrubuGetir();
            if (zorluk != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, zorluk.Select(tur => new
                {
                    tur.zorlukgrubu_id,
                    tur.tanim,
                    tur.varsayilan,
                    tur.sira,
                    tur.kayit_silindi
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// zorluk grubunun insert metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/parametre/zorlukgrubu")]
        public HttpResponseMessage ZorlukgrubuPost(Zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                int sira = 0;
                List<parametre_zorlukgrubu> zorluksirasi = new List<parametre_zorlukgrubu>(); 
                parametreRepository = new ParametreRepository();
                zorluksirasi = parametreRepository.ZorlukGrubuGetir();
                if (zorluksirasi != null)
                {
                    sira = zorluksirasi.Select(x => x.sira).Last().acekaToInt();
                }


                String[] not_include = { "zorlukgrubu_id", "degistiren_tarih" };
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                zor.sira = sira + 1;
                acekaResult = CrudRepository<Zorlukgrubu>.Insert(zor, "parametre_zorlukgrubu", not_include);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful - Zorlukgrubu_ID = " + acekaResult.RetVal.ToString() });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// zorluk grubunun update metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/parametre/zorlukgrubu")]
        public HttpResponseMessage ZorlukgrubuPut(Zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = Tools.PersonelId;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("tanim", zor.tanim);
                fields.Add("varsayilan", zor.varsayilan);
                fields.Add("degistiren_carikart_id", zor.degistiren_carikart_id);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                fields.Add("sira",zor.sira);
                string[] Wherefields = { "zorlukgrubu_id" };

                acekaResult = CrudRepository.Update("parametre_zorlukgrubu", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// zorluk grubunun delete metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/parametre/zorlukgrubu")]
        public HttpResponseMessage ZorlukgrubuDelete(Zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("kayit_silindi", 1);
                fields.Add("degistiren_carikart_id", zor.degistiren_carikart_id);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                string[] Wherefields = { "zorlukgrubu_id" };
                acekaResult = CrudRepository.Update("parametre_zorlukgrubu", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region planlama zorlukgrubu metodu get , insert-update-delete işlemleri

        /// <summary>
        /// Zorluk gruplarının listesini veren metod
        /// </summary>
        /// <returns>
        ///{
        ///  "zorlukgrubu_id": 1,
        ///  "tanim": "Genel"
        ///},
        ///{
        ///  "zorlukgrubu_id": 3,
        ///  "tanim": "ni-Re"
        ///}
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/planlama-zorlukgrubulistesi")]
        public HttpResponseMessage ZorlukgrubuListesi()
        {
            parametreRepository = new ParametreRepository();
            var zorluk = parametreRepository.ZorlukGrubuGetir();
            if (zorluk != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, zorluk.Select(tur => new
                {
                    tur.zorlukgrubu_id,
                    tur.tanim
                }));

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// planlama zorluk grubunu veren metod
        /// </summary>
        /// <returns>
        ///{
        ///  "zorlukgrubu_id": 8,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "2017-03-10T11:48:30",
        ///  "kesimfire": 0,
        ///  "kesimfazla": 0,
        ///  "musterifazla": 0,
        ///  "bedenbazinda": 1
        ///}
        /// </returns>
        /// 
        [HttpGet]
        [Route("api/planlama-zorlukgrubu/{zorlukgrubuid}")]
        public HttpResponseMessage PlanlamaZorlukgrubuGetir(int zorlukgrubuid)
        {
            parametreRepository = new ParametreRepository();
            var zorluk = parametreRepository.PlanlamaZorlukGrubuGetir(zorlukgrubuid);
            if (zorluk != null)
            {
                var zor = new
                {
                    zorlukgrubu_id = zorluk.zorlukgrubu_id,
                    degistiren_carikart_id = zorluk.degistiren_carikart_id,
                    degistiren_tarih = zorluk.degistiren_tarih,
                    kesimfire = zorluk.kesimfire,
                    kesimfazla = zorluk.kesimfazla,
                    musterifazla = zorluk.musterifazla,
                    bedenbazinda = zorluk.bedenbazinda,
                };
                return Request.CreateResponse(HttpStatusCode.OK, zor);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// planlama_zorlukgrubu insert metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/planlama-zorlukgrubu")]
        public HttpResponseMessage PlanlamaZorlukgrubuPost(planlama_zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                String[] not_include = { "degistiren_tarih" };
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                acekaResult = CrudRepository<planlama_zorlukgrubu>.Insert(zor, "planlama_zorlukgrubu", not_include);
                return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// planlama zorluk grubunun update metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/planlama-zorlukgrubu")]
        public HttpResponseMessage PlanlamaZorlukgrubuPut(planlama_zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("degistiren_carikart_id", zor.degistiren_carikart_id);
                fields.Add("kesimfire", zor.kesimfire);
                fields.Add("kesimfazla", zor.kesimfazla);
                fields.Add("musterifazla", zor.musterifazla);
                fields.Add("bedenbazinda", zor.bedenbazinda);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                string[] Wherefields = { "zorlukgrubu_id" };
                acekaResult = CrudRepository.Update("planlama_zorlukgrubu", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// planlama zorluk grubunun delete metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/planlama-zorlukgrubu")]
        public HttpResponseMessage PlanlamaZorlukgrubuDelete(planlama_zorlukgrubu zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("kayit_silindi", 1);
                fields.Add("degistiren_carikart_id", zor.degistiren_carikart_id);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                string[] Wherefields = { "zorlukgrubu_id" };
                acekaResult = CrudRepository.Delete("planlama_zorlukgrubu", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region planlama zorlukgrubu oranları metodu get , insert-update-delete işlemleri

        /// <summary>
        /// planlama zorluk grubu oranlarını veren metod
        /// </summary>
        /// <returns>
        ///{
        /// "zorlukgrubu_id": 1,
        /// "sira": 13,
        /// "altseviye": 180,
        /// "ustseviye": 299,
        /// "kesimfire": 9,
        /// "kesimfazla": 3
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/planlama-zorlukgrubu-oranlari/{zorlukgrubuid}")]
        public HttpResponseMessage PlanlamaZorlukgrubuOranlariGetir(int zorlukgrubuid)
        {
            parametreRepository = new ParametreRepository();
            var zorluk = parametreRepository.PlanlamaZorlukGrubuOranlariGetir(zorlukgrubuid);
            if (zorluk != null)
            {
                var zor = zorluk.Select(z => new
                {
                    z.zorlukgrubu_id,
                    z.sira,
                    z.altseviye,
                    z.ustseviye,
                    z.kesimfire,
                    z.kesimfazla,
                    degistiren_tarih=DateTime.Now,
                    degistiren_carikart_id= Tools.PersonelId
                });
                return Request.CreateResponse(HttpStatusCode.OK, zor);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// planlama_zorlukgrubu insert metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/planlama-zorlukgrubu-oranlari")]
        public HttpResponseMessage PlanlamaZorlukgrubuOranlariPost(planlama_zorlukgrubu_oranlari_Model zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                int sira = 0;
                List<planlama_zorlukgrubu_oranlari> zorluksirasi = new List<planlama_zorlukgrubu_oranlari>(); //
                parametreRepository = new ParametreRepository();
                zorluksirasi = parametreRepository.PlanlamaZorlukGrubuOranlariGetir(zor.zorlukgrubu_id);
                if (zorluksirasi != null)
                {
                    sira = zorluksirasi.Select(x => x.sira).Last().acekaToInt();
                }

                String[] not_include = { "degistiren_tarih" };
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                zor.sira = (sira + 1).acekaToShort();
                acekaResult = CrudRepository<planlama_zorlukgrubu_oranlari_Model>.Insert(zor, "planlama_zorlukgrubu_oranlari", not_include);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful - Zorlukgrubu_ID = " + acekaResult.RetVal.ToString() });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// planlama zorluk grubunun update metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/planlama-zorlukgrubu-oranlari")]
        public HttpResponseMessage PlanlamaZorlukgrubuOranlariPut(planlama_zorlukgrubu_oranlari_Model zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("degistiren_carikart_id", zor.degistiren_carikart_id);
                fields.Add("kesimfire", zor.kesimfire);
                fields.Add("kesimfazla", zor.kesimfazla);
                fields.Add("altseviye", zor.altseviye);
                fields.Add("ustseviye", zor.ustseviye);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                fields.Add("sira", zor.sira);
                string[] Wherefields = { "zorlukgrubu_id","sira" };
                acekaResult = CrudRepository.Update("planlama_zorlukgrubu_oranlari", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// planlama zorluk grubunun delete metodu
        /// </summary>
        /// /// <param name="zor"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/planlama-zorlukgrubu-oranlari")]
        public HttpResponseMessage PlanlamaZorlukgrubuOranlariDelete(planlama_zorlukgrubu_oranlari_Model zor)
        {
            AcekaResult acekaResult = null;
            if (zor != null)
            {
                zor.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("sira", zor.sira);
                fields.Add("zorlukgrubu_id", zor.zorlukgrubu_id);
                string[] Wherefields = { "zorlukgrubu_id","sira" };
                acekaResult = CrudRepository.Delete("planlama_zorlukgrubu_oranlari", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region kalite kontrol oranları metodu get , insert-update-delete işlemleri

        /// <summary>
        /// kalite kontrol oranlarının kodlarını liste halinde veren metod
        /// </summary>
        /// <returns>
        ///{
        ///  "kalite_kontrol_kod": "A1"
        ///},
        ///{
        ///  "kalite_kontrol_kod": "A1_MD"
        ///},
        ///{
        ///  "kalite_kontrol_kod": "A1_MD_KIDS"
        ///},
        ///{
        ///  "kalite_kontrol_kod": "B1"
        ///},
        ///{
        ///  "kalite_kontrol_kod": "M1"
        ///},
        ///{
        ///  "kalite_kontrol_kod": "M1_MD"
        ///}
        /// </returns>
        /// 
        [HttpGet]
        [Route("api/kalitekontrol-oranlari")]
        public HttpResponseMessage KaliteKontrolOranlariListeGetir()
        {
            parametreRepository = new ParametreRepository();
            var kalite = parametreRepository.KaliteKontrolOranlariListeGetir();
            if (kalite != null)
            {
                var kal = kalite.Select(k => new
                {
                    k.kalite_kontrol_kod,
                });
                return Request.CreateResponse(HttpStatusCode.OK, kal);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// kalite kontrol oranlarını veren metod
        /// </summary>
        /// <returns>
        /// {
        ///   "kalite_kontrol_kod": "B1",
        ///   "stokkart_tipi_id": 21,
        ///   "adet": 100,
        ///   "kontrol_miktar": 10,
        ///   "red_miktar": 13,
        ///   "miktarlar_oran_mi": true
        /// }
        /// </returns>
        [HttpGet]
        [Route("api/kalitekontrol-oranlari/{kalite_kontrol_kod}")]
        public HttpResponseMessage KaliteKontrolOranlariGetir(string kalite_kontrol_kod)
        {
            parametreRepository = new ParametreRepository();
            var kalite = parametreRepository.KaliteKontrolOranlariGetir(kalite_kontrol_kod);
            if (kalite != null)
            {
                var kal = kalite.Select(k => new
                {
                    k.kalite_kontrol_kod,
                    k.stokkart_tipi_id,
                    k.adet,
                    k.sira_id,
                    k.degistiren_carikart_id,
                    k.degistiren_tarih,
                    k.kontrol_miktar,
                    k.red_miktar,
                    k.miktarlar_oran_mi,
                    stokkarttipleri = k.stokkart_tipleri.Select(s =>
                   new
                   {
                       s.stokkarttipi,
                       s.tanim
                   })
                });
                return Request.CreateResponse(HttpStatusCode.OK, kal);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// kalite kontrol oranlarının insert metodu
        /// </summary>
        /// /// <param name="kal"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/kalitekontrol-oranlari")]
        public HttpResponseMessage KaliteKontrolOranlariPost(kalite_kontrol_oranlari_model kal)
        {
            AcekaResult acekaResult = null;
            if (kal != null)
            {
                String[] not_include = { "degistiren_tarih","sira_id" };
                kal.degistiren_carikart_id = Tools.PersonelId.acekaToLong();


                acekaResult = CrudRepository<kalite_kontrol_oranlari_model>.Insert(kal, "kalite_kontrol_oranlari", not_include);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful - Zorlukgrubu_ID = " + acekaResult.RetVal.ToString() });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// kalite kontrol oranlarının update metodu
        /// </summary>
        /// /// <param name="kal"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/kalitekontrol-oranlari")]
        public HttpResponseMessage KaliteKontrolOranlariPut(kalite_kontrol_oranlari_model kal)
        {
            AcekaResult acekaResult = null;
            if (kal != null)
            {
                kal.degistiren_carikart_id = -1;//buraya sistemi açan kullanıcı bilgisi gelecek
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("degistiren_carikart_id", kal.degistiren_carikart_id);
                fields.Add("adet", kal.adet);
                fields.Add("sira_id", kal.sira_id);
                fields.Add("kalite_kontrol_kod", kal.kalite_kontrol_kod);
                fields.Add("stokkart_tipi_id", kal.stokkart_tipi_id);
                fields.Add("kontrol_miktar", kal.kontrol_miktar);
                fields.Add("red_miktar", kal.red_miktar);
                fields.Add("miktarlar_oran_mi", kal.miktarlar_oran_mi);
                string[] Wherefields = { "kalite_kontrol_kod", "sira_id" };
                acekaResult = CrudRepository.Update("kalite_kontrol_oranlari", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// kalite kontrol oranlarının delete metodu
        /// </summary>
        /// /// <param name="kal"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/kalitekontrol-oranlari")]
        public HttpResponseMessage KaliteKontrolOranlariDelete(kalite_kontrol_oranlari_model kal)
        {
            AcekaResult acekaResult = null;
            if (kal != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("kalite_kontrol_kod", kal.kalite_kontrol_kod);
                fields.Add("sira_id", kal.sira_id);
                string[] Wherefields = { "kalite_kontrol_kod", "sira_id" };
                acekaResult = CrudRepository.Delete("kalite_kontrol_oranlari", Wherefields, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return null;
            }
        }

        #endregion

        #endregion
    }
}
