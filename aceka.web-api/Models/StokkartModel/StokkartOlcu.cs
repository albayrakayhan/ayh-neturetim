using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart Ölçü tablosu POST, PUT, DELETE Model
    /// </summary>
    public class StokkartOlcu
    {
        public int olcu_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public long stokkart_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public string olcuyeri { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public short beden_id { get; set; }

        public string beden_adi { get; set; }

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
        public System.DateTime degistiren_tarih { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public float? deger { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte birim_id { get; set; }

        public string birim_adi { get; set; }

    }
}