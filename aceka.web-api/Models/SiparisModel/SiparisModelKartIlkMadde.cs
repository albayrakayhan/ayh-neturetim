using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace aceka.web_api.Models.SiparisModel
{
    /// <summary>
    /// Sipariş - > Tab - > İlk madde model
    /// </summary>
    public class SiparisModelKartIlkMadde
    {
        /// <summary>
        /// Modelkart id
        /// </summary>
        public long siparis_id { get; set; }

        /// <summary>
        /// POST işleminde boş bırakılacak. PUT ve DELETE metodlarında gönderilmeli
        /// Aynı stokkart_id kayıtlarından kaçıncı olduğunu MAX ile bulup manuel vereceğiz, 
        /// </summary>
        public short sira_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public short beden_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public long degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
        public System.DateTime degistiren_tarih { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte talimatturu_id { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public string modelyeri { get; set; }
        /// <summary>
        /// Seçilen stokkart_id. (İlk Madde listesinden gelen)
        /// </summary>

        public long alt_stokkart_id { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public int renk_id { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<byte> sira { get; set; }

        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte ana_kayit { get; set; }
        /// <summary>
        /// opsiyonel
        /// </summary>
        public string aciklama { get; set; }
        /// <summary>
        /// Zorunlu
        /// </summary>
        public byte birim_id { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<byte> birim_id3 { get; set; }
        /// <summary>
        /// Opsiyonel
        /// </summary>
        public Nullable<float> miktar { get; set; }
        /// <summary>
        /// opsiyonel
        /// </summary>
        public Nullable<float> miktar3 { get; set; }
    }
}