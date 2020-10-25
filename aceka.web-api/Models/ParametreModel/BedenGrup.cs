using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.ParametreModel
{
    public class BedenGrup
    {
        public int beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public bool kayit_silindi { get; set; }
        public string bedengrubu { get; set; }
        public string beden { get; set; }
        public string beden_tanimi { get; set; }
        public int sira { get; set; }

        public string GrupAdi { get; set; }
    }
}