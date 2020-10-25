using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Repositories;
using aceka.infrastructure.Models;
using aceka.infrastructure.Core;


namespace aceka.infrastructure.Repositories
{

    public class PersonelRepository
    {
        #region Degiskenler
        private DataTable dt;
        private DataSet ds;
        private Personel personel = null;
        private Personel_Parametreler personel_parametre = null;
        private List<Personel> personeller = null;
        private carikart_muhasebe_personel muhasebe = null;
        private carikart_personel_calisma_yerleri Per_calisma_yeri = null;
        private List<carikart_personel_calisma_yerleri> Per_calisma_yerleri = null;
        private carikart_genel_kimlik per_kimlik = null;
        private List<giz_sabit_carikart_tipi> caritipler = null;
        private giz_sabit_carikart_tipi caritip = null;
        private parametre_genel parametre = null;
        private List<parametre_genel> parametreler = null;
        private parametre_carikart_rapor personelparametre = null;
        private List<parametre_carikart_rapor> personelparametreler = null;
        #endregion


        /// <summary>
        /// Personel bulma fonksiyonu
        /// </summary>
        /// <param name="cari"></param>
        /// <returns></returns>
        public List<Personel> Bul(long carikart_id = 0, string cari_unvan = "", string ozel_kod = "", int carikart_tipi_id = 0, string statu = "")
        {//bu kısmın where şartını değiştirdim şimdili bu şekilde kullanalım test için daha sonra eski haline getirebiliriz
            short parameterControl = 0;

            #region Query
            string query = @"
			                select
				                CK.carikart_id, CK.degistiren_carikart_id, CK.degistiren_tarih, CK.kayit_silindi, 
				                CK.statu,gs.statu_adi, CK.carikart_turu_id,ctur.carikart_turu_adi as carikart_turu,
				                CK.carikart_tipi_id,ct.carikart_tipi_adi as carikart_tipi, CK.cari_unvan,--gk.dogum_tarihi,
				                CK.ozel_kod,
				                CK.transfer_depo_id, CK.giz_kullanici_adi, CK.giz_kullanici_sifre, 
                                CK.sube_carikart_id
				                --R.cari_parametre_1, R.cari_parametre_2, R.cari_parametre_3, 
				                --R.cari_parametre_4, R.cari_parametre_5, R.cari_parametre_6, 
				                --R.cari_parametre_7, CK.sube_carikart_id
                            from carikart as CK   
                            --INNER JOIN carikart_rapor_parametre R On r.carikart_id =CK.carikart_id             
			                left join giz_sabit_carikart_turu ctur on ctur.carikart_turu_id = CK.carikart_turu_id
			                left join giz_sabit_carikart_tipi ct on CK.carikart_tipi_id=ct.carikart_tipi_id      
			                left join giz_sabit_statu gs on gs.statu = CK.statu 
			                --left join carikart_genel_kimlik gk on gk.carikart_id = CK.carikart_id                         
                            Where CK.carikart_turu_id = 2   ";
            if (carikart_id > 0)
            {
                query += " and CK.carikart_id = " + carikart_id.acekaToLong();
            }
            if (cari_unvan.acekaToString().Length > 0)
            {
                query += " and CK.cari_unvan like '%" + cari_unvan + "%'  ";
            }
            if (ozel_kod.acekaToString().Length > 0)
            {
                query += " and CK.ozel_kod like '%" + ozel_kod + "%'  ";
            }
            if (statu != null)
            {
                query += " and CK.statu = " + statu;
            }
            if (carikart_tipi_id > 0)
            {
                query += " and CK.carikart_tipi_id = " + carikart_tipi_id;
            }
            #region
            string orStatement = "";
            if (carikart_id > 0)
            {
                parameterControl++;
                orStatement += "CK.carikart_id = @carikart_id AND ";
            }
            if (!string.IsNullOrEmpty(cari_unvan))
            {
                parameterControl++;
                orStatement += "CK.cari_unvan like @unvan AND ";
            }

            if (!string.IsNullOrEmpty(ozel_kod))
            {
                parameterControl++;
                orStatement += "CK.ozel_kod like @ozelkod AND ";
            }

            if (carikart_tipi_id > 0)
            {
                parameterControl++;
                orStatement += "CK.carikart_tipi_id = @carikart_tipi_id AND ";
            }
            if (statu != null)
            {
                parameterControl++;
                orStatement += "CK.statu = @statu AND ";
            }


            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
                //orStatement += " AND ";
            }

            orStatement += "";
            #endregion


            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@carikart_id",carikart_id),
                    new SqlParameter("@unvan","%"+cari_unvan+"%"),
                    new SqlParameter("@ozelkod","%"+ozel_kod+"%"),
                    new SqlParameter("@carikart_tipi_id",carikart_tipi_id),
                    new SqlParameter("@statu",statu)
            };
            #endregion

            //if (parameterControl > 0 | carikart_id > 0)
            //{
            string _v = CommandType.Text.ToString();
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                personeller = new List<Personel>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //per_kimlik = new carikart_genel_kimlik();
                    personel = new Personel();
                    personel.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    personel.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    personel.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    personel.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    //personel.kayit_yeri = dt.Rows[i]["kayit_yeri"].acekaToLong();
                    personel.statu = dt.Rows[i]["statu"].acekaToBool();
                    personel.statu_adi = dt.Rows[i]["statu_adi"].acekaToString();
                    personel.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    personel.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    personel.carikart_turu_adi = dt.Rows[i]["carikart_turu"].ToString();
                    personel.carikart_tipi_adi = dt.Rows[i]["carikart_tipi"].ToString();
                    personel.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                    personel.ozel_kod = dt.Rows[i]["ozel_kod"].ToString();
                    //personel.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    //personel.giz_yazilim_kodu = dt.Rows[i]["giz_yazilim_kodu"].acekaToShort();
                    //personel.transfer_depo_id = dt.Rows[i]["transfer_depo_id"].acekaToLong();
                    personel.giz_kullanici_adi = dt.Rows[i]["giz_kullanici_adi"].ToString();
                    personel.giz_kullanici_sifre = dt.Rows[i]["giz_kullanici_sifre"].ToString();
                    //personel.cari_parametre_1 = dt.Rows[i]["cari_parametre_1"].acekaToInt();
                    //personel.cari_parametre_2 = dt.Rows[i]["cari_parametre_2"].acekaToInt();
                    //personel.cari_parametre_3 = dt.Rows[i]["cari_parametre_3"].acekaToInt();
                    //personel.cari_parametre_4 = dt.Rows[i]["cari_parametre_4"].acekaToInt();
                    //personel.cari_parametre_5 = dt.Rows[i]["cari_parametre_5"].acekaToInt();
                    //personel.cari_parametre_6 = dt.Rows[i]["cari_parametre_6"].acekaToInt();
                    //personel.cari_parametre_7 = dt.Rows[i]["cari_parametre_7"].acekaToInt();
                    //per_kimlik.dogum_tarihi = dt.Rows[i]["dogum_tarihi"].acekaToDateTime();
                    //personel.personel_kimlik = per_kimlik;

