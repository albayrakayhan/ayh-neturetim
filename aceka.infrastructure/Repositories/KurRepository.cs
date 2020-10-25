using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aceka.infrastructure.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Core;

namespace aceka.infrastructure.Repositories
{
    public class KurRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private SqlConnection conn = null;
        private Kur kur = null;
        private List<Kur> kurlar = null;
        #endregion

        public List<Kur> Getir(string yil,string ay, int pb)
        {

            #region Query
            string query = @"SELECT 
                tarih, 
                pb, 
                degistiren_carikart_id, 
                degistiren_tarih, 
                prog_alis, 
                prog_satis, 
                mb_alis, 
                mb_satis, 
                ser_alis, 
                ser_satis 
                FROM kur WHERE YEAR(tarih) = @yil AND MONTH(tarih) = @ay AND pb = @pb";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@yil",yil),
               new SqlParameter("@ay",ay),
               new SqlParameter("@pb",pb)
            };

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                kurlar = new List<Kur>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kur = new Kur();
                    kur.pb = dt.Rows[i]["pb"].ToString();
                    kur.tarih = (dt.Rows[i]["tarih"].acekaToDateTime());
                    kur.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    kur.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    kur.prog_alis = dt.Rows[i]["prog_alis"].acekaToDouble();
                    kur.prog_satis = dt.Rows[i]["prog_satis"].acekaToDouble();
                    kur.mb_alis = dt.Rows[i]["mb_alis"].acekaToDouble();
                    kur.mb_satis = dt.Rows[i]["mb_satis"].acekaToDouble();
                    kur.ser_alis = dt.Rows[i]["ser_alis"].acekaToDouble();
                    kur.ser_satis = dt.Rows[i]["ser_satis"].acekaToDouble();
                    kurlar.Add(kur);
                    kur = null;
                }
            }

            return kurlar;
        }

        public List<Kur> seneGetir()
        {
            #region Query
            string query = @"SET language turkish
                            SELECT distinct datename(YYYY,tarih) as sene_adi
                            FROM
                            kur order by datename(YYYY,tarih)";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            Kur kur = null;
            kurlar = new List<Kur>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kur = new Kur();
                kur.sene_adi = dt.Rows[i]["sene_adi"].ToString();
                kurlar.Add(kur);
            }
            return kurlar;

        }

        public List<Kur> ayGetir()
        {
            #region Query
            string query = @"SET language turkish
                            select distinct DATENAME(mm,tarih)  as Aylar,MONTH(tarih) as ay_no
                            FROM
                            kur order by  MONTH(tarih)";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            Kur kur = null;
            kurlar = new List<Kur>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kur = new Kur();
                kur.ay_adi = dt.Rows[i]["aylar"].ToString();
                kur.ay_no = dt.Rows[i]["ay_no"].ToString();
                kurlar.Add(kur);
            }
            return kurlar;

        }



    }
}
