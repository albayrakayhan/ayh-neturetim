using System.Collections.Generic;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;
using System.Data.SqlClient;

namespace aceka.infrastructure.Repositories
{
    public class MuhasebeRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;
        #endregion

        public List<muhasebe_tanim_masrafmerkezleri> MuhasebeTanimMasrafMerkezleri()
        {
            List<muhasebe_tanim_masrafmerkezleri> masrafMerkezleri = null;

            #region Query
            string query = @"
                        SELECT
	                        masraf_merkezi_id,
	                        sirket_id, 
	                        ana_masraf_merkezi_id, 
	                        masraf_merkezi_kodu, 
	                        masraf_merkezi_adi, 
	                        statu, 
	                        muhkod_ek, 
	                        grup_kodu, 
	                        grup_adi, 
	                        personel_carikart_id_1, 
	                        stokyeri_carikart_id
                        FROM muhasebe_tanim_masrafmerkezleri WHERE kayit_silindi=0 AND Statu = 1
                        ORDER BY masraf_merkezi_adi
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                masrafMerkezleri = new List<muhasebe_tanim_masrafmerkezleri>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    muhasebe_tanim_masrafmerkezleri masrafMerkezi = new muhasebe_tanim_masrafmerkezleri();
                    masrafMerkezi.masraf_merkezi_id = dt.Rows[i]["masraf_merkezi_id"].acekaToInt();
                    masrafMerkezi.sirket_id = dt.Rows[i]["sirket_id"].acekaToByte();
                    masrafMerkezi.ana_masraf_merkezi_id = dt.Rows[i]["ana_masraf_merkezi_id"].acekaToInt();
                    masrafMerkezi.masraf_merkezi_kodu = dt.Rows[i]["masraf_merkezi_kodu"].ToString();
                    masrafMerkezi.masraf_merkezi_adi = dt.Rows[i]["masraf_merkezi_adi"].ToString();
                    masrafMerkezi.statu = dt.Rows[i]["statu"].acekaToBool();
                    masrafMerkezi.muhkod_ek = dt.Rows[i]["muhkod_ek"].ToString();
                    masrafMerkezi.grup_kodu = dt.Rows[i]["grup_kodu"].ToString();
                    masrafMerkezi.grup_adi = dt.Rows[i]["grup_adi"].ToString();
                    masrafMerkezi.personel_carikart_id_1 = dt.Rows[i]["personel_carikart_id_1"].acekaToLong();
                    masrafMerkezi.stokyeri_carikart_id = dt.Rows[i]["stokyeri_carikart_id"].acekaToLong();
                    masrafMerkezleri.Add(masrafMerkezi);
                    masrafMerkezi = null;
                }
            }
            return masrafMerkezleri;
        }

        public muhasebe_tanim_masrafmerkezleri MuhasebeMasrafMerkezBilgileri(long carikart_id)
        {
            muhasebe_tanim_masrafmerkezleri ckmasrafmerkezi = null;

            #region Query
            string query = @"
                        SELECT
	                          [masraf_merkezi_id]
                             ,[degistiren_carikart_id]
                             ,[degistiren_tarih]
                             ,[sirket_id]
                             ,[ana_masraf_merkezi_id]
                             ,[masraf_merkezi_kodu]
                             ,[masraf_merkezi_adi]
                             ,[statu]
                             ,[muhkod_ek]
                             ,[grup_kodu]
                             ,[grup_adi]
                             ,[kayit_silindi]
                             ,[personel_carikart_id_1]
                             ,[stokyeri_carikart_id]
                        FROM muhasebe_tanim_masrafmerkezleri
                        WHERE kayit_silindi = 0 AND stokyeri_carikart_id = @stokyeri_carikart_id                                                        
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokyeri_carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckmasrafmerkezi = new muhasebe_tanim_masrafmerkezleri();
                ckmasrafmerkezi.stokyeri_carikart_id = dt.Rows[0]["stokyeri_carikart_id"].acekaToLong();
                ckmasrafmerkezi.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckmasrafmerkezi.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckmasrafmerkezi.grup_adi = dt.Rows[0]["grup_adi"].ToString();
                ckmasrafmerkezi.grup_kodu = dt.Rows[0]["grup_kodu"].ToString();
                ckmasrafmerkezi.kayit_silindi = false;
                ckmasrafmerkezi.ana_masraf_merkezi_id = dt.Rows[0]["ana_masraf_merkezi_id"].acekaToInt();
                ckmasrafmerkezi.masraf_merkezi_adi = dt.Rows[0]["masraf_merkezi_adi"].ToString(); 
                ckmasrafmerkezi.masraf_merkezi_kodu = dt.Rows[0]["masraf_merkezi_kodu"].ToString(); 
                ckmasrafmerkezi.muhkod_ek = dt.Rows[0]["muhkod_ek"].ToString();
                ckmasrafmerkezi.personel_carikart_id_1 = dt.Rows[0]["personel_carikart_id_1"].acekaToLong();
                ckmasrafmerkezi.sirket_id = dt.Rows[0]["sirket_id"].acekaToByte();
                ckmasrafmerkezi.statu = dt.Rows[0]["statu"].acekaToBool();
                ckmasrafmerkezi.personel_carikart_id_1 = dt.Rows[0]["personel_carikart_id_1"].acekaToLong();
            }

            return ckmasrafmerkezi;
        }

    }
}
