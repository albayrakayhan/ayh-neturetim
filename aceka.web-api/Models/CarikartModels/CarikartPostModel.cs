using aceka.infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace aceka.web_api.Models.CarikartModels
{
    /// <summary>
    /// POST işlemlerinde Front End in modele eklemeyeceği alanlar ignore ediliyor!
    /// </summary>
    public class Carikart
    {
        public long carikart_id { get; set; }
        public string cari_unvan { get; set; }
        public byte carikart_turu_id { get; set; }
        public byte carikart_tipi_id { get; set; }

        public string ozel_kod { get; set; }
        public string fiyattipi { get; set; }

        public Int64 ana_carikart_id { get; set; }
        public Int64 transfer_depo_id { get; set; }
        public bool statu { get; set; }
        public Int64 sube_carikart_id { get; set; }
        public long ilgili_sube_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public Int64 degistiren_carikart_id { get; set; }

        /// <summary>
        /// carikart_finans tablosu
        /// </summary>
        public long finans_sorumlu_carikart_id { get; set; }

        /// <summary>
        /// carikart_firma_ozel tablosu
        /// </summary>
        public long? satin_alma_sorumlu_carikart_id { get; set; }

        /// <summary>
        ///  carikart_firma_ozel tablosu
        /// </summary>
        public long? satis_sorumlu_carikart_id { get; set; }

        /// <summary>
        /// carikartrapor_parametre tablosu
        /// </summary>
        public int cari_parametre_1 { get; set; }
        public int cari_parametre_2 { get; set; }
        public int cari_parametre_3 { get; set; }
        public int cari_parametre_4 { get; set; }
        public int cari_parametre_5 { get; set; }
        public int cari_parametre_6 { get; set; }
        public int cari_parametre_7 { get; set; }
    }

    public class Carikart_IletisimBilgileri
    {
        public long carikart_id { get; set; }
        /// <summary>
        /// Bu ala char(2) FA ya da IA parametresini alır. Amacı varsayılan işetişim adres seçimini belirlemek.
        /// </summary>
        public string iletisim_adres_tipi_id { get; set; }

        public Adres fatura_adresi { get; set; }
        public Adres irsaliye_adresi { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public Adres finans_iletisim_adresi { get; set; }
    }

    public class Adres
    {
        [JsonIgnore]
        [ScriptIgnore]
        public long carikart_id { get; set; }
        public long carikart_adres_id { get; set; }
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public string adres_tipi_id { get; set; }


        public string adrestanim { get; set; }
        public string adresunvan { get; set; }
        public string adres { get; set; }
        public string postakodu { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public Nullable<int> semt_id { get; set; }
        public string vergidairesi { get; set; }
        public string vergino { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string websitesi { get; set; }
        public string yetkili_ad_soyad { get; set; }
        public string yetkili_tel { get; set; }

         
        [JsonIgnore]
        [ScriptIgnore]
        public bool faturaadresi { get; set; }

        public bool statu { get; set; }
    }
    public class Carikart_Finans_IletisimBilgileri
    {

        public long carikart_id { get; set; }
        public long carikart_adres_id { get; set; }
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        public string email { get; set; }
        public string websitesi { get; set; }
        public string yetkili_ad_soyad { get; set; }
        public string yetkili_tel { get; set; }
        public bool kayit_silindi { get; set; }
    }
    public class carikart_ozelalanlar
    {

        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public long? satin_alma_sorumlu_carikart_id { get; set; }

        public long? satis_sorumlu_carikart_id { get; set; }
        public DateTime baslamatarihi { get; set; }
        public string ozel { get; set; }
    }
    public class carikartelektronik_bilgilendirme
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public string irsaliye_eposta { get; set; }
        public string perakende_fatura_eposta { get; set; }
        public string toptan_fatura_eposta { get; set; }
        public string siparis_formu_eposta { get; set; }
        public string babs_formu_eposta { get; set; }
        public string cari_mutabakat_formu_eposta { get; set; }
        public string odeme_hatirlatma_eposta { get; set; }
    }
    public class carikartstokyeri
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<System.DateTime> acilis_tarihi { get; set; }
        public Nullable<System.DateTime> kapanis_tarihi { get; set; }
    }
    public class Carikart_Notlar
    {
        public long carikart_not_id { get; set; }
        public long carikart_id { get; set; }

        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }


        public string aciklama { get; set; }
        public string nereden { get; set; }
        public bool kayit_silindi { get; set; }
    }

    public class carikartrapor_parametre
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public Nullable<int> cari_parametre_1 { get; set; }
        public Nullable<int> cari_parametre_2 { get; set; }
        public Nullable<int> cari_parametre_3 { get; set; }
        public Nullable<int> cari_parametre_4 { get; set; }
        public Nullable<int> cari_parametre_5 { get; set; }
        public Nullable<int> cari_parametre_6 { get; set; }
        public Nullable<int> cari_parametre_7 { get; set; }
    }
}