using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartUyari
    {
        public long stokkart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public bool saticiya_siparis { get; set; }
        public bool musteri_siparisi_icin_acik { get; set; }
        public bool eksi_stok_izin { get; set; }
        public bool eksi_stok_uyari { get; set; }
        public bool min_stok_uyari { get; set; }
        public bool satin_alma_testi_gerekli_uyari { get; set; }
        public bool her_sezon_onay_gerekli { get; set; }
        public bool beden_bazli_kullanim { get; set; }
        public bool sezon_onayi_yok_uyarisi { get; set; }
        public bool tedarik_edilemez { get; set; }
    }
}