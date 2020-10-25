using System;
using System.Collections.Generic;
using System.Linq;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartIlkmaddePivotModel
    {
        //[JsonIgnore]
        //[ScriptIgnore]

        public long stokkart_id { get; set; }
        public short sira_id { get; set; }
        public string modelyeri { get; set; }
        public string aciklama { get; set; }
        public string stok_adi { get; set; }
        public int? renk_id { get; set; }
        public string renk_adi { get; set; }

        public byte talimatturu_id { get; set; }
        public string talimat_tanim { get; set; }
        public long? alt_stokkart_id { get; set; }

        public float? genel { get; set; }

        public byte? birim_id { get; set; }
        public string birim_adi { get; set; }
        public Nullable<float> miktar { get; set; }

        public byte? birim_id3 { get; set; }
        public string birim_adi3 { get; set; }
        public Nullable<float> miktar3 { get; set; }
        public bool birim_id_2_zorunlu { get; set; }
        public bool birim_id_3_zorunlu { get; set; }

        public byte ana_kayit { get; set; }
        public Nullable<byte> sira { get; set; }

        public List<StokkartIlkmaddePivotMatrix> pivotMatrixData { get; set; }
    }

    public class StokkartIlkmaddePivotMatrix
    {
        public int? olcu_id { get; set; }
        public short id { get; set; }
        public string name { get; set; }
        public float? value { get; set; }
    }
}