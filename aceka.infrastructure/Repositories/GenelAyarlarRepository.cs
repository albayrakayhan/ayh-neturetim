using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static aceka.infrastructure.Models.GenelAyarlar;

namespace aceka.infrastructure.Repositories
{
    public class GenelAyarlarRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private SqlConnection conn = null;
        private takvim takvim = null;
        private List<takvim> takvimListe = null;
        private List<gtip_belge> gtip_belgeler = null;
        private gtip_belge gtip_belge = null;
        private List<SistemAyarlari> mSistemAyarlar = null;
        private SistemAyarlari mSistemAyari = null;
        #endregion
        #region Çalışma Takvimi
        public List<takvim> Getir()
        {

            #region Query
            string query = @"SET LANGUAGE Turkish
                            SELECT  sene,tarih, datename(dw,gun) as gun_adi, day(hafta),ay,aciklama,tatil_turu,hafta,gun
                            FROM takvim order by tarih ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               //new SqlParameter("@sene",sene)
               //new SqlParameter("@ay",tarih.Month)
            };

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                takvimListe = new List<takvim>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    takvim = new takvim();
                    takvim.sene = dt.Rows[i]["sene"].acekaToShort();
                    takvim.ay = dt.Rows[i]["ay"].acekaToByte();
                    takvim.hafta = dt.Rows[i]["hafta"].acekaToByte();
                    takvim.tarih = dt.Rows[i]["tarih"].acekaToDateTime();
                    takvim.gun = dt.Rows[i]["gun"].acekaToByte();
                    takvim.gun_adi = dt.Rows[i]["gun_adi"].ToString();
                    takvim.tatil_turu = dt.Rows[i]["tatil_turu"].acekaToByte();
                    takvim.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                    takvimListe.Add(takvim);
                    takvim = null;
                }
            }

            return takvimListe;
        }
        public List<takvim> SeneGetir()
        {

            #region Query
            string query = @"SELECT
                              sene	
                                    FROM takvim group by sene order by sene";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               //new SqlParameter("@yil",sene.Year),
               //new SqlParameter("@ay",tarih.Month)
            };

            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                takvimListe = new List<takvim>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    takvim = new takvim();
                    takvim.sene = dt.Rows[i]["sene"].acekaToShort();
                    takvimListe.Add(takvim);
                    takvim = null;
                }
            }

            return takvimListe;
        }
        #endregion
        #region GTipListe
        /// <summary>
        /// GTipListe Liste Döndürür DropDownList için
        /// </summary>
        /// <returns></returns>
        public List<gtip_belge> GtipListe()
        {

            string query = "Select * From gtip_belge";

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                gtip_belgeler = new List<gtip_belge>();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gtip_belge = new gtip_belge();
                    gtip_belge.belge_id = dt.Rows[i]["belge_id"].acekaToInt();
                    gtip_belge.belgeno = dt.Rows[i]["belgeno"].acekaToString();
                    gtip_belge.belge_tarihi = dt.Rows[i]["belge_tarihi"].acekaToDateTime();
                    gtip_belge.bitis_tarihi = dt.Rows[i]["bitis_tarihi"].acekaToDateTime();
                    gtip_belgeler.Add(gtip_belge);
                    gtip_belge = null;
                }
                return gtip_belgeler;
            }

            return null;
        }
        #endregion
        #region GTipGetMethod
        /// <summary>
        /// Gtip Get Methodu
        /// </summary>
        /// <returns></returns>
        public List<gtip_belge> gtipBelgeListDetay()
        {
            gtip_belgeler = new List<gtip_belge>();

            #region Query
            /*string query = @"SELECT p1.parametre_grubu, p1.parametre_id , p2.parametre_id, p3.parametre_id,
                                p1.tanim + ' ' + p2.tanim + ' ' + p3.tanim AS aciklama, 
                                d.gtip_genel, d.gtip_bayan, isnull(d.birim,1) AS birim, d.adet, d.kg, d.birim_fob, d.toplam_fob
                                FROM vparametre_stokkart_rapor1                p1 
                                INNER JOIN vparametre_stokkart_rapor2   p2 ON p2.parametre_grubu=20 
                                INNER JOIN vparametre_stokkart_rapor5   p3 ON p3.parametre_grubu=0 
                                LEFT JOIN gtip_belge                           b ON b.belge_id=4 
                                LEFT JOIN gtip_belgedetay                      d ON d.belge_id=b.belge_id AND d.stokkart_tipi_id=0 AND d.stokalan_id_1=p1.parametre_id AND d.stokalan_id_2=p2.parametre_id
                                AND  d.stokalan_id_3=p3.parametre_id 
                                --WHERE p1.parametre_grubu = 20 AND Len(p1.kod2) > 0 AND Len(p2.kod2) > 0 AND Len(p3.kod2) > 0 
                                AND
                                EXISTS(
                                SELECT 1 
                                FROM stokkart_model x 
                                INNER JOIN stokkart xm on xm.stokkart_id=x.stokkart_id and xm.statu=0 
                                INNER JOIN stokkart_rapor_parametre xk ON xk.stokkart_id=x.alt_stokkart_id 
                                WHERE xk.stokalan_id_5=p3.parametre_id and xk.stokalan_id_1=p1.parametre_id and xk.stokalan_id_2=p2.parametre_id) ORDER BY 1,2,3;";*/

            string new_query = "Select * From gtip_belge gb INNER JOIN gtip_belgedetay gd ON gb.belge_id = gd.belge_id";
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, new_query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {


                    gtip_belge = new gtip_belge();
                    gtip_belge.belge_id = dt.Rows[i]["belge_id"].acekaToInt();
                    gtip_belge.belgeno = dt.Rows[i]["belgeno"].acekaToString();
                    gtip_belge.belge_tarihi = dt.Rows[i]["belge_tarihi"].acekaToDateTime();
                    gtip_belge.bitis_tarihi = dt.Rows[i]["bitis_tarihi"].acekaToDateTime();
                    gtip_belge.belge_tarihi = dt.Rows[i]["belge_tarihi"].acekaToDateTime();
                    gtip_belge.bitis_tarihi = dt.Rows[i]["bitis_tarihi"].acekaToDateTime();

                    gtip_belge.gtipdetay = new gtip_belgedetay();
                    gtip_belge.gtipdetay.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                    gtip_belge.gtipdetay.adet = dt.Rows[i]["adet"].acekaToLong();
                    gtip_belge.gtipdetay.kg = dt.Rows[i]["kg"].acekaToInt();
                    gtip_belge.gtipdetay.gtip_genel = dt.Rows[i]["gtip_genel"].acekaToString();
                    gtip_belge.gtipdetay.acan_tarih = dt.Rows[i]["acan_tarih"].acekaToDateTime();
                    gtip_belge.gtipdetay.gtip_bayan = dt.Rows[i]["gtip_bayan"].acekaToString();
                    gtip_belge.gtipdetay.birim_fob = dt.Rows[i]["birim_fob"].acekaToFloat();
                    gtip_belge.gtipdetay.toplam_fob = dt.Rows[i]["toplam_fob"].acekaToFloat();
                    gtip_belge.gtipdetay.birim = dt.Rows[i]["birim"].acekaToInt();
                    gtip_belgeler.Add(gtip_belge);
                    gtip_belge = null;
                }
                return gtip_belgeler;
            }
            return null;

        }
        #endregion


        /// <summary>
        /// GTip Belge Araması Yapmak İçin Kullanılıyor
        /// </summary>
        /// <returns></returns>
        #region GTip Tanımlama için GET Methodu
        public List<gtip_belge> GTipBul(byte stokkart_tipi_id = 200, int belge_id = 0, int modeltipi_id = -1, int kumastipi_id = 0)
        {
            if (stokkart_tipi_id != 0 && stokkart_tipi_id > 0 && belge_id != 0 && belge_id > 0)
            {
                short StatementCount = 0;
                string JoinString = "";
                string orStatement = "";

                #region QueryControls
                if (belge_id != 0)
                {
                    StatementCount++;
                    JoinString += " LEFT JOIN gtip_belge gb ON gd.belge_id = gb.belge_id ";
                    orStatement += " gb.belge_id = @belge_id ";
                }

                if (stokkart_tipi_id != 200)
                {
                    StatementCount++;
                    orStatement += " AND gd.stokkart_tipi_id = @stokkart_tipi_id ";
                }

                if (modeltipi_id != -1)
                {
                    StatementCount++;
                    JoinString += " LEFT JOIN parametre_stokkart_rapor psrr ON gd.stokalan_id_3 = psrr.parametre_id ";
                    orStatement += " AND gd.stokalan_id_3  = @stokalan_id_5 ";
                }

                if (kumastipi_id != 0)
                {
                    StatementCount++;
                    JoinString += " LEFT JOIN parametre_stokkart_rapor prr ON prr.parametre_id = gd.stokalan_id_2 ";
                    orStatement += " AND gd.stokalan_id_2 = @stokalan_id_2 ";

                }
                #endregion

                #region Query
                string query = @"Select * From gtip_belgedetay gd 
                                LEFT JOIN parametre_birim pb ON pb.birim_id = gd.birim 
                                LEFT JOIN parametre_stokkart_rapor psr ON gd.stokalan_id_3 = psr.parametre_id " + JoinString + "WHERE " + orStatement;
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@stokkart_tipi_id",stokkart_tipi_id),
                    new SqlParameter("@belge_id",belge_id),
                    new SqlParameter("@stokalan_id_5",modeltipi_id),
                    new SqlParameter("@stokalan_id_2",kumastipi_id)
            };

                #endregion

                if (StatementCount > 0)
                {
                    dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                    if (dt != null && dt.Rows.Count != 0)
                    {
                        gtip_belgeler = new List<gtip_belge>();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            /*Ayahn*/
                            gtip_belge = new gtip_belge();
                            gtip_belge.gtipdetay = new gtip_belgedetay();
                            gtip_belge.belge_id = dt.Rows[i]["belge_id"].acekaToInt();
                            gtip_belge.gtipdetay.gtip_genel = dt.Rows[i]["gtip_genel"].acekaToString();
                            gtip_belge.gtipdetay.gtip_bayan = dt.Rows[i]["gtip_bayan"].acekaToString();
                            gtip_belge.gtipdetay.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                            gtip_belge.gtipdetay.birim = dt.Rows[i]["birim"].acekaToInt();
                            gtip_belge.gtipdetay.adet = dt.Rows[i]["adet"].acekaToLong();
                            gtip_belge.gtipdetay.kg = dt.Rows[i]["kg"].acekaToInt();
                            gtip_belge.gtipdetay.birim_fob = dt.Rows[i]["birim_fob"].acekaToFloat();
                            gtip_belge.gtipdetay.toplam_fob = dt.Rows[i]["toplam_fob"].acekaToFloat();
                            gtip_belge.gtipdetay.birim_adi = dt.Rows[i]["birim_adi"].acekaToString();
                            gtip_belge.gtipdetay.stokalan_id_1 = dt.Rows[i]["stokalan_id_1"].acekaToInt();
                            gtip_belge.gtipdetay.stokalan_id_2 = dt.Rows[i]["stokalan_id_2"].acekaToInt();
                            gtip_belge.gtipdetay.stokalan_id_3 = dt.Rows[i]["stokalan_id_3"].acekaToInt();
                            gtip_belge.gtipdetay.stokalan_id_4 = dt.Rows[i]["stokalan_id_4"].acekaToInt();
                            gtip_belge.gtipdetay.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                            gtip_belge.gtipdetay.stokkart_tipi_id = dt.Rows[i]["stokkart_tipi_id"].acekaToByte();
                            gtip_belge.gtipdetay.pb = dt.Rows[i]["pb"].acekaToString();

                            gtip_belgeler.Add(gtip_belge);
                            gtip_belge = null;
                        }
                        return gtip_belgeler;
                    }
                    query = null;
                }
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Sistem Ayarları metodları burada tanımlanmıştır.
        /// </summary>
        #region SistemAyarlari

        /// <summary>
        /// Sistem Ayarlarındaki tüm kayıtları döndürür.
        /// </summary>
        /// <returns></returns>
        public List<SistemAyarlari> TumSistemAyarlari()
        {

            #region Query
            string query = @"SELECT ayaradi, ayaraciklama, ayar, degistiren_carikart_id FROM giz_setup WHERE ayaradi='versiyon_program'
                                OR ayaradi='kesim_fazla'
                                OR ayaradi='kesim_fire'
                                OR ayaradi='musteri_fazla'
                                OR ayaradi='koligram_m2'									
                                OR ayaradi='model_birim'
                                OR ayaradi='kumas_birim'
                                OR ayaradi='kumas_birim2'
                                OR ayaradi='kumas_birim3'
                                OR ayaradi='kumas_fire_orani'
                                OR ayaradi='aksesuar_birim'
                                OR ayaradi='aksesuar_birim2'
                                OR ayaradi='aksesuar_birim3'
                                OR ayaradi='aksesuar_fire_orani'
                                OR ayaradi='siparis_sezon'
                                OR ayaradi='organikkesimfisiraporyuzdesi'
                                OR ayaradi='iplik_kodu'
                                OR ayaradi='iplik_aks_stkalan01'
                                OR ayaradi='kilcik_kodu'
                                OR ayaradi='kilcik_aks_stkalan01'
                                OR ayaradi='iplik_birim_carpan'
                                OR ayaradi='kilcik_birim_carpan'
                                OR ayaradi='iplik_talimatturu'
                                OR ayaradi='kilcik_talimatturu'
                                OR ayaradi='kalitekontrol_aks_satinalma_oto_onay'
                                OR ayaradi='kalitekontrol_aks_satinalma_oto_onaykullanici'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_desc'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_format1'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_format2'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_format3'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_format4'
                                OR ayaradi='stokkart_aksesuar_addto_talimat_format5'
                                OR ayaradi='asorti_kullanim'
                                OR ayaradi='model_grup6_default'
                                OR ayaradi='stokkart_model_muhkod_default'
                                OR ayaradi='stokkart_kumas_muhkod_default'
                                OR ayaradi='stokkart_aksesuar_muhkod_default'
                                OR ayaradi='stokkart_iplik_muhkod_default'
                                OR ayaradi='demand_pull_yukleme_haftasi'
                                AND degistiren_carikart_id = 2226";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                mSistemAyarlar = new List<SistemAyarlari>();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    mSistemAyari = new SistemAyarlari();
                    mSistemAyari.ayaradi = dt.Rows[i]["ayaradi"].acekaToString();
                    mSistemAyari.ayar = dt.Rows[i]["ayar"].acekaToString();
                    mSistemAyari.ayaraciklama = dt.Rows[i]["ayaraciklama"].acekaToString();
                    mSistemAyari.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    mSistemAyarlar.Add(mSistemAyari);
                    mSistemAyari = null;
                }
                return mSistemAyarlar;
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Tailmat Turu Tanimlama İçin KDV Tevkifat ve Fasoncu Metodları
        /// </summary>
        /// <returns></returns>
        #region Talimat Turu Tanimlama için Kdv Tevkifat ve Fasoncu Metodları
        // Talimat Turu Tanımlama için Kdv Tevkifat Listesi
        public List<parametre_kdv> TalimatKdvTevkifatListesi()
        {
            List<parametre_kdv> kdvler = null;
            #region Query
            string query = @"SELECT * FROM parametre_kdv";
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
                    kdv.oran = dt.Rows[i]["oran"].acekaToLong();
                    kdvler.Add(kdv);
                    kdv = null;
                }
            }
            return kdvler;
        }
        // Talimat Turu Tanımlama için Fasoncu Listesi
        public List<cari_kart> TalimatFasoncuListesi()
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
                    fason.cari_unvan = dt.Rows[i]["cari_unvan"].acekaToString();
                    fasoncular.Add(fason);
                    fason = null;
                }
                return fasoncular;
            }
            return null;
        }
        #endregion
    }
}