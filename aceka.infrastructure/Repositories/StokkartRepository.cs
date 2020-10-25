using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;

namespace aceka.infrastructure.Repositories
{
    public class StokkartRepository
    {
        #region Değişkenler
        private stokkart stokKart = null;
        private List<stokkart> stoklar = null;
        private DataTable dt = null;
        private stokkart_rapor_parametre stok_rapor_param = null;
        private giz_setup_ekturu ekturu = null;
        private DataSet ds;
        private List<cari_kart> saticilist;
        private cari_kart satici;
        #endregion

        public List<stokkart> Bul(string stok_adi = "", short stokkart_tur_id = 0, int stokkart_tipi_id = 0, string stok_kodu = "", byte stokkartturu = 0, string orjinal_stok_kodu = "")
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            //if (stokkart_id > 0)
            //{
            //    parameterControl++;
            //    orStatement += "SK.stokkart_id = @stokkart_id AND ";
            //}
            if (stokkartturu > 0)
            {
                parameterControl++;
                orStatement += "SKTip.stokkartturu = @stokkartturu AND ";
            }
            if (stok_adi != null && !string.IsNullOrEmpty(stok_adi.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SK.stok_adi like @stok_adi AND ";
            }
            if (stokkart_tipi_id > 0)
            {
                parameterControl++;
                orStatement += " SK.stokkart_tipi_id  = @stokkart_tipi_id AND ";
            }
            if (stokkart_tur_id > 0)
            {
                parameterControl++;
                orStatement += " SK.stokkart_tur_id  = @stokkart_tur_id AND ";
            }
            if (stok_kodu != null && !string.IsNullOrEmpty(stok_kodu.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SK.stok_kodu like @stok_kodu AND ";
            }
            if (orjinal_stok_kodu != null && !string.IsNullOrEmpty(orjinal_stok_kodu.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SO.orjinal_stok_kodu like @orjinal_stok_kodu AND ";
            }


            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            orStatement += " ";

            string query = @"
                            SELECT
                                Top(50)
	                            SK.stokkart_id,
	                            SK.statu, 
	                            SK.stokkart_tur_id, 
	                            SKTur.tanim as 'tur_tanim',
	                            SK.stokkart_tipi_id, 
	                            SKTip.tanim as 'tip_tanim', 
	                            --Kısa ad => stok_kodu 
	                            SK.stok_kodu,
                                SK.stok_adi,
                                SO.orjinal_stok_kodu,
								SO.orjinal_stok_adi,
								SO.stok_adi_uzun
                            FROM [dbo].[stokkart] SK   
                            LEFT JOIN giz_sabit_stokkarttipi SKTip ON SK.stokkart_tipi_id = SKTip.stokkarttipi 
                            LEFT JOIN giz_sabit_stokkartturu SKTur ON SK.stokkart_tur_id = SKTur.stokkartturu
                            LEFT JOIN stokkart_ozel SO ON SO.stokkart_id=SK.stokkart_id 
                            WHERE " + orStatement + " AND isnull(SK.kayit_silindi,0) = 0 AND SKTip.stokkartturu = @stokkartturu";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                   // new SqlParameter("@stokkart_id",stokkart_id),
                    new SqlParameter("@stok_adi","%"  +stok_adi + "%"),
                    new SqlParameter("@stokkart_tipi_id",stokkart_tipi_id),
                    new SqlParameter("@stokkart_tur_id",stokkart_tur_id),
                    new SqlParameter("@stok_kodu","%"  +stok_kodu + "%"),
                    new SqlParameter("@stokkartturu",stokkartturu),
                     new SqlParameter("@orjinal_stok_kodu",orjinal_stok_kodu +"%")
            };
            #endregion

            if (parameterControl > 0)
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    stoklar = new List<stokkart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stokKart = new stokkart();
                        stokKart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        stokKart.statu = dt.Rows[i]["statu"].acekaToBool();
                        //giz_sabit_stokkartturu tablosu Start
                        stokKart.stokkart_tur_id = dt.Rows[i]["stokkart_tur_id"].acekaToShort();
                        stokKart.stokkartturu = new giz_sabit_stokkartturu();
                        stokKart.stokkartturu.stokkartturu = dt.Rows[i]["stokkart_tur_id"].acekaToByte();
                        stokKart.stokkartturu.tanim = dt.Rows[i]["tur_tanim"].ToString();
                        //giz_sabit_stokkartturu tablosu End

                        //giz_sabit_stokkarttipi tablosu Start
                        //stokKart.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToInt();
                        stokKart.stokkarttipi = new giz_sabit_stokkarttipi();
                        stokKart.stokkarttipi.stokkarttipi = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
                        stokKart.stokkarttipi.tanim = dt.Rows[i]["tip_tanim"].ToString();
                        //giz_sabit_stokkarttipi tablosu End

                        //stokkart_ozel tablosu Start
                        stokKart.stokkart_ozel = new stokkart_ozel();
                        stokKart.stokkart_ozel.stok_adi_uzun = dt.Rows[i]["stok_adi_uzun"].ToString();
                        stokKart.stokkart_ozel.orjinal_stok_kodu = dt.Rows[i]["orjinal_stok_kodu"].ToString();
                        stokKart.stokkart_ozel.orjinal_stok_adi = dt.Rows[i]["orjinal_stok_adi"].ToString();
                        //stokKart.stokkart_ozel.orjinal_renk_kodu = dt.Rows[i]["orjinal_renk_kodu"].ToString();
                        //stokKart.stokkart_ozel.orjinal_renk_adi = dt.Rows[i]["orjinal_renk_adi"].ToString();
                        //stokkart_ozel tablosu End

                        stokKart.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                        stokKart.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                        stoklar.Add(stokKart);
                        stokKart = null;
                    }
                }
            }
            return stoklar;
        }

        public List<stokkart> Bul(string stok_kodu = "", byte stokartturu = 0)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (!string.IsNullOrEmpty(stok_kodu.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SK.stok_kodu like @stok_kodu AND ";
                //orStatement += "SK.stok_adi like @stok_adi AND ";
            }
            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            orStatement += " ";

            string query = @"
                            SELECT  
                                SK.stokkart_id,
	                            --Kısa ad => stok_kodu 
	                            SK.stok_kodu,
                                stok_adi
                            FROM [dbo].[stokkart] SK 
                            LEFT JOIN giz_sabit_stokkarttipi SKTip ON SK.stokkart_tipi_id = SKTip.stokkarttipi 
                            LEFT JOIN giz_sabit_stokkartturu SKTur ON SK.stokkart_tur_id = SKTur.stokkartturu --and SK.stokkart_tur_id=2
                            WHERE " + orStatement + " AND isnull(SK.kayit_silindi,0) = 0 AND SKTip.stokkartturu = @stokkartturu --AND SK.stokkart_tipi_id IN (20, 21, 22)";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stok_adi",stok_kodu + "%"),
                    new SqlParameter("@stokkartturu",stokartturu)
            };
            #endregion

            if (parameterControl > 0)
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    stoklar = new List<stokkart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stokKart = new stokkart();
                        stokKart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        stokKart.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                        stokKart.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                        stoklar.Add(stokKart);
                        stokKart = null;
                    }
                }
            }
            return stoklar;
        }

        public List<stokkart> Bul(string stok_kodu = "", string stok_adi = "", byte stokkartturu = 0)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (!string.IsNullOrEmpty(stok_kodu.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SK.stok_kodu like @stok_kodu AND ";
            }
            if (!string.IsNullOrEmpty(stok_adi.TrimEnd()))
            {
                parameterControl++;
                orStatement += "SK.stok_adi like @stok_adi AND ";
            }

            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            orStatement += " ";

            string query = @"
                            SELECT  
                            TOP 200
                                SK.stokkart_id,
	                            --Kısa ad => stok_kodu 
	                            SK.stok_kodu,
                                stok_adi
                            FROM [dbo].[stokkart] SK 
                            LEFT JOIN giz_sabit_stokkarttipi SKTip ON SK.stokkart_tipi_id = SKTip.stokkarttipi 
                            LEFT JOIN giz_sabit_stokkartturu SKTur ON SK.stokkart_tur_id = SKTur.stokkartturu 
                            WHERE " + orStatement + " AND SKTip.stokkartturu = @stokkartturu AND isnull(SK.kayit_silindi,0) = 0";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stok_kodu","%"  +stok_kodu + "%"),
                    new SqlParameter("@stok_adi","%"  +stok_adi + "%"),
                    new SqlParameter("@stokkartturu",stokkartturu)
            };
            #endregion

            if (parameterControl > 0)
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    stoklar = new List<stokkart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stokKart = new stokkart();
                        stokKart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        stokKart.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                        stokKart.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                        stoklar.Add(stokKart);
                        stokKart = null;
                    }
                }
            }
            return stoklar;
        }

        public List<stokkart> Bul_StokAdi_ve_StokkartTipi(string stok_adi, byte stokkart_tipi_id)
        {
            #region Query
            string query = @"
                            SELECT
                            TOP 200
                            stokkart_id,
                            stok_kodu,
                            stok_adi,
                            stokkart_tipi_id,
                            birim_id_1
                            FROM stokkart stokkart_tipi_id
                            WHERE ISNULL(kayit_silindi,0)=0  AND stok_adi like @stok_adi
                            AND stokkart_tipi_id = @stokkart_tipi_id  --AND (stok_adi is not null AND stok_adi <> '')
                            ORDER BY stok_adi";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stok_adi",stok_adi + "%"),
                    new SqlParameter("@stokkart_tipi_id",stokkart_tipi_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stoklar = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stokKart = new stokkart();
                    stokKart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    stokKart.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    stokKart.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    stokKart.birim_id_1 = dt.Rows[i]["birim_id_1"].acekaToByte();
                    stokKart.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToInt();
                    stoklar.Add(stokKart);
                    stokKart = null;
                }
            }
            return stoklar;
        }

        public List<stokkart> Bul_StokAdi_ve_StokkartTur(string stok_adi, byte stokkart_tur_id)
        {
            #region Query
            string query = @"
                            SELECT
                            TOP 200
                            stokkart_id,
                            stok_kodu,
                            stok_adi
                            FROM stokkart stokkart_tipi_id
                            WHERE ISNULL(kayit_silindi,0)=0  AND stok_adi like @stok_adi
                            AND stokkart_tur_id = @stokkart_tur_id  --AND (stok_adi is not null AND stok_adi <> '')
                            ORDER BY stok_adi";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stok_adi",stok_adi + "%"),
                    new SqlParameter("@stokkart_tur_id",stokkart_tur_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stoklar = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stokKart = new stokkart();
                    stokKart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    stokKart.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    stokKart.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    stoklar.Add(stokKart);
                    stokKart = null;
                }
            }
            return stoklar;
        }
        public stokkart Getir(long stokkart_id)
        {
            #region Query
            string query = @"
                            SELECT 
	                            SK.stokkart_id, 
	                            SK.degistiren_carikart_id, 
	                            SK.degistiren_tarih, 
	                            SK.kayit_silindi, 
	                            SK.statu, 
	                            SK.stokkart_tur_id, 
	                            SKTur.tanim as 'tur_tanim',
	                            SK.stokkart_tipi_id, 
	                            SKTip.tanim as 'tip_tanim', 
	                            --Kısa ad => stok_kodu 
	                            SK.stok_kodu, 
                                SK.stok_adi, 
	                            SK.kdv_alis_id, 
	                            KDV1.kod as 'kdv_alis_kod', 
	                            SK.kdv_satis_id, 
	                            KDV2.kod as 'kdv_satis_kod', 
	                            SK.birim_id_1, 
	                            BRM1.birim_adi as 'birim_adi_1', 
	                            BRM1.birim_kod as 'birim_kod_1', 
	                            SK.birim_id_2, 
	                            BRM2.birim_adi as 'birim_adi_2', 
	                            BRM2.birim_kod as 'birim_kod_2', 
	                            SK.birim_id_2_zorunlu, 
                                SK.birim_id_3_zorunlu, 
	                            SK.birim_id_3, 
	                            BRM3.birim_adi as 'birim_adi_3', 
	                            BRM3.birim_kod as 'birim_kod_3', 
	                            SK.birim_id_3_zorunlu, 
	                            --stokkart_ozel TABLOSU Inseret Update, ayrıca stokkart_id identity=no. stokkart tablosundan gelecek. 
	                            SKOzel.stok_adi_uzun, 
	                            SKOzel.orjinal_stok_kodu, 
	                            SKOzel.orjinal_stok_adi,  
                                SKOzel.tek_varyant,
                                SKOzel.orjinal_renk_kodu,
                                SKOzel.orjinal_renk_adi,
	                            T.talimatturu_id, 
	                            T.kod, 
	                            T.tanim as 'talimat_tanim', 
	                            SKTalimat.aciklama, 
	                            SK.stokkart_sezon_id,
	                            SK.tevkifat_alis,
	                            SK.tevkifat_satis,
	                            SK.maliyetlendirme_turu,SKOnay.genel_onay
                            FROM [dbo].[stokkart] SK   
                            LEFT JOIN giz_sabit_stokkarttipi SKTip ON SK.stokkart_tipi_id = SKTip.stokkarttipi 
                            LEFT JOIN giz_sabit_stokkartturu SKTur ON SK.stokkart_tur_id = SKTur.stokkartturu 
                            LEFT JOIN parametre_kdv KDV1  ON KDV1.kod = SK.kdv_alis_id 
                            LEFT JOIN parametre_kdv KDV2  ON KDV2.kod = SK.kdv_satis_id 
                            LEFT JOIN parametre_birim BRM1 ON BRM1.birim_id = SK.birim_id_1 
                            LEFT JOIN parametre_birim BRM2 ON BRM2.birim_id = SK.birim_id_2 
                            LEFT JOIN parametre_birim BRM3 ON BRM3.birim_id = SK.birim_id_3 
                            LEFT JOIN stokkart_ozel SKOzel ON SKOzel.stokkart_id=SK.stokkart_id 
                            LEFT JOIN stokkart_talimat SKTalimat ON SKTalimat.stokkart_id = SK.stokkart_id 
                            LEFT JOIN stokkart_onay SKOnay  ON SKOnay.stokkart_id = SK.stokkart_id 
                            LEFT JOIN talimat T ON  SKTalimat.talimatturu_id = T.talimatturu_id 
                            Where SK.stokkart_id = @stokkart_id 
                            --AND ISNULL(SK.kayit_silindi,0) = 0 Ayhan. 23.02.2017 --AND ISNULL(SK.statu,0) = 1
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokKart = new stokkart();
                stokKart.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                stokKart.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                stokKart.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                stokKart.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                stokKart.statu = dt.Rows[0]["statu"].acekaToBool();
                //giz_sabit_stokkartturu tablosu Start
                stokKart.stokkart_tur_id = dt.Rows[0]["stokkart_tur_id"].acekaToShort();
                stokKart.stokkartturu = new giz_sabit_stokkartturu();
                stokKart.stokkartturu.stokkartturu = dt.Rows[0]["stokkart_tur_id"].acekaToByte();
                stokKart.stokkartturu.tanim = dt.Rows[0]["tur_tanim"].ToString();
                //giz_sabit_stokkartturu tablosu End

                //giz_sabit_stokkarttipi tablosu Start
                stokKart.stokkart_tipi_id = dt.Rows[0]["stokkart_tipi_id"].acekaToInt();
                stokKart.stokkarttipi = new giz_sabit_stokkarttipi();
                stokKart.stokkarttipi.stokkarttipi = dt.Rows[0]["stokkart_tipi_id"].acekaToByte();
                stokKart.stokkarttipi.tanim = dt.Rows[0]["tip_tanim"].ToString();
                //giz_sabit_stokkarttipi tablosu End

                stokKart.stok_kodu = dt.Rows[0]["stok_kodu"].ToString();
                stokKart.stok_adi = dt.Rows[0]["stok_adi"].ToString();

                stokKart.kdv_alis_id = dt.Rows[0]["kdv_alis_id"].acekaToByteWithNullable();
                stokKart.kdv_satis_id = dt.Rows[0]["kdv_satis_id"].acekaToByteWithNullable();
                stokKart.birim_id_1 = dt.Rows[0]["birim_id_1"].acekaToByte();
                stokKart.birim_id_2 = dt.Rows[0]["birim_id_2"].acekaToByteWithNullable();
                stokKart.birim_id_2_zorunlu = dt.Rows[0]["birim_id_2_zorunlu"].acekaToBool();
                stokKart.birim_id_3 = dt.Rows[0]["birim_id_3"].acekaToByteWithNullable();
                stokKart.birim_id_3_zorunlu = dt.Rows[0]["birim_id_3_zorunlu"].acekaToBool();

                //stokkart_ozel tablosu Start
                stokKart.stokkart_ozel = new stokkart_ozel();
                stokKart.stokkart_ozel.stok_adi_uzun = dt.Rows[0]["stok_adi_uzun"].ToString();
                stokKart.stokkart_ozel.orjinal_stok_kodu = dt.Rows[0]["orjinal_stok_kodu"].ToString();
                stokKart.stokkart_ozel.orjinal_stok_adi = dt.Rows[0]["orjinal_stok_adi"].ToString();
                stokKart.stokkart_ozel.tek_varyant = dt.Rows[0]["tek_varyant"].acekaToBoolWithNullable();
                stokKart.stokkart_ozel.orjinal_renk_kodu = dt.Rows[0]["orjinal_renk_kodu"].ToString();
                stokKart.stokkart_ozel.orjinal_renk_adi = dt.Rows[0]["orjinal_renk_adi"].ToString();
                //stokkart_ozel tablosu End

                //talimat tablosu Start
                stokKart.talimat = new talimat();
                stokKart.talimat.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToInt();
                stokKart.talimat.kod = dt.Rows[0]["kod"].ToString();
                stokKart.talimat.tanim = dt.Rows[0]["talimat_tanim"].ToString();
                //talimat tablosu End

                //stokkart_talimat tablosu Start
                stokKart.stokkart_talimat = new stokkart_talimat();
                stokKart.stokkart_talimat.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                //stokkart_talimat tablosu End

                //stokkart_onay tablosu Start AA.02.03.2017
                stokKart.stokkartonay = new stokkart_onay();
                stokKart.stokkartonay.genel_onay = dt.Rows[0]["genel_onay"].acekaToBool();
                //stokkart_onay tablosu End 


                stokKart.stokkart_sezon_id = dt.Rows[0]["stokkart_sezon_id"].acekaToShortWithNullable();
                stokKart.tevkifat_alis = dt.Rows[0]["tevkifat_alis"].acekaToByteWithNullable();
                stokKart.tevkifat_satis = dt.Rows[0]["tevkifat_satis"].acekaToByteWithNullable();
                stokKart.maliyetlendirme_turu = dt.Rows[0]["maliyetlendirme_turu"].acekaToByteWithNullable();
            }
            return stokKart;
        }
        public stokkart Getir(long stokkart_id, byte stokkartturu)
        {
            #region Query
            string query = @"
                            SELECT 
	                            SK.stokkart_id, 
	                            SK.degistiren_carikart_id, 
	                            SK.degistiren_tarih, 
	                            SK.kayit_silindi, 
	                            SK.statu, 
	                            SK.stokkart_tur_id, 
	                            SKTur.tanim as 'tur_tanim',
	                            SK.stokkart_tipi_id, 
	                            SKTip.tanim as 'tip_tanim', 
	                            --Kısa ad => stok_kodu 
	                            SK.stok_kodu, 
                                SK.stok_adi, 
	                            SK.kdv_alis_id, 
	                            KDV1.kod as 'kdv_alis_kod', 
	                            SK.kdv_satis_id, 
	                            KDV2.kod as 'kdv_satis_kod', 
	                            SK.birim_id_1, 
	                            BRM1.birim_adi as 'birim_adi_1', 
	                            BRM1.birim_kod as 'birim_kod_1', 
	                            SK.birim_id_2, 
	                            BRM2.birim_adi as 'birim_adi_2', 
	                            BRM2.birim_kod as 'birim_kod_2', 
	                            SK.birim_id_2_zorunlu, 
                                SK.birim_id_3_zorunlu, 
	                            SK.birim_id_3, 
	                            BRM3.birim_adi as 'birim_adi_3', 
	                            BRM3.birim_kod as 'birim_kod_3', 
	                            --stokkart_ozel TABLOSU Inseret Update, ayrıca stokkart_id identity=no. stokkart tablosundan gelecek. 
	                            SKOzel.stok_adi_uzun, 
	                            SKOzel.orjinal_stok_kodu, 
	                            SKOzel.orjinal_stok_adi,  
                                SKOzel.tek_varyant,
                                SKOzel.orjinal_renk_kodu,
                                SKOzel.orjinal_renk_adi,
	                            T.talimatturu_id, 
	                            T.kod, 
	                            T.tanim as 'talimat_tanim', 
	                            SKTalimat.aciklama, 
	                            SK.stokkart_sezon_id,
	                            SK.tevkifat_alis,
	                            SK.tevkifat_satis,
	                            SK.maliyetlendirme_turu,SKOnay.genel_onay,
                                SK.anastokkart_id,
                                SK.uretimyeri_id
                            FROM [dbo].[stokkart] SK   
                            LEFT JOIN giz_sabit_stokkarttipi SKTip ON SK.stokkart_tipi_id = SKTip.stokkarttipi 
                            LEFT JOIN giz_sabit_stokkartturu SKTur ON SK.stokkart_tur_id = SKTur.stokkartturu 
                            LEFT JOIN parametre_kdv KDV1  ON KDV1.kod = SK.kdv_alis_id 
                            LEFT JOIN parametre_kdv KDV2  ON KDV2.kod = SK.kdv_satis_id 
                            LEFT JOIN parametre_birim BRM1 ON BRM1.birim_id = SK.birim_id_1 
                            LEFT JOIN parametre_birim BRM2 ON BRM2.birim_id = SK.birim_id_2 
                            LEFT JOIN parametre_birim BRM3 ON BRM3.birim_id = SK.birim_id_3 
                            LEFT JOIN stokkart_ozel SKOzel ON SKOzel.stokkart_id=SK.stokkart_id 
                            LEFT JOIN stokkart_talimat SKTalimat ON SKTalimat.stokkart_id = SK.stokkart_id 
                            LEFT JOIN stokkart_onay SKOnay  ON SKOnay.stokkart_id = SK.stokkart_id 
                            LEFT JOIN talimat T ON  SKTalimat.talimatturu_id = T.talimatturu_id 
                            Where SK.stokkart_id = @stokkart_id AND SKTip.stokkartturu=@stokkartturu
                            -- SK.stokkart_tipi_id IN (20, 21, 22) AND ISNULL(SK.kayit_silindi,0) = 0 Ayhan. 23.02.2017 --AND ISNULL(SK.statu,0) = 1
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@stokkartturu",stokkartturu)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokKart = new stokkart();
                stokKart.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                stokKart.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                stokKart.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                stokKart.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                stokKart.statu = dt.Rows[0]["statu"].acekaToBool();
                stokKart.anastokkart_id = dt.Rows[0]["anastokkart_id"].acekaToLong();
                //giz_sabit_stokkartturu tablosu Start
                stokKart.stokkart_tur_id = dt.Rows[0]["stokkart_tur_id"].acekaToShort();
                stokKart.stokkartturu = new giz_sabit_stokkartturu();
                stokKart.stokkartturu.stokkartturu = dt.Rows[0]["stokkart_tur_id"].acekaToByte();
                stokKart.stokkartturu.tanim = dt.Rows[0]["tur_tanim"].ToString();
                stokKart.uretimyeri_id = dt.Rows[0]["uretimyeri_id"].acekaToShort();
                //giz_sabit_stokkartturu tablosu End

                //giz_sabit_stokkarttipi tablosu Start
                stokKart.stokkart_tipi_id = dt.Rows[0]["stokkart_tipi_id"].acekaToInt();
                stokKart.stokkarttipi = new giz_sabit_stokkarttipi();
                stokKart.stokkarttipi.stokkarttipi = dt.Rows[0]["stokkart_tipi_id"].acekaToByte();
                stokKart.stokkarttipi.tanim = dt.Rows[0]["tip_tanim"].ToString();
                //giz_sabit_stokkarttipi tablosu End

                stokKart.stok_kodu = dt.Rows[0]["stok_kodu"].ToString();
                stokKart.stok_adi = dt.Rows[0]["stok_adi"].ToString();

                stokKart.kdv_alis_id = dt.Rows[0]["kdv_alis_id"].acekaToByteWithNullable();
                stokKart.kdv_satis_id = dt.Rows[0]["kdv_satis_id"].acekaToByteWithNullable();
                stokKart.birim_id_1 = dt.Rows[0]["birim_id_1"].acekaToByte();
                stokKart.birim_id_2 = dt.Rows[0]["birim_id_2"].acekaToByteWithNullable();
                stokKart.birim_id_2_zorunlu = dt.Rows[0]["birim_id_2_zorunlu"].acekaToBool();
                stokKart.birim_id_3 = dt.Rows[0]["birim_id_3"].acekaToByteWithNullable();
                stokKart.birim_id_3_zorunlu = dt.Rows[0]["birim_id_3_zorunlu"].acekaToBool();

                //stokkart_ozel tablosu Start
                stokKart.stokkart_ozel = new stokkart_ozel();
                stokKart.stokkart_ozel.stok_adi_uzun = dt.Rows[0]["stok_adi_uzun"].ToString();
                stokKart.stokkart_ozel.orjinal_stok_kodu = dt.Rows[0]["orjinal_stok_kodu"].ToString();
                stokKart.stokkart_ozel.orjinal_stok_adi = dt.Rows[0]["orjinal_stok_adi"].ToString();
                stokKart.stokkart_ozel.tek_varyant = dt.Rows[0]["tek_varyant"].acekaToBoolWithNullable();
                stokKart.stokkart_ozel.orjinal_renk_kodu = dt.Rows[0]["orjinal_renk_kodu"].ToString();
                stokKart.stokkart_ozel.orjinal_renk_adi = dt.Rows[0]["orjinal_renk_adi"].ToString();
                //stokKart.stokkart_ozel.birim1x = dt.Rows[0]["birim1x"].acekaToFloat();
                //stokKart.stokkart_ozel.birim2x = dt.Rows[0]["birim2x"].acekaToFloat();
                //stokKart.stokkart_ozel.birim3x = dt.Rows[0]["birim3x"].acekaToFloat();
                //stokKart.stokkart_ozel.M2_gram = dt.Rows[0]["M2_gram"].acekaToFloat();
                //stokKart.stokkart_ozel.eni = dt.Rows[0]["eni"].acekaToFloat();
                //stokKart.stokkart_ozel.fyne = dt.Rows[0]["fyne"].acekaToFloat();
                //stokKart.stokkart_ozel.fire_orani = dt.Rows[0]["fire_orani"].acekaToFloat();
                //stokKart.stokkart_ozel.birim_gram = dt.Rows[0]["birim_gram"].acekaToFloat();


                //stokkart_ozel tablosu End

                //talimat tablosu Start
                stokKart.talimat = new talimat();
                stokKart.talimat.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToInt();
                stokKart.talimat.kod = dt.Rows[0]["kod"].ToString();
                stokKart.talimat.tanim = dt.Rows[0]["talimat_tanim"].ToString();
                //talimat tablosu End

                //stokkart_talimat tablosu Start
                stokKart.stokkart_talimat = new stokkart_talimat();
                stokKart.stokkart_talimat.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                //stokkart_talimat tablosu End

                //stokkart_onay tablosu Start AA.02.03.2017
                stokKart.stokkartonay = new stokkart_onay();
                stokKart.stokkartonay.genel_onay = dt.Rows[0]["genel_onay"].acekaToBool();
                //stokkart_onay tablosu End 


                stokKart.stokkart_sezon_id = dt.Rows[0]["stokkart_sezon_id"].acekaToShortWithNullable();
                stokKart.tevkifat_alis = dt.Rows[0]["tevkifat_alis"].acekaToByteWithNullable();
                stokKart.tevkifat_satis = dt.Rows[0]["tevkifat_satis"].acekaToByteWithNullable();
                stokKart.maliyetlendirme_turu = dt.Rows[0]["maliyetlendirme_turu"].acekaToByteWithNullable();
            }
            return stokKart;
        }
        public List<stokkart> StokListesiniGetir(byte stokkart_tipi_id, int count = 50)
        {
            List<stokkart> modeller = null;
            #region Query
            string query = @"
                           SELECT 
                            Top 
                            @count
                            stokkart_id,
	                        stok_adi,
							stok_kodu,
                            birim_id_1
	                        FROM stokkart
							WHERE kayit_silindi=0 and stokkart_tipi_id=@stokkart_tipi_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_tipi_id",stokkart_tipi_id),
               new SqlParameter("@count",count)
            };

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart model = null;
                modeller = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model = new stokkart();
                    model.birim_id_1 = dt.Rows[i]["birim_id_1"].acekaToByte();
                    model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    model.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    model.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    modeller.Add(model);
                    model = null;
                }
            }
            return modeller;
        }
        public List<stokkart> StokTuruListesiniGetir(byte stokkart_tur_id)
        {
            List<stokkart> modeller = null;
            #region Query
            string query = @"
                           SELECT 
                            stokkart_id,
	                        stok_adi,
							stok_kodu
	                        FROM stokkart
							WHERE kayit_silindi=0 and stokkart_tur_id=@stokkart_tur_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_tur_id",stokkart_tur_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart model = null;
                modeller = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model = new stokkart();
                    model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    model.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    model.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    modeller.Add(model);
                    model = null;
                }
            }
            return modeller;
        }
        public List<stokkart> StokTuruListesiniGetir(string stok_adi, byte stokkart_tur_id)
        {
            List<stokkart> modeller = null;
            #region Query
            string query = @"
                           SELECT 
                            TOP 200
                            stokkart_id,
	                        stok_adi,
							stok_kodu
	                        FROM stokkart
							WHERE kayit_silindi=0 and stokkart_tur_id=@stokkart_tur_id and stok_adi like @stok_adi
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_tur_id",stokkart_tur_id),
               new SqlParameter("@stok_adi",stok_adi + "%"),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart model = null;
                modeller = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model = new stokkart();
                    model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    model.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    model.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    modeller.Add(model);
                    model = null;
                }
            }
            return modeller;
        }
        public List<stokkart> StokTuruListesiniGetirStokKoduIle(string stok_kodu, byte stokkart_tur_id)
        {
            List<stokkart> modeller = null;
            #region Query
            string query = @"
                           SELECT 
                            TOP 200
                            stokkart_id,
	                        stok_adi,
							stok_kodu
	                        FROM stokkart
							WHERE kayit_silindi=0 and stokkart_tur_id=@stokkart_tur_id and stok_kodu like @stok_kodu
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_tur_id",stokkart_tur_id),
               new SqlParameter("@stok_kodu",stok_kodu + "%"),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart model = null;
                modeller = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model = new stokkart();
                    model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    model.stok_adi = dt.Rows[i]["stok_adi"].ToString();
                    model.stok_kodu = dt.Rows[i]["stok_kodu"].ToString();
                    modeller.Add(model);
                    model = null;
                }
            }
            return modeller;
        }
        public List<cari_kart> FasoncuGetir()
        {
            List<cari_kart> fasoncular = null;
            #region Query
            string query = @"
                           SELECT 
                            carikart_id,
	                        cari_unvan
	                        from carikart
							where kayit_silindi=0 and carikart_tipi_id=14
                            ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                cari_kart fason = null;
                fasoncular = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fason = new cari_kart();
                    fason.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    fason.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    fasoncular.Add(fason);
                    fason = null;
                }
            }
            return fasoncular;
        }
        public stokkart_talimat StokkartTalimatDetay(long stokkart_id, short sira_id)
        {
            stokkart_talimat talimat = null;

            #region Query
            string query = @"
                        SELECT 
                                stokkart_id, 
                                sira_id, 
                                degistiren_carikart_id, 
                                degistiren_tarih, 
                                talimatturu_id, 
                                fasoncu_carikart_id, 
                                aciklama, 
                                irstalimat, 
                                islem_sayisi
                        FROM stokkart_talimat
                        WHERE stokkart_id = @stokkart_id AND sira_id = @sira_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@sira_id",sira_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                talimat = new stokkart_talimat();
                talimat.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                talimat.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                talimat.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                talimat.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                talimat.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToInt();
                talimat.fasoncu_carikart_id = dt.Rows[0]["fasoncu_carikart_id"].acekaToLongWithNullable();
                talimat.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                talimat.irstalimat = dt.Rows[0]["irstalimat"].acekaToString();
                talimat.islem_sayisi = dt.Rows[0]["islem_sayisi"].acekaToShortWithNullable();
            }
            return talimat;
        }
        public List<stokkart_talimat> StokkartTalimatlar(long stokkart_id, ref string errorMessage)
        {
            List<stokkart_talimat> talimatlar = null;

            #region Query
            string query = @"
                        --SELECT 
                          --      t.stokkart_id, 
                          --      t.sira_id, 
                          --      t.degistiren_carikart_id, 
                          --      t.degistiren_tarih, 
                          --      t.talimatturu_id, 
                          --      t.fasoncu_carikart_id, 
                          --      t.talimat_adi,
                          --      t.aciklama, 
                          --      t.irstalimat, 
                          --      t.islem_sayisi,
                          --      c.cari_unvan
                        --FROM stokkart_talimat t
                        --LEFT JOIN carikart c ON c.carikart_id=t.fasoncu_carikart_id
                        --WHERE stokkart_id = @stokkart_id 
                        SELECT 
                                st.stokkart_id, 
                                st.sira_id, 
                                st.degistiren_carikart_id, 
                                st.degistiren_tarih, 
                                st.talimatturu_id, 
                                st.fasoncu_carikart_id, 
                                t.tanim ,
                                st.aciklama, 
                                st.irstalimat, 
                                st.islem_sayisi,
                                c.cari_unvan
                        FROM stokkart_talimat st
                        LEFT JOIN carikart c ON c.carikart_id=st.fasoncu_carikart_id
						INNER JOIN talimat t ON t.talimatturu_id=st.talimatturu_id and t.kayit_silindi=0
                        WHERE stokkart_id = @stokkart_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    talimatlar = new List<stokkart_talimat>();
                    stokkart_talimat talimat = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        talimat = new stokkart_talimat();
                        talimat.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        talimat.sira_id = dt.Rows[i]["sira_id"].acekaToShort();
                        talimat.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        talimat.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        talimat.talimatturu_id = dt.Rows[i]["talimatturu_id"].acekaToInt();
                        talimat.talimat_adi = dt.Rows[i]["tanim"].ToString();
                        talimat.fasoncu_carikart_id = dt.Rows[i]["fasoncu_carikart_id"].acekaToLongWithNullable();
                        talimat.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                        talimat.irstalimat = dt.Rows[i]["irstalimat"].acekaToString();
                        talimat.islem_sayisi = dt.Rows[i]["islem_sayisi"].acekaToShortWithNullable();
                        talimat.cari_kart = new cari_kart();
                        talimat.cari_kart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        talimatlar.Add(talimat);
                        talimat = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


            return talimatlar;
        }
        public short StokkartTalimatEnBuyukSiraNo(long stokkart_id, ref string errorMessage)
        {
            short result = 0;

            #region Query
            string query = @"
                            SELECT 
                             MAX(sira_id) as 'MaxSiraId'
                            FROM stokkart_talimat
                            WHERE stokkart_id = @stokkart_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
            #endregion

            try
            {
                short.TryParse(SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).ToString(), out result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return result;

        }
        public short StokkartTalimatSiraID_Kontrol(long stokkart_id, short sira_id, ref string errorMessage)
        {
            short result = 0;

            #region Query
            string query = @"
                            SELECT 
	                            Count(*)
                            FROM stokkart_talimat
                            WHERE stokkart_id = @stokkart_id AND sira_id = @sira_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@sira_id",sira_id)
                };
            #endregion

            try
            {
                short.TryParse(SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).ToString(), out result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return result;

        }
        public stokkart_ozel StokkartOzelDetay(long stokkart_id)
        {
            stokkart_ozel ozel = null;

            #region Query
            string query = @"
                        SELECT 
                                stokkart_id,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                stok_adi_uzun,
                                orjinal_stok_kodu,
                                orjinal_stok_adi,
                                orjinal_renk_kodu,
                                orjinal_renk_adi,
                                tek_varyant,
                                gtipno,
                                urun_grubu,
                                birim1x,
                                birim2x,
                                birim3x,
                                M2_gram,
                                eni,
                                fyne,
                                fire_orani,
                                birim_gram
                        FROM stokkart_ozel
                        WHERE stokkart_id = @stokkart_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ozel = new stokkart_ozel();
                ozel.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                ozel.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ozel.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ozel.stok_adi_uzun = dt.Rows[0]["stok_adi_uzun"].acekaToString();
                ozel.orjinal_stok_kodu = dt.Rows[0]["orjinal_stok_kodu"].acekaToString();
                ozel.orjinal_stok_adi = dt.Rows[0]["orjinal_stok_adi"].acekaToString();
                ozel.orjinal_renk_kodu = dt.Rows[0]["orjinal_renk_kodu"].acekaToString();
                ozel.orjinal_renk_adi = dt.Rows[0]["orjinal_renk_adi"].acekaToString();
                ozel.tek_varyant = dt.Rows[0]["tek_varyant"].acekaToBoolWithNullable();
                ozel.gtipno = dt.Rows[0]["gtipno"].acekaToString();
                ozel.urun_grubu = dt.Rows[0]["urun_grubu"].acekaToString();
                ozel.birim1x = dt.Rows[0]["birim1x"].acekaToDoubleWithNullable();
                ozel.birim2x = dt.Rows[0]["birim2x"].acekaToDoubleWithNullable();
                ozel.birim3x = dt.Rows[0]["birim3x"].acekaToDoubleWithNullable();
                ozel.M2_gram = dt.Rows[0]["M2_gram"].acekaToDoubleWithNullable();
                ozel.eni = dt.Rows[0]["eni"].acekaToDoubleWithNullable();
                ozel.fyne = dt.Rows[0]["fyne"].acekaToDoubleWithNullable();
                ozel.fire_orani = dt.Rows[0]["fire_orani"].acekaToDoubleWithNullable();
                ozel.birim_gram = dt.Rows[0]["birim_gram"].acekaToDoubleWithNullable();
            }
            return ozel;
        }
        public stokkart_onay StokkartOnay(long stokkart_id)
        {
            stokkart_onay onay = null;

            #region Query
            string query = @"
                        SELECT 
                                *
                        FROM stokkart_onay
                        WHERE stokkart_id = @stokkart_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                onay = new stokkart_onay();
                onay.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                onay.genel_onay = dt.Rows[0]["genel_onay"].acekaToBool();
                onay.malzeme_onay = dt.Rows[0]["malzeme_onay"].acekaToBool();
                onay.yukleme_onay = dt.Rows[0]["yukleme_onay"].acekaToBool();
                onay.uretim_onay = dt.Rows[0]["uretim_onay"].acekaToBool();

            }
            return onay;
        }
        public stokkart StokkartKodu_Bul(string stok_kodu)
        {
            stokkart stk = null;
            #region Query
            string query = @"
                        SELECT stokkart_id
                        FROM stokkart
                        WHERE stok_kodu = @stok_kodu
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stok_kodu",stok_kodu),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stk = new stokkart();
                stk.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();

            }
            return stk;
        }

        public stokkart_onay_log StokkartOnayLog(long stokkart_id)
        {
            stokkart_onay_log onay = null;

            #region Query
            string query = @"
                        SELECT 
                                *
                        FROM stokkart_onay_log
                        WHERE stokkart_id = @stokkart_id --AND iptal_tarihi IS NULL
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                onay = new stokkart_onay_log();
                onay.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                onay.onay_alan_adi = "genel_onay"; //dt.Rows[0]["genel_onay"].acekaToBool();
                onay.iptal_carikart_id = dt.Rows[0]["iptal_carikart_id"].acekaToLong();
                onay.iptal_tarihi = dt.Rows[0]["iptal_tarihi"].acekaToDateTime();
                onay.onay_carikart_id = dt.Rows[0]["onay_carikart_id"].acekaToLong();
                onay.onay_tarihi = dt.Rows[0]["onay_tarihi"].acekaToDateTime();

            }
            return onay;
        }

        public List<stokkart_onay_log> StokkartOnayLoglari(long stokkart_id, CustomEnums.OnayLogTipi OnayLogTipi, ref string errorMessage)
        {
            List<stokkart_onay_log> loglar = null;
            try
            {

                #region Query
                string query = @"
                            SELECT 
	                            SKOnayLog.stokkart_id,
	                            SKOnayLog.onay_carikart_id,
	                            OC.cari_unvan as 'onaylayan_cari',
	                            SKOnayLog.onay_tarihi,
	                            SKOnayLog.iptal_carikart_id,
	                            IC.cari_unvan as 'iptal_eden_cari',
	                            SKOnayLog.iptal_tarihi
                            FROM stokkart_onay_log AS SKOnayLog
                            LEFT JOIN carikart AS OC ON OC.carikart_id = SKOnayLog.onay_carikart_id
                            LEFT JOIN carikart AS IC ON IC.carikart_id = SKOnayLog.iptal_carikart_id
                            WHERE stokkart_id = @stokkart_id AND onay_alan_adi = '" + OnayLogTipi.ToString() + "'";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    loglar = new List<stokkart_onay_log>();

                    stokkart_onay_log log = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        log = new stokkart_onay_log();
                        log.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();

                        log.onay_carikart_id = dt.Rows[i]["onay_carikart_id"].acekaToLongWithNullable();
                        log.onay_tarihi = dt.Rows[i]["onay_tarihi"].acekaToDateTime();
                        log.onaylayan_carikart = new cari_kart();
                        log.onaylayan_carikart.cari_unvan = dt.Rows[i]["onaylayan_cari"].acekaToString();

                        log.iptal_carikart_id = dt.Rows[i]["iptal_carikart_id"].acekaToLongWithNullable();
                        log.iptal_tarihi = dt.Rows[i]["iptal_tarihi"].acekaToDateTimeWithNullable();
                        log.iptal_eden_carikart = new cari_kart();
                        log.iptal_eden_carikart.cari_unvan = dt.Rows[i]["onaylayan_cari"].acekaToString();
                        loglar.Add(log);
                        log = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return loglar;
        }

        #region TAB

        #region Genel

        public stokkart_rapor_parametre Stokkart_Genel_Parametreler(long stokkart_id)
        {

            #region Query
            string query = @"
                            --Table[0]
                            SELECT
	                            S.stokkart_id,
	                            S.satici_carikart_id,
                                C.cari_unvan as satici_cari_unvan,
                                S.uretimyeri_id,
                                S.stokkart_tipi_id,
	                            SKP.stokalan_id_1,
	                            SKP.stokalan_id_2,
	                            SKP.stokalan_id_3,
	                            SKP.stokalan_id_4,
	                            SKP.stokalan_id_5,
	                            SKP.stokalan_id_6,
	                            SKP.stokalan_id_7,
	                            SKP.stokalan_id_8,
	                            SKP.stokalan_id_9,
	                            SKP.stokalan_id_10,
	                            SKP.stokalan_id_11,
	                            SKP.stokalan_id_12,
	                            SKP.stokalan_id_13,
	                            SKP.stokalan_id_14,
	                            SKP.stokalan_id_15,
	                            SKP.stokalan_id_16,
	                            SKP.stokalan_id_17,
	                            SKP.stokalan_id_18,
	                            SKP.stokalan_id_19,
	                            SKP.stokalan_id_20
                            FROM stokkart S
                            INNER JOIN stokkart_rapor_parametre SKP ON  SKP.stokkart_id = S.stokkart_id
                            LEFT JOIN carikart C ON C.carikart_id=S.satici_carikart_id
                            WHERE  S.stokkart_id = @stokkart_id AND ISNULL(S.kayit_silindi,0) = 0
                       
                       
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion

            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                stok_rapor_param = new stokkart_rapor_parametre();
                stok_rapor_param.stokkart_id = ds.Tables[0].Rows[0]["stokkart_id"].acekaToLong();
                stok_rapor_param.satici_carikart_id = ds.Tables[0].Rows[0]["satici_carikart_id"].acekaToLongWithNullable();
                stok_rapor_param.satici_cari_unvan = ds.Tables[0].Rows[0]["satici_cari_unvan"].acekaToString();
                stok_rapor_param.stokalan_id_1 = ds.Tables[0].Rows[0]["stokalan_id_1"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_2 = ds.Tables[0].Rows[0]["stokalan_id_2"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_3 = ds.Tables[0].Rows[0]["stokalan_id_3"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_4 = ds.Tables[0].Rows[0]["stokalan_id_4"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_5 = ds.Tables[0].Rows[0]["stokalan_id_5"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_6 = ds.Tables[0].Rows[0]["stokalan_id_6"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_7 = ds.Tables[0].Rows[0]["stokalan_id_7"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_8 = ds.Tables[0].Rows[0]["stokalan_id_8"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_9 = ds.Tables[0].Rows[0]["stokalan_id_9"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_10 = ds.Tables[0].Rows[0]["stokalan_id_10"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_11 = ds.Tables[0].Rows[0]["stokalan_id_11"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_12 = ds.Tables[0].Rows[0]["stokalan_id_12"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_13 = ds.Tables[0].Rows[0]["stokalan_id_13"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_14 = ds.Tables[0].Rows[0]["stokalan_id_14"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_15 = ds.Tables[0].Rows[0]["stokalan_id_15"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_16 = ds.Tables[0].Rows[0]["stokalan_id_16"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_17 = ds.Tables[0].Rows[0]["stokalan_id_17"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_18 = ds.Tables[0].Rows[0]["stokalan_id_18"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_19 = ds.Tables[0].Rows[0]["stokalan_id_19"].acekaToIntWithNullable();
                stok_rapor_param.stokalan_id_20 = ds.Tables[0].Rows[0]["stokalan_id_20"].acekaToIntWithNullable();
                stok_rapor_param.stokkart = new stokkart();
                stok_rapor_param.stokkart.stokkart_tipi_id = ds.Tables[0].Rows[0]["stokkart_tipi_id"].acekaToInt();
                stok_rapor_param.stokkart.uretimyeri_id = ds.Tables[0].Rows[0]["uretimyeri_id"].acekaToShort();
            }
            return stok_rapor_param;
        }


        public List<stokkart_ekler> Stokkart_Ekler(long stokkart_id)
        {
            List<stokkart_ekler> ekler = null;

            #region Query
            string query = @"
                            SELECT 
	                            stokkart_id, 
	                            ek_id, 
	                            degistiren_tarih, 
	                            degistiren_carikart_id
                            FROM stokkart_ekler
                            WHERE stokkart_id = @stokkart_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_ekler ek = null;
                ekler = new List<stokkart_ekler>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ek = new stokkart_ekler();
                    ek.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    ek.ek_id = dt.Rows[i]["ek_id"].acekaToInt();
                    ek.degistiren_tarih = dt.Rows[i]["stokkart_id"].acekaToDateTime();
                    ek.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    ekler.Add(ek);
                    ek = null;
                }
            }
            return ekler;
        }

        /// <summary>
        /// Şirket Listesini getiren Metod
        /// </summary>
        /// <returns></returns>
        public List<cari_kart> VarsayilanSaticiListesi()
        {
            #region Query
            string query = @"
                            --Table[0]
                            select k.carikart_id, k.cari_unvan,t.carikart_tipi_adi,k.kayit_silindi
                            FROM carikart k
                            INNER JOIN giz_sabit_carikart_tipi t on t.carikart_tipi_id=k.carikart_tipi_id 
                            where  t.carikart_tipi_id=12 OR t.carikart_tipi_id=13 order by t.carikart_tipi_adi
							
                ";
            #endregion
            #region Parameters
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                saticilist = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    satici = new cari_kart();
                    satici.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    satici.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    satici.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                    satici.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();
                    satici.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    saticilist.Add(satici);
                    satici = null;
                }

            }
            return saticilist;
        }
        #endregion

        #region Stokkart Varyant
        public List<stokkart_sku> VaryantListesi(long stokkart_id)
        {
            List<stokkart_sku> varyantlar = null;
            #region Query
            string query = @"
                            SELECT 
	                            SK_SKU.sku_id,
	                            SK_SKU.stokkart_id,
	                            SK_SKU.sku_no,
	                            SK_SKU.statu,
	                            SK_SKU.renk_id,
	                            R.renk_kodu,
	                            R.renk_adi,
	                            SK_SKU.beden_id
                            FROM stokkart_sku SK_SKU
                            LEFT JOIN parametre_renk R ON SK_SKU.renk_id = R.renk_id
                            WHERE SK_SKU.stokkart_id = @stokkart_id AND SK_SKU.kayit_silindi = 0 AND R.kayit_silindi = 0
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sku varyant = null;
                varyantlar = new List<stokkart_sku>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    varyant = new stokkart_sku();
                    varyant.sku_id = dt.Rows[i]["sku_id"].acekaToLong();
                    varyant.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    varyant.sku_no = dt.Rows[i]["sku_no"].acekaToString();
                    varyant.statu = dt.Rows[i]["statu"].acekaToBool();
                    varyant.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    varyant.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    varyant.renk = new parametre_renk();
                    varyant.renk.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    varyant.renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();
                    varyant.renk.renk_kodu = dt.Rows[i]["renk_kodu"].acekaToString();
                    varyantlar.Add(varyant);
                    varyant = null;
                }
            }

            return varyantlar;
        }
        public List<stokkart_sku> BedenListesi(long stokkart_id)
        {
            List<stokkart_sku> varyantlar = null;
            #region Query
            string query = @"
                            SELECT 
	                            SK_SKU.sku_id,
	                            SK_SKU.stokkart_id,
	                            SK_SKU.sku_no,
	                            SK_SKU.statu,
	                            SK_SKU.renk_id,
	                            B.beden_id,
								B.bedengrubu,
								B.beden,
	                            B.beden_tanimi,
								B.sira,
                                B.kayit_silindi,
                                B.degistiren_carikart_id,
                                B.degistiren_tarih
								FROM stokkart_sku SK_SKU
                            LEFT JOIN parametre_beden B ON SK_SKU.beden_id = B.beden_id
                            WHERE SK_SKU.stokkart_id = @stokkart_id AND SK_SKU.kayit_silindi = 0 AND B.kayit_silindi = 0
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sku varyant = null;
                varyantlar = new List<stokkart_sku>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    varyant = new stokkart_sku();
                    varyant.sku_id = dt.Rows[i]["sku_id"].acekaToLong();
                    varyant.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    varyant.sku_no = dt.Rows[i]["sku_no"].acekaToString();
                    varyant.statu = dt.Rows[i]["statu"].acekaToBool();
                    varyant.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    varyant.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    varyant.parametre_beden = new parametre_beden();
                    varyant.parametre_beden.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    varyant.parametre_beden.bedengrubu = dt.Rows[i]["bedengrubu"].acekaToString();
                    varyant.parametre_beden.beden_tanimi = dt.Rows[i]["beden_tanimi"].acekaToString();
                    varyant.parametre_beden.beden = dt.Rows[i]["beden"].acekaToString();
                    varyant.parametre_beden.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    varyant.parametre_beden.sira = dt.Rows[i]["sira"].acekaToInt();
                    varyant.parametre_beden.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    varyant.parametre_beden.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    varyantlar.Add(varyant);
                    varyant = null;
                }
            }

            return varyantlar;
        }

        public List<stokkart_sku> BedenListesi(long stokkart_id, int beden_id)
        {
            List<stokkart_sku> varyantlar = null;
            #region Query
            string query = @"
                            SELECT 
	                            SK_SKU.sku_id,
	                            SK_SKU.stokkart_id,
	                            SK_SKU.sku_no,
	                            SK_SKU.statu,
	                            SK_SKU.renk_id,
	                            B.beden_id,
								B.bedengrubu,
								B.beden,
	                            B.beden_tanimi,
								B.sira,
                                B.kayit_silindi,
                                B.degistiren_carikart_id,
                                B.degistiren_tarih
								FROM stokkart_sku SK_SKU
                            LEFT JOIN parametre_beden B ON SK_SKU.beden_id = B.beden_id
                            WHERE SK_SKU.stokkart_id = @stokkart_id AND SK_SKU.beden_id=@beden_id AND  SK_SKU.kayit_silindi = 0 AND B.kayit_silindi = 0
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@beden_id",beden_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sku varyant = null;
                varyantlar = new List<stokkart_sku>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    varyant = new stokkart_sku();
                    varyant.sku_id = dt.Rows[i]["sku_id"].acekaToLong();
                    varyant.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    varyant.sku_no = dt.Rows[i]["sku_no"].acekaToString();
                    varyant.statu = dt.Rows[i]["statu"].acekaToBool();
                    varyant.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    varyant.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    varyant.parametre_beden = new parametre_beden();
                    varyant.parametre_beden.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    varyant.parametre_beden.bedengrubu = dt.Rows[i]["bedengrubu"].acekaToString();
                    varyant.parametre_beden.beden_tanimi = dt.Rows[i]["beden_tanimi"].acekaToString();
                    varyant.parametre_beden.beden = dt.Rows[i]["beden"].acekaToString();
                    varyant.parametre_beden.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    varyant.parametre_beden.sira = dt.Rows[i]["sira"].acekaToInt();
                    varyant.parametre_beden.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    varyant.parametre_beden.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    varyantlar.Add(varyant);
                    varyant = null;
                }
            }

            return varyantlar;
        }

        public List<giz_setup_varyant_oto> OtoVaryantList()
        {
            List<giz_setup_varyant_oto> sku_oto_fieldlar = null;
            #region Query
            string query = @"
                            SELECT 
	                            sku_oto_field_id,
	                            statu,
	                            table_name,
	                            field_name,
	                            tanim
                            FROM giz_setup_varyant_oto 
                            ";
            #endregion

            #region Parameters
            //SqlParameter[] parameters = new SqlParameter[] {
            //   new SqlParameter("@sku_oto_field_id",sku_oto_field_id),
            //};
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                giz_setup_varyant_oto sku_oto_field = null;
                sku_oto_fieldlar = new List<giz_setup_varyant_oto>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    sku_oto_field = new giz_setup_varyant_oto();
                    sku_oto_field.sku_oto_field_id = dt.Rows[i]["sku_oto_field_id"].acekaToShort();
                    sku_oto_field.statu = dt.Rows[i]["statu"].acekaToBool();
                    sku_oto_field.table_name = dt.Rows[i]["table_name"].ToString();
                    sku_oto_field.field_name = dt.Rows[i]["field_name"].ToString();
                    sku_oto_field.tanim = dt.Rows[i]["tanim"].ToString();
                    sku_oto_fieldlar.Add(sku_oto_field);
                    sku_oto_field = null;
                }
            }

            return sku_oto_fieldlar;
        }

        public stokkart_sku VaryantDetay(long sku_id)
        {
            stokkart_sku varyant = null;
            #region Query
            string query = @"
                            SELECT 
	                            SK_SKU.sku_id,
	                            SK_SKU.stokkart_id,
	                            SK_SKU.sku_no,
	                            SK_SKU.statu,
	                            SK_SKU.renk_id,
	                            R.renk_kodu,
	                            R.renk_adi,
	                            SK_SKU.beden_id
                            FROM stokkart_sku SK_SKU
                            LEFT JOIN parametre_renk R ON SK_SKU.renk_id = R.renk_id
                            WHERE SK_SKU.sku_id = @sku_id  AND SK_SKU.kayit_silindi = 0 AND R.kayit_silindi = 0
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@sku_id",sku_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {

                varyant = new stokkart_sku();
                varyant.sku_id = dt.Rows[0]["sku_id"].acekaToLong();
                varyant.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                varyant.sku_no = dt.Rows[0]["sku_no"].acekaToString();
                varyant.statu = dt.Rows[0]["statu"].acekaToBool();
                varyant.renk_id = dt.Rows[0]["renk_id"].acekaToLong();
                varyant.beden_id = dt.Rows[0]["beden_id"].acekaToInt();
                varyant.renk = new parametre_renk();
                varyant.renk.renk_id = dt.Rows[0]["renk_id"].acekaToLong();
                varyant.renk.renk_adi = dt.Rows[0]["renk_adi"].acekaToString();
                varyant.renk.renk_kodu = dt.Rows[0]["renk_kodu"].acekaToString();
            }

            return varyant;
        }

        public stokkart_sku_oto sku_otobul(long stokkart_id, short sku_oto_field_id)
        {
            stokkart_sku_oto skoto = null;
            #region Query
            string query = @"
                            SELECT 
                                    SKUOTO.stokkart_id,
	                                SKUOTO.sku_oto_field_id
                            FROM stokkart_sku_oto SKUOTO
                            WHERE SKUOTO.stokkart_id= @stokkart_id AND SKUOTO.sku_oto_field_id = @sku_oto_field_id
                            ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@sku_oto_field_id",sku_oto_field_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                skoto = new stokkart_sku_oto();
                skoto.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                skoto.sku_oto_field_id = dt.Rows[0]["sku_oto_field_id"].acekaToShort();
            }
            return skoto;
        }

        public List<parametre_beden> BedenGetir()
        {
            List<parametre_beden> prbedenler = null;
            #region Query
            string query = @"
                           SELECT 
                                beden_id,
                                kayit_silindi,
                                bedengrubu,
                                beden,
                                beden_tanimi,
                                sira
                            FROM parametre_beden --GROUP BY bedengrubu 
                            ";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_beden beden = null;
                prbedenler = new List<parametre_beden>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    beden = new parametre_beden();
                    beden.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    //beden.degistiren_carikart_id = 0;
                    //beden.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    beden.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    beden.bedengrubu = dt.Rows[i]["bedengrubu"].ToString();
                    beden.beden = dt.Rows[i]["beden"].ToString();
                    beden.beden_tanimi = dt.Rows[i]["beden_tanimi"].ToString();
                    beden.sira = dt.Rows[i]["sira"].acekaToInt();
                    prbedenler.Add(beden);
                    beden = null;
                }
            }
            return prbedenler;
        }

        public List<stokkart_sku> BedenGetir(long stokkart_id)
        {
            List<stokkart_sku> prbedenler = null;
            #region Query
            string query = @"
                           SELECT 
                                sku_id,
                                stokkart_id,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                kayit_silindi,
                                sku_no,
                                renk_id,
                                beden_id,
                                asorti,
                                asorti_miktar
                            FROM stokkart_sku  where stokkart_id=@stokkart_id
                            ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sku beden = null;
                prbedenler = new List<stokkart_sku>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    beden = new stokkart_sku();
                    beden.sku_id = dt.Rows[i]["sku_id"].acekaToInt();
                    beden.degistiren_carikart_id = 0;
                    beden.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    beden.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    beden.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    beden.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    beden.beden_id = dt.Rows[i]["asorti_miktar"].acekaToInt();
                    beden.asorti = dt.Rows[i]["asorti"].acekaToBool();
                    beden.asorti_miktar = dt.Rows[i]["asorti_miktar"].acekaToDouble();
                    prbedenler.Add(beden);
                    beden = null;
                }
            }
            return prbedenler;
        }
        public List<stokkart_sku_oto> SkuOtoGetir(long stokkart_id)
        {
            List<stokkart_sku_oto> sku_oto_fieldlar = null;
            #region Query
            string query = @"
                            SELECT PS.stokkart_id,ps.sira_id,SKS.tanim,case when PS.sku_oto_field_id>0 then 'true' else 'false' end as secili,sks.sku_oto_field_id,o.tek_varyant  
                                    FROM 
                            		(SELECT S.*,row_number() OVER (ORDER BY S.sku_oto_field_id ASC) AS ortakNum
                                          FROM stokkart_sku_oto S
                            			 WHERE S.stokkart_id = @stokkart_id
                                    ) PS FULL OUTER JOIN
                                    (SELECT SS.*, row_number() OVER (ORDER by SS.sku_oto_field_id ASC) AS ortakNum
                                     FROM giz_setup_varyant_oto SS
                                     --where  sku_oto_field_id= SS.sku_oto_field_id
                                    ) SKS
                                    ON SKS.sku_oto_field_id = PS.sku_oto_field_id
                                    LEFT JOIN stokkart_ozel o ON o.stokkart_id=PS.stokkart_id --ORDER BY PS.sira_id
                            ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sku_oto sku_oto_field = null;
                sku_oto_fieldlar = new List<stokkart_sku_oto>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sku_oto_field = new stokkart_sku_oto();
                    sku_oto_field.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    sku_oto_field.sku_oto_field_id = dt.Rows[i]["sku_oto_field_id"].acekaToShort();
                    sku_oto_field.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    sku_oto_field.sira_id = dt.Rows[i]["sira_id"].acekaToByte();
                    sku_oto_field.stokkart_ozel = new stokkart_ozel();
                    sku_oto_field.stokkart_ozel.tek_varyant = dt.Rows[i]["tek_varyant"].acekaToBool();
                    sku_oto_field.giz_setup_varyant_oto = new giz_setup_varyant_oto();
                    sku_oto_field.giz_setup_varyant_oto.tanim = dt.Rows[i]["tanim"].ToString();
                    sku_oto_field.secili = dt.Rows[i]["secili"].acekaToBool();
                    sku_oto_fieldlar.Add(sku_oto_field);
                    sku_oto_field = null;
                }
            }
            return sku_oto_fieldlar;
        }
        #endregion

        #region Stokkart Diğer

        public List<stokkart_sezon> StokkartSezonListesi(long stokkart_id)
        {
            List<stokkart_sezon> sezonlar = null;
            #region Query
            // 28.02.2017 Adnan TÜRK. Yeni sorgu ile değişti
            //string query = @"
            //            SELECT 
            //             SS.stokkart_id,
            //             SS.sezon_id,
            //             SS.statu,
            //             PS.sezon_adi,
            //             PS.sezon_kodu
            //            FROM stokkart_sezon SS
            //            LEFT JOIN parametre_sezon PS ON SS.sezon_id=PS.sezon_id
            //            WHERE SS.stokkart_id = @stokkart_id 
            //                ";
            string query = @"
                            SELECT PS.sezon_id, PS.sezon_adi,PS.sezon_kodu,SKS.stokkart_id
                            FROM (SELECT S.*, row_number() OVER (ORDER BY S.sezon_id DESC) AS ortakNum
                                  FROM parametre_sezon S where kayit_silindi=0
                                 ) PS FULL outer join
                                 (SELECT SS.*, row_number() OVER (ORDER by SS.sezon_id DESC) AS ortakNum
                                  FROM stokkart_sezon SS
	                              WHERE SS.stokkart_id = @stokkart_id
                                 ) SKS
                                 --ON SKS.ortakNum = PS.ortakNum
                                  ON SKS.sezon_id = PS.sezon_id";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_sezon sezon = null;
                sezonlar = new List<stokkart_sezon>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    sezon = new stokkart_sezon();
                    sezon.sezon_id = dt.Rows[i]["sezon_id"].acekaToShort();
                    sezon.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    //sezon.statu = dt.Rows[i]["statu"].acekaToBool();

                    sezon.sezon = new parametre_sezon();
                    sezon.sezon.sezon_id = dt.Rows[i]["sezon_id"].acekaToShort();
                    sezon.sezon.sezon_adi = dt.Rows[i]["sezon_adi"].acekaToString();
                    sezon.sezon.sezon_kodu = dt.Rows[i]["sezon_kodu"].acekaToString();
                    sezonlar.Add(sezon);
                    sezon = null;
                }
            }

            return sezonlar;
        }

        public stokkart_sezon StokkartSezonDetay(long stokkart_id, short sezon_id)
        {
            stokkart_sezon sezon = null;
            #region Query
            string query = @"
                        SELECT 
	                        SS.stokkart_id,
	                        SS.sezon_id,
	                        SS.statu,
	                        PS.sezon_adi,
	                        PS.sezon_kodu
                        FROM stokkart_sezon SS
                        INNER JOIN parametre_sezon PS ON SS.sezon_id=PS.sezon_id
                        WHERE SS.stokkart_id = @stokkart_id AND SS.sezon_id = @sezon_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@sezon_id",sezon_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                sezon = new stokkart_sezon();
                sezon.sezon_id = dt.Rows[0]["sezon_id"].acekaToShort();
                sezon.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                sezon.statu = dt.Rows[0]["statu"].acekaToBool();

                sezon.sezon = new parametre_sezon();
                sezon.sezon.sezon_id = dt.Rows[0]["sezon_id"].acekaToShort();
                sezon.sezon.sezon_adi = dt.Rows[0]["sezon_adi"].acekaToString();
                sezon.sezon.sezon_kodu = dt.Rows[0]["sezon_kodu"].acekaToString();
            }

            return sezon;
        }

        public List<stokkart_muadil> StokkartMuadilListesi(long stokkart_id)
        {
            List<stokkart_muadil> muadiller = null;
            #region Query
            string query = @"
                        SELECT 
	                        M.stokkart_id,
	                        M.muadil_stokkart_id,
	                        S.stok_adi as tanim,M.degistiren_carikart_id,M.degistiren_tarih
                        FROM stokkart_muadil M
                        INNER JOIN stokkart S ON S.stokkart_id =M.muadil_stokkart_id
                        WHERE M.stokkart_id = @stokkart_id AND M.statu = 1
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stokkart_muadil muadil = null;
                muadiller = new List<stokkart_muadil>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    muadil = new stokkart_muadil();
                    muadil.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    muadil.muadil_stokkart_id = dt.Rows[i]["muadil_stokkart_id"].acekaToLong();
                    muadil.tanim = dt.Rows[i]["tanim"].acekaToString();
                    muadil.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                    muadil.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                    muadiller.Add(muadil);
                    muadil = null;
                }
            }

            return muadiller;
        }

        public stokkart_muadil StokkartMuadilDetay(long stokkart_id, long muadil_stokkart_id)
        {
            stokkart_muadil muadil = null;
            #region Query
            string query = @"
                        SELECT 
	                        M.stokkart_id,
	                        M.muadil_stokkart_id,
	                        S.stok_adi as Tanim
                        FROM stokkart_muadil M
                        INNER JOIN stokkart S ON S.stokkart_id =M.stokkart_id
                        WHERE M.stokkart_id = @stokkart_id AND M.muadil_stokkart_id = @muadil_stokkart_id AND M.statu = 1
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@muadil_stokkart_id",muadil_stokkart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                muadil = new stokkart_muadil();
                muadil.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                muadil.muadil_stokkart_id = dt.Rows[0]["muadil_stokkart_id"].acekaToLong();
                muadil.tanim = dt.Rows[0]["Tanim"].acekaToString();

            }

            return muadil;
        }

        public stokkart_muadil StokkartOzelDetay(long stokkart_id, long muadil_stokkart_id)
        {
            stokkart_muadil muadil = null;
            #region Query
            string query = @"
                        SELECT 
	                        M.stokkart_id,
	                        M.muadil_stokkart_id,
	                        S.stok_adi as Tanim
                        FROM stokkart_muadil M
                        INNER JOIN stokkart S ON S.stokkart_id =M.stokkart_id
                        WHERE M.stokkart_id = @stokkart_id AND M.muadil_stokkart_id = @muadil_stokkart_id AND M.statu = 1
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id),
               new SqlParameter("@muadil_stokkart_id",muadil_stokkart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                muadil = new stokkart_muadil();
                muadil.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                muadil.muadil_stokkart_id = dt.Rows[0]["muadil_stokkart_id"].acekaToLong();
                muadil.tanim = dt.Rows[0]["Tanim"].acekaToString();

            }

            return muadil;
        }
        #endregion
        #region Stokkart_fiyat
        public List<stokkart_fiyat> StokkartFiyat(long stokkart_id)
        {
            List<stokkart_fiyat> fiyatlar = null;
            #region Query
            string query = @"
                            SELECT 
 			                    SF.tarih,SF.stokkart_id,
			                    (case when FT.fiyattipi='AF' then SF.fiyat else '-' end) as alis_fiyati,
			                    (case when FT.fiyattipi='SF' then SF.fiyat else '-' end) as satis_fiyati,
                                FT.fiyattipi_adi,SF.fiyat,PB.pb,PB.pb_kodu,PB.pb_adi,
                                SF.fiyattipi,SF.degistiren_carikart_id
                                FROM stokkart_fiyat SF
                                INNER JOIN parametre_parabirimi PB On SF.pb=PB.pb
                                INNER JOIN parametre_fiyattipi FT On FT.fiyattipi=SF.fiyattipi
                            WHERE FT.fiyattipi IN ('AF','SF') AND SF.stokkart_id = @stokkart_id 
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {

                stokkart_fiyat fiyat = null;
                fiyatlar = new List<stokkart_fiyat>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fiyat = new stokkart_fiyat();
                    fiyat.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    fiyat.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
                    fiyat.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    fiyat.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
                    fiyat.pb_kodu = dt.Rows[i]["pb_kodu"].ToString();
                    fiyat.pb = dt.Rows[i]["pb"].acekaToInt();
                    fiyat.fiyat = dt.Rows[i]["fiyat"].acekaToDecimal();
                    fiyat.alis_fiyati = dt.Rows[i]["alis_fiyati"].acekaToDecimal();
                    fiyat.satis_fiyati = dt.Rows[i]["satis_fiyati"].acekaToDecimal();
                    fiyatlar.Add(fiyat);
                    fiyat = null;
                }

            }
            return fiyatlar;
        }
        public stokkart_fiyat StkkartFiyat(long stokkart_id)
        {
            #region Query
            string query = @"
                            SELECT 
 			                    SF.tarih,SF.stokkart_id,
                                FT.fiyattipi_adi,SF.fiyat,PB.pb,PB.pb_kodu,PB.pb_adi,
                                SF.fiyattipi,SF.degistiren_carikart_id
                                FROM stokkart_fiyat SF
                                INNER JOIN parametre_parabirimi PB On SF.pb=PB.pb
                                INNER JOIN parametre_fiyattipi FT On FT.fiyattipi=SF.fiyattipi
                            WHERE FT.fiyattipi IN ('AF','SF') AND SF.stokkart_id = @stokkart_id 
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            stokkart_fiyat fiyat = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                fiyat = new stokkart_fiyat();
                fiyat.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                fiyat.tarih = dt.Rows[0]["tarih"].acekaToDateTime();
                fiyat.fiyattipi = dt.Rows[0]["fiyattipi"].ToString();
                fiyat.fiyattipi_adi = dt.Rows[0]["fiyattipi_adi"].ToString();
                fiyat.pb_kodu = dt.Rows[0]["pb_kodu"].ToString();
                fiyat.pb = dt.Rows[0]["pb"].acekaToInt();
                fiyat.fiyat = dt.Rows[0]["fiyat"].acekaToDecimal();

            }
            return fiyat;
        }
        #endregion
        #region stokkart_fiyat_talimat
        public List<stokkart_fiyat_talimat> StokkartFiyatTalimatlariGetir(long stokkart_id, ref string errorMessage)
        {

            List<stokkart_fiyat_talimat> talimatlar = null;

            try
            {
                #region Query
                string query = @"
                            IF OBJECT_ID('tempdb.dbo.#Temp', 'U') IS NOT NULL
                              DROP TABLE #Temp;

                            create table #Temp
                            (
                            talimatturu_id		int, 
                            tarih				Datetime,
                            stokkart_id			bigint,
                            carikart_id			BigInt,
                            kod					varchar(10),
                            varsayilan			bit,
                            tanim				nvarchar(100),
                            sira				int,
                            renk_rgb			int,
                            kesim				bit,
                            dikim				bit,
                            parca				bit,
                            model				bit,
                            stokkart_tipi_id	tinyint,
                            onayoto				bit,
                            parcamodel_giris	bit,
                            parcamodel_cikis	bit,
                            model_zorunlu		bit,
                            varsayilan_fasoncu	bigint,
                            kdv_tevkifat		tinyint,	
                            fiyat				money,
                            pb					tinyint,
							pb_kodu				varchar(5)
                            )


                            DECLARE @telimatSayisi int
                            SET @telimatSayisi = 1
                            WHILE @telimatSayisi <=9

	                            BEGIN
	                            INSERT INTO  #Temp
	                            SELECT
		                            T.talimatturu_id,
		                            SFT.tarih,
                                    SFT.stokkart_id,
		                            SFT.carikart_id,
		                            T.kod,
		                            T.varsayilan,
		                            T.tanim,
		                            T.sira,
		                            T.renk_rgb,
		                            T.kesim,
		                            T.dikim,
		                            T.parca,
		                            T.model,
		                            T.stokkart_tipi_id,
		                            T.onayoto,
		                            T.parcamodel_giris,
		                            T.parcamodel_cikis,
		                            T.model_zorunlu,
		                            T.varsayilan_fasoncu,
		                            T.kdv_tevkifat,
		                            SFT.fiyat,
		                            SFT.pb,
									PB.pb_kodu
		                            FROM stokkart_fiyat_talimat SFT
									INNER JOIN parametre_parabirimi PB ON SFT.pb = PB.pb
		                            INNER JOIN talimat T ON 
		                            (
		                            CASE
		                            WHEN @telimatSayisi = 1
			                            THEN SFT.talimatturu_id_1
		                            WHEN @telimatSayisi = 2
			                            THEN SFT.talimatturu_id_2
		                            WHEN @telimatSayisi = 3
			                            THEN SFT.talimatturu_id_3
		                            WHEN @telimatSayisi = 4
			                            THEN SFT.talimatturu_id_4
		                            WHEN @telimatSayisi = 5
			                            THEN SFT.talimatturu_id_5
		                            WHEN @telimatSayisi = 6
			                            THEN SFT.talimatturu_id_6
		                            WHEN @telimatSayisi = 7
			                            THEN SFT.talimatturu_id_7
		                            WHEN @telimatSayisi = 8
			                            THEN SFT.talimatturu_id_8
		                            WHEN @telimatSayisi = 9
			                            THEN SFT.talimatturu_id_9

		                            END) = 
		                            T.talimatturu_id  
		                            WHERE SFT.stokkart_id =  @stokkart_id  AND ISNULL(T.kayit_silindi,0) = 0 AND T.statu = 1

		                            SET @telimatSayisi = @telimatSayisi + 1
                            END
                            SELECT * FROM #Temp
                            DROP TABLE #Temp
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                   new SqlParameter("@stokkart_id",stokkart_id)
                };
                #endregion
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {

                    stokkart_fiyat_talimat fiyatTalimat = null;
                    talimatlar = new List<stokkart_fiyat_talimat>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fiyatTalimat = new stokkart_fiyat_talimat();
                        fiyatTalimat.stokkart_id = stokkart_id;
                        fiyatTalimat.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        fiyatTalimat.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
                        fiyatTalimat.fiyat = dt.Rows[i]["fiyat"].acekaToDecimal();
                        fiyatTalimat.pb = dt.Rows[i]["pb"].acekaToInt();
                        fiyatTalimat.parabirimi = new parametre_parabirimi();
                        fiyatTalimat.parabirimi.pb = dt.Rows[i]["pb"].acekaToByte();
                        fiyatTalimat.parabirimi.pb_kodu = dt.Rows[i]["pb_kodu"].acekaToString();

                        fiyatTalimat.talimat = new talimat();
                        fiyatTalimat.talimat.talimatturu_id = dt.Rows[i]["talimatturu_id"].acekaToInt();
                        fiyatTalimat.talimat.kod = dt.Rows[i]["kod"].acekaToString();
                        fiyatTalimat.talimat.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                        fiyatTalimat.talimat.tanim = dt.Rows[i]["tanim"].acekaToString();
                        fiyatTalimat.talimat.sira = dt.Rows[i]["sira"].acekaToInt();
                        fiyatTalimat.talimat.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToIntWithNullable();
                        fiyatTalimat.talimat.kesim = dt.Rows[i]["kesim"].acekaToBool();
                        fiyatTalimat.talimat.dikim = dt.Rows[i]["dikim"].acekaToBool();
                        fiyatTalimat.talimat.parca = dt.Rows[i]["parca"].acekaToBool();
                        fiyatTalimat.talimat.model = dt.Rows[i]["model"].acekaToBool();
                        fiyatTalimat.talimat.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
                        fiyatTalimat.talimat.onayoto = dt.Rows[i]["onayoto"].acekaToBool();
                        fiyatTalimat.talimat.parcamodel_giris = dt.Rows[i]["parcamodel_giris"].acekaToBool();
                        fiyatTalimat.talimat.parcamodel_cikis = dt.Rows[i]["parcamodel_cikis"].acekaToBool();
                        fiyatTalimat.talimat.model_zorunlu = dt.Rows[i]["model_zorunlu"].acekaToBool();
                        fiyatTalimat.talimat.varsayilan_fasoncu = dt.Rows[i]["varsayilan_fasoncu"].acekaToLongWithNullable();
                        fiyatTalimat.talimat.kdv_tevkifat = dt.Rows[i]["kdv_tevkifat"].acekaToByteWithNullable();

                        talimatlar.Add(fiyatTalimat);
                        fiyatTalimat = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return talimatlar;
        }
        //public List<stokkart_fiyat_talimat> StokkartFiyatTalimat(long stokkart_id)
        //{
        //    List<stokkart_fiyat_talimat> fiyatlar = null;
        //    #region Query
        //    string query = @"
        //                    SELECT 
        //                        FT.fiyattipi_adi,SF.tarih,SF.fiyat,PB.pb,PB.pb_kodu,PB.pb_adi,SF.stokkart_id,SF.fiyattipi,SF.degistiren_carikart_id
        //                        FROM stokkart_fiyat SF
        //                        INNER JOIN parametre_parabirimi PB On SF.pb=PB.pb
        //                        INNER JOIN parametre_fiyattipi FT On FT.fiyattipi=SF.fiyattipi
        //                    WHERE SF.stokkart_id = @stokkart_id 
        //                ";
        //    #endregion

        //    #region Parameters
        //    SqlParameter[] parameters = new SqlParameter[] {
        //       new SqlParameter("@stokkart_id",stokkart_id)
        //    };
        //    #endregion
        //    dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

        //    if (dt != null && dt.Rows.Count > 0)
        //    {

        //        stokkart_fiyat_talimat fiyat = null;
        //        fiyatlar = new List<stokkart_fiyat_talimat>();

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            fiyat = new stokkart_fiyat_talimat();
        //            fiyat.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
        //            //fiyat.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
        //            fiyat.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
        //            //fiyat.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
        //            fiyat.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
        //            fiyat.fiyat = dt.Rows[i]["fiyat"].acekaToDecimal();
        //            //fiyat.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
        //            //fiyat.pb_adi = dt.Rows[i]["pb_adi"].ToString();
        //            fiyat.pb = dt.Rows[i]["pb"].acekaToInt();
        //            //fiyat.pb_kodu = dt.Rows[i]["pb_kodu"].ToString();
        //            //fiyat.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
        //            fiyatlar.Add(fiyat);
        //            fiyat = null;
        //        }

        //    }
        //    return fiyatlar;
        //}
        #endregion

        #region Stokkart Uyarılar
        public stokkart_kontrol StokkartUyariGetir(long stokkart_id)
        {
            stokkart_kontrol uyari = null;
            #region Query
            string query = @"
                            SELECT 
                                stokkart_id,
                                degistiren_tarih,
                                degistiren_carikart_id,
                                tedarik_edilemez,
                                musteri_siparisi_icin_acik,
                                eksi_stok_izin,
                                eksi_stok_uyari,
                                min_stok_uyari,
                                satin_alma_testi_gerekli_uyari,
                                her_sezon_onay_gerekli,
                                beden_bazli_kullanim,
                                sezon_onayi_yok_uyarisi
                                FROM stokkart_kontrol
                            WHERE stokkart_id = @stokkart_id 
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@stokkart_id",stokkart_id)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                uyari = new stokkart_kontrol();
                uyari.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                uyari.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                uyari.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                uyari.min_stok_uyari = dt.Rows[0]["min_stok_uyari"].acekaToBool();
                uyari.her_sezon_onay_gerekli = dt.Rows[0]["her_sezon_onay_gerekli"].acekaToBool();
                uyari.beden_bazli_kullanim = dt.Rows[0]["beden_bazli_kullanim"].acekaToBool();
                uyari.eksi_stok_izin = dt.Rows[0]["eksi_stok_izin"].acekaToBool();
                uyari.eksi_stok_uyari = dt.Rows[0]["eksi_stok_uyari"].acekaToBool();
                uyari.satin_alma_testi_gerekli_uyari = dt.Rows[0]["satin_alma_testi_gerekli_uyari"].acekaToBool();
                uyari.tedarik_edilemez = dt.Rows[0]["tedarik_edilemez"].acekaToBool();
                uyari.musteri_siparisi_icin_acik = dt.Rows[0]["musteri_siparisi_icin_acik"].acekaToBool();
                uyari.sezon_onayi_yok_uyarisi = dt.Rows[0]["sezon_onayi_yok_uyarisi"].acekaToBool();

            }
            return uyari;
        }
        #endregion
        #endregion

        /*Model Kart için gerekli bazı metodlar*/
        public List<stokkart_model> StokkartModelListesiniGetir(long stokkart_id, byte stokkart_tipi_id, ref string errorMessage)
        {
            List<stokkart_model> modeller = null;
            try
            {

                #region Query
                string query = @"
                                SELECT
	                                SM.stokkart_id, 
	                                SM.sira_id, 
	                                SM.beden_id, 
	                                SM.degistiren_carikart_id, 
	                                SM.degistiren_tarih, 
	                                SM.talimatturu_id, 
	                                SM.modelyeri, 
	                                SM.alt_stokkart_id, 
	                                SM.renk_id, 
	                                SM.sira, 
	                                SM.ana_kayit, 
	                                SM.aciklama, 
	                                SM.birim_id, 
	                                SM.birim_id3, 
	                                SM.miktar, 
	                                SM.miktar3
                                FROM [dbo].[stokkart_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                                WHERE SM.stokkart_id = @stokkart_id AND SK.stokkart_tipi_id = @stokkart_tipi_id
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@stokkart_tipi_id",stokkart_tipi_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    modeller = new List<stokkart_model>();

                    stokkart_model model = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        model = new stokkart_model();
                        model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        model.sira_id = dt.Rows[i]["sira_id"].acekaToShort();
                        model.beden_id = dt.Rows[i]["beden_id"].acekaToShort();
                        model.talimatturu_id = dt.Rows[i]["talimatturu_id"].acekaToByte();
                        model.modelyeri = dt.Rows[i]["modelyeri"].acekaToString();
                        model.alt_stokkart_id = dt.Rows[i]["alt_stokkart_id"].acekaToLong();
                        model.renk_id = dt.Rows[i]["renk_id"].acekaToInt();
                        model.ana_kayit = dt.Rows[i]["ana_kayit"].acekaToByte();
                        model.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                        model.birim_id = dt.Rows[i]["birim_id"].acekaToByte();
                        model.birim_id3 = dt.Rows[i]["birim_id3"].acekaToByteWithNullable();
                        model.miktar = dt.Rows[i]["miktar"].acekaToFloat();
                        model.miktar3 = dt.Rows[i]["miktar3"].acekaToFloat();
                        modeller.Add(model);
                        model = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return modeller;
        }

        /*Model Kart için gerekli bazı metodlar*/
        public DataTable StokkartPivotModelListesiniGetir(long stokkart_id, byte stokkart_tipi_id, ref string errorMessage)
        {
            try
            {
                #region Query
                string query = @"SP_GetModelKartIlkmaddePivot";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@stokkart_tipi_id",stokkart_tipi_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, query, parameters).Tables[0];
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return dt;
        }

        public stokkart_model StokkartModelDetayiniGetir(long stokkart_id, short sira_id, ref string errorMessage)
        {
            stokkart_model model = null;
            try
            {

                #region Query
                string query = @"
                                SELECT
	                                SM.stokkart_id, 
	                                SM.sira_id, 
	                                SM.beden_id, 
	                                SM.degistiren_carikart_id, 
	                                SM.degistiren_tarih, 
	                                SM.talimatturu_id, 
	                                SM.modelyeri, 
	                                SM.alt_stokkart_id, 
	                                SM.renk_id, 
	                                SM.sira, 
	                                SM.ana_kayit, 
	                                SM.aciklama, 
	                                SM.birim_id, 
	                                SM.birim_id3, 
	                                SM.miktar, 
	                                SM.miktar3
                                FROM [dbo].[stokkart_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                                WHERE SM.stokkart_id = @stokkart_id AND SM.sira_id = @sira_id
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@sira_id",sira_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {

                    model = new stokkart_model();
                    model.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                    model.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                    model.beden_id = dt.Rows[0]["beden_id"].acekaToShort();
                    model.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToByte();
                    model.modelyeri = dt.Rows[0]["modelyeri"].acekaToString();
                    model.alt_stokkart_id = dt.Rows[0]["alt_stokkart_id"].acekaToLong();
                    model.renk_id = dt.Rows[0]["renk_id"].acekaToInt();
                    model.ana_kayit = dt.Rows[0]["ana_kayit"].acekaToByte();
                    model.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                    model.birim_id = dt.Rows[0]["birim_id"].acekaToByte();
                    model.birim_id3 = dt.Rows[0]["birim_id3"].acekaToByteWithNullable();
                    model.miktar = dt.Rows[0]["miktar"].acekaToFloat();
                    model.miktar3 = dt.Rows[0]["miktar3"].acekaToFloat();

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return model;
        }

        public stokkart_model StokkartModelDetayiniGetir(long stokkart_id, short sira_id, short beden_id)
        {
            stokkart_model model = null;

            #region Query
            string query = @"
                                SELECT
	                                SM.stokkart_id, 
	                                SM.sira_id, 
	                                SM.beden_id, 
	                                SM.degistiren_carikart_id, 
	                                SM.degistiren_tarih, 
	                                SM.talimatturu_id, 
	                                SM.modelyeri, 
	                                SM.alt_stokkart_id, 
	                                SM.renk_id, 
	                                SM.sira, 
	                                SM.ana_kayit, 
	                                SM.aciklama, 
	                                SM.birim_id, 
	                                SM.birim_id3, 
	                                SM.miktar, 
	                                SM.miktar3
                                FROM [dbo].[stokkart_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                                WHERE SM.stokkart_id = @stokkart_id AND SM.sira_id = @sira_id and SM.beden_id = @beden_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@sira_id",sira_id),
                    new  SqlParameter("@beden_id",beden_id)
                };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {

                model = new stokkart_model();
                model.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                model.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                model.beden_id = dt.Rows[0]["beden_id"].acekaToShort();
                model.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToByte();
                model.modelyeri = dt.Rows[0]["modelyeri"].acekaToString();
                model.alt_stokkart_id = dt.Rows[0]["alt_stokkart_id"].acekaToLong();
                model.renk_id = dt.Rows[0]["renk_id"].acekaToInt();
                model.ana_kayit = dt.Rows[0]["ana_kayit"].acekaToByte();
                model.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                model.birim_id = dt.Rows[0]["birim_id"].acekaToByte();
                model.birim_id3 = dt.Rows[0]["birim_id3"].acekaToByteWithNullable();
                model.miktar = dt.Rows[0]["miktar"].acekaToFloat();
                model.miktar3 = dt.Rows[0]["miktar3"].acekaToFloat();
            }

            return model;
        }

        public short StokkartModelEnBuyukSiraNo(long stokkart_id, ref string errorMessage)
        {
            short result = 0;

            #region Query
            string query = @"
                                SELECT 
	                            ISNULL(MAX(SM.sira_id),0) as MaxSiraId
                                FROM [dbo].[stokkart_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                            WHERE SM.stokkart_id = @stokkart_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
            #endregion

            try
            {
                short.TryParse(SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).ToString(), out result);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return result;

        }

        public List<stokkart_olcu> StokkartModelOlcuListesi(long stokkart_id, ref string errorMessage)
        {
            List<stokkart_olcu> olculer = null;
            try
            {

                #region Query
                string query = @"
                            SELECT 
                                O.olcu_id,
	                            O.stokkart_id, 
	                            O.olcuyeri, 
	                            O.beden_id, 
	                            O.degistiren_carikart_id, 
	                            O.degistiren_tarih, 
	                            O.deger, 
	                            O.birim_id
                            FROM stokkart_olcu O
                            INNER JOIN parametre_beden B On B.beden_id=O.beden_id
                            WHERE O.stokkart_id = @stokkart_id
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    olculer = new List<stokkart_olcu>();

                    stokkart_olcu olcu = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        olcu = new stokkart_olcu();
                        olcu.olcu_id = dt.Rows[i]["olcu_id"].acekaToInt();
                        olcu.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        olcu.olcuyeri = dt.Rows[i]["olcuyeri"].acekaToString();
                        olcu.beden_id = dt.Rows[i]["beden_id"].acekaToShort();
                        olcu.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        olcu.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        olcu.deger = dt.Rows[i]["deger"].acekaToFloat();
                        olcu.birim_id = dt.Rows[i]["birim_id"].acekaToByte();
                        olcu.parametrebeden = new parametre_beden();
                        olcu.parametrebeden.beden_tanimi = dt.Rows[i]["beden_tanimi"].acekaToString();
                        olculer.Add(olcu);
                        olcu = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return olculer;
        }

        public List<parametre_beden> StokkartModelOlcuBedenleri(long stokkart_id, ref string errorMessage)
        {
            List<parametre_beden> bedenler = null;
            try
            {

                #region Query
                string query = @"
                                SELECT 
	                                PB.beden_id,
	                                PB.bedengrubu,
	                                PB.beden 
                                FROM stokkart SK
                                INNER JOIN stokkart_sku SK_Sku ON SK.stokkart_id = SK_Sku.stokkart_id
                                INNER JOIN parametre_beden PB ON PB.beden_id = SK_Sku.beden_id
                                WHERE SK_Sku.stokkart_id = @stokkart_id
                                ORDER BY PB.bedengrubu, PB.sira 
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    bedenler = new List<parametre_beden>();

                    parametre_beden beden = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        beden = new parametre_beden();
                        beden.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                        beden.bedengrubu = dt.Rows[i]["bedengrubu"].acekaToString();
                        beden.beden = dt.Rows[i]["beden"].acekaToString();
                        bedenler.Add(beden);
                        beden = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return bedenler;
        }


        /*
        /// <summary>
        /// Model ve stokkart a ait bedenlere ait ölçülerin getirildiği metod
        /// </summary>
        /// <param name="stokkart_id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public DataTable StokkartPivotOlcuListesiniGetir(long stokkart_id, ref string errorMessage)
        {
            try
            {
                #region Query
                string query = @"SP_GetModelKartOlculerPivot";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, query, parameters).Tables[0];
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return dt;
        }*/

        public DataTable StokkartPivotOlcuListesiniGetir(long stokkart_id)
        {
            #region Query
            string query = @"SP_GetModelKartOlculerPivot";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id)
                };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, query, parameters).Tables[0];

            return dt;
        }


        public List<stokkart_olcu> StokkartModelOlcuKontrol(long stokkart_id, int beden_id, string olcuyeri, ref string errorMessage)
        {
            List<stokkart_olcu> olculer = null;
            try
            {

                #region Query
                string query = @"
                            SELECT 
	                            stokkart_id, 
	                            olcuyeri, 
	                            beden_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            deger, 
	                            birim_id
                            FROM stokkart_olcu
                            WHERE stokkart_id = @stokkart_id AND beden_id=@beden_id AND olcuyeri=@olcuyeri
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new SqlParameter("@beden_id",beden_id),
                    new SqlParameter("@olcuyeri",olcuyeri)

                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    olculer = new List<stokkart_olcu>();

                    stokkart_olcu olcu = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        olcu = new stokkart_olcu();
                        olcu.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        olcu.olcuyeri = dt.Rows[i]["olcuyeri"].acekaToString();
                        olcu.beden_id = dt.Rows[i]["beden_id"].acekaToShort();
                        olcu.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        olcu.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        olcu.deger = dt.Rows[i]["deger"].acekaToFloat();
                        olcu.birim_id = dt.Rows[i]["birim_id"].acekaToByte();
                        olculer.Add(olcu);
                        olcu = null;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return olculer;
        }

        public List<stokkart> ModelGetir(string modelozellik, int marka, int uretimyeri)
        {//bu sorgu düzenlenecek henüz tamam değil duruma göre değişiklik yapılabilir

            string query = "";
            #region sorgu bu şekilde kullanıldığında where koşuluna parametreler eklenemedi şimdilik aşağıdaki şekilde değiştirildi ama düzeltilecek

            #region Query
            query = @" 
                            declare @pr varchar(500)
                            set @pr = isnull(@modelozellik,'')
                            declare @mr int 
                            set @mr = @marka 
                            declare @uyr int 
                            set @uyr = @uretimyeri
                            select 
	                              s.stok_kodu,so.orjinal_stok_kodu
                                  ,ps1.tanim as marka,ps2.tanim as uretimyeri
                                  , s.*
	                              from stokkart s
	                              inner join stokkart_ozel so on so.stokkart_id = s.stokkart_id
	                              inner join giz_sabit_stokkartturu sttr on sttr.stokkartturu = s.stokkart_tur_id and sttr.stokkartturu = 0   /*model kartı için*/
	                              inner join giz_sabit_stokkarttipi sttp on sttp.stokkarttipi = s.stokkart_tipi_id and sttp.stokkarttipi in(1) /*model kartı için*/
                                  left join stokkart_rapor_parametre sp on sp.stokkart_id = s.stokkart_id
	                              left join parametre_stokkart_rapor ps1 on ps1.parametre_id = sp.stokalan_id_1 
	                              left join parametre_stokkart_rapor ps2 on ps2.parametre_id = sp.stokalan_id_2                                 
	                              where  
                                  isnull(s.kayit_silindi,0) = 0 and isnull(s.statu,0) = 1 and

	                              ps1.parametre = 1 and ps2.parametre = 2
	                              and ps1.parametre_grubu = 0 and ps2.parametre_grubu = 0
	                              and exists(
		                            select * from giz_setup_stokkart_parametre where parametre_grubu = ps1.parametre_grubu and parametre_grubu = 0 and parametre = 1 and ps1.parametre = parametre
		                            )
	                              and exists(
		                            select * from giz_setup_stokkart_parametre where parametre_grubu = ps2.parametre_grubu and parametre_grubu = 0 and parametre = 2 and ps2.parametre = parametre
		                            )

	                          and ( /*model adı,no,orjinal no*/
		                               s.stok_adi like '%'+@pr+'%'
		                            or s.stok_kodu like '%'+@pr+'%'
		                            or so.orjinal_stok_kodu like '%'+@pr+'%'
	                                  )

	                             and @mr  = case when @mr > 0 then  sp.stokalan_id_1 else @mr end
					             and @uyr = case when @uyr > 0 then sp.stokalan_id_2 else @uyr end
                               
                                    
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@modelozellik",modelozellik),
               new SqlParameter("@marka",marka),
               new SqlParameter("@uretimyeri",uretimyeri)
            };
            #endregion

            #endregion

            query = @"
                            select 
	                              s.stok_kodu,so.orjinal_stok_kodu
                                  ,ps1.tanim as marka,ps2.tanim as uretimyeri
                                  , s.*
	                              from stokkart s
	                              inner join stokkart_ozel so on so.stokkart_id = s.stokkart_id
	                              inner join giz_sabit_stokkartturu sttr on sttr.stokkartturu = s.stokkart_tur_id and sttr.stokkartturu = 0   /*model kartı için*/
	                              inner join giz_sabit_stokkarttipi sttp on sttp.stokkarttipi = s.stokkart_tipi_id and sttp.stokkarttipi in(1) /*model kartı için*/
                                  left join stokkart_rapor_parametre sp on sp.stokkart_id = s.stokkart_id
	                              left join parametre_stokkart_rapor ps1 on ps1.parametre_id = sp.stokalan_id_1 
	                              left join parametre_stokkart_rapor ps2 on ps2.parametre_id = sp.stokalan_id_2                                 
	                              where  
                                  isnull(s.kayit_silindi,0) = 0 and isnull(s.statu,0) = 1 and

	                              ps1.parametre = 1 and ps2.parametre = 2
	                              and ps1.parametre_grubu = 0 and ps2.parametre_grubu = 0
	                              and exists(
		                            select * from giz_setup_stokkart_parametre where parametre_grubu = ps1.parametre_grubu and parametre_grubu = 0 and parametre = 1 and ps1.parametre = parametre
		                            )
	                              and exists(
		                            select * from giz_setup_stokkart_parametre where parametre_grubu = ps2.parametre_grubu and parametre_grubu = 0 and parametre = 2 and ps2.parametre = parametre
		                            )";
            if (!string.IsNullOrEmpty(modelozellik))
            {
                query += @"
	                          and ( /*model adı,no,orjinal no*/
		                               s.stok_adi like '%" + modelozellik + @"%'
		                            or s.stok_kodu like '%" + modelozellik + @"%'
		                            or so.orjinal_stok_kodu like '%" + modelozellik + @"%'
	                                  )";
            }
            if (marka > 0)
            {
                query += " and sp.stokalan_id_1 = " + marka;
            }
            if (marka > 0)
            {
                query += " and sp.stokalan_id_2 = " + uretimyeri;
            }

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                stoklar = new List<stokkart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stokKart = new stokkart();
                    stokKart.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                    stokKart.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                    stokKart.stok_adi = dt.Rows[0]["stok_adi"].acekaToString();
                    stokKart.stok_kodu = dt.Rows[0]["stok_kodu"].acekaToString();
                    stokKart.stokkart_ozel = new stokkart_ozel();
                    stokKart.stokkart_ozel.orjinal_stok_kodu = dt.Rows[0]["orjinal_stok_kodu"].acekaToString();
                    stoklar.Add(stokKart);
                    stokKart = null;
                }
            }
            return stoklar;
        }

    }
}
