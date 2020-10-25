using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartFiyatParameter
    {
        /// <summary>
        /// Stokkart fiyat stokkart No
        /// </summary>
        public long stokkart_id { get; set; }

        /// <summary>
        /// Stokkart fiyat fiyattipi
        /// </summary>
        public string fiyattipi { get; set; }
        /// <summary>
        /// Stokkart fiyat tarih
        /// </summary>
        public DateTime tarih { get; set; }
    }
}