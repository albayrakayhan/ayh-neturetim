using aceka.web_api.Models.ParametreModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stokkart a eklenecek ekler
    /// </summary>
    public class StokkartEkler : Ekler
    {

        /// <summary>
        /// Zorunlu
        /// </summary>
        public long stokkart_id { get; set; }

    }
}