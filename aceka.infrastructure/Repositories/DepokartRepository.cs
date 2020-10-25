using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Repositories
{
    public class DepokartRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;
        private cari_kart carikart = null;
        private List<cari_kart> carikartlar = null;
        private giz_sirket sirket;
        private List<giz_sirket> sirketler;
        #endregion

        public cari_kart Getir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                          	   	SELECT  c.carikart_id,c.statu,c.cari_unvan,tur.carikart_turu_adi ,c.transfer_depo_id,c.kayit_silindi,c.ozel_kod,
                                  c.giz_yazilim_kodu,ea.earsiv_seri,ef.efatura_seri,m.muh_kod,m.sirket_id,masraf.masraf_merkezi_adi,sy.acilis_tarihi,sy.kapanis_tarihi,sy.kapali,
                                    c.ana_carikart_id,c.cari_unvan as baglistokyeri_unvan,
									--ck.cari_unvan as baglistokyeri_unvan,tip.carikart_tipi_id,tur.carikart_turu_id,masraf.masraf_merkezi_id,
                                    sy.transfer_depo_kullan --,c.ana_carikart_id
									--CK.ana_carikart_id
                            ,gs.sirket_adi,gs.sirket_id, tip.carikart_tipi_adi as stokyeri_tipi_adi, 
                                --ana_carikart_id > 0 ise sorguya ana_cari_unvan adında bir alan ekleniyor
                                    (CASE
                                    WHEN c.ana_carikart_id > 0 THEN (Select cari_unvan from  carikart where carikart_id = C.ana_carikart_id)
                                    END) as 'ana_cari_unvan',c.carikart_turu_id,c.carikart_tipi_id,masraf.masraf_merkezi_id
	                        FROM carikart c 
                            --LEFT join carikart ck on ck.ana_carikart_id= c.carikart_id 
	                        LEFT join carikart_earsiv ea on ea.carikart_id=c.carikart_id
	                        INNER join giz_sabit_carikart_tipi tip on tip.carikart_tipi_id=c.carikart_tipi_id --and tip.carikart_tipi_id in(2,3)
	                        INNER join giz_sabit_carikart_turu tur on tur.carikart_turu_id=c.carikart_turu_id and tur.carikart_turu_id = 3
	                        LEFT join carikart_efatura ef on ef.carikart_id=c.carikart_id
	                        LEFT join carikart_muhasebe m on m.carikart_id=c.carikart_id
	                        LEFT join muhasebe_tanim_masrafmerkezleri masraf on masraf.masraf_merkezi_id = m.masraf_merkezi_id
                            LEFT join giz_sirket gs on gs.sirket_id=m.sirket_id
	                        LEFT join carikart_stokyeri sy on sy.carikart_id=c.carikart_id 
                            WHERE c.carikart_id= @carikart_id --AND C.statu=1
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

                carikart.carikart_id        = ds.Tables[0].Rows[0]["carikart_id"].acekaToLong();
                carikart.statu              = ds.Tables[0].Rows[0]["statu"].acekaToBool();
                carikart.cari_unvan         = ds.Tables[0].Rows[0]["cari_unvan"].ToString();
                carikart.ozel_kod           = ds.Tables[0].Rows[0]["ozel_kod"].ToString();

                carikart.ana_carikart_id    = ds.Tables[0].Rows[0]["ana_carikart_id"].acekaToLong();
                carikart.ana_cari_unvan     = ds.Tables[0].Rows[0]["ana_cari_unvan"].ToString();


                carikart.giz_sabit_carikart_turu                    = new giz_sabit_carikart_turu();
                carikart.giz_sabit_carikart_turu.carikart_turu_adi  = ds.Tables[0].Rows[0]["carikart_turu_adi"].ToString();
                carikart.giz_sabit_carikart_turu.carikart_turu_id   = ds.Tables[0].Rows[0]["carikart_turu_id"].acekaToByte();

                carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                carikart.giz_sabit_carikart_tipi.carikart_tipi_adi  = ds.Tables[0].Rows[0]["stokyeri_tipi_adi"].ToString();
                carikart.giz_sabit_carikart_tipi.carikart_tipi_id   = ds.Tables[0].Rows[0]["carikart_tipi_id"].acekaToByte();


                carikart.transfer_depo_id   = ds.Tables[0].Rows[0]["transfer_depo_id"].acekaToLong();
                carikart.giz_yazilim_kodu   = ds.Tables[0].Rows[0]["giz_yazilim_kodu"].acekaToShort();

                carikart.carikart_earsiv = new carikart_earsiv();
                carikart.carikart_earsiv.earsiv_seri = ds.Tables[0].Rows[0]["earsiv_seri"].ToString();

                carikart.carikart_efatura = new carikart_efatura();
                carikart.carikart_efatura.efatura_seri = ds.Tables[0].Rows[0]["efatura_seri"].ToString();

                carikart.carikart_muhasebe = new carikart_muhasebe();
                carikart.carikart_muhasebe.muh_kod      = ds.Tables[0].Rows[0]["muh_kod"].ToString();
                carikart.carikart_muhasebe.sirket_id    = ds.Tables[0].Rows[0]["sirket_id"].acekaToByte();

                carikart.muhasebe_tanim_masrafmerkezleri    = new muhasebe_tanim_masrafmerkezleri();
                carikart.muhasebe_tanim_masrafmerkezleri.masraf_merkezi_adi = ds.Tables[0].Rows[0]["masraf_merkezi_adi"].ToString();
                carikart.muhasebe_tanim_masrafmerkezleri.masraf_merkezi_id  = ds.Tables[0].Rows[0]["masraf_merkezi_id"].acekaToInt();

                carikart.carikart_stokyeri = new carikart_stokyeri();
                carikart.carikart_stokyeri.acilis_tarihi        = ds.Tables[0].Rows[0]["acilis_tarihi"].acekaToDateTimeWithNullable();
                carikart.carikart_stokyeri.kapanis_tarihi       = ds.Tables[0].Rows[0]["kapanis_tarihi"].acekaToDateTimeWithNullable();
                carikart.carikart_stokyeri.kapali               = ds.Tables[0].Rows[0]["kapali"].acekaToBoolWithNullable();
                carikart.carikart_stokyeri.transfer_depo_kullan = ds.Tables[0].Rows[0]["transfer_depo_kullan"].acekaToBoolWithNullable();
                


                carikart.giz_sirket = new giz_sirket();
                carikart.giz_sirket.sirket_adi  = ds.Tables[0].Rows[0]["sirket_adi"].ToString();
                carikart.giz_sirket.sirket_id   = ds.Tables[0].Rows[0]["sirket_id"].acekaToByte();
            }

            return carikart;
        }

        /// <summary>
        /// Depo adreslerini getirir. carikart_tipi_id in(2,3) (Depo), carikart_turu_id = 3 (Lokasyon)
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_genel_adres DepokartAdresleriniGetir(long carikart_id)
        {
            //List<carikart_genel_adres> adresler = null;
            #region Query
            string query = @"
                        SELECT 
                        GA.carikart_id,
                        GA.carikart_adres_id,GA.adres_tipi_id,
                        GA.adres,PUL.ulke_adi,PUS.sehir_adi,
                        PIL.ilce_adi,PSM.semt_adi,GA.postakodu,GA.yetkili_ad_soyad,
                        GA.Tel1,GA.Tel2,GA.fax,GA.yetkili_tel,GA.email,GA.websitesi,
                        GA.adresunvan,GA.ulke_id,GA.sehir_id,GA.ilce_id,GA.semt_id,GA.postakodu
                        from carikart_genel_adres GA
                        inner join carikart C on C.carikart_id = GA.carikart_id and carikart_turu_id = 3 -- and carikart_tipi_id in(2,3) depolar.
                        left join parametre_ulke PUL on PUL.ulke_id = GA.ulke_id
                        left join parametre_ulke_sehir PUS on PUS.sehir_id = GA.sehir_id
                        left join parametre_ulke_sehir_ilce PIL on PIL.ilce_id = GA.ilce_id
                        left join parametre_ulke_sehir_ilce_semt PSM on PSM.semt_id = GA.semt_id
                        where GA.adres_tipi_id='DI' AND GA.carikart_id = @carikart_id and isnull(C.kayit_silindi,0) = 0 and isnull(GA.kayit_silindi,0) = 0
                        --DI -> Depo İletişim demek.
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            carikart_genel_adres adres = new carikart_genel_adres();
            if (dt != null && dt.Rows.Count > 0)
            {
                adres.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                adres.carikart_adres_id = dt.Rows[0]["carikart_adres_id"].acekaToLong();
                adres.adresunvan = dt.Rows[0]["adresunvan"].ToString(); //Resmi Ünvan
                adres.adres = dt.Rows[0]["adres"].ToString();
                adres.ulke_id = dt.Rows[0]["ulke_id"].acekaToShort();
                adres.sehir_id = dt.Rows[0]["sehir_id"].acekaToShort();
                adres.ilce_id = dt.Rows[0]["ilce_id"].acekaToShort();
                adres.semt_id = dt.Rows[0]["semt_id"].acekaToShort();
                adres.tel1 = dt.Rows[0]["tel1"].ToString();
                adres.tel2 = dt.Rows[0]["tel2"].ToString();
                adres.fax = dt.Rows[0]["fax"].ToString();
                adres.email = dt.Rows[0]["email"].ToString();
                adres.websitesi = dt.Rows[0]["websitesi"].ToString();
                adres.postakodu = dt.Rows[0]["postakodu"].ToString();
                adres.yetkili_ad_soyad = dt.Rows[0]["yetkili_ad_soyad"].ToString();
                adres.yetkili_tel = dt.Rows[0]["yetkili_tel"].ToString(); //Cep telefonu
                adres.adres_tipi_id = dt.Rows[0]["adres_tipi_id"].ToString();

            }
            return adres;


        }

        private List<carikart_firma_ozel> CarikartFirmaOzelGetir(DataTable dt)
        {
            List<carikart_firma_ozel> carikartfirma_ozel = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                carikartfirma_ozel = new List<carikart_firma_ozel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikart_firma_ozel carikart_firma_ozel = new carikart_firma_ozel();
                    carikart_firma_ozel.carikart_id = Convert.ToInt64("0" + dt.Rows[i]["carikart_id"].ToString());
                    carikart_firma_ozel.degistiren_carikart_id = Convert.ToInt64(dt.Rows[i]["degistiren_carikart_id"].ToString());
                    carikart_firma_ozel.degistiren_tarih = Convert.ToDateTime(dt.Rows[i]["degistiren_tarih"].ToString());
                    carikart_firma_ozel.satin_alma_sorumlu_carikart_id = Convert.ToInt64(dt.Rows[i]["satin_alma_sorumlu_carikart_id"].ToString());
                    carikart_firma_ozel.satis_sorumlu_carikart_id = Convert.ToInt64(dt.Rows[i]["satis_sorumlu_carikart_id"].ToString());
                    carikart_firma_ozel.baslamatarihi = dt.Rows[i]["baslamatarihi"].acekaToDateTime().Date;
                    carikart_firma_ozel.ozel = dt.Rows[i]["ozel"].ToString();
                    carikartfirma_ozel.Add(carikart_firma_ozel);
                    carikart_firma_ozel = null;
                }
            }
            return carikartfirma_ozel;
        }

        /// <summary>
        /// Cari bulma fonksiyonu
        /// </summary>
        /// <param name="cari"></param>
        /// <returns></returns>
        public List<cari_kart> DepoBul(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", byte carikart_tipi_id = 0)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (carikart_id > 0)
            {
                parameterControl++;
                orStatement += "c.carikart_id = @carikart_id OR ";
            }
            if (!string.IsNullOrEmpty(cari_unvan.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.cari_unvan like @unvan OR ";
            }

            if (!string.IsNullOrEmpty(ozel_kod.TrimEnd()))
            {
                parameterControl++;
                orStatement += "c.ozel_kod like @ozelkod OR ";
            }
            if (carikart_tipi_id > 0)
            {
                parameterControl++;
                orStatement += "c.carikart_tipi_id = @carikart_tipi_id OR ";
            }

            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'O', 'R', ' ' }) + ")";
            }

            orStatement += "";
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
		                         LEFT JOIN  carikart_finans f on f.carikart_id=c.carikart_id
		                         LEFT JOIN carikart_firma_ozel o on o.carikart_id=c.carikart_id
                                 LEFT JOIN giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = c.carikart_turu_id
                                 INNER join giz_sabit_carikart_tipi ct on c.carikart_tipi_id=ct.carikart_tipi_id  and ct.carikart_tipi_id in(2,3)
                                 LEFT JOIN carikart_genel_adres ca on c.carikart_id=ca.carikart_id 
                                 LEFT JOIN carikart_finans cf on cf.carikart_id=c.carikart_id 
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

                        //carikart = new cari_kart();
                        //carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                        //carikart.statu = dt.Rows[i]["statu"].acekaToBool();
                        //carikart.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        //carikart.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        //carikart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        //carikart.ozel_kod = dt.Rows[i]["ozel_kod"].ToString();
                        //carikart.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                        //carikart.giz_yazilim_kodu = dt.Rows[i]["giz_yazilim_kodu"].acekaToShort();

                        //carikart.giz_sabit_carikart_turu = new giz_sabit_carikart_turu();
                        //carikart.giz_sabit_carikart_turu.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                        //carikart.giz_sabit_carikart_turu.carikart_turu_adi = dt.Rows[i]["carikart_turu_adi"].ToString();

                        //carikart.giz_sabit_carikart_tipi = new giz_sabit_carikart_tipi();
                        //carikart.giz_sabit_carikart_tipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                        //carikart.giz_sabit_carikart_tipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();

                        //carikart.carikart_finans = new carikart_finans();
                        //carikart.carikart_finans.pb = dt.Rows[i]["pb"].ToString();

                        //carikartlar.Add(carikart);
                        //carikart = null;
                    }
                }
            }
            return carikartlar;
        }

        /// <summary>
        /// Özel Alanlar Sekmesini getiren Metod
        /// </summary>
        /// <param name="carikart_id"></param>
        /// <returns></returns>
        public carikart_firma_ozel DepokartOzelalanlarGetir(long carikart_id)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select 
                            CK.carikart_id,
							(select cari_unvan from carikart cr where cr.carikart_id= CK.satin_alma_sorumlu_carikart_id) as satin_alma_sorumlu_unvan,
							(select cari_unvan from carikart cr where cr.carikart_id= CK.satis_sorumlu_carikart_id) as satis_sorumlu_unvan,
                            CK.baslamatarihi,CK.ozel
                            FROM carikart_firma_ozel CK
							INNER JOIN carikart C ON c.carikart_id=CK.carikart_id
                            WHERE CK.carikart_id = @carikart_id
                ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            carikart_firma_ozel ozelalan = new carikart_firma_ozel();
            if (dt != null && dt.Rows.Count > 0)
            {
                ozelalan.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                ozelalan.baslamatarihi = dt.Rows[0]["baslamatarihi"].acekaToDateTime();
                ozelalan.satis_sorumlu_cari_unvan = dt.Rows[0]["satis_sorumlu_unvan"].ToString();
                ozelalan.satin_alma_sorumlu_cari_unvan = dt.Rows[0]["satin_alma_sorumlu_unvan"].ToString();
                ozelalan.ozel = dt.Rows[0]["ozel"].ToString();
            }
            return ozelalan;
        }

        public List<carikart_fiyat_tipi> DepokartFiyatTipleri(long carikart_id)
        {
            List<carikart_fiyat_tipi> fiyatTipleri = null;

            #region Query
            string query = @"
                            SELECT
	                            CF.carikart_id, 
                                FT.fiyattipi_adi,
                                CF.fiyattipi,
                                CF.degistiren_carikart_id,
                                CF.degistiren_tarih,
                                CF.kayit_silindi,
                                CF.statu,
                                CF.varsayilan
                            FROM carikart_fiyat_tipi CF
                            LEFT JOIN parametre_fiyattipi FT on FT.fiyattipi=CF.fiyattipi
                            WHERE carikart_id= @carikart_id AND CF.kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikart_fiyat_tipi fiyatTipi = null;
                fiyatTipleri = new List<carikart_fiyat_tipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fiyatTipi = new carikart_fiyat_tipi();
                    fiyatTipi.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    fiyatTipi.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
                    fiyatTipi.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    fiyatTipi.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    fiyatTipi.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    fiyatTipi.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    fiyatTipi.statu = dt.Rows[i]["statu"].acekaToBool();
                    fiyatTipi.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                    fiyatTipleri.Add(fiyatTipi);
                    fiyatTipi = null;
                }
            }
            return fiyatTipleri;
        }

        public carikart_fiyat_tipi DepokartFiyatTipiDetay(long carikart_id, string fiyatTipi)
        {
            carikart_fiyat_tipi fiyatTip = null;

            #region Query
            string query = @"
                            SELECT
	                            CF.carikart_id, 
                                FT.fiyattipi_adi,
                                CF.fiyattipi,
                                CF.degistiren_carikart_id,
                                CF.degistiren_tarih,
                                CF.kayit_silindi,
                                CF.statu,
                                CF.varsayilan
                            FROM carikart_fiyat_tipi CF
                            INNER JOIN parametre_fiyattipi FT on FT.fiyattipi=CF.fiyattipi
                            WHERE carikart_id= @carikart_id AND CF.fiyattipi = @fiyattipi AND CF.kayit_silindi = 0
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@carikart_id",carikart_id),
                new SqlParameter("@fiyatTipi",fiyatTipi)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                fiyatTip = new carikart_fiyat_tipi();
                fiyatTip.fiyattipi = dt.Rows[0]["fiyattipi"].ToString();
                fiyatTip.fiyattipi_adi = dt.Rows[0]["fiyattipi_adi"].ToString();
                fiyatTip.carikart_id = dt.Rows[0]["carikart_id"].acekaToLong();
                fiyatTip.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                fiyatTip.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                fiyatTip.statu = dt.Rows[0]["statu"].acekaToBool();
                fiyatTip.varsayilan = dt.Rows[0]["varsayilan"].acekaToBool();

            }
            return fiyatTip;
        }
        /// <summary>
        /// Select için Depo yerlerini getiren metod. carikart_turu_id =3 ve carikart_tipi_id=3 olanlar.
        /// </summary>
        /// <returns>List<carikart></returns>
        public List<cari_kart> DepoListesi()
        {

            #region Query
            string query = @"SELECT 
                            carikart_id,cari_unvan
                            FROM carikart
                            WHERE kayit_silindi = 0 and carikart_turu_id = 3 and carikart_tipi_id = 3
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
                    carikartlar.Add(carikart);
                    carikart = null;
                }
            }

            return carikartlar;
        }



        /// <summary>
        /// Select için Depo yerlerini getiren metod. carikart_turu_id =3 ve carikart_tipi_id=3 olanlar.
        /// </summary>
        /// <returns>List<carikart></returns>
        public List<giz_sirket> SirketListesi()
        {

            #region Query
            string query = @"SELECT 
                            *
                            FROM giz_sirket
                            ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                sirketler = new List<giz_sirket>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sirket = new giz_sirket();
                    sirket.sirket_id = dt.Rows[i]["sirket_id"].acekaToByte();
                    sirket.sirket_adi = dt.Rows[i]["sirket_adi"].ToString();
                    sirketler.Add(sirket);
                    sirket = null;
                }
            }

            return sirketler;
        }


    }
}
