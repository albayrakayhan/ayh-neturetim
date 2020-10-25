using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{

    public class finans_tanim_odemeplani
    {
        public int odeme_plani_id { get; set; }
        public bool kayit_silindi { get; set; }
        public long degistiren_carikodu { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool statu { get; set; }
        public string odeme_plani_kodu { get; set; }
        public string odeme_plani_adi { get; set; }
        public long banka_hesap_id { get; set; }
    }

    public class finans_tanim_odemeplani_detay
    {
        public int odeme_plani_id { get; set; }
        public byte sira { get; set; }
        public long degistiren_carikodu { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tutar_alani { get; set; }
        public Nullable<double> yuzde_orani { get; set; }
        public Nullable<double> sabit_tutar { get; set; }
        public string vade_turu { get; set; }
        public Nullable<double> vade_degeri { get; set; }
        public string fatura_tarihi_turu { get; set; }
        public string odeme_sekli { get; set; }
        public Nullable<double> odeme_yeri_id { get; set; }
        public Nullable<double> banka_hesap_id { get; set; }
        public Nullable<byte> og1 { get; set; }
        public Nullable<byte> og2 { get; set; }
        public Nullable<byte> og3 { get; set; }
        public Nullable<byte> og4 { get; set; }
        public Nullable<byte> og5 { get; set; }
        public Nullable<byte> og6 { get; set; }
        public Nullable<byte> og7 { get; set; }
        public Nullable<byte> og8 { get; set; }
        public Nullable<byte> og9 { get; set; }
        public Nullable<byte> og10 { get; set; }
        public Nullable<byte> og11 { get; set; }
        public Nullable<byte> og12 { get; set; }
        public Nullable<byte> og13 { get; set; }
        public Nullable<byte> og14 { get; set; }
        public Nullable<byte> og15 { get; set; }
        public Nullable<byte> og16 { get; set; }
        public Nullable<byte> og17 { get; set; }
        public Nullable<byte> og18 { get; set; }
        public Nullable<byte> og19 { get; set; }
        public Nullable<byte> og20 { get; set; }
        public Nullable<byte> og21 { get; set; }
        public Nullable<byte> og22 { get; set; }
        public Nullable<byte> og23 { get; set; }
        public Nullable<byte> og24 { get; set; }
        public Nullable<byte> og25 { get; set; }
        public Nullable<byte> og26 { get; set; }
        public Nullable<byte> og27 { get; set; }
        public Nullable<byte> og28 { get; set; }
        public Nullable<byte> og29 { get; set; }
        public Nullable<byte> og30 { get; set; }
        public Nullable<byte> og31 { get; set; }
        public string vade_odeme_gunu { get; set; }
    }
}
