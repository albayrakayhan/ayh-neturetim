using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class GenelAyarlar
    {
        public class takvim
        {
            public short sene { get; set; } // smallint, not null
            public byte ay { get; set; } // tinyint, not null
            public byte hafta { get; set; } // tinyint, not null
            public DateTime tarih { get; set; } // datetime, not null
            public byte gun { get; set; } // tinyint, not null
            public byte tatil_turu { get; set; } // string, not null
            public string aciklama { get; set; } // string, not null

            //Gün adını yazabilmek için eklendi.
            public string gun_adi { get; set; }
        }

        #region GTIP Belge
        public class gtip_belge
        {
            public int belge_id { get; set; }
            public long acan_carikart_id { get; set; }
            public DateTime acan_tarih { get; set; }
            public long degistiren_carikart_id { get; set; }
            public DateTime degistiren_tarih { get; set; }
            public long carikart_id { get; set; }
            public string belgeno { get; set; }
            public DateTime belge_tarihi { get; set; }
            public DateTime bitis_tarihi { get; set; }
            public string cari_unvan { get; set; }
            public gtip_belgedetay gtipdetay { get; set; }

        }

        public class gtip_belgedetay
        {
            public int belge_id { get; set; }
            public byte stokkart_tipi_id { get; set; }
            public int stokalan_id_1 { get; set; }
            public int stokalan_id_2 { get; set; }
            public int stokalan_id_3 { get; set; }
            public int stokalan_id_4 { get; set; }
            public long degistiren_carikart_id { get; set; }
            public DateTime degistiren_tarih { get; set; }
            public string gtip_genel { get; set; }
            public string gtip_bayan { get; set; }
            public string aciklama { get; set; }
            public DateTime acan_tarih { get; set; }
            public int birim { get; set; }
            public long? adet { get; set; }
            public int? kg { get; set; }
            public Single? birim_fob { get; set; }
            public Single? toplam_fob { get; set; }
            public string pb { get; set; }
            public string birim_adi { get; set; }
        }
        #endregion


        #region Sistem Ayarları
        public class SistemAyarlari
        {
            public string ayaradi { get; set; }
            public long degistiren_carikart_id { get; set; }
            public DateTime degistiren_tarih { get; set; }
            public string ayar { get; set; }
            public string ayaraciklama { get; set; }
            public string ayar_grubu { get; set; }
            public byte veri_turu { get; set; }
            public string combo_Sql { get; set; }
        }
        #endregion
    }
}
