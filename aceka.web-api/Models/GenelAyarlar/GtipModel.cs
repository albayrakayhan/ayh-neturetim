using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.GenelAyarlar.GtipModel
{
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
        public byte stokkart_tipi_id { get; set; }
        public int stokalan_id_1 { get; set; }
        public int stokalan_id_2 { get; set; }
        public int stokalan_id_3 { get; set; }
        public int stokalan_id_4 { get; set; }
        public string gtip_genel { get; set; }
        public string gtip_bayan { get; set; }
        public string aciklama { get; set; }
        public int birim { get; set; }
        public long? adet { get; set; }
        public int? kg { get; set; }
        public Single? birim_fob { get; set; }
        public Single? toplam_fob { get; set; }
        public string pb { get; set; }
        public string birim_adi { get; set; }

    }
}