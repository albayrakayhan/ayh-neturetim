using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.SiparisModel
{
    /// <summary>
    /// Sipariş Model
    /// </summary>
    public class Siparis
    {
        /// <summary>
        /// PUT ve DELETE işlemleri için Zorunlu
        /// </summary>
        public long siparis_id { get; set; }

        /// <summary>
        /// İşlem yapan cari
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// İşlem yapılan tarih
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        /// <summary>
        /// Kaydın silinip silinmediğinin bildirildiği değişken
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public bool kayit_silindi { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>

        public byte? statu { get; set; }

        /// <summary>
        /// Zorunlu
        /// Max Length = 10
        /// </summary>
        public string siparis_no { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> sirket_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [Required(ErrorMessage = "Lütfen Müşteri seçiniz")]
        public long? musteri_carikart_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<long> stokyeri_carikart_id { get; set; }

        /// <summary>
        /// Zorunlu Format DD.MM.YYYY
        /// </summary>
        public System.DateTime siparis_tarihi { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte siparisturu_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte zorlukgrubu_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> musterifazla { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string siparis_not { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> uretimyeri_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> mense_uretimyeri_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<byte> pb { get; set; }

        /// <summary>
        /// Sipariş tablosunun ilişkili olduğu tablo
        /// </summary>
        public Siparis_Ozel siparis_ozel { get; set; }

        //public parametresiparisturu parametre_siparisturu { get; set; }
        //public parametrezorlukgrubu parametre_zorlukgrubu { get; set; }
        //public infrastructure.Models.cari_kart musteri_carikart { get; set; }
        //public infrastructure.Models.cari_kart stokyeri_carikart { get; set; }
        //public siparisdetay siparis_detay { get; set; }
        //public siparisozel siparis_ozel { get; set; }
        //public siparisnotlar  siparis_notlar { get; set; }

    }

    /// <summary>
    ///  Sipariş tablosunun ilişkili olduğu tablo
    /// </summary>
    public class Siparis_Ozel
    {
        /// <summary>
        /// Sipariş tablosu ile ilişkili field0
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public long siparis_id { get; set; }

        /// <summary>
        /// Zorunlu alan. ilk sipariş kaydı için sira_id "0" olarak atanır. 
        /// Bu sayede Sipariş kaydının diğer detayları elde edilmiş olur.
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public short sira_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        /// <summary>
        /// Model No. Çoklu Seçim Olacak. Zorunlu
        /// </summary>
        public Nullable<long> stokkart_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<long> bayi_carikart_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public System.DateTime isteme_tarihi { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<System.DateTime> tahmini_uretim_tarihi { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> sezon_id { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<System.DateTime> tahmini_dikim_tarihi { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string ref_siparis_no { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string ref_siparis_no2 { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string ref_sistem_name { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<byte> ref_link_status { get; set; }
     
    }

    public class siparisdetay
    {
        public long siparis_id { get; set; }
        public long stokkart_id { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int adet { get; set; }
        public Nullable<decimal> birimfiyat { get; set; }
    }

    public class siparisnotlar
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public string aciklama { get; set; }
    }

    public class parametresiparisturu
    {
        public byte siparisturu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
        public bool genel { get; set; }
        public bool kalite2 { get; set; }
        public bool numune { get; set; }
    }

    public class parametrezorlukgrubu
    {
        public int zorlukgrubu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
    }


}