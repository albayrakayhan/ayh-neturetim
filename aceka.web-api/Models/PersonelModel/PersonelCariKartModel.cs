using aceka.infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.PersonelModel
{

    public class PersonelCariKartCalismaYeriListele
    {
        public long carikart_id { get; set; }
    }
    public class PersonelCariKartCalismaYeriModel
    {
        public long carikart_id { get; set; }
        public long stokyeri_carikart_id { get; set; }
        public string stokyeri_carikart_adi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int gorev_id { get; set; }
        public string gorev_adi { get; set; }
        public int departman_id { get; set; }
        public string departman_adi { get; set; }
    }

    public class Personel_Carikart_genel_adres_Listele_Model
    {
        public long carikart_id { get; set; }
    }
    public class Personel_Carikart_genel_adres_Model
    {
        public long carikart_adres_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string adres_tipi_id { get; set; }
        public long carikart_id { get; set; }
        public string adrestanim { get; set; }
        public string adresunvan { get; set; }
        public string adres { get; set; }
        public string postakodu { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public Nullable<int> semt_id { get; set; }
        public string vergidairesi { get; set; }
        public string vergino       { get; set; }
        public string tel1          { get; set; }
        public string tel2          { get; set; }
        public string fax           { get; set; }
        public string email         { get; set; }
        public string websitesi         { get; set; }
        public string yetkili_ad_soyad { get; set; }
        public string yetkili_tel        { get; set; }
        public bool   faturaadresi         { get; set; }
    }

    public class Personel_Liste_Model
    {
        public long carikart_id { get; set; }
        public string cari_unvan { get; set; }
    }
    public class Personel_Model
    {
        public long carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public int carikart_turu_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public int carikart_tipi_id { get; set; }

        public string cari_unvan { get; set; }
        public string ozel_kod { get; set; }
        public string fiyattipi { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public Int16 giz_yazilim_kodu { get; set; }

        public long sube_carikart_id { get; set; }

        public string giz_kullanici_adi { get; set; }
        public string giz_kullanici_sifre { get; set; }
        public carikart_muhasebe_personel_Model muh_masraf { get; set; }


        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        public Personel_Carikart_genel_adres_Model iletisim { get; set; }
        public Personel_Parametre_Model parametre { get; set; }
    }
    public class carikart_muhasebe_personel_Model
    {
        public Int64 carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public int sirket_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public string sirket_adi { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public int sene { get; set; }


        public string muh_kod { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public string muh_kod_adi { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public int masraf_merkezi_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public string masraf_merkezi_adi { get; set; }

    }

    public class Personel_Parametre_Listele_Model
    {
        public long carikart_id { get; set; }
        public int cari_parametre_1 { get; set; }
        public int cari_parametre_2 { get; set; }
        public int cari_parametre_3 { get; set; }
        public int cari_parametre_4 { get; set; }
        public int cari_parametre_5 { get; set; }
        public int cari_parametre_6 { get; set; }
        public int cari_parametre_7 { get; set; }
    }
    public class Personel_Parametre_Model
    {
        public long carikart_id { get; set; }
        public int cari_parametre_1 { get; set; }
        public int cari_parametre_2 { get; set; }
        public int cari_parametre_3 { get; set; }
        public int cari_parametre_4 { get; set; }
        public int cari_parametre_5 { get; set; }
        public int cari_parametre_6 { get; set; }
        public int cari_parametre_7 { get; set; }
    }
}