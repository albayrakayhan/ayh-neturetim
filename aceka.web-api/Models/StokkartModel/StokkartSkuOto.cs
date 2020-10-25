using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartSku
    {
        public long sku_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string sku_no { get; set; }
        public long stokkart_id { get; set; }
        public long renk_id { get; set; }
        public int beden_id { get; set; }
        public Nullable<bool> asorti { get; set; }
        public Nullable<double> asorti_miktar { get; set; }

        //public List<StokkartSku> stokkartSku { get; set; }
    }

    public class StokkartSkuOto
    {
        public long stokkart_id { get; set; }

        public short sku_oto_field_id { get; set; }

        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }

        public byte sira_id { get; set; }

        public bool secili { get; set; }
        public string tanim { get; set; }

        public Stokkartozel stokkart_ozel { get; set; }
    }

    public class Stokkarttekvaryant
    {
        public bool? tek_varyant { get; set; }

        public List<StokkartSkuOto> stokkartSkuOto { get; set; }
        public List<StokkartSku> stokkartSku { get; set; }

    }

    /// <summary>
    /// Varyant ek object
    /// </summary>
    public class Stokkartozel
    {
        public long stokkart_id { get; set; }
    }
}