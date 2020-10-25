using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models.CarikartModels;
using aceka.web_api.Models.StokkartModel;
using aceka.web_api.Models.PersonelModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using aceka.web_api.Models;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Stok kart işlemleri
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StokkartController : ApiController
    {
        #region Değişkenler
        private StokkartRepository stokKartRepository = null;
        private stokkart_rapor_parametre stok_rapor = null;
        private stokkart_ekler stok_ekler = null;
        private stokkart_fiyat stok_fiyat = null;
        private stokkart_kontrol stokkartkontrol = null;
        private ParametreRepository parametreRepository = null;
        private string errorMessage = "";
        #endregion

        /// <summary>
        /// Stok Kart Arama Metodu
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="stok_adi"></param>
        /// <param name="stokkart_tur_id"></param>
        /// <param name="stokkart_tipi_id"></param>
        /// <param name="stok_kodu"></param>
        /// <param name="stokkartturu"></param>
        /// <param name="orjinal_stok_kodu"></param>
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
        [CustAuthFilter(ApiUrl = "api/stokkart/arama")]
        [Route("api/stokkart/arama")]
        public HttpResponseMessage StokkartAra(string stok_adi = "", short stokkart_tur_id = 0, int stokkart_tipi_id = 0, string stok_kodu = "", byte stokkartturu = 0, string orjinal_stok_kodu = "")
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul(stok_adi, stokkart_tur_id, stokkart_tipi_id, stok_kodu, 2, orjinal_stok_kodu // stokkart turu kumaş,aksesuar ve iplik olanlar.
            );
            if (stokkartlar != null && stokkartlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, stokkartlar.Select(sk => new
                {
                    sk.stokkart_id,
                    sk.stok_adi,
                    sk.stok_kodu,
                    stokkart_tipi = sk.stokkart_tipi_id > 0 ? sk.stokkarttipi.tanim : "",
                    stokkart_turu = sk.stokkart_tur_id > 0 ? sk.stokkartturu.tanim : "",
                    sk.statu
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
                //return Request.CreateResponse(HttpStatusCode.NotFound, new List<stokkart>());
            }
        }

        /// <summary>
        /// Stok Koda göre arama yapan metod. (Autocomplate için kullanıldı)
        /// </summary>
        /// <param name="stok_kodu"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-kod-arama")]
        [Route("api/stokkart/stok-kod-arama/{stok_kodu}")]
        public HttpResponseMessage StokKodArama(string stok_kodu)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul(stok_kodu, 2);
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

        /// <summary>
        /// Stokkart Türüne göre stokkartları listeleyen metod  (Autocomplate için kullanıldı)
        /// </summary>
        /// <param name="stok_adi"></param>
        /// <param name="stokkart_tur_id">
        /// 0->Mamul , 1-> Yarı Mamül , 2->İlk Madde (Kumaş,Aksesuar ve iplik), 9->Diğer
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-adi-arama")]
        [Route("api/stokkart/stok-adi-arama-tur/{stok_adi},{stokkart_tur_id}")]
        public HttpResponseMessage StokAdiTurArama(string stok_adi, byte stokkart_tur_id)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul_StokAdi_ve_StokkartTur(stok_adi, stokkart_tur_id);
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
            }

        }

        /// <summary>
        /// Mamül, Yarı Mamul, ilk Madde ve Diğer kartlarını listeleyen metod  (Autocomplate için kullanıldı). İlk madde= Stok kartı, Mamül= Model Kartı demek.
        /// </summary>
        /// <param name="stokkart_tur_id">
        /// 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı
        /// </param>
        /// <returns>
        /// [
        /// {
        ///  "stokkart_id": 1,
        ///  "stok_adi": "selam",
        ///  "stokadi_kucuk": "selam",
        ///  "stok_kodu": "Albayrak",
        ///  "stokkodu_kucuk":"albayrak",
        ///  "stokkart_tipi_id": 0
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-tipadi-arama")]
        [Route("api/stokkart/stok-tipadi-arama/{stokkart_tur_id}")]
        public HttpResponseMessage ModelTuruneGoreArama(byte stokkart_tur_id)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.StokTuruListesiniGetir(stokkart_tur_id);
            if (stokkartlar != null && stokkartlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, stokkartlar.Select(sk => new
                {
                    sk.stokkart_id,
                    sk.stok_adi,
                    stokadi_kucuk = sk.stok_adi.ToLower(),
                    sk.stok_kodu,
                    stokkodu_kucuk = sk.stok_kodu.ToLower(),
                    sk.stokkart_tipi_id
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }

        }

        /// <summary>
        /// Mamül, Yarı Mamul, ilk Madde ve Diğer kartlarını listeleyen metod  (Autocomplate için kullanıldı). İlk madde= Stok kartı, Mamül= Model Kartı demek.
        /// </summary>
        /// <param name="stok_adi"></param>
        /// <param name="stokkart_tur_id">
        /// 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-tipadi-arama")]
        [Route("api/stokkart/stok-tipadi-arama/{stok_adi},{stokkart_tur_id}")]
        public HttpResponseMessage ModelTuruneGoreArama(string stok_adi, byte stokkart_tur_id)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.StokTuruListesiniGetir(stok_adi, stokkart_tur_id);
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
            }

        }

        /// <summary>
        /// Mamül, Yarı Mamul, ilk Madde ve Diğer kartlarını listeleyen metod  (Autocomplate için kullanıldı). İlk madde= Stok kartı, Mamül= Model Kartı demek.
        /// </summary>
        /// <param name="stok_kodu"></param>
        /// <param name="stokkart_tur_id">
        /// 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-tipadi-arama")]
        [Route("api/stokkart/stok-tipadi-arama-stokkodu-ile/{stok_kodu},{stokkart_tur_id}")]
        public HttpResponseMessage ModelTuruneGoreAramaStokAdi(string stok_kodu, byte stokkart_tur_id)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.StokTuruListesiniGetirStokKoduIle(stok_kodu, stokkart_tur_id);
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
            }

        }

        /// <summary>
        /// Stokkart Tipine göre stokkartları listeleyen metod  (Autocomplate için kullanıldı)
        /// </summary>
        /// <param name="stok_adi"></param>
        /// <param name="stokkart_tipi_id">
        /// 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-adi-arama")]
        [Route("api/stokkart/stok-adi-arama/{stok_adi},{stokkart_tipi_id}")]
        public HttpResponseMessage StokAdiArama(string stok_adi, byte stokkart_tipi_id)
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul_StokAdi_ve_StokkartTipi(stok_adi, stokkart_tipi_id);
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
            }
        }

        /// <summary>
        /// Model, Kumaş, Aksesuar ve İplik kartlarını listeleyen metod  (Autocomplate için kullanıldı)
        /// </summary>
        /// <param name="stokkart_tipi_id">
        /// 1->Mamul Model Kartı, 2->Hizmet Kartı, 3->Sarf Malzemesi, 4->Kıymetli Ürün, 13->Sabit Kıymet, 20->Kumaş Kartı, 21->Aksesuar Kartı,22->İplik Kartı
        /// </param>
        /// <returns>
        /// [
        /// {
        ///  "stokkart_id": 1,
        ///  "stok_adi": "selam",
        ///  "stokadi_kucuk": "selam",
        ///  "stok_kodu": "Albayrak",
        ///  "stokkodu_kucuk":"albayrak",
        ///  "stokkart_tipi_id": 0
        /// },
        /// {
        ///  "stokkart_id": 2,
        ///  "stok_adi": "Stok Adı Yok",
        ///  "stokadi_kucuk": "stok adı yok",
        ///  "stok_kodu": "0130 X",
        ///  "stokkodu_kucuk": "0130 x",
        ///  "stokkart_tipi_id": 0
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/stok-adi-arama")]
        [Route("api/stokkart/stok-adi-arama/{stokkart_tipi_id}")]
        public HttpResponseMessage ModelArama(byte stokkart_tipi_id, string stok_adi = "")
        {
            stokKartRepository = new StokkartRepository();
            var stokkartlar = stokKartRepository.Bul_StokAdi_ve_StokkartTipi(stok_adi, stokkart_tipi_id);

            if (stokkartlar == null)
                stokkartlar = new List<stokkart>();

            return Request.CreateResponse(HttpStatusCode.OK, stokkartlar.Select(sk => new
            {
                sk.birim_id_1,
                sk.stokkart_id,
                sk.stok_adi,
                stokadi_kucuk = sk.stok_adi.ToLower(),
                sk.stok_kodu,
                stokkodu_kucuk = sk.stok_kodu.ToLower(),
                sk.stokkart_tipi_id
            }));
        }

        #region stokkart genel 

        /// <summary>
        /// Stok Kart detayını getiren metod
        /// NOT: Talimat tanımlama yapılacağı zaman, "talimat.kod" seçimine bağlı olarak "talimatturu_id" bilgisinin, Insert ve Update için gönderilmesi gerekyor!
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// {
        ///  "stokkart_id": 29660,
        ///  "birim_id_1": 3,
        ///  "birim_id_2": 2,
        ///  "birim_id_2_zorunlu": true,
        ///  "birim_id_3": 4,
        ///  "birim_id_3_zorunlu": false,
        ///  "kdv_alis_id": 8,
        ///  "kdv_satis_id": 18,
        ///  "stok_kodu": "465465456564",
        ///  "stokkart_tipi_id": 20,
        ///  "statu": true,
        ///  "stok_adi": "kerem",
        ///  "degistiren_carikart_id": 0,
        ///  "stokkart_onay": {
        ///    "genel_onay": true
        ///  },
        ///  "talimat": {
        ///    "kod": "DKM",
        ///    "talimatturu_id": 3
        ///  },
        ///  "stok_talimat": {
        ///    "aciklama": "kullanma talimatı türü için açıklama"
        ///  },
        ///  "stokkart_ozel": {
        ///    "stok_adi_uzun": "",
        ///    "orjinal_stok_kodu": "1234rib",
        ///    "orjinal_stok_adi": "",
        ///    "tek_varyant": false
        ///  }
        /// }
        /// </returns>        
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart")]
        [Route("api/stokkart/{stokkart_id}")]
        public HttpResponseMessage Getir(long stokkart_id)
        {
            parametreRepository = new ParametreRepository();
            var stokKartTipi = parametreRepository.StokkartTip(stokkart_id);
            stokKartRepository = new StokkartRepository();

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
                    stokKart.uretimyeri_id,

                    stokkart_onay = new
                    {
                        genel_onay = (stokKart.stokkartonay != null && stokKart.stokkartonay.genel_onay ? true : false)
                    },

                    talimat = (stokKart.talimat != null ?
                    new
                    {
                        stokKart.talimat.kod,
                        stokKart.talimat.talimatturu_id
                    } : null),
                    stok_talimat = (stokKart.stokkart_talimat != null ?
                    new
                    {
                        stokKart.stokkart_talimat.aciklama
                    } : null),
                    stokkart_ozel = (stokKart.stokkart_ozel != null ?
                    new
                    {
                        stokKart.stokkart_ozel.stok_adi_uzun,
                        stokKart.stokkart_ozel.orjinal_stok_kodu,
                        stokKart.stokkart_ozel.orjinal_stok_adi,
                        //stokKart.stokkart_ozel.orjinal_renk_kodu,
                        //stokKart.stokkart_ozel.orjinal_renk_adi,
                        stokKart.stokkart_ozel.tek_varyant
                    } : null)
                });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// POST - > stokkart insert method.
        /// NOT: stokkart_id boş gönderilmeli
        /// </summary>
        /// <param name="stokkart"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart")]
        [Route("api/stokkart")]
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

                acekaResult = CrudRepository<Stokkart>.Insert(stokkart, "stokkart", new string[] { "stokkart_id" });
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    long stokkartId = acekaResult.RetVal.acekaToLong();

                    #region stok_talimat -> Gelen talimatturu_id ye göre "stok_talimat" kaydediliyor

                    //if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0
                    //    && stokkart.stok_talimat != null && !string.IsNullOrEmpty(stokkart.stok_talimat.aciklama.Trim()))
                    if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0)
                    {
                        stokkart_talimat stokkartTalimat = new stokkart_talimat
                        {
                            stokkart_id = stokkartId,
                            sira_id = 1,
                            aciklama = stokkart.stok_talimat.aciklama.Trim(),
                            talimatturu_id = stokkart.talimat.talimatturu_id,
                            degistiren_carikart_id = Tools.PersonelId,

                            degistiren_tarih = DateTime.Now
                        };
                        var talimatRetVal = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi", "fasoncu_carikart_id" });
                    }
                    #endregion

                    #region stokkart_ozel
                    if (stokkart.stokkart_ozel != null)
                    {
                        stokkart.stokkart_ozel.stokkart_id = stokkartId;
                        stokkart.stokkart_ozel.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                        stokkart.stokkart_ozel.degistiren_tarih = DateTime.Now;
                        stokkart.stokkart_ozel.stok_adi_uzun = stokkart.stokkart_ozel.orjinal_stok_kodu;
                        stokkart.stokkart_ozel.orjinal_stok_kodu = stokkart.stokkart_ozel.stok_adi_uzun;
                        stokkart.stokkart_ozel.orjinal_stok_adi = stokkart.stokkart_ozel.orjinal_stok_adi;
                        //stokkart.stokkart_ozel.orjinal_renk_kodu = stokkart.stokkart_ozel.orjinal_renk_kodu;
                        //stokkart.stokkart_ozel.orjinal_renk_adi = stokkart.stokkart_ozel.orjinal_renk_adi;
                        var stokkartOzelRetVal = CrudRepository<Stokkart_Ozel>.Insert(stokkart.stokkart_ozel, "stokkart_ozel");
                    }
                    #endregion

                    #region stokkart_onay
                    //stokkart_onay
                    if (stokkart.stokkart_onay != null)
                    {
                        stokkart.stokkart_onay.stokkart_id = stokkartId;
                        stokkart.stokkart_onay.genel_onay = stokkart.stokkart_onay.genel_onay;
                        var stokkartOnayRetVal = CrudRepository<Stokkart_onay>.Insert(stokkart.stokkart_onay, "stokkart_onay");
                    }
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
        ///  PUT - > stokkart update method.
        /// </summary>
        /// <param name="stokkart"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart")]
        [Route("api/stokkart")]
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

                parametreRepository = new ParametreRepository();
                var stokKartTip = parametreRepository.StokkartTip(stokkart.stokkart_id);

                var detay = stokKartRepository.Getir(stokkart.stokkart_id, stokKartTip.stokkartturu);
                if (detay != null)
                {
                    detay.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                    detay.degistiren_tarih = DateTime.Now;
                    detay.birim_id_1 = stokkart.birim_id_1;
                    detay.birim_id_2 = stokkart.birim_id_2;
                    detay.birim_id_2_zorunlu = stokkart.birim_id_2_zorunlu;
                    detay.birim_id_3 = stokkart.birim_id_3;
                    detay.birim_id_3_zorunlu = stokkart.birim_id_3_zorunlu;
                    detay.stok_kodu = stokkart.stok_kodu;
                    detay.stok_adi = stokkart.stok_adi;
                    detay.uretimyeri_id = stokkart.uretimyeri_id;
                    detay.kdv_alis_id = stokkart.kdv_alis_id;
                    detay.kdv_satis_id = stokkart.kdv_satis_id;
                    detay.stokkart_tur_id = stokKartTip.stokkartturu;
                    detay.statu = stokkart.statu;

                    if (detay.stokkart_tipi_id != stokkart.stokkart_tipi_id)
                    {
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
                        var stokkartTalimat = stokKartRepository.StokkartTalimatDetay(stokkart.stokkart_id, 1);
                        if (stokkartTalimat != null)
                        {
                            //update
                            if (stokkart.stok_talimat != null)
                            {
                                stokkartTalimat.aciklama = stokkart.stok_talimat.aciklama;
                                stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                                stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                                stokkartTalimat.degistiren_tarih = DateTime.Now;

                                var stokTalimatRet = acekaResult = CrudRepository<stokkart_talimat>.Update(stokkartTalimat, new string[] { "stokkart_id", "sira_id" }, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                            }
                            else if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0
                        && stokkart.stok_talimat != null && !string.IsNullOrEmpty(stokkart.stok_talimat.aciklama.Trim()))
                            {
                                //insert
                                stokkartTalimat = new stokkart_talimat();
                                stokkartTalimat.stokkart_id = stokkart.stokkart_id;
                                stokkartTalimat.sira_id = 1;
                                stokkartTalimat.aciklama = stokkart.stok_talimat.aciklama.Trim();
                                stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                                stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                                stokkartTalimat.degistiren_tarih = DateTime.Now;
                                var stokTalimatRet = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                            }

                        }
                        else if (stokkart.talimat != null && stokkart.talimat.talimatturu_id > 0)
                        {
                            //insert. Ayhan
                            stokkartTalimat = new stokkart_talimat();
                            stokkartTalimat.stokkart_id = stokkart.stokkart_id;
                            stokkartTalimat.sira_id = 1;
                            stokkartTalimat.aciklama = stokkart.stok_talimat != null ? stokkart.stok_talimat.aciklama.Trim() : "";
                            stokkartTalimat.talimatturu_id = stokkart.talimat.talimatturu_id;
                            stokkartTalimat.degistiren_carikart_id = stokkart.degistiren_carikart_id;
                            stokkartTalimat.degistiren_tarih = DateTime.Now;
                            var stokTalimatRet = CrudRepository<stokkart_talimat>.Insert(stokkartTalimat, new string[] { "talimat_adi", "fasoncu_carikart_adi" });
                        }
                        #endregion

                        #region stokkart_ozel
                        var stokkartOzel = stokKartRepository.StokkartOzelDetay(stokkart.stokkart_id);
                        if (stokkartOzel != null)
                        {
                            if (stokkart.stokkart_ozel != null)
                            {
                                // Update
                                stokkartOzel.degistiren_carikart_id = stokkart.stokkart_ozel.degistiren_carikart_id;
                                stokkartOzel.degistiren_tarih = DateTime.Now;
                                stokkartOzel.stok_adi_uzun = stokkart.stokkart_ozel.stok_adi_uzun;
                                stokkartOzel.orjinal_stok_kodu = stokkart.stokkart_ozel.orjinal_stok_kodu;
                                stokkartOzel.orjinal_stok_adi = stokkart.stokkart_ozel.orjinal_stok_adi;
                                //stokkartOzel.orjinal_renk_kodu = stokkart.stokkart_ozel.orjinal_renk_kodu;
                                //stokkartOzel.orjinal_renk_adi = stokkart.stokkart_ozel.orjinal_renk_adi;
                                stokkartOzel.tek_varyant = stokkart.stokkart_ozel.tek_varyant;

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
                            var onaylog = stokKartRepository.StokkartOnayLog(stokkart.stokkart_id);
                            if (stokkart.stokkart_onay.genel_onay == false)
                            {
                                Dictionary<string, object> fields = new Dictionary<string, object>();
                                fields.Add("onay_alan_adi", "genel_onay"); //onaylog.onay_alan_adi
                                fields.Add("iptal_carikart_id", Tools.PersonelId);
                                fields.Add("iptal_tarihi", DateTime.Now);
                                fields.Add("stokkart_id", stokkart.stokkart_id);//onaylog.stokkart_id
                                //fields.Add("onay_tarihi", onaylog.onay_tarihi);
                                //fields.Add("onay_carikart_id", onaylog.onay_carikart_id);

                                acekaResult = CrudRepository.Update("stokkart_onay_log", "stokkart_id = " + stokkart.stokkart_id + " AND onay_alan_adi = 'genel_onay'", fields, true); //AND iptal_tarihi is null
                            }
                            else
                            {
                                //Insert
                                stokkart.stokkart_onay_log.stokkart_id = stokkart.stokkart_id;
                                stokkart.stokkart_onay_log.onay_alan_adi = "genel_onay";// stokkart.stokkart_onay.genel_onay.ToString();
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
        /// DELETE - > stokkart delete method.
        /// Not: Stokkart model içerisinde sadece "stokkart_id" ve "degistiren_carikart_id" değişkenlerinin dolu gelmesi yeterlidir! 
        /// </summary>
        /// <param name="stokkart"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/stokkart")]
        [Route("api/stokkart")]
        public HttpResponseMessage Delete(StokkartUpdate stokkart)
        {
            if (stokkart != null && stokkart.stokkart_id > 0)
            {
                AcekaResult acekaResult = null;
                stokKartRepository = new StokkartRepository();
                parametreRepository = new ParametreRepository();
                var stokKartTipi = parametreRepository.StokkartTip(stokkart.stokkart_id);
                //if (stokKartTipi != null)
                //stokkart.gizsabit_stokkarttipi.stokkartturu = stokKartTipi.stokkartturu; 
                var detay = stokKartRepository.Getir(stokkart.stokkart_id, stokKartTipi.stokkartturu);
                if (detay != null)
                {
                    detay.kayit_silindi = true;
                    detay.statu = stokkart.statu; // Ayhan. 23.02.2017
                    detay.degistiren_tarih = DateTime.Now;
                    detay.degistiren_carikart_id = stokkart.degistiren_carikart_id;

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
                    return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }

        }

        /// <summary>
        /// Stokkart Onay Loglarını ve Model Kart Onay Loglarını Getiren Metod
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
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/onay-loglari")]
        [Route("api/stokkart/onay-loglari/{stokkart_id}")]
        public HttpResponseMessage StokkartOnayLoglari(long stokkart_id)
        {
            string errorMessage = "";
            stokKartRepository = new StokkartRepository();
            var loglar = stokKartRepository.StokkartOnayLoglari(stokkart_id, CustomEnums.OnayLogTipi.genel_onay, ref errorMessage);
            if (loglar != null && loglar.Count > 0 && string.IsNullOrEmpty(errorMessage))
            {
                return Request.CreateResponse(HttpStatusCode.OK, loglar.Select(lg => new
                {
                    lg.stokkart_id,
                    lg.onay_tarihi,
                    onaylayan_cari = lg.onaylayan_carikart.cari_unvan,
                    lg.iptal_tarihi,
                    iptal_eden_cari = lg.iptal_carikart_id > 0 ? lg.iptal_eden_carikart.cari_unvan : ""
                }));
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NotFound, new { });
            }
        }
        #endregion

        #region Stok Kart Onay Ve Logları

        /// <summary>
        /// Stok Kart Onay Logları POST Metod
        /// </summary>
        /// <param name="stkOnay"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/onay")]
        [Route("api/stokkart/onay")]
        public HttpResponseMessage StokkartOnayLogPost(Stokkart_onay stkOnay)
        {
            stokKartRepository = new StokkartRepository();

            if (stkOnay != null)
            {
                stkOnay.genel_onay = true;

                var siparisOnayRetVal = CrudRepository<Stokkart_onay>.Insert(stkOnay, "stokkart_onay");
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Stok Kart Onay Logları PUT Metod
        /// </summary>
        /// <param name="stkOnay"></param>
        /// <returns>
        /// </returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/onay")]
        [Route("api/stokkart/onay")]
        public HttpResponseMessage StokkartOnayLogPut(Stokkart_onay stkOnay)
        {
            AcekaResult acekaResult = null;
            stokKartRepository = new StokkartRepository();

            if (stkOnay != null && stkOnay.stokkart_id > 0)
            {
                stokkart_onay stok_onay = new stokkart_onay();
                stokkart_onay_log stok_log = new stokkart_onay_log();
                if (stkOnay.genel_onay == false)
                {
                    #region Update
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("onay_alan_adi", "genel_onay"); //onaylog.onay_alan_adi
                    fields.Add("iptal_carikart_id", Tools.PersonelId);
                    fields.Add("iptal_tarihi", DateTime.Now);
                    fields.Add("stokkart_id", stkOnay.stokkart_id);//onaylog.stokkart_id

                    acekaResult = CrudRepository.Update("stokkart_onay_log", "stokkart_id = " + stkOnay.stokkart_id + " AND onay_alan_adi = 'genel_onay' AND  iptal_tarihi is null", fields, true);
                    #endregion
                }
                else
                {
                    #region Insert

                    var sonuc = stokKartRepository.StokkartOnay(stkOnay.stokkart_id);
                    if (sonuc == null)
                    {
                        stok_onay.genel_onay = false;
                        stok_onay.stokkart_id = stkOnay.stokkart_id;
                        CrudRepository<stokkart_onay>.Insert(stok_onay, "stokkart_onay");
                    }


                    stok_log.onay_carikart_id = Tools.PersonelId;
                    stok_log.onay_tarihi = DateTime.Now;
                    stok_log.onay_alan_adi = "genel_onay";
                    stok_log.stokkart_id = stkOnay.stokkart_id;

                    CrudRepository<stokkart_onay_log>.Insert(stok_log, "stokkart_onay_log", new string[] { "onaylayan_carikart", "iptal_eden_carikart" });
                    #endregion
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion


        #region tab içerikleri

        #region genel tabı
        /// <summary>
        /// Stokkart -> Genel Tab -> Rapor parametreleri GET
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        ///     {
        ///     satici_carikart_id: null,
        ///     stokkart_tipi_id:2,
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
        ///     stokalan_id_20: null
        ///     }
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/rapor-parametreler")]
        [Route("api/stokkart/rapor-parametreler/{stokkart_id}")]
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
                    parametreler.stokalan_id_20

                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stokkart -> Genel Tab -> Rapor parametreleri PUT  işlemi
        /// </summary>
        /// <param name="stokkartRaporParametreler"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/rapor-parametreler")]
        [Route("api/stokkart/rapor-parametreler")]
        public HttpResponseMessage StokkartParametrelerPut(StokkartRaporParametreler stokkartRaporParametreler)
        {
            AcekaResult acekaResult = null;

            if (stokkartRaporParametreler != null)//&& stokkartRaporParametreler.stokkart_id > 0
            {
                if (stokkartRaporParametreler.satici_carikart_id != null && stokkartRaporParametreler.satici_carikart_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("stokkart_id", stokkartRaporParametreler.stokkart_id);
                    fields.Add("satici_carikart_id", stokkartRaporParametreler.satici_carikart_id);
                    var retVal = CrudRepository.Update("stokkart", "stokkart_id", fields);
                }

                stokKartRepository = new StokkartRepository();
                stokkart_rapor_parametre parametre = stokKartRepository.Stokkart_Genel_Parametreler(stokkartRaporParametreler.stokkart_id);
                if (parametre != null)
                {

                    #region Update
                    // Update
                    parametre.degistiren_carikart_id = stokkartRaporParametreler.degistiren_carikart_id;
                    parametre.degistiren_tarih = DateTime.Now;
                    parametre.stokkart_id = stokkartRaporParametreler.stokkart_id;
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
        /// Stokkart -> Genel tabındaki dosya eklerinin bilgileri döndürür        
        /// </summary>
        /// /// <param name="stokkart_id">stokkart_ekler tablosunda stokkart_id alanı</param>
        /// <returns>
        ///[
        ///    {
        ///        stokkart_id,
        ///        ek_id: 3752,
        ///        ekadi: "IMG_0209.JPG",
        ///        aciklama: "deneme resim ekleme",
        ///        filepath: "MODELKART\x",
        ///        filename: "IMG_0209.JPG",
        ///        ekturu_id: 1
        ///    },
        ///    {
        ///        stokkart_id,
        ///        ek_id: 3753,
        ///        ekadi: "IMG_0210.JPG",
        ///        aciklama: "başka deneme resimleri",
        ///        filepath: "MODELKART\x",
        ///        filename: "IMG_0210.JPG",
        ///        ekturu_id: 2
        ///    }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/genel-ekler")]
        [Route("api/stokkart/genel-ekler/{stokkart_id}")]
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
                        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
                    }
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
        ///Stokkart -> Genel tabındaki dosya ekleri POST metodu
        /// </summary>
        /// <param name="ekler"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/genel-ekler")]
        [Route("api/stokkart/genel-ekler")]
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
                    ek.degistiren_carikart_id = item.degistiren_carikart_id;
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
                    //4/4/2017 de eklendi AA.return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    return Request.CreateResponse(HttpStatusCode.Created, new Models.AnonymousModels.Successful { message = "Create Data" });
                }
            }
            else
            {
                //4/4/2017 de eklendi AA.return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }

        }

        /// <summary>
        /// Stokkart -> Genel tabındaki dosya ekleri DELETE metodu
        /// Not: "stokkart_id", "ek_id" ve "filename" gönderilmelidir.
        /// </summary>
        /// <param name="ekler"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/stokkart/genel-ekler")]
        [Route("api/stokkart/genel-ekler")]
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
                            acekaResult = CrudRepository.Delete("ekler",new string[] { "ek_id" }, fields);

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
        /// Stokkart -> Genel tabındaki Varsayılan Satıcı Listesi Auto complate. AA
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
        [CustAuthFilter(ApiUrl = "api/stokkart/varsayilan_satici")]
        [Route("api/stokkart/varsayilan_satici")]
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

        #region Stokkart Varyant Sekmesi: stokkart_sku tablosu

        ///<summary>
        /// İptal. Stok Kart -> Siparişte Otomatik Varyant oluşturma End pointi. AA. 
        /// </summary>
        /// <returns>
        /// [
        ///  {
        ///    "sku_oto_field_id": 1,
        ///    "statu": true,
        ///    "table_name": "siparis",
        ///    "field_name": "siparis_turu_id",
        ///    "tanim": "Sipariş Türü"
        ///  },
        ///  {
        ///    "sku_oto_field_id": 2,
        ///    "statu": true,
        ///    "table_name": "stokkart",
        ///    "field_name": "stok_kodu",
        ///    "tanim": "Model No"
        ///  }
        ///]
        ///</returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/tekvaryant")]
        [Route("api/stokkart/tekvaryant")]
        public HttpResponseMessage VaryantGetir()
        {
            //giz_setup_varyant_oto tablosundan gelecek.
            stokKartRepository = new StokkartRepository();
            var otovaryantlar = stokKartRepository.OtoVaryantList();
            if (otovaryantlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, otovaryantlar.Select(tekvaryant => new
                {
                    tekvaryant.sku_oto_field_id,
                    tekvaryant.statu,
                    tekvaryant.table_name,
                    tekvaryant.field_name,
                    tekvaryant.tanim
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        ///<summary>
        /// Stok Kart -> Tek varyant ve Seçenekleri veren Metod. AA.
        /// </summary>
        /// <returns>
        /// {
        ///  "tek_varyant": false,
        ///  "stokkartSkuOto": [
        ///    {
        ///      "stokkart_id": 0,
        ///      "sku_oto_field_id": 0,
        ///      "degistiren_carikart_id": 0,
        ///      "sira_id": 0,
        ///      "secili": false,
        ///      "tanim": "Model No",
        ///      "stokkart_ozel": null
        ///    },
        ///    {
        ///      "stokkart_id": 0,
        ///      "sku_oto_field_id": 0,
        ///      "degistiren_carikart_id": 0,
        ///      "sira_id": 0,
        ///      "secili": false,
        ///      "tanim": "Ayhan",
        ///      "stokkart_ozel": null
        ///    },
        ///    {
        ///      "stokkart_id": 1,
        ///      "sku_oto_field_id": 1,
        ///      "degistiren_carikart_id": 0,
        ///      "sira_id": 1,
        ///      "secili": true,
        ///      "tanim": "Sipariş Türü",
        ///      "stokkart_ozel": null
        ///    }
        ///  ]
        /// }
        ///</returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/sku-oto")]
        [Route("api/stokkart/sku-oto/{stokkart_id}")]
        public HttpResponseMessage skuotoGet(long stokkart_id)
        {
            //stokakrt_ozel'e tekvaryant ve stokkart_sku_oto tablosuna ekleme yapılacak..
            stokKartRepository = new StokkartRepository();
            var skuoto = stokKartRepository.SkuOtoGetir(stokkart_id);
            if (skuoto != null)
            {
                List<StokkartSkuOto> parametreler = skuoto.Select(p => new StokkartSkuOto
                {
                    secili = p.secili,
                    tanim = p.giz_setup_varyant_oto.tanim,
                    stokkart_id = p.stokkart_id,
                    sku_oto_field_id = p.sku_oto_field_id,
                    sira_id = p.sira_id
                }).ToList();

                Stokkarttekvaryant retParametreler = new Stokkarttekvaryant
                {
                    tek_varyant = skuoto[0].stokkart_ozel.tek_varyant,
                    stokkartSkuOto = parametreler,
                };
                return Request.CreateResponse(HttpStatusCode.OK, retParametreler);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// Stokkart -> Tek Varyant -> POST.
        /// seçilenler sıra ile "stokkart_sku_oto" tablosuna seçilen sıra ile ekliyoruz.
        /// diyelimki önce "Model No" sonra "Sipariş Türü" seçildi.
        /// "stokkart_sku_oto" tablosuna sıra ile önce "Model No" yu sonra "Sipariş Türü" nü ekliyoruz.
        /// Model no için       sira_id = 1 
        /// Sipariş Türü için   sira_id = 2  olacak. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/sku-oto")]
        [Route("api/stokkart/sku-oto")]
        public HttpResponseMessage skuotoPost(Stokkarttekvaryant skuoto)
        {
            AcekaResult acekaResult = null;
            if (skuoto.tek_varyant == true)
            {
                stokkart_sku_oto skk = new stokkart_sku_oto();
                stokkart_ozel ozl = new stokkart_ozel();
                foreach (var item in skuoto.stokkartSkuOto)
                {
                    skk.stokkart_id = item.stokkart_id;
                    skk.sku_oto_field_id = item.sku_oto_field_id;
                    skk.degistiren_carikart_id = item.degistiren_carikart_id;
                    skk.degistiren_tarih = DateTime.Now;
                    skk.sira_id = item.sira_id;
                    if (item.secili == true)
                    {
                        CrudRepository<stokkart_sku_oto>.Insert(skk, new string[] { "secili" });
                    }
                }

                ozl.tek_varyant = skuoto.tek_varyant;
                ozl.stokkart_id = skuoto.stokkartSkuOto[0].stokkart_id;
                ozl.degistiren_carikart_id = 0;
                ozl.degistiren_tarih = DateTime.Now;
                // CrudRepository<stokkart_ozel>.Insert(ozl);
                CrudRepository<stokkart_ozel>.Update(ozl, "stokkart_id", new string[] { "stok_adi_uzun", "orjinal_stok_kodu", "orjinal_stok_adi", "orjinal_renk_kodu", "orjinal_renk_adi", "gtipno", "urun_grubu", "birim1x", "birim2x", "birim3x", "M2_gram", "eni", "fyne", "fire_orani", "birim_gram" });
            }
            //return null;
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }

        /// <summary>
        /// Stokkart -> Tek Varyant -> PUT.
        /// seçilenler sıra ile "stokkart_sku_oto" tablosuna seçilen sıra ile ekliyoruz.
        /// diyelimki önce "Model No" sonra "Sipariş Türü" seçildi.
        /// "stokkart_sku_oto" tablosuna sıra ile önce "Model No" yu sonra "Sipariş Türü" nü ekliyoruz.
        /// Model no için       sira_id = 1 
        /// Sipariş Türü için   sira_id = 2  olacak. 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter]
        [Route("api/stokkart/sku-oto")]
        public HttpResponseMessage skuotoPut(Stokkarttekvaryant skuoto)
        {
            AcekaResult acekaResult = null;
            if (skuoto.tek_varyant == true)
            {
                #region Silme - Önce Silip sonra ekleyeceğiz.
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("stokkart_id", skuoto.stokkartSkuOto[0].stokkart_id);
                CrudRepository.Delete("stokkart_sku_oto", new string[] { "stokkart_id" }, fields);
                #endregion

                foreach (var item in skuoto.stokkartSkuOto)
                {
                    stokkart_sku_oto skk = new stokkart_sku_oto();
                    skk.stokkart_id = item.stokkart_id;
                    //skk.stokkart_ozel.tek_varyant = skuoto.tek_varyant;
                    skk.sku_oto_field_id = item.sku_oto_field_id;
                    skk.degistiren_carikart_id = item.degistiren_carikart_id;
                    skk.degistiren_tarih = DateTime.Now;
                    skk.sira_id = item.sira_id;
                    if (item.secili == true)
                    {
                        var stokTalimatRet = CrudRepository<stokkart_sku_oto>.Insert(skk, new string[] { "secili", "tanim" });
                    }
                }

                stokKartRepository = new StokkartRepository();
                var stokkartOzel = stokKartRepository.StokkartOzelDetay(skuoto.stokkartSkuOto[0].stokkart_id);
                if (stokkartOzel != null)
                {
                    #region Update
                    stokkartOzel.degistiren_carikart_id = skuoto.stokkartSkuOto[0].degistiren_carikart_id;
                    stokkartOzel.degistiren_tarih = DateTime.Now;
                    stokkartOzel.tek_varyant = skuoto.tek_varyant;

                    acekaResult = CrudRepository<stokkart_ozel>.Update(stokkartOzel, "stokkart_id");
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                    }
                    #endregion
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        }

        /// <summary>
        /// Stok Kart -> Varyant -> GET -> varyant detayını getiren metod.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="sku_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/varyant")]
        [Route("api/stokkart/varyant/{stokkart_id}/{sku_id}")]
        public HttpResponseMessage VaryantDetay(long stokkart_id, long sku_id)
        {
            stokKartRepository = new StokkartRepository();
            var varyant = stokKartRepository.VaryantDetay(sku_id);
            if (varyant != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    varyant.sku_id,
                    varyant.stokkart_id,
                    varyant.sku_no,
                    varyant.statu,
                    renk = new
                    {
                        varyant.renk.renk_id,
                        varyant.renk.renk_adi,
                        varyant.renk.renk_kodu
                    }

                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Stok Kart -> Varyant -> GET -> Varyant listesini getiren metod.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// 
        /// [
        ///     {
        ///     sku_id: 203566,
        ///     stokkart_id: 32776,
        ///     sku_no: "2030001149833",
        ///     statu: true,
        ///     renk: {
        ///             renk_id: 0,
        ///             renk_adi: "-",
        ///             renk_kodu: ""
        ///         }
        ///     },
        ///     {
        ///     sku_id: 203567,
        ///     stokkart_id: 32776,
        ///     sku_no: "2030001395797",
        ///     statu: true,
        ///     renk: {
        ///             renk_id: 76574,
        ///             renk_adi: "L/725887 100/GEO/EMEA",
        ///             renk_kodu: ""
        ///         }
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/varyant")]
        [Route("api/stokkart/varyant/{stokkart_id}")]
        public HttpResponseMessage VaryantListesi(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var varyantlar = stokKartRepository.VaryantListesi(stokkart_id);
            if (varyantlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, varyantlar.Select(varyant => new
                {
                    varyant.sku_id,
                    varyant.stokkart_id,
                    varyant.sku_no,
                    varyant.statu,
                    renk = new
                    {
                        varyant.renk.renk_id,
                        varyant.renk.renk_adi,
                        varyant.renk.renk_kodu
                    }
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stokkart ->  Varyant -> POST.
        /// Not: Post işlemi sırasında Renk objesinde sadece "Renk ID" göndermek yeterlidir!
        /// slm. burada giz_setup_varyant_oto tablosunu biz dolduracağız.
        /// "giz_setup_varyant_oto" tablosu "Siparişte Otomatik Varyant"  kısmını seçmemize yarıyor.
        /// </summary>
        /// <param name="stokkartVaryant"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/varyant")]
        [Route("api/stokkart/varyant")]
        public HttpResponseMessage VaryantPost(StokkartVaryant stokkartVaryant)
        {
            AcekaResult acekaResult = null;
            if (stokkartVaryant != null && stokkartVaryant.stokkart_id > 0)
            {
                stokkart_sku varyant = new stokkart_sku();
                varyant.stokkart_id = stokkartVaryant.stokkart_id;
                varyant.sku_no = stokkartVaryant.sku_no;
                varyant.renk_id = stokkartVaryant.renk.renk_id;
                varyant.beden_id = 0;
                varyant.degistiren_carikart_id = Tools.PersonelId;
                varyant.degistiren_tarih = DateTime.Now;
                varyant.kayit_silindi = false;
                varyant.statu = stokkartVaryant.statu;

                acekaResult = CrudRepository<stokkart_sku>.Insert(varyant, new string[] { "sku_id", "asorti", "asorti_miktar" });
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
        /// Stokkart ->  Varyant -> PUT.
        /// </summary>
        /// <param name="stokkartVaryant"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/varyant")]
        [Route("api/stokkart/varyant")]
        public HttpResponseMessage VaryantPut(StokkartVaryant stokkartVaryant)
        {
            AcekaResult acekaResult = null;

            if (stokkartVaryant != null && stokkartVaryant.stokkart_id > 0 && stokkartVaryant.sku_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var varyant = stokKartRepository.VaryantDetay(stokkartVaryant.sku_id);

                varyant.sku_no = stokkartVaryant.sku_no;
                varyant.renk_id = stokkartVaryant.renk.renk_id;
                varyant.degistiren_carikart_id = stokkartVaryant.degistiren_carikart_id;
                varyant.degistiren_tarih = DateTime.Now;
                varyant.statu = stokkartVaryant.statu;

                acekaResult = CrudRepository<stokkart_sku>.Update(varyant, "sku_id", new string[] { "asorti", "asorti_miktar" });

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
        /// Stokkart ->  Varyant -> DELETE.
        /// NOT: obje içerisinde sadece "stokkart_id" ve "sku_id" bilgisinin dolu gelmesi yeterlidir!
        /// </summary>
        /// <param name="stokkartVaryant"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/stokkart/varyant")]
        [Route("api/stokkart/varyant")]
        public HttpResponseMessage VaryantDelete(StokkartVaryant stokkartVaryant)
        {
            AcekaResult acekaResult = null;

            if (stokkartVaryant != null && stokkartVaryant.stokkart_id > 0 && stokkartVaryant.sku_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var varyant = stokKartRepository.VaryantDetay(stokkartVaryant.sku_id);

                varyant.degistiren_carikart_id = stokkartVaryant.degistiren_carikart_id;
                varyant.degistiren_tarih = DateTime.Now;
                varyant.kayit_silindi = true;

                acekaResult = CrudRepository<stokkart_sku>.Update(varyant, "sku_id", new string[] { "asorti", "asorti_miktar" });

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
        #endregion

        #region Stokkart Diğer Sekmesi
        /// <summary>
        /// Stok Kart -> Diger / Sezon -> GET -> sezon detayını getiren metod.
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="sezon_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/sezon")]
        [Route("api/stokkart/sezon/{stokkart_id}/{sezon_id}")]
        public HttpResponseMessage StokkartSezonDetay(long stokkart_id, short sezon_id)
        {
            stokKartRepository = new StokkartRepository();
            var sezon = stokKartRepository.StokkartSezonDetay(stokkart_id, sezon_id);
            if (sezon != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    sezon.stokkart_id,
                    sezon.sezon_id,
                    sezon.sezon.sezon_adi,
                    sezon.sezon.sezon_kodu,
                    sezon.statu
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Stok Kart -> Diger / Sezon -> GET -> Sezon listesini getiren metod.
        ///  NOT: stokkart_id = 0 ya da statu = false iken checkbox lar seçili olmayacak!
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// 
        /// [
        ///     {
        ///     sezon_id: 203566,
        ///     stokkart_id: 32776,
        ///     sezon_adi :"2017",
        ///     sezon_kodu : "abcdefg"
        ///     statu: true,
        ///     },
        ///     {
        ///     sezon_id: 203566,
        ///     stokkart_id: 32776,
        ///     sezon_adi :"2017",
        ///     sezon_kodu : "abcdefg"
        ///     statu: true,
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/sezon")]
        [Route("api/stokkart/sezon/{stokkart_id}")]
        public HttpResponseMessage StokkartSezonListesi(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var sezonlar = stokKartRepository.StokkartSezonListesi(stokkart_id);
            if (sezonlar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, sezonlar.Select(szn => new
                {
                    szn.stokkart_id,
                    szn.sezon_id,
                    szn.sezon.sezon_adi,
                    szn.sezon.sezon_kodu,
                    statu = szn.stokkart_id > 0 ? true : false

                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Stok Kart -> Diger / Sezon -> POST -> Sadece sezonlar onaylanacak. Diğer bilgiler değiştirilemiyecek.
        /// </summary>
        /// <param name="stokkartSezon"></param>
        /// <returns>
        /// [
        ///   {
        ///     "stokkart_id": 52023,
        ///             "sezon_id": 1,
        ///             "sezon_kodu": "",
        ///             "sezon_adi": "",
        ///             "statu": true,
        ///             "degistiren_carikart_id": -1
        ///   },
        ///   {
        ///     "stokkart_id": 52023,
        ///     "sezon_id": 2,
        ///     "sezon_kodu": "",
        ///     "sezon_adi": "",
        ///     "statu": true,
        ///     "degistiren_carikart_id": -1
        ///   }
        /// ]
        /// </returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/sezon")]
        [Route("api/stokkart/sezon")]
        public HttpResponseMessage StokkartSezonUpdate(List<StokkartSezon> stokkartSezon)
        {
            AcekaResult acekaResult = null;

            if (stokkartSezon != null)
            {
                if (stokkartSezon[0].stokkart_id > 0)
                {
                    var deleteRetVal = CrudRepository<stokkart_sezon>.Delete(new stokkart_sezon { stokkart_id = stokkartSezon[0].stokkart_id }, "stokkart_sezon", new string[] { "stokkart_id" }, new string[] { "stokkart_id" });

                    foreach (var item in stokkartSezon)
                    {
                        if (item.sezon_id > 0 && item.stokkart_id > 0)
                        {
                            stokKartRepository = new StokkartRepository();
                            stokkart_sezon sezon = new stokkart_sezon();

                            sezon.sezon_id = item.sezon_id;
                            sezon.stokkart_id = item.stokkart_id;
                            sezon.degistiren_carikart_id = Tools.PersonelId;
                            sezon.degistiren_tarih = DateTime.Now;
                            sezon.statu = true;
                            acekaResult = CrudRepository<stokkart_sezon>.Insert(sezon);
                            if (acekaResult.ErrorInfo != null)
                            {
                                break;
                            }
                        }
                    }
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
        /// Stok Kart -> Diger / Muadil -> GET -> Muadil detayını getiren metod.
        /// </summary>
        /// <param name="stokkart_id">Zorunlu</param>
        /// <param name="muadil_stokkart_id">Zorunlu</param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/muadil")]
        [Route("api/stokkart/muadil/{stokkart_id}/{muadil_stokkart_id}")]
        public HttpResponseMessage StokkartMuadilDetay(long stokkart_id, long muadil_stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var muadil = stokKartRepository.StokkartMuadilDetay(stokkart_id, muadil_stokkart_id);
            if (muadil != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    muadil.stokkart_id,
                    muadil.muadil_stokkart_id,
                    muadil.tanim
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        ///  Stok Kart -> Diger / Muadil -> GET -> Muadil Stokkartların listesini getiren metod.
        /// </summary>
        /// <param name="stokkart_id">Zorunlu</param>
        /// <returns>
        /// 
        ///[
        ///  {
        ///    "stokkart_id": 1, O anda açık olan stokart' ın id si.
        ///    "muadil_stokkart_id": 2, seçilen stokkart_id.
        ///    "tanim": "",
        ///    "degistiren_carikart_id": 0
        ///  }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/muadil")]
        [Route("api/stokkart/muadil/{stokkart_id}")]
        public HttpResponseMessage StokkartMuadilListesi(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var muadiller = stokKartRepository.StokkartMuadilListesi(stokkart_id);
            if (muadiller != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, muadiller.Select(muadil => new
                {
                    muadil.stokkart_id,
                    muadil.muadil_stokkart_id,
                    muadil.tanim,
                    muadil.degistiren_carikart_id
                }));
            }
            else
            {
                //return Request.CreateResponse(HttpStatusCode.NoContent, new List<StokkartMuadil>());
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Stok Kart -> Diger / Muadil -> POST -> Muadil stokart ekler
        /// Not: Post işlemi sırasında stokkart_id ve  muadil_stokkart_id değişkenleri zorunludur!
        /// </summary>
        /// <param name="stokkartMuadil"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/muadil")]
        [Route("api/stokkart/muadil")]
        public HttpResponseMessage StokkartMuadilPost(StokkartMuadil stokkartMuadil)
        {
            AcekaResult acekaResult = null;
            if (stokkartMuadil != null && stokkartMuadil.stokkart_id > 0 && stokkartMuadil.muadil_stokkart_id > 0 && stokkartMuadil.stokkart_id != stokkartMuadil.muadil_stokkart_id)
            {
                stokkartMuadil.degistiren_tarih = DateTime.Now;
                stokkartMuadil.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<StokkartMuadil>.Insert(stokkartMuadil, "stokkart_muadil", new string[] { "tanim" });

                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else
                {
                    if (acekaResult.ErrorInfo.Message.Contains("duplicate"))
                    {
                        acekaResult.ErrorInfo.Message = "Bu kayıt daha önce açılmış. Lütfen farklı bir stok kart seçin!"; //POST tan sonra clientta hata dönüyorsa Http response da 405 hata döneceğiz. 
                    }
                    return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, acekaResult.ErrorInfo);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// Stok Kart -> Diger / Muadil -> DELETE -> Muadil stokart siler
        /// Not: Post işlemi sırasında stokkart_id ve  muadil_stokkart_id değişkenleri zorunludur!
        /// </summary>
        /// <param name="stokkartMuadil"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/stokkart/muadil")]
        [Route("api/stokkart/muadil")]
        public HttpResponseMessage StokkartMuadilDelete(StokkartMuadil stokkartMuadil)
        {
            AcekaResult acekaResult = null;
            if (stokkartMuadil != null && stokkartMuadil.stokkart_id > 0 && stokkartMuadil.muadil_stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var muadil = stokKartRepository.StokkartMuadilDetay(stokkartMuadil.stokkart_id, stokkartMuadil.muadil_stokkart_id);
                if (muadil != null)
                {
                    acekaResult = CrudRepository<stokkart_muadil>.Delete(muadil, "stokkart_muadil", new string[] { "stokkart_id", "muadil_stokkart_id" }, new string[] { "stokkart_id", "muadil_stokkart_id" });
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
                    return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        ///  Stok Kart -> Diger / Birim Dönüsturürücü -> GET -> Birim Dönüsturürücü detayını getiren metod.
        /// </summary>
        /// <param name="stokkart_id">Zorunlu</param>
        /// <returns>
        ///     {
        ///         stokkart_id:52034,
        ///         birim1x:0,
        ///         birim2x:0,
        ///         birim3x:0,
        ///         M2_gram:0,
        ///         eni:0,
        ///         fyne:0,
        ///         fire_orani:0,
        ///         birim_gram:0
        ///     }
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/birim-donustur")]
        [Route("api/stokkart/birim-donustur/{stokkart_id}")]
        public HttpResponseMessage StokkartBirim(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var ozel = stokKartRepository.StokkartOzelDetay(stokkart_id);
            if (ozel != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ozel.stokkart_id,
                    ozel.birim1x,
                    ozel.birim2x,
                    ozel.birim3x,
                    ozel.M2_gram,
                    ozel.eni,
                    ozel.fyne,
                    ozel.fire_orani
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Kullanılmayacak. Stok Kart -> Diger -> Birim Dönüsturürücü -> POST Metodu -> Birim Dönüsturürücü bilgilerini EKLER.
        /// </summary>
        /// <param name="stokkartBirim"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/birim-donustur")]
        [Route("api/stokkart/birim-donustur")]
        public HttpResponseMessage StokkartBirimPost(StokkartBirimDonusturucu stokkartBirim)
        {
            AcekaResult acekaResult = null;

            //if (stokkartBirim != null && stokkartBirim.stokkart_id > 0)
            //{
            stokKartRepository = new StokkartRepository();
            var stokkartOzel = stokKartRepository.StokkartOzelDetay(stokkartBirim.stokkart_id);
            if (stokkartOzel == null)
            {
                #region insert
                stokkartBirim.degistiren_tarih = DateTime.Now;
                acekaResult = CrudRepository<StokkartBirimDonusturucu>.Insert(stokkartBirim, "stokkart_ozel");
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                }
                #endregion
            }
            return null;
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            //}

        }

        /// <summary>
        ///  Stok Kart -> Diger -> Birim Dönüsturürücü -> PUT -> Birim Dönüsturürücü bilgilerini GÜNCELLER.
        /// </summary>
        /// <param name="stokkartBirim"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/birim-donustur")]
        [Route("api/stokkart/birim-donustur")]
        public HttpResponseMessage StokkartBirimPut(StokkartBirimDonusturucu stokkartBirim)
        {
            AcekaResult acekaResult = null;

            if (stokkartBirim != null && stokkartBirim.stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var stokkartOzel = stokKartRepository.StokkartOzelDetay(stokkartBirim.stokkart_id);
                if (stokkartOzel != null)
                {
                    #region Update
                    stokkartOzel.degistiren_carikart_id = stokkartBirim.degistiren_carikart_id;
                    stokkartOzel.degistiren_tarih = DateTime.Now;
                    stokkartOzel.birim1x = stokkartBirim.birim1x;
                    stokkartOzel.birim2x = stokkartBirim.birim2x;
                    stokkartOzel.birim3x = stokkartBirim.birim3x;
                    stokkartOzel.M2_gram = stokkartBirim.M2_gram;
                    stokkartOzel.eni = stokkartBirim.eni;
                    stokkartOzel.fyne = stokkartBirim.fyne;
                    stokkartOzel.fire_orani = stokkartBirim.fire_orani;

                    acekaResult = CrudRepository<stokkart_ozel>.Update(stokkartOzel, "stokkart_id");
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                    }
                    #endregion
                }
                else
                {
                    #region insert
                    stokkartBirim.degistiren_tarih = DateTime.Now;
                    acekaResult = CrudRepository<StokkartBirimDonusturucu>.Insert(stokkartBirim, "stokkart_ozel");
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                    }
                    #endregion
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }

        #endregion

        #region Stokkart_Fiyat Sekmesi
        /// <summary>
        /// stokkart Fiyat tabındaki bilgileri döndürür stokkart_id parametresini almaktadır
        /// </summary>
        /// <param name="stokkart_id">stokkart_fiyat tablosunda stokkart_id alanı.</param>
        /// <returns>
        ///[
        /// {
        ///   "stokkart_id": 29657,
        ///   "fiyattipi": "AF",
        ///   "tarih": "2017-03-25T00:00:00",
        ///   "fiyat": 200,
        ///   "fiyattipi_adi": "Sabit Alış Fiyatı",
        ///   "pb": 0,
        ///   "pb_adi": null,
        ///   "pb_kodu": "TL",
        ///   "degistiren_carikart_id": 0
        /// },
        /// {
        ///   "stokkart_id": 29657,
        ///   "fiyattipi": "AF",
        ///   "tarih": "2017-04-16T00:00:00",
        ///   "fiyat": 235,
        ///   "fiyattipi_adi": "Sabit Alış Fiyatı",
        ///   "pb": 0,
        ///   "pb_adi": null,
        ///   "pb_kodu": "TL",
        ///   "degistiren_carikart_id": 0
        /// }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/fiyat")]
        [Route("api/stokkart/fiyat/{stokkart_id}")]
        public HttpResponseMessage StokkartGetir(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            var stok_fiyat = stokKartRepository.StokkartFiyat(stokkart_id);
            if (stok_fiyat != null && stok_fiyat.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, stok_fiyat.Select(f => new
                {
                    f.stokkart_id,
                    f.fiyattipi,
                    f.tarih,
                    f.fiyat,
                    f.fiyattipi_adi,
                    f.pb,
                    f.pb_adi,
                    f.pb_kodu,
                    f.degistiren_carikart_id,
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// POST - > stokkart insert method.
        /// NOT: stokkart_id boş gönderilmemeli.
        /// DİKKAT Aynı stokkart_id, fiyattipi ve tarih bilgilerinden sadece bir adet olabilir. Tabloda 3 adet key var. DİKKAT.
        /// PUT ve Delete Methodları olmayacak. 
        /// </summary>
        /// <param name="stokkartfiyat"></param>
        /// <returns>
        /// {
        ///  "stokkart_id": 1,
        ///  "fiyattipi": "sample string 2",
        ///  "tarih": "2017-02-20T09:07:14.693+03:00",
        ///  "degistiren_carikart_id": 4,
        ///  "degistiren_tarih": "2017-02-20T09:07:14.693+03:00",
        ///  "fiyat": 6.0,
        ///  "pb": 0
        ///}
        /// </returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/fiyat")]
        [Route("api/stokkart/fiyat")]
        public HttpResponseMessage Post(stokkart_fiyat stokkartfiyat)
        {
            /*DİKKAT Aynı stokkart_id, fiyattipi ve tarih bilgilerinden sadece bir adet olabilir. DİKKAT.*/
            AcekaResult acekaResult = null;

            if (stokkartfiyat != null && stokkartfiyat.stokkart_id > 0)
            {

                stokkart_fiyat stokfiyat = new stokkart_fiyat();
                stokfiyat.stokkart_id = stokkartfiyat.stokkart_id;
                stokfiyat.fiyat = stokkartfiyat.fiyat;
                stokfiyat.degistiren_tarih = DateTime.Now;
                stokfiyat.degistiren_carikart_id = Tools.PersonelId;
                stokfiyat.fiyattipi = stokkartfiyat.fiyattipi;
                stokfiyat.pb = stokkartfiyat.pb;
                stokfiyat.tarih = stokkartfiyat.tarih;


                acekaResult = CrudRepository<stokkart_fiyat>.Insert(stokfiyat, "stokkart_fiyat", new string[] { "fiyattipi_adi", "pb_adi", "kayit_silindi", "pb_kodu", "alis_fiyati", "satis_fiyati" });

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
        ///  PUT - > stokkart fiyat update method. UPDATE olmayacak. Yanlış girilen kaydı yeni tarihli tekrar girecek.
        ///  İleri tarih için update yapabilecek.
        /// </summary>
        /// <param name="stokkartfiyat"></param>
        /// <returns>null</returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/fiyat")]
        [Route("api/stokkart/fiyat")]
        public HttpResponseMessage Put(stokkart_fiyat stokkartfiyat)
        {
            AcekaResult acekaResult = null;
            if (stokkartfiyat != null && stokkartfiyat.stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var fiyatdetay = stokKartRepository.StkkartFiyat(stokkartfiyat.stokkart_id);
                if (fiyatdetay != null && stokkartfiyat.tarih < fiyatdetay.tarih)
                {
                    fiyatdetay.degistiren_carikart_id = Tools.PersonelId;
                    fiyatdetay.degistiren_tarih = DateTime.Now;
                    fiyatdetay.fiyat = stokkartfiyat.fiyat;
                    fiyatdetay.fiyattipi = stokkartfiyat.fiyattipi;
                    fiyatdetay.tarih = stokkartfiyat.tarih;
                    fiyatdetay.pb = stokkartfiyat.pb;
                    acekaResult = CrudRepository<stokkart_fiyat>.Update(fiyatdetay, "stokkart_id", new string[] { "fiyattipi_adi", "pb_adi", "alis_fiyati", "satis_fiyati", "pb_kodu", "kayit_silindi" });
                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Bugün ve geçmiş tarihli kaydı güncelleyemezsiniz." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
            return null;
        }

        ///// <summary>
        /////  Delete - > stokkart fiyat Silme method. Silme Kesinlikle Olmayacak.
        ///// </summary>
        ///// <param name="stokkartfiyat"></param>
        ///// <returns>null</returns>
        //[HttpDelete]
        //[CustAuthFilter(ApiUrl = "api/stokkart/fiyat")]
        //[Route("api/stokkart/fiyat")]
        //public HttpResponseMessage Delete(stokkart_fiyat stokkartfiyat)
        //{
        //    //YAPILMAYACAK.
        //    return null;

        //}

        #endregion

        #region StokKart_Uyarilar
        /// <summary>
        /// stokkart Uyarilar tabındaki bilgileri döndürür stokkart_id parametresini almaktadır
        /// </summary>
        /// <param name="stokkart_id">stokkart_kontrol tablosunda stokkart_id alanı.</param>
        /// <returns>
        ///{
        ///  "stokkart_id": 1,
        ///  "degistiren_carikart_id": 0,
        ///  "degistiren_tarih": "2017-02-24T15:49:17",
        ///  "tedarik_edilemez": false,
        ///  "musteri_siparisi_icin_acik": false,
        ///  "eksi_stok_izin": false,
        ///  "eksi_stok_uyari": true,
        ///  "min_stok_uyari": true,
        ///  "satin_alma_testi_gerekli_uyari": true,
        ///  "her_sezon_onay_gerekli": true,
        ///  "beden_bazli_kullanim": true,
        ///  "sezon_onayi_yok_uyarisi": false
        ///}
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/stokkart/uyari")]
        [Route("api/stokkart/uyari/{stokkart_id}")]
        public stokkart_kontrol StokkartUyariGetir(long stokkart_id)
        {
            stokKartRepository = new StokkartRepository();
            stokkartkontrol = stokKartRepository.StokkartUyariGetir(stokkart_id);
            return stokkartkontrol;
        }

        /// <summary>
        /// Stokkart ->  Uyarılar -> POST.
        /// </summary>
        /// <param name="stokuyarilar">stokkart_kontrol tablosunda stokuyarilar.stokkart_id alanı.</param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/stokkart/uyari")]
        [Route("api/stokkart/uyari")]
        public HttpResponseMessage UyarilarPost(StokkartUyari stokuyarilar)
        {
            AcekaResult acekaResult = null;

            if (stokuyarilar != null && stokuyarilar.stokkart_id > 0)
            {
                stokkart_kontrol uyarilar = new stokkart_kontrol();
                uyarilar.stokkart_id = stokuyarilar.stokkart_id;
                uyarilar.beden_bazli_kullanim = stokuyarilar.beden_bazli_kullanim;
                uyarilar.degistiren_carikart_id = Tools.PersonelId;
                uyarilar.degistiren_tarih = DateTime.Now;
                uyarilar.eksi_stok_izin = stokuyarilar.eksi_stok_izin;
                uyarilar.eksi_stok_uyari = stokuyarilar.eksi_stok_uyari;
                uyarilar.her_sezon_onay_gerekli = stokuyarilar.her_sezon_onay_gerekli;
                uyarilar.min_stok_uyari = stokuyarilar.min_stok_uyari;
                uyarilar.musteri_siparisi_icin_acik = stokuyarilar.musteri_siparisi_icin_acik;
                uyarilar.tedarik_edilemez = stokuyarilar.tedarik_edilemez;
                uyarilar.satin_alma_testi_gerekli_uyari = stokuyarilar.satin_alma_testi_gerekli_uyari;
                uyarilar.sezon_onayi_yok_uyarisi = stokuyarilar.sezon_onayi_yok_uyarisi;

                acekaResult = CrudRepository<stokkart_kontrol>.Insert(uyarilar);

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
        /// PUT -> stokkart Uyarilar tabındaki bilgileri günceller.
        /// </summary>
        /// <param name="stokuyari">stokkart_kontrol tablosunda stokuyari.stokkart_id alanı.</param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/stokkart/uyari")]
        [Route("api/stokkart/uyari")]
        public HttpResponseMessage StokkartUyariGetir(StokkartUyari stokuyari)
        {
            AcekaResult acekaResult = null;

            if (stokuyari != null && stokuyari.stokkart_id > 0)
            {
                stokKartRepository = new StokkartRepository();
                var stokkartkontrol = stokKartRepository.StokkartUyariGetir(stokuyari.stokkart_id);
                if (stokkartkontrol != null)
                {
                    //update
                    stokkartkontrol.her_sezon_onay_gerekli = stokuyari.her_sezon_onay_gerekli;
                    stokkartkontrol.min_stok_uyari = stokuyari.min_stok_uyari;
                    stokkartkontrol.musteri_siparisi_icin_acik = stokuyari.musteri_siparisi_icin_acik;
                    stokkartkontrol.tedarik_edilemez = stokuyari.tedarik_edilemez;
                    stokkartkontrol.satin_alma_testi_gerekli_uyari = stokuyari.satin_alma_testi_gerekli_uyari;
                    stokkartkontrol.sezon_onayi_yok_uyarisi = stokuyari.sezon_onayi_yok_uyarisi;
                    stokkartkontrol.eksi_stok_izin = stokuyari.eksi_stok_izin;
                    stokkartkontrol.eksi_stok_uyari = stokuyari.eksi_stok_uyari;
                    stokkartkontrol.beden_bazli_kullanim = stokuyari.beden_bazli_kullanim;
                    stokkartkontrol.degistiren_tarih = DateTime.Now;
                    stokkartkontrol.degistiren_carikart_id = 0;

                    acekaResult = CrudRepository<stokkart_kontrol>.Update(stokkartkontrol, "stokkart_id");
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
                    //insert
                    stokkart_kontrol uyarilar = new stokkart_kontrol();
                    uyarilar.stokkart_id = stokuyari.stokkart_id;
                    uyarilar.beden_bazli_kullanim = stokuyari.her_sezon_onay_gerekli;
                    uyarilar.degistiren_carikart_id = stokuyari.degistiren_carikart_id;
                    uyarilar.degistiren_tarih = DateTime.Now;
                    uyarilar.eksi_stok_izin = stokuyari.eksi_stok_izin;
                    uyarilar.eksi_stok_uyari = stokuyari.eksi_stok_uyari;
                    uyarilar.her_sezon_onay_gerekli = stokuyari.her_sezon_onay_gerekli;
                    uyarilar.min_stok_uyari = stokuyari.min_stok_uyari;
                    uyarilar.musteri_siparisi_icin_acik = stokuyari.musteri_siparisi_icin_acik;
                    uyarilar.tedarik_edilemez = stokuyari.tedarik_edilemez;
                    uyarilar.satin_alma_testi_gerekli_uyari = stokuyari.satin_alma_testi_gerekli_uyari;
                    uyarilar.sezon_onayi_yok_uyarisi = stokuyari.sezon_onayi_yok_uyarisi;

                    acekaResult = CrudRepository<stokkart_kontrol>.Insert(uyarilar);

                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo);
                    }

                }


            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            }

        }
        #endregion

        #endregion
    }
}
