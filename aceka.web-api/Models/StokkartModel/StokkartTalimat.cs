using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart Talimat POST, PUT ve DELETE Model
    /// </summary>
    public class StokkartTalimat
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long stokkart_id { get; set; }

        /// <summary>
        /// PUT, POST, DELETE işlemleri için Zorunlu
        /// </summary>
        public short sira_id { get; set; }

        /// <summary>
        /// PUT işlemleride swap olarak kullanılacak.
        /// </summary>
        public short eski_sira_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public int talimatturu_id { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<long> fasoncu_carikart_id { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string aciklama { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string irstalimat { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<short> islem_sayisi { get; set; }        
    }
}