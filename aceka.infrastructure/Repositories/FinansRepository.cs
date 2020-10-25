using System.Collections.Generic;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;


namespace aceka.infrastructure.Repositories
{
    public class FinansRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;
        #endregion

        public List<finans_tanim_odemeplani> FinansTanimOdemeplanlari()
        {
            List<finans_tanim_odemeplani> odemePlanlari = null;

            #region Query
            string query = @"
                        SELECT
	                        odeme_plani_id, 
	                        statu, 
	                        odeme_plani_kodu, 
	                        odeme_plani_adi, 
	                        banka_hesap_id
                        FROM finans_tanim_odemeplani WHERE kayit_silindi=0 AND Statu = 1
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                odemePlanlari = new List<finans_tanim_odemeplani>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    finans_tanim_odemeplani odemePlani = new finans_tanim_odemeplani();
                    odemePlani.odeme_plani_id = dt.Rows[i]["odeme_plani_id"].acekaToInt();
                    odemePlani.statu = dt.Rows[i]["statu"].acekaToBool();
                    odemePlani.odeme_plani_kodu = dt.Rows[i]["odeme_plani_kodu"].ToString();
                    odemePlani.odeme_plani_adi = dt.Rows[i]["odeme_plani_adi"].ToString();
                    odemePlani.banka_hesap_id = dt.Rows[i]["banka_hesap_id"].acekaToLong();
                    odemePlanlari.Add(odemePlani);
                    odemePlani = null;
                }
            }
            return odemePlanlari;
        }
    }
}
