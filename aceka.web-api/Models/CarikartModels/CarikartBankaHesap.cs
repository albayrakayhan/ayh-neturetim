
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.CarikartModels
{
    public class CarikartBankaHesap
    {
        public long carikart_banka_id { get; set; }
        
        /// <summary>
        /// Zorunlu alan
        /// </summary>
        public long carikart_id { get; set; }

        //[JsonIgnore]
        //[ScriptIgnore]
        public bool kayit_silindi { get; set; }

        /// <summary>
        /// Zorunlu alan
        /// </summary>
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        public Nullable<short> banka_id { get; set; }
        public Nullable<short> banka_sube_id { get; set; }
        public string ibanno { get; set; }
        public string pb { get; set; }
        public Nullable<bool> ebanka { get; set; }
        public Nullable<bool> odemehesabi { get; set; }
        public Nullable<decimal> kredi_limiti_dbs { get; set; }
    }
}