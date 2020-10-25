using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
using aceka.web_api.Models.GenelAyarlar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using static aceka.infrastructure.Models.GenelAyarlar;

namespace aceka.web_api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GenelAyarlarController : ApiController
    {
        #region Degiskenler
        private List<takvim> takvim = null;
        private GenelAyarlarRepository genelayarlarRepository = null;
        private ParametreRepository parametreRepository = null;
        private CarikartRepository carikartRepository = null;
        private List<cari_kart> cari_kart = null;
        private StokkartRepository stokkartrepository = null;
        private List<stokkart> stk = null;
        private gtip_belge gtip_belge = null;
        private List<gtip_belge> gtip_belgeler = null;
        private Models.GenelAyarlar.GtipModel.gtip_belge gtipModel = null;
        private gtip_belgedetay gtip_belgedetay = null;
        private List<Kur> kurlar = null;
        private KurRepository kurRepository = null;
        private List<SistemAyarlari> mSistemAyarlari = null;
        private talimattanim mTalimatTanim = null;
        private List<talimattanim> mTalimatList = null;

        #endregion

        /// <summary>
        /// Gtip belgelerin listesini veriyor.
        /// </summary>
        /// <returns>
        ///[
        ///    {
        ///        "belgeno": "2010/H-19/2",
        ///        "belge_tarihi": "2010-03-11T00:00:00",
        ///        "bitis_tarihi": "2012-03-27T00:00:00",
        ///        "cari_unvan": "AJARA TEXTILE LTD."
        ///    },
        ///    {
        ///        "belgeno": "2010/H-19/3",
        ///        "belge_tarihi": "2012-03-27T00:00:00",
        ///        "bitis_tarihi": "2013-11-15T00:00:00",
        ///        "cari_unvan": "AJARA TEXTILE LTD."
        ///    },
        ///    {
        ///        "belgeno": "2013/H-72",
        ///        "belge_tarihi": "2013-11-15T00:00:00",
        ///        "bitis_tarihi": "2014-12-10T00:00:00",
        ///        "cari_unvan": "AJARA TEXTILE LTD."
        ///    },
        ///    {
        ///        "belgeno": "2014/H-79",
        ///        "belge_tarihi": "2014-12-10T00:00:00",
        ///        "bitis_tarihi": "2018-12-10T00:00:00",
        ///        "cari_unvan": "AJARA TEXTILE LTD."
        ///    }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/gtipBelgeler")]
        [Route("api/genelayarlar/gtipBelgeler")]
        public HttpResponseMessage gtipBelgeler()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            gtip_belgeler = new List<gtip_belge>();
            gtip_belgeler = genelayarlarRepository.GtipListe();

            if (gtip_belgeler != null)
            {
                var gtiplist = gtip_belgeler.Select(o => new
                {
                    o.belge_id,
                    o.belgeno,
                    belge_tarihi = o.belge_tarihi.ToShortDateString(),
                    // o.belge_tarihi.DateTime.ToString("d"),
                    bitis_tarihi = o.bitis_tarihi.ToShortDateString(),
                    o.cari_unvan
                });
                return Request.CreateResponse(HttpStatusCode.OK, gtip_belgeler);
            }
            else return null; ;

        }

        #region SistemAyarlari

        /// <summary>
        /// Sistem Ayarlari GET Metodu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/sistemayarlari")]
        [Route("api/genelayarlar/sistemayarlari")]
        public HttpResponseMessage sistemayarlari()
        {

            genelayarlarRepository = new GenelAyarlarRepository();

            mSistemAyarlari = genelayarlarRepository.TumSistemAyarlari();

            if (mSistemAyarlari != null)
            {
                var sistemAyarlariListesi = mSistemAyarlari.Select(x => new
                {
                    x.ayar,
                    x.ayaradi,
                    x.ayaraciklama,
                    x.degistiren_carikart_id
                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, sistemAyarlariListesi);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sistemModel"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/sistemayarlariput")]
        [Route("api/genelayarlar/sistemayarlariput")]
        public IHttpActionResult sistemayarlariput(Models.GenelAyarlar.SistemModel.SistemAyarlariModel sistemModel)
        {
            AcekaResult acekaResult = null;
            if (sistemModel != null && sistemModel.ayar != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("belge_id", sistemModel.ayaradi);
                fields.Add("ayar", sistemModel.ayar);
                
                

                string[] Wherefields = { "belge_id" };
                acekaResult = CrudRepository.Update("gtip_belgedetay", Wherefields, fields);

                return Ok(acekaResult);
            }
            return BadRequest();
        }
        #endregion

        /// <summary>
        /// GTip All Get Methodu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/gtipget")]
        [Route("api/genelayarlar/gtipget")]
        public HttpResponseMessage GTipGet()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            gtipModel = new Models.GenelAyarlar.GtipModel.gtip_belge();
            gtip_belgeler = genelayarlarRepository.gtipBelgeListDetay();

            if (gtipModel != null)
            {

                var gtiplist = gtip_belgeler.Select(x => new
                {
                    x.acan_carikart_id,
                    x.acan_tarih,
                    x.belgeno,
                    x.gtipdetay.gtip_bayan,
                    x.gtipdetay.gtip_genel,
                    x.gtipdetay.birim,
                    x.gtipdetay.kg,
                    x.belge_id,
                    x.belge_tarihi,
                    x.bitis_tarihi,
                    x.gtipdetay.aciklama,
                    x.gtipdetay.adet,
                    x.gtipdetay.birim_fob,
                    x.gtipdetay.toplam_fob

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, gtiplist);

            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new Exception().Message);
        }

        /// <summary>
        /// GTip GET Sorgulama
        /// </summary>
        /// <param name="stokkart_tipi_id"></param>
        /// <param name="modeltipi_id"></param>
        /// <param name="kumastipi_id"></param>
        /// <param name="belge_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/gtiparama")]
        [Route("api/genelayarlar/gtiparama")]
        public HttpResponseMessage GTipArama(byte stokkart_tipi_id = 0, int belge_id = 0, int modeltipi_id = -1, int kumastipi_id = 0)
        {
            if (stokkart_tipi_id != 0 && belge_id > 0 && belge_id != 0)
            {
                genelayarlarRepository = new GenelAyarlarRepository();
                gtipModel = new Models.GenelAyarlar.GtipModel.gtip_belge();
                gtip_belgeler = genelayarlarRepository.GTipBul(stokkart_tipi_id, belge_id, modeltipi_id, kumastipi_id);

                if (gtip_belgeler != null)
                {

                    var gtiplist = gtip_belgeler.Select(x => new
                    {
                        x.acan_carikart_id,
                        x.acan_tarih,
                        x.belgeno,
                        x.gtipdetay.gtip_bayan,
                        x.gtipdetay.gtip_genel,
                        x.gtipdetay.birim,
                        x.gtipdetay.kg,
                        x.belge_id,
                        x.belge_tarihi,
                        x.bitis_tarihi,
                        x.gtipdetay.aciklama,
                        x.gtipdetay.adet,
                        x.gtipdetay.birim_fob,
                        x.gtipdetay.toplam_fob,
                        x.gtipdetay.birim_adi,
                        x.gtipdetay.degistiren_tarih,
                        x.gtipdetay.stokalan_id_1,
                        x.gtipdetay.stokalan_id_2,
                        x.gtipdetay.stokalan_id_3,
                        x.gtipdetay.stokalan_id_4,
                        x.gtipdetay.stokkart_tipi_id,
                        x.gtipdetay.pb

                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, gtiplist);
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
        /// GTip PUT İşlemi
        /// </summary>
        /// <param name="gtipModel"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/gtipput")]
        [Route("api/genelayarlar/gtipput")]
        public IHttpActionResult GTipPut(Models.GenelAyarlar.GtipModel.gtip_belge gtipModel)
        {
            AcekaResult acekaResult = null;
            if (gtipModel != null && gtipModel.belge_id > 0)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("belge_id", gtipModel.belge_id);
                fields.Add("stokkart_tipi_id", gtipModel.stokkart_tipi_id);
                fields.Add("stokalan_id_1", gtipModel.stokalan_id_1);
                fields.Add("stokalan_id_2", gtipModel.stokalan_id_2);
                fields.Add("stokalan_id_3", gtipModel.stokalan_id_3);
                fields.Add("stokalan_id_4", gtipModel.stokalan_id_4);
                fields.Add("degistiren_carikart_id", gtipModel.degistiren_carikart_id);
                fields.Add("degistiren_tarih", gtipModel.degistiren_tarih);
                fields.Add("gtip_genel", gtipModel.gtip_genel);
                fields.Add("gtip_bayan", gtipModel.gtip_bayan);
                fields.Add("aciklama", gtipModel.aciklama);
                fields.Add("birim", gtipModel.birim);
                fields.Add("adet", gtipModel.adet);
                fields.Add("kg", gtipModel.kg);
                fields.Add("birim_fob", gtipModel.birim_fob);
                fields.Add("toplam_fob", gtipModel.toplam_fob);
                fields.Add("pb", gtipModel.pb);

                string[] Wherefields = { "belge_id", "stokkart_tipi_id", "stokalan_id_1", "stokalan_id_2", "stokalan_id_3", "stokalan_id_4" };
                acekaResult = CrudRepository.Update("gtip_belgedetay", Wherefields, fields);

                return Ok(acekaResult);
            }
            return BadRequest();
        }


        //// <summary>
        //// GTip Detayları Silme İşlemi İçin Kullanılır
        //// </summary>
        //// <param name="gtipModel"></param>
        //// <returns></returns>
        //[HttpDelete]
        //[CustAuthFilter(ApiUrl = "api/genelayarlar/gtipsil")]
        //[Route("api/genelayarlar/gtipsil")]
        //public HttpResponseMessage GTipSil(Models.GenelAyarlar.GtipModel.gtip_belge gtipModel)
        //{
        //    AcekaResult acekaResult = null;
        //    if (gtipModel != null && gtipModel.belge_id > 0)
        //    {
        //        Dictionary<string, object> fields = new Dictionary<string, object>();
        //        fields.Add("belge_id", gtipModel.belge_id);
        //        fields.Add("stokkart_tipi_id", gtipModel.stokkart_tipi_id);
        //        fields.Add("stokalan_id_1", gtipModel.stokalan_id_1);
        //        fields.Add("stokalan_id_2", gtipModel.stokalan_id_2);
        //        fields.Add("stokalan_id_3", gtipModel.stokalan_id_3);
        //        fields.Add("stokalan_id_4", gtipModel.stokalan_id_4);
        //        fields.Add("degistiren_carikart_id", gtipModel.degistiren_carikart_id);
        //        fields.Add("degistiren_tarih", gtipModel.degistiren_tarih);
        //        fields.Add("gtip_genel", gtipModel.gtip_genel);
        //        fields.Add("gtip_bayan", gtipModel.gtip_bayan);
        //        fields.Add("aciklama", gtipModel.aciklama);
        //        fields.Add("birim", gtipModel.birim);
        //        fields.Add("adet", gtipModel.adet);
        //        fields.Add("kg", gtipModel.kg);
        //        fields.Add("birim_fob", gtipModel.birim_fob);
        //        fields.Add("toplam_fob", gtipModel.toplam_fob);
        //        fields.Add("pb", gtipModel.pb);
        //        acekaResult = CrudRepository.Delete("gtip_belgedetay", new string[] { "belge_id", "stokkart_tipi_id", "stokalan_id_1", "stokalan_id_2", "stokalan_id_3", "stokalan_id_4", "degistiren_carikart_id", "degistiren_tarih", "gtip_genel", "gtip_bayan", "aciklama", "birim", "adet", "kg", "birim_fob", "toplam_fob", "pb" }, fields);
        //        return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
        //    }
        //    return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
        //}

        /// <summary>
        /// GTip Post Methodu
        /// </summary>
        /// <param name="gtipModel"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/genelayarlar/GTipPost")]
        [Route("api/genelayarlar/gtippost")]
        public HttpResponseMessage GTipPost(Models.GenelAyarlar.GtipModel.gtip_belge gtipModel)
        {
            AcekaResult acekaResult = null;

            var model = new Models.GenelAyarlar.GtipModel.gtip_belge
            {
                acan_carikart_id = Tools.PersonelId,
                belge_id = gtipModel.belge_id,
                acan_tarih = DateTime.Now,
                belgeno = gtipModel.belgeno,
                belge_tarihi = gtipModel.belge_tarihi,
                bitis_tarihi = gtipModel.bitis_tarihi,
                carikart_id = gtipModel.carikart_id,
                cari_unvan = gtipModel.cari_unvan,
                degistiren_carikart_id = Tools.PersonelId,
                degistiren_tarih = DateTime.Now,
                aciklama = gtipModel.aciklama,
                adet = gtipModel.adet,
                birim = gtipModel.birim,
                birim_fob = gtipModel.birim_fob,
                gtip_bayan = gtipModel.gtip_bayan,
                gtip_genel = gtipModel.gtip_genel,
                kg = gtipModel.kg,
                pb = gtipModel.pb,
                stokalan_id_1 = gtipModel.stokalan_id_1,
                stokalan_id_2 = gtipModel.stokalan_id_2,
                stokalan_id_3 = gtipModel.stokalan_id_3,
                stokalan_id_4 = gtipModel.stokalan_id_4,
                stokkart_tipi_id = gtipModel.stokkart_tipi_id,
                toplam_fob = gtipModel.toplam_fob
            };

            acekaResult = CrudRepository<Models.GenelAyarlar.GtipModel.gtip_belge>.Insert(model, "gtip_belge", new string[] { "stokkart_tipi_id", "stokalan_id_1", "stokalan_id_2", "stokalan_id_3", "stokalan_id_4", "gtip_genel", "gtip_bayan", "aciklama", "birim", "adet", "kg", "birim_fob", "toplam_fob", "pb" });
            acekaResult = CrudRepository<Models.GenelAyarlar.GtipModel.gtip_belge>.Insert(model, "gtip_belgedetay", new string[] { "carikart_id", "belgeno", "belge_tarihi", "bitis_tarihi", "cari_unvan" });

            if (acekaResult == null || acekaResult.ErrorInfo != null)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
            }

            return null;
        }

        /// <summary>
        /// Takvim GET metodu. AA. sadece sene bilgilerini listeler
        ///  api/GenelAyarlar
        /// </summary>
        /// <returns>
        /// {
        ///    "sene": 2016
        ///  },
        ///  {
        ///    "sene": 2017
        ///  }
        /// </returns>
        [HttpGet]
        [Route("api/GenelAyarlarSeneler")]
        public HttpResponseMessage Get()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            takvim = genelayarlarRepository.SeneGetir();
            if (takvim != null)
            {
                var sene = takvim.Select(s => new
                {
                    s.sene
                });
                return Request.CreateResponse(HttpStatusCode.OK, sene);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Takvim GET metodu. AA. (takvim metoduna sene parametresi verilerek ilgili senenin günlerini listeler)
        ///  api/GenelAyarlar
        /// </summary>
        /// <param name="sene">Listelenecek tarih parametresi</param>
        /// <returns>
        /// {
        ///    "sene": 2017,
        ///    "ay": 4,
        ///    "hafta": 18,
        ///    "tarih": "2017-04-29T00:00:00",
        ///    "gun": 6,
        ///    "tatil_turu": 161,
        ///    "aciklama": "Hafta Sonu"
        ///  },
        ///  {
        ///    "sene": 2017,
        ///    "ay": 4,
        ///    "hafta": 18,
        ///    "tarih": "2017-04-30T00:00:00",
        ///    "gun": 7,
        ///    "tatil_turu": 161,
        ///    "aciklama": "Hafta Sonu"
        ///  }
        /// </returns>
        [HttpGet]
        [Route("api/GenelAyarlar/{sene}")]
        public IList<takvim> Get(int sene)
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            takvim = genelayarlarRepository.Getir();
            if (takvim != null)
            {
                List<takvim> ayar = takvim.Where(s => s.sene == sene).ToList();
                return ayar;
            }
            return null; ;
        }

        /// <summary>
        /// Takvim GET metodu. AA. bütün seneleri günleriyle beraber listeler 
        ///  api/GenelAyarlar
        /// </summary>
        /// <returns>
        /// {
        ///    "sene": 2015,
        ///    "ay": 4,
        ///    "hafta": 18,
        ///    "tarih": "2017-04-29T00:00:00",
        ///    "gun": 6,
        ///    "tatil_turu": 161,
        ///    "aciklama": "Hafta Sonu"
        ///  },
        ///  {
        ///    "sene": 2016,
        ///    "ay": 4,
        ///    "hafta": 18,
        ///    "tarih": "2017-04-29T00:00:00",
        ///    "gun": 6,
        ///    "tatil_turu": 161,
        ///    "aciklama": "Hafta Sonu"
        ///  },
        ///  {
        ///    "sene": 2017,
        ///    "ay": 4,
        ///    "hafta": 18,
        ///    "tarih": "2017-04-30T00:00:00",
        ///    "gun": 7,
        ///    "tatil_turu": 161,
        ///    "aciklama": "Hafta Sonu"
        ///  }
        /// </returns>
        [HttpGet]
        [Route("api/GenelAyarlar")]
        public IList<takvim> ButunSeneler()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            takvim = genelayarlarRepository.Getir();
            return takvim; ;
        }
        /// PUT: api/GenelAyarlar/5
        /// <summary>
        /// tarih bilgilerini UPDATE yapar
        /// {"tarih": "2017-05-01T00:00:00","tatil_turu" :"162",   "aciklama":"1 Mayıs" } 
        /// bu üç alan yeterlidir
        /// </summary>
        /// /// <param name="t"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/GenelAyarlar")]
        public HttpResponseMessage PersonelParametreput(takvim t)
        {
            AcekaResult acekaResult = null;
            if (t != null)
            {
                if (t.tarih.Date.acekaToString().Length.acekaToInt() > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("tarih", t.tarih);
                    fields.Add("tatil_turu", t.tatil_turu);
                    fields.Add("aciklama", t.aciklama);
                    acekaResult = CrudRepository.Update("takvim", "tarih", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        // DELETE: api/GenelAyarlar/5
        //public void Delete(int id)
        //{
        //}

        /// <summary>
        /// toplu fiyat tanım ekranı için marka listesini döndürür
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         parametre_id = 2
        ///         tanim =Puma TR
        ///     }
        ///     {
        ///         parametre_id = 11
        ///         tanim = Lotto TR
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/fiyat/markalar")]
        public HttpResponseMessage TopluFiyatTanim_Marka()
        {
            parametreRepository = new ParametreRepository();
            var parametreler = parametreRepository.StokkartRaporParametreGetir();
            if (parametreler != null && parametreler.Count > 0)
            {
                var marka = parametreler.Where(p => p.parametre == 1 & p.parametre_grubu == 0).Select(s => new
                {
                    s.parametre_id,
                    s.kod,
                    s.tanim
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, marka);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// toplu fiyat tanım ekranı için üretim yeri listesini döndürür
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         parametre_id = 104
        ///         tanim = AJARA-Georgia
        ///     }
        ///     {
        ///         parametre_id = 106
        ///         tanim = MILTEKS-Turkey
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/fiyat/uretimyeri")]
        public HttpResponseMessage TopluFiyatTanim_UretimYeri()
        {
            parametreRepository = new ParametreRepository();
            var parametreler = parametreRepository.StokkartRaporParametreGetir();
            if (parametreler != null && parametreler.Count > 0)
            {
                var marka = parametreler.Where(p => p.parametre == 2 & p.parametre_grubu == 0).Select(s => new
                {
                    s.parametre_id,
                    s.kod,
                    s.tanim
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, marka);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// toplu fiyat tanım ekranı için fasoncu listesini döndürür
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         parametre_id = 104
        ///         tanim = AJARA-Georgia
        ///     }
        ///     {
        ///         parametre_id = 106
        ///         tanim = MILTEKS-Turkey
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/fiyat/uretimyeri")]
        public HttpResponseMessage TopluFiyatTanim_Fasoncular()
        {
            stokkartrepository = new StokkartRepository();
            var fasoncu = stokkartrepository.FasoncuGetir();
            if (fasoncu != null && fasoncu.Count > 0)
            {
                var f = fasoncu.Select(s => new
                {
                    s.carikart_id,
                    s.cari_unvan
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, f);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// toplu fiyat tanım ekranı için seçilen filtrelere göre ürün listesi
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        /// [
        ///     {
        ///         
        ///     }
        ///     {
        ///      
        ///     }
        /// ]
        /// </returns>
        [HttpGet]
        [Route("api/fiyat/modellistesi")]
        public HttpResponseMessage ModelKart(string modelozellik, int marka, int uretimyeri)
        {
            stokkartrepository = new StokkartRepository();
            stk = new List<stokkart>();
            stk = stokkartrepository.ModelGetir(modelozellik, marka, uretimyeri);

            if (stk != null)
            {
                var s = stk.Select(o => new
                {
                    o.stokkart_id,
                    o.stok_adi,
                    o.stok_kodu,
                    o.stokkart_ozel.orjinal_stok_kodu
                });
                return Request.CreateResponse(HttpStatusCode.OK, s);
            }
            else return null; ;
        }

        /// <summary>
        /// toplu fiyat tanım ekranı Post metodu
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/fiyat/fiyat-talimat")]
        public HttpResponseMessage FiyatTalimat(stokkart_fiyat_talimat t)
        {
            AcekaResult acekaResult = new AcekaResult();
            if (t != null)
            {
                t.degistiren_carikart_id = 0;
                acekaResult = CrudRepository<stokkart_fiyat_talimat>.Insert(t, "stokkart_fiyat_talimat", new string[] { "degistiren_tarih" });
                return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
            }
            return null;
        }
        /// <summary>
        /// toplu fiyat tanım ekranı Put metodu
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/fiyat/fiyat-talimat")]
        public HttpResponseMessage FiyatTalimatUpdate(stokkart_fiyat_talimat t)
        {
            AcekaResult acekaResult = new AcekaResult();
            if (t != null)
            {
                t.degistiren_carikart_id = 0;
                Dictionary<string, object> fields_muh = new Dictionary<string, object>();
                fields_muh.Add("fiyat", t.fiyat);
                fields_muh.Add("degistiren_carikart_id", 0);

                fields_muh.Add("stokkart_id", t.stokkart_id);
                fields_muh.Add("carikart_id", t.carikart_id);
                fields_muh.Add("talimatturu_id_1", t.talimatturu_id_1);
                fields_muh.Add("talimatturu_id_2", t.talimatturu_id_2);
                fields_muh.Add("talimatturu_id_3", t.talimatturu_id_3);
                fields_muh.Add("talimatturu_id_4", t.talimatturu_id_4);
                fields_muh.Add("talimatturu_id_5", t.talimatturu_id_5);
                fields_muh.Add("talimatturu_id_6", t.talimatturu_id_6);
                fields_muh.Add("talimatturu_id_7", t.talimatturu_id_7);
                fields_muh.Add("talimatturu_id_8", t.talimatturu_id_8);
                fields_muh.Add("talimatturu_id_9", t.talimatturu_id_9);
                fields_muh.Add("pb", t.pb);
                //acekaResult = CrudRepository<stokkart_fiyat_talimat>.Update(t, "stokkart_fiyat_talimat", new string[] { "stokkart_id", "carikart_id", "talimatturu_id_1", "talimatturu_id_2", "talimatturu_id_3", "talimatturu_id_4", "talimatturu_id_5", "talimatturu_id_6", "talimatturu_id_7", "talimatturu_id_8", "talimatturu_id_9", "pb" });
                acekaResult = CrudRepository.Update("stokkart_fiyat_talimat", new string[] { "stokkart_id", "carikart_id", "talimatturu_id_1", "talimatturu_id_2", "talimatturu_id_3", "talimatturu_id_4", "talimatturu_id_5", "talimatturu_id_6", "talimatturu_id_7", "talimatturu_id_8", "talimatturu_id_9", "pb" }, fields_muh);
                return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
            }
            return null;
        }

        /// <summary>
        /// toplu fiyat tanım ekranı Delete metodu
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/fiyat/fiyat-talimat")]
        public HttpResponseMessage FiyatTalimatDelete(stokkart_fiyat_talimat t)
        {
            AcekaResult acekaResult = new AcekaResult();
            if (t != null)
            {
                t.degistiren_carikart_id = 0;
                Dictionary<string, object> fields_muh = new Dictionary<string, object>();
                fields_muh.Add("fiyat", t.fiyat);
                fields_muh.Add("degistiren_carikart_id", 0);

                fields_muh.Add("stokkart_id", t.stokkart_id);
                fields_muh.Add("carikart_id", t.carikart_id);
                fields_muh.Add("talimatturu_id_1", t.talimatturu_id_1);
                fields_muh.Add("talimatturu_id_2", t.talimatturu_id_2);
                fields_muh.Add("talimatturu_id_3", t.talimatturu_id_3);
                fields_muh.Add("talimatturu_id_4", t.talimatturu_id_4);
                fields_muh.Add("talimatturu_id_5", t.talimatturu_id_5);
                fields_muh.Add("talimatturu_id_6", t.talimatturu_id_6);
                fields_muh.Add("talimatturu_id_7", t.talimatturu_id_7);
                fields_muh.Add("talimatturu_id_8", t.talimatturu_id_8);
                fields_muh.Add("talimatturu_id_9", t.talimatturu_id_9);
                fields_muh.Add("pb", t.pb);
                //acekaResult = CrudRepository<stokkart_fiyat_talimat>.Update(t, "stokkart_fiyat_talimat", new string[] { "stokkart_id", "carikart_id", "talimatturu_id_1", "talimatturu_id_2", "talimatturu_id_3", "talimatturu_id_4", "talimatturu_id_5", "talimatturu_id_6", "talimatturu_id_7", "talimatturu_id_8", "talimatturu_id_9", "pb" });
                acekaResult = CrudRepository.Delete("stokkart_fiyat_talimat", new string[] { "stokkart_id", "carikart_id", "talimatturu_id_1", "talimatturu_id_2", "talimatturu_id_3", "talimatturu_id_4", "talimatturu_id_5", "talimatturu_id_6", "talimatturu_id_7", "talimatturu_id_8", "talimatturu_id_9", "pb" }, fields_muh);
                return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
            }
            return null;
        }

        #region Talimat Turu Tanimlama Metodları

        /// <summary>
        /// Talimat Türleri Listesi için Bütün Talimat Türlerini Listeler
        /// </summary>
        [HttpGet]
        [Route("api/talimatlar")]
        public HttpResponseMessage TalimatTurleriListesi()
        {
            parametreRepository = new ParametreRepository();
            var talimatlar = parametreRepository.TalimatListesiTanim();
            if (talimatlar != null && talimatlar.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, talimatlar.Select(talimat => new
                {
                    talimat.talimatturu_id,
                    talimat.statu,
                    talimat.kod,
                    talimat.varsayilan,
                    talimat.tanim,
                    talimat.cari_unvan,
                    talimat.tanim_dil1,
                    talimat.tanim_dil2,
                    talimat.tanim_dil3,
                    talimat.tanim_dil4,
                    talimat.tanim_dil5,
                    talimat.sira,
                    talimat.renk_rgb,
                    talimat.kesim,
                    talimat.dikim,
                    talimat.parca,
                    talimat.model,
                    talimat.stokkart_tipi_id,
                    talimat.onayoto,
                    talimat.parcamodel_giris,
                    talimat.parcamodel_cikis,
                    talimat.model_zorunlu,
                    talimat.varsayilan_fasoncu,
                    talimat.kdv_tevkifat,
                    stokkarttipleri = talimat.stokkart_tipleri.Select(s =>
                      new
                      {
                          s.stokkarttipi,
                          s.tanim,
                      }),

                }).OrderBy(x => x.sira));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }


        /// <summary>
        /// Sipariş ve Sıra ID'ye göre talimat döndürme
        /// </summary>
        /// <param name="siparisID"></param>
        /// <param name="siraID"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl ="api/talimatget/")]
        [Route("api/talimatget")]
        public IHttpActionResult TalimatTuruGet(long siparisID, byte siraID)
        {
            parametreRepository = new ParametreRepository();
            mTalimatList = parametreRepository.TalimatListesiTanimWithID(siparisID, siraID);
            mTalimatTanim = new talimattanim();
            if (mTalimatList != null && mTalimatList.Count>0)
            {
                var talimatList = mTalimatList.Select(x => new {
                    x.cari_unvan,
                    x.degistiren_carikart_id,
                    x.degistiren_tarih,
                    x.dikim,
                    x.kayit_silindi,
                    x.kdv_tevkifat,
                    x.kesim,
                    x.kod,
                    x.model,
                    x.model_zorunlu,
                    x.onayoto,
                    x.parca,
                    x.parcamodel_cikis,
                    x.parcamodel_giris,
                    x.renk_rgb,
                    x.sira,
                    x.statu,
                    x.stokkart_tipi_id,
                    x.stokkart_tipleri,
                    x.talimatturu_id,
                    x.tanim,
                    x.tanim_dil1,
                    x.tanim_dil2,
                    x.tanim_dil3,
                    x.tanim_dil4,
                    x.tanim_dil5,
                    x.varsayilan,
                    x.varsayilan_fasoncu,
                }).ToList();

                return Ok(talimatList);
            }


            return BadRequest();
        }
        /// <summary>
        /// Talimat Türlerin Tanım Post Metodu
        /// </summary>
        [HttpPost]
        [Route("api/talimatlar")]
        public IHttpActionResult TalimatTurleriTanim(talimattanim talimat)
        {
            AcekaResult acekaResult = null;
            if (talimat == null)
                return BadRequest();

            talimattanim talimatlar = new talimattanim
            {
                degistiren_carikart_id = Tools.PersonelId,
                degistiren_tarih = DateTime.Now,
                kayit_silindi = false,
                statu = true,
                kod = talimat.kod,
                varsayilan = talimat.varsayilan,
                tanim = talimat.tanim,
                sira = talimat.sira,
                renk_rgb = talimat.renk_rgb,
                kesim = talimat.kesim,
                dikim = talimat.dikim,
                parca = talimat.parca,
                model = talimat.model,
                //onayoto = true,
                parcamodel_giris = talimat.parcamodel_giris,
                parcamodel_cikis = talimat.parcamodel_cikis,
                model_zorunlu = talimat.model_zorunlu,
                varsayilan_fasoncu = talimat.varsayilan_fasoncu,
                kdv_tevkifat = talimat.kdv_tevkifat
            };

            acekaResult = CrudRepository<talimattanim>.Insert(talimatlar, "talimat", new string[] { "talimatturu_id", "cari_unvan", "onayoto", "storkart_tipi_id", "storkart_tipleri" });

            if (acekaResult == null || acekaResult.ErrorInfo != null)
                return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));

            return Ok(acekaResult);
        }

        /// <summary>
        /// talimat türü tanımlarının update metodu
        /// </summary>
        [HttpPut]
        [Route("api/talimatlar")]
        public HttpResponseMessage talimatturleriUpdate(talimattanim talimat)
        {
            AcekaResult acekaResult = null;
            if (talimat != null)
            {
                talimat.degistiren_carikart_id = Tools.PersonelId;//buraya sistemi açan kullanıcı bilgisi gelecek
                talimat.degistiren_tarih = DateTime.Now;  // Kayıtın Eklendiği Tarih Bilgisi
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("talimatturu_id", talimat.talimatturu_id);
                fields.Add("degistiren_carikart_id", talimat.degistiren_carikart_id);
                fields.Add("degistiren_tarih", talimat.degistiren_tarih);
                fields.Add("statu", talimat.statu);
                fields.Add("kod", talimat.kod);
                fields.Add("varsayilan", talimat.varsayilan);
                fields.Add("tanim", talimat.tanim);
                fields.Add("tanim_dil1", talimat.tanim_dil1);
                fields.Add("tanim_dil2", talimat.tanim_dil2);
                fields.Add("tanim_dil3", talimat.tanim_dil3);
                fields.Add("tanim_dil4", talimat.tanim_dil4);
                fields.Add("tanim_dil5", talimat.tanim_dil5);
                fields.Add("sira", talimat.sira);
                fields.Add("renk_rgb", talimat.renk_rgb);
                fields.Add("kesim", talimat.kesim);
                fields.Add("dikim", talimat.dikim);
                fields.Add("parca", talimat.parca);
                fields.Add("model", talimat.model);
                fields.Add("stokkart_tipi_id", 21);
                fields.Add("onayoto", 1);
                fields.Add("parcamodel_giris", talimat.parcamodel_giris);
                fields.Add("parcamodel_cikis", talimat.parcamodel_cikis);
                fields.Add("model_zorunlu", talimat.model_zorunlu);
                fields.Add("varsayilan_fasoncu", talimat.varsayilan_fasoncu);
                fields.Add("kdv_tevkifat", talimat.kdv_tevkifat);

                acekaResult = CrudRepository.Update("talimat", new string[] { "talimatturu_id" }, fields);

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful - talimatturu_id = " + acekaResult.RetVal.ToString() });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// talimat türü tanımlarının delete metodu
        /// </summary>
        [HttpDelete]
        [Route("api/talimatlar")]
        public HttpResponseMessage talimatturleriDelete(talimattanim talimat)
        {
            AcekaResult acekaResult = null;
            if (talimat != null)
            {
                talimat.degistiren_carikart_id = Tools.PersonelId;//buraya sistemi açan kullanıcı bilgisi gelecek
                talimat.degistiren_tarih = DateTime.Now; // Kayıtın Silindiği Tarih Bilgisi

                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("talimatturu_id", talimat.talimatturu_id);
                fields.Add("degistiren_carikart_id", talimat.degistiren_carikart_id);
                fields.Add("degistiren_tarih", talimat.degistiren_tarih);
                fields.Add("statu", 0);
                fields.Add("kayit_silindi", 1);

                acekaResult = CrudRepository.Update("talimat", new string[] { "talimatturu_id" }, fields);

                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful - talimatturu_id = " + acekaResult.RetVal.ToString() });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Talimat Turu Tanimlama ekranı için Fasoncu Get metodu
        /// </summary>
        [HttpGet]
        [Route("api/talimat/fasoncular")]
        public HttpResponseMessage TalimatTuruTanimlama_Fasoncular()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            var fasoncular = genelayarlarRepository.TalimatFasoncuListesi();
            if (fasoncular != null && fasoncular.Count > 0)
            {
                var f = fasoncular.Select(s => new
                {
                    s.carikart_id,
                    s.cari_unvan
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, f);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Talimat Turu Tanimlama ekranı için Kdv Tevkifat Get metodu
        /// </summary>
        [HttpGet]
        [Route("api/talimat/kdv-tevkifat")]
        public HttpResponseMessage TalimatTuruTanimlama_KdvTevkifat()
        {
            genelayarlarRepository = new GenelAyarlarRepository();
            var kdvler = genelayarlarRepository.TalimatKdvTevkifatListesi();
            if (kdvler != null && kdvler.Count > 0)
            {
                var t = kdvler.Select(k => new
                {
                    k.kod,
                    k.oran
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, t);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }
        #endregion
    }
}