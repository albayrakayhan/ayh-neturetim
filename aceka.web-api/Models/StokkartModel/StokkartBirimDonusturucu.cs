using Newtonsoft.Json;
using System;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart birim dönüştürücü modeli
    /// </summary>
    public class StokkartBirimDonusturucu
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long stokkart_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// /// <summary>
        /// Zorunlu
        /// </summary>
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> birim1x { get; set; }
        
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> birim2x { get; set; }
        
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> birim3x { get; set; }
        
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> M2_gram { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> eni { get; set; }
        
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> fyne { get; set; }
        
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<double> fire_orani { get; set; }
    }
}