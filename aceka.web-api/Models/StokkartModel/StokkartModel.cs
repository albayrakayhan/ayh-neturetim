using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{

    public class StokkartUpdate : Stokkart
    {
        public long stokkart_id { get; set; }
    }

    public class Stokkart
    {
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        public bool statu { get; set; }
        public byte stokkart_tipi_id { get; set; }

        //[JsonIgnore]
        //[ScriptIgnore]
        public short stokkart_tur_id { get; set; }

        public string stok_kodu { get; set; }
        public string stok_adi { get; set; }
        public Nullable<byte> kdv_alis_id { get; set; }
        public Nullable<byte> kdv_satis_id { get; set; }
        public byte birim_id_1 { get; set; }
        public Nullable<byte> birim_id_2 { get; set; }
        public Nullable<byte> birim_id_3 { get; set; }
        public bool birim_id_2_zorunlu { get; set; }
        public bool birim_id_3_zorunlu { get; set; }

        public _Talimat talimat { get; set; }
        public Stok_Talimat stok_talimat { get; set; }
        public Stokkart_Ozel stokkart_ozel { get; set; }
        public Stokkart_onay stokkart_onay { get; set; }
        public Stokkart_onay_log stokkart_onay_log { get; set; }
        public gizsabit_stokkarttipi gizsabit_stokkarttipi { get; set; }
        public short uretimyeri_id { get; set; }
    }

    public class _Talimat
    {
        public int talimatturu_id { get; set; }
        public string kod { get; set; }
    }

    public class Stok_Talimat
    {
        public string aciklama { get; set; }
    }

    public class Stokkart_Ozel
    {
        [JsonIgnore]
        [ScriptIgnore]
        public long stokkart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }
        public string stok_adi_uzun { get; set; }
        public string orjinal_stok_kodu { get; set; }
        public string orjinal_stok_adi { get; set; }
        public string orjinal_renk_kodu { get; set; }
        public string orjinal_renk_adi { get; set; }
        public Nullable<bool> tek_varyant { get; set; }
    }

    public partial class stokkartfiyat
    {
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public long stokkart_id { get; set; }
        public string fiyattipi { get; set; }
        public DateTime tarih { get; set; }
        public decimal fiyat { get; set; }
        public int pb { get; set; }
        public string fiyattipi_adi { get; set; }
        public string pb_adi { get; set; }
        public bool kayit_silindi { get; set; }
    }

    public class Stokkart_onay
    {
        //[JsonIgnore]
        //[ScriptIgnore]
        public long stokkart_id { get; set; }
        public bool genel_onay { get; set; }
        public bool malzeme_onay { get; set; }
        public bool yukleme_onay { get; set; }
        public bool uretim_onay { get; set; }
    }

    public class Stokkart_onay_log
    {
        public long stokkart_id { get; set; }
        public string onay_alan_adi { get; set; }
        public DateTime onay_tarihi { get; set; }
        public Nullable<long> onay_carikart_id { get; set; }
        public Nullable<DateTime> iptal_tarihi { get; set; }
        public Nullable<long> iptal_carikart_id { get; set; }
    }

    public class gizsabit_stokkarttipi
    {
        public byte stokkarttipi { get; set; }
        public string tanim { get; set; }
        public string otostokkodu { get; set; }
        public byte parametre_grubu { get; set; }
        public byte stokkartturu { get; set; }
    }

    public partial class parametrebeden
    {
        public int beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string bedengrubu { get; set; }
        public string beden { get; set; }
        public string beden_tanimi { get; set; }
        public int sira { get; set; }
    }

}