using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace aceka.infrastructure.Repositories
{
    public class MemberRepository
    {

        #region Değişkenler
        private DataTable dt = null;
        private DataSet ds;
        #endregion

        public cari_kart Giris(string giz_kullanici_adi, string giz_kullanici_sifre)
        {
            cari_kart cariKart = null;

            #region Query
            string query = @"
                            SELECT 
                            carikart_id,
                            cari_unvan
                            FROM carikart
                            WHERE giz_kullanici_adi is not null 
		                            AND giz_kullanici_adi <> '' 
		                            AND isnull(kayit_silindi,0) = 0
		                            AND giz_kullanici_adi = @giz_kullanici_adi AND giz_kullanici_sifre = @giz_kullanici_sifre 
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@giz_kullanici_adi",giz_kullanici_adi),
               new SqlParameter("@giz_kullanici_sifre",giz_kullanici_sifre)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                cariKart = new cari_kart();
                cariKart.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                cariKart.cari_unvan = dt.Rows[0]["cari_unvan"].acekaToString();
            }
            return cariKart;
        }

        public cari_kart GirisSuperAdmin(string giz_kullanici_adi, string giz_kullanici_sifre)
        {
            cari_kart cariKart = null;

            #region Query
            string query = @"
                            SELECT 
                            carikart_id,
                            cari_unvan
                            FROM carikart
                            WHERE giz_kullanici_adi is not null 
		                            AND giz_kullanici_adi <> '' 
		                            AND isnull(kayit_silindi,0) = 0
		                            AND giz_kullanici_adi = @giz_kullanici_adi AND giz_kullanici_sifre = @giz_kullanici_sifre e AND giz_yazilim_kodu = 9999
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@giz_kullanici_adi",giz_kullanici_adi),
               new SqlParameter("@giz_kullanici_sifre",giz_kullanici_sifre)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                cariKart = new cari_kart();
                cariKart.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                cariKart.cari_unvan = dt.Rows[0]["cari_unvan"].acekaToString();
            }
            return cariKart;
        }

        public KullaniciYetki kullaniciYetkisiniGetir(long carikart_id, string apiUrl, ref string errorMessage)
        {
            KullaniciYetki kullaniciYetki = null;

            #region Query
            string query = @"
                        SELECT * FROM [dbo].[GetKullaniciYetki]('URETIM', @carikart_id)
                        Where url = @url 
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id),
               new SqlParameter("@url",apiUrl)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    kullaniciYetki = new KullaniciYetki();
                    kullaniciYetki.modul = dt.Rows[0]["modul"].acekaToString();
                    kullaniciYetki.url = dt.Rows[0]["url"].acekaToString();
                    kullaniciYetki.yetkialani = dt.Rows[0]["yetkialani"].acekaToString();
                    kullaniciYetki.tanim = dt.Rows[0]["tanim"].acekaToString();
                    kullaniciYetki.okuma = dt.Rows[0]["okuma"].acekaToBool();
                    kullaniciYetki.yazma = dt.Rows[0]["yazma"].acekaToBool();
                    kullaniciYetki.silme = dt.Rows[0]["silme"].acekaToBool();

                    kullaniciYetki.ozel1 = dt.Rows[0]["ozel1"].acekaToBool();
                    kullaniciYetki.ozel2 = dt.Rows[0]["ozel2"].acekaToBool();
                    kullaniciYetki.ozel3 = dt.Rows[0]["ozel3"].acekaToBool();
                    kullaniciYetki.ozel4 = dt.Rows[0]["ozel4"].acekaToBool();
                    kullaniciYetki.ozel5 = dt.Rows[0]["ozel5"].acekaToBool();
                    kullaniciYetki.ozel6 = dt.Rows[0]["ozel6"].acekaToBool();
                    kullaniciYetki.ozel7 = dt.Rows[0]["ozel7"].acekaToBool();
                    kullaniciYetki.ozel8 = dt.Rows[0]["ozel8"].acekaToBool();
                    kullaniciYetki.ozel9 = dt.Rows[0]["ozel9"].acekaToBool();
                    kullaniciYetki.ozel10 = dt.Rows[0]["ozel10"].acekaToBool();
                    kullaniciYetki.ozel11 = dt.Rows[0]["ozel11"].acekaToBool();
                    kullaniciYetki.ozel12 = dt.Rows[0]["ozel12"].acekaToBool();
                    kullaniciYetki.ozel13 = dt.Rows[0]["ozel13"].acekaToBool();
                    kullaniciYetki.ozel14 = dt.Rows[0]["ozel14"].acekaToBool();
                    kullaniciYetki.ozel15 = dt.Rows[0]["ozel15"].acekaToBool();
                    kullaniciYetki.ozel16 = dt.Rows[0]["ozel16"].acekaToBool();
                    kullaniciYetki.ozel17 = dt.Rows[0]["ozel17"].acekaToBool();
                    kullaniciYetki.ozel18 = dt.Rows[0]["ozel18"].acekaToBool();
                    kullaniciYetki.ozel19 = dt.Rows[0]["ozel19"].acekaToBool();
                    kullaniciYetki.ozel20 = dt.Rows[0]["ozel20"].acekaToBool();
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
            }
            return kullaniciYetki;
        }

        public List<KullaniciYetki> kullaniciYetkileriniGetir(long carikart_id, ref string errorMessage)
        {
            List<KullaniciYetki> kullaniciYetkileri = null;

            #region Query
            string query = @"
                        SELECT * FROM [dbo].[GetKullaniciYetki]('URETIM', @carikart_id)
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    KullaniciYetki kullaniciYetki = null;
                    kullaniciYetkileri = new List<KullaniciYetki>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        kullaniciYetki = new KullaniciYetki();
                        kullaniciYetki.modul = dt.Rows[i]["modul"].acekaToString();
                        kullaniciYetki.url = dt.Rows[i]["url"].acekaToString();
                        kullaniciYetki.yetkialani = dt.Rows[i]["yetkialani"].acekaToString();
                        kullaniciYetki.tanim = dt.Rows[i]["tanim"].acekaToString();
                        kullaniciYetki.okuma = dt.Rows[i]["okuma"].acekaToBool();
                        kullaniciYetki.yazma = dt.Rows[i]["yazma"].acekaToBool();
                        kullaniciYetki.silme = dt.Rows[i]["silme"].acekaToBool();
                        kullaniciYetki.ozel1 = dt.Rows[i]["ozel1"].acekaToBool();
                        kullaniciYetki.ozel2 = dt.Rows[i]["ozel2"].acekaToBool();
                        kullaniciYetki.ozel3 = dt.Rows[i]["ozel3"].acekaToBool();
                        kullaniciYetki.ozel4 = dt.Rows[i]["ozel4"].acekaToBool();
                        kullaniciYetki.ozel5 = dt.Rows[i]["ozel5"].acekaToBool();
                        kullaniciYetki.ozel6 = dt.Rows[i]["ozel6"].acekaToBool();
                        kullaniciYetki.ozel7 = dt.Rows[i]["ozel7"].acekaToBool();
                        kullaniciYetki.ozel8 = dt.Rows[i]["ozel8"].acekaToBool();
                        kullaniciYetki.ozel9 = dt.Rows[i]["ozel9"].acekaToBool();
                        kullaniciYetki.ozel10 = dt.Rows[i]["ozel10"].acekaToBool();
                        kullaniciYetki.ozel11 = dt.Rows[i]["ozel11"].acekaToBool();
                        kullaniciYetki.ozel12 = dt.Rows[i]["ozel12"].acekaToBool();
                        kullaniciYetki.ozel13 = dt.Rows[i]["ozel13"].acekaToBool();
                        kullaniciYetki.ozel14 = dt.Rows[i]["ozel14"].acekaToBool();
                        kullaniciYetki.ozel15 = dt.Rows[i]["ozel15"].acekaToBool();
                        kullaniciYetki.ozel16 = dt.Rows[i]["ozel16"].acekaToBool();
                        kullaniciYetki.ozel17 = dt.Rows[i]["ozel17"].acekaToBool();
                        kullaniciYetki.ozel18 = dt.Rows[i]["ozel18"].acekaToBool();
                        kullaniciYetki.ozel19 = dt.Rows[i]["ozel19"].acekaToBool();
                        kullaniciYetki.ozel20 = dt.Rows[i]["ozel20"].acekaToBool();
                    }

                }
            }
            catch (System.Exception ex)
            {
                errorMessage = ex.Message;
            }
            return kullaniciYetkileri;
        }
    }
}
