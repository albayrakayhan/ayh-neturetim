using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.CarikartModels
{
    public class CariSube
    {
        public Int64 carikart_id { get; set; }
        public Int64 ana_carikart_id { get; set; }
        public Int64 degistiren_carikart_id { get; set; }
        public bool kayit_silindi { get; set; }
        public string cari_unvan { get; set; }
    }
}