using Newtonsoft.Json;
using System;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.ParametreModel
{
    /// <summary>
    /// Kartlar için ekleme class ı
    /// </summary>
    public class Ekler
    {
        /// <summary>
        /// Delete işlemi için gerekli
        /// </summary>
        public int ek_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        /// <summary>
        /// İşlem tarihi
        /// </summary>
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public short ekturu_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public string ekadi { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string aciklama { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string filepath { get; set; }

        /// <summary>
        /// Opsiyonel
        /// </summary>
        public string filename { get; set; }

    }
}