using System.Collections.Generic;

namespace aceka.web_api.Models.StokkartModel
{
    public class StokkartOlculerPivotModel
    {
        public long stokkart_id { get; set; }
        public string birim_adi { get; set; }
        public byte birim_id { get; set; }
        public string olcuyeri { get; set; }

        public List<StokkartIlkmaddePivotMatrix> pivotMatrixData { get; set; }
    }
}