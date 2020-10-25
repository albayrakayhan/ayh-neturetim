using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;

namespace aceka.infrastructure.Repositories
{
    public class CarikartRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;
        private cari_kart carikart = null;
        private carikart_rapor_parametre carirapor_parametre = null;
        private carikart_firma_ozel ozelalan = null;
        private Muhasebe.muhasebe_tanim_hesapkodlari muhkod = null;
        //private parametre_carikart_rapor carikartapor = null;
        //private List<parametre_carikart_rapor> parametreList = null;
        private List<cari_kart> carikartlar = null;
        private List<carikart_rapor_parametre> cariraporlar = null;
        private giz_sirket gizsirket = null;
        private List<giz_sirket> gizsirketlist = null;
        private List<carikart_firma_ozel> ozelalanlar = null;
        private List<Muhasebe.muhasebe_tanim_hesapkodlari> muhkodlar = null;
        private carikart_muhasebe_personel muhasebe = null;
        private List<carikart_denetim_aksesuar> carikartdenetimaks = null;
        private List<carikart_denetim_aksesuar_kosullar> carikartKosullar = null;

        #endregion

        public cari_kart Getir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select
                            CK.carikart_id, CK.degistiren_carikart_id, CK.degistiren_tarih, CK.kayit_silindi, 
                            CK.kayit_yeri, CK.statu, CK.carikart_turu_id, 
                            CK.carikart_tipi_id,CK.cari_unvan, CK.ozel_kod, 
                            CK.fiyattipi, CK.giz_yazilim_kodu, 
                            --ana_carikart_id > 0 ise sorguya ana_cari_unvan adında bir alan ekleniyor
                            CK.ana_carikart_id, 
                            (CASE
                            WHEN ana_carikart_id > 0 THEN (Select cari_unvan from  carikart where carikart_id = CK.ana_carikart_id)
                            END) as 'ana_cari_unvan', 
                            CK.sube_carikart_id,
                            (CASE
                            WHEN sube_carikart_id > 0 THEN (Select cari_unvan from  carikart where carikart_id = CK.sube_carikart_id)
                            END) as 'sube_cari_unvan', 
                            CK.transfer_depo_id, CK.giz_kullanici_adi, CK.giz_kullanici_sifre                       
                            FROM carikart as CK                          
                            WHERE CK.carikart_id = @carikart_id AND kayit_silindi = 0

                            --Table[1] Cari Kart Finans
                            SELECT
	                            cf.carikart_id, 
	                            cf.finans_sorumlu_carikart_id,
	                            c.cari_unvan as 'finans_sorumlu_cari_unvan'
                            FROM carikart_finans cf
                            LEFT JOIN carikart c  ON c.carikart_id = cf.finans_sorumlu_carikart_id
                            where cf.carikart_id=@carikart_id

                            --Table[2] Cari Kart Firma Özel Satın Alma Sorumlusu
                            SELECT 	
	                            cFirmaOzelSatinAlmaSorumlu.carikart_id,
	                            cFirmaOzelSatinAlmaSorumlu.satin_alma_sorumlu_carikart_id,
	                            c.cari_unvan as 'satin_alma_sorumlu_cari_unvan'
                            FROM carikart_firma_ozel cFirmaOzelSatinAlmaSorumlu
                            LEFT JOIN carikart c  ON c.carikart_id = cFirmaOzelSatinAlmaSorumlu.satin_alma_sorumlu_carikart_id
                            WHERE cFirmaOzelSatinAlmaSorumlu.carikart_id= @carikart_id

                            --Table[3] Cari Kart Firma Özel Satış Sorumlusu
                            SELECT 	
	                            cFirmaOzelSatisSorumlu.carikart_id,
	                            cFirmaOzelSatisSorumlu.satis_sorumlu_carikart_id,
	                            c.cari_unvan as 'satis_sorumlu_cari_unvan'
                            FROM carikart_firma_ozel cFirmaOzelSatisSorumlu
                            LEFT JOIN carikart c  ON c.carikart_id = cFirmaOzelSatisSorumlu.satis_sorumlu_carikart_id
                            WHERE cFirmaOzelSatisSorumlu.carikart_id= @carikart_id

                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                carikart = new cari_kart();
                carikart.carikart_id            = ds.Tables[0].Rows[0]["carikart_id"].acekaToLong();
                carikart.degistiren_carikart_id = ds.Tables[0].Rows[0]["degistiren_carikart_id"].acekaToLong();
                carikart.degistiren_tarih       = ds.Tables[0].Rows[0]["degistiren_tarih"].acekaToDateTime();
                carikart.kayit_silindi          = ds.Tables[0].Rows[0]["kayit_silindi"].acekaToBool();
                carikart.kayit_yeri             = ds.Tables[0].Rows[0]["kayit_yeri"].acekaToLong();
                carikart.statu                  = ds.Tables[0].Rows[0]["statu"].acekaToBool();
                carikart.carikart_turu_id       = ds.Tables[0].Rows[0]["carikart_turu_id"].acekaToByte();
                carikart.carikart_tipi_id       = ds.Tables[0].Rows[0]["carikart_tipi_id"].acekaToByte();
                carikart.cari_unvan             = ds.Tables[0].Rows[0]["cari_unvan"].ToString();
                carikart.ozel_kod               = ds.Tables[0].Rows[0]["ozel_kod"].ToString();
                carikart.fiyattipi              = ds.Tables[0].Rows[0]["fiyattipi"].ToString();
                carikart.giz_yazilim_kodu       = ds.Tables[0].Rows[0]["giz_yazilim_kodu"].acekaToShort();
                carikart.ana_carikart_id        = ds.Tables[0].Rows[0]["ana_carikart_id"].acekaToLong();
                carikart.ana_cari_unvan         = ds.Tables[0].Rows[0]["ana_cari_unvan"].ToString();
                carikart.transfer_depo_id       = ds.Tables[0].Rows[0]["transfer_depo_id"].acekaToLong();
                carikart.giz_kullanici_adi      = ds.Tables[0].Rows[0]["giz_kullanici_adi"].ToString();
                carikart.giz_kullanici_sifre    = ds.Tables[0].Rows[0]["giz_kullanici_sifre"].ToString();
                carikart.sube_carikart_id       = ds.Tables[0].Rows[0]["sube_carikart_id"].acekaToLong();
                carikart.sube_cari_unvan        = ds.Tables[0].Rows[0]["sube_cari_unvan"].ToString();

                //Cari finans 
                carikart.carikart_finans = new carikart_finans();
                CarikartFinansBilgileriGetir(ds.Tables[1], carikart.carikart_finans);

                //Cari Kart Firma Özel
                carikart.carikart_firma_ozel = new carikart_firma_ozel();

                //Cari Kart Firma Özel - Satın Alma Sorumlusu
                CarikartFirmaOzel_SatinAlmaSorumlusu_Getir(ds.Tables[2], carikart.carikart_firma_ozel);

                //Cari Kart Firma Özel - Satis Sorumlusu
                CarikartFirmaOzel_SatisSorumlusu_Getir(ds.Tables[3], carikart.carikart_firma_ozel);
            }
            return carikart;
        }

        /// <summary>
        /// Bu Metod finans_sorumlu_carikart_id ve finans_sorumlu_unvan bilgilerini doner
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="finansbilgiler"></param>
        /// <returns></returns>
        private carikart_finans CarikartFinansBilgileriGetir(DataTable dt, carikart_finans finansbilgiler)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                finansbilgiler.finans_sorumlu_carikart_id = dt.Rows[0]["finans_sorumlu_carikart_id"].acekaToLong();
                finansbilgiler.finans_sorumlu_cari_unvan = dt.Rows[0]["finans_sorumlu_cari_unvan"].ToString();
            }
            return finansbilgiler;
        }

        private carikart_firma_ozel CarikartFirmaOzelGetir(DataTable dt)
        {
            carikart_firma_ozel carikart_firma_ozel = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                carikart_firma_ozel = new carikart_firma_ozel();
                carikart_firma_ozel.carikart_id = Convert.ToInt64("0" + dt.Rows[0]["carikart_id"].ToString());
                carikart_firma_ozel.degistiren_carikart_id = Convert.ToInt64(dt.Rows[0]["degistiren_carikart_id"].ToString());
                carikart_firma_ozel.degistiren_tarih = Convert.ToDateTime(dt.Rows[0]["degistiren_tarih"].ToString());
                carikart_firma_ozel.satin_alma_sorumlu_carikart_id = Convert.ToInt64(dt.Rows[0]["satin_alma_sorumlu_carikart_id"].ToString());
                carikart_firma_ozel.satis_sorumlu_carikart_id = Convert.ToInt64(dt.Rows[0]["satis_sorumlu_carikart_id"].ToString());
                carikart_firma_ozel.baslamatarihi = dt.Rows[0]["baslamatarihi"].acekaToDateTime().Date;
                carikart_firma_ozel.ozel = dt.Rows[0]["ozel"].ToString();
            }
            return carikart_firma_ozel;
        }

        private carikart_firma_ozel CarikartFirmaOzel_SatinAlmaSorumlusu_Getir(DataTable dt, carikart_firma_ozel carikart_firma_ozel)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                carikart_firma_ozel.satin_alma_sorumlu_carikart_id = dt.Rows[0]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                carikart_firma_ozel.satin_alma_sorumlu_cari_unvan = dt.Rows[0]["satin_alma_sorumlu_cari_unvan"].ToString();
            }
            return carikart_firma_ozel;
        }

        private carikart_firma_ozel CarikartFirmaOzel_SatisSorumlusu_Getir(DataTable dt, carikart_firma_ozel carikart_firma_ozel)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                carikart_firma_ozel.satis_sorumlu_carikart_id = dt.Rows[0]["satis_sorumlu_carikart_id"].acekaToLong();
                carikart_firma_ozel.satis_sorumlu_cari_unvan = dt.Rows[0]["satis_sorumlu_cari_unvan"].ToString();
            }
            return carikart_firma_ozel;
        }

        /// <summary>
        /// Cari bulma fonksiyonu
        /// </summary>
        /// <param name="cari"></param>
        /// <returns></returns>
        public List<cari_kart> Bul(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", byte carikart_tipi_id = 0)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (carikart_id > 0)
            {
                parameterControl++;
                orStatement += "CK.carikart_id = @carikart_id AND ";
            }
            if (!string.IsNullOrEmpty(cari_unvan.TrimEnd()))
            {
                parameterControl++;
                orStatement += "CK.cari_unvan like @unvan AND ";
            }

            if (!string.IsNullOrEmpty(ozel_kod.TrimEnd()))
            {
                parameterControl++;
                orStatement += "CK.ozel_kod like @ozelkod AND ";
            }
            if (carikart_tipi_id > 0)
            {
                parameterControl++;
                orStatement += "CK.carikart_tipi_id = @carikart_tipi_id AND ";
            }
            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            orStatement += "";

            string query = @"
                            SELECT
                                CK.carikart_id, CK.degistiren_carikart_id, CK.degistiren_tarih, CK.kayit_silindi, 
                                CK.kayit_yeri, CK.statu, CK.carikart_turu_id, 
                                CK.carikart_tipi_id,CK.cari_unvan, CK.ozel_kod, 
                                CK.fiyattipi, CK.giz_yazilim_kodu, CK.ana_carikart_id, 
                                CK.transfer_depo_id, CK.giz_kullanici_adi, CK.giz_kullanici_sifre, 
                                CK.cari_parametre_1, CK.cari_parametre_2, CK.cari_parametre_3, 
                                CK.cari_parametre_4, CK.cari_parametre_5, CK.cari_parametre_6, 
                                CK.cari_parametre_7, CK.sube_carikart_id,ct.carikart_tipi_adi,p.pb_kodu
                                --,p.pb_kodu
                            FROM carikart as CK
                            left join  carikart_finans f on f.carikart_id=ck.carikart_id
                            --left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = cK.carikart_turu_id
                            left join giz_sabit_carikart_tipi ct on cK.carikart_tipi_id=ct.carikart_tipi_id
                            left join parametre_parabirimi p On p.pb_kodu=f.pb                     
                            WHERE " + orStatement + " ";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@carikart_id",carikart_id),
                    new SqlParameter("@unvan","%"+cari_unvan+"%"),
                    new SqlParameter("@ozelkod","%"+ozel_kod+"%")

            };
            #endregion

            if (parameterControl > 0)
            {

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    carikartlar = new List<cari_kart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        carikart = new cari_kart();
                        carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        carikart.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        carikart.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        carikart.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                        carikart.kayit_yeri = dt.Rows[i]["kayit_yeri"].acekaToLong();
                        carikart.statu = dt.Rows[i]["statu"].acekaToBool();
                        carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        carikart.ozel_kod = dt.Rows[i]["ozel_kod"].ToString();
                        carikart.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                        carikart.giz_yazilim_kodu = dt.Rows[i]["giz_yazilim_kodu"].acekaToShort();
                        carikart.ana_carikart_id = dt.Rows[i]["ana_carikart_id"].acekaToLong();
                        carikart.transfer_depo_id = dt.Rows[i]["transfer_depo_id"].acekaToLong();
                        carikart.giz_kullanici_adi = dt.Rows[i]["giz_kullanici_adi"].ToString();
                        carikart.giz_kullanici_sifre = dt.Rows[i]["giz_kullanici_sifre"].ToString();
                        carikart.cari_parametre_1 = dt.Rows[i]["cari_parametre_1"].acekaToInt();
                        carikart.cari_parametre_2 = dt.Rows[i]["cari_parametre_2"].acekaToInt();
                        carikart.cari_parametre_3 = dt.Rows[i]["cari_parametre_3"].acekaToInt();
                        carikart.cari_parametre_4 = dt.Rows[i]["cari_parametre_4"].acekaToInt();
                        carikart.cari_parametre_5 = dt.Rows[i]["cari_parametre_5"].acekaToInt();
                        carikart.cari_parametre_6 = dt.Rows[i]["cari_parametre_6"].acekaToInt();
                        carikart.cari_parametre_7 = dt.Rows[i]["cari_parametre_7"].acekaToInt();
                        carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                        carikart.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();

                        carikart.parametre_parabirimi = new parametre_parabirimi();
                        carikart.parametre_parabirimi.pb_kodu = dt.Rows[i]["pb_kodu"].ToString();

                        carikartlar.Add(carikart);
                        carikart = null;
                    }
                }
            }
            return carikartlar;
        }

        /// <summary>
        /// Cari bulma fonksiyonu
        /// </summary>
        /// <param name="cari"></param>
        /// <returns></returns>
        public List<cari_kart> BulOzet(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", byte carikart_tipi_id = 0)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (carikart_id > 0)
            {
                parameterControl++;
                orStatement += "c.carikart_id = @carikart_id AND ";
            }
            if (!string.IsNullOrEmpty(cari_unvan.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.cari_unvan like @unvan AND ";
            }
            if (carikart_tipi_id > 0)
            {
                parameterControl++;
                orStatement += "c.carikart_tipi_id = @carikart_tipi_id AND ";
            }
            if (!string.IsNullOrEmpty(ozel_kod.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.ozel_kod like @ozelkod AND ";
            }

            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            orStatement += "";

            //25.01.2003 Tarihinde değiştirdim. Kerem arama ekranından detaya geçerken bu bilgileri kullanıyor.
            //string query = @"SELECT
            //                c.carikart_id,cari_unvan,ozel_kod,giz_yazilim_kodu,c.statu,
            //                ct.carikart_tipi_id,ct.carikart_tipi_adi,ctur.carikart_turu_id ,ctur.carikart_turu_adi,c.fiyattipi,
            //                ca.adres,ca.tel1,ca.email,ca.websitesi,cf.pb
            //                FROM 
            //                carikart c 
            //                left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
            //                left join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id
            //                left join carikart_genel_adres ca on c.carikart_id=ca.carikart_id and ca.adres_tipi_id='IF'
            //                left join carikart_finans cf on cf.carikart_id=c.carikart_id                        
            //                Where " + orStatement + " AND c.statu=1";



            string query = @"
                            SELECT  c.carikart_id,cari_unvan,ozel_kod,giz_yazilim_kodu,c.statu,
		                            ct.carikart_tipi_id,ct.carikart_tipi_adi,ctur.carikart_turu_id ,ctur.carikart_turu_adi,c.fiyattipi,
		                            ca.adres,ca.tel1,ca.email,ca.websitesi,f.pb,ana_carikart_id,cf.odeme_tipi,
                                o.satin_alma_sorumlu_carikart_id,o.satis_sorumlu_carikart_id, cf.finans_sorumlu_carikart_id, 
	                            (CASE  WHEN f.finans_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  f.finans_sorumlu_carikart_id) END) as               'finans_sorumlu_cari_unvan',
	                            (CASE  WHEN f.ilgili_sube_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  f.ilgili_sube_carikart_id) END) as         'ilgili_sube_cari_unvan',
	                            (CASE  WHEN o.satin_alma_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  o.satin_alma_sorumlu_carikart_id) END) as               'satin_alma_sorumlu_cari_unvan',
	                            (CASE  WHEN o.satis_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  o.satis_sorumlu_carikart_id) END) as                 'satis_sorumlu_cari_unvan',
                                (CASE  WHEN  c.ana_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =   c.ana_carikart_id) END) as 'ana_cari_unvan'
	                            --(CASE  WHEN  c.ana_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE ana_carikart_id =   c.carikart_id) END) as 'ilgili_sube'
		                           
                                 FROM 
                                 carikart c 
		                         left join  carikart_finans f on f.carikart_id=c.carikart_id
		                         left join carikart_firma_ozel o on o.carikart_id=c.carikart_id
                                 left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
                                 left join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id
                                 left join carikart_genel_adres ca on c.carikart_id=ca.carikart_id --and ca.adres_tipi_id='IF'
                                 left join carikart_finans cf on cf.carikart_id=c.carikart_id 
                                WHERE " + orStatement + " ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@carikart_id",carikart_id),
                    new SqlParameter("@unvan","%"+cari_unvan+"%"),
                    new SqlParameter("@ozelkod","%"+ozel_kod+"%"),
                    new SqlParameter("@carikart_tipi_id",carikart_tipi_id)
            };
            #endregion

            if (parameterControl > 0)
            {

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    carikartlar = new List<cari_kart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        carikart = new cari_kart();
                        carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        carikart.statu = dt.Rows[i]["statu"].acekaToBool();
                        carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        carikart.ozel_kod = dt.Rows[i]["ozel_kod"].ToString();
                        carikart.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                        carikart.giz_yazilim_kodu = dt.Rows[i]["giz_yazilim_kodu"].acekaToShort();
                        carikart.ana_cari_unvan = dt.Rows[i]["ana_cari_unvan"].ToString();
                        carikart.ana_carikart_id = dt.Rows[i]["ana_carikart_id"].acekaToLong();


                        carikart.giz_sabit_carikart_turu = new giz_sabit_carikart_turu();

                        carikart.giz_sabit_carikart_turu.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        carikart.giz_sabit_carikart_turu.carikart_turu_adi = dt.Rows[i]["carikart_turu_adi"].ToString();

                        carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                        carikart.giz_sabit_carikart_tipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        carikart.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();

                        carikart.carikart_genel_adres = new List<carikart_genel_adres>();
                        carikart.carikart_genel_adres.Add(new carikart_genel_adres
                        {
                            adres = dt.Rows[i]["adres"].ToString(),
                            tel1 = dt.Rows[i]["tel1"].ToString(),
                            email = dt.Rows[i]["email"].ToString(),
                            websitesi = dt.Rows[i]["websitesi"].ToString(),
                        });

                        //Cari finans 
                        carikart.carikart_finans = new carikart_finans();
                        carikart.carikart_finans.finans_sorumlu_carikart_id = dt.Rows[i]["finans_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_finans.finans_sorumlu_cari_unvan = dt.Rows[i]["finans_sorumlu_cari_unvan"].ToString();
                        carikart.carikart_finans.odeme_tipi = dt.Rows[i]["odeme_tipi"].acekaToByte();


                        //Cari Kart Firma Özel
                        carikart.carikart_firma_ozel = new carikart_firma_ozel();
                        carikart.carikart_firma_ozel.ozel = dt.Rows[i]["ozel_kod"].ToString();
                        carikart.carikart_firma_ozel.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satin_alma_sorumlu_carikart_id = dt.Rows[i]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satin_alma_sorumlu_cari_unvan = dt.Rows[i]["satin_alma_sorumlu_cari_unvan"].ToString();
                        carikart.carikart_firma_ozel.satis_sorumlu_carikart_id = dt.Rows[i]["satis_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satis_sorumlu_cari_unvan = dt.Rows[i]["satis_sorumlu_cari_unvan"].ToString();


                        // carikart.carikart_finans = new carikart_finans();
                        //carikart.carikart_finans.pb = dt.Rows[i]["pb"].ToString();
                        //carikart.carikart_finans.finans_sorumlu_carikart_id = dt.Rows[i]["finans_sorumlu_carikart_id"].acekaToLong();

                        //carikart.carikart_firma_ozel = new carikart_firma_ozel();
                        //carikart.carikart_firma_ozel.satin_alma_sorumlu_carikart_id = dt.Rows[i]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                        //carikart.carikart_firma_ozel.satis_sorumlu_carikart_id = dt.Rows[i]["satis_sorumlu_carikart_id"].acekaToLong();
                        carikartlar.Add(carikart);
                        carikart = null;
                    }
                }
            }
            return carikartlar;
        }

        public List<cari_kart> BulOzetTest(cari_kart cari)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (cari.carikart_id > 0)
            {
                parameterControl++;
                orStatement += "c.carikart_id = @carikart_id OR ";
            }
            if (!string.IsNullOrEmpty(cari.cari_unvan.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.cari_unvan like @unvan OR ";
            }

            if (!string.IsNullOrEmpty(cari.ozel_kod.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.ozel_kod like @ozelkod OR ";
            }

            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'O', 'R', ' ' }) + ")";
            }

            orStatement += "";

            //25.01.2003 Tarihinde değiştirdim. Kerem arama ekranından detaya geçerken bu bilgileri kullanıyor.
            //string query = @"SELECT
            //                c.carikart_id,cari_unvan,ozel_kod,giz_yazilim_kodu,c.statu,
            //                ct.carikart_tipi_id,ct.carikart_tipi_adi,ctur.carikart_turu_id ,ctur.carikart_turu_adi,c.fiyattipi,
            //                ca.adres,ca.tel1,ca.email,ca.websitesi,cf.pb
            //                FROM 
            //                carikart c 
            //                left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
            //                left join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id
            //                left join carikart_genel_adres ca on c.carikart_id=ca.carikart_id and ca.adres_tipi_id='IF'
            //                left join carikart_finans cf on cf.carikart_id=c.carikart_id                        
            //                Where " + orStatement + " AND c.statu=1";
            string query = @"
                            SELECT  c.carikart_id,cari_unvan,ozel_kod,giz_yazilim_kodu,c.statu,
		                            ct.carikart_tipi_id,ct.carikart_tipi_adi,ctur.carikart_turu_id ,ctur.carikart_turu_adi,c.fiyattipi,
                                    ca.adres,ca.tel1,ca.email,ca.websitesi,f.pb,ana_carikart_id,cf.odeme_tipi,c.degistiren_carikart_id,c.degistiren_tarih,c.kayit_silindi,c.kayit_yeri,
                                c.transfer_depo_id,c.giz_kullanici_adi,c.giz_kullanici_sifre,
                                o.satin_alma_sorumlu_carikart_id,o.satis_sorumlu_carikart_id, cf.finans_sorumlu_carikart_id, 
	                            (CASE  WHEN f.finans_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  f.finans_sorumlu_carikart_id) END) as               'finans_sorumlu_cari_unvan',
	                            (CASE  WHEN f.ilgili_sube_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  f.ilgili_sube_carikart_id) END) as         'ilgili_sube_cari_unvan',
	                            (CASE  WHEN o.satin_alma_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  o.satin_alma_sorumlu_carikart_id) END) as               'satin_alma_sorumlu_cari_unvan',
	                            (CASE  WHEN o.satis_sorumlu_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  o.satis_sorumlu_carikart_id) END) as                 'satis_sorumlu_cari_unvan',
                                (CASE  WHEN  c.ana_carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =   c.ana_carikart_id) END) as 'ana_cari_unvan'
	                            --(CASE  WHEN  c.carikart_id > 0 THEN (SELECT cari_unvan FROM carikart WHERE ana_carikart_id =   c.carikart_id) END) as 'ilgili_sube'
		                           
                                 FROM 
                                 carikart c 
		                         left join  carikart_finans f on f.carikart_id=c.carikart_id
		                         left join carikart_firma_ozel o on o.carikart_id=c.carikart_id
                                 left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
                                 left join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id
                                 left join carikart_genel_adres ca on c.carikart_id=ca.carikart_id and ca.adres_tipi_id='IF'
                                 left join carikart_finans cf on cf.carikart_id=c.carikart_id 
                                Where " + orStatement + "  ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@carikart_id",cari.carikart_id),
                    new SqlParameter("@unvan","%"+cari.cari_unvan+"%"),
                    new SqlParameter("@ozelkod","%"+cari.ozel_kod+"%")
            };
            #endregion

            if (parameterControl > 0)
            {

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    carikartlar = new List<cari_kart>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        carikart = new cari_kart();
                        carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        carikart.statu = dt.Rows[i]["statu"].acekaToBool();
                        carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        carikart.ozel_kod = dt.Rows[i]["ozel_kod"].ToString();
                        carikart.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                        carikart.giz_yazilim_kodu = dt.Rows[i]["giz_yazilim_kodu"].acekaToShort();
                        carikart.ana_cari_unvan = dt.Rows[i]["ana_cari_unvan"].ToString();
                        carikart.ana_carikart_id = dt.Rows[i]["ana_carikart_id"].acekaToLong();
                        //cari.carikart_finans.odeme_tipi = dt.Rows[i]["odeme_tipi"].acekaToByte() ;

                        carikart.giz_sabit_carikart_turu = new giz_sabit_carikart_turu();

                        carikart.giz_sabit_carikart_turu.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        carikart.giz_sabit_carikart_turu.carikart_turu_adi = dt.Rows[i]["carikart_turu_adi"].ToString();

                        carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                        carikart.giz_sabit_carikart_tipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        carikart.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();
                        carikart.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        carikart.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        carikart.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                        carikart.kayit_yeri = dt.Rows[i]["kayit_yeri"].acekaToLong();
                        carikart.transfer_depo_id = dt.Rows[i]["transfer_depo_id"].acekaToLong();
                        carikart.giz_kullanici_adi = dt.Rows[i]["giz_kullanici_adi"].ToString();
                        carikart.giz_kullanici_sifre = dt.Rows[i]["giz_kullanici_sifre"].ToString();

                        carikart.carikart_genel_adres = new List<carikart_genel_adres>();
                        carikart.carikart_genel_adres.Add(new carikart_genel_adres
                        {
                            adres = dt.Rows[i]["adres"].ToString(),
                            tel1 = dt.Rows[i]["tel1"].ToString(),
                            email = dt.Rows[i]["email"].ToString(),
                            websitesi = dt.Rows[i]["websitesi"].ToString(),
                        });

                        //Cari finans 
                        carikart.carikart_finans = new carikart_finans();
                        carikart.carikart_finans.finans_sorumlu_carikart_id = dt.Rows[i]["finans_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_finans.finans_sorumlu_cari_unvan = dt.Rows[i]["finans_sorumlu_cari_unvan"].ToString();
                        carikart.carikart_finans.odeme_tipi = dt.Rows[i]["odeme_tipi"].acekaToByte();
                        carikart.carikart_finans.pb = dt.Rows[i]["pb"].ToString();

                        //Cari Kart Firma Özel
                        carikart.carikart_firma_ozel = new carikart_firma_ozel();
                        carikart.carikart_firma_ozel.ozel = dt.Rows[i]["ozel_kod"].ToString();
                        carikart.carikart_firma_ozel.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satin_alma_sorumlu_carikart_id = dt.Rows[i]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satin_alma_sorumlu_cari_unvan = dt.Rows[i]["satin_alma_sorumlu_cari_unvan"].ToString();
                        carikart.carikart_firma_ozel.satis_sorumlu_carikart_id = dt.Rows[i]["satis_sorumlu_carikart_id"].acekaToLong();
                        carikart.carikart_firma_ozel.satis_sorumlu_cari_unvan = dt.Rows[i]["satis_sorumlu_cari_unvan"].ToString();

                        carikartlar.Add(carikart);
                        carikart = null;
                    }
                }
            }
            return carikartlar;
        }

        /// <summary>
        /// Autocomplate için cari türü = bayi ve cari olan liste getiriliyor
        /// </summary>
        /// <returns>List<carikart></returns>
        public List<cari_kart> CariListeTurBayi_veCari()
        {

            #region Query
            string query = @"SELECT 
                            carikart_id,cari_unvan,carikart_turu_id,carikart_tipi_id
                            FROM carikart
                            WHERE  kayit_silindi=0 AND carikart_turu_id IN (1,4) 
                            ORDER BY cari_unvan";

            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartlar = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart = new cari_kart();
                    carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    carikartlar.Add(carikart);
                    carikart = null;
                }
            }

            return carikartlar;
        }

        /// <summary>
        /// Müşterilerin listesi getiriliyor
        /// </summary>
        /// <returns>List<carikart></returns>
        public List<cari_kart> CariListeMusteri()
        {

            #region Query
            string query = @"SELECT 
                            carikart_id,cari_unvan
                            FROM carikart
                            WHERE  kayit_silindi=0 AND carikart_tipi_id IN (11,13,15)
                            ORDER BY cari_unvan";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartlar = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart = new cari_kart();
                    carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    carikartlar.Add(carikart);
                    carikart = null;
                }
            }

            return carikartlar;
        }

        /// <summary>
        /// Firma/Bayi listesi getiriliyor
        /// </summary>
        /// <returns>List<carikart></returns>
        public List<cari_kart> FirmaBayiListesi()
        {
            #region Query
            string query = @"SELECT 
                            carikart_id,cari_unvan
                            FROM carikart
                            WHERE  kayit_silindi=0 AND carikart_tipi_id = 33
                            ORDER BY cari_unvan";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartlar = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart = new cari_kart();
                    carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    carikartlar.Add(carikart);
                    carikart = null;
                }
            }

            return carikartlar;
        }

        public List<Muhasebe.muhasebe_tanim_hesapkodlari> MuhasebeKodlariGetir()
        {
            #region Query
            string query = @"SELECT 
                            m.muh_kod_id,m.muh_kod,m.muh_kod_adi 
                            FROM muhasebe_tanim_hesapkodlari m
                            ORDER BY muh_kod";

            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                muhkodlar = new List<Muhasebe.muhasebe_tanim_hesapkodlari>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    muhkod = new Muhasebe.muhasebe_tanim_hesapkodlari();
                    muhkod.muh_kod_id = dt.Rows[i]["muh_kod_id"].acekaToInt();
                    muhkod.muh_kod = dt.Rows[i]["muh_kod"].ToString();
                    muhkod.muh_kod_adi = dt.Rows[i]["muh_kod_adi"].ToString();
                    muhkodlar.Add(muhkod);
                    muhkod = null;
                }

            }
            return muhkodlar;
        }


        #region Carikart ekranındaki TAB içerikleri
        /// <summary>
        /// Cari adreslerini getirir
        /// adresler "adres_tipi_id" alanına göre gruplanıyor. FA,IA,IF,II
        /// Cari kart için Fatura Adresi -> FA ve İrsali adresi -> IA alanları ile filtreleme yapıldı.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public List<carikart_genel_adres> CarikartAdresleriniGetir(long carikart_id, out string iletisim_adres_tipi_id)
        {
            List<carikart_genel_adres> adresler = null;
            iletisim_adres_tipi_id = "";

            #region Query
            string query = @"
                        --Table[0]
						SELECT 
                        CKA.carikart_adres_id, CKA.degistiren_carikart_id, CKA.degistiren_tarih, 
                        CKA.kayit_silindi, CKA.statu, CKA.adres_tipi_id, CKA.carikart_id, 
                        CKA.adrestanim, CKA.adresunvan, CKA.adres, CKA.postakodu, 
                        CKA.ulke_id, 
                        U.ulke_adi,
                        CKA.sehir_id, 
                        US.sehir_adi,
                        CKA.ilce_id, 
                        USI.ilce_adi,
                        CKA.semt_id,
                        USIS.semt_adi,
                        CKA.vergidairesi, 
                        CKA.vergino, 
                        CKA.tel1, 
                        CKA.tel2, 
                        CKA.fax, 
                        CKA.email, 
                        CKA.websitesi, 
                        CKA.yetkili_ad_soyad, 
                        CKA.yetkili_tel, 
                        CKA.faturaadresi
                        FROM carikart_genel_adres AS CKA
                        LEFT JOIN parametre_ulke U ON U.ulke_id=CKA.ulke_id
                        LEFT JOIN parametre_ulke_sehir US ON US.sehir_id=CKA.sehir_id
                        LEFT JOIN parametre_ulke_sehir_ilce USI ON USI.ilce_id=CKA.ilce_id
                        LEFT JOIN parametre_ulke_sehir_ilce_semt USIS ON USIS.semt_id=CKA.semt_id
                        WHERE CKA.carikart_id = @carikart_id  AND CKA.kayit_silindi=0 --AND  CKA.adres_tipi_id in('FA','IA')

                        --Table[1]
						SELECT iletisim_adres_tipi_id FROM carikart WHERE carikart_id = @carikart_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                adresler = new List<carikart_genel_adres>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    carikart_genel_adres adres = new carikart_genel_adres();
                    adres.carikart_adres_id = ds.Tables[0].Rows[i]["carikart_adres_id"].acekaToLong();
                    adres.degistiren_carikart_id = ds.Tables[0].Rows[i]["degistiren_carikart_id"].acekaToLong();
                    adres.degistiren_tarih = ds.Tables[0].Rows[i]["degistiren_tarih"].acekaToDateTime();
                    adres.statu = ds.Tables[0].Rows[i]["statu"].acekaToBool();
                    adres.adres_tipi_id = ds.Tables[0].Rows[i]["adres_tipi_id"].ToString();
                    adres.carikart_id = ds.Tables[0].Rows[i]["carikart_id"].acekaToLong();
                    adres.adrestanim = ds.Tables[0].Rows[i]["adrestanim"].ToString();
                    adres.adresunvan = ds.Tables[0].Rows[i]["adresunvan"].ToString();
                    adres.adres = ds.Tables[0].Rows[i]["adres"].ToString();
                    adres.postakodu = ds.Tables[0].Rows[i]["postakodu"].ToString();
                    //Ülke Model
                    adres.ulke_id = ds.Tables[0].Rows[i]["ulke_id"].acekaToShort();
                    //Şehir Model
                    adres.sehir_id = ds.Tables[0].Rows[i]["sehir_id"].acekaToShort();
                    //İlçe Model
                    adres.ilce_id = ds.Tables[0].Rows[i]["ilce_id"].acekaToShort();
                    //Semt Model
                    adres.semt_id = ds.Tables[0].Rows[i]["semt_id"].acekaToShort();
                    adres.vergidairesi = ds.Tables[0].Rows[i]["vergidairesi"].ToString();
                    adres.vergino = ds.Tables[0].Rows[i]["vergino"].ToString();
                    adres.tel1 = ds.Tables[0].Rows[i]["tel1"].ToString();
                    adres.tel2 = ds.Tables[0].Rows[i]["tel2"].ToString();
                    adres.fax = ds.Tables[0].Rows[i]["fax"].ToString();
                    adres.email = ds.Tables[0].Rows[i]["email"].ToString();
                    adres.websitesi = ds.Tables[0].Rows[i]["websitesi"].ToString();
                    adres.yetkili_ad_soyad = ds.Tables[0].Rows[i]["yetkili_ad_soyad"].ToString();
                    adres.yetkili_tel = ds.Tables[0].Rows[i]["yetkili_tel"].ToString();
                    adres.faturaadresi = ds.Tables[0].Rows[i]["faturaadresi"].acekaToBool();
                    adresler.Add(adres);
                    adres = null;
                }
                iletisim_adres_tipi_id = ds.Tables[1].Rows[0]["iletisim_adres_tipi_id"].ToString();

            }
            return adresler;
        }

        /// <summary>
        /// Depo Kartı carikart_rapor_parametre tablosundaki cari karta ait depo parametrelerini getirir parametreleri getirir
        /// </summary>
        /// <returns></returns>
        public carikart_rapor_parametre CarikartParametreleriniGetir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            SELECT
                            CK.carikart_id,
                            CK.cari_parametre_1,
                            CK.cari_parametre_2, 
                            CK.cari_parametre_3, 
                            CK.cari_parametre_4, 
                            CK.cari_parametre_5, 
                            CK.cari_parametre_6, 
                            CK.cari_parametre_7
                            FROM carikart_rapor_parametre as CK                          
                            WHERE CK.carikart_id = @carikart_id 
                            
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                //cariraporlar = new List<carikart_rapor_parametre>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carirapor_parametre = new carikart_rapor_parametre();
                    carirapor_parametre.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carirapor_parametre.cari_parametre_1 = dt.Rows[i]["cari_parametre_1"].acekaToInt();
                    carirapor_parametre.cari_parametre_2 = dt.Rows[i]["cari_parametre_2"].acekaToInt();
                    carirapor_parametre.cari_parametre_3 = dt.Rows[i]["cari_parametre_3"].acekaToInt();
                    carirapor_parametre.cari_parametre_4 = dt.Rows[i]["cari_parametre_4"].acekaToInt();
                    carirapor_parametre.cari_parametre_5 = dt.Rows[i]["cari_parametre_5"].acekaToInt();
                    carirapor_parametre.cari_parametre_6 = dt.Rows[i]["cari_parametre_6"].acekaToInt();
                    carirapor_parametre.cari_parametre_7 = dt.Rows[i]["cari_parametre_7"].acekaToInt();
                }
            }
            return carirapor_parametre;
        }


        /// <summary>
        /// Özel Alanlar Sekmesini getiren Metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_firma_ozel CarikartOzelalanlarGetir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select 
                            CK.carikart_id,
							satin_alma_sorumlu_carikart_id,
							(select cari_unvan from carikart cr where cr.carikart_id= CK.satin_alma_sorumlu_carikart_id) as satin_alma_sorumlu_unvan,

							satis_sorumlu_carikart_id,
							(select cari_unvan from carikart cr where cr.carikart_id= CK.satis_sorumlu_carikart_id) as satis_sorumlu_unvan,
                            CK.baslamatarihi,CK.ozel
                            FROM carikart_firma_ozel CK
							INNER JOIN carikart C ON c.carikart_id=CK.carikart_id
                            WHERE CK.carikart_id =  @carikart_id
                ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            carikart_firma_ozel ozelalan = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                //ozelalanlar = new List<carikart_firma_ozel>();
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    ozelalan = new carikart_firma_ozel();
                //    ozelalan = new carikart_firma_ozel();
                //    ozelalan.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                //    ozelalan.baslamatarihi = dt.Rows[0]["baslamatarihi"].acekaToDateTime();
                //    ozelalan.satis_sorumlu_carikart_id = dt.Rows[0]["satis_sorumlu_carikart_id"].acekaToLong();
                //    ozelalan.satis_sorumlu_cari_unvan = dt.Rows[0]["satis_sorumlu_unvan"].ToString();
                //    ozelalan.satin_alma_sorumlu_carikart_id = dt.Rows[0]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                //    ozelalan.satin_alma_sorumlu_cari_unvan = dt.Rows[0]["satin_alma_sorumlu_unvan"].ToString();
                //    ozelalan.ozel = dt.Rows[0]["ozel"].ToString();
                //    ozelalanlar.Add(ozelalan);
                //    ozelalan = null;
                //}

                ozelalan = new carikart_firma_ozel();
                ozelalan.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ozelalan.baslamatarihi = dt.Rows[0]["baslamatarihi"].acekaToDateTime();
                ozelalan.satis_sorumlu_carikart_id = dt.Rows[0]["satis_sorumlu_carikart_id"].acekaToLong();
                ozelalan.satis_sorumlu_cari_unvan = dt.Rows[0]["satis_sorumlu_unvan"].ToString();
                ozelalan.satin_alma_sorumlu_carikart_id = dt.Rows[0]["satin_alma_sorumlu_carikart_id"].acekaToLong();
                ozelalan.satin_alma_sorumlu_cari_unvan = dt.Rows[0]["satin_alma_sorumlu_unvan"].ToString();
                ozelalan.ozel = dt.Rows[0]["ozel"].ToString();

            }
            return ozelalan;
        }

        /// <summary>
        /// E-posta sekmesini getiren Metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_elektronik_bilgilendirme EpostaGrupGetir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            select	B.carikart_id,
		                    B.irsaliye_eposta,
		                    B.perakende_fatura_eposta,
		                    B.toptan_fatura_eposta,
		                    B.siparis_formu_eposta,
		                    B.babs_formu_eposta,
		                    B.cari_mutabakat_formu_eposta,
		                    B.odeme_hatirlatma_eposta ,
		                    B.degistiren_carikart_id,
		                    B.degistiren_tarih
		                    from carikart_elektronik_bilgilendirme B
                            WHERE B.carikart_id = @carikart_id
                ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            carikart_elektronik_bilgilendirme epostagrup = new carikart_elektronik_bilgilendirme();
            if (dt != null && dt.Rows.Count > 0)
            {
                epostagrup.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                epostagrup.irsaliye_eposta = dt.Rows[0]["irsaliye_eposta"].ToString();
                epostagrup.odeme_hatirlatma_eposta = dt.Rows[0]["odeme_hatirlatma_eposta"].ToString();
                epostagrup.perakende_fatura_eposta = dt.Rows[0]["perakende_fatura_eposta"].ToString();
                epostagrup.siparis_formu_eposta = dt.Rows[0]["siparis_formu_eposta"].ToString();
                epostagrup.toptan_fatura_eposta = dt.Rows[0]["toptan_fatura_eposta"].ToString();
                epostagrup.babs_formu_eposta = dt.Rows[0]["babs_formu_eposta"].ToString();
                epostagrup.cari_mutabakat_formu_eposta = dt.Rows[0]["cari_mutabakat_formu_eposta"].ToString();
            }
            return epostagrup;
        }

        /// <summary>
        /// Carikart Finans Bilgilerini Getirir
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_finans CarikartFinansBilgileriniGetir(long carikart_id)
        {
            carikart_finans carikartFinans = null;

            #region Query
            string query = @"
                            --Table[0]
                                SELECT 
	                                CF.carikart_id, 
	                                CF.tckimlikno, 
	                                CF.vergi_no, 
	                                CF.vergi_dairesi, 
	                                CF.yabanci_uyruklu, 
	                                CF.diger_kod, 
	                                CF.pb, 
	                                CF.vade_alis, 
	                                CF.vade_satis, 
	                                CF.odeme_plani_id, 
                                -- odeme_plani_adi
	                            --eski hali    FP.odeme_plani_adi  as 'odeme_plani_adi',
                                    FP.odeme_plani_id,
                                -- odeme_plani_kodu
	                                FP.odeme_plani_kodu  as 'odeme_plani_kodu',			 	
	                                CF.iskonto_alis, 
	                                CF.iskonto_satis, 
	                                CF.odeme_tipi, 
	                                CF.kur_farki, 
	                                CF.odeme_listesinde_cikmasin, 
	                                CF.alacak_listesinde_cikmasin, 
	                                CF.ticari_islem_grubu, 
	                                CF.ilgili_sube_carikart_id, 
                                    --ilgili_sube_cari_unvan
	                                (CASE  
		                                WHEN CF.ilgili_sube_carikart_id > 0 
		                                THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  CF.ilgili_sube_carikart_id) END) as 'ilgili_sube_cari_unvan',
	
	                                CF.finans_sorumlu_carikart_id, 
	                                (CASE  
		                                WHEN CF.finans_sorumlu_carikart_id > 0 
		                                THEN (SELECT cari_unvan FROM carikart WHERE carikart_id =  CF.finans_sorumlu_carikart_id) END) as 'finans_sorumlu_cari_unvan',
	                                CF.swift_kodu, 
	                                CF.tedarik_gunu, 
	                                CF.cari_hesapta_ciksin,
	                                CM.sirket_id,
	                                CM.muh_kod,
	                                CM.sene,
	                                MTMM.masraf_merkezi_id,
	                                MTMM.masraf_merkezi_adi,
                                -- fiyattipi_kodu
                                    FT.fiyattipi
                                FROM Carikart_finans CF
                                LEFT JOIN finans_tanim_odemeplani FP on FP.odeme_plani_id = CF.odeme_plani_id   
                                LEFT JOIN carikart_muhasebe CM ON CM.carikart_id = CF.carikart_id
                                LEFT JOIN muhasebe_tanim_masrafmerkezleri MTMM ON MTMM.masraf_merkezi_id = CM.masraf_merkezi_id
                                LEFT JOIN carikart FT ON FT.carikart_id = cf.carikart_id
                                --LEFT JOIN carikart_fiyat_tipi FT ON FT.carikart_id = cf.carikart_id
                                Where CF.carikart_id=@carikart_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartFinans = new carikart_finans();
                carikartFinans.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                carikartFinans.tckimlikno = dt.Rows[0]["tckimlikno"].ToString();
                carikartFinans.vergi_no = dt.Rows[0]["vergi_no"].ToString();
                carikartFinans.vergi_dairesi = dt.Rows[0]["vergi_dairesi"].ToString();
                carikartFinans.yabanci_uyruklu = dt.Rows[0]["yabanci_uyruklu"].acekaToBool();
                carikartFinans.diger_kod = dt.Rows[0]["diger_kod"].ToString();
                carikartFinans.pb = dt.Rows[0]["pb"].ToString();
                carikartFinans.vade_alis = dt.Rows[0]["vade_alis"].acekaToDecimal();
                carikartFinans.vade_satis = dt.Rows[0]["vade_satis"].acekaToDecimal();


                carikartFinans.odeme_plani = new finans_tanim_odemeplani();
                //carikartFinans.odeme_plani.odeme_plani_id = dt.Rows[0]["odeme_plani_id"].acekaToInt();
                //carikartFinans.odeme_plani.odeme_plani_adi = dt.Rows[0]["odeme_plani_adi"].ToString();

                carikartFinans.odeme_plani.odeme_plani_id = dt.Rows[0]["odeme_plani_id"].acekaToInt();


                carikartFinans.iskonto_alis = dt.Rows[0]["iskonto_alis"].acekaToDecimal();
                carikartFinans.iskonto_satis = dt.Rows[0]["iskonto_satis"].acekaToDecimal();
                carikartFinans.odeme_tipi = dt.Rows[0]["odeme_tipi"].acekaToByte();
                carikartFinans.kur_farki = dt.Rows[0]["kur_farki"].acekaToByte();
                carikartFinans.odeme_listesinde_cikmasin = dt.Rows[0]["odeme_listesinde_cikmasin"].acekaToByte();
                carikartFinans.alacak_listesinde_cikmasin = dt.Rows[0]["alacak_listesinde_cikmasin"].acekaToByte();
                carikartFinans.ticari_islem_grubu = dt.Rows[0]["ticari_islem_grubu"].acekaToByte();
                carikartFinans.ilgili_sube_carikart_id = dt.Rows[0]["ilgili_sube_carikart_id"].acekaToLong();
                //carikartFinans.ilgili_sube_cari_unvan = dt.Rows[0]["ilgili_sube_cari_unvan"].ToString();
                carikartFinans.finans_sorumlu_carikart_id = dt.Rows[0]["finans_sorumlu_carikart_id"].acekaToLong();
                //carikartFinans.finans_sorumlu_cari_unvan = dt.Rows[0]["finans_sorumlu_cari_unvan"].ToString();
                carikartFinans.swift_kodu = dt.Rows[0]["swift_kodu"].ToString();
                carikartFinans.tedarik_gunu = dt.Rows[0]["tedarik_gunu"].acekaToInt();
                carikartFinans.cari_hesapta_ciksin = dt.Rows[0]["cari_hesapta_ciksin"].acekaToBool();

                carikartFinans.carikart_muhasebe = new carikart_muhasebe();
                carikartFinans.carikart_muhasebe.sirket_id = dt.Rows[0]["sirket_id"].acekaToByte();
                carikartFinans.carikart_muhasebe.muh_kod = dt.Rows[0]["muh_kod"].ToString();
                carikartFinans.carikart_muhasebe.sene = dt.Rows[0]["sene"].acekaToInt();
                carikartFinans.carikart_muhasebe.masraf_merkezi_id = dt.Rows[0]["masraf_merkezi_id"].acekaToShort();
                // carikartFinans.carikart_muhasebe.masraf_merkezi_adi = dt.Rows[0]["masraf_merkezi_adi"].ToString();

                carikartFinans.carikart = new cari_kart();
                carikartFinans.carikart.fiyattipi = dt.Rows[0]["fiyattipi"].ToString();
            }

            return carikartFinans;
        }

        /// <summary>
        /// Şube Listesi sekmesini getiren Metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public List<cari_kart> SubeListesiGetir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            SELECT 
	                        C.carikart_id,
	                        C.cari_unvan,
                            c.kayit_silindi,
	                        C.statu,
	                        C.carikart_turu_id,
                            ctur.carikart_turu_adi,
	                        C.carikart_tipi_id ,
							ct.carikart_tipi_adi,
                            C.ana_carikart_id
	                        from carikart C 
                            left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
                            left join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id
							where C.kayit_silindi=0 and ana_carikart_id=@carikart_id
                ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartlar = new List<cari_kart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart = new cari_kart();
                    carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikart.statu = dt.Rows[i]["statu"].acekaToBool();
                    carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    carikart.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    carikart.ana_carikart_id = dt.Rows[i]["ana_carikart_id"].acekaToLong();


                    carikart.giz_sabit_carikart_turu = new giz_sabit_carikart_turu();
                    carikart.giz_sabit_carikart_turu.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    carikart.giz_sabit_carikart_turu.carikart_turu_adi = dt.Rows[i]["carikart_turu_adi"].ToString();

                    carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                    carikart.giz_sabit_carikart_tipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    carikart.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();
                    carikartlar.Add(carikart);
                    carikart = null;

                }

            }
            return carikartlar;
        }

        /// <summary>
        /// Şirket Listesini getiren Metod
        /// </summary>
        /// <returns></returns>
        public List<giz_sirket> SirketListesiGetir()
        {
            #region Query
            string query = @"
                            --Table[0]
                            SELECT 
	                        sirket_id,
                            sirket_adi
	                        from giz_sirket 
							where kayit_silindi=0 
                ";
            #endregion
            #region Parameters
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                gizsirketlist = new List<giz_sirket>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gizsirket = new giz_sirket();
                    gizsirket.sirket_id = dt.Rows[i]["sirket_id"].acekaToByte();
                    gizsirket.sirket_adi = dt.Rows[i]["sirket_adi"].ToString();
                    gizsirketlist.Add(gizsirket);
                    gizsirket = null;
                }

            }
            return gizsirketlist;
        }

        /// <summary>
        /// Carikart a ait Notların Listesi.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public List<carikart_genel_notlar> CarikartNotlar(long carikart_id)
        {
            List<carikart_genel_notlar> notlar = null;

            #region Query
            string query = @"
                            SELECT 
	                            carikart_not_id, 
                                carikart_id, 
                                aciklama, 
                                kayit_silindi,
                                nereden
                            FROM carikart_genel_notlar 
                            WHERE  carikart_id = @carikart_id AND kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                notlar = new List<carikart_genel_notlar>();

                carikart_genel_notlar not = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    not = new carikart_genel_notlar();
                    not.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    not.carikart_not_id = dt.Rows[i]["carikart_not_id"].acekaToLong();
                    not.aciklama = dt.Rows[i]["aciklama"].ToString();
                    not.nereden = dt.Rows[i]["nereden"].ToString();
                    notlar.Add(not);
                    not = null;
                }

            }
            return notlar;
        }

        /// <summary>
        /// Update işlemleri için Not Detay çağıran metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <param name="carikart_not_id"></param>
        /// <returns></returns>
        public carikart_genel_notlar CarikartNotDetay(long carikart_id, long carikart_not_id)
        {
            carikart_genel_notlar not = null;

            #region Query
            string query = @"
                            SELECT 
	                            carikart_not_id, 
                                carikart_id, 
                                aciklama, 
                                kayit_silindi,
                                nereden
                            FROM carikart_genel_notlar 
                            WHERE carikart_not_id = @carikart_not_id  AND carikart_id = @carikart_id AND kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id),
               new SqlParameter("@carikart_not_id",carikart_not_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                not = new carikart_genel_notlar();
                not.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                not.carikart_not_id = dt.Rows[0]["carikart_not_id"].acekaToLong();
                not.aciklama = dt.Rows[0]["aciklama"].ToString();
                not.nereden = dt.Rows[0]["nereden"].ToString();
                not.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
            }
            return not;
        }

        /// <summary>
        /// Carikart banka hesapları listesi
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public List<carikart_finans_banka_hesaplari> CarikartBankaHesaplari(long carikart_id)
        {
            List<carikart_finans_banka_hesaplari> bankaHesaplari = null;

            #region Query
            string query = @"
                            SELECT 
	                            CF.carikart_banka_id, 
	                            CF.carikart_id, 
	                            B.ulke_id,
	                            B.banka_id,
	                            B.banka_adi, 
	                            BS.banka_sube_id, 
	                            BS.banka_sube_adi,
	                            CF.ibanno, 
	                            CF.pb, 
	                            CF.ebanka, 
	                            CF.odemehesabi,
	                            CF.kredi_limiti_dbs	
                            FROM carikart_finans_banka_hesaplari CF
                            INNER JOIN parametre_banka_sube BS ON BS.banka_sube_id = CF.banka_sube_id
                            INNER JOIN parametre_banka B ON B.banka_id = BS.banka_id
                            WHERE CF.carikart_id=@carikart_id AND CF.kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                bankaHesaplari = new List<carikart_finans_banka_hesaplari>();
                carikart_finans_banka_hesaplari bankaHesap = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bankaHesap = new carikart_finans_banka_hesaplari();
                    bankaHesap.carikart_banka_id = dt.Rows[i]["carikart_banka_id"].acekaToLong();
                    bankaHesap.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();

                    bankaHesap.banka_id = dt.Rows[i]["banka_id"].acekaToShort();
                    bankaHesap.banka = new parametre_banka();
                    bankaHesap.banka.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    bankaHesap.banka.banka_id = dt.Rows[i]["banka_id"].acekaToShort();
                    bankaHesap.banka.banka_adi = dt.Rows[i]["banka_adi"].ToString();

                    bankaHesap.banka_sube_id = dt.Rows[i]["banka_sube_id"].acekaToShort();
                    bankaHesap.banka.subeler = new List<parametre_banka_sube>{
                        new parametre_banka_sube
                        {
                            banka_sube_id= dt.Rows[i]["banka_sube_id"].acekaToShort(),
                            banka_sube_adi= dt.Rows[i]["banka_sube_adi"].ToString()
                        }
                    };

                    bankaHesap.ibanno = dt.Rows[i]["ibanno"].ToString();
                    bankaHesap.pb = dt.Rows[i]["pb"].ToString();
                    bankaHesap.ebanka = dt.Rows[i]["ebanka"].acekaToBool();
                    bankaHesap.odemehesabi = dt.Rows[i]["odemehesabi"].acekaToBool();
                    bankaHesap.kredi_limiti_dbs = dt.Rows[i]["kredi_limiti_dbs"].acekaToDecimal();
                    bankaHesaplari.Add(bankaHesap);
                    bankaHesap = null;
                }
            }
            return bankaHesaplari;
        }

        /// <summary>
        /// Banka Hesap detayını getirir
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_finans_banka_hesaplari CarikartBankaHesapDetayi(long carikart_banka_id, long carikart_id)
        {
            carikart_finans_banka_hesaplari bankaHesap = null;

            #region Query
            string query = @"
                            SELECT 
	                            CF.carikart_banka_id, 
	                            CF.carikart_id, 
	                            B.banka_id,
	                            B.banka_adi, 
	                            BS.banka_sube_id, 
	                            BS.banka_sube_adi,
	                            CF.ibanno, 
	                            CF.pb, 
	                            CF.ebanka, 
	                            CF.odemehesabi,
	                            CF.kredi_limiti_dbs,
                                CF.kayit_silindi	
                            FROM carikart_finans_banka_hesaplari CF
                            INNER JOIN parametre_banka_sube BS ON BS.banka_sube_id = CF.banka_sube_id
                            INNER JOIN parametre_banka B ON B.banka_id = BS.banka_id
                            WHERE CF.carikart_banka_id=@carikart_banka_id AND CF.carikart_id=@carikart_id AND CF.kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_banka_id",carikart_banka_id),
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                bankaHesap = new carikart_finans_banka_hesaplari();
                bankaHesap.carikart_banka_id = dt.Rows[0]["carikart_banka_id"].acekaToLong();
                bankaHesap.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                bankaHesap.banka_id = dt.Rows[0]["banka_id"].acekaToShort();

                bankaHesap.banka = new parametre_banka();
                bankaHesap.banka.banka_id = dt.Rows[0]["banka_id"].acekaToShort();
                bankaHesap.banka.banka_adi = dt.Rows[0]["banka_adi"].ToString();

                bankaHesap.banka.subeler = new List<parametre_banka_sube>()
                {
                    new parametre_banka_sube {
                        banka_sube_id = dt.Rows[0]["banka_sube_id"].acekaToShort(),
                        banka_sube_adi = dt.Rows[0]["banka_sube_adi"].ToString()
                        }
                };
                bankaHesap.ibanno = dt.Rows[0]["ibanno"].ToString();
                bankaHesap.pb = dt.Rows[0]["pb"].ToString();
                bankaHesap.ebanka = dt.Rows[0]["ebanka"].acekaToBool();
                bankaHesap.odemehesabi = dt.Rows[0]["odemehesabi"].acekaToBool();
                bankaHesap.kredi_limiti_dbs = dt.Rows[0]["kredi_limiti_dbs"].acekaToDecimal();
                bankaHesap.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
            }
            return bankaHesap;
        }

        /// <summary>
        /// Finans Bilgileri POST işlemleri için kullanılan GET metod.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_muhasebe CarikartMuhasebeBilgileri(long carikart_id)
        {
            carikart_muhasebe ckMuhasebe = null;

            #region Query
            string query = @"
                        SELECT
	                        carikart_id, 
	                        sirket_id, 
	                        sene, 
	                        degistiren_carikart_id, 
	                        degistiren_tarih, 
	                        muh_kod, 
	                        masraf_merkezi_id
                        FROM [dbo].[carikart_muhasebe]
                        WHERE carikart_id = @carikart_id                                                          
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckMuhasebe = new carikart_muhasebe();
                ckMuhasebe.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ckMuhasebe.sirket_id = dt.Rows[0]["sirket_id"].acekaToByte();
                ckMuhasebe.sene = dt.Rows[0]["sene"].acekaToInt();
                ckMuhasebe.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckMuhasebe.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckMuhasebe.muh_kod = dt.Rows[0]["muh_kod"].ToString();
                ckMuhasebe.masraf_merkezi_id = dt.Rows[0]["muh_kod"].acekaToShort();
            }

            return ckMuhasebe;
        }

        /// <summary>
        /// Finans Bilgileri POST işlemleri için kullanılan GET metod.
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_fiyat_tipi CarikartFiyatTipi(long carikart_id)
        {
            carikart_fiyat_tipi ckFiyatTipi = null;

            #region Query
            string query = @"
                        SELECT
	                        carikart_id, 
	                        fiyattipi, 
	                        degistiren_carikart_id, 
	                        degistiren_tarih, 
	                        statu, 
	                        varsayilan
                        FROM carikart_fiyat_tipi
                        WHERE kayit_silindi = 0 AND carikart_id = @carikart_id                                                        
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckFiyatTipi = new carikart_fiyat_tipi();
                ckFiyatTipi.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ckFiyatTipi.fiyattipi = dt.Rows[0]["fiyattipi"].ToString();
                ckFiyatTipi.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckFiyatTipi.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckFiyatTipi.statu = dt.Rows[0]["statu"].acekaToBool();
                ckFiyatTipi.varsayilan = dt.Rows[0]["varsayilan"].acekaToBool();
            }

            return ckFiyatTipi;
        }
        public carikart_earsiv CarikartEarsivBilgileri(long carikart_id, string earsivseri)
        {
            carikart_earsiv ckearsiv = null;

            #region Query
            string query = @"
                         --Table[0]
                        SELECT
						    carikart_id, 
						    sene, 
						    earsiv_seri, 
						    degistiren_carikart_id, 
						    degistiren_tarih
                        FROM carikart_earsiv
                        WHERE carikart_id = @carikart_id and  earsiv_seri=@earsiv_seri                                                      
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id),
               new  SqlParameter("@earsiv_seri",earsivseri)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckearsiv = new carikart_earsiv();
                ckearsiv.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ckearsiv.sene = dt.Rows[0]["sene"].acekaToInt();
                ckearsiv.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckearsiv.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckearsiv.earsiv_seri = dt.Rows[0]["earsiv_seri"].ToString();
            }

            return ckearsiv;
        }
        public carikart_efatura CarikartEfaturaBilgileri(long carikart_id)
        {
            carikart_efatura ckefatura = null;

            #region Query
            string query = @"
                         --Table[0]
                        SELECT
						    carikart_id, 
						    efatura_seri, 
						    degistiren_carikart_id, 
						    degistiren_tarih
                        FROM carikart_efatura
                        WHERE carikart_id = @carikart_id                                                        
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckefatura = new carikart_efatura();
                ckefatura.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ckefatura.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckefatura.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckefatura.efatura_seri = dt.Rows[0]["efatura_seri"].ToString();
            }

            return ckefatura;
        }
        public carikart_stokyeri CarikartStokYerilgileri(long carikart_id)
        {
            carikart_stokyeri ckstokyeri = null;

            #region Query
            string query = @"
                        SELECT
	                         [carikart_id]
                            ,[degistiren_carikart_id]
                            ,[degistiren_tarih]
                            ,[acilis_tarihi]
                            ,[kapanis_tarihi]
                        FROM carikart_stokyeri
                        WHERE carikart_id = @carikart_id                                                        
                ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ckstokyeri = new carikart_stokyeri();
                ckstokyeri.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ckstokyeri.acilis_tarihi = dt.Rows[0]["acilis_tarihi"].acekaToDateTimeWithNullable();
                ckstokyeri.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                ckstokyeri.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                ckstokyeri.kapanis_tarihi = dt.Rows[0]["kapanis_tarihi"].acekaToDateTimeWithNullable();

            }

            return ckstokyeri;
        }

        #endregion

        #region Depokart Rapor Parametrelri GET carikart_rapor_parametre
        //Yapılacak.
        #endregion

        #region Carikart Aksesuar Ekleme

        public List<carikart_denetim_aksesuar> CarikartDenetimAksesuarList(long carikart_id, byte tip)
        {

            #region Query
            //string query = @"SELECT  a.degistiren_carikart_id,a.degistiren_tarih,miktar,carikart_id,tip,sira,s.stokkart_id,aksesuarkart_id,stok_kodu,
            //                stok_adi,o.orjinal_stok_kodu,o.orjinal_stok_adi,o.orjinal_renk_adi,o.orjinal_renk_kodu,kosul,b.birim_adi,carikart_id,
            //                s.stokkart_id,aksesuarkart_id,stok_kodu,stok_adi,o.orjinal_stok_kodu,o.orjinal_stok_adi,o.orjinal_renk_adi,o.orjinal_renk_kodu,kosul,s.birim_id_1,b.birim_adi,s.stok_kodu,s.stok_adi,s.birim_id_1,r.renk_id, isnull(r.renk_adi, 'Her Hangi Bir Renk') as renk_adi,b.ondalik
            //                FROM carikart_denetim_aksesuar a
            //                INNER JOIN stokkart s ON s.stokkart_id = a.aksesuarkart_id
            //                INNER JOIN stokkart_ozel o ON o.stokkart_id = a.aksesuarkart_id
            //                LEFT JOIN parametre_birim b ON b.birim_id=s.birim_id_1
            //                LEFT JOIN parametre_renk r ON r.renk_id=a.renk_id
            //                WHERE carikart_id=@carikart_id and tip=@tip";
            #endregion
            
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id),
               new SqlParameter("@tip",tip)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, "[NetUretim].[dbo].[SP_CarikartDenetimAksesuarList]", parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                carikartdenetimaks = new List<carikart_denetim_aksesuar>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart_denetim_aksesuar carikartdenetim = new carikart_denetim_aksesuar();

                    carikartdenetim.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikartdenetim.tip = dt.Rows[i]["tip"].acekaToByte();
                    carikartdenetim.sira = dt.Rows[i]["sira"].acekaToShort();
                    carikartdenetim.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    carikartdenetim.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    carikartdenetim.aksesuarkart_id = dt.Rows[i]["aksesuarkart_id"].acekaToLong();
                    carikartdenetim.renk_id = dt.Rows[i]["renk_id"].acekaToInt();
                    carikartdenetim.miktar = dt.Rows[i]["miktar"].acekaToLong();
                    carikartdenetim.kosul = dt.Rows[i]["kosul"].acekaToString();

                    carikartdenetim.stokkart = new stokkart();
                    carikartdenetim.stokkart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    carikartdenetim.stokkart.stok_kodu = dt.Rows[i]["stok_kodu"].acekaToString();
                    carikartdenetim.stokkart.stok_adi = dt.Rows[i]["stok_adi"].acekaToString();
                    carikartdenetim.stokkart.birim_id_1 = dt.Rows[i]["birim_id_1"].acekaToByte();

                    carikartdenetim.stokkart_ozel = new stokkart_ozel();
                    carikartdenetim.stokkart_ozel.orjinal_stok_kodu = dt.Rows[i]["orjinal_stok_kodu"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_stok_adi = dt.Rows[i]["orjinal_stok_adi"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_renk_adi = dt.Rows[i]["orjinal_renk_adi"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_renk_kodu = dt.Rows[i]["orjinal_renk_kodu"].acekaToString();

                    carikartdenetim.parametre_birim = new parametre_birim();
                    carikartdenetim.parametre_birim.birim_adi = dt.Rows[i]["birim_adi"].acekaToString();
                    carikartdenetim.parametre_birim.ondalik = dt.Rows[i]["ondalik"].acekaToByte();

                    carikartdenetim.parametre_renk = new parametre_renk();
                    carikartdenetim.parametre_renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();

                    carikartdenetimaks.Add(carikartdenetim);
                    carikartdenetim = null;
                }
            }
            return carikartdenetimaks;
        }
        public carikart_denetim_aksesuar CarikartDenetimAksesuar(long carikart_id, byte tip)
        {

            #region Query
            string query = @"SELECT  a.degistiren_carikart_id,a.degistiren_tarih,miktar,carikart_id,tip,sira,s.stokkart_id,aksesuarkart_id,stok_kodu,
                            stok_adi,o.orjinal_stok_kodu,o.orjinal_stok_adi,o.orjinal_renk_adi,o.orjinal_renk_kodu,kosul,b.birim_adi,carikart_id,
                            s.stokkart_id,aksesuarkart_id,stok_kodu,stok_adi,o.orjinal_stok_kodu,o.orjinal_stok_adi,o.orjinal_renk_adi,o.orjinal_renk_kodu,kosul,s.birim_id_1,b.birim_adi,
                            s.stok_kodu,s.stok_adi,s.birim_id_1,r.renk_id,isnull(r.renk_adi, 'Her Hangi Bir Renk') as renk_adi,b.ondalik
                            FROM carikart_denetim_aksesuar a
                            INNER JOIN stokkart s ON s.stokkart_id = a.aksesuarkart_id
                            INNER JOIN stokkart_ozel o ON o.stokkart_id = a.aksesuarkart_id
                            LEFT JOIN parametre_birim b ON b.birim_id=s.birim_id_1
                            LEFT JOIN parametre_renk r ON r.renk_id=a.renk_id
                            WHERE carikart_id=@carikart_id and tip=@tip";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id),
               new SqlParameter("@tip",tip)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            carikart_denetim_aksesuar carikartdenetim = new carikart_denetim_aksesuar();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikartdenetim.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    carikartdenetim.tip = dt.Rows[i]["tip"].acekaToByte();
                    carikartdenetim.sira = dt.Rows[i]["sira"].acekaToShort();
                    carikartdenetim.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    carikartdenetim.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    carikartdenetim.aksesuarkart_id = dt.Rows[i]["aksesuarkart_id"].acekaToLong();
                    carikartdenetim.renk_id = dt.Rows[i]["renk_id"].acekaToInt();
                    carikartdenetim.miktar = dt.Rows[i]["miktar"].acekaToLong();
                    carikartdenetim.kosul = dt.Rows[i]["kosul"].acekaToString();

                    carikartdenetim.stokkart = new stokkart();
                    carikartdenetim.stokkart.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    carikartdenetim.stokkart.stok_kodu = dt.Rows[i]["stok_kodu"].acekaToString();
                    carikartdenetim.stokkart.stok_adi = dt.Rows[i]["stok_adi"].acekaToString();
                    carikartdenetim.stokkart.birim_id_1 = dt.Rows[i]["birim_id_1"].acekaToByte();

                    carikartdenetim.stokkart_ozel = new stokkart_ozel();
                    carikartdenetim.stokkart_ozel.orjinal_stok_kodu = dt.Rows[i]["orjinal_stok_kodu"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_stok_adi = dt.Rows[i]["orjinal_stok_adi"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_renk_adi = dt.Rows[i]["orjinal_renk_adi"].acekaToString();
                    carikartdenetim.stokkart_ozel.orjinal_renk_kodu = dt.Rows[i]["orjinal_renk_kodu"].acekaToString();

                    carikartdenetim.parametre_birim = new parametre_birim();
                    carikartdenetim.parametre_birim.birim_adi = dt.Rows[i]["birim_adi"].acekaToString();
                    carikartdenetim.parametre_birim.ondalik = dt.Rows[i]["ondalik"].acekaToByte();

                    carikartdenetim.parametre_renk = new parametre_renk();
                    carikartdenetim.parametre_renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();

                }
            }
            return carikartdenetim;
        }

        #endregion

        #region Carikart Aksesuar Koşulları
        public List<carikart_denetim_aksesuar_kosullar> CarikartAksesuarKosulList()
        {
            #region Query
            string query = @"select grup_adi,sira,param_tanim,param_field_name,direkt_kosul,operator_liste,cevap_liste_sql,tip from carikart_denetim_aksesuar_kosullar";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                carikartKosullar = new List<carikart_denetim_aksesuar_kosullar>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart_denetim_aksesuar_kosullar carikartKosul = new carikart_denetim_aksesuar_kosullar();

                    carikartKosul.grup_adi = dt.Rows[i]["grup_adi"].ToString();
                    carikartKosul.tip = dt.Rows[i]["tip"].acekaToByte();
                    carikartKosul.sira = dt.Rows[i]["sira"].acekaToByte();
                    carikartKosul.operator_liste = dt.Rows[i]["operator_liste"].ToString();
                    carikartKosul.param_tanim = dt.Rows[i]["param_tanim"].ToString();
                    carikartKosul.param_field_name = dt.Rows[i]["param_field_name"].ToString();
                    carikartKosul.direkt_kosul = dt.Rows[i]["direkt_kosul"].acekaToBool();
                    carikartKosul.cevap_liste_sql = dt.Rows[i]["cevap_liste_sql"].ToString();
                    carikartKosullar.Add(carikartKosul);
                    carikartKosul = null;
                }
            }
            return carikartKosullar;
        }
        public List<carikart_denetim_aksesuar_kosullar> CarikartAksesuarKosulList(byte sira, string grup_adi)
        {
            #region Query
            string query = @"SELECT grup_adi,sira,param_tanim,param_field_name,direkt_kosul,operator_liste,cevap_liste_sql,tip 
                              FROM carikart_denetim_aksesuar_kosullar
                              WHERE sira=@sira and grup_adi=@grup_adi";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@sira",sira),
               new SqlParameter("@grup_adi",grup_adi)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query,parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                carikartKosullar = new List<carikart_denetim_aksesuar_kosullar>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart_denetim_aksesuar_kosullar carikartKosul = new carikart_denetim_aksesuar_kosullar();

                    carikartKosul.grup_adi = dt.Rows[i]["grup_adi"].ToString();
                    carikartKosul.tip = dt.Rows[i]["tip"].acekaToByte();
                    carikartKosul.sira = dt.Rows[i]["sira"].acekaToByte();
                    carikartKosul.operator_liste = dt.Rows[i]["operator_liste"].ToString();
                    carikartKosul.param_tanim = dt.Rows[i]["param_tanim"].ToString();
                    carikartKosul.param_field_name = dt.Rows[i]["param_field_name"].ToString();
                    carikartKosul.direkt_kosul = dt.Rows[i]["direkt_kosul"].acekaToBool();
                    carikartKosul.cevap_liste_sql = dt.Rows[i]["cevap_liste_sql"].ToString();
                    carikartKosullar.Add(carikartKosul);
                    carikartKosul = null;
                }
            }
            return carikartKosullar;
        }
        public List<string> CarikartAksesuarKosullari(byte sira, string grup_adi)
        {
            List<string> Olist = new List<string>();
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@grup_adi",grup_adi),
               new SqlParameter("@sira",sira)
            };
            #endregion
            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, "SP_CarikartAksesuarKosullari", parameters);
            if (ds.Tables.Count > 0)
            {
                Olist = Tools.CreateJson(ds);
            }
            return Olist;
        }
        #endregion
    }
}