                    personeller.Add(personel);
                    personel = null;
                    per_kimlik = null;
                }
            }
            // }
            return personeller;
        }

        public Personel Getir(long carikart_id = 0)
        {
            #region Query
            string query = @"
                            --Table[0]
                select 
	                              c.carikart_id,c.degistiren_carikart_id,c.degistiren_tarih,c.kayit_silindi,c.kayit_yeri
	                              ,c.statu,c.carikart_tipi_id,ct.carikart_tipi_adi,c.carikart_turu_id,ctu.carikart_turu_adi
	                              ,c.cari_unvan,c.ozel_kod,c.fiyattipi,c.giz_yazilim_kodu,c.ana_carikart_id,cAna.cari_unvan as ana_cari_unvan
	                              ,c.transfer_depo_id,c.giz_kullanici_adi,c.giz_kullanici_sifre,r.cari_parametre_1,r.cari_parametre_2
	                              ,r.cari_parametre_3,r.cari_parametre_4,r.cari_parametre_5,r.cari_parametre_6,r.cari_parametre_7
	                              ,c.sube_carikart_id,cSube.cari_unvan as sube_carikart_adi
	                              ,cm.muh_kod,hd.muh_kod_adi,cm.masraf_merkezi_id,mm.masraf_merkezi_adi
                            from carikart c 
									INNER JOIN carikart_rapor_parametre r On r.carikart_id = r.carikart_id
									LEFT JOIN carikart_muhasebe cm on cm.carikart_id = c.carikart_id
									LEFT JOIN muhasebe_tanim_hesapkodlari hd on hd.muh_kod = cm.muh_kod
									LEFT JOIN muhasebe_tanim_masrafmerkezleri mm on mm.masraf_merkezi_id = cm.masraf_merkezi_id
									LEFT JOIN giz_sabit_carikart_tipi ct on ct.carikart_tipi_id = c.carikart_tipi_id
									LEFT JOIN giz_sabit_carikart_turu ctu on ctu.carikart_turu_id = c.carikart_turu_id
									LEFT JOIN carikart cAna on cAna.carikart_id = c.carikart_id
									LEFT JOIN carikart cSube on cSube.carikart_id = c.sube_carikart_id
                            WHERE c.carikart_turu_id = 2 and c.carikart_id = @carikart_id 

                            --Table[1] Cari Kart Finans
                            select
                                 cm.*,hd.muh_kod_adi,mm.masraf_merkezi_adi,gs.sirket_adi
                            from carikart c 
	                              left join carikart_muhasebe cm on cm.carikart_id = c.carikart_id
	                              left join muhasebe_tanim_hesapkodlari hd on hd.muh_kod = cm.muh_kod
	                              left join muhasebe_tanim_masrafmerkezleri mm on mm.masraf_merkezi_id = cm.masraf_merkezi_id
	                              left join giz_sirket gs on gs.sirket_id = cm.sirket_id
                            WHERE c.carikart_id = @carikart_id 
";

            //            --Table[3] Cari Kart Firma Özel Satış Sorumlusu
            //            SELECT 	
            //             cFirmaOzelSatisSorumlu.carikart_id,
            //             cFirmaOzelSatisSorumlu.satis_sorumlu_carikart_id,
            //             c.cari_unvan as 'satis_sorumlu_cari_unvan'
            //            FROM carikart_firma_ozel cFirmaOzelSatisSorumlu
            //            LEFT JOIN carikart c  ON c.carikart_id = cFirmaOzelSatisSorumlu.satis_sorumlu_carikart_id
            //            WHERE cFirmaOzelSatisSorumlu.carikart_id= @carikart_id

            //";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@carikart_id",carikart_id)
            };
            #endregion

            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                personel = new Personel();
                personel.carikart_id = ds.Tables[0].Rows[0]["carikart_id"].acekaToLong();
                personel.degistiren_carikart_id = ds.Tables[0].Rows[0]["degistiren_carikart_id"].acekaToLong();
                personel.degistiren_tarih = ds.Tables[0].Rows[0]["degistiren_tarih"].acekaToDateTime();
                personel.kayit_silindi = ds.Tables[0].Rows[0]["kayit_silindi"].acekaToBool();
                personel.kayit_yeri = ds.Tables[0].Rows[0]["kayit_yeri"].acekaToLong();
                personel.statu = ds.Tables[0].Rows[0]["statu"].acekaToBool();
                personel.carikart_turu_id = ds.Tables[0].Rows[0]["carikart_turu_id"].acekaToByte();
                personel.carikart_turu_adi = ds.Tables[0].Rows[0]["carikart_turu_adi"].ToString();
                personel.carikart_tipi_id = ds.Tables[0].Rows[0]["carikart_tipi_id"].acekaToByte();
                personel.carikart_tipi_adi = ds.Tables[0].Rows[0]["carikart_tipi_adi"].ToString();
                personel.cari_unvan = ds.Tables[0].Rows[0]["cari_unvan"].ToString();
                personel.ozel_kod = ds.Tables[0].Rows[0]["ozel_kod"].ToString();
                personel.fiyattipi = ds.Tables[0].Rows[0]["fiyattipi"].ToString();
                personel.giz_yazilim_kodu = ds.Tables[0].Rows[0]["giz_yazilim_kodu"].acekaToShort();
                personel.transfer_depo_id = ds.Tables[0].Rows[0]["transfer_depo_id"].acekaToLong();
                personel.giz_kullanici_adi = ds.Tables[0].Rows[0]["giz_kullanici_adi"].ToString();
                personel.giz_kullanici_sifre = ds.Tables[0].Rows[0]["giz_kullanici_sifre"].acekaToString();
                personel.sube_carikart_id = ds.Tables[0].Rows[0]["sube_carikart_id"].acekaToLong();
                muhasebe = new carikart_muhasebe_personel();
                //Personel_Muhasebe_Kodu(ds.Tables[1], personel.muh_masraf);
                muhasebe.carikart_id_m = ds.Tables[1].Rows[0]["carikart_id"].acekaToLong();
                muhasebe.muh_kod = ds.Tables[1].Rows[0]["muh_kod"].ToString();
                /*
                muhasebe.sene = ds.Tables[1].Rows[0]["sene"].acekaToInt();
                muhasebe.sirket_adi = ds.Tables[1].Rows[0]["sirket_adi"].ToString();
                muhasebe.sirket_id = ds.Tables[1].Rows[0]["sirket_id"].acekaToInt();
                muhasebe.muh_kod = ds.Tables[1].Rows[0]["muh_kod"].ToString();
                muhasebe.muh_kod_adi = ds.Tables[1].Rows[0]["muh_kod_adi"].ToString();
                muhasebe.masraf_merkezi_adi = ds.Tables[1].Rows[0]["masraf_merkezi_adi"].ToString();
                muhasebe.masraf_merkezi_id = ds.Tables[1].Rows[0]["masraf_merkezi_id"].acekaToInt();*/
                personel.muh_masraf = muhasebe;
            }

            return personel;
        }
        private carikart_muhasebe_personel Personel_Muhasebe_Kodu(DataTable dt, carikart_muhasebe_personel personelbilgiler)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                muhasebe.carikart_id_m = dt.Rows[0]["carikart_id"].acekaToLong();
                muhasebe.sene = dt.Rows[0]["sene"].acekaToInt();
                muhasebe.sirket_adi = dt.Rows[0]["sirket_adi"].ToString();
                muhasebe.sirket_id = dt.Rows[0]["sirket_id"].acekaToInt();
                muhasebe.muh_kod = dt.Rows[0]["muh_kod"].ToString();
                muhasebe.muh_kod_adi = dt.Rows[0]["muh_kod_adi"].ToString();
                muhasebe.masraf_merkezi_adi = dt.Rows[0]["masraf_merkezi_adi"].ToString();
                muhasebe.masraf_merkezi_id = dt.Rows[0]["masraf_merkezi_id"].acekaToInt();
            }
            return muhasebe;
        }

        public carikart_genel_adres PersonelAdresBul(long carikart_id)
        {
            carikart_genel_adres adres = null;

            #region Query
            string query = @"
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
                        INNER JOIN carikart CK on CK.carikart_id = CKA.carikart_id
                        LEFT JOIN parametre_ulke U ON U.ulke_id=CKA.ulke_id
                        LEFT JOIN parametre_ulke_sehir US ON US.sehir_id=CKA.sehir_id
                        LEFT JOIN parametre_ulke_sehir_ilce USI ON USI.ilce_id=CKA.ilce_id
                        LEFT JOIN parametre_ulke_sehir_ilce_semt USIS ON USIS.semt_id=CKA.semt_id
                         WHERE CKA.kayit_silindi = 0 AND CK.carikart_turu_id = 2 AND CKA.carikart_id = @carikart_id 
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    adres = new carikart_genel_adres();
                    adres.carikart_adres_id = dt.Rows[i]["carikart_adres_id"].acekaToLong();
                    adres.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    adres.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    adres.statu = dt.Rows[i]["statu"].acekaToBool();
                    adres.adres_tipi_id = dt.Rows[i]["adres_tipi_id"].ToString();
                    adres.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    adres.adrestanim = dt.Rows[i]["adrestanim"].ToString();
                    adres.adresunvan = dt.Rows[i]["adresunvan"].ToString();
                    adres.adres = dt.Rows[i]["adres"].ToString();
                    adres.postakodu = dt.Rows[i]["postakodu"].ToString();


                    //Ülke Model
                    adres.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    adres.ulke = new parametre_ulke();
                    adres.ulke.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    adres.ulke.ulke_adi = dt.Rows[i]["ulke_adi"].ToString();

                    //Şehir Model
                    adres.sehir_id = dt.Rows[i]["sehir_id"].acekaToShort();
                    adres.ulke_sehir = new parametre_ulke_sehir();
                    adres.ulke_sehir.sehir_id = dt.Rows[i]["sehir_id"].acekaToShort();
                    adres.ulke_sehir.sehir_adi = dt.Rows[i]["sehir_adi"].ToString();

                    //İlçe Model
                    adres.ilce_id = dt.Rows[i]["ilce_id"].acekaToShort();
                    adres.ulke_sehir_ilce = new parametre_ulke_sehir_ilce();
                    adres.ulke_sehir_ilce.ilce_id = dt.Rows[i]["ilce_id"].acekaToShort();
                    adres.ulke_sehir_ilce.ilce_adi = dt.Rows[i]["ilce_adi"].ToString();

                    //Semt Model
                    adres.semt_id = dt.Rows[i]["semt_id"].acekaToShort();
                    adres.ulke_sehir_ilce_semt = new parametre_ulke_sehir_ilce_semt();
                    adres.ulke_sehir_ilce_semt.semt_id = dt.Rows[i]["semt_id"].acekaToShort();
                    adres.ulke_sehir_ilce_semt.semt_adi = dt.Rows[i]["semt_adi"].ToString();

                    adres.vergidairesi = dt.Rows[i]["vergidairesi"].ToString();
                    adres.vergino = dt.Rows[i]["vergino"].ToString();
                    adres.tel1 = dt.Rows[i]["tel1"].ToString();
                    adres.tel2 = dt.Rows[i]["tel2"].ToString();
                    adres.fax = dt.Rows[i]["fax"].ToString();
                    adres.email = dt.Rows[i]["email"].ToString();
                    adres.websitesi = dt.Rows[i]["websitesi"].ToString();
                    adres.yetkili_ad_soyad = dt.Rows[i]["yetkili_ad_soyad"].ToString();
                    adres.yetkili_tel = dt.Rows[i]["yetkili_tel"].ToString();
                    adres.faturaadresi = dt.Rows[i]["faturaadresi"].acekaToBool();

                }
            }
            return adres;
        }

        public List<carikart_personel_calisma_yerleri> Personel_Calisma_Yerleri_Getir(long carikart_id)
        {
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            if (carikart_id > 0)
            {
                parameterControl++;
                orStatement += "  CK.carikart_id = @carikart_id OR ";
            }
            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = " AND (" + orStatement.TrimEnd(new char[] { 'O', 'R', ' ' }) + ")";
            }

            orStatement += "";

            string query = @"
                            Select
                            CK.carikart_id,
							CP.stokyeri_carikart_id,CST.cari_unvan as stokyeri_carikart_adi,
							CP.degistiren_carikart_id, CP.degistiren_tarih,
							CP.gorev_id,PGG.parametre_adi as gorev_adi,
							CP.departman_id,PGD.parametre_adi as departman_adi
                            from carikart as CK    
							INNER JOIN carikart_personel_calisma_yerleri CP on CP.carikart_id = CK.carikart_id
							INNER JOIN carikart CST on CST.carikart_id = CP.stokyeri_carikart_id    
							LEFT JOIN parametre_genel PGG on PGG.parametre_id = CP.gorev_id AND PGG.parametre_grup_id = 'PRGOREV'
							LEFT JOIN parametre_genel PGD on PGD.parametre_id = CP.departman_id AND PGD.parametre_grup_id = 'PRDEPART'        
                            Where CK.statu = 1  " + orStatement + @"  AND CK.carikart_turu_id = 2 
                            ";

            #endregion

            #region Parameters
            SqlParameter[] parameters = null;
            if (parameterControl > 0)
            {
                parameters = new SqlParameter[] {

                    new SqlParameter("@carikart_id",carikart_id),
            };
            }
            #endregion

            //if (parameterControl > 0)
            //{
            string _v = CommandType.Text.ToString();
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null)
            {
                Per_calisma_yerleri = new List<carikart_personel_calisma_yerleri>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Per_calisma_yeri = new carikart_personel_calisma_yerleri();
                    Per_calisma_yeri.carikart_id = ds.Tables[0].Rows[i]["carikart_id"].acekaToLong();
                    Per_calisma_yeri.degistiren_carikart_id = ds.Tables[0].Rows[i]["degistiren_carikart_id"].acekaToLong();
                    Per_calisma_yeri.degistiren_tarih = ds.Tables[0].Rows[i]["degistiren_tarih"].acekaToDateTime();
                    Per_calisma_yeri.stokyeri_carikart_id = ds.Tables[0].Rows[i]["stokyeri_carikart_id"].acekaToLong();
                    Per_calisma_yeri.stokyeri_carikart_adi = ds.Tables[0].Rows[i]["stokyeri_carikart_adi"].ToString();
                    Per_calisma_yeri.departman_adi = ds.Tables[0].Rows[i]["departman_adi"].ToString();
                    Per_calisma_yeri.departman_id = ds.Tables[0].Rows[i]["departman_id"].acekaToInt();
                    Per_calisma_yeri.gorev_id = ds.Tables[0].Rows[i]["gorev_id"].acekaToInt();
                    Per_calisma_yeri.gorev_adi = ds.Tables[0].Rows[i]["gorev_adi"].ToString();


                    //parametreler = new List<parametre_genel>();
                    //for (int d = 0; d < ds.Tables[1].Rows.Count; d++)
                    //{
                    //    parametre = new parametre_genel();
                    //    parametre.parametre_id = ds.Tables[1].Rows[d]["parametre_id"].acekaToByte();
                    //    parametre.parametre_adi = ds.Tables[1].Rows[d]["parametre_adi"].acekaToString();
                    //    parametreler.Add(parametre);
                    //    parametre = null;
                    //}
                    //Per_calisma_yeri.departmanlar = parametreler;

                    //parametreler = new List<parametre_genel>();
                    //for (int d = 0; d < ds.Tables[2].Rows.Count; d++)
                    //{
                    //    parametre = new parametre_genel();
                    //    parametre.parametre_id = ds.Tables[2].Rows[d]["parametre_id"].acekaToByte();
                    //    parametre.parametre_adi = ds.Tables[2].Rows[d]["parametre_adi"].acekaToString();
                    //    parametreler.Add(parametre);
                    //    parametre = null;
                    //}
                    //Per_calisma_yeri.gorevler = parametreler;

                    Per_calisma_yerleri.Add(Per_calisma_yeri);
                }

            }
            //}
            return Per_calisma_yerleri;
        }

        public Personel_Parametreler Personel_parametre_getir(long carikart_id = 0)
        {
            #region Query
            string query = @"
                            --Table[0]
                           	 select 
	                              c.carikart_id,r.cari_parametre_1,r.cari_parametre_2
	                              ,r.cari_parametre_3,r.cari_parametre_4,r.cari_parametre_5,r.cari_parametre_6,r.cari_parametre_7
                                  ,PG1.tanim as cari_parametre_1_tanim
                                  ,PG2.tanim as cari_parametre_2_tanim
                                  ,PG3.tanim as cari_parametre_3_tanim
                                  ,PG4.tanim as cari_parametre_4_tanim
                                  ,PG5.tanim as cari_parametre_5_tanim
                                  ,PG6.tanim as cari_parametre_6_tanim
                                  ,PG7.tanim as cari_parametre_7_tanim
                                  ,PG1.parametre_grubu
                            from carikart c 
							INNER JOIN carikart_rapor_parametre R On R.carikart_id = c.carikart_id
                            LEFT JOIN parametre_carikart_rapor PG1 on PG1.parametre_id = R.cari_parametre_1
                            LEFT JOIN parametre_carikart_rapor PG2 on PG2.parametre_id = R.cari_parametre_2          
                            LEFT JOIN parametre_carikart_rapor PG3 on PG3.parametre_id = R.cari_parametre_3
                            LEFT JOIN parametre_carikart_rapor PG4 on PG4.parametre_id = R.cari_parametre_4  
                            LEFT JOIN parametre_carikart_rapor PG5 on PG5.parametre_id = R.cari_parametre_5
                            LEFT JOIN parametre_carikart_rapor PG6 on PG6.parametre_id = R.cari_parametre_6  
                            LEFT JOIN parametre_carikart_rapor PG7 on PG7.parametre_id = R.cari_parametre_7             
                            WHERE c.carikart_id = @carikart_id  AND c.statu=1 AND isnull(c.kayit_silindi,0) = 0 
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
                personel_parametre = new Personel_Parametreler();
                personel_parametre.carikart_id = ds.Tables[0].Rows[0]["carikart_id"].acekaToLong();
                personel_parametre.cari_parametre_1 = ds.Tables[0].Rows[0]["cari_parametre_1"].acekaToInt();
                personel_parametre.cari_parametre_2 = ds.Tables[0].Rows[0]["cari_parametre_2"].acekaToInt();
                personel_parametre.cari_parametre_3 = ds.Tables[0].Rows[0]["cari_parametre_3"].acekaToInt();
                personel_parametre.cari_parametre_4 = ds.Tables[0].Rows[0]["cari_parametre_4"].acekaToInt();
                personel_parametre.cari_parametre_5 = ds.Tables[0].Rows[0]["cari_parametre_5"].acekaToInt();
                personel_parametre.cari_parametre_6 = ds.Tables[0].Rows[0]["cari_parametre_6"].acekaToInt();
                personel_parametre.cari_parametre_7 = ds.Tables[0].Rows[0]["cari_parametre_7"].acekaToInt();
                personel_parametre.cari_parametre_1_tanim = ds.Tables[0].Rows[0]["cari_parametre_1_tanim"].ToString();
                personel_parametre.cari_parametre_2_tanim = ds.Tables[0].Rows[0]["cari_parametre_2_tanim"].ToString();
                personel_parametre.cari_parametre_3_tanim = ds.Tables[0].Rows[0]["cari_parametre_3_tanim"].ToString();
                personel_parametre.cari_parametre_4_tanim = ds.Tables[0].Rows[0]["cari_parametre_4_tanim"].ToString();
                personel_parametre.cari_parametre_5_tanim = ds.Tables[0].Rows[0]["cari_parametre_5_tanim"].ToString();
                personel_parametre.cari_parametre_6_tanim = ds.Tables[0].Rows[0]["cari_parametre_6_tanim"].ToString();
                personel_parametre.cari_parametre_7_tanim = ds.Tables[0].Rows[0]["cari_parametre_7_tanim"].ToString();

            }

            return personel_parametre;
        }

        public List<giz_sabit_carikart_tipi> CarikartTipleriniGetir()
        {
            #region Query
            string query = @"select * from giz_sabit_carikart_tipi where carikart_turu_id = 2";

            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                caritipler = new List<giz_sabit_carikart_tipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    caritip = new giz_sabit_carikart_tipi();
                    caritip.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    caritip.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    caritip.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].acekaToString();
                    caritip.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                    caritipler.Add(caritip);
                    caritip = null;
                }

            }
            return caritipler;
        }

        public List<Personel> PersonelCalismaYerleri()
        {
            #region Query
            string query = @"select carikart_id,cari_unvan from carikart where kayit_silindi = 0 and statu = 1 and carikart_turu_id = 3 --and carikart_tipi_id in(1,2,3,4,6,7)";

            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                personeller = new List<Personel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    personel = new Personel();
                    personel.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    personel.cari_unvan = dt.Rows[i]["cari_unvan"].acekaToString();
                    personeller.Add(personel);
                    personel = null;
                }

            }
            return personeller;
        }

        public List<parametre_genel> PersonelParametreleri()
        {
            #region Query
            string query = @"select * from parametre_genel where kayit_silindi = 0 and statu = 1 ";

            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametreler = new List<parametre_genel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parametre = new parametre_genel();
                    parametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToByte();
                    parametre.parametre_adi = dt.Rows[i]["parametre_adi"].acekaToString();
                    parametre.parametre_grup_id = dt.Rows[i]["parametre_grup_id"].acekaToString();
                    parametreler.Add(parametre);
                    parametre = null;
                }

            }
            return parametreler;
        }



        public List<parametre_carikart_rapor> PersonelRaporParametreleriTumunuListele()
        {
            #region Query
            string query = @"select * from parametre_carikart_rapor where isnull(kayit_silindi,0) = 0  order by parametre";

            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                personelparametreler = new List<parametre_carikart_rapor>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    personelparametre = new parametre_carikart_rapor();
                    personelparametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToByte();
                    personelparametre.tanim = dt.Rows[i]["tanim"].acekaToString();
                    personelparametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    personelparametre.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    personelparametreler.Add(personelparametre);
                    parametre = null;
                }

            }
            return personelparametreler;
        }

        public List<parametre_carikart_rapor> PersonelRaporParametreleri(int kaynak_1_parametre_id = 0)
        {
            #region Query
            string query = @"select * from parametre_carikart_rapor where isnull(kayit_silindi,0) = 0 and  kaynak_1_parametre_id = @kaynak_1_parametre_id order by parametre";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@kaynak_1_parametre_id",kaynak_1_parametre_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                personelparametreler = new List<parametre_carikart_rapor>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    personelparametre = new parametre_carikart_rapor();
                    personelparametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToByte();
                    personelparametre.tanim = dt.Rows[i]["tanim"].acekaToString();
                    personelparametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    personelparametre.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    personelparametreler.Add(personelparametre);
                    parametre = null;
                }
                return personelparametreler;
            }
            else
            {
                return null;
            }
        }
    }
}
