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
    public class ParametreRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;

        #region Ulke değişkenleri
        private parametre_ulke ulke = null;
        private List<parametre_ulke> ulkeler = null;

        #endregion

        #region Sehir değişkenleri
        private parametre_ulke_sehir sehir = null;
        private List<parametre_ulke_sehir> sehirler = null;
        #endregion

        #region İlçe değişkenleri
        private parametre_ulke_sehir_ilce ilce = null;
        private List<parametre_ulke_sehir_ilce> ilceler = null;
        #endregion

        #region Semt değişkenleri        
        private parametre_ulke_sehir_ilce_semt semt = null;
        private List<parametre_ulke_sehir_ilce_semt> semtler = null;
        private List<parametre_ulke_vergi_daireleri> vergidaireleri = null;
        private parametre_ulke_vergi_daireleri vd = null;
        #endregion

        #region CarikartTipi değişkenleri        
        private giz_sabit_carikart_tipi carikartTipi = null;
        private List<giz_sabit_carikart_tipi> carikartTipleri = null;
        #endregion

        #region CarikartTur değişkenleri        
        private giz_sabit_carikart_turu carikartTuru = null;
        private List<giz_sabit_carikart_turu> carikartTurleri = null;
        #endregion

        #region Banka değişkenleri        
        private parametre_banka banka = null;
        private List<parametre_banka> bankalar = null;

        private parametre_banka_sube bankaSube = null;
        private List<parametre_banka_sube> bankaSubeler = null;
        #endregion

        #region Birim Değişkenleri        
        private parametre_birim birim = null;
        private List<parametre_birim> birimler = null;
        #endregion

        #region Genel Değişkenleri
        private parametre_genel genel = null;
        private List<parametre_genel> geneller = null;
        private List<parametre_stokkart_rapor> parametreList;
        private parametre_stokkart_rapor stokkartapor;
        private List<parametre_sezon> sezonlar;
        private parametre_sezon sezon;
        private giz_setup parametre;
        private List<giz_setup> parametreler;
        private giz_setup_combo parametre_combo;
        private List<giz_setup_combo> parametreler_combo;

        #endregion

        #region Zorluk Grubu Değişenkenleri
        private List<parametre_zorlukgrubu> zorluklar;
        private parametre_zorlukgrubu zorluk;
        private planlama_zorlukgrubu planlamazorlukgrubu;
        private planlama_zorlukgrubu_oranlari zorlukorani;
        private List<planlama_zorlukgrubu_oranlari> zorlukoranlari;
        #endregion

        private cari_parametreler parametreadi = null;
        private List<cari_parametreler> parametreadlari = null;

        #region kalite kontrol oranları Değişenkenleri
        private List<kalite_kontrol_oranlari> kaliteler;
        private kalite_kontrol_oranlari kalite;
        #endregion
        #endregion

        public List<cari_parametreler> PersonelParametreleAdlari()
        {
            #region Query
            string query = @"
                            select 
	                            cp.parametre_grubu
	                            ,cp.parametre 
	                            ,cp.parametre_adi as label
	                            ,p.parametre_id 
	                            ,p.tanim
	                            ,p.kaynak_1_parametre_id
	                            from giz_setup_carikart_parametre cp 
	                            left join parametre_carikart_rapor p  on cp.parametre = p.parametre and cp.parametre_grubu = p.parametre_grubu
	                            where isnull(p.kayit_silindi,0) = 0  
	
	                            order by parametre_grubu , cp.parametre";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    //new SqlParameter("@parametre_grubu",parametre_grubu)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametreadlari = new List<cari_parametreler>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parametreadi = new cari_parametreler();
                    parametreadi.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToInt();
                    parametreadi.parametre = dt.Rows[i]["parametre"].acekaToInt();
                    parametreadi.label = dt.Rows[i]["label"].acekaToString();
                    parametreadi.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    parametreadi.tanim = dt.Rows[i]["tanim"].acekaToString();
                    parametreadi.kaynak1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    parametreadlari.Add(parametreadi);
                    parametreadi = null;
                }

            }
            return parametreadlari;
        }
        public List<parametre_genel> ParametreGenel()
        {
            #region Query
            string query = @"
                            select * from parametre_genel where isnull(kayit_silindi,0) = 0 ";
            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                geneller = new List<parametre_genel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    genel = new parametre_genel();
                    genel.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    genel.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    genel.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    genel.statu = dt.Rows[i]["statu"].acekaToBool();
                    genel.parametre_grup_id = dt.Rows[i]["parametre_grup_id"].acekaToString();
                    genel.parametre_kodu = dt.Rows[i]["parametre_kodu"].acekaToString();
                    genel.parametre_adi = dt.Rows[i]["parametre_adi"].acekaToString();
                    genel.deger_metin1 = dt.Rows[i]["deger_metin1"].acekaToString();
                    genel.deger_metin2 = dt.Rows[i]["deger_metin2"].acekaToString();
                    genel.deger_metin3 = dt.Rows[i]["deger_metin3"].acekaToString();
                    genel.deger_sayi1 = dt.Rows[i]["deger_sayi1"].acekaToInt();
                    genel.deger_sayi2 = dt.Rows[i]["deger_sayi2"].acekaToInt();
                    genel.deger_sayi3 = dt.Rows[i]["deger_sayi3"].acekaToInt();
                    genel.parametre_adi_dil1 = dt.Rows[i]["parametre_adi_dil1"].acekaToString();
                    genel.parametre_adi_dil2 = dt.Rows[i]["parametre_adi_dil2"].acekaToString();
                    genel.parametre_adi_dil3 = dt.Rows[i]["parametre_adi_dil3"].acekaToString();
                    genel.parametre_adi_dil4 = dt.Rows[i]["parametre_adi_dil4"].acekaToString();
                    genel.parametre_adi_dil5 = dt.Rows[i]["parametre_adi_dil5"].acekaToString();
                    geneller.Add(genel);
                    genel = null;
                }

            }
            return geneller;
        }
        public List<giz_setup> giz_setup_parametreler()
        {
            #region Query
            string query = @"
                            select  ayaradi,ayar,ayaraciklama,ayar_grubu
		                            ,case when veri_turu = 0 then 'String' 
			                              when veri_turu = 1 then 'Integer' 
			                              when veri_turu = 2 then 'Double' 
			                              when veri_turu = 3 then 'Date' 
			                              when veri_turu = 4 then 'Bool' 
		                            end as veri_turu,combo_sql  
                            from giz_setup ";
            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            parametreler = new List<giz_setup>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parametre = new giz_setup();
                    parametre.ayaradi = dt.Rows[i]["ayaradi"].acekaToString();
                    parametre.ayar = dt.Rows[i]["ayar"].acekaToString();
                    parametre.ayaraciklama = dt.Rows[i]["ayaraciklama"].acekaToString();
                    parametre.veri_turu = dt.Rows[i]["veri_turu"].acekaToString();
                    parametre.ayar_grubu = dt.Rows[i]["ayar_grubu"].acekaToString();
                    if (dt.Rows[i]["combo_sql"].acekaToString().Length > 0)
                    {
                        parametre.combo_sql = dt.Rows[i]["combo_sql"].acekaToString();
                        parametre.combo_degeri = giz_setup_parametreler_combosql(dt.Rows[i]["combo_sql"].acekaToString());
                    }

                    parametreler.Add(parametre);
                    parametre = null;
                }
            }
            return parametreler;
        }
        public List<giz_setup_combo> giz_setup_parametreler_combosql(string query)
        {
            DataTable dt2 = null;
            dt2 = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            parametreler_combo = new List<giz_setup_combo>();
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    parametre_combo = new giz_setup_combo();
                    parametre_combo.key = dt2.Rows[i][0].acekaToString();
                    parametre_combo.value = dt2.Rows[i][1].acekaToString();
                    parametreler_combo.Add(parametre_combo);
                    parametre_combo = null;
                }
            }
            return parametreler_combo;
        }

        public List<giz_setup> GenelAyarlar()
        {
            #region Query
            string query = @"
                            select * from giz_setup ";
            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                geneller = new List<parametre_genel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                }
            }
            return parametreler;
        }

        #region Ülke, Şekir, İlçe, Semt
        public List<parametre_ulke> UlkeleriGetir()
        {
            #region Query
            string query = @"
                            SELECT 
                                ulke_id,
                                ulke_adi,
                                ulke_plaka_kodu,
                                ulke_telefon_kodu,
                                ulke_adi_dil_1,
                                kayit_silindi
                            FROM parametre_ulke
                            WHERE kayit_silindi=0 AND statu=1 Order by ulke_adi
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ulkeler = new List<parametre_ulke>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ulke = new parametre_ulke();
                    ulke.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    ulke.ulke_adi = dt.Rows[i]["ulke_adi"].ToString();
                    ulke.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    ulke.ulke_plaka_kodu = dt.Rows[i]["ulke_plaka_kodu"].ToString();
                    ulke.ulke_telefon_kodu = dt.Rows[i]["ulke_telefon_kodu"].ToString();
                    ulke.ulke_adi_dil_1 = dt.Rows[i]["ulke_adi_dil_1"].ToString();
                    ulkeler.Add(ulke);
                    ulke = null;
                }

            }
            return ulkeler;
        }
        public parametre_ulke UlkeleriGetir(short ulke_id)
        {
            #region Query
            string query = @"
                            SELECT 
                                ulke_id,
                                ulke_adi,
                                ulke_plaka_kodu,
                                ulke_telefon_kodu,
                                ulke_adi_dil_1
                            FROM parametre_ulke
                            WHERE kayit_silindi=0 AND statu=1 AND ulke_id=@ulke_id
                            Order by ulke_adi
                ";
            #endregion

            #region Parameters
            SqlParameter[] paramaters = new SqlParameter[]
            {
                new SqlParameter("@ulke_id",ulke_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, paramaters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ulke = new parametre_ulke();
                ulke.ulke_id = dt.Rows[0]["ulke_id"].acekaToShort();
                ulke.ulke_adi = dt.Rows[0]["ulke_adi"].ToString();
                ulke.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                ulke.ulke_plaka_kodu = dt.Rows[0]["ulke_plaka_kodu"].ToString();
                ulke.ulke_telefon_kodu = dt.Rows[0]["ulke_telefon_kodu"].ToString();
                ulke.ulke_adi_dil_1 = dt.Rows[0]["ulke_adi_dil_1"].ToString();
                ulkeler.Add(ulke);
                ulke = null;

            }
            return ulke;
        }
        public List<parametre_ulke_sehir> SehirleriGetir(short ulke_id)
        {
            #region Query
            string query = @"
                            SELECT 
	                            sehir_id,
	                            ulke_id, 
	                            sehir_adi, 
	                            sehir_dunya_kodu, 
	                            sehir_telefon_kodu, 
	                            sehir_plaka_kodu
                            FROM parametre_ulke_sehir
                            WHERE  kayit_silindi=0 AND ulke_id=@ulke_id ORDER BY sehir_adi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@ulke_id",ulke_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                sehirler = new List<parametre_ulke_sehir>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sehir = new parametre_ulke_sehir();
                    sehir.sehir_id = dt.Rows[i]["sehir_id"].acekaToInt();
                    sehir.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    sehir.sehir_adi = dt.Rows[i]["sehir_adi"].ToString();
                    sehir.sehir_dunya_kodu = dt.Rows[i]["sehir_dunya_kodu"].ToString();
                    sehir.sehir_telefon_kodu = dt.Rows[i]["sehir_telefon_kodu"].ToString();
                    sehir.sehir_plaka_kodu = dt.Rows[i]["sehir_plaka_kodu"].ToString();
                    sehirler.Add(sehir);
                    sehir = null;
                }
            }
            return sehirler;
        }
        public List<parametre_ulke_sehir_ilce> IlceleriGetir(short sehir_id)
        {
            #region Query
            string query = @"
                            SELECT 
	                            ilce_id, 
                                ulke_id, 
                                sehir_id, 
                                ilce_adi, 
                                ups_id
                            FROM [dbo].[parametre_ulke_sehir_ilce]
                            WHERE kayit_silindi=0 AND sehir_id=@sehir_id ORDER BY sehir_id, ilce_adi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@sehir_id",sehir_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ilceler = new List<parametre_ulke_sehir_ilce>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ilce = new parametre_ulke_sehir_ilce();
                    ilce.ilce_id = dt.Rows[i]["ilce_id"].acekaToInt();
                    ilce.sehir_id = dt.Rows[i]["sehir_id"].acekaToShort();
                    ilce.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    ilce.ilce_adi = dt.Rows[i]["ilce_adi"].ToString();
                    ilce.ups_id = dt.Rows[i]["ups_id"].acekaToDouble();
                    ilceler.Add(ilce);
                    ilce = null;
                }
            }
            return ilceler;
        }
        public List<parametre_ulke_sehir_ilce_semt> SemtleriGetir(short ilce_id)
        {
            #region Query
            string query = @"
                            SELECT 
	                            semt_id,
	                            semt_adi, 
	                            posta_kodu
                            FROM parametre_ulke_sehir_ilce_semt
                            WHERE kayit_silindi = 0 AND ilce_id = @ilce_id
                            ORDER BY semt_id, semt_adi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@ilce_id",@ilce_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                semtler = new List<parametre_ulke_sehir_ilce_semt>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    semt = new parametre_ulke_sehir_ilce_semt();
                    semt.semt_id = dt.Rows[i]["semt_id"].acekaToInt();
                    semt.semt_adi = dt.Rows[i]["semt_adi"].ToString();
                    semt.posta_kodu = dt.Rows[i]["posta_kodu"].ToString();
                    semtler.Add(semt);
                    semt = null;
                }
            }
            return semtler;
        }
        public List<parametre_ulke_vergi_daireleri> UlkeVergiDaireleri()
        {
            #region Query
            string query = @"
                            SELECT 
	                            ulke_id,
                                vergi_daire_no,
                                vergi_daire_adi,
                                statu,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                kayit_silindi
                            FROM parametre_ulke_vergi_daireleri
                            WHERE kayit_silindi = 0 
                            ORDER BY vergi_daire_adi,vergi_daire_no
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                vergidaireleri = new List<parametre_ulke_vergi_daireleri>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vd = new parametre_ulke_vergi_daireleri();
                    vd.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    vd.vergi_daire_no = dt.Rows[i]["vergi_daire_no"].ToString();
                    vd.vergi_daire_adi = dt.Rows[i]["vergi_daire_adi"].ToString();
                    vd.statu = dt.Rows[i]["statu"].acekaToBool();
                    vd.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    vd.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    vergidaireleri.Add(vd);
                    vd = null;
                }
            }
            return vergidaireleri;
        }
        #endregion

        #region Cari Kart Tipi,  Cari Kart Türü, Bankalar, Birim ve Genel
        public List<giz_sabit_carikart_tipi> CariKartTipleri()
        {
            #region Query
            string query = @"
                            SELECT
	                            carikart_tipi_id, 
	                            carikart_turu_id, 
	                            carikart_tipi_adi, 
	                            aciklama
                            FROM giz_sabit_carikart_tipi 
                            ORDER BY carikart_tipi_adi
                ";
            #endregion

            #region Parameters
            //SqlParameter[] parameters = new SqlParameter[] {
            //        new SqlParameter("@carikart_turu_id",carikart_turu_id)
            //};
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartTipleri = new List<giz_sabit_carikart_tipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikartTipi = new giz_sabit_carikart_tipi();
                    carikartTipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    carikartTipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();
                    carikartTipi.aciklama = dt.Rows[i]["aciklama"].ToString();
                    carikartTipleri.Add(carikartTipi);
                    carikartTipi = null;
                }
            }
            return carikartTipleri;
        }
        public List<giz_sabit_carikart_tipi> CariKartTipleri(short carikart_turu_id)
        {
            #region Query

            string query = @"
                            SELECT
	                            carikart_tipi_id, 
	                            carikart_turu_id, 
	                            carikart_tipi_adi, 
	                            aciklama
                            FROM giz_sabit_carikart_tipi 
                            WHERE carikart_turu_id=@carikart_turu_id ORDER BY carikart_tipi_adi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@carikart_turu_id",carikart_turu_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartTipleri = new List<giz_sabit_carikart_tipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikartTipi = new giz_sabit_carikart_tipi();
                    carikartTipi.carikart_tipi_id = dt.Rows[i]["carikart_tipi_id"].acekaToByte();
                    carikartTipi.carikart_tipi_adi = dt.Rows[i]["carikart_tipi_adi"].ToString();
                    carikartTipi.aciklama = dt.Rows[i]["aciklama"].ToString();
                    carikartTipleri.Add(carikartTipi);
                    carikartTipi = null;
                }
            }
            return carikartTipleri;
        }
        public List<giz_sabit_carikart_turu> CariKartTurleri()
        {
            #region Query
            string query = @"
                            SELECT
	                            carikart_turu_id, 
	                            carikart_turu_adi, 
	                            aciklama
                            FROM giz_sabit_carikart_turu 
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                carikartTurleri = new List<giz_sabit_carikart_turu>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carikartTuru = new giz_sabit_carikart_turu();
                    carikartTuru.carikart_turu_id = dt.Rows[i]["carikart_turu_id"].acekaToByte();
                    carikartTuru.carikart_turu_adi = dt.Rows[i]["carikart_turu_adi"].ToString();
                    carikartTuru.aciklama = dt.Rows[i]["aciklama"].ToString();
                    carikartTurleri.Add(carikartTuru);
                    carikartTuru = null;
                }
            }
            return carikartTurleri;
        }
        public List<parametre_banka> BankaListesi()
        {
            #region Query
            string query = @"
                            SELECT
	                            banka_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            banka_adi, 
	                            ulke_id, 
	                            banka_eft_kodu, 
	                            banka_swift_kodu
                            FROM parametre_banka
                            WHERE kayit_silindi=0 ORDER BY banka_adi  
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                bankalar = new List<parametre_banka>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    banka = new parametre_banka();
                    banka.banka_id = dt.Rows[i]["banka_id"].acekaToShort();
                    banka.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    banka.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    banka.banka_adi = dt.Rows[i]["banka_adi"].ToString();
                    banka.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    banka.banka_eft_kodu = dt.Rows[i]["banka_eft_kodu"].ToString();
                    banka.banka_swift_kodu = dt.Rows[i]["banka_swift_kodu"].ToString();
                    bankalar.Add(banka);
                    banka = null;
                }
            }
            return bankalar;
        }
        public List<parametre_banka_sube> BankaSubeListeleri(short banka_id)
        {
            short parameterControl = 0;

            #region Query
            string andStatement = "";
            if (banka_id > 0)
            {
                parameterControl++;
                andStatement += "banka_id = @banka_id AND ";
            }

            if (!string.IsNullOrEmpty(andStatement))
            {
                andStatement = "(" + andStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            string query = @"
                            SELECT
	                            banka_sube_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            statu,
	                            banka_sube_kodu, 
	                            banka_sube_adi, 
	                            ulke_id, 
	                            sehir_id
                            FROM parametre_banka_sube
                            WHERE " + andStatement + " AND kayit_silindi=0 AND statu=1 ORDER BY banka_sube_adi";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@banka_id",banka_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                bankaSubeler = new List<Models.parametre_banka_sube>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bankaSube = new parametre_banka_sube();
                    bankaSube.banka_sube_id = dt.Rows[i]["banka_sube_id"].acekaToShort();
                    bankaSube.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    bankaSube.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    bankaSube.statu = dt.Rows[i]["statu"].acekaToBool();
                    bankaSube.banka_sube_kodu = dt.Rows[i]["banka_sube_kodu"].ToString();
                    bankaSube.banka_sube_adi = dt.Rows[i]["banka_sube_adi"].ToString();
                    bankaSube.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    bankaSube.sehir_id = dt.Rows[i]["sehir_id"].acekaToShort();
                    bankaSubeler.Add(bankaSube);
                    banka = null;
                }
            }
            return bankaSubeler;
        }
        public List<parametre_banka> Bankalar(short ulke_id)
        {
            #region Query
            string query = @"
                            SELECT
	                            banka_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            banka_adi, 
	                            ulke_id, 
	                            banka_eft_kodu, 
	                            banka_swift_kodu
                            FROM parametre_banka
                            WHERE ulke_id = ulke_id AND kayit_silindi=0 ORDER BY banka_adi  
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@ulke_id",ulke_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                bankalar = new List<parametre_banka>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    banka = new parametre_banka();
                    banka.banka_id = dt.Rows[i]["banka_id"].acekaToShort();
                    banka.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    banka.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    banka.banka_adi = dt.Rows[i]["banka_adi"].ToString();
                    banka.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    banka.banka_eft_kodu = dt.Rows[i]["banka_eft_kodu"].ToString();
                    banka.banka_swift_kodu = dt.Rows[i]["banka_swift_kodu"].ToString();
                    bankalar.Add(banka);
                    banka = null;
                }
            }
            return bankalar;
        }
        public List<parametre_banka_sube> BankaSubeler(short banka_id, Nullable<short> sehir_id)
        {
            short parameterControl = 0;

            #region Query
            string andStatement = "";
            if (banka_id > 0)
            {
                parameterControl++;
                andStatement += "banka_id = @banka_id AND ";
            }
            if (sehir_id != null && sehir_id > 0)
            {
                parameterControl++;
                andStatement += "sehir_id = @sehir_id AND ";
            }


            if (!string.IsNullOrEmpty(andStatement))
            {
                andStatement = "(" + andStatement.TrimEnd(new char[] { 'A', 'N', 'D', ' ' }) + ")";
            }

            string query = @"
                            SELECT
	                            banka_sube_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            statu,
	                            banka_sube_kodu, 
	                            banka_sube_adi, 
	                            ulke_id, 
	                            sehir_id
                            FROM parametre_banka_sube
                            WHERE " + andStatement + " AND kayit_silindi=0 AND statu=1 ORDER BY banka_sube_adi";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@banka_id",banka_id),
                    new SqlParameter("@sehir_id",sehir_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                bankaSubeler = new List<Models.parametre_banka_sube>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bankaSube = new parametre_banka_sube();
                    bankaSube.banka_sube_id = dt.Rows[i]["banka_sube_id"].acekaToShort();
                    bankaSube.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    bankaSube.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    bankaSube.statu = dt.Rows[i]["statu"].acekaToBool();
                    bankaSube.banka_sube_kodu = dt.Rows[i]["banka_sube_kodu"].ToString();
                    bankaSube.banka_sube_adi = dt.Rows[i]["banka_sube_adi"].ToString();
                    bankaSube.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    bankaSube.sehir_id = dt.Rows[i]["sehir_id"].acekaToShort();
                    bankaSubeler.Add(bankaSube);
                    banka = null;
                }
            }
            return bankaSubeler;
        }

        /// <summary>
        /// Ölçü bürümleri
        /// </summary>
        /// <returns></returns>
        public List<parametre_birim> Birimler()
        {
            #region Query
            string query = @"
                            SELECT
                                birim_id, 
                                degistiren_carikart_id, 
                                degistiren_tarih, 
                                statu, 
                                birim_adi, 
                                birim_kod, 
                                ondalik, birim_adi_dil_1, birim_adi_dil_2, birim_adi_dil_3, birim_adi_dil_4, birim_adi_dil_5
                            FROM parametre_birim
                            WHERE kayit_silindi = 0 
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                birimler = new List<parametre_birim>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    birim = new parametre_birim();
                    birim.birim_id = dt.Rows[i]["birim_id"].acekaToInt();
                    birim.birim_adi = dt.Rows[i]["birim_adi"].ToString();
                    birim.birim_kod = dt.Rows[i]["birim_kod"].ToString();
                    birim.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    birim.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    birim.statu = dt.Rows[i]["statu"].acekaToBool();
                    birim.ondalik = dt.Rows[i]["degistiren_carikart_id"].acekaToByte();
                    birimler.Add(birim);
                    birim = null;
                }
            }
            return birimler;
        }
        public parametre_birim Birimler(int birim_id)
        {
            #region Query
            string query = @"
                            SELECT
                                birim_id, 
                                degistiren_carikart_id, 
                                degistiren_tarih, 
                                statu, 
                                birim_adi, 
                                birim_kod, 
                                ondalik, birim_adi_dil_1, birim_adi_dil_2, birim_adi_dil_3, birim_adi_dil_4, birim_adi_dil_5
                            FROM parametre_birim
                            WHERE kayit_silindi = 0 AND birim_id=@birim_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@birim_id",birim_id)
            };

            #endregion
            parametre_birim prbirim = null;
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                prbirim = new parametre_birim();
                prbirim.birim_id = birim_id;
                prbirim.kayit_silindi = true;
            }
            return prbirim;
        }
        public List<parametre_cari_odeme_sekli> CariOdemeSekilleri()
        {
            List<parametre_cari_odeme_sekli> odemeSekilleri = null;

            #region Query
            string query = @"
                            SELECT
	                            cari_odeme_sekli_id, 
	                            cari_odeme_sekli
                            FROM
                            parametre_cari_odeme_sekli 
                            WHERE kayit_silindi = 0
                            ORDER BY cari_odeme_sekli
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_cari_odeme_sekli odemeSekli = null;
                odemeSekilleri = new List<parametre_cari_odeme_sekli>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    odemeSekli = new parametre_cari_odeme_sekli();
                    odemeSekli.cari_odeme_sekli_id = dt.Rows[i]["cari_odeme_sekli_id"].acekaToByte();
                    odemeSekli.cari_odeme_sekli = dt.Rows[i]["cari_odeme_sekli"].ToString();
                    odemeSekilleri.Add(odemeSekli);
                    odemeSekli = null;
                }
            }
            return odemeSekilleri;
        }
        public List<parametre_parabirimi> ParaBirimleri()
        {
            List<parametre_parabirimi> paraBirimleri = null;

            #region Query
            string query = @"
                            SELECT 
	                            pb, 
                                pb_kodu,
	                            pb_adi,
                                kayit_silindi,
                                Ulke_id,
	                            merkezbankasi_kodu, 
                                sira,
	                            kusurat_tanimi,
	                            pr_kodu,
                                degistiren_tarih,
                                degistiren_carikart_id
                            FROM parametre_parabirimi 
                            WHERE kayit_silindi = 0
                            ORDER BY sira
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_parabirimi birim = null;
                paraBirimleri = new List<parametre_parabirimi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    birim = new parametre_parabirimi();
                    birim.pb = dt.Rows[i]["pb"].acekaToByte();
                    birim.pb_adi = dt.Rows[i]["pb_adi"].ToString();
                    birim.pb_kodu = dt.Rows[i]["pb_kodu"].ToString();
                    birim.degistiren_carikart_id = Tools.PersonelId;
                    birim.degistiren_tarih = DateTime.Now;
                    birim.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    birim.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    birim.merkezbankasi_kodu = dt.Rows[i]["merkezbankasi_kodu"].ToString();
                    birim.sira = dt.Rows[i]["sira"].acekaToByte();
                    birim.kusurat_tanimi = dt.Rows[i]["kusurat_tanimi"].ToString();
                    birim.pr_kodu = dt.Rows[i]["pr_kodu"].ToString();
                    paraBirimleri.Add(birim);
                    birim = null;
                }
            }
            return paraBirimleri;
        }
        public parametre_parabirimi ParaBirimleri(byte pb)
        {

            #region Query
            string query = @"
                            SELECT 
	                            pb,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                kayit_silindi,
                                pb_kodu,
                                pb_adi,
                                ulke_id,
                                merkezbankasi_kodu,
                                sira,
                                kusurat_tanimi,
                                pr_kodu
                            FROM parametre_parabirimi 
                            WHERE kayit_silindi = 0 AND pb = @pb
                            ORDER BY sira
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@pb",pb)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            parametre_parabirimi pbirim = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    pbirim = new parametre_parabirimi();
                    pbirim.pb = dt.Rows[i]["pb"].acekaToByte();
                    pbirim.pb_kodu = dt.Rows[i]["pb_kodu"].ToString();
                    pbirim.pb_adi = dt.Rows[i]["pb_adi"].ToString();
                    pbirim.ulke_id = dt.Rows[i]["ulke_id"].acekaToShort();
                    pbirim.merkezbankasi_kodu = dt.Rows[i]["merkezbankasi_kodu"].ToString();
                    pbirim.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    pbirim.sira = dt.Rows[i]["sira"].acekaToByte();
                    pbirim.kusurat_tanimi = dt.Rows[i]["kusurat_tanimi"].ToString();
                    pbirim.pr_kodu = dt.Rows[i]["pr_kodu"].ToString();
                    pbirim.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    pbirim.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();

                }
            }
            return pbirim;
        }
        public List<parametre_fiyattipi> FiyatTipleri()
        {
            List<parametre_fiyattipi> fiyatTipleri = null;

            #region Query
            string query = @"
                            SELECT
	                            fiyattipi, 
	                            fiyattipi_adi, 
	                            fiyattipi_turu, 
	                            kdv_dahil, 
	                            kullanici_giris
                            FROM parametre_fiyattipi
                            WHERE kayit_silindi = 0
                            ORDER BY sira
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_fiyattipi fiyatTipi = null;
                fiyatTipleri = new List<parametre_fiyattipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fiyatTipi = new parametre_fiyattipi();
                    fiyatTipi.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    fiyatTipi.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
                    fiyatTipi.fiyattipi_turu = dt.Rows[i]["fiyattipi_turu"].ToString();
                    fiyatTipi.kdv_dahil = dt.Rows[i]["kdv_dahil"].acekaToBool();
                    fiyatTipi.kullanici_giris = dt.Rows[i]["kullanici_giris"].acekaToBool();
                    fiyatTipleri.Add(fiyatTipi);
                    fiyatTipi = null;
                }
            }
            return fiyatTipleri;
        }
        public List<parametre_fiyattipi> FiyatTipleri(long carikart_id)
        {
            List<parametre_fiyattipi> fiyatTipleri = null;

            #region Query
            string query = @"
                            SELECT * from parametre_fiyattipi PF 
                            INNER JOIN carikart_fiyat_tipi  CF ON CF.fiyattipi= PF.fiyattipi AND  CF.carikart_id=@carikart_id";
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
                parametre_fiyattipi fiyatTipi = null;
                fiyatTipleri = new List<parametre_fiyattipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fiyatTipi = new parametre_fiyattipi();
                    fiyatTipi.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    fiyatTipi.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
                    fiyatTipi.fiyattipi_turu = dt.Rows[i]["fiyattipi_turu"].ToString();
                    fiyatTipi.kdv_dahil = dt.Rows[i]["kdv_dahil"].acekaToBool();
                    fiyatTipi.kullanici_giris = dt.Rows[i]["kullanici_giris"].acekaToBool();
                    fiyatTipi.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                    fiyatTipi.statu = dt.Rows[i]["statu"].acekaToBool();
                    fiyatTipleri.Add(fiyatTipi);

                    fiyatTipi = null;
                }
            }
            return fiyatTipleri;
        }
        public List<parametre_fiyattipi> FiyatTiplers(long carikart_id)
        {
            List<parametre_fiyattipi> fiyatTipleri = null;

            #region Query
            string query = @"
                            SELECT * FROM parametre_fiyattipi  CF 
                            WHERE not exists(select * from carikart_fiyat_tipi x where x.fiyattipi =CF.fiyattipi and  x.carikart_id=" + carikart_id + " AND  CF.kayit_silindi = 0)  ORDER BY sira";
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
                parametre_fiyattipi fiyatTipi = null;
                fiyatTipleri = new List<parametre_fiyattipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    fiyatTipi = new parametre_fiyattipi();
                    fiyatTipi.fiyattipi = dt.Rows[i]["fiyattipi"].ToString();
                    fiyatTipi.fiyattipi_adi = dt.Rows[i]["fiyattipi_adi"].ToString();
                    fiyatTipi.fiyattipi_turu = dt.Rows[i]["fiyattipi_turu"].ToString();
                    fiyatTipi.kdv_dahil = dt.Rows[i]["kdv_dahil"].acekaToBool();
                    fiyatTipi.kullanici_giris = dt.Rows[i]["kullanici_giris"].acekaToBool();
                    fiyatTipleri.Add(fiyatTipi);

                    fiyatTipi = null;
                }
            }
            return fiyatTipleri;
        }

        /// <summary>
        /// parametre_stokkart_rapor tablosundaki parametreleri getirir
        /// </summary>
        /// <returns></returns>
        public List<parametre_stokkart_rapor> StokkartRaporParametreGetir()
        {
            #region Query
            string query = @"
                            --Table[0]
                            SELECT
                            parametre_id,tanim,parametre,kod ,parametre_grubu
							FROM parametre_stokkart_rapor  where kayit_silindi=0
							GROUP BY parametre_id,tanim,parametre,kod,parametre_grubu
                ";
            #endregion

            #region Parameters
            //SqlParameter[] parameters = new SqlParameter[] {
            //   new SqlParameter("@carikart_id",carikart_id)
            //};
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametreList = new List<parametre_stokkart_rapor>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    stokkartapor = new parametre_stokkart_rapor();
                    stokkartapor.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    stokkartapor.tanim = dt.Rows[i]["tanim"].ToString();
                    stokkartapor.kod = dt.Rows[i]["kod"].ToString();
                    stokkartapor.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    stokkartapor.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToByte();
                    parametreList.Add(stokkartapor);
                    stokkartapor = null;
                }
            }
            return parametreList;
        }

        #endregion

        public List<parametre_kdv> KDVler()
        {
            List<parametre_kdv> kdvler = null;

            #region Query
            string query = @"
                            --Table[0]
                           select * from parametre_kdv
                ";
            #endregion

            #region Parameters

            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_kdv kdv = null;
                kdvler = new List<parametre_kdv>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kdv = new parametre_kdv();
                    kdv.kod = dt.Rows[i]["kod"].acekaToByte();
                    kdv.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
                    kdv.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    kdv.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    kdv.oran = dt.Rows[i]["oran"].acekaToShort();
                    kdvler.Add(kdv);
                    kdv = null;
                }
            }
            return kdvler;
        }
        public parametre_kdv KDVler(byte kod)
        {
            #region Query
            string query = @"
                            --Table[0]
                           select * from parametre_kdv WHERE kod=@kod
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@kod",kod),
            };
            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            parametre_kdv kdv = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kdv = new parametre_kdv();
                    kdv.kod = dt.Rows[i]["kod"].acekaToByte();
                    kdv.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
                    kdv.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    kdv.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    kdv.oran = dt.Rows[i]["oran"].acekaToShort();
                }

            }
            return kdv;
        }
        // public List<talimat> TalimatListesi(int? talimatturu_id = null)
        // {
        //     List<talimat> talimatlar = null;

        //     #region Query
        //     string query = @"
        //                     --Table[0]
        //                     select * from talimat where statu = 1 and kayit_silindi = 0 and (@is_talimat_tur_id = 1 and talimatturu_id = @talimatturu_id OR @is_talimat_tur_id != 1 )
        //         ";
        //     #endregion

        //     #region Parameters

        //     SqlParameter[] parameters = new SqlParameter[] {
        //             new SqlParameter("@talimatturu_id",talimatturu_id),
        //             new SqlParameter("@is_talimat_tur_id",talimatturu_id.HasValue ? 1 : 2),
        //     };

        //     #endregion

        //     dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

        //     if (dt != null && dt.Rows.Count > 0)
        //     {
        //         talimat _talimat = null;
        //         talimatlar = new List<talimat>();
        //         for (int i = 0; i < dt.Rows.Count; i++)
        //         {
        //             _talimat = new talimat();
        //             _talimat.talimatturu_id = dt.Rows[i]["talimatturu_id"].acekaToInt();
        //             _talimat.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
        //             _talimat.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
        //             _talimat.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
        //             _talimat.statu = dt.Rows[i]["statu"].acekaToBool();
        //             _talimat.kod = dt.Rows[i]["kod"].acekaToString();
        //             _talimat.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
        //             _talimat.tanim = dt.Rows[i]["tanim"].acekaToString();
        //             _talimat.tanim_dil1 = dt.Rows[i]["tanim_dil1"].acekaToString();
        //             _talimat.tanim_dil2 = dt.Rows[i]["tanim_dil2"].acekaToString();
        //             _talimat.tanim_dil3 = dt.Rows[i]["tanim_dil3"].acekaToString();
        //             _talimat.tanim_dil4 = dt.Rows[i]["tanim_dil4"].acekaToString();
        //             _talimat.tanim_dil5 = dt.Rows[i]["tanim_dil5"].acekaToString();
        //             _talimat.sira = dt.Rows[i]["sira"].acekaToByte();
        //             _talimat.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToInt();
        //             _talimat.kesim = dt.Rows[i]["kesim"].acekaToBool();
        //             _talimat.dikim = dt.Rows[i]["dikim"].acekaToBool();
        //             _talimat.parca = dt.Rows[i]["parca"].acekaToBool();
        //             _talimat.model = dt.Rows[i]["model"].acekaToBool();
        //             _talimat.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
        //             _talimat.onayoto = dt.Rows[i]["onayoto"].acekaToBool();
        //             _talimat.parcamodel_giris = dt.Rows[i]["parcamodel_giris"].acekaToBool();
        //             _talimat.parcamodel_cikis = dt.Rows[i]["parcamodel_cikis"].acekaToBool();
        //             _talimat.model_zorunlu = dt.Rows[i]["model_zorunlu"].acekaToBool();
        //             _talimat.varsayilan_fasoncu = dt.Rows[i]["varsayilan_fasoncu"].acekaToLong();
        //             _talimat.kdv_tevkifat = dt.Rows[i]["kdv_tevkifat"].acekaToByte();
        //             talimatlar.Add(_talimat);
        //             _talimat = null;
        //         }
        //     }
        //     return talimatlar;
        // }


        public List<talimattanim> TalimatListesiTanimWithID(long siparisID, byte siraID)
        {
            List<talimattanim> talimatlar = null;

            #region query
            string query = @"Select siparis_id, sira_id, t.tanim, ck.cari_unvan,aciklama, irstalimat,islem_sayisi From siparis_talimat st
                                LEFT JOIN carikart ck ON ck.carikart_id = st.fasoncu_carikart_id
                                LEFT JOIN talimat t ON t.talimatturu_id = st.talimatturu_id";
            #endregion
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                talimattanim _talimat = null;
                talimatlar = new List<talimattanim>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    _talimat = new talimattanim();
                    _talimat.talimatturu_id = ds.Tables[0].Rows[i]["talimatturu_id"].acekaToInt();
                    _talimat.degistiren_carikart_id = ds.Tables[0].Rows[i]["degistiren_carikart_id"].acekaToLong();
                    _talimat.degistiren_tarih = ds.Tables[0].Rows[i]["degistiren_tarih"].acekaToDateTime();
                    _talimat.kayit_silindi = ds.Tables[0].Rows[i]["kayit_silindi"].acekaToBool();
                    _talimat.statu = ds.Tables[0].Rows[i]["statu"].acekaToBool();
                    _talimat.kod = ds.Tables[0].Rows[i]["kod"].acekaToString();
                    _talimat.varsayilan = ds.Tables[0].Rows[i]["varsayilan"].acekaToBool();
                    _talimat.tanim = ds.Tables[0].Rows[i]["tanim"].acekaToString();
                    _talimat.tanim_dil1 = ds.Tables[0].Rows[i]["tanim_dil1"].acekaToString();
                    _talimat.tanim_dil2 = ds.Tables[0].Rows[i]["tanim_dil2"].acekaToString();
                    _talimat.tanim_dil3 = ds.Tables[0].Rows[i]["tanim_dil3"].acekaToString();
                    _talimat.tanim_dil4 = ds.Tables[0].Rows[i]["tanim_dil4"].acekaToString();
                    _talimat.tanim_dil5 = ds.Tables[0].Rows[i]["tanim_dil5"].acekaToString();
                    _talimat.sira = ds.Tables[0].Rows[i]["sira"].acekaToByte();
                    _talimat.renk_rgb = ds.Tables[0].Rows[i]["renk_rgb"].acekaToString();
                    _talimat.kesim = ds.Tables[0].Rows[i]["kesim"].acekaToBool();
                    _talimat.dikim = ds.Tables[0].Rows[i]["dikim"].acekaToBool();
                    _talimat.parca = ds.Tables[0].Rows[i]["parca"].acekaToBool();
                    _talimat.model = ds.Tables[0].Rows[i]["model"].acekaToBool();
                    _talimat.stokkart_tipi_id = ds.Tables[0].Rows[i]["stokkart_tipi_id"].acekaToByte();
                    _talimat.onayoto = ds.Tables[0].Rows[i]["onayoto"].acekaToBool();
                    _talimat.parcamodel_giris = ds.Tables[0].Rows[i]["parcamodel_giris"].acekaToBool();
                    _talimat.parcamodel_cikis = ds.Tables[0].Rows[i]["parcamodel_cikis"].acekaToBool();
                    _talimat.model_zorunlu = ds.Tables[0].Rows[i]["model_zorunlu"].acekaToBool();
                    _talimat.varsayilan_fasoncu = ds.Tables[0].Rows[i]["varsayilan_fasoncu"].acekaToLong();
                    _talimat.cari_unvan = ds.Tables[0].Rows[i]["cari_unvan"].acekaToString();
                    _talimat.kdv_tevkifat = ds.Tables[0].Rows[i]["kdv_tevkifat"].acekaToByte();
                    talimatlar.Add(_talimat);
                    _talimat = null;
                }
            }
            return talimatlar;
        }
        public List<talimattanim> TalimatListesiTanim()
        {
            List<talimattanim> talimatlar = null;


            #region Query
            string query = @"
                            --Table[0]
                            SELECT t.*, ck.cari_unvan FROM talimat t
                            LEFT JOIN carikart ck ON ck.carikart_id = t.varsayilan_fasoncu
                            WHERE t.kayit_silindi = 0
                            --Tables[1]
                            SELECT stokkarttipi,tanim FROM giz_sabit_stokkarttipi
                ";
            #endregion
            
            
            DataSet ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                talimattanim _talimat = null;
                talimatlar = new List<talimattanim>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    _talimat = new talimattanim();
                    _talimat.talimatturu_id = ds.Tables[0].Rows[i]["talimatturu_id"].acekaToInt();
                    _talimat.degistiren_carikart_id = ds.Tables[0].Rows[i]["degistiren_carikart_id"].acekaToLong();
                    _talimat.degistiren_tarih = ds.Tables[0].Rows[i]["degistiren_tarih"].acekaToDateTime();
                    _talimat.kayit_silindi = ds.Tables[0].Rows[i]["kayit_silindi"].acekaToBool();
                    _talimat.statu = ds.Tables[0].Rows[i]["statu"].acekaToBool();
                    _talimat.kod = ds.Tables[0].Rows[i]["kod"].acekaToString();
                    _talimat.varsayilan = ds.Tables[0].Rows[i]["varsayilan"].acekaToBool();
                    _talimat.tanim = ds.Tables[0].Rows[i]["tanim"].acekaToString();
                    _talimat.tanim_dil1 = ds.Tables[0].Rows[i]["tanim_dil1"].acekaToString();
                    _talimat.tanim_dil2 = ds.Tables[0].Rows[i]["tanim_dil2"].acekaToString();
                    _talimat.tanim_dil3 = ds.Tables[0].Rows[i]["tanim_dil3"].acekaToString();
                    _talimat.tanim_dil4 = ds.Tables[0].Rows[i]["tanim_dil4"].acekaToString();
                    _talimat.tanim_dil5 = ds.Tables[0].Rows[i]["tanim_dil5"].acekaToString();
                    _talimat.sira = ds.Tables[0].Rows[i]["sira"].acekaToByte();
                    _talimat.renk_rgb = ds.Tables[0].Rows[i]["renk_rgb"].acekaToString();
                    _talimat.kesim = ds.Tables[0].Rows[i]["kesim"].acekaToBool();
                    _talimat.dikim = ds.Tables[0].Rows[i]["dikim"].acekaToBool();
                    _talimat.parca = ds.Tables[0].Rows[i]["parca"].acekaToBool();
                    _talimat.model = ds.Tables[0].Rows[i]["model"].acekaToBool();
                    _talimat.stokkart_tipi_id = ds.Tables[0].Rows[i]["stokkart_tipi_id"].acekaToByte();
                    _talimat.onayoto = ds.Tables[0].Rows[i]["onayoto"].acekaToBool();
                    _talimat.parcamodel_giris = ds.Tables[0].Rows[i]["parcamodel_giris"].acekaToBool();
                    _talimat.parcamodel_cikis = ds.Tables[0].Rows[i]["parcamodel_cikis"].acekaToBool();
                    _talimat.model_zorunlu = ds.Tables[0].Rows[i]["model_zorunlu"].acekaToBool();
                    _talimat.varsayilan_fasoncu = ds.Tables[0].Rows[i]["varsayilan_fasoncu"].acekaToLong();
                    _talimat.cari_unvan = ds.Tables[0].Rows[i]["cari_unvan"].acekaToString();
                    _talimat.kdv_tevkifat = ds.Tables[0].Rows[i]["kdv_tevkifat"].acekaToByte();
                    _talimat.stokkart_tipleri = stokkarttiplerilistesi(ds.Tables[1]);
                    talimatlar.Add(_talimat);
                    _talimat = null;
                }
            }
            return talimatlar;
        }
        public List<giz_sabit_stokkarttipi> StokkartTipleri()
        {
            List<giz_sabit_stokkarttipi> kartTipleri = null;

            #region Query
            string query = @"
                            --Table[0]
                            select * from giz_sabit_stokkarttipi  WHERE parametre_grubu >0 
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                giz_sabit_stokkarttipi kartTipi = null;
                kartTipleri = new List<giz_sabit_stokkarttipi>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kartTipi = new giz_sabit_stokkarttipi();
                    kartTipi.stokkarttipi = dt.Rows[i]["stokkarttipi"].acekaToByte();
                    kartTipi.tanim = dt.Rows[i]["tanim"].acekaToString();
                    kartTipi.otostokkodu = dt.Rows[i]["otostokkodu"].acekaToString();
                    kartTipi.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToByte();
                    kartTipi.stokkartturu = dt.Rows[i]["stokkartturu"].acekaToByte();
                    kartTipleri.Add(kartTipi);
                    kartTipi = null;
                }
            }
            return kartTipleri;
        }
        public giz_sabit_stokkarttipi StokkartTipi(byte stokkarttipi)
        {
            giz_sabit_stokkarttipi kartTipi = null;

            #region Query
            string query = @"
                            --Table[0]
                            SELECT * from giz_sabit_stokkarttipi
                            WHERE stokkarttipi = @stokkarttipi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stokkarttipi",stokkarttipi)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                kartTipi = new giz_sabit_stokkarttipi();
                kartTipi.stokkarttipi = dt.Rows[0]["stokkarttipi"].acekaToByte();
                kartTipi.tanim = dt.Rows[0]["tanim"].acekaToString();
                kartTipi.otostokkodu = dt.Rows[0]["otostokkodu"].acekaToString();
                kartTipi.parametre_grubu = dt.Rows[0]["parametre_grubu"].acekaToByte();
                kartTipi.stokkartturu = dt.Rows[0]["stokkartturu"].acekaToByte();
            }
            return kartTipi;
        }
        public giz_sabit_stokkarttipi StokkartTip(long stokkart_id)
        {
            giz_sabit_stokkarttipi kartTipi = null;

            #region Query
            string query = @"
                            --Table[0]
                            SELECT * from giz_sabit_stokkarttipi t
							INNER JOIN stokkart k on k.stokkart_tipi_id=t.stokkarttipi
                            WHERE k.stokkart_id = @stokkart_id
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
                kartTipi = new giz_sabit_stokkarttipi();
                kartTipi.stokkarttipi = dt.Rows[0]["stokkarttipi"].acekaToByte();
                kartTipi.tanim = dt.Rows[0]["tanim"].acekaToString();
                kartTipi.otostokkodu = dt.Rows[0]["otostokkodu"].acekaToString();
                kartTipi.parametre_grubu = dt.Rows[0]["parametre_grubu"].acekaToByte();
                kartTipi.stokkartturu = dt.Rows[0]["stokkartturu"].acekaToByte();
            }
            return kartTipi;
        }
        public List<giz_sabit_stokkartturu> StokkartTurleri()
        {
            List<giz_sabit_stokkartturu> kartTurleri = null;

            #region Query
            string query = @"
                            --Table[0]
                            SELECT * from giz_sabit_stokkartturu
                ";
            #endregion

            #region Parameters

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                giz_sabit_stokkartturu kartTuru = null;
                kartTurleri = new List<giz_sabit_stokkartturu>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kartTuru = new giz_sabit_stokkartturu();
                    kartTuru.stokkartturu = dt.Rows[i]["stokkartturu"].acekaToByte();
                    kartTuru.tanim = dt.Rows[i]["tanim"].acekaToString();
                    kartTurleri.Add(kartTuru);
                    kartTuru = null;
                }
            }
            return kartTurleri;
        }
        public List<parametre_renk> RenkListesi(string renk_adi = "")
        {
            List<parametre_renk> renkler = null;

            #region Query
            string query = @"
                            WITH CTE AS
                            (   SELECT  
	                            renk_id,
	                            renk_kodu,
	                            renk_adi,
	                            renk_rgb,
	                            kayit_silindi,
                                RowNum = ROW_NUMBER() OVER(PARTITION BY renk_adi ORDER BY renk_adi DESC)
                                FROM   parametre_renk 
                            )
                            SELECT  
	                            renk_id,
	                            renk_kodu,
	                            renk_adi,
	                            renk_rgb
                            FROM    CTE
                            WHERE renk_adi like @renk_adi AND RowNum = 1 AND kayit_silindi = 0 AND renk_adi is not null AND renk_adi NOT in('')
                            Order by renk_adi;
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@renk_adi","%"+renk_adi+"%")
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                renkler = new List<parametre_renk>();
                parametre_renk renk = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    renk = new parametre_renk();
                    renk.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    renk.renk_kodu = dt.Rows[i]["renk_kodu"].acekaToString();
                    renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();
                    renk.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToIntWithNullable();
                    renkler.Add(renk);
                    renk = null;
                }
            }

            return renkler;
        }
        public List<parametre_renk> KumasRenk(long stokkart_id)
        {
            List<parametre_renk> renkler = null;

            #region Query
            string query = @"
                    SELECT
	                   ss.renk_id, pr.renk_adi
                    FROM stokkart_sku as ss
                    LEFT JOIN parametre_renk as pr ON ss.renk_id = pr.renk_id
                    WHERE stokkart_id = @stokkart_id 
                    Order by ss.sku_id;
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
                renkler = new List<parametre_renk>();
                parametre_renk renk = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    renk = new parametre_renk();
                    renk.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();
                    renkler.Add(renk);
                    renk = null;
                }
            }

            return renkler;
        }
        public List<parametre_renk> RenkListesi(long renk_id)
        {
            List<parametre_renk> renkler = null;

            #region Query
            string query = @"SELECT 
                                renk_id,
                                renk_kodu,
                                renk_adi,
                                renk_rgb,
                                renk_kodu2,
                                siparis_ozel,
                                stokkart_tipi_id,
                                stokalan_id_1,
                                renk_kodu_dil_1,
                                renk_adi_dil_1,
                                renk_kodu_dil_2,
                                renk_adi_dil_2,
                                renk_kodu_dil_3,
                                renk_adi_dil_3,
                                renk_kodu_dil_4,
                                renk_adi_dil_4,
                                renk_kodu_dil_5,
                                renk_adi_dil_5,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                kayit_silindi
                            FROM  parametre_renk WHERE renk_id = @renk_id
                            Order by renk_adi;
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@renk_id",renk_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                renkler = new List<parametre_renk>();
                parametre_renk renk = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    renk = new parametre_renk();
                    renk.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    renk.renk_kodu = dt.Rows[i]["renk_kodu"].acekaToString();
                    renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();
                    renk.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToIntWithNullable();
                    renk.renk_kodu2 = dt.Rows[i]["renk_kodu2"].acekaToString();
                    renk.siparis_ozel = dt.Rows[i]["siparis_ozel"].acekaToBool();
                    renk.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
                    renk.stokalan_id_1 = dt.Rows[i]["stokalan_id_1"].acekaToInt();
                    renk.renk_kodu_dil_1 = dt.Rows[i]["renk_kodu_dil_1"].acekaToString();
                    renk.renk_adi_dil_1 = dt.Rows[i]["renk_adi_dil_1"].acekaToString();
                    renk.renk_kodu_dil_2 = dt.Rows[i]["renk_kodu_dil_2"].acekaToString();
                    renk.renk_adi_dil_2 = dt.Rows[i]["renk_adi_dil_2"].acekaToString();
                    renk.renk_kodu_dil_3 = dt.Rows[i]["renk_kodu_dil_3"].acekaToString();
                    renk.renk_adi_dil_3 = dt.Rows[i]["renk_adi_dil_3"].acekaToString();
                    renk.renk_kodu_dil_4 = dt.Rows[i]["renk_kodu_dil_4"].acekaToString();
                    renk.renk_adi_dil_4 = dt.Rows[i]["renk_adi_dil_4"].acekaToString();
                    renk.renk_kodu_dil_5 = dt.Rows[i]["renk_kodu_dil_5"].acekaToString();
                    renk.renk_adi_dil_5 = dt.Rows[i]["renk_adi_dil_5"].acekaToString();
                    renk.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    renk.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    renk.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    renkler.Add(renk);
                    renk = null;
                }

            }

            return renkler;
        }
        public List<parametre_renk> RenkListesi()
        {
            List<parametre_renk> renkler = null;

            #region Query
            string query = @"SELECT 
                                renk_id,
                                renk_kodu,
                                renk_adi,
                                renk_rgb,
                                renk_kodu2,
                                siparis_ozel,
                                stokkart_tipi_id,
                                stokalan_id_1,
                                renk_kodu_dil_1,
                                renk_adi_dil_1,
                                renk_kodu_dil_2,
                                renk_adi_dil_2,
                                renk_kodu_dil_3,
                                renk_adi_dil_3,
                                renk_kodu_dil_4,
                                renk_adi_dil_4,
                                renk_kodu_dil_5,
                                renk_adi_dil_5,
                                degistiren_carikart_id,
                                degistiren_tarih,
                                kayit_silindi
                            FROM  parametre_renk
                            Order by renk_adi;
                ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                renkler = new List<parametre_renk>();
                parametre_renk renk = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    renk = new parametre_renk();
                    renk.renk_id = dt.Rows[i]["renk_id"].acekaToLong();
                    renk.renk_kodu = dt.Rows[i]["renk_kodu"].acekaToString();
                    renk.renk_adi = dt.Rows[i]["renk_adi"].acekaToString();
                    renk.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToIntWithNullable();
                    renk.renk_kodu2 = dt.Rows[i]["renk_kodu2"].acekaToString();
                    renk.siparis_ozel = dt.Rows[i]["siparis_ozel"].acekaToBool();
                    renk.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
                    renk.stokalan_id_1 = dt.Rows[i]["stokalan_id_1"].acekaToInt();
                    renk.renk_kodu_dil_1 = dt.Rows[i]["renk_kodu_dil_1"].acekaToString();
                    renk.renk_adi_dil_1 = dt.Rows[i]["renk_adi_dil_1"].acekaToString();
                    renk.renk_kodu_dil_2 = dt.Rows[i]["renk_kodu_dil_2"].acekaToString();
                    renk.renk_adi_dil_2 = dt.Rows[i]["renk_adi_dil_2"].acekaToString();
                    renk.renk_kodu_dil_3 = dt.Rows[i]["renk_kodu_dil_3"].acekaToString();
                    renk.renk_adi_dil_3 = dt.Rows[i]["renk_adi_dil_3"].acekaToString();
                    renk.renk_kodu_dil_4 = dt.Rows[i]["renk_kodu_dil_4"].acekaToString();
                    renk.renk_adi_dil_4 = dt.Rows[i]["renk_adi_dil_4"].acekaToString();
                    renk.renk_kodu_dil_5 = dt.Rows[i]["renk_kodu_dil_5"].acekaToString();
                    renk.renk_adi_dil_5 = dt.Rows[i]["renk_adi_dil_5"].acekaToString();
                    renk.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    renk.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    renk.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    renkler.Add(renk);
                    renk = null;
                }

            }

            return renkler;
        }
        public List<parametre_sezon> Sezonlistesi()
        {
            #region Query
            string query = @"
                            SELECT
	                            sezon_id,sezon_adi,sezon_kodu,kayit_silindi,
                                degistiren_carikart_id,degistiren_tarih 
                            FROM parametre_sezon
                            ORDER BY sezon_id
                ";
            #endregion

            #region Parameters
            //SqlParameter[] parameters = new SqlParameter[] {
            //        new SqlParameter("@carikart_turu_id",carikart_turu_id)
            //};
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                sezonlar = new List<parametre_sezon>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sezon = new parametre_sezon();
                    sezon.sezon_id = dt.Rows[i]["sezon_id"].acekaToShort();
                    sezon.sezon_adi = dt.Rows[i]["sezon_adi"].ToString();
                    sezon.sezon_kodu = dt.Rows[i]["sezon_kodu"].ToString();
                    sezon.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    sezon.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    sezon.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    sezonlar.Add(sezon);
                    sezon = null;
                }
            }
            return sezonlar;
        }
        public List<parametre_stokkart_rapor> StokkartRaporParametreleri(long stokkart_id, byte parametre)
        {

            List<parametre_stokkart_rapor> raporParametreleri = null;

            #region Query
            string query = @"
                            SELECT DISTINCT
	                            SKR.parametre_id,
	                            SKP.parametre_adi,
	                            SKR.kod,
	                            SKR.tanim
                            FROM stokkart S 
                            INNER JOIN giz_sabit_stokkarttipi SKT ON SKT.stokkarttipi = S.stokkart_tipi_id
                            INNER JOIN parametre_stokkart_rapor SKR  ON SKR.parametre_grubu = SKT.parametre_grubu
                            INNER JOIN giz_setup_stokkart_parametre SKP  ON SKP.parametre = SKR.parametre
                            WHERE S.stokkart_id = @stokkart_id AND SKR.parametre = @parametre AND SKP.parametre_grubu = SKT.parametre_grubu
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stokkart_id",stokkart_id),
                    new SqlParameter("@parametre",parametre)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                raporParametreleri = new List<parametre_stokkart_rapor>();
                parametre_stokkart_rapor raporParametre = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    raporParametre = new parametre_stokkart_rapor();
                    raporParametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    raporParametre.parametre_adi = dt.Rows[i]["parametre_adi"].ToString();
                    raporParametre.kod = dt.Rows[i]["kod"].ToString();
                    raporParametre.tanim = dt.Rows[i]["tanim"].ToString();
                    raporParametreleri.Add(raporParametre);
                    raporParametre = null;
                }
            }
            return raporParametreleri;
        }
        public List<parametre_stokkart_rapor> StokkartRaporParametreleri(byte parametregrubu)
        {

            List<parametre_stokkart_rapor> raporParametreleri = null;

            #region Query
            string query = @"
                            Select
                                PSR.parametre_id,
                                PSR.parametre_grubu,
                                PSR.parametre,
                                PSR.tanim,
                                SKP.parametre_adi, 
                                PSR.kod
                                from parametre_stokkart_rapor PSR
                                INNER JOIN giz_setup_stokkart_parametre SKP ON PSR.parametre_grubu=SKP.parametre_grubu AND SKP.parametre = PSR.parametre
                                Where PSR.parametre_grubu = @parametregrubu

                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@parametregrubu",parametregrubu)
                    //,new SqlParameter("@parametre",parametre)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                raporParametreleri = new List<parametre_stokkart_rapor>();
                parametre_stokkart_rapor raporParametre = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    raporParametre = new parametre_stokkart_rapor();
                    raporParametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    raporParametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    raporParametre.parametre_adi = dt.Rows[i]["parametre_adi"].ToString();
                    raporParametre.kod = dt.Rows[i]["kod"].ToString();
                    raporParametre.tanim = dt.Rows[i]["tanim"].ToString();
                    raporParametre.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToByte();
                    raporParametreleri.Add(raporParametre);
                    raporParametre = null;
                }
            }
            return raporParametreleri;
        }
        public List<parametre_stokkart_rapor> StokkartRaporParametreleri()
        {

            List<parametre_stokkart_rapor> raporParametreleri = null;

            #region Query
            string query = @"
                            SELECT *
                            FROM parametre_stokkart_rapor 
                ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                raporParametreleri = new List<parametre_stokkart_rapor>();
                parametre_stokkart_rapor raporParametre = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    raporParametre = new parametre_stokkart_rapor();
                    raporParametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    raporParametre.degistiren_tarih = DateTime.Now;
                    raporParametre.degistiren_carikart_id = Tools.PersonelId;
                    raporParametre.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    raporParametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    raporParametre.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToByte();
                    raporParametre.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    raporParametre.kaynak_2_parametre_id = dt.Rows[i]["kaynak_2_parametre_id"].acekaToInt();
                    raporParametre.kaynak_3_parametre_id = dt.Rows[i]["kaynak_3_parametre_id"].acekaToInt();
                    raporParametre.kaynak_4_parametre_id = dt.Rows[i]["kaynak_4_parametre_id"].acekaToInt();
                    raporParametre.kod = dt.Rows[i]["kod"].ToString();
                    raporParametre.tanim = dt.Rows[i]["tanim"].ToString();
                    raporParametre.dil_1_tanim = dt.Rows[i]["dil_1_tanim"].ToString();
                    raporParametre.dil_2_tanim = dt.Rows[i]["dil_2_tanim"].ToString();
                    raporParametre.dil_3_tanim = dt.Rows[i]["kaynak_2_parametre_id"].ToString();
                    raporParametre.dil_4_tanim = dt.Rows[i]["dil_4_tanim"].ToString();
                    raporParametre.dil_5_tanim = dt.Rows[i]["dil_5_tanim"].ToString();
                    raporParametre.sira = dt.Rows[i]["sira"].acekaToInt();
                    raporParametre.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToInt();
                    raporParametre.kod1 = dt.Rows[i]["kod1"].ToString();
                    raporParametre.kod2 = dt.Rows[i]["kod2"].ToString();
                    raporParametre.kod3 = dt.Rows[i]["kod3"].ToString();
                    raporParametre.kod4 = dt.Rows[i]["kod4"].ToString();
                    raporParametre.kod5 = dt.Rows[i]["kod5"].ToString();
                    raporParametre.kod6 = dt.Rows[i]["kod6"].ToString();
                    raporParametre.deger1 = dt.Rows[i]["deger1"].acekaToDouble();
                    raporParametre.deger2 = dt.Rows[i]["deger2"].acekaToDouble();
                    raporParametreleri.Add(raporParametre);
                    raporParametre = null;
                }
            }
            return raporParametreleri;
        }
        public List<parametre_stokkart_rapor> StokkartRaporParametreleri(byte parametregrubu, byte parametre)
        {
            List<parametre_stokkart_rapor> raporParametreleri = null;

            #region Query
            string query = @"
                            SELECT *
                            FROM parametre_stokkart_rapor WHERE parametre=@parametre AND parametre_grubu =@parametre_grubu order by kod1
                ";
            #endregion
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@parametre",parametre),
                new SqlParameter("@parametre_grubu",parametregrubu)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                raporParametreleri = new List<parametre_stokkart_rapor>();
                parametre_stokkart_rapor raporParametre = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    raporParametre = new parametre_stokkart_rapor();
                    raporParametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToInt();
                    raporParametre.degistiren_tarih = DateTime.Now;
                    raporParametre.degistiren_carikart_id = Tools.PersonelId;
                    raporParametre.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    raporParametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    raporParametre.parametre_grubu = dt.Rows[i]["parametre_grubu"].acekaToByte();
                    raporParametre.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    raporParametre.kaynak_2_parametre_id = dt.Rows[i]["kaynak_2_parametre_id"].acekaToInt();
                    raporParametre.kaynak_3_parametre_id = dt.Rows[i]["kaynak_3_parametre_id"].acekaToInt();
                    raporParametre.kaynak_4_parametre_id = dt.Rows[i]["kaynak_4_parametre_id"].acekaToInt();
                    raporParametre.kod = dt.Rows[i]["kod"].ToString();
                    raporParametre.tanim = dt.Rows[i]["tanim"].ToString();
                    raporParametre.dil_1_tanim = dt.Rows[i]["dil_1_tanim"].ToString();
                    raporParametre.dil_2_tanim = dt.Rows[i]["dil_2_tanim"].ToString();
                    raporParametre.dil_3_tanim = dt.Rows[i]["kaynak_2_parametre_id"].ToString();
                    raporParametre.dil_4_tanim = dt.Rows[i]["dil_4_tanim"].ToString();
                    raporParametre.dil_5_tanim = dt.Rows[i]["dil_5_tanim"].ToString();
                    raporParametre.sira = dt.Rows[i]["sira"].acekaToInt();
                    raporParametre.renk_rgb = dt.Rows[i]["renk_rgb"].acekaToInt();
                    raporParametre.kod1 = dt.Rows[i]["kod1"].ToString();
                    raporParametre.kod2 = dt.Rows[i]["kod2"].ToString();
                    raporParametre.kod3 = dt.Rows[i]["kod3"].ToString();
                    raporParametre.kod4 = dt.Rows[i]["kod4"].ToString();
                    raporParametre.kod5 = dt.Rows[i]["kod5"].ToString();
                    raporParametre.kod6 = dt.Rows[i]["kod6"].ToString();
                    raporParametre.deger1 = dt.Rows[i]["deger1"].acekaToDouble();
                    raporParametre.deger2 = dt.Rows[i]["deger2"].acekaToDouble();
                    raporParametreleri.Add(raporParametre);
                    raporParametre = null;
                }
            }
            return raporParametreleri;
        }
        public parametre_stokkart_rapor StokkartRaporParametreleri(int parametre_id)
        {
            #region Query
            string query = @"
                            SELECT *
                            FROM parametre_stokkart_rapor WHERE parametre_id=@parametre_id
                ";
            #endregion
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("parametre_id",parametre_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            parametre_stokkart_rapor raporParametre = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                raporParametre = new parametre_stokkart_rapor();
                raporParametre.parametre_id = dt.Rows[0]["parametre_id"].acekaToInt();
                raporParametre.degistiren_tarih = DateTime.Now;
                raporParametre.degistiren_carikart_id = Tools.PersonelId;
                raporParametre.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                raporParametre.parametre = dt.Rows[0]["parametre"].acekaToByte();
                raporParametre.parametre_grubu = dt.Rows[0]["parametre_grubu"].acekaToByte();
                raporParametre.kaynak_1_parametre_id = dt.Rows[0]["kaynak_1_parametre_id"].acekaToInt();
                raporParametre.kaynak_2_parametre_id = dt.Rows[0]["kaynak_2_parametre_id"].acekaToInt();
                raporParametre.kaynak_3_parametre_id = dt.Rows[0]["kaynak_3_parametre_id"].acekaToInt();
                raporParametre.kaynak_4_parametre_id = dt.Rows[0]["kaynak_4_parametre_id"].acekaToInt();
                raporParametre.kod = dt.Rows[0]["kod"].ToString();
                raporParametre.tanim = dt.Rows[0]["tanim"].ToString();
                raporParametre.dil_1_tanim = dt.Rows[0]["dil_1_tanim"].ToString();
                raporParametre.dil_2_tanim = dt.Rows[0]["dil_2_tanim"].ToString();
                raporParametre.dil_3_tanim = dt.Rows[0]["kaynak_2_parametre_id"].ToString();
                raporParametre.dil_4_tanim = dt.Rows[0]["dil_4_tanim"].ToString();
                raporParametre.dil_5_tanim = dt.Rows[0]["dil_5_tanim"].ToString();
                raporParametre.sira = dt.Rows[0]["sira"].acekaToInt();
                raporParametre.renk_rgb = dt.Rows[0]["renk_rgb"].acekaToInt();
                raporParametre.kod1 = dt.Rows[0]["kod1"].ToString();
                raporParametre.kod2 = dt.Rows[0]["kod2"].ToString();
                raporParametre.kod3 = dt.Rows[0]["kod3"].ToString();
                raporParametre.kod4 = dt.Rows[0]["kod4"].ToString();
                raporParametre.kod5 = dt.Rows[0]["kod5"].ToString();
                raporParametre.kod6 = dt.Rows[0]["kod6"].ToString();
                raporParametre.deger1 = dt.Rows[0]["deger1"].acekaToDouble();
                raporParametre.deger2 = dt.Rows[0]["deger2"].acekaToDouble();
            }
            return raporParametre;
        }
        public List<giz_setup_ekturu> Dosyaturleri()
        {
            List<giz_setup_ekturu> turler = null;

            #region Query
            string query = @"
                            --Table[0]
                            SELECT ekturu_id,tanim,file_ext,file_types,preview 
                            FROM giz_setup_ekturu WHERE stokkart_tipi_id=1
                ";
            #endregion

            #region Parameters

            #endregion


            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                giz_setup_ekturu tur = null;
                turler = new List<giz_setup_ekturu>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tur = new giz_setup_ekturu();
                    tur.ekturu_id = dt.Rows[i]["ekturu_id"].acekaToShort();
                    tur.file_ext = dt.Rows[i]["file_ext"].ToString();
                    tur.file_types = dt.Rows[i]["file_types"].ToString();
                    tur.preview = dt.Rows[i]["preview"].acekaToBool();
                    tur.tanim = dt.Rows[i]["tanim"].ToString();
                    turler.Add(tur);
                    tur = null;
                }
            }
            return turler;
        }
        public List<ekler> EkleriGetir(int[] ek_id)
        {
            List<Models.ekler> ekler = null;

            #region Parameters

            string inPrams = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < ek_id.Length; i++)
            {
                inPrams += "@param" + i.ToString() + ",";
                parameters.Add(new SqlParameter("@param" + i.ToString(), ek_id[i]));
            }

            inPrams = inPrams.Trim(',');

            #endregion

            #region Query
            string query = @"SELECT * FROM ekler WHERE ek_id in ({0})";
            query = string.Format(query, inPrams);
            #endregion



            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters.ToArray()).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                ekler = new List<Models.ekler>();
                ekler ek = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ek = new Models.ekler();
                    ek.ek_id = dt.Rows[i]["ek_id"].acekaToInt();
                    ek.ekturu_id = dt.Rows[i]["ekturu_id"].acekaToShort();
                    ek.ekadi = dt.Rows[i]["ekadi"].acekaToString();
                    ek.filepath = dt.Rows[i]["filepath"].acekaToString();
                    ek.filename = dt.Rows[i]["filename"].acekaToString();
                    ek.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                    ekler.Add(ek);
                    ek = null;
                }

            }
            return ekler;
        }
        public List<parametre_zorlukgrubu> ZorlukGrubuGetir()
        {
            #region Query
            string query = @"
                            --Table[0]
                            SELECT * FROM parametre_zorlukgrubu
                ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                zorluklar = new List<parametre_zorlukgrubu>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    zorluk = new parametre_zorlukgrubu();
                    zorluk.zorlukgrubu_id = dt.Rows[i]["zorlukgrubu_id"].acekaToInt();
                    zorluk.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    zorluk.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    zorluk.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    zorluk.tanim = dt.Rows[i]["tanim"].acekaToString();
                    zorluk.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                    zorluk.sira = dt.Rows[i]["sira"].acekaToInt();
                    zorluklar.Add(zorluk);
                    zorluk = null;
                }
            }
            return zorluklar;
        }
        public parametre_zorlukgrubu ZorlukGrubuGetir(int zorlukgrubuid)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select * from parametre_zorlukgrubu where zorlukgrubu_id = @zorlukgrubu_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@zorlukgrubu_id",zorlukgrubuid)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                zorluk = new parametre_zorlukgrubu();
                zorluk.zorlukgrubu_id = dt.Rows[0]["zorlukgrubu_id"].acekaToByte();
                zorluk.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                zorluk.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                zorluk.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                zorluk.tanim = dt.Rows[0]["tanim"].ToString();
                zorluk.varsayilan = dt.Rows[0]["varsayilan"].acekaToBool();
                zorluk.sira = dt.Rows[0]["sira"].acekaToInt();
            }
            return zorluk;
        }
        public planlama_zorlukgrubu PlanlamaZorlukGrubuGetir(int zorlukgrubuid)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select * from planlama_zorlukgrubu where zorlukgrubu_id = @zorlukgrubu_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@zorlukgrubu_id",zorlukgrubuid)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                planlamazorlukgrubu = new planlama_zorlukgrubu();
                planlamazorlukgrubu.zorlukgrubu_id = dt.Rows[0]["zorlukgrubu_id"].acekaToByte();
                planlamazorlukgrubu.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                planlamazorlukgrubu.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                planlamazorlukgrubu.kesimfire = dt.Rows[0]["kesimfire"].acekaToShort();
                planlamazorlukgrubu.kesimfazla = dt.Rows[0]["kesimfazla"].acekaToShort();
                planlamazorlukgrubu.musterifazla = dt.Rows[0]["musterifazla"].acekaToShort();
                planlamazorlukgrubu.bedenbazinda = dt.Rows[0]["bedenbazinda"].acekaToByte();
            }
            return planlamazorlukgrubu;
        }
        public List<planlama_zorlukgrubu_oranlari> PlanlamaZorlukGrubuOranlariGetir(int zorlukgrubuid)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select * from planlama_zorlukgrubu_oranlari where zorlukgrubu_id = @zorlukgrubu_id
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@zorlukgrubu_id",zorlukgrubuid)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                zorlukoranlari = new List<planlama_zorlukgrubu_oranlari>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    zorlukorani = new planlama_zorlukgrubu_oranlari();
                    zorlukorani.zorlukgrubu_id = dt.Rows[i]["zorlukgrubu_id"].acekaToByte();
                    zorlukorani.sira = dt.Rows[i]["sira"].acekaToShort();
                    zorlukorani.altseviye = dt.Rows[i]["altseviye"].acekaToShort();
                    zorlukorani.ustseviye = dt.Rows[i]["ustseviye"].acekaToShort();
                    zorlukorani.kesimfire = dt.Rows[i]["kesimfire"].acekaToShort();
                    zorlukorani.kesimfazla = dt.Rows[i]["kesimfazla"].acekaToShort();
                    zorlukoranlari.Add(zorlukorani);
                    zorlukorani = null;
                }
            }
            return zorlukoranlari;
        }
        public List<kalite_kontrol_oranlari> KaliteKontrolOranlariListeGetir()
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select kalite_kontrol_kod from kalite_kontrol_oranlari group by kalite_kontrol_kod
                ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                kaliteler = new List<kalite_kontrol_oranlari>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kalite = new kalite_kontrol_oranlari();
                    kalite.kalite_kontrol_kod = dt.Rows[i]["kalite_kontrol_kod"].acekaToString();
                    kaliteler.Add(kalite);
                    kalite = null;
                }
            }
            return kaliteler;
        }
        public List<kalite_kontrol_oranlari> KaliteKontrolOranlariGetir(string kod)
        {
            #region Query
            string query = @"
                            --Table[0]
                            Select * from kalite_kontrol_oranlari where kalite_kontrol_kod = @kalite_kontrol_kod
                            --Tables[1]
                            select stokkarttipi,tanim from giz_sabit_stokkarttipi
                ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                 new SqlParameter("@kalite_kontrol_kod", kod)
                };
            #endregion

            DataSet ds = new DataSet();
            //dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            ds = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                kaliteler = new List<kalite_kontrol_oranlari>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    kalite = new kalite_kontrol_oranlari();
                    kalite.kalite_kontrol_kod = ds.Tables[0].Rows[i]["kalite_kontrol_kod"].acekaToString();
                    kalite.stokkart_tipi_id = ds.Tables[0].Rows[i]["stokkart_tipi_id"].acekaToByte();
                    kalite.adet = ds.Tables[0].Rows[i]["adet"].acekaToDouble();
                    kalite.sira_id = ds.Tables[0].Rows[i]["sira_id"].acekaToInt();
                    kalite.kontrol_miktar = ds.Tables[0].Rows[i]["kontrol_miktar"].acekaToDouble();
                    kalite.red_miktar = ds.Tables[0].Rows[i]["red_miktar"].acekaToDouble();
                    kalite.miktarlar_oran_mi = ds.Tables[0].Rows[i]["miktarlar_oran_mi"].acekaToBool();

                    kalite.stokkart_tipleri = stokkarttiplerilistesi(ds.Tables[1]);


                    kaliteler.Add(kalite);
                    kalite = null;
                }
            }
            return kaliteler;
        }
        private List<giz_sabit_stokkarttipi> stokkarttiplerilistesi(DataTable dt)
        {
            List<giz_sabit_stokkarttipi> tipler = new List<giz_sabit_stokkarttipi>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                giz_sabit_stokkarttipi tip = new giz_sabit_stokkarttipi();
                tip.stokkarttipi = dt.Rows[i]["stokkarttipi"].acekaToByte();
                tip.tanim = dt.Rows[i]["tanim"].acekaToString();
                tipler.Add(tip);
                tip = null;
            }
            return tipler;
        }
        public List<parametre_beden> BedenleriGetir(string[] bedengrubu)
        {
            List<parametre_beden> bedenler = null;

            #region Query
            string query = @"
                            Select beden_id,bedengrubu,beden,beden_tanimi,sira from parametre_beden
                            Where ISNULL(kayit_silindi,0) = 0 AND bedengrubu IN(replaceBedenGrubu)
                            ORDER BY bedengrubu,beden,sira
                ";
            #endregion

            #region Parameters 
            List<SqlParameter> parameters = new List<SqlParameter>();

            string bendenGruplari = "";

            for (int i = 0; i < bedengrubu.Length; i++)
            {
                parameters.Add(new SqlParameter("@grup" + i.ToString(), bedengrubu[i]));
                bendenGruplari += "@grup" + i.ToString() + ",";
            }

            bendenGruplari = bendenGruplari.TrimEnd(',');

            query = query.Replace("replaceBedenGrubu", bendenGruplari);

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters.ToArray()).Tables[0];

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
                    beden.beden_tanimi = dt.Rows[i]["beden_tanimi"].acekaToString();
                    beden.sira = dt.Rows[i]["sira"].acekaToInt();
                    bedenler.Add(beden);
                    beden = null;
                }
            }
            return bedenler;
        }
        public List<parametre_beden> BedenleriGetir()
        {
            List<parametre_beden> bedenler = null;
            #region Query
            string query = @"
                            Select beden_id,bedengrubu,beden,beden_tanimi,sira from parametre_beden
                            Where ISNULL(kayit_silindi,0) = 0 
                            ORDER BY bedengrubu,beden,sira
                ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
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
                    beden.beden_tanimi = dt.Rows[i]["beden_tanimi"].acekaToString();
                    beden.sira = dt.Rows[i]["sira"].acekaToInt();
                    bedenler.Add(beden);
                    beden = null;
                }
            }
            return bedenler;
        }
        public parametre_beden BedenleriGetir(int beden_id)
        {
            #region Query
            string query = @"
                            Select beden_id,bedengrubu,beden,beden_tanimi,sira from parametre_beden
                            Where ISNULL(kayit_silindi,0) = 0  AND beden_id=@beden_id
                            ORDER BY bedengrubu,beden,sira
                ";
            #endregion
            #region Paramaters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@beden_id",beden_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            parametre_beden beden = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                beden = new parametre_beden();
                beden.beden_id = dt.Rows[0]["beden_id"].acekaToInt();
                beden.bedengrubu = dt.Rows[0]["bedengrubu"].acekaToString();
                beden.beden = dt.Rows[0]["beden"].acekaToString();
                beden.beden_tanimi = dt.Rows[0]["beden_tanimi"].acekaToString();
                beden.sira = dt.Rows[0]["sira"].acekaToInt();
            }
            return beden;
        }
        public List<parametre_uretimyeri> Uretimyer()
        {
            List<parametre_uretimyeri> uretimyerleri = null;

            #region Query
            string query = @"
                            SELECT 
                             uretimyeri_id
                            ,uretimyeri_tanim
                            ,uretimyeri_kod 
                            ,kayit_silindi
                            ,uretimyeri_rgb
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            FROM parametre_uretimyeri
                        ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_uretimyeri uretimyeri = null;
                uretimyerleri = new List<parametre_uretimyeri>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    uretimyeri = new parametre_uretimyeri();
                    uretimyeri.uretimyeri_id = dt.Rows[i]["uretimyeri_id"].acekaToShort();
                    uretimyeri.uretimyeri_tanim = dt.Rows[i]["uretimyeri_tanim"].ToString();
                    uretimyeri.uretimyeri_kod = dt.Rows[i]["uretimyeri_kod"].ToString();
                    uretimyeri.uretimyeri_rgb = dt.Rows[i]["uretimyeri_rgb"].acekaToInt();
                    uretimyeri.degistiren_carikart_id = Tools.PersonelId;
                    uretimyeri.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    uretimyerleri.Add(uretimyeri);
                    uretimyeri = null;
                }
            }
            return uretimyerleri;
        }
        public parametre_uretimyeri Uretimyer(int uretimyeri_id)
        {
            #region Query
            string query = @"
                            SELECT 
                             uretimyeri_id
                            ,uretimyeri_tanim
                            ,uretimyeri_kod 
                            ,kayit_silindi
                            ,uretimyeri_rgb
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            FROM parametre_uretimyeri
                            WHERE uretimyeri_id=@uretimyeri_id
                        ";
            #endregion
            #region parameters
            SqlParameter[] parameters = new SqlParameter[] {
                 new SqlParameter("@uretimyeri_id", uretimyeri_id)
                };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            parametre_uretimyeri uretimyeri = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                uretimyeri = new parametre_uretimyeri();
                uretimyeri.uretimyeri_id = dt.Rows[0]["uretimyeri_id"].acekaToShort();
                uretimyeri.uretimyeri_tanim = dt.Rows[0]["uretimyeri_tanim"].ToString();
                uretimyeri.uretimyeri_kod = dt.Rows[0]["uretimyeri_kod"].ToString();
                uretimyeri.uretimyeri_rgb = dt.Rows[0]["uretimyeri_rgb"].acekaToInt();
                uretimyeri.degistiren_carikart_id = Tools.PersonelId;
                uretimyeri.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
            }
            return uretimyeri;
        }
        public List<parametre_siparisturu> SiparisTuruGetir()
        {
            List<parametre_siparisturu> siparisturleri = null;

            #region Query
            string query = @"
                            SELECT siparisturu_id
                            ,kayit_silindi
                            ,siparisturu_tanim
                            ,varsayilan
                            ,sira
                            ,genel
                            ,kalite2
                            ,numune
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            FROM parametre_siparisturu";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_siparisturu siparisturu = null;
                siparisturleri = new List<parametre_siparisturu>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    siparisturu = new parametre_siparisturu();
                    siparisturu.siparisturu_id = dt.Rows[i]["siparisturu_id"].acekaToByte();
                    siparisturu.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    siparisturu.siparisturu_tanim = dt.Rows[i]["siparisturu_tanim"].ToString();
                    siparisturu.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                    siparisturu.sira = dt.Rows[i]["sira"].acekaToInt();
                    siparisturu.genel = dt.Rows[i]["genel"].acekaToBool();
                    siparisturu.kalite2 = dt.Rows[i]["kalite2"].acekaToBool();
                    siparisturu.numune = dt.Rows[i]["numune"].acekaToBool();
                    siparisturu.degistiren_carikart_id = Tools.PersonelId;
                    siparisturu.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    siparisturleri.Add(siparisturu);
                }

            }

            return siparisturleri;
        }
        public parametre_siparisturu SiparisTuruGetir(byte siparisturu_id)
        {
            #region Query
            string query = @"
                            SELECT siparisturu_id
                            ,kayit_silindi
                            ,siparisturu_tanim
                            ,varsayilan
                            ,sira
                            ,genel
                            ,kalite2
                            ,numune
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            FROM parametre_siparisturu 
                            WHERE siparisturu_id = @siparisturu_id";
            #endregion
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@siparisturu_id",siparisturu_id)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            parametre_siparisturu siparisturu = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                siparisturu = new parametre_siparisturu();
                siparisturu.siparisturu_id = dt.Rows[0]["siparisturu_id"].acekaToByte();
                siparisturu.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                siparisturu.siparisturu_tanim = dt.Rows[0]["siparisturu_tanim"].ToString();
                siparisturu.varsayilan = dt.Rows[0]["varsayilan"].acekaToBool();
                siparisturu.sira = dt.Rows[0]["sira"].acekaToInt();
                siparisturu.genel = dt.Rows[0]["genel"].acekaToBool();
                siparisturu.kalite2 = dt.Rows[0]["kalite2"].acekaToBool();
                siparisturu.numune = dt.Rows[0]["numune"].acekaToBool();
                siparisturu.degistiren_carikart_id = Tools.PersonelId;
                siparisturu.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
            }
            return siparisturu;
        }
        public List<parametre_beden_carikart> BedenCarikartListe()
        {
            List<parametre_beden_carikart> bdncarikartlar = null;

            #region Query
            string query = @"
                            SELECT 
                            *
                            FROM parametre_beden_carikart
                        ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_beden_carikart beden_carikart = null;
                bdncarikartlar = new List<parametre_beden_carikart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    beden_carikart = new parametre_beden_carikart();
                    beden_carikart.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    beden_carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    beden_carikart.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    beden_carikart.bedenkodu = dt.Rows[i]["bedenkodu"].ToString();
                    beden_carikart.degistiren_carikart_id = Tools.PersonelId;
                    beden_carikart.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    bdncarikartlar.Add(beden_carikart);
                    beden_carikart = null;
                }
            }
            return bdncarikartlar;
        }
        public List<parametre_beden_carikart> BedenCarikartListe(short beden_id, long carikart_id)
        {
            List<parametre_beden_carikart> bedenlerCarikart = null;
            #region Query
            string query = @"
                            SELECT 
                            *
                            FROM parametre_beden_carikart
                        ";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_beden_carikart beden_carikart = null;
                bedenlerCarikart = new List<parametre_beden_carikart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    beden_carikart = new parametre_beden_carikart();
                    beden_carikart.beden_id = dt.Rows[i]["beden_id"].acekaToInt();
                    beden_carikart.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    beden_carikart.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    beden_carikart.bedenkodu = dt.Rows[i]["bedenkodu"].ToString();
                    beden_carikart.degistiren_carikart_id = Tools.PersonelId;
                    beden_carikart.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    bedenlerCarikart.Add(beden_carikart);
                    beden_carikart = null;
                }
            }
            return bedenlerCarikart;
        }
        public List<parametre_carikart_rapor> cariraporListe()
        {
            List<parametre_carikart_rapor> cariraporlar = null;
            #region Query
            string query = @"select * from parametre_carikart_rapor where isnull(kayit_silindi,0) = 0  order by parametre";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_carikart_rapor carirapor = null;
                cariraporlar = new List<parametre_carikart_rapor>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    carirapor = new parametre_carikart_rapor();
                    carirapor.parametre_id = dt.Rows[i]["parametre_id"].acekaToByte();
                    carirapor.tanim = dt.Rows[i]["tanim"].acekaToString();
                    carirapor.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    carirapor.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    cariraporlar.Add(carirapor);
                    carirapor = null;
                }
            }
            return cariraporlar;
        }
        public List<parametre_carikart_rapor> CariRaporGetir(short parametre_grubu, short parametre)
        {
            List<parametre_carikart_rapor> cariraporlar = null;
            #region Query
            string query = @"select * from parametre_carikart_rapor where isnull(kayit_silindi,0) = 0  
                             AND parametre_grubu = @parametre_grubu AND parametre = @parametre                           
                             order by parametre";
            #endregion
            #region parameters
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@parametre_grubu",parametre_grubu),
                 new SqlParameter("@parametre",parametre)
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_carikart_rapor raporparametre = null;
                cariraporlar = new List<parametre_carikart_rapor>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    raporparametre = new parametre_carikart_rapor();
                    raporparametre.parametre_id = dt.Rows[i]["parametre_id"].acekaToByte();
                    raporparametre.tanim = dt.Rows[i]["tanim"].acekaToString();
                    raporparametre.parametre = dt.Rows[i]["parametre"].acekaToByte();
                    //raporparametre.kaynak_1_parametre_id = dt.Rows[i]["kaynak_1_parametre_id"].acekaToInt();
                    cariraporlar.Add(raporparametre);
                    raporparametre = null;
                }

                //carirapor = new parametre_carikart_rapor();
                //carirapor.parametre_id = dt.Rows[0]["parametre_id"].acekaToInt();
                //carirapor.kayit_silindi = dt.Rows[0]["kayit_silindi"].acekaToBool();
                //carirapor.dil_1_tanim = dt.Rows[0]["dil_1_tanim"].ToString();
                //carirapor.dil_2_tanim = dt.Rows[0]["dil_2_tanim"].ToString();
                //carirapor.dil_3_tanim = dt.Rows[0]["dil_3_tanim"].ToString();
                //carirapor.dil_4_tanim = dt.Rows[0]["dil_4_tanim"].ToString();
                //carirapor.dil_5_tanim = dt.Rows[0]["dil_5_tanim"].ToString();
                //carirapor.grup1 = dt.Rows[0]["grup1"].ToString();
                //carirapor.grup2 = dt.Rows[0]["grup2"].ToString();
                //carirapor.kaynak_1_parametre_id = dt.Rows[0]["kaynak_1_parametre_id"].acekaToInt();
                //carirapor.kaynak_2_parametre_id = dt.Rows[0]["kaynak_2_parametre_id"].acekaToInt();
                //carirapor.kaynak_3_parametre_id = dt.Rows[0]["kaynak_3_parametre_id"].acekaToInt();
                //carirapor.kaynak_4_parametre_id = dt.Rows[0]["kaynak_4_parametre_id"].acekaToInt();
                //carirapor.sira = dt.Rows[0]["sira"].acekaToInt();
                //carirapor.kod = dt.Rows[0]["kod"].ToString();
                //carirapor.parametre = dt.Rows[0]["parametre"].acekaToByte();
                //carirapor.parametre_grubu = dt.Rows[0]["parametre_grubu"].acekaToByte();
                //carirapor.tanim = dt.Rows[0]["tanim"].ToString();
                //carirapor.degistiren_carikart_id = Tools.PersonelId;
                //carirapor.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
            }
            //return carirapor;
            return cariraporlar;
        }
        public List<parametre_kalite2_tur> KaliteTurListe()
        {
            List<parametre_kalite2_tur> parametrekaliteTurleri = null;
            #region Query
            string query = @"select * from parametre_kalite2_tur";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_kalite2_tur parametrekalite2 = null;
                parametrekaliteTurleri = new List<parametre_kalite2_tur>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parametrekalite2 = new parametre_kalite2_tur();
                    parametrekalite2.kalite2tur_id = dt.Rows[i]["kalite2tur_id"].acekaToInt();
                    parametrekalite2.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    parametrekalite2.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    parametrekalite2.kayit_silindi = dt.Rows[i]["kayit_silindi"].acekaToBool();
                    parametrekalite2.numune = dt.Rows[i]["numune"].acekaToByte();
                    parametrekalite2.sira = dt.Rows[i]["sira"].acekaToByte();
                    parametrekalite2.tanim = dt.Rows[i]["tanim"].ToString();
                    parametrekaliteTurleri.Add(parametrekalite2);
                    parametre = null;
                }
            }
            return parametrekaliteTurleri;
        }
        public List<parametre_uretimyeri_carikart> UretimyeriCarikarListe()
        {
            List<parametre_uretimyeri_carikart> parametreuretimyerleri = null;
            #region Query
            string query = @"select * from parametre_uretimyeri_carikart";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                parametre_uretimyeri_carikart parametreuretimyeri = null;
                parametreuretimyerleri = new List<parametre_uretimyeri_carikart>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    parametreuretimyeri = new parametre_uretimyeri_carikart();
                    parametreuretimyeri.carikart_id = dt.Rows[i]["carikart_id"].acekaToLong();
                    parametreuretimyeri.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    parametreuretimyeri.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    parametreuretimyeri.kod1 = dt.Rows[i]["kod1"].ToString();
                    parametreuretimyeri.kod2 = dt.Rows[i]["kod2"].ToString();
                    parametreuretimyeri.kod3 = dt.Rows[i]["kod3"].ToString();
                    parametreuretimyeri.made_in = dt.Rows[i]["made_in"].ToString();
                    parametreuretimyeri.oncelik_sira = dt.Rows[i]["oncelik_sira"].acekaToByte();
                    parametreuretimyeri.uretimyeri_id = dt.Rows[i]["uretimyeri_id"].acekaToShort();
                    parametreuretimyeri.varsayilan = dt.Rows[i]["varsayilan"].acekaToBool();
                    parametreuretimyerleri.Add(parametreuretimyeri);
                    parametre = null;
                }
            }
            return parametreuretimyerleri;
        }
    }
}
