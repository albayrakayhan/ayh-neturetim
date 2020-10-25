using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.ParametreModel
{
    public class StokkartRaporParametre
    {
        public string parametre_adi { get; set; }

        public List<StokkartRaporParametreler> stokkartRaporParametreler { get; set; }
    }

    public class StokkartRaporParametreler
    {
        public byte parametre_grubu { get; set; }
        public byte parametre { get; set; }
        public string kod { get; set; }
        public int parametre_id { get; set; }        
        public string tanim { get; set; }
        public string parametre_adi { get; set; }
       
    }
}