using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class Kur
    {
        public DateTime tarih { get; set; }
        public string pb { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public double prog_alis { get; set; }
        public double prog_satis { get; set; }
        public double mb_alis { get; set; }
        public double mb_satis { get; set; }
        public double ser_alis { get; set; }
        public double ser_satis { get; set; }

        //sene ve aya göre select yapacağımızda eklendi
        public string  sene_adi { get; set; }
        public string ay_adi { get; set; }
        public string ay_no { get; set; }


    }
}
