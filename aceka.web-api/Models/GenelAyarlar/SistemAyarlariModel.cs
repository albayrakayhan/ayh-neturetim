using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.GenelAyarlar.SistemModel
{
    public class SistemAyarlariModel
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
}