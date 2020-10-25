using aceka.web_api.Models.StokkartModel;
using System.Collections.Generic;

namespace aceka.web_api.Models.SiparisModel
{
    public class SiparisIlkmaddePivotModel
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public byte? sira { get; set; }
        public byte talimatturu_id { get; set; }
        public string talimat_tanim { get; set; }
        public long? alt_stokkart_id { get; set; }
        public string modelyeri { get; set; }
        public string aciklama { get; set; }
        public string stok_adi { get; set; }
        public int? renk_id { get; set; }
        public string renk_adi { get; set; }
        public byte? birim_id { get; set; }
        public float? miktar { get; set; }
        public string birim_adi { get; set; }
        public byte? birim_id3 { get; set; }
        public string birim_adi3 { get; set; }
        public List<StokkartIlkmaddePivotMatrix> pivotMatrixData { get; set; }
    }

}