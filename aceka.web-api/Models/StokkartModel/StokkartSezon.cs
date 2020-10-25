using System;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{

    public class StokkartSezon
    {
        public long stokkart_id { get; set; }
        public short sezon_id { get; set; }
        public string sezon_kodu { get; set; }
        public string sezon_adi { get; set; }
        public bool statu { get; set; }

        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public bool kayit_silindi { get; set; }
    }
}