using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.CarikartModels
{
    /// <summary>
    /// Carikart arama parametreleri
    /// </summary>
    public class AramaParameters
    {
        /// <summary>
        /// Carikart Tamımlayıcı Kimlik
        /// </summary>
        public long carikart_id { get; set; }
        /// <summary>
        /// Carikart Ünvan
        /// </summary>
        public string unvan { get; set; }
        /// <summary>
        /// Carikart Özel Kod
        /// </summary>
        public string ozel_kod { get; set; }

        /// <summary>
        /// Carikart Tip Bilgisi
        /// </summary>
        public byte carikart_tipi_id { get; set; }
        
    }
}