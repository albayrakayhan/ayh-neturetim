using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.App_Start.Filters;
using aceka.web_api.Models;
using aceka.web_api.Models.SiparisModel;
using aceka.web_api.Models.StokkartModel;
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
    /// Sipariş Modülü
    /// </summary>

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SiparisController : ApiController
    {
        #region Değişkenler
        private SiparisRepository siparisRepository = null;
        private StokkartRepository stokkartRepository = null;
        private ParametreRepository parametreRepository = null;
        private siparis_notlar siparis_notlar = null;
        // private siparis_ozel siparis_ozel = null;
        private string errorMessage = "";

        #endregion

        /// <summary>
        /// Sipariş -> Sipariş NO Listesi Auto complate. AA
        /// </summary>
        /// <returns>
        /// [
        /// {
        ///   "siparis_id": 2,
        ///   "siparis_no": "2"
        /// }
        /// ]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-no")]
        [Route("api/siparis/siparis-no")]
        public HttpResponseMessage SiparisNoListesi(string siparis_no = "")
        {
            siparisRepository = new SiparisRepository();
            var siparisler = siparisRepository.SiparisNoAutoComplate(siparis_no);
            if (siparisler != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, siparisler.Select(spr => new
                {
                    spr.siparis_id,
                    spr.siparis_no
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// Sipariş Arama Metodu
        /// </summary>
        /// <param name="siparis_no"></param>
        /// <param name="musteri_carikart_id"></param>
        /// <param name="sezon_id"></param>
        /// <param name="modelno"></param>
        /// <param name="modeladi"></param>
        /// <param name="baslangic_tarihi">Format : DD.MM.YYYY</param>
        /// <param name="bitis_tarihi">Format : DD.MM.YYYY</param>
        /// <returns>
        ///[
        ///    {
        ///    siparis_id: 1,
        ///    siparis_no: "123",
        ///    siparis_tarihi: "2017-04-06T00:00:00",
        ///    stok_kodu: "Albayrak",
        ///    stok_adi: "selam",
        ///    musteri_carikart: {
        ///    musteri_carikart_id: 100120000015,
        ///            cari_unvan: "Yeni Cari Kerem"
        ///    },
        ///    stokyeri_carikart: {
        ///    stokyeri_carikart_id: 100120000015,
        ///    cari_unvan: "Yeni Cari Kerem"
        ///    },
        ///    siparisturu: {
        ///    siparisturu_id: 1,
        ///    siparisturu_tanim: "Genel-Üretim"
        ///    },
        ///    zorlukgrubu: {
        ///    zorlukgrubu_id: 1,
        ///    tanim: "Genel"
        ///    },
        ///    siparis_ozel: [
        ///        {
        ///        sezon_id: 1,
        ///        sira_id: 1,
        ///        tahmini_uretim_tarihi: null,
        ///        tahmini_dikim_tarihi: null,
        ///        isteme_tarihi: "2017-04-15T00:00:00",
        ///        stokkart_id: 1,
        ///        stok_adi: "selam",
        ///        stok_kodu: "Albayrak"
        ///        },
        ///        {
        ///        sezon_id: 2,
        ///        sira_id: 2,
        ///        tahmini_uretim_tarihi: null,
        ///        tahmini_dikim_tarihi: null,
        ///        isteme_tarihi: "2017-04-15T00:00:00",
        ///        stokkart_id: 2,
        ///        stok_adi: "Stok Adı Yok",
        ///        stok_kodu: "0130 X"
        ///        }
        ///    ]
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/arama")]
        [Route("api/siparis/arama")]
        //public HttpResponseMessage SiparisAra(string siparis_no = "", long musteri_carikart_id = 0, byte sezon_id = 0, long stokkart_id = 0, string baslangic_tarihi = "", string bitis_tarihi = "")
        public HttpResponseMessage SiparisAra(string siparis_no = "", long musteri_carikart_id = 0, byte sezon_id = 0, string modelno = "", string modeladi = "", string baslangic_tarihi = "", string bitis_tarihi = "")
        {
            siparisRepository = new SiparisRepository();

            var siparisler = siparisRepository.Bul(siparis_no, musteri_carikart_id, sezon_id, modelno, modeladi, baslangic_tarihi, bitis_tarihi);
            if (siparisler != null && siparisler.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, siparisler.Select(s => new
                {
                    s.siparis_id,
                    s.siparis_no,
                    s.siparis_tarihi,
                    stok_kodu = s.stokkart.stok_kodu,
                    stok_adi = s.stokkart.stok_adi,
                    musteri_carikart = new
                    {
                        s.musteri_carikart_id,
                        s.musteri_carikart.cari_unvan
                    },
                    stokyeri_carikart = new
                    {
                        s.stokyeri_carikart_id,
                        s.stokyeri_carikart.cari_unvan
                    },
                    siparisturu = new
                    {
                        s.siparisturu_id,
                        s.parametre_siparisturu.siparisturu_tanim
                    },
                    zorlukgrubu = new
                    {
                        s.zorlukgrubu_id,
                        s.parametre_zorlukgrubu.tanim
                    },
                    siparis_ozel =
                        s.siparis_ozel.Select(so => new
                        {
                            so.sezon_id,
                            so.sira_id,
                            so.tahmini_uretim_tarihi,
                            so.tahmini_dikim_tarihi,
                            so.ref_siparis_no,
                            so.ref_siparis_no2,
                            so.ref_sistem_name,
                            so.ref_link_status,
                            so.isteme_tarihi,
                            so.stokkart_id,
                            so.stokkart.stok_adi,
                            so.stokkart.stok_kodu
                        }).ToList()

                })

                );
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "Aradığınız Kiritere Uygun Bir Kayıt Bulunamadı.'" });
            }
        }

        /// <summary>
        /// Sipariş Özel tablosundaki siparişleri getirir. 
        /// Siparişe ait modelkart lardan kopyalanacak kayıtların kontrolü için tutulan kayıtların listesini verir. 
        /// sira_id  = 0 olan kayıtlar genel içerik için gerekli olduğundan sorguda hariç tutulur.
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-ozel")]
        [Route("api/siparis/siparis-ozel/{siparis_id}")]
        public HttpResponseMessage SiparisOzelListesi(long siparis_id)
        {
            siparisRepository = new SiparisRepository();
            var siparisOzelListe = siparisRepository.SiparisOzelListesiniGetir(siparis_id, ref errorMessage);
            if (siparisOzelListe != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, siparisOzelListe.Select(spr => new
                {
                    spr.siparis_id,
                    spr.sira_id,
                    spr.stokkart_id,
                    stok_kodu = spr.stokkart != null ? spr.stokkart.stok_kodu : ""
                }));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        #region Sipariş Modülü Üst Kısmı
        /// <summary>
        /// Siparis Get Metodu (Sipariş detay)
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns>
        /// {
        ///   "siparis_id": 1,
        ///   "siparis_no": "123",
        ///   "siparis_tarihi": "2017-04-06T00:00:00",
        ///   "stokkart_id": 2,
        ///   "musteri_carikart_id": 100120000015,
        ///   "stokyeri_carikart_id": 100120000015,
        ///   "siparisturu_id": 1,
        ///   "zorlukgrubu_id": 1,
        ///   "musterifazla": 2,
        ///   "uretimyeri_id": 1,
        ///   "mense_uretimyeri_id": 1,
        ///   "siparis_not": "Al gülüm ver gülüm",
        ///   "pb": 0,
        ///   "statu": 1,
        ///   "stok_kodu": "0130 X",
        ///   "stok_adi": "Stok Adı Yok",
        /// siparis_onay: {
        ///     genel_onay: false,
        ///     dikim_onay: false,
        ///     malzeme_onay: false,
        ///     uretim_onay: false,
        ///     yukleme_onay: false
        /// },
        ///   "siparis_ozel": {
        ///     "isteme_tarihi": "2017-04-15T00:00:00",
        ///     "tahmini_uretim_tarihi": null,
        ///     "tahmini_dikim_tarihi": null,
        ///     "sezon_id": 2,
        ///     "sira_id": 0,
        ///     "stokkart_id": 2,
        ///     "ref_link_status": null,
        ///     "ref_siparis_no": "",
        ///     "ref_siparis_no2": "",
        ///     "ref_sistem_name": "",
        ///     "bayi_carikart_id": 0
        ///   }
        /// }
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis")]
        [Route("api/siparis/{siparis_id}")]
        public HttpResponseMessage Get(long siparis_id)
        {
            if (siparis_id > 0)
            {
                siparisRepository = new SiparisRepository();

                var _siparis = siparisRepository.Getir(siparis_id, ref errorMessage);
                if (_siparis != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        _siparis.siparis_id,
                        _siparis.siparis_no,
                        _siparis.siparis_tarihi,
                        _siparis.musteri_carikart_id,
                        _siparis.stokyeri_carikart_id,
                        _siparis.siparisturu_id,
                        _siparis.zorlukgrubu_id,
                        _siparis.musterifazla,
                        _siparis.uretimyeri_id,
                        _siparis.mense_uretimyeri_id,
                        _siparis.siparis_not,
                        _siparis.pb,
                        statu = _siparis.statu == 1 ? true : false,
                        _siparis.stokkart.stok_kodu,
                        _siparis.stokkart.stok_adi,
                        _siparis.stokkart.stokkart_id,
                        siparis_onay = _siparis.siparisonay != null ? new
                        {
                            _siparis.siparisonay.genel_onay,
                            _siparis.siparisonay.dikim_onay,
                            _siparis.siparisonay.malzeme_onay,
                            _siparis.siparisonay.uretim_onay,
                            _siparis.siparisonay.yukleme_onay,
                        } : null,


                        siparis_ozel = _siparis.siparisozel != null ? new
                        {
                            _siparis.siparisozel.isteme_tarihi,
                            _siparis.siparisozel.tahmini_uretim_tarihi,
                            _siparis.siparisozel.tahmini_dikim_tarihi,
                            _siparis.siparisozel.sezon_id,
                            _siparis.siparisozel.sira_id,
                            _siparis.siparisozel.stokkart_id,
                            _siparis.siparisozel.ref_link_status,
                            _siparis.siparisozel.ref_siparis_no,
                            _siparis.siparisozel.ref_siparis_no2,
                            _siparis.siparisozel.ref_sistem_name,
                            _siparis.siparisozel.bayi_carikart_id,
                        } : null

                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                        return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = errorMessage });
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "Geçersiz Kayıt Girildi!" });
            }

        }


        /// <summary>
        /// Siparis Post Metodu 
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis")]
        [Route("api/siparis/{stokkart_id}")]
        public HttpResponseMessage siparisOlustur(long stokkart_id)
        {
            Siparis siparis = new Siparis();
            stokkartRepository = new StokkartRepository();
            var stokKart = stokkartRepository.Getir(stokkart_id, 0);


            //AcekaResult acekaResult = null;
            //siparisRepository = new SiparisRepository();

            //if (siparis != null)
            //{
            //    siparis.degistiren_tarih = DateTime.Now;
            //    siparis.degistiren_carikart_id = Tools.PersonelId;
            //    if (siparis.siparis_no != null)
            //    {
            //        siparis.siparis_no = Tools.Substring(siparis.siparis_no, 10);
            //    }


            //    acekaResult = CrudRepository<Siparis>.Insert(siparis, "siparis", new string[] { "siparis_id" });
            //    if (acekaResult != null && acekaResult.ErrorInfo == null)
            //    {
            //        if (siparis.siparis_ozel == null)
            //            siparis.siparis_ozel = new Siparis_Ozel();
            //        /*
            //         sira_id = İlk kayıt olması ve 
            //                    Sipariş e ait diğer detayların tutulacağı kayıt olması sebebi ile sira_id = 0 dır.
            //         */
            //        //var siparisler = stokkartRepository.StokkartKodu_Bul("740003 01");
            //        siparis.siparis_ozel.siparis_id = Convert.ToInt64(acekaResult.RetVal);
            //        siparis.siparis_ozel.sira_id = 0;
            //        siparis.siparis_ozel.degistiren_carikart_id = Tools.PersonelId;
            //        siparis.siparis_ozel.degistiren_tarih = DateTime.Now;

            //        var retSiparis_Ozel = CrudRepository<Siparis_Ozel>.Insert(siparis.siparis_ozel, "siparis_ozel", null);

            //        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = acekaResult.RetVal.ToString() });
            //    }
            //    else
            //    {
            //        return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
            //    }
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
            //}
            return null;
        }

        /// <summary>
        /// Siparis Insert Metodu
        /// sira_id = İlk kayıt olması ve 
        /// Sipariş e ait diğer detayların tutulacağı kayıt olması sebebi ile sira_id = 0 dır.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis")]
        [Route("api/siparis")]
        public IHttpActionResult Post(Siparis siparis)
        {
            AcekaResult acekaResult = null;
            siparisRepository = new SiparisRepository();

            siparis.degistiren_tarih = DateTime.Now;
            siparis.degistiren_carikart_id = Tools.PersonelId;
            siparis.siparis_no = string.Empty;

            acekaResult = CrudRepository<Siparis>.Insert(siparis, "siparis", new string[] { "siparis_id" });

            if (acekaResult != null && acekaResult.ErrorInfo != null)
            {
                return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
            }

            string siparisNoHarf = siparisRepository.SiparisNoHarfGetir(acekaResult.RetVal.acekaToLong()).Trim();
            string siparisNo = Tools.CalculateSiparisNo(acekaResult.RetVal.acekaToLong(), siparisNoHarf);
            var result = siparisRepository.UpdateSiparisNo(acekaResult.RetVal.acekaToLong(), siparisNo);

            if (result < 1)
            {
                return InternalServerError(new Exception("Siparis no güncellenemedi"));
            }

            //siparisRepository.SiparisDataOsturStokkartan(acekaResult.RetVal.acekaToLong(), siparis.siparis_ozel.stokkart_id.Value, Tools.PersonelId);

            if (siparis.siparis_ozel == null)
                siparis.siparis_ozel = new Siparis_Ozel();

            /*
             sira_id = İlk kayıt olması ve 
                        Sipariş e ait diğer detayların tutulacağı kayıt olması sebebi ile sira_id = 0 dır.
             */
            //var siparisler = stokkartRepository.StokkartKodu_Bul("740003 01");

            siparis.siparis_ozel.siparis_id = Convert.ToInt64(acekaResult.RetVal);
            siparis.siparis_ozel.sira_id = 0;
            siparis.siparis_ozel.degistiren_carikart_id = Tools.PersonelId;
            siparis.siparis_ozel.degistiren_tarih = DateTime.Now;

            var retSiparis_Ozel = CrudRepository<Siparis_Ozel>.Insert(siparis.siparis_ozel, "siparis_ozel", null);
            return Ok(new Models.AnonymousModels.Successful { message = "successful", ret_val = acekaResult.RetVal.ToString() });
        }

        /// <summary>
        /// Siparis Put Metodu
        /// </summary>
        /// <param name="siparis"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis")]
        [Route("api/siparis")]
        public IHttpActionResult Put(Siparis siparis)
        {
            if (siparis == null || siparis.siparis_id < 0)
                return BadRequest("Update İşlemi Yapılamadı. Sipariş No Bulunamadı!!");


            siparisRepository = new SiparisRepository();
            var siparisDetay = siparisRepository.Getir(siparis.siparis_id, ref errorMessage);

            if (siparisDetay == null)
                return BadRequest(errorMessage);

            siparisDetay.degistiren_carikart_id = Tools.PersonelId;
            siparisDetay.degistiren_tarih = DateTime.Now;
            siparisDetay.statu = siparis.statu.Value;
            //siparisDetay.siparis_no = siparis.siparis_no;
            siparisDetay.sirket_id = siparis.sirket_id;
            siparisDetay.musteri_carikart_id = siparis.musteri_carikart_id.Value;
            siparisDetay.stokyeri_carikart_id = siparis.stokyeri_carikart_id;
            siparisDetay.siparis_tarihi = siparis.siparis_tarihi;
            siparisDetay.siparisturu_id = siparis.siparisturu_id;
            siparisDetay.zorlukgrubu_id = siparis.zorlukgrubu_id;
            siparisDetay.musterifazla = siparis.musterifazla;
            siparisDetay.siparis_not = siparis.siparis_not;
            siparisDetay.uretimyeri_id = siparis.uretimyeri_id;
            siparisDetay.mense_uretimyeri_id = siparis.mense_uretimyeri_id;
            siparisDetay.pb = siparis.pb;
            siparisDetay.statu = siparis.statu.Value;

            var acekaResult = CrudRepository<siparis>.Update(siparisDetay, "siparis_id", new string[] { "siparis_no" });

            if (acekaResult.ErrorInfo != null)
                return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));

            #region Sipariş Özel
            var siparisOzel = siparisRepository.SiparisOzelGetir(siparis.siparis_id, 0, ref errorMessage);
            if (siparisOzel != null)
            {
                #region Update
                if (siparis.siparis_ozel != null)
                {
                    siparisOzel.degistiren_carikart_id = Tools.PersonelId;
                    siparisOzel.degistiren_tarih = DateTime.Now;
                    siparisOzel.stokkart_id = siparis.siparis_ozel.stokkart_id;
                    siparisOzel.bayi_carikart_id = siparis.siparis_ozel.bayi_carikart_id;
                    siparisOzel.isteme_tarihi = siparis.siparis_ozel.isteme_tarihi; //siparis.siparis_ozel.isteme_tarihi;
                    siparisOzel.tahmini_uretim_tarihi = siparis.siparis_ozel.tahmini_uretim_tarihi;
                    siparisOzel.sezon_id = siparis.siparis_ozel.sezon_id;
                    siparisOzel.stokkart_id = siparis.siparis_ozel.stokkart_id;
                    //siparisOzel.tahmini_dikim_tarihi = siparis.siparis_ozel.tahmini_dikim_tarihi;
                    siparisOzel.ref_siparis_no = siparis.siparis_ozel.ref_siparis_no;
                    siparisOzel.ref_siparis_no2 = siparis.siparis_ozel.ref_siparis_no2;
                    siparisOzel.ref_sistem_name = siparis.siparis_ozel.ref_sistem_name;
                    siparisOzel.ref_link_status = siparis.siparis_ozel.ref_link_status;

                    var siparisOzelResult = CrudRepository<siparis_ozel>.Update(siparisOzel, new string[] { "siparis_id", "sira_id" }, null);

                    if (siparisOzelResult.ErrorInfo != null)
                        return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));

                }
                #endregion

            }
            else
            {
                #region Insert
                if (siparis.siparis_ozel == null)
                    siparis.siparis_ozel = new Siparis_Ozel();

                siparis.siparis_ozel.siparis_id = siparis.siparis_id;
                siparis.sirket_id = 0;
                siparis.siparis_ozel.degistiren_carikart_id = Tools.PersonelId;
                siparis.siparis_ozel.degistiren_tarih = DateTime.Now;
                var siparisOzelResult = CrudRepository<Siparis_Ozel>.Insert(siparis.siparis_ozel, "siparis_ozel", null);

                if (siparisOzelResult.ErrorInfo != null)
                    return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                #endregion
            }
            #endregion

            return Ok(new Models.AnonymousModels.Successful { message = "successful" });
        }

        /// <summary>
        /// Siparis Delete Metodu
        /// Not: Siparis nesnesi içerisindeki sadce "siparis_id" bilgisini göndermek yeterlidir.
        /// </summary>
        /// <param name="siparis"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/siparis")]
        [Route("api/siparis")]
        public HttpResponseMessage Delete(Siparis siparis)
        {
            if (siparis != null && siparis.siparis_id > 0)
            {
                AcekaResult acekaResult = null;

                siparisRepository = new SiparisRepository();
                var siparisDetay = siparisRepository.Getir(siparis.siparis_id, ref errorMessage);

                if (siparisDetay != null)
                {
                    siparisDetay.degistiren_carikart_id = Tools.PersonelId;
                    siparisDetay.degistiren_tarih = DateTime.Now;
                    siparisDetay.kayit_silindi = true;

                    acekaResult = CrudRepository<infrastructure.Models.siparis>.Update(siparisDetay, "siparis_id", null);
                    if (acekaResult.ErrorInfo != null)
                        errorMessage = acekaResult.ErrorInfo.Message;
                }
                else
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "No Record!";
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(errorMessage.Contains("No Record!") ? HttpStatusCode.NotFound : HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #region Tablar
        #region Genel Sekmesi

        /// <summary>
        ///  Sipariş - > Tab - > Genel -> Adetler GET Metodu
        ///  Sabit Kolon sayısı = 3, 3 ten sonraki gelecek kolon sayısı pivot sorguya göre dinamik gelecek
        ///  sabit kolonlar sırasıyla;
        ///              beden_id,
        ///              beden,
        ///              toplam
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-adetler/{siparis_id}")]
        public IHttpActionResult GenelAdetler(long siparis_id)
        {
            siparisRepository = new SiparisRepository();
            var adetler = siparisRepository.SiparisAdetFiyatGetir(siparis_id, CustomEnums.SiparisDetayPivotType.adet);

            if (adetler == null || adetler.Rows.Count < 1)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            /*Sabit Kolon sayısı = 3, 3 ten sonraki gelecek kolon sayısı pivot sorguya göre dinamik gelecek
            sabit kolonlar sırasıyla;
                beden_id,
                beden,
                toplam
            */
            var dinamikKolonSayisi = adetler.Columns.Count - 3;
            //Kolon Başlıkları Ekleniyor Start
            List<SiparisPivotColumnModel> columns = new List<SiparisPivotColumnModel>();
            columns.Add(new SiparisPivotColumnModel { id = 1, title = "Beden" });
            columns.Add(new SiparisPivotColumnModel { id = 2, title = "Toplam" });
            for (int i = 0; i < dinamikKolonSayisi; i++)
            {
                columns.Add(new SiparisPivotColumnModel { id = (columns.Count + 1), title = adetler.Columns[3 + (i)].ColumnName });
            }
            //Kolon Başlıkları Ekleniyor End

            //Kayıtlar oluşturuluyor Start
            List<SiparisPivotModel> siparisPivotModelList = new List<SiparisPivotModel>();
            SiparisPivotModel siparisPivotModel = null;
            for (int i = 0; i < adetler.Rows.Count; i++)
            {
                siparisPivotModel = new SiparisPivotModel();
                siparisPivotModel.siparis_id = siparis_id;
                siparisPivotModel.beden_id = adetler.Rows[i]["beden_id"].acekaToByte();
                siparisPivotModel.beden = adetler.Rows[i]["beden"].acekaToString();
                siparisPivotModel.toplam = adetler.Rows[i]["toplam"].acekaToInt();

                siparisPivotModel.siparisPivotArray = new List<SiparisPivotArray>();
                for (int x = 0; x < dinamikKolonSayisi; x++)
                {
                    siparisPivotModel.siparisPivotArray.Add(
                        new SiparisPivotArray
                        {
                            stokkart_id = columns[2 + x].title.Replace("\r", ";").Split(';')[0].acekaToLong(),
                            stok_kodu = columns[2 + x].title.Replace("\r", ";").Split(';')[1],
                            adet = adetler.Rows[i][3 + x].acekaToIntWithNullable()
                        }
                        );
                }

                siparisPivotModelList.Add(siparisPivotModel);
                siparisPivotModel = null;
            }
            //Kayıtlar oluşturuluyor End

            return Ok(siparisPivotModelList);
        }

        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-adetler")]
        public IHttpActionResult GenelAdetGuncelle(SiparisPivotModel model)
        {
            siparisRepository = new SiparisRepository();
            AcekaResult result = new AcekaResult();
            foreach (var item in model.siparisPivotArray)
            {
                var siprisDetay = siparisRepository.SiparisDetayGetir(model.siparis_id.Value, item.stokkart_id, model.beden_id);
                siprisDetay.adet = item.adet.Value;
                siprisDetay.degistiren_carikart_id = Tools.PersonelId;
                siprisDetay.degistiren_tarih = DateTime.Now;

                result = CrudRepository<siparis_detay>.Update(siprisDetay,
                new string[] { "siparis_id", "stokkart_id", "beden_id" },
                new string[] { "siparis_id", "stokkart_id", "beden_id", "birimfiyat" });

                if (result.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(result.ErrorInfo.Message));
                }
            }

            return Ok(result);
        }

        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-adetler/list")]
        public IHttpActionResult GenelAdetPUT(List<SiparisPivotModel> models)
        {
            siparisRepository = new SiparisRepository();

            try
            {
                models.ForEach(x =>
                {
                    GenelAdetItemPUT(x);

                });
            }
            catch (Exception e)
            {
                InternalServerError(e);
            }

            return Ok();
        }

        private void GenelAdetItemPUT(SiparisPivotModel model)
        {
            AcekaResult result = new AcekaResult();

            foreach (var item in model.siparisPivotArray)
            {
                var siprisDetay = siparisRepository.SiparisDetayGetir(model.siparis_id.Value, item.stokkart_id, model.beden_id);
                siprisDetay.adet = item.adet.Value;
                siprisDetay.degistiren_carikart_id = Tools.PersonelId;
                siprisDetay.degistiren_tarih = DateTime.Now;

                result = CrudRepository<siparis_detay>.Update(siprisDetay,
                new string[] { "siparis_id", "stokkart_id", "beden_id" },
                new string[] { "siparis_id", "stokkart_id", "beden_id", "birimfiyat" });

                if (result.ErrorInfo != null)
                {
                    new Exception(result.ErrorInfo.Message);
                }
            }
        }

        /// <summary>
        /// Sipariş - > Tab - > Genel -> Fiyatlar GET Metodu
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-fiyatlar/{siparis_id}")]
        public IHttpActionResult GenelFiyatlar(long siparis_id)
        {
            siparisRepository = new SiparisRepository();
            var adetler = siparisRepository.SiparisAdetFiyatGetir(siparis_id, CustomEnums.SiparisDetayPivotType.birimfiyat);
            if (adetler == null || adetler.Rows.Count < 1)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }

            /*Sabit Kolon sayısı = 3, 3 ten sonraki gelecek kolon sayısı pivot sorguya göre dinamik gelecek
            sabit kolonlar sırasıyla;
                beden_id,
                beden,
                toplam
            */

            var dinamikKolonSayisi = adetler.Columns.Count - 3;

            //Kolon Başlıkları Ekleniyor
            List<SiparisPivotColumnModel> columns = new List<SiparisPivotColumnModel>();
            columns.Add(new SiparisPivotColumnModel { id = 1, title = "Beden" });
            columns.Add(new SiparisPivotColumnModel { id = 2, title = "Toplam" });

            for (int i = 0; i < dinamikKolonSayisi; i++)
            {
                columns.Add(new SiparisPivotColumnModel { id = (columns.Count + 1), title = adetler.Columns[3 + (i)].ColumnName });
            }

            //Kolon Başlıkları Ekleniyor

            //Kayıtlar oluşturuluyor
            List<SiparisPivotModel> siparisPivotModelList = new List<SiparisPivotModel>();
            SiparisPivotModel siparisPivotModel = null;
            for (int i = 0; i < adetler.Rows.Count; i++)
            {
                siparisPivotModel = new SiparisPivotModel();
                siparisPivotModel.siparis_id = siparis_id;
                siparisPivotModel.beden_id = adetler.Rows[i]["beden_id"].acekaToShort();
                siparisPivotModel.beden = adetler.Rows[i]["beden"].acekaToString();
                //siparisPivotModel.toplam = adetler.Rows[i]["toplam"].acekaToFloat();

                siparisPivotModel.siparisPivotArray = new List<SiparisPivotArray>();
                for (int x = 0; x < dinamikKolonSayisi; x++)
                {
                    siparisPivotModel.siparisPivotArray.Add(
                        new SiparisPivotArray
                        {
                            stokkart_id = columns[2 + x].title.Replace("\r", ";").Split(';')[0].acekaToLong(),
                            stok_kodu = columns[2 + x].title.Replace("\r", ";").Split(';')[1],
                            adet = adetler.Rows[i][3 + x].acekaToIntWithNullable(),
                            birimfiyat = adetler.Rows[i][3 + x].acekaToDecimal()
                        });
                }

                siparisPivotModelList.Add(siparisPivotModel);
                siparisPivotModel = null;
            }

            return Ok(siparisPivotModelList);
        }

        /// <summary>
        /// Sipariş - > Tab - > Genel -> Fiyatlar GET Metodu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-fiyatlar")]
        public IHttpActionResult GenelFiyatGuncelle(SiparisPivotModel model)
        {
            siparisRepository = new SiparisRepository();

            foreach (var item in model.siparisPivotArray)
            {
                var siprisDetay = siparisRepository.SiparisDetayGetir(model.siparis_id.Value, item.stokkart_id, model.beden_id);
                siprisDetay.birimfiyat = item.birimfiyat;
                siprisDetay.degistiren_carikart_id = Tools.PersonelId;
                siprisDetay.degistiren_tarih = DateTime.Now;

                var result = CrudRepository<siparis_detay>.Update(siprisDetay,
                    new string[] { "siparis_id", "stokkart_id", "beden_id" },
                    new string[] { "siparis_id", "stokkart_id", "beden_id", "adet" });

                if (result.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(result.ErrorInfo.Message));
                }
            }

            return Ok();
        }

        /// <summary>
        /// Sipariş - > Tab - > Genel -> Fiyatlar GET Metodu
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel-fiyatlar/list")]
        public IHttpActionResult GenelFiyatPUT(List<SiparisPivotModel> models)
        {
            siparisRepository = new SiparisRepository();
            try
            {
                models.ForEach(x =>
                {
                    GenelFiyatItemPUT(x);
                });
            }
            catch (Exception e)
            {
                InternalServerError(e);
            }

            return Ok();
        }

        private void GenelFiyatItemPUT(SiparisPivotModel model)
        {
            foreach (var item in model.siparisPivotArray)
            {
                var siprisDetay = siparisRepository.SiparisDetayGetir(model.siparis_id.Value, item.stokkart_id, model.beden_id);
                siprisDetay.birimfiyat = item.birimfiyat;
                siprisDetay.degistiren_carikart_id = Tools.PersonelId;
                siprisDetay.degistiren_tarih = DateTime.Now;

                var result = CrudRepository<siparis_detay>.Update(siprisDetay,
                    new string[] { "siparis_id", "stokkart_id", "beden_id" },
                    new string[] { "siparis_id", "stokkart_id", "beden_id", "adet" });

                if (result.ErrorInfo != null)
                {
                    new Exception(result.ErrorInfo.Message);
                }
            }
        }

        /// <summary>
        /// Sipariş - > Tab - > Genel -> Fiyatlar GET Metodu
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis/genel")]
        [Route("api/siparis/genel/list")]
        public IHttpActionResult GenelPOST(List<SiparisPivotModel> models)
        {
            siparisRepository = new SiparisRepository();
            try
            {
                models.ForEach(x =>
                {
                    GenelItemPOST(x);
                });
            }
            catch (Exception e)
            {
                InternalServerError(e);
            }

            return Ok();
        }

        private void GenelItemPOST(SiparisPivotModel model)
        {
            foreach (var item in model.siparisPivotArray)
            {
                siparis_detay siprisDetay = new siparis_detay
                {
                    adet = item.adet,
                    stokkart_id = item.stokkart_id,
                    beden_id = model.beden_id,
                    siparis_id = model.siparis_id.Value,
                    birimfiyat = item.birimfiyat,
                    degistiren_carikart_id = Tools.PersonelId,
                    degistiren_tarih = DateTime.Now
                };

                var result = CrudRepository<siparis_detay>.Insert(siprisDetay);

                if (result.ErrorInfo != null)
                {
                    new Exception(result.ErrorInfo.Message);
                }
            }
        }


        #endregion
        #region Talimatlar
        /// <summary>
        /// Siparis - > Talimatlar GET Metod (List).
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns>
        /// [
        ///  {
        ///    "siparis_id": 4,
        ///    "sira_id": 1,
        ///    "eski_sira_id": 1,
        ///    "talimatturu_id": 1,
        ///    "fasoncu_carikart_id": 100000001010,
        ///    "aciklama": "açıklama 1",
        ///    "talimat_adi": "KESİM",
        ///    "irstalimat": "Kesim",
        ///    "islem_sayisi": 2,
        ///    "cari_unvan": "DALLI TEKSTİL MAKİNA"
        ///  },
        ///  {
        ///    "siparis_id": 4,
        ///    "sira_id": 2,
        ///    "eski_sira_id": 2,
        ///    "talimatturu_id": 3,
        ///    "fasoncu_carikart_id": 100000001359,
        ///    "aciklama": "açıklama 2",
        ///    "talimat_adi": "DİKİM",
        ///    "irstalimat": "Dikim",
        ///    "islem_sayisi": 2,
        ///    "cari_unvan": "AJARA TEXTILE LTD."
        ///  }
        ///]
        /// </returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimatlar/{siparis_id}")]
        [HttpGet]
        public HttpResponseMessage TalimatlarGet(long siparis_id)
        {
            if (siparis_id > 0)
            {
                siparisRepository = new SiparisRepository();

                var talimatlar = siparisRepository.SiparisTalimatGetir(siparis_id, ref errorMessage);
                if (talimatlar != null && string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, talimatlar.Select(talimat => new
                    {
                        talimat.siparis_id,
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
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Siparis Talimatlar POST Metod     
        /// NOT 1 : sira_id gönderilmez ise sistem sira_id yi otomatik oluşturacak.
        /// NOT 2: Bir siparise ait birden fazla aynı talimatturu_id verilemez. Herkayıt için farklı talimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="siparisTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimatlar")]
        [HttpPost]
        public HttpResponseMessage TalimatlarPost(SiparisTalimat siparisTalimat)
        {
            AcekaResult acekaResult = null;

            if (siparisTalimat != null && siparisTalimat.siparis_id > 0)
            {
                siparisRepository = new SiparisRepository();

                short yeniSiraId = 0;

                //Sira_id elle değiştirildiğinde aynı stokkart_id ye ait var olan başka bir sira_id var mı diye kontrol ediyoruz
                if (siparisTalimat.sira_id > 0)
                {
                    var siraIDRetVal = siparisRepository.SiparisTalimatSiraID_Kontrol(siparisTalimat.siparis_id, siparisTalimat.sira_id, ref errorMessage);
                    if (siraIDRetVal > 0 && string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Modelkarta ait zaten bir 'sira_id' mevcut!" });
                    }
                    else
                    {
                        yeniSiraId = siparisTalimat.sira_id;

                    }
                }
                else
                {
                    var enBuyukSiraNo = siparisRepository.SiparisTalimatEnBuyukSiraNo(siparisTalimat.siparis_id, ref errorMessage);
                    yeniSiraId = Convert.ToInt16(enBuyukSiraNo + 1);
                }

                siparisTalimat.sira_id = yeniSiraId;
                siparisTalimat.degistiren_carikart_id = Tools.PersonelId;
                siparisTalimat.degistiren_tarih = DateTime.Now;

                acekaResult = CrudRepository<SiparisTalimat>.Insert(siparisTalimat, "siparis_talimat", new string[] { "eski_sira_id" });
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
                            acekaResult.ErrorInfo.Message = "Siparişe dea aynı talimat türü 2.kez tanımlanamaz!";
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
        /// Siparis Talimatlar POST Metod. Model Karttan Gelen Kayıtları eklemek için eklendi. AA     
        /// NOT 1 : sira_id gönderilmez ise sistem sira_id yi otomatik oluşturacak.
        /// NOT 2: Bir siparise ait birden fazla aynı talimatturu_id verilemez. Herkayıt için farklı talimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="siparisTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimat")]
        [HttpPost]
        public HttpResponseMessage TalimatlarPost(List<SiparisTalimat> siparisTalimat)
        {
            AcekaResult acekaResult = null;

            if (siparisTalimat != null)
            {
                for (int i = 0; i < siparisTalimat.Count; i++)
                {
                    siparisRepository = new SiparisRepository();

                    short yeniSiraId = 0;

                    //Sira_id elle değiştirildiğinde aynı stokkart_id ye ait var olan başka bir sira_id var mı diye kontrol ediyoruz
                    if (siparisTalimat[i].sira_id > 0)
                    {
                        var siraIDRetVal = siparisRepository.SiparisTalimatSiraID_Kontrol(siparisTalimat[i].siparis_id, siparisTalimat[i].sira_id, ref errorMessage);
                        if (siraIDRetVal > 0 && string.IsNullOrEmpty(errorMessage))
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Modelkarta ait zaten bir 'sira_id' mevcut!" });
                        }
                        else
                        {
                            yeniSiraId = siparisTalimat[i].sira_id;
                        }
                    }
                    else
                    {
                        var enBuyukSiraNo = siparisRepository.SiparisTalimatEnBuyukSiraNo(siparisTalimat[i].siparis_id, ref errorMessage);
                        yeniSiraId = Convert.ToInt16(enBuyukSiraNo + 1);
                    }
                    siparisTalimat[i].sira_id = yeniSiraId;
                    siparisTalimat[i].degistiren_carikart_id = Tools.PersonelId;
                    siparisTalimat[i].degistiren_tarih = DateTime.Now;
                    acekaResult = CrudRepository<SiparisTalimat>.Insert(siparisTalimat[i], "siparis_talimat", new string[] { "eski_sira_id" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process. Please check input areas!" });
            }
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
                        acekaResult.ErrorInfo.Message = "Siparişe ait aynı talimat türü 2.kez tanımlanamaz!";
                    }

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
        }


        /// <summary>
        /// Siparis Talimatlar PUT Liste Metod     
        /// Bir siparise ait birden fazla aynı talimatturu_id verilemez. Herkayıt için farklı talimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="siparisTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimat")]
        [HttpPut]
        public HttpResponseMessage TalimatlarPut(SiparisTalimat siparisTalimat)
        {
            AcekaResult acekaResult = null;
            if (siparisTalimat != null && siparisTalimat.siparis_id > 0 && siparisTalimat.eski_sira_id > 0)
            {
                siparisRepository = new SiparisRepository();
                siparis_talimat talimatDetay = siparisRepository.SiparisTalimatDetay(siparisTalimat.siparis_id, siparisTalimat.eski_sira_id);

                bool islemDevam = false;
                //Sira_id elle değiştirildiğinde aynı siparis_id ye ait var olan başka bir sira_id var mı diye kontrol ediyoruz
                if (siparisTalimat.sira_id != siparisTalimat.eski_sira_id)
                {
                    var siraIDRetval = siparisRepository.SiparisTalimatSiraID_Kontrol(siparisTalimat.siparis_id, siparisTalimat.sira_id, ref errorMessage);
                    if (siraIDRetval > 0 && string.IsNullOrEmpty(errorMessage))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Siparişe ait bir 'Sira Id' mevcut! Başka bir sira id giriniz." });
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
                        fields.Add("sira_id", siparisTalimat.sira_id);
                        fields.Add("siparis_id", siparisTalimat.siparis_id);
                        fields.Add("degistiren_carikart_id", siparisTalimat.degistiren_carikart_id);
                        fields.Add("degistiren_tarih", DateTime.Now);
                        fields.Add("talimatturu_id", siparisTalimat.talimatturu_id);
                        fields.Add("irstalimat", siparisTalimat.irstalimat);
                        fields.Add("islem_sayisi", siparisTalimat.islem_sayisi);
                        fields.Add("fasoncu_carikart_id", siparisTalimat.fasoncu_carikart_id);
                        fields.Add("aciklama", siparisTalimat.aciklama);


                        acekaResult = CrudRepository.Update("siparis_talimat", "siparis_id=" + siparisTalimat.siparis_id + " AND sira_id= " + siparisTalimat.eski_sira_id, fields, true);
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
                        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record Found!" });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "Not Found!" });
            }


        }

        /// <summary>
        /// Siparis Talimatlar PUT Liste Metod     
        /// Bir siparise ait birden fazla aynı talimatturu_id verilemez. Herkayıt için farklı talimattturu_id kullanılmalı!
        /// </summary>
        /// <param name="siparisTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimatlar")]
        [HttpPut]
        public HttpResponseMessage TalimatlarPut(List<SiparisTalimat> siparisTalimat)
        {
            AcekaResult acekaResult = null;
            if (siparisTalimat != null)
            {
                for (int i = 0; i < siparisTalimat.Count; i++)
                {
                    siparisRepository = new SiparisRepository();
                    short yeniSiraId = 0;
                    if (siparisTalimat[i].sira_id > 0)
                    {
                        var siraIDRetVal = siparisRepository.SiparisTalimatSiraID_Kontrol(siparisTalimat[i].siparis_id, siparisTalimat[i].sira_id, ref errorMessage);
                        // sira_id düzenlemesi 
                        //if (siraIDRetVal > 0 && string.IsNullOrEmpty(errorMessage))
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Siparis ait zaten bir 'sira_id' mevcut!" });
                        }
                        else
                        {
                            yeniSiraId = siparisTalimat[i].sira_id;
                        }
                    }
                    else
                    {
                        var enBuyukSiraNo = siparisRepository.SiparisTalimatEnBuyukSiraNo(siparisTalimat[i].siparis_id, ref errorMessage);
                        yeniSiraId = Convert.ToInt16(enBuyukSiraNo + 1);
                    }

                    siparisTalimat[i].sira_id = yeniSiraId;
                    siparisTalimat[i].degistiren_carikart_id = Tools.PersonelId;
                    siparisTalimat[i].degistiren_tarih = DateTime.Now;
                    acekaResult = CrudRepository<SiparisTalimat>.Update(siparisTalimat[i], "siparis_talimat", new string[] { "siparis_id", "sira_id" }, new string[]{"eski_sira_id"});
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process. Please check input areas!" });
            }
            if (acekaResult != null && acekaResult.ErrorInfo == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                if (acekaResult.ErrorInfo != null)
                {
                    if (acekaResult.ErrorInfo.Message.ToLower().Contains("duplicate"))
                    {
                        acekaResult.ErrorInfo.Message = "Siparişe dea aynı talimat türü 2.kez tanımlanamaz!";
                    }

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
                }
            }

        }

        /// <summary>
        /// Siparis Talimatlar DELETE Metod
        /// </summary>
        /// <param name="siparisTalimat"></param>
        /// <returns></returns>
        [CustAuthFilter(ApiUrl = "api/siparis/talimatlar")]
        [Route("api/siparis/talimatlar")]
        [HttpDelete]
        public HttpResponseMessage TalimatlarDelete(SiparisTalimat siparisTalimat)
        {
            AcekaResult acekaResult = null;
            if (siparisTalimat != null && siparisTalimat.siparis_id > 0 && siparisTalimat.sira_id > 0)
            {
                siparisRepository = new SiparisRepository();
                siparis_talimat talimatDetay = siparisRepository.SiparisTalimatDetay(siparisTalimat.siparis_id, siparisTalimat.sira_id);
                if (talimatDetay != null)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("sira_id", siparisTalimat.sira_id);
                    fields.Add("siparis_id", siparisTalimat.siparis_id);
                    acekaResult = CrudRepository.Delete("siparis_talimat", new string[] { "siparis_id", "sira_id" }, fields);

                    if (acekaResult != null && acekaResult.ErrorInfo == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
                    }
                    else
                    {
                        if (acekaResult.ErrorInfo != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "No Record!" });
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
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "Silinecek Kayıt Bulunamamıştır.!" });
            }

        }

        #endregion
        #region İlk Madde Kumaş, İlk Madde Aksesuar, İlk Madde İplik

        /// <summary>
        /// Sipariş - > Tab - > İlk madde modelleri (Kumaş, Aksesuar, İplik)
        /// </summary>
        /// <param name="siparis_id"></param>
        ///  <param name="stokkart_tipi_id">20-> Kumaş Kartı, 21-> Aksesuar Kartı, 22-> İplik Kartı</param>
        /// <returns>
        /// [
        ///  {
        ///    "siparis_id": 1,
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
        ///    "siparis_id": 32445,
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
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller/{siparis_id},{stokkart_tipi_id}")]
        public HttpResponseMessage SiparisModelListesiniGetir(long siparis_id, byte stokkart_tipi_id)
        {
            siparisRepository = new SiparisRepository();
            var modeler = siparisRepository.SiparisModelListesiniGetir(siparis_id, stokkart_tipi_id, ref errorMessage);
            if (modeler != null && string.IsNullOrEmpty(errorMessage))
            {
                return Request.CreateResponse(HttpStatusCode.OK, modeler.Select(model => new
                {
                    model.siparis_id,
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
        /// Sipariş - > Tab - > İlk madde modelleri pivot liste (Kumaş = 20, Aksesuar = 21, İplik = 22)
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <param name="stokkart_id"></param>
        /// <param name="stokkart_tipi_id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller-pivot/{siparis_id},{stokkart_id},{stokkart_tipi_id}")]
        public IHttpActionResult StokkartPivotModelListesiniGetir(long siparis_id, long stokkart_id, byte stokkart_tipi_id)
        {
            siparisRepository = new SiparisRepository();
            var modeler = siparisRepository.SiparisPivotIlkMaddeListesiniGetir(siparis_id, stokkart_id, stokkart_tipi_id, ref errorMessage);

            if (modeler == null || modeler.Rows.Count < 1 || !string.IsNullOrEmpty(errorMessage))
            {
                if (!string.IsNullOrEmpty(errorMessage))
                    return InternalServerError(new Exception(errorMessage));
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }

            List<SiparisIlkmaddePivotModel> pivot = new List<SiparisIlkmaddePivotModel>();
            SiparisIlkmaddePivotModel item = null;

            for (int i = 0; i < modeler.Rows.Count; i++)
            {
                item = new SiparisIlkmaddePivotModel
                {
                    siparis_id = modeler.Rows[i]["siparis_id"].acekaToLong(),
                    sira_id = modeler.Rows[i]["sira_id"].acekaToShort(),
                    sira = modeler.Rows[i]["sira"].acekaToByte(),
                    modelyeri = modeler.Rows[i]["modelyeri"].acekaToString(),
                    aciklama = modeler.Rows[i]["aciklama"].acekaToString(),
                    stok_adi = modeler.Rows[i]["stok_adi"].acekaToString(),
                    renk_id = modeler.Rows[i]["renk_id"].acekaToIntWithNullable(),
                    renk_adi = modeler.Rows[i]["renk_adi"].acekaToString(),
                    talimatturu_id = modeler.Rows[i]["talimatturu_id"].acekaToByte(),
                    talimat_tanim = modeler.Rows[i]["talimat_tanim"].acekaToString(),
                    alt_stokkart_id = modeler.Rows[i]["alt_stokkart_id"].acekaToLongWithNullable(),
                    birim_id = modeler.Rows[i]["birim_id"].acekaToByteWithNullable(),
                    birim_adi = modeler.Rows[i]["birim_adi"].acekaToString(),
                    birim_id3 = modeler.Rows[i]["birim_id3"].acekaToByteWithNullable(),
                    birim_adi3 = modeler.Rows[i]["birim_adi3"].acekaToString()
                };

                item.pivotMatrixData = new List<StokkartIlkmaddePivotMatrix>();

                for (int x = 14; x < modeler.Columns.Count; x++)
                {
                    if (modeler.Columns[x].ColumnName == "sira_id" || modeler.Columns[x].ColumnName == "miktar")
                        continue;

                    float? value = modeler.Rows[i][modeler.Columns[x].ColumnName].acekaToFloatWithNullable();
                    var pivotNameData = modeler.Columns[x].ColumnName.Split(' ');

                    item.pivotMatrixData.Add(new StokkartIlkmaddePivotMatrix
                    {
                        id = pivotNameData[0].acekaToShort(),
                        name = modeler.Columns[x].ColumnName,
                        value = value
                    });
                }

                pivot.Add(item);
                item = null;
            }

            return Ok(pivot.OrderBy(x => x.sira));
        }

        /// <summary>
        /// Sipariş -> Tab -> İlk madde ortak PUT metodu 
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller")]
        public IHttpActionResult ModelkartIlkMaddePOST(SiparisIlkmaddePivotModel modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;

            siparisRepository = new SiparisRepository();

            short maxSiraId = siparisRepository.SiparisModelEnBuyukSiraNo(modelKartIlkMadde.siparis_id, ref errorMessage);
            maxSiraId += 1;

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return InternalServerError(new Exception("Sıra numarası alırken bir hata oluştu!"));
            }

            var isGenel = modelKartIlkMadde.pivotMatrixData.Where(x => x.id != 0).Sum(x => x.value) == 0;

            if (isGenel)
            {
                var model = new siparis_model
                {
                    degistiren_tarih = DateTime.Now,
                    degistiren_carikart_id = Tools.PersonelId,
                    aciklama = modelKartIlkMadde.aciklama,
                    modelyeri = modelKartIlkMadde.modelyeri,
                    alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                    siparis_id = modelKartIlkMadde.siparis_id,
                    ana_kayit = 0,
                    sira = modelKartIlkMadde.sira,
                    sira_id = maxSiraId,
                    birim_id = modelKartIlkMadde.birim_id.Value,
                    renk_id = modelKartIlkMadde.renk_id.Value,
                    beden_id = 0,
                    miktar = modelKartIlkMadde.pivotMatrixData.Where(x => x.id == 0).FirstOrDefault().value,
                };

                acekaResult = CrudRepository<siparis_model>.Insert(model);

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                }
            }
            else
            {
                var bedenler = modelKartIlkMadde.pivotMatrixData.Where(x => x.id != 0).ToList();

                for (int i = 0; i < bedenler.Count; i++)
                {
                    var model = new siparis_model
                    {
                        degistiren_tarih = DateTime.Now,
                        degistiren_carikart_id = Tools.PersonelId,
                        aciklama = modelKartIlkMadde.aciklama,
                        modelyeri = modelKartIlkMadde.modelyeri,
                        alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                        siparis_id = modelKartIlkMadde.siparis_id,
                        ana_kayit = 0,
                        sira = modelKartIlkMadde.sira,
                        sira_id = maxSiraId,
                        birim_id = modelKartIlkMadde.birim_id.Value,
                        renk_id = modelKartIlkMadde.renk_id.Value,
                        beden_id = bedenler[i].id,
                        miktar = bedenler[i].value,
                    };

                    acekaResult = CrudRepository<siparis_model>.Insert(model);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        var databaseError = acekaResult.ErrorInfo.Message;
                        if (databaseError.ToLower().Contains("duplicate"))
                        {
                            return InternalServerError(new Exception("Duplicate record!"));
                        }

                        return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                    }
                }
            }

            return Ok(new AcekaResult { RetVal = 1 });
        }

        /// <summary>
        /// Sipariş -> Tab -> İlk madde ortak PUT metodu 
        /// </summary>
        /// <param name="modelKartIlkMaddeler"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller/list")]
        public IHttpActionResult ModelkartIlkMaddePOST(List<SiparisIlkmaddePivotModel> modelKartIlkMaddeler)
        {
            siparisRepository = new SiparisRepository();
            try
            {
                modelKartIlkMaddeler.ForEach(item =>
                {
                    ModelKartIlkMaddeItemPost(item);
                });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(new AcekaResult { RetVal = 1 });
        }

        private void ModelKartIlkMaddeItemPost(SiparisIlkmaddePivotModel modelKartIlkMadde)
        {
            short maxSiraId = siparisRepository.SiparisModelEnBuyukSiraNo(modelKartIlkMadde.siparis_id, ref errorMessage);
            maxSiraId += 1;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                new Exception("Sıra numarası alırken bir hata oluştu!");
            }
            var isGenel = modelKartIlkMadde.pivotMatrixData.Where(x => x != null && x.id != 0).Sum(x => x.value) == 0;
            if (isGenel)
            {
                var model = new siparis_model
                {
                    degistiren_tarih = DateTime.Now,
                    degistiren_carikart_id = Tools.PersonelId,
                    aciklama = modelKartIlkMadde.aciklama,
                    modelyeri = modelKartIlkMadde.modelyeri,
                    alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                    siparis_id = modelKartIlkMadde.siparis_id,
                    ana_kayit = 0,
                    sira = modelKartIlkMadde.sira,
                    sira_id = maxSiraId,
                    birim_id = modelKartIlkMadde.birim_id.Value,
                    renk_id = modelKartIlkMadde.renk_id.Value,
                    beden_id = 0,
                    miktar = modelKartIlkMadde.pivotMatrixData.Where(x => x.id == 0).FirstOrDefault().value,
                };
                var acekaResult = CrudRepository<siparis_model>.Insert(model);
                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    new Exception(acekaResult.ErrorInfo.Message);
                }
            }
            else
            {
                var bedenler = modelKartIlkMadde.pivotMatrixData.Where(x => x != null && x.id != 0).ToList();

                for (int i = 0; i < bedenler.Count; i++)
                {
                    var model = new siparis_model
                    {
                        degistiren_tarih = DateTime.Now,
                        degistiren_carikart_id = Tools.PersonelId,
                        aciklama = modelKartIlkMadde.aciklama,
                        modelyeri = modelKartIlkMadde.modelyeri,
                        alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                        siparis_id = modelKartIlkMadde.siparis_id,
                        ana_kayit = 0,
                        sira = modelKartIlkMadde.sira,
                        sira_id = maxSiraId,
                        birim_id = modelKartIlkMadde.birim_id.Value,
                        renk_id = modelKartIlkMadde.renk_id.Value,
                        beden_id = bedenler[i].id,
                        miktar = bedenler[i].value,
                    };

                    var acekaResult = CrudRepository<siparis_model>.Insert(model);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        var databaseError = acekaResult.ErrorInfo.Message;
                        if (databaseError.ToLower().Contains("duplicate"))
                        {
                            new Exception("Duplicate record!");
                        }

                        new Exception(acekaResult.ErrorInfo.Message);
                    }
                }
            }
        }


        /// <summary>
        /// Sipariş -> Tab -> İlk madde ortak PUT metodu 
        /// Not: PUT metodu için  "siparis_id", "sira_id", "alt_stokkart_id" bilgisi muhakkak gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller")]
        public IHttpActionResult ModelkartIlkMaddePUT(SiparisIlkmaddePivotModel modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;
            if (modelKartIlkMadde == null)
                return BadRequest();

            siparisRepository = new SiparisRepository();

            var isGenel = modelKartIlkMadde.pivotMatrixData.Where(x => x.id != 0).Sum(x => x.value) == 0;

            if (isGenel)
            {
                var deleteData = new siparis_model
                {
                    siparis_id = modelKartIlkMadde.siparis_id,
                    sira_id = modelKartIlkMadde.sira_id,
                };

                acekaResult = CrudRepository<siparis_model>.Delete(deleteData, "siparis_model", new string[] { "siparis_id", "sira_id" }, new string[] { "siparis_id", "sira_id" });

                var model = new siparis_model
                {
                    degistiren_tarih = DateTime.Now,
                    degistiren_carikart_id = Tools.PersonelId,
                    talimatturu_id = modelKartIlkMadde.talimatturu_id,
                    aciklama = modelKartIlkMadde.aciklama,
                    modelyeri = modelKartIlkMadde.modelyeri,
                    alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                    siparis_id = modelKartIlkMadde.siparis_id,
                    ana_kayit = 0,
                    sira = modelKartIlkMadde.sira,
                    sira_id = modelKartIlkMadde.sira_id,
                    renk_id = modelKartIlkMadde.renk_id.Value,
                    beden_id = 0,
                    birim_id = modelKartIlkMadde.birim_id.Value,
                    birim_id3 = modelKartIlkMadde.birim_id3,
                    miktar = modelKartIlkMadde.pivotMatrixData.Where(x => x.id == 0).First().value
                };

                acekaResult = CrudRepository<siparis_model>.Insert(model);

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                }
            }
            else
            {
                var deleteData = new siparis_model
                {
                    siparis_id = modelKartIlkMadde.siparis_id,
                    sira_id = modelKartIlkMadde.sira_id,
                };

                acekaResult = CrudRepository<siparis_model>.Delete(deleteData, "siparis_model", new string[] { "siparis_id", "sira_id" }, new string[] { "siparis_id", "sira_id" });

                var bedenler = modelKartIlkMadde.pivotMatrixData.Where(x => x.id != 0).ToList();

                for (int i = 0; i < bedenler.Count; i++)
                {
                    var model = new siparis_model
                    {
                        degistiren_tarih = DateTime.Now,
                        degistiren_carikart_id = Tools.PersonelId,
                        talimatturu_id = modelKartIlkMadde.talimatturu_id,
                        aciklama = modelKartIlkMadde.aciklama,
                        modelyeri = modelKartIlkMadde.modelyeri,
                        alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                        siparis_id = modelKartIlkMadde.siparis_id,
                        ana_kayit = 0,
                        sira = modelKartIlkMadde.sira,
                        sira_id = modelKartIlkMadde.sira_id,
                        renk_id = modelKartIlkMadde.renk_id.Value,
                        beden_id = bedenler[i].id.acekaToByte(),
                        birim_id = modelKartIlkMadde.birim_id.Value,
                        miktar = bedenler[i].value,
                    };

                    acekaResult = CrudRepository<siparis_model>.Insert(model);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
                    }
                }
            }

            return Ok(acekaResult);
        }


        /// <summary>
        /// Sipariş -> Tab -> İlk madde ortak PUT metodu 
        /// Not: PUT metodu için  "siparis_id", "sira_id", "alt_stokkart_id" bilgisi muhakkak gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller/list")]
        public IHttpActionResult ModelkartIlkMaddePUT(List<SiparisIlkmaddePivotModel> modelKartIlkMadde)
        {
            AcekaResult acekaResult = new AcekaResult();
            if (modelKartIlkMadde == null)
                return BadRequest();

            siparisRepository = new SiparisRepository();
            try
            {
                modelKartIlkMadde.ForEach(x =>
                {
                    ModelKartIlkMaddeItemPUT(x);
                });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok(acekaResult);
        }

        private void ModelKartIlkMaddeItemPUT(SiparisIlkmaddePivotModel modelKartIlkMadde)
        {
            var acekaResult = new AcekaResult();
            var isGenel = modelKartIlkMadde.pivotMatrixData.Where(x => x.id == 0).Sum(x => x.value) > 0;

            if (isGenel)
            {
                var deleteData = new siparis_model
                {
                    siparis_id = modelKartIlkMadde.siparis_id,
                    sira_id = modelKartIlkMadde.sira_id,
                };

                CrudRepository<siparis_model>.Delete(deleteData, "siparis_model", new string[] { "siparis_id", "sira_id" }, new string[] { "siparis_id", "sira_id" });

                var model = new siparis_model
                {
                    degistiren_tarih = DateTime.Now,
                    degistiren_carikart_id = Tools.PersonelId,
                    talimatturu_id = modelKartIlkMadde.talimatturu_id,
                    aciklama = modelKartIlkMadde.aciklama,
                    modelyeri = modelKartIlkMadde.modelyeri,
                    alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                    siparis_id = modelKartIlkMadde.siparis_id,
                    ana_kayit = 0,
                    sira = modelKartIlkMadde.sira,
                    sira_id = modelKartIlkMadde.sira_id,
                    renk_id = modelKartIlkMadde.renk_id.Value,
                    beden_id = 0,
                    birim_id = modelKartIlkMadde.birim_id.Value,
                    birim_id3 = modelKartIlkMadde.birim_id3,
                    miktar = modelKartIlkMadde.pivotMatrixData.Where(x => x.id == 0).First().value
                };

                acekaResult = CrudRepository<siparis_model>.Insert(model);

                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    new Exception(acekaResult.ErrorInfo.Message);
                }
            }
            else
            {
                var deleteData = new siparis_model
                {
                    siparis_id = modelKartIlkMadde.siparis_id,
                    sira_id = modelKartIlkMadde.sira_id,
                };

                acekaResult = CrudRepository<siparis_model>.Delete(deleteData, "siparis_model", new string[] { "siparis_id", "sira_id" }, new string[] { "siparis_id", "sira_id" });

                var bedenler = modelKartIlkMadde.pivotMatrixData.Where(x => x.id != 0).ToList();

                for (int i = 0; i < bedenler.Count; i++)
                {
                    var model = new siparis_model
                    {
                        degistiren_tarih = DateTime.Now,
                        degistiren_carikart_id = Tools.PersonelId,
                        talimatturu_id = modelKartIlkMadde.talimatturu_id,
                        aciklama = modelKartIlkMadde.aciklama,
                        modelyeri = modelKartIlkMadde.modelyeri,
                        alt_stokkart_id = modelKartIlkMadde.alt_stokkart_id.Value,
                        siparis_id = modelKartIlkMadde.siparis_id,
                        ana_kayit = 0,
                        sira = modelKartIlkMadde.sira,
                        sira_id = modelKartIlkMadde.sira_id,
                        renk_id = modelKartIlkMadde.renk_id.Value,
                        beden_id = bedenler[i].id.acekaToByte(),
                        birim_id = modelKartIlkMadde.birim_id.Value,
                        miktar = bedenler[i].value,
                    };

                    acekaResult = CrudRepository<siparis_model>.Insert(model);

                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        new Exception(acekaResult.ErrorInfo.Message);
                    }
                }
            }
        }


        /// <summary>
        /// Siparis -> Tab -> İlk madde ortak DELETE metodu 
        /// Not: DELETE metodu için  sadece "siparis_id", "sira_id" gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller")]
        public IHttpActionResult ModelkartIlkMaddeDelete(SiparisModelKartIlkMadde modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;

            siparisRepository = new SiparisRepository();

            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("siparis_id", modelKartIlkMadde.siparis_id);
            fields.Add("sira_id", modelKartIlkMadde.sira_id);

            acekaResult = CrudRepository.Delete("siparis_model", new string[] { "siparis_id", "sira_id" }, fields);

            if (acekaResult != null && acekaResult.ErrorInfo != null)
            {
                return InternalServerError(new Exception(acekaResult.ErrorInfo.Message));
            }

            return Ok(acekaResult);
        }

        /// <summary>
        /// Siparis -> Tab -> İlk madde ortak DELETE metodu 
        /// Not: DELETE metodu için  sadece "siparis_id", "sira_id" gönderilmelidir!
        /// </summary>
        /// <param name="modelKartIlkMadde"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/siparis/ilk-madde-modeller")]
        [Route("api/siparis/ilk-madde-modeller/list")]
        public IHttpActionResult ModelkartIlkMaddeDelete(List<SiparisModelKartIlkMadde> modelKartIlkMadde)
        {
            AcekaResult acekaResult = null;
            siparisRepository = new SiparisRepository();
            try
            {
                modelKartIlkMadde.ForEach(x =>
                {
                    ModelkartIlkMaddeItemDelete(x);
                });
            }
            catch (Exception e)
            {
                InternalServerError(e);
            }

            return Ok(acekaResult);
        }

        private void ModelkartIlkMaddeItemDelete(SiparisModelKartIlkMadde modelKartIlkMadde)
        {
            var acekaResult = new AcekaResult();

            Dictionary<string, object> fields = new Dictionary<string, object>();
            fields.Add("siparis_id", modelKartIlkMadde.siparis_id);
            fields.Add("sira_id", modelKartIlkMadde.sira_id);

            CrudRepository.Delete("siparis_model", new string[] { "siparis_id", "sira_id" }, fields);

            if (acekaResult != null && acekaResult.ErrorInfo != null)
            {
                new Exception(acekaResult.ErrorInfo.Message);
            }
        }

        #endregion
        #region Sipariş Notlar

        /// <summary>
        /// Siparis Notlar Get Metodu 
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <returns>
        ///[
        ///  {
        ///    "siparis_id": 123,
        ///    "sira_id": 1,
        ///    "aciklama": "Not 1",
        ///    "degistiren_carikart_id": 100000000100,
        ///    "degistiren_tarih": "2017-04-13T09:36:17"
        ///  },
        ///  {
        ///    "siparis_id": 123,
        ///    "sira_id": 2,
        ///    "aciklama": "Not 2",
        ///    "degistiren_carikart_id": 100000000100,
        ///    "degistiren_tarih": "2017-04-13T09:36:17"
        ///  }
        ///]
        /// </returns>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        [Route("api/siparis/siparis-notlar/{siparis_id}")]
        public HttpResponseMessage NotlarGet(long siparis_id)
        {
            if (siparis_id > 0)
            {
                siparisRepository = new SiparisRepository();

                var siparisnot = siparisRepository.SiparisNotGetir(siparis_id, ref errorMessage);
                if (siparisnot != null && string.IsNullOrEmpty(errorMessage))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, siparisnot.Select(notlar => new
                    {
                        notlar.siparis_id,
                        notlar.sira_id,
                        notlar.aciklama,
                        notlar.degistiren_carikart_id,
                        notlar.degistiren_tarih
                    }));
                }
                else
                {
                    if (!string.IsNullOrEmpty(errorMessage))
                        return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = errorMessage });
                    else
                        return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "Geçersiz Kayıt Girildi!" });
            }

        }

        ///// <summary>
        ///// Sipariş Notlar -> POST metodu
        ///// </summary>
        ///// <param name="notlar"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        //[Route("api/siparis/siparis-notlar")]
        //public HttpResponseMessage NotlarPost(siparis_notlar notlar)
        //{
        //    siparisRepository = new SiparisRepository();
        //    AcekaResult acekaResult = null;
        //    if (notlar != null)
        //    {
        //        int maxSiraId = siparisRepository.NotsiraidGetir(notlar.siparis_id, ref errorMessage);
        //        maxSiraId = maxSiraId + 1;
        //        notlar.sira_id = (short)maxSiraId;
        //        notlar.degistiren_tarih = DateTime.Now;
        //        notlar.degistiren_carikart_id = Tools.PersonelId;
        //        acekaResult = CrudRepository<siparis_notlar>.Insert(notlar, "siparis_notlar");
        //        if (acekaResult != null && acekaResult.ErrorInfo == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful", ret_val = acekaResult.RetVal.ToString() });
        //        }
        //        else
        //        {
        //            if (acekaResult.ErrorInfo != null)
        //            {
        //                if (acekaResult.ErrorInfo.Message.ToLower().Contains("duplicate"))
        //                {
        //                    acekaResult.ErrorInfo.Message = "Sipariş Notlar 'a Aynı Kayıt 2.kez tanımlanamaz!";
        //                }
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = acekaResult.ErrorInfo.Message });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
        //    }
        //}


        /// <summary>
        /// Sipariş Notlar -> POST List metodu
        /// </summary>
        /// <param name="notlar"></param>
        /// <returns></returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        [Route("api/siparis/siparis-notlar")]
        public IHttpActionResult NotlarPost(List<siparis_notlar> notlar)
        {
            siparisRepository = new SiparisRepository();
            try
            {
                notlar.ForEach(item =>
                {
                    NotlarItemPost(item);
                });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(new AcekaResult { RetVal = 1 });
        }
        private void NotlarItemPost(siparis_notlar siparisnotlar)
        {
            AcekaResult acekaResult = null;
            if (siparisnotlar != null)
            {
                int maxSiraId = siparisRepository.NotsiraidGetir(siparisnotlar.siparis_id, ref errorMessage);
                maxSiraId = maxSiraId + 1;

                siparisnotlar.sira_id = (short)maxSiraId;
                siparisnotlar.degistiren_tarih = DateTime.Now;
                siparisnotlar.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<siparis_notlar>.Insert(siparisnotlar, "siparis_notlar");
            }
            else
            {
                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    new Exception(acekaResult.ErrorInfo.Message);
                }
            }

        }

        ///// <summary>
        ///// Sipariş Notlar -> PUT metodu
        ///// </summary>
        ///// <param name="notlar"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        //[Route("api/siparis/siparis-notlar")]
        //public HttpResponseMessage NotlarPut(siparis_notlar notlar)
        //{
        //    AcekaResult acekaResult = null;
        //    siparisRepository = new SiparisRepository();
        //    if (notlar != null)
        //    {
        //        var not = siparisRepository.NotGetir(notlar.siparis_id, notlar.sira_id, ref errorMessage);
        //        if (not != null)
        //        {
        //            not.siparis_id = notlar.siparis_id;
        //            not.sira_id = notlar.sira_id;
        //            not.aciklama = notlar.aciklama;
        //            not.degistiren_carikart_id = Tools.PersonelId;
        //            not.degistiren_tarih = DateTime.Now;
        //        }
        //        acekaResult = CrudRepository<siparis_notlar>.Update(not, new string[] { "siparis_id", "sira_id" });
        //        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "A problem has been occurred during the process." });
        //    }
        //}

        /// <summary>
        /// Sipariş Notlar -> POST metodu
        /// </summary>
        /// <param name="notlar"></param>
        /// <returns></returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        [Route("api/siparis/siparis-notlar")]
        public IHttpActionResult NotlarPut(List<siparis_notlar> notlar)
        {
            siparisRepository = new SiparisRepository();
            try
            {
                notlar.ForEach(item =>
                {
                    NotlarItemPut(item);
                });
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok(new AcekaResult { RetVal = 1 });
        }
        private void NotlarItemPut(siparis_notlar siparisnotlar)
        {
            AcekaResult acekaResult = null;
            if (siparisnotlar != null)
            {
                //int maxSiraId = siparisRepository.NotsiraidGetir(siparisnotlar.siparis_id, ref errorMessage);
                //maxSiraId = maxSiraId + 1;
                //siparisnotlar.sira_id = (short)maxSiraId;
                siparisnotlar.degistiren_tarih = DateTime.Now;
                siparisnotlar.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<siparis_notlar>.Update(siparisnotlar, new string[] { "siparis_id", "sira_id" });
            }
            else
            {
                if (acekaResult != null && acekaResult.ErrorInfo != null)
                {
                    new Exception(acekaResult.ErrorInfo.Message);
                }
            }

        }

        /// <summary>
        /// Sipariş Notlar -> PUT metodu
        /// </summary>
        /// <param name="sipnot"></param>
        /// <returns></returns>
        [HttpDelete]
        [CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        [Route("api/siparis/siparis-notlar")]
        public HttpResponseMessage Notlar(siparis_notlar sipnot)
        {
            AcekaResult acekaResult = null;
            if (
                sipnot != null &&
                sipnot.siparis_id > 0 &&
                sipnot.sira_id > 0
                )
            {
                siparisRepository = new SiparisRepository();
                var model = siparisRepository.NotGetir(sipnot.siparis_id, sipnot.sira_id, ref errorMessage);
                if (model != null && string.IsNullOrEmpty(errorMessage))
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("siparis_id", sipnot.siparis_id);
                    fields.Add("sira_id", sipnot.sira_id);
                    acekaResult = CrudRepository.Delete("siparis_notlar", new string[] { "siparis_id", "sira_id" }, fields);
                    if (acekaResult != null && acekaResult.ErrorInfo != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.Successful { message = acekaResult.ErrorInfo.Message });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
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
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Siparişe ait böyle bir kayıt bulunamadı!" });
                    }
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Lütfen POST model içerisindeki verileri kontrol ediniz!" });
            }
        }



        ///// <summary>
        ///// Sipariş Notlar -> PUT metodu
        ///// </summary>
        ///// <param name="sipnotlar"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[CustAuthFilter(ApiUrl = "api/siparis/siparis-notlar")]
        //[Route("api/siparis/siparis-notlar")]
        //public IHttpActionResult Notlar(List<siparis_notlar> sipnotlar)
        //{
        //    AcekaResult acekaResult = null;
        //    siparisRepository = new SiparisRepository();
        //    try
        //    {
        //        sipnotlar.ForEach(x =>
        //        {
        //            notlarItemDelete(x);
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        InternalServerError(e);
        //    }

        //    return Ok(acekaResult);

        //}
        //private void notlarItemDelete(siparis_notlar sipnot)
        //{
        //    AcekaResult acekaResult = null;

        //    if (
        //        sipnot != null &&
        //        sipnot.siparis_id > 0 &&
        //        sipnot.sira_id > 0
        //        )
        //    {
        //        siparisRepository = new SiparisRepository();
        //        var model = siparisRepository.NotGetir(sipnot.siparis_id, sipnot.sira_id, ref errorMessage);

        //        if (model != null && string.IsNullOrEmpty(errorMessage))
        //        {
        //            Dictionary<string, object> fields = new Dictionary<string, object>();
        //            fields.Add("siparis_id", sipnot.siparis_id);
        //            fields.Add("sira_id", sipnot.sira_id);

        //            acekaResult = CrudRepository.Delete("siparis_notlar", new string[] { "siparis_id", "sira_id" }, fields);
        //            if (acekaResult != null && acekaResult.ErrorInfo != null)
        //            {
        //                new Exception(acekaResult.ErrorInfo.Message);
        //            }
        //            else
        //            {
        //                Ok();
        //            }
        //        }
        //        else
        //        {
        //            new Exception(acekaResult.ErrorInfo.Message);
        //            //if (!string.IsNullOrEmpty(errorMessage))
        //            //{
        //            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = errorMessage });
        //            //}
        //            //else
        //            //{
        //            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Siparişe ait böyle bir kayıt bulunamadı!" });
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        new Exception(acekaResult.ErrorInfo.Message);
        //        //return Request.CreateResponse(HttpStatusCode.InternalServerError, new Models.AnonymousModels.NotFound { message = "Lütfen POST model içerisindeki verileri kontrol ediniz!" });
        //    }
        //}
        #endregion
        #region Sipariş Onay Ve Logları
        /// <summary>
        /// Siparis Onay Loglarını Getiren Metod
        /// </summary>
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
        /// <param name="siparis_id"></param>
        [HttpGet]
        [CustAuthFilter(ApiUrl = "api/siparis/onay")]
        [Route("api/siparis/onay/{siparis_id}")]
        public HttpResponseMessage SiparisOnayLoglari(long siparis_id)
        {
            siparisRepository = new SiparisRepository();
            var onaylar = siparisRepository.SiparisOnayLoglari(siparis_id, CustomEnums.OnayLogTipi.genel_onay, ref errorMessage);
            if (onaylar != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, onaylar.Select(ony => new
                {
                    ony.siparis_id,
                    ony.onay_carikart_id,
                    ony.onaylayan_carikart.cari_unvan,
                    ony.onay_tarihi,
                    ony.iptal_carikart_id,
                    ony.iptal_tarihi,
                    iptal_eden_cari_kart = ony.iptal_eden_carikart.cari_unvan,
                    ony.onay_alan_adi
                }));

            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Siparis Onay Logları POST Metod
        /// </summary>
        /// <param name="sipOnay"></param>
        /// <returns>
        /// </returns>
        [HttpPost]
        [CustAuthFilter(ApiUrl = "api/siparis/onay")]
        [Route("api/siparis/onay")]
        public HttpResponseMessage SiparisOnayLogPost(siparis_onay sipOnay)
        {
            siparisRepository = new SiparisRepository();

            if (sipOnay != null)
            {
                sipOnay.genel_onay = true;
                var siparisOnayRetVal = CrudRepository<siparis_onay>.Insert(sipOnay, "siparis_onay");
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        /// <summary>
        /// Siparis Onay Logları PUT Metod
        /// </summary>
        /// <param name="sipOnay"></param>
        /// <returns>
        /// </returns>
        [HttpPut]
        [CustAuthFilter(ApiUrl = "api/siparis/onay")]
        [Route("api/siparis/onay")]
        public HttpResponseMessage SiparisOnayLogPut(siparis_onay sipOnay)
        {
            AcekaResult acekaResult = null;
            siparisRepository = new SiparisRepository();

            if (sipOnay != null && sipOnay.siparis_id > 0)
            {
                siparis_onay_log sip_log = new siparis_onay_log();
                if (sipOnay.genel_onay == false)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("onay_alan_adi", "genel_onay"); //onaylog.onay_alan_adi
                    fields.Add("iptal_carikart_id", Tools.PersonelId);
                    fields.Add("iptal_tarihi", DateTime.Now);
                    fields.Add("siparis_id", sipOnay.siparis_id);//onaylog.stokkart_id

                    acekaResult = CrudRepository.Update("siparis_onay_log", "siparis_id = " + sipOnay.siparis_id + " AND onay_alan_adi = 'genel_onay' AND  iptal_tarihi is null", fields, true);
                }
                else
                {
                    sip_log.onay_carikart_id = Tools.PersonelId;
                    sip_log.onay_tarihi = DateTime.Now;
                    sip_log.onay_alan_adi = "genel_onay";
                    sip_log.siparis_id = sipOnay.siparis_id;
                    CrudRepository<siparis_onay_log>.Insert(sip_log, "siparis_onay_log", new string[] { "onaylayan_carikart", "iptal_eden_carikart" });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "Successful" });
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }

        #endregion
        #endregion

    }
}
