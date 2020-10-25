using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartRaporParametreler
    {
        public long stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }

        public Nullable<long> satici_carikart_id { get; set; }
        public short uretimyeri_id { get; set; }

        public Nullable<int> stokalan_id_1 { get; set; }
        public Nullable<int> stokalan_id_2 { get; set; }
        public Nullable<int> stokalan_id_3 { get; set; }
        public Nullable<int> stokalan_id_4 { get; set; }
        public Nullable<int> stokalan_id_5 { get; set; }
        public Nullable<int> stokalan_id_6 { get; set; }
        public Nullable<int> stokalan_id_7 { get; set; }
        public Nullable<int> stokalan_id_8 { get; set; }
        public Nullable<int> stokalan_id_9 { get; set; }
        public Nullable<int> stokalan_id_10 { get; set; }
        public Nullable<int> stokalan_id_11 { get; set; }
        public Nullable<int> stokalan_id_12 { get; set; }
        public Nullable<int> stokalan_id_13 { get; set; }
        public Nullable<int> stokalan_id_14 { get; set; }
        public Nullable<int> stokalan_id_15 { get; set; }
        public Nullable<int> stokalan_id_16 { get; set; }
        public Nullable<int> stokalan_id_17 { get; set; }
        public Nullable<int> stokalan_id_18 { get; set; }
        public Nullable<int> stokalan_id_19 { get; set; }
        public Nullable<int> stokalan_id_20 { get; set; }
    }
}