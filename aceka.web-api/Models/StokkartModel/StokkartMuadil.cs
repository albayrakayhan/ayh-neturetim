using System;
using Newtonsoft.Json;
using System.Web.Script.Serialization;


namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart Muadil Model
    /// </summary>
    public class StokkartMuadil
    {
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long stokkart_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long muadil_stokkart_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// Yapılan işlemin tarihi
        /// </summary>        
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }


        /// <summary>
        /// GET için kullanılır. POST işlemleri için zorunlu değildir!
        /// </summary>
        public string tanim { get; set; }
    }
}