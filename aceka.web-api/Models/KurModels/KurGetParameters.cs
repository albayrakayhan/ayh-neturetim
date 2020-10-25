using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.KurModels
{
    /// <summary>
    /// Kur listesi için gerekli parametreler
    /// </summary>
    public class KurGetParameters
    {
        /// <summary>
        /// Listelenecek tarih parametresi
        /// </summary>
        public DateTime tarih { get; set; }

        /// <summary>
        /// Listelenmek istenen kur'un tipi
        /// Currency
        /// </summary>
        public string pb { get; set; }
    }
}