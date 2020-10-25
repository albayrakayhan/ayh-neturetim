using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart varyant POST, PUT, DELETE işlemleri için kullanılacak
    /// </summary>
    public class StokkartVaryant
    {
        public long sku_id { get; set; }
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public bool kayit_silindi { get; set; }

        public bool statu { get; set; }
        public string sku_no { get; set; }
        public long stokkart_id { get; set; }

        public StokkartVaryant_Renk renk { get; set; }
    }
    /// <summary>
    /// Varyant ek object
    /// </summary>
    public class StokkartVaryant_Renk
    {
        public long renk_id { get; set; }
        public string renk_kodu { get; set; }
        public string renk_adi { get; set; }
    }

}