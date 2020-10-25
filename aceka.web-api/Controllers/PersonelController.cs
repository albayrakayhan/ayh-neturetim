using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models.PersonelModel;
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
    /// Personel İşlemleri ile ilgili Metodlar yer alır.
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PersonelController : ApiController
    {
        #region Degiskenler
        private Personel per = null;
        private List<Personel> personeller = null;
        private PersonelRepository personelRepository = null;
        private carikart_muhasebe_personel muh = null;
        private List<carikart_personel_calisma_yerleri> personel_calisma_yerleri = null;
        private List<giz_sabit_carikart_tipi> caritip = null;
        private List<parametre_genel> parametre = null;
        private List<parametre_carikart_rapor> personelparametre = null;
        #endregion

        /// <summary>
        /// Personel arama metodu.
        /// Not: 5 parametreden en az bir tanesi gönderilmelidir!
        /// </summary>
        /// <param name="carikart_id">Opsiyonel</param>
        /// <param name="cari_unvan">Opsiyonel</param>
        /// <param name="ozel_kod">Opsiyonel</param>
        /// <param name="carikart_tipi_id">Opsiyonel</param>
        /// <param name="statu">Opsiyonel</param>
        /// <returns>
        /// [
        /// {
        /// carikart_id: 100000000947,
        /// unvan: "hasan",
        /// ozel_kod: 1234
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [Route("api/personel/personel-bul")]
        public IList<Personel> PersonelAra(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", int carikart_tipi_id = 0, string statu = "")
        {
            personelRepository = new PersonelRepository();
            personeller = personelRepository.Bul(carikart_id, cari_unvan, ozel_kod, carikart_tipi_id, statu);
            return personeller;
        }

        /// <summary>
        /// personel arama metodu özet .
        /// carikart_id,unvan,ozel_kod parametrelerini almaktadır
        /// </summary>
        /// <param name="carikart_id">Opsiyonel</param>
        /// <param name="cari_unvan">Opsiyonel</param>
        /// <param name="ozel_kod">Opsiyonel</param>
        /// <param name="carikart_tipi_id">Opsiyonel</param>
        /// <param name="statu">Opsiyonel</param>
        /// <returns>
        /// [
        /// {
        /// carikart_id: 100000000947,
        /// unvan: "hasan",
        /// ozel_kod: 1234
        ///}
        ///]
        /// </returns>
        [HttpGet]
        [Route("api/personel/personel-arama")]
        public HttpResponseMessage PersonelAraOzet(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", int carikart_tipi_id = 0, string statu = "")
        {
            personelRepository = new PersonelRepository();
            personeller = personelRepository.Bul(carikart_id, cari_unvan, ozel_kod, carikart_tipi_id, statu);
            if (personeller != null)
            {
                var person = personeller.Select(s => new
                {
                    tanim = s.cari_unvan,
                    personel_no = s.carikart_id,
                    personel_kod = s.ozel_kod,
                    statu = s.statu_adi,
                    personeltipi = s.carikart_tipi_adi
                    ////,dogumtarihi = s.personel_kimlik.dogum_tarihi
                });
                return Request.CreateResponse(person);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// giz_sabit_carikart_tipi personel
        /// </summary>
        /// <returns>
        /// {
        ///      "personel_tipi_id": 21,
        ///      "personel_tipi_adi": "Personel"
        /// }
        /// </returns>
        [HttpGet]
        [Route("api/personel/tipleri")]
        public HttpResponseMessage PersonelTipleriniGetir()
        {
            personelRepository = new PersonelRepository();
            caritip = personelRepository.CarikartTipleriniGetir();

            if (caritip != null)
            {
                var ozet = caritip.Select(o => new
                {
                    personel_tipi_id = o.carikart_tipi_id,
                    personel_tipi_adi = o.carikart_tipi_adi,
                });

                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }

        }

        #region Personel genel Liste-insert-update

        /// <summary>
        /// personelin genel bilgilerini döndürür
        /// carikart_id parametresini almaktadır
        /// </summary>
        /// /// <param name="carikart_id">carikart tablosunda carikart_id alanı</param>
        /// <returns>
        ///{
        ///  "carikart_id": 100120000041,
        ///  "kayit_silindi": false,
        ///  "statu": true,
        ///  "carikart_turu_id": 2,
        ///  "carikart_tipi_id": 21,
        ///  "cari_unvan": "Ahmet KAPLAN",
        ///  "ozel_kod": "1234567",
        ///  "giz_yazilim_kodu": 123,
        ///  "sube_carikart_id": 100000001360,
        ///  "giz_kullanici_adi": "A.Kap",
        ///  "giz_kullanici_sifre": "adeneşifreee",
        ///  "muh_masraf": {
        ///    "carikart_id_m": 100120000041,
        ///    "sirket_id": 1,
        ///    "sirket_adi": "Giz Yazılım",
        ///    "sene": 0,
        ///    "muh_kod": "120",
        ///    "muh_kod_adi": "Alıcılar",
        ///    "masraf_merkezi_id": 0,
        ///    "masraf_merkezi_adi": ""
        ///  },
        ///  "degistiren_carikart_id": 0
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/genel/{carikart_id}")]
        public HttpResponseMessage Personel_Genel(long carikart_id)
        {
            personelRepository = new PersonelRepository();
            var person = personelRepository.Getir(carikart_id);
            if (person != null)
            {
                var ozet = new
                {
                    carikart_id = person.carikart_id,
                    statu = person.statu,
                    cari_unvan = person.cari_unvan,
                    ozel_kod = person.ozel_kod,
                    sube_carikart_id = person.sube_carikart_id,
                    giz_kullanici_adi = person.giz_kullanici_adi,
                    giz_kullanici_sifre = person.giz_kullanici_sifre,
                    person.muh_masraf,
                    degistiren_carikart_id = person.degistiren_carikart_id,
                };
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin genel bilgilerinin INSERT edilği bölüm
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/personel/genel")]
        public HttpResponseMessage Personel_Genel(Personel_Model per)
        {
            AcekaResult acekaResult = null;
            personelRepository = new PersonelRepository();

            if (per != null && per.carikart_id ==0)
            {
                per.carikart_turu_id = 2;
                per.carikart_tipi_id = 21;
                acekaResult = CrudRepository<Personel_Model>.Insert(per, "carikart", new string[] { "degistiren_tarih", "carikart_tipi_adi", "carikart_id", "carikart_turu_adi" });
                if (acekaResult != null && acekaResult.ErrorInfo == null)
                {
                    //var person = personelRepository.Getir(acekaResult.RetVal.acekaToLong());
                    long carikartId = acekaResult.RetVal.acekaToLong();
                    if (per.muh_masraf != null && per.muh_masraf.carikart_id == 0)
                    {
                        per.muh_masraf.carikart_id = acekaResult.RetVal.acekaToLong();
                        var muhmasraf = CrudRepository<carikart_muhasebe_personel_Model>.Insert(per.muh_masraf, "carikart_muhasebe", new string[] { "sirket_adi", "muh_kod_adi", "masraf_merkezi_adi", "masraf_merkezi_id", "degistiren_tarih", "masraf_merkezi_id" });
                    }
                    if (per.iletisim != null && per.iletisim.carikart_id == 0)
                    {
                        per.iletisim.carikart_id = carikartId;
                        PersonelIletisimPost(per.iletisim);
                    }
                    if (per.parametre != null && per.parametre.carikart_id == 0)
                    {
                        per.parametre.carikart_id = carikartId; 
                        PersonelParametreinsert(per.parametre);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful", ret_val = carikartId.ToString() });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
                }

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, acekaResult.ErrorInfo.Message);
            }
        }

        /// <summary>
        /// personelin genel bilgilerinin UPDATE edildiği bölüm
        /// </summary>
        /// /// <param name="personel"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/personel/genel")]
        public HttpResponseMessage Personel_GenelPut(Personel_Model personel)
        {
            AcekaResult acekaResult = null;

            if (personel != null)
            {
                if (personel.carikart_id > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", personel.carikart_id);
                    fields.Add("degistiren_carikart_id", Tools.PersonelId);
                    fields.Add("statu", personel.statu);
                    fields.Add("cari_unvan", personel.cari_unvan);
                    fields.Add("ozel_kod", personel.ozel_kod);
                    fields.Add("giz_kullanici_adi", personel.giz_kullanici_adi);
                    fields.Add("giz_kullanici_sifre", personel.giz_kullanici_sifre);
                    // fields.Add("statu", personel.statu);

                    if (personel.sube_carikart_id > 0) fields.Add("sube_carikart_id", personel.sube_carikart_id);
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                    if (acekaResult != null && acekaResult.RetVal != null)
                    {
                        personelRepository = new PersonelRepository();
                        var person = personelRepository.Getir(personel.carikart_id);
                        if (person != null)
                        {
                            if (person.muh_masraf.carikart_id_m > 0 && (personel.muh_masraf != null && personel.muh_masraf.muh_kod != null && personel.muh_masraf.muh_kod.ToString().Length > 0))
                            {
                                person.muh_masraf.carikart_id_m = person.carikart_id;
                                Dictionary<string, object> fields_muh = new Dictionary<string, object>();
                                if (personel.muh_masraf.sirket_id > 0) fields_muh.Add("sirket_id", personel.muh_masraf.sirket_id);
                                if (personel.muh_masraf.sene > 0) fields_muh.Add("sene", personel.muh_masraf.sene);
                                fields_muh.Add("carikart_id", personel.carikart_id);
                                fields_muh.Add("degistiren_carikart_id", personel.degistiren_carikart_id);
                                fields_muh.Add("muh_kod", personel.muh_masraf.muh_kod);
                                AcekaResult acekaResult2 = null;
                                acekaResult2 = CrudRepository.Update("carikart_muhasebe", "carikart_id", fields_muh);
                            }
                            else if (personel.muh_masraf != null && personel.muh_masraf.muh_kod != null && personel.muh_masraf.muh_kod.ToString().Length > 0)
                            {
                                personel.muh_masraf.carikart_id = person.carikart_id;
                                string[] not_include2 = { "sirket_adi", "muh_kod_adi", "masraf_merkezi_adi", "sene", "masraf_merkezi_id", "degistiren_tarih", "masraf_merkezi_id" };
                                AcekaResult acekaResult2 = null;
                                acekaResult2 = CrudRepository<carikart_muhasebe_personel_Model>.Insert(personel.muh_masraf, "carikart_muhasebe", not_include2);
                            }
                            if (personel.parametre != null)
                            {
                                personel.parametre.carikart_id = person.carikart_id;
                                PersonelParametreput(personel.parametre);
                            }
                            if (personel.iletisim != null)
                            {
                                //personelRepository = new PersonelRepository();
                                //var o = personelRepository.PersonelAdresBul(person.carikart_id);
                                var o = personel.iletisim.carikart_adres_id;
                                personel.iletisim.carikart_id = person.carikart_id;
                                //if (o.carikart_adres_id > 0)
                                if (personel.iletisim.carikart_adres_id > 0)
                                {
                                    personel.iletisim.carikart_adres_id = personel.iletisim.carikart_adres_id; ;
                                    PersonelIletisimPut(personel.iletisim);
                                }
                                else
                                {
                                    PersonelIletisimPost(personel.iletisim);
                                }
                                //var pSonuc = PersonelIletisimGetir(personel.carikart_id);
                                // personel.iletisim.carikart_id = person.carikart_id;
                                // if(pSonuc != null && pSonuc.IsSuccessStatusCode)
                                // {
                                //     var ii = ((HttpResponseMessage)pSonuc.Content).TryGetContentValue("carikart_adres_id");

                                //     PersonelIletisimPut(personel.iletisim);
                                // }
                                // else
                                // {
                                //     PersonelIletisimPost(personel.iletisim);
                                // }
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
            }
            return null;
        }

        /// <summary>
        /// personelin genel bilgilerinin UPDATE edildiği bölüm
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/personel/genel")]
        public HttpResponseMessage Personel_GenelDelete(Personel_Model per)
        {
            AcekaResult acekaResult = null;

            if (per != null)
            {
                if (per.carikart_id.acekaToLong() > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", per.carikart_id);
                    fields.Add("degistiren_carikart_id", per.degistiren_carikart_id);
                    fields.Add("statu", per.statu);
                    fields.Add("cari_unvan", per.cari_unvan);
                    fields.Add("ozel_kod", per.ozel_kod);
                    fields.Add("giz_kullanici_adi", per.giz_kullanici_adi);
                    fields.Add("giz_kullanici_sifre", per.giz_kullanici_sifre);
                    fields.Add("kayit_silindi", 1);
                    if (per.sube_carikart_id > 0) fields.Add("sube_carikart_id", per.sube_carikart_id);
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                    if (acekaResult != null && acekaResult.RetVal.acekaToInt() > 0 & (per.muh_masraf != null && per.muh_masraf.muh_kod.ToString().Length > 0))
                    {
                        if (per.muh_masraf != null)
                        {
                            Dictionary<string, object> fields_muh = new Dictionary<string, object>();
                            fields_muh.Add("carikart_id", per.carikart_id);
                            if (per.muh_masraf.sirket_id > 0) fields_muh.Add("sirket_id", per.muh_masraf.sirket_id);
                            fields_muh.Add("degistiren_carikart_id", per.degistiren_carikart_id);
                            fields_muh.Add("muh_kod", per.muh_masraf.muh_kod);
                            string[] Wherefields = { "sirket_id", "carikart_id" };
                            acekaResult = CrudRepository.Delete("carikart_muhasebe", Wherefields, fields_muh);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
            }
            return null;
        }

        #endregion

        #region tab içerikleri Liste-insert-update

        #region Personel parametreler Liste-insert-update

        /// <summary>
        /// rapor parametreleri için parametrelerin listesini döndürür ekran ilk açıldığı anda bu metodlar çalışacak.
        /// burada parametre alanı 1den 7ye kadar cari_parametre_1---7 alanına ait olduğunu belirtmektedir
        /// yani parametre 2 ise cari_parametre_2 ye ait olan kayıtlar olduğunu belirtir
        /// kaynak_1_parametre_id alanı ise mesela 1. nesneden seçim yapıldığı anda o nesnenin parametre_id alanı  ikinci nesnede de 
        /// kaynak_1_parametre_id alanı 1 olan kayıtlar listelenecek böylelikle birinci listeden seçilen kayıta ait alt kayıtlar ikinci nesnede listelenmiş olacak
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        ///
        ///{
        ///  "parametre_id": 3,    seçim yapıldığında parametre_id değeri kullanılacak
        ///  "parametre_adi": "Alt Grup 1",
        ///  "parametre": 2,
        ///  "kaynak_1_parametre_id": 1
        ///},
        ///{
        ///  "parametre_id": 4,   seçim yapıldığında parametre_id değeri kullanılacak
        ///  "parametre_adi": "TEST 1",
        ///  "parametre": 2,
        ///  "kaynak_1_parametre_id": 1
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/cariparametrelistesi")]
        public HttpResponseMessage PersonelParametreAnaGrup()
        {
            personelRepository = new PersonelRepository();
            personelparametre = personelRepository.PersonelRaporParametreleriTumunuListele();
            if (personelparametre != null)
            {
                var ozet = personelparametre.Select(o => new
                {
                    parametre_id = o.parametre_id,
                    parametre_adi = o.tanim,
                    parametre = o.parametre,
                    kaynak_1_parametre_id = o.kaynak_1_parametre_id,
                }).OrderBy(p => p.parametre_adi);
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// rapor parametreleri için seçilen parametreye ait alt parametre varsa o parametrelerin listesini döndürür
        /// burada 1nci nesneden bir kayıt seçildiği anda paramere_id alanının değeri alınacak ve ikinci nesne için bu metod çalıştırılacak bu metoda da parametre olarak verilecek  
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        ///{
        ///  "parametre_id": 3,
        ///  "parametre_adi": "Alt Grup 1",
        ///  "parametre": 2,
        ///  "kaynak_1_parametre_id": 1   
        ///},
        ///{
        ///  "parametre_id": 4,
        ///  "parametre_adi": "TEST 1",
        ///  "parametre": 2,
        ///  "kaynak_1_parametre_id": 1
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/cariparametreler/{kaynak_1_parametre_id}")]
        public HttpResponseMessage PersonelParametreAnaGrup(int kaynak_1_parametre_id = 0)
        {
            personelRepository = new PersonelRepository();
            personelparametre = personelRepository.PersonelRaporParametreleri(kaynak_1_parametre_id);
            if (personelparametre != null)
            {
                var ozet = personelparametre.Select(o => new
                {
                    parametre_id = o.parametre_id,
                    parametre_adi = o.tanim,
                    parametre = o.parametre,
                    kaynak_1_parametre_id = o.kaynak_1_parametre_id,
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin parametre bilgilerini döndürür yani cari_parametre_1 ile cari_parametre_7 arasında
        /// carikart_id parametresini almaktadır
        /// </summary>
        /// /// <param name="carikart_id">carikart tablosunda carikart_id alanı</param>
        /// <returns>
        ///{
        ///  "carikart_id": 100000000100,
        ///  "cari_parametre_1": 2,
        ///  "cari_parametre_2": 3,
        ///  "cari_parametre_3": 4,
        ///  "cari_parametre_4": 0,
        ///  "cari_parametre_5": 0,
        ///  "cari_parametre_6": 0,
        ///  "cari_parametre_7": 0
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/parametre/{carikart_id}")]
        public HttpResponseMessage PersonelParametre(long carikart_id)
        {
            personelRepository = new PersonelRepository();
            var person = personelRepository.Personel_parametre_getir(carikart_id);
            if (person != null)
            {
                var per = new
                {
                    carikart_id = person.carikart_id,
                    cari_parametre_1 = person.cari_parametre_1,
                    cari_parametre_2 = person.cari_parametre_2,
                    cari_parametre_3 = person.cari_parametre_3,
                    cari_parametre_4 = person.cari_parametre_4,
                    cari_parametre_5 = person.cari_parametre_5,
                    cari_parametre_6 = person.cari_parametre_6,
                    cari_parametre_7 = person.cari_parametre_7,
                };
                return Request.CreateResponse(HttpStatusCode.OK, per);
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin parametre bilgilerini INSERT yapar
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/personel/parametre")]
        public HttpResponseMessage PersonelParametreinsert(Personel_Parametre_Model per)
        {
            AcekaResult acekaResult = null;
            if (per != null)
            {
                if (per.carikart_id.acekaToLong() > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", per.carikart_id);
                    fields.Add("cari_parametre_1", per.cari_parametre_1);
                    fields.Add("cari_parametre_2", per.cari_parametre_2);
                    fields.Add("cari_parametre_3", per.cari_parametre_3);
                    fields.Add("cari_parametre_4", per.cari_parametre_4);
                    fields.Add("cari_parametre_5", per.cari_parametre_5);
                    fields.Add("cari_parametre_6", per.cari_parametre_6);
                    fields.Add("cari_parametre_7", per.cari_parametre_7);
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NoContent, acekaResult);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin parametre bilgilerini UPDATE yapar
        /// carikart_id parametresini almaktadır
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/personel/parametre")]
        public HttpResponseMessage PersonelParametreput(Personel_Parametre_Model per)
        {
            AcekaResult acekaResult = null;
            if (per != null)
            {
                if (per.carikart_id.acekaToLong() > 0)
                {
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("carikart_id", per.carikart_id);
                    fields.Add("cari_parametre_1", per.cari_parametre_1);
                    fields.Add("cari_parametre_2", per.cari_parametre_2);
                    fields.Add("cari_parametre_3", per.cari_parametre_3);
                    fields.Add("cari_parametre_4", per.cari_parametre_4);
                    fields.Add("cari_parametre_5", per.cari_parametre_5);
                    fields.Add("cari_parametre_6", per.cari_parametre_6);
                    fields.Add("cari_parametre_7", per.cari_parametre_7);
                    acekaResult = CrudRepository.Update("carikart", "carikart_id", fields);
                    return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
                }
                else return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        #endregion

        #region personel iletişim bilgileri Liste-İnsert-Update

        /// <summary>
        /// personelin iletişim bilgilerini döndürür 
        /// carikart_id parametresini almaktadır
        /// </summary>
        /// /// <param name="carikart_id">carikart tablosunda carikart_id alanı</param>
        /// <returns>
        ///{
        ///  "carikart_adres_id": 100000000100,
        ///  "degistiren_carikart_id": -1,
        ///  "degistiren_tarih": "2017-02-07T22:10:02",
        ///  "kayit_silindi": false,
        ///  "statu": true,
        ///  "adres_tipi_id": "IF",
        ///  "carikart_id": 100000000100,
        ///  "adresunvan": "",
        ///  "postakodu": "",
        ///  "ulke_id": 90,
        ///  "sehir_id": 34,
        ///  "ilce_id": 419,
        ///  "semt_id": 1839,
        ///  "tel1": "",
        ///  "tel2": "",
        ///  "fax": "",
        ///  "email": "hasan@milteks.com.tr",
        ///  "websitesi": "",
        ///  "yetkili_ad_soyad": "",
        ///  "yetkili_tel": "",
        ///  "adres": ""
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/iletisim/{carikart_id}")]
        public HttpResponseMessage PersonelIletisimGetir(long carikart_id)
        {
            personelRepository = new PersonelRepository();
            var o = personelRepository.PersonelAdresBul(carikart_id);
            if (o != null)
            {
                var adres = new
                {
                    carikart_adres_id = o.carikart_adres_id,
                    degistiren_carikart_id = o.degistiren_carikart_id,
                    degistiren_tarih = o.degistiren_tarih,
                    kayit_silindi = o.kayit_silindi,
                    statu = o.statu,
                    adres_tipi_id = o.adres_tipi_id,
                    carikart_id = o.carikart_id,

                    postakodu = o.postakodu,
                    ulke_id = o.ulke_id,
                    sehir_id = o.sehir_id,
                    ilce_id = o.ilce_id,
                    semt_id = o.semt_id,

                    tel1 = o.tel1,
                    tel2 = o.tel2,
                    fax = o.fax,
                    email = o.email,
                    websitesi = o.websitesi,
                    yetkili_ad_soyad = o.yetkili_ad_soyad,
                    yetkili_tel = o.yetkili_tel,
                    adres = o.adres,


                };
                return Request.CreateResponse(HttpStatusCode.OK, adres);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personel iletişim bilgilerinin  INSERT methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/personel/iletisim")]
        public HttpResponseMessage PersonelIletisimPost(Personel_Carikart_genel_adres_Model per)
        {
            AcekaResult acekaResult = null;
            if (per != null)
            {
                per.adres_tipi_id = "II";
                per.statu = true;
                if (per.degistiren_carikart_id == 0) per.degistiren_carikart_id = Tools.PersonelId;
                acekaResult = CrudRepository<Personel_Carikart_genel_adres_Model>.Insert(per, "carikart_genel_adres", new string[] { "degistiren_tarih", "carikart_adres_id" });
                return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NoContent, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personel iletişim bilgilerinin UPDATE methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/personel/iletisim")]
        public HttpResponseMessage PersonelIletisimPut(Personel_Carikart_genel_adres_Model per)
        {
            AcekaResult acekaResult = null;
            if (per != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("carikart_adres_id", per.carikart_adres_id);
                fields.Add("carikart_id", per.carikart_id);
                fields.Add("degistiren_carikart_id", per.degistiren_carikart_id);
                fields.Add("kayit_silindi", per.kayit_silindi);
                fields.Add("statu", per.statu);
                fields.Add("adres_tipi_id", per.adres_tipi_id);
                fields.Add("postakodu", per.postakodu);
                fields.Add("adres", per.adres);
                if (per.adrestanim != null) fields.Add("adrestanim", per.adrestanim);
                if (per.adresunvan != null) fields.Add("adresunvan", per.adresunvan);
                fields.Add("ulke_id ", per.ulke_id);
                fields.Add("sehir_id", per.sehir_id);
                fields.Add("ilce_id ", per.ilce_id);
                fields.Add("semt_id", per.semt_id);
                if (per.vergidairesi != null) fields.Add("vergidairesi", per.vergidairesi);
                if (per.vergino != null) fields.Add("vergino", per.vergino);
                fields.Add("tel1", per.tel1);
                fields.Add("tel2", per.tel2);
                fields.Add("fax", per.fax);
                fields.Add("email", per.email);
                fields.Add("websitesi", per.websitesi);
                fields.Add("yetkili_ad_soyad", per.yetkili_ad_soyad);
                fields.Add("yetkili_tel", per.yetkili_tel);
                fields.Add("faturaadresi", per.faturaadresi);
                acekaResult = CrudRepository.Update("carikart_genel_adres", "carikart_adres_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin iletişim bilgilerinin DELETE methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/personel/iletisim")]
        public HttpResponseMessage PersonelIletisimDelete(Personel_Carikart_genel_adres_Model per)
        {
            AcekaResult acekaResult = null;
            if (per != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("carikart_adres_id", per.carikart_adres_id);
                fields.Add("carikart_id", per.carikart_id);
                fields.Add("degistiren_carikart_id", per.degistiren_carikart_id);
                fields.Add("kayit_silindi", 1);
                acekaResult = CrudRepository.Update("carikart_genel_adres", "carikart_adres_id", fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }


        #endregion

        #region personel çalışma yeri bilgileri Liste-İnsert-Update

        /// <summary>
        /// çalışma yeri tabı için görevlerin listesini döndürür
        /// </summary>
        /// <returns>
        /// Geriye döndürülen json object : 
        ///{
        ///[
        ///{
        ///        "parametre_id": 51,
        ///        "parametre_adi": "6 - Bebekolog (Full)"
        ///},
        ///{
        ///        "parametre_id": 52,
        ///        "parametre_adi": "5 - Takım Lideri"
        ///},
        ///]
        ///}
        /// </returns>
        [HttpGet]
        [Route("api/personel/gorev-parametreler")]
        public HttpResponseMessage PersonelParametreGorevGetir()
        {
            personelRepository = new PersonelRepository();
            parametre = personelRepository.PersonelParametreleri();

            if (parametre != null)
            {
                var ozet = parametre.Where(o => o.parametre_grup_id == "PRGOREV").Select(o => new
                {
                    parametre_id = o.parametre_id,
                    parametre_adi = o.parametre_adi,
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// çalışma yeri tabı için departmanların listesini döndürür
        /// </summary>
        /// <returns>
        /// { 
        ///"parametre_id": 5,
        ///"parametre_adi": "1000 - Mağaza Operasyon"
        /// }
        /// </returns>
        [HttpGet]
        [Route("api/personel/departman-parametreler")]
        public HttpResponseMessage PersonelParametreDepartmanGetir()
        {
            personelRepository = new PersonelRepository();
            parametre = personelRepository.PersonelParametreleri();

            if (parametre != null)
            {
                var ozet = parametre.Where(o => o.parametre_grup_id == "PRDEPART").Select(o => new
                {
                    parametre_id = o.parametre_id,
                    parametre_adi = o.parametre_adi,
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// çalışma yeri tabı için çalışma yerlerinin listesini döndürür
        /// </summary>
        /// <returns>
        ///{
        ///  "carikart_id": 100000001053,
        ///  "cari_unvan": "SATIN ALMA DEPOSU"
        ///},
        ///{
        ///  "carikart_id": 100000002270,
        ///  "cari_unvan": "SATIN ALMA DEP. 2"
        ///},
        ///{
        ///  "carikart_id": 100120000000,
        ///  "cari_unvan": "Merkez Ofis"
        ///}   
        /// </returns>
        [HttpGet]
        [Route("api/personel/calismayerleri")]
        public HttpResponseMessage PersonelCalismaYerleriGetir()
        {
            personelRepository = new PersonelRepository();
            personeller = personelRepository.PersonelCalismaYerleri();

            if (personeller != null)
            {
                var ozet = personeller.Select(o => new
                {
                    carikart_id = o.carikart_id,
                    cari_unvan = o.cari_unvan,
                });
                return Request.CreateResponse(HttpStatusCode.OK, ozet);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin çalışma yeri  bilgilerini döndürür 
        /// carikart_id parametresini almaktadır
        /// </summary>
        /// /// <param name="carikart_id">carikart tablosunda carikart_id alanı</param>
        /// <returns>
        /// [
        /// {
        ///{
        ///  "carikart_id": 100000000100,
        ///  "stokyeri_carikart_id": 100000001040,
        ///  "stokyeri_carikart_adi": "MERKEZ DEPO",
        ///  "departman_id": 45,
        ///  "departman_adi": "7000 - Eticaret",
        ///  "gorev_id": 60,
        ///  "gorev_adi": "5 - Uzman"
        ///}
        ///}
        ///]
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/personel/calismayeri/{carikart_id}")]
        public HttpResponseMessage Personel_CalismaYerleri(long carikart_id)
        {
            personelRepository = new PersonelRepository();
            personel_calisma_yerleri = personelRepository.Personel_Calisma_Yerleri_Getir(carikart_id);
            if (personel_calisma_yerleri != null)
            {

                var per = personel_calisma_yerleri.Select(o => new
                {
                    carikart_id = o.carikart_id,
                    stokyeri_carikart_id = o.stokyeri_carikart_id,
                    stokyeri_carikart_adi = o.stokyeri_carikart_adi,
                    departman_id = o.departman_id,
                    departman_adi = o.departman_adi,
                    gorev_id = o.gorev_id,
                    gorev_adi = o.gorev_adi,

                });
                /*
                 * departmanlar = o.departmanlar.Select(d => new
                    {
                        departman_id = d.parametre_id,
                        departman_adi = d.parametre_adi,
                    })
                     gorevler = o.gorevler.Select(g => new
                    {
                        gorev_id = g.parametre_id,
                        gorev_adi = g.parametre_adi,
                    }),
                    */
                return Request.CreateResponse(HttpStatusCode.OK, per);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record" });
            }
        }

        /// <summary>
        /// personelin çalışma yeri bilgilerinin  INSERT methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/personel/calismayeri")]
        public HttpResponseMessage Personel_CalismaYerleri(PersonelCariKartCalismaYeriModel per)
        {
            AcekaResult acekaResult = null;

            if (per != null)
            {
                acekaResult = CrudRepository<PersonelCariKartCalismaYeriModel>.Insert(per, "carikart_personel_calisma_yerleri", new string[] { "degistiren_tarih", "gorev_adi", "departman_adi", "stokyeri_carikart_adi" });
                //return Request.CreateResponse(HttpStatusCode.OK, acekaResult);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// personelin çalışma yeri bilgilerinin  UPDATE methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/personel/calismayeri")]
        public HttpResponseMessage PersonelCalismaYerleriUpdate(PersonelCariKartCalismaYeriModel per)
        {
            AcekaResult acekaResult = null;

            if (per != null)
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("stokyeri_carikart_id", per.stokyeri_carikart_id);
                fields.Add("carikart_id", per.carikart_id);
                fields.Add("degistiren_carikart_id", per.degistiren_carikart_id);
                fields.Add("gorev_id", per.gorev_id);
                fields.Add("departman_id", per.departman_id);
                acekaResult = CrudRepository.Update("carikart_personel_calisma_yerleri", new string[] { "carikart_id" }, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// personelin çalışma yeri bilgilerinin DELETE methodu
        /// </summary>
        /// /// <param name="per"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/personel/calismayeri")]
        public HttpResponseMessage PersonelCalismaYerleriDelete(PersonelCariKartCalismaYeriModel per)
        {
            AcekaResult acekaResult = null;

            if (per.carikart_id > 0 && per.stokyeri_carikart_id > 0)
            {

                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("stokyeri_carikart_id", per.stokyeri_carikart_id);
                fields.Add("carikart_id", per.carikart_id);

                acekaResult = CrudRepository.Delete("carikart_personel_calisma_yerleri", new string[] { "stokyeri_carikart_id", "carikart_id" }, fields);
                return Request.CreateResponse(HttpStatusCode.OK, new Models.AnonymousModels.Successful { message = "successful" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, new Models.AnonymousModels.NotFound { message = "No Record!" });
            }
        }
        #endregion

        #endregion
    }
}
