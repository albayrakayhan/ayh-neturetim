using aceka.infrastructure.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.CarikartModels
{
    public class DepokartPostModel
    {

        /// <summary>
        /// POST işlemlerinde Front End in modele eklemeyeceği alanlar ignore ediliyor!
        /// </summary>
        public class Depokart
        {
            public Int64 carikart_id { get; set; }
            public string cari_unvan { get; set; }
            public byte carikart_turu_id { get; set; }
            public byte carikart_tipi_id { get; set; }

            public string ozel_kod { get; set; }
            public string fiyattipi { get; set; }

            //public Int64 bagli_stokyeri_id { get; set; }
            public Int64 ana_carikart_id { get; set; }
            public Int64 transfer_depo_id { get; set; }
            public bool statu { get; set; }
            public Int64 sube_carikart_id { get; set; }
            public Int16 giz_yazilim_kodu { get; set; }
            public bool kayit_silindi { get; set; }


            [JsonIgnore]
            [ScriptIgnore]
            public DateTime degistiren_tarih { get; set; }
            public Int64 degistiren_carikart_id { get; set; }

            //Tablo
            //carikart_earsiv tablosu
            public string earsiv_seri { get; set; }
            //Tablo
            //carikart_efatura tablosu
            public string efatura_seri { get; set; }

            //Tablo
            //carikart_muhasebe tablosu
            public string carikart_muhasebe { get; set; }
            //Tablo
            //muhasebe_tanim_masrafmerkezleri tablosu
            public string masraf_merkezi { get; set; }
            public Int16 masraf_merkezi_id { get; set; }
            public string muh_kod { get; set; }

            //Tablo
            //carikart_stokyeri tablosu
            public long carikart_stokyeri { get; set; }
            public Nullable<DateTime> acilis_tarihi { get; set; }
            public Nullable<DateTime> kapanis_tarihi { get; set; }
            public Nullable<bool> kapali { get; set; }
            public Nullable<bool> transfer_depo_kullan { get; set; }

            //Tablo
            //giz_sirket
            public byte sirket_id { get; set; }
            public string sirket_adi { get; set; }
            public string sirket_unvan { get; set; }
        }

        public class carikartgenel_adres
        {
            public long carikart_adres_id { get; set; }
            public long degistiren_carikart_id { get; set; }
            public DateTime degistiren_tarih { get; set; }
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
            public string vergino { get; set; }
            public string tel1 { get; set; }
            public string tel2 { get; set; }
            public string fax { get; set; }
            public string email { get; set; }
            public string websitesi { get; set; }
            public string yetkili_ad_soyad { get; set; }
            public string yetkili_tel { get; set; }
            public bool faturaadresi { get; set; }

            /// <summary>
            /// Adnan TÜRK. 23.01.2017
            /// Model dönebilmek için eklendi.
            /// </summary>
            public parametre_ulke ulke { get; set; }
            public parametre_ulke_sehir ulke_sehir { get; set; }
            public parametre_ulke_sehir_ilce ulke_sehir_ilce { get; set; }
            public parametre_ulke_sehir_ilce_semt ulke_sehir_ilce_semt { get; set; }
        }

        public class carikartfiyat_tipi
        {
            /// <summary>
            /// Zorunlu
            /// </summary>
            public long carikart_id { get; set; }

            /// <summary>
            /// Zorunlu
            /// </summary>
            public string fiyattipi { get; set; }

            /// <summary>
            /// Zorunlu
            /// </summary>
            public long degistiren_carikart_id { get; set; }

            [JsonIgnore]
            [ScriptIgnore]
            public DateTime degistiren_tarih { get; set; }

            //[JsonIgnore]
            //[ScriptIgnore]
            public bool kayit_silindi { get; set; }
            /// <summary>
            /// Opsiyonel
            /// </summary>
            public bool statu { get; set; }

            /// <summary>
            /// Zorunlu
            /// </summary>
            public bool varsayilan { get; set; }

        }
    }
       

     


    
}