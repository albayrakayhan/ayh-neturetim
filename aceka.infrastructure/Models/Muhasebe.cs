using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class Muhasebe
    {
        public class muhasebe_tanim_hesapkodlari
        {
            public int muh_kod_id { get; set; }
            public byte sirket_id { get; set; }
            public short sene { get; set; }
            public long degistiren_carikart_id { get; set; }
            public DateTime degistiren_tarih { get; set; }
            public bool kayit_silindi { get; set; }
            public bool statu { get; set; }
            public string muh_kod { get; set; }
            public string muh_kod_adi { get; set; }
            public Nullable<System.DateTime> kilit_tarihi { get; set; }
            public string aciklama { get; set; }
            public string birim { get; set; }
            public long stokkart_id { get; set; }
            public string bakiye_tipi { get; set; }
            public string bilanco_tipi { get; set; }
            public string pb { get; set; }
            public Nullable<byte> bilanco_tablosu { get; set; }
            public Nullable<byte> gelir_tablosu { get; set; }
            public Nullable<byte> gider_tablosu { get; set; }
            public Nullable<byte> nakit_tablosu { get; set; }
            public Nullable<byte> nazim_hesap { get; set; }
            public Nullable<byte> ozel_tablo { get; set; }
            public Nullable<byte> seviye { get; set; }
            public string seviye1 { get; set; }
            public string seviye2 { get; set; }
            public string seviye3 { get; set; }
            public string seviye4 { get; set; }
            public string seviye5 { get; set; }
            public string seviye6 { get; set; }
            public byte kdv_alis_id { get; set; }
            public byte kdv_satis_id { get; set; }
            public Nullable<byte> tevkifat_alis { get; set; }
            public Nullable<byte> tevkifat_satis { get; set; }
        }

        public class muhasebe_tanim_masrafmerkezleri
        {
            public int masraf_merkezi_id { get; set; }
            public long degistiren_carikart_id { get; set; }
            public System.DateTime degistiren_tarih { get; set; }
            public byte sirket_id { get; set; }
            public Nullable<int> ana_masraf_merkezi_id { get; set; }
            public string masraf_merkezi_kodu { get; set; }
            public string masraf_merkezi_adi { get; set; }
            public bool statu { get; set; }
            public string muhkod_ek { get; set; }
            public string grup_kodu { get; set; }
            public string grup_adi { get; set; }
            public bool kayit_silindi { get; set; }
            public Nullable<long> personel_carikart_id_1 { get; set; }
            public Nullable<long> stokyeri_carikart_id { get; set; }
        }
    }
}
