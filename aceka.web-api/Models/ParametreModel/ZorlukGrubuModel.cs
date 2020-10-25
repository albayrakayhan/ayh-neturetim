using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.ParametreModel
{
    public class ZorlukGrubuModel
    {
    }
    public class Zorlukgrubu
    {
        public int zorlukgrubu_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
    }

    public class planlama_zorlukgrubu_model
    {
        public byte zorlukgrubu_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public DateTime degistiren_tarih { get; set; }
        public Nullable<short> kesimfire { get; set; }
        public Nullable<short> kesimfazla { get; set; }
        public Nullable<short> musterifazla { get; set; }
        public byte bedenbazinda { get; set; }
    }
    public class planlama_zorlukgrubu_oranlari_Model
    {
        public byte zorlukgrubu_id { get; set; }
        public short sira { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }
        public int altseviye { get; set; }
        public int ustseviye { get; set; }
        public short kesimfire { get; set; }
        public short kesimfazla { get; set; }
    }
    public class kalite_kontrol_oranlari_model
    {
        public string kalite_kontrol_kod { get; set; }
        public byte stokkart_tipi_id { get; set; }
        public double adet { get; set; }
        public int sira_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<double> kontrol_miktar { get; set; }
        public Nullable<double> red_miktar { get; set; }
        public bool miktarlar_oran_mi { get; set; }
    }
}