using aceka.infrastructure.Core;
using aceka.infrastructure.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace aceka.infrastructure.Repositories
{
    public class SiparisRepository
    {
        #region Değişkenler
        private DataTable dt = null;
        private DataSet ds;
        private List<siparis> siparisler = null;
        #endregion

        public List<siparis> Bul(string siparis_no = "", long musteri_carikart_id = 0, byte sezon_id = 0, string modelno = "", string modeladi = "", string baslangic_tarihi = "", string bitis_tarihi = "")
        {
            List<siparis> siparisler = null;
            short parameterControl = 0;

            #region Query
            string orStatement = "";
            //if (siparis_id > 0)
            //{
            //    parameterControl++;
            //    orStatement += "S.siparis_id = @siparis_id OR ";
            //}
            if (!string.IsNullOrEmpty(siparis_no.TrimEnd()))
            {
                parameterControl++;
                orStatement += "S.siparis_no like @siparis_no OR ";
            }
            if (musteri_carikart_id > 0)
            {
                parameterControl++;
                orStatement += " S.musteri_carikart_id  = @musteri_carikart_id OR ";
            }
            //if (stokyeri_carikart_id > 0)
            //{
            //    parameterControl++;
            //    orStatement += " S.stokyeri_carikart_id  = @stokyeri_carikart_id OR ";
            //}
            if (sezon_id > 0)
            {
                parameterControl++;
                orStatement += "  O.sezon_id  = @sezon_id OR ";
            }
            if (!string.IsNullOrEmpty(modelno.Trim()))
            {
                parameterControl++;
                orStatement += " SK.stok_kodu  = @stok_kodu OR ";
            }
            if (!string.IsNullOrEmpty(modeladi))
            {
                parameterControl++;
                orStatement += " SK.stok_adi  = @stok_adi OR ";
            }

            if (!string.IsNullOrEmpty(baslangic_tarihi))
            {
                parameterControl++;

                if (!string.IsNullOrEmpty(bitis_tarihi))
                {
                    orStatement += " (S.siparis_tarihi between @baslangic_tarihi AND @bitis_tarihi) OR ";
                }
                else
                {
                    orStatement += " S.siparis_tarihi >= @baslangic_tarihi OR ";
                }
            }

            if (!string.IsNullOrEmpty(orStatement))
            {
                orStatement = "(" + orStatement.TrimEnd(new char[] { 'O', 'R', ' ' }) + ")";
            }

            orStatement += " ";

            string query = @"
                            SET DATEFORMAT DMY
                            SELECT 
                                S.siparis_id,
                                S.siparis_no, 
                                S.siparis_tarihi,
                                S.musteri_carikart_id,
                                CK_Musteri.cari_unvan as 'musteri_cari_unvan',
                                S.stokyeri_carikart_id,
                                CK_Stokyeri.cari_unvan as 'stokyeri_cari_unvan',
                                S.Siparisturu_id,
                                ST.siparisturu_tanim as 'siparisturu_tanim',
                                S.zorlukgrubu_id,
                                PRM_ZorlukGrubu.tanim as 'zorlukgrubu_tanim',
                                O.sezon_id,
								O.sira_id,
								O.tahmini_uretim_tarihi,
								O.tahmini_dikim_tarihi,
								O.isteme_tarihi,
								O.stokkart_id,
								SK.stok_adi,
								SK.stok_kodu	
                            FROM siparis S
                            LEFT JOIN siparis_ozel O ON O.siparis_id = S.siparis_id
                            LEFT JOIN stokkart SK ON SK.stokkart_id = O.stokkart_id
                            LEFT JOIN parametre_siparisturu ST ON ST.siparisturu_id = S.siparisturu_id
                            LEFT JOIN carikart CK_Musteri ON CK_Musteri.carikart_id = S.musteri_carikart_id
                            LEFT JOIN carikart CK_Stokyeri ON CK_Stokyeri.carikart_id = S.stokyeri_carikart_id
                            LEFT JOIN parametre_zorlukgrubu PRM_ZorlukGrubu ON PRM_ZorlukGrubu.zorlukgrubu_id = S.zorlukgrubu_id
                            WHERE " + orStatement + " AND isnull(S.kayit_silindi,0) = 0";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    //new SqlParameter("@siparis_id",siparis_id),
                    new SqlParameter("@siparis_no","%"  +siparis_no + "%"),
                    new SqlParameter("@musteri_carikart_id",musteri_carikart_id),
                    //new SqlParameter("@stokyeri_carikart_id",stokyeri_carikart_id),
                    new SqlParameter("@sezon_id",sezon_id),
                    new SqlParameter("@stok_kodu",modelno),
                    new SqlParameter("@stok_adi",modeladi),
                    new SqlParameter("@baslangic_tarihi",baslangic_tarihi),
                    new SqlParameter("@bitis_tarihi",bitis_tarihi)
            };
            #endregion

            if (parameterControl > 0)
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    //sorgu sonucunda aynı olan siparis_id distinct yapılarak elde edilen unique ID ler ile select yapılarak siparis_ozel dataları elde ediliyor!

                    DataView view = new DataView(dt);
                    DataTable distinctValues = view.ToTable(true, "siparis_id");

                    if (distinctValues != null && distinctValues.Rows.Count > 0)
                    {
                        siparisler = new List<siparis>();
                        siparis _siparis = null;

                        long sipariId = 0;
                        for (int temp = 0; temp < distinctValues.Rows.Count; temp++)
                        {
                            sipariId = distinctValues.Rows[temp]["siparis_id"].acekaToLong();

                            var siparisTableRows = dt.Select("siparis_id = " + sipariId);

                            if (siparisTableRows != null && siparisTableRows.Length > 0)
                            {

                                _siparis = new siparis();
                                _siparis.siparis_id = siparisTableRows[0]["siparis_id"].acekaToLong();
                                _siparis.siparis_no = siparisTableRows[0]["siparis_no"].acekaToString();
                                _siparis = new siparis();
                                _siparis.siparis_id = siparisTableRows[0]["siparis_id"].acekaToLong();
                                _siparis.siparis_no = siparisTableRows[0]["siparis_no"].acekaToString();
                                _siparis.siparis_tarihi = siparisTableRows[0]["siparis_tarihi"].acekaToDateTime();
                                // stokkart Model No ve Model Adı için eklendi.AA
                                _siparis.stokkart = new stokkart();
                                _siparis.stokkart.stok_adi = dt.Rows[0]["stok_adi"].ToString();
                                _siparis.stokkart.stok_kodu = dt.Rows[0]["stok_kodu"].ToString();

                                //müşteri carikart
                                _siparis.musteri_carikart_id = siparisTableRows[0]["musteri_carikart_id"].acekaToLong();
                                _siparis.musteri_carikart = new cari_kart();
                                _siparis.musteri_carikart.cari_unvan = siparisTableRows[0]["musteri_cari_unvan"].acekaToString();

                                //stokyeri carikart
                                _siparis.stokyeri_carikart_id = siparisTableRows[0]["stokyeri_carikart_id"].acekaToLong();
                                _siparis.stokyeri_carikart = new cari_kart();
                                _siparis.stokyeri_carikart.cari_unvan = siparisTableRows[0]["stokyeri_cari_unvan"].acekaToString();

                                //parametre_siparisturu
                                _siparis.siparisturu_id = siparisTableRows[0]["Siparisturu_id"].acekaToByte();
                                _siparis.parametre_siparisturu = new parametre_siparisturu();
                                _siparis.parametre_siparisturu.siparisturu_tanim = siparisTableRows[0]["siparisturu_tanim"].acekaToString();

                                //parametre_zorlukgrubu 
                                _siparis.zorlukgrubu_id = siparisTableRows[0]["zorlukgrubu_id"].acekaToByte();
                                _siparis.parametre_zorlukgrubu = new parametre_zorlukgrubu();
                                _siparis.parametre_zorlukgrubu.tanim = siparisTableRows[0]["zorlukgrubu_tanim"].acekaToString();


                                //siparis_ozel                                                                
                                _siparis.siparis_ozel = new List<siparis_ozel>();
                                for (int i = 0; i < siparisTableRows.Length; i++)
                                {
                                    _siparis.siparis_ozel.Add(
                                        new siparis_ozel
                                        {
                                            sezon_id = dt.Rows[i]["sezon_id"].acekaToShort(),
                                            sira_id = dt.Rows[i]["sira_id"].acekaToShort(),
                                            stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLongWithNullable(),
                                            tahmini_uretim_tarihi = dt.Rows[i]["tahmini_uretim_tarihi"].acekaToDateTimeWithNullable(),
                                            tahmini_dikim_tarihi = dt.Rows[i]["tahmini_dikim_tarihi"].acekaToDateTimeWithNullable(),
                                            isteme_tarihi = dt.Rows[i]["isteme_tarihi"].acekaToDateTime(),
                                            stokkart = new stokkart()
                                            {
                                                stok_adi = dt.Rows[i]["stok_adi"].acekaToString(),
                                                stok_kodu = dt.Rows[i]["stok_kodu"].acekaToString()
                                            }

                                        });
                                }

                                siparisler.Add(_siparis);
                                _siparis = null;
                            }
                        }
                    }
                }
            }
            return siparisler;
        }

        public siparis Getir(long siparis_id, ref string errorMessage)
        {
            siparis _siparis = null;

            #region Query
            string query = @"
                            SELECT 
                                S.siparis_id,
                                S.siparis_no, 
                                S.siparis_tarihi,
                                S.musteri_carikart_id,
                                CK_Musteri.cari_unvan as 'musteri_cari_unvan',
                                S.stokyeri_carikart_id,
                                CK_Stokyeri.cari_unvan as 'stokyeri_cari_unvan',
                                S.Siparisturu_id,
                                ST.siparisturu_tanim as 'siparisturu_tanim',
                                S.zorlukgrubu_id,
                                PRM_ZorlukGrubu.tanim as 'zorlukgrubu_tanim',
	                            S.musterifazla,
	                            S.uretimyeri_id,
	                            S.mense_uretimyeri_id,
	                            S.siparis_not,
	                            S.pb,
	                            S.statu,
                                SO.siparis_id,
                                SO.sira_id,
                                SO.degistiren_tarih,
                                SO.stokkart_id,
                                SO.bayi_carikart_id,
                                SO.isteme_tarihi,
	                            SO.tahmini_uretim_tarihi,
                                SO.sezon_id,
	                            SO.tahmini_dikim_tarihi,
								SO.ref_siparis_no,
								SO.ref_siparis_no2,
                                SO.ref_sistem_name,
								SO.ref_link_status,
	                            SK.stok_kodu,
								SK.stok_adi,
	                            Ony.genel_onay,
                                Ony.dikim_onay,
								Ony.malzeme_onay,
								Ony.uretim_onay,
								Ony.yukleme_onay
                            FROM siparis S
                            LEFT JOIN siparis_onay Ony ON ony.siparis_id=S.siparis_id
                            LEFT JOIN siparis_ozel SO ON SO.siparis_id = S.siparis_id AND SO.sira_id=0
                            LEFT JOIN parametre_siparisturu ST ON ST.siparisturu_id = S.siparisturu_id
                            LEFT JOIN carikart CK_Musteri  ON CK_Musteri.carikart_id = S.musteri_carikart_id
                            LEFT JOIN carikart CK_Stokyeri  ON CK_Stokyeri.carikart_id = S.stokyeri_carikart_id
                            LEFT JOIN parametre_zorlukgrubu PRM_ZorlukGrubu  ON PRM_ZorlukGrubu.zorlukgrubu_id = S.zorlukgrubu_id
                            LEFT JOIN stokkart SK ON SK.stokkart_id = SO.stokkart_id
                            WHERE S.siparis_id = @siparis_id AND isnull(S.kayit_silindi,0) = 0";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    _siparis = new siparis();
                    _siparis.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                    _siparis.siparis_no = dt.Rows[0]["siparis_no"].acekaToString();
                    _siparis.siparis_tarihi = dt.Rows[0]["siparis_tarihi"].acekaToDateTime();
                    _siparis.statu = dt.Rows[0]["statu"].acekaToByte();

                    //siparis_ozel
                    _siparis.siparisozel = new siparis_ozel();
                    _siparis.siparisozel.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                    _siparis.siparisozel.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                    _siparis.siparisozel.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                    _siparis.siparisozel.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                    _siparis.siparisozel.bayi_carikart_id = dt.Rows[0]["bayi_carikart_id"].acekaToLong();
                    _siparis.siparisozel.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                    _siparis.siparisozel.isteme_tarihi = dt.Rows[0]["isteme_tarihi"].acekaToDateTime();
                    _siparis.siparisozel.tahmini_uretim_tarihi = dt.Rows[0]["tahmini_uretim_tarihi"].acekaToDateTimeWithNullable();
                    _siparis.siparisozel.sezon_id = dt.Rows[0]["sezon_id"].acekaToShortWithNullable();
                    _siparis.siparisozel.tahmini_dikim_tarihi = dt.Rows[0]["tahmini_dikim_tarihi"].acekaToDateTimeWithNullable();
                    _siparis.siparisozel.ref_siparis_no = dt.Rows[0]["ref_siparis_no"].ToString();
                    _siparis.siparisozel.ref_siparis_no2 = dt.Rows[0]["ref_siparis_no2"].ToString();
                    _siparis.siparisozel.ref_sistem_name = dt.Rows[0]["ref_sistem_name"].ToString();
                    _siparis.siparisozel.ref_link_status = dt.Rows[0]["ref_link_status"].acekaToByteWithNullable();


                    //siparis_detay
                    //_siparis.siparis_detay = new siparis_detay();
                    //_siparis.siparis_detay.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();

                    //müşteri carikart
                    _siparis.musteri_carikart_id = dt.Rows[0]["musteri_carikart_id"].acekaToLong();
                    _siparis.musteri_carikart = new cari_kart();
                    _siparis.musteri_carikart.cari_unvan = dt.Rows[0]["musteri_cari_unvan"].acekaToString();

                    //stokyeri carikart
                    _siparis.stokyeri_carikart_id = dt.Rows[0]["stokyeri_carikart_id"].acekaToLong();
                    _siparis.stokyeri_carikart = new cari_kart();
                    _siparis.stokyeri_carikart.cari_unvan = dt.Rows[0]["stokyeri_cari_unvan"].acekaToString();

                    //parametre_siparisturu
                    _siparis.siparisturu_id = dt.Rows[0]["Siparisturu_id"].acekaToByte();
                    _siparis.parametre_siparisturu = new parametre_siparisturu();
                    _siparis.parametre_siparisturu.siparisturu_tanim = dt.Rows[0]["siparisturu_tanim"].acekaToString();

                    //parametre_zorlukgrubu
                    _siparis.zorlukgrubu_id = dt.Rows[0]["zorlukgrubu_id"].acekaToByte();
                    _siparis.parametre_zorlukgrubu = new parametre_zorlukgrubu();
                    _siparis.parametre_zorlukgrubu.tanim = dt.Rows[0]["zorlukgrubu_tanim"].acekaToString();

                    _siparis.musterifazla = dt.Rows[0]["musterifazla"].acekaToShortWithNullable();
                    _siparis.uretimyeri_id = dt.Rows[0]["uretimyeri_id"].acekaToShortWithNullable();
                    _siparis.mense_uretimyeri_id = dt.Rows[0]["mense_uretimyeri_id"].acekaToShortWithNullable();
                    _siparis.siparis_not = dt.Rows[0]["siparis_not"].acekaToString();
                    _siparis.pb = dt.Rows[0]["pb"].acekaToByteWithNullable();
                    _siparis.statu = dt.Rows[0]["statu"].acekaToByte();

                    //stokkart
                    _siparis.stokkart = new stokkart();
                    _siparis.stokkart.stok_kodu = dt.Rows[0]["stok_kodu"].ToString();
                    _siparis.stokkart.stok_adi = dt.Rows[0]["stok_adi"].ToString();
                    _siparis.stokkart.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();


                    //siparis_onay
                    _siparis.siparisonay = new siparis_onay();
                    _siparis.siparisonay.genel_onay = dt.Rows[0]["genel_onay"].acekaToBool();
                    _siparis.siparisonay.dikim_onay = dt.Rows[0]["dikim_onay"].acekaToBool();
                    _siparis.siparisonay.malzeme_onay = dt.Rows[0]["malzeme_onay"].acekaToBool();
                    _siparis.siparisonay.uretim_onay = dt.Rows[0]["uretim_onay"].acekaToBool();
                    _siparis.siparisonay.yukleme_onay = dt.Rows[0]["yukleme_onay"].acekaToBool();
                    //_siparis.stokkart.stok_adi = dt.Rows[0]["stok_adi"].ToString();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return _siparis;
        }

        public List<siparis> SiparisNoAutoComplate(string siparis_no = "")
        {
            siparis sip = null;

            #region Query
            string query = @"
                        SELECT 
                                siparis_id,
                                siparis_no
                        FROM siparis
                        WHERE siparis_no like @siparis_no
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@siparis_no",siparis_no+"%")
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            siparisler = new List<siparis>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sip = new siparis();
                    sip.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                    sip.siparis_no = dt.Rows[i]["siparis_no"].acekaToString();
                    siparisler.Add(sip);
                    sip = null;
                }
            }
            return siparisler;
        }

        /// <summary>
        /// siparis_id ve sira_id parametreleri kullanılacak tek bir kayıt getirir
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <param name="sira_id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public siparis_ozel SiparisOzelGetir(long siparis_id, short sira_id, ref string errorMessage)
        {
            siparis_ozel siparisOzel = null;

            #region Query
            string query = @"
                           SELECT 
	                            siparis_id, 
	                            sira_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            stokkart_id, 
	                            bayi_carikart_id, 
	                            isteme_tarihi, 
	                            tahmini_uretim_tarihi, 
	                            sezon_id, 
	                            tahmini_dikim_tarihi, 
	                            ref_siparis_no, 
	                            ref_siparis_no2, 
	                            ref_sistem_name, 
	                            ref_link_status,
                                stokkart_id
                        FROM siparis_ozel 
                        WHERE siparis_id = @siparis_id AND sira_id = @sira_id";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id),
                    new SqlParameter("@sira_id",sira_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    siparisOzel = new siparis_ozel();
                    siparisOzel.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                    siparisOzel.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                    siparisOzel.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                    siparisOzel.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                    siparisOzel.stokkart_id = dt.Rows[0]["stokkart_id"].acekaToLong();
                    siparisOzel.bayi_carikart_id = dt.Rows[0]["bayi_carikart_id"].acekaToLong();
                    siparisOzel.isteme_tarihi = dt.Rows[0]["isteme_tarihi"].acekaToDateTime();
                    siparisOzel.tahmini_uretim_tarihi = dt.Rows[0]["tahmini_uretim_tarihi"].acekaToDateTimeWithNullable();
                    siparisOzel.sezon_id = dt.Rows[0]["sezon_id"].acekaToShortWithNullable();
                    //siparisOzel.tahmini_dikim_tarihi = dt.Rows[0]["tahmini_dikim_tarihi"].acekaToDateTimeWithNullable();
                    siparisOzel.ref_siparis_no = dt.Rows[0]["ref_siparis_no"].ToString();
                    siparisOzel.ref_siparis_no2 = dt.Rows[0]["ref_siparis_no2"].ToString();
                    siparisOzel.ref_sistem_name = dt.Rows[0]["ref_sistem_name"].ToString();
                    siparisOzel.ref_link_status = dt.Rows[0]["ref_link_status"].acekaToByteWithNullable();
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return siparisOzel;
        }

        /// <summary>
        /// Siparişe ait modelkart lardan kopyalanacak kayıtların kontrolü için tutulan kayıtların listesini verir. 
        /// sira_id = 0 olan kayıtlar genel içerik için gerekli olduğundan sorguda hariç tutulur.
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<siparis_ozel> SiparisOzelListesiniGetir(long siparis_id, ref string errorMessage)
        {
            List<siparis_ozel> siparisOzelListe = null;
            siparis_ozel siparisOzel = null;

            #region Query
            string query = @"
                            SELECT 
	                            SO.siparis_id,
	                            SO.sira_id,
	                            SO.stokkart_id,
	                            S.stok_kodu
                            FROM
                            siparis_ozel SO 
                            INNER JOIN stokkart S ON SO.stokkart_id = S.stokkart_id
                            WHERE siparis_id = @siparis_id --AND sira_id <> 0                             
                            ";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    siparisOzelListe = new List<siparis_ozel>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        siparisOzel = new siparis_ozel();
                        siparisOzel.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                        siparisOzel.sira_id = dt.Rows[i]["sira_id"].acekaToShort();
                        siparisOzel.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        siparisOzel.stokkart = new stokkart
                        {
                            stok_kodu = dt.Rows[i]["stok_kodu"].acekaToString()
                        };

                        siparisOzelListe.Add(siparisOzel);
                        siparisOzel = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return siparisOzelListe;
        }

        /// <summary>
        /// siparis_id parametresi kullanılacak liste getirir. 
        /// Not sira_id <> 0 parametresi ile siparis detayının dışında TAB lerde kullanılacak kayıtlar getiriliyor.
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public List<siparis_ozel> SiparisOzelGetir(long siparis_id, ref string errorMessage)
        {
            List<siparis_ozel> siparisOzelList = null;

            #region Query
            string query = @"
                           SELECT 
	                            siparis_id, 
	                            sira_id, 
	                            degistiren_carikart_id, 
	                            degistiren_tarih, 
	                            stokkart_id, 
	                            bayi_carikart_id, 
	                            isteme_tarihi, 
	                            tahmini_uretim_tarihi, 
	                            sezon_id, 
	                            tahmini_dikim_tarihi, 
	                            ref_siparis_no, 
	                            ref_siparis_no2, 
	                            ref_sistem_name, 
	                            ref_link_status
                        FROM siparis_ozel 
                        WHERE siparis_id = @siparis_id AND sira_id <> 0";
            //Not : sira_id = 0 Sipariş detayına ait. Diğer kayıtlar TAB altında kullanılacak!
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    siparisOzelList = new List<siparis_ozel>();
                    siparis_ozel siparisOzel = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        siparisOzel = new siparis_ozel();
                        siparisOzel.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                        siparisOzel.sira_id = dt.Rows[i]["sira_id"].acekaToShort();
                        siparisOzel.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        siparisOzel.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        siparisOzel.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                        siparisOzel.bayi_carikart_id = dt.Rows[i]["bayi_carikart_id"].acekaToLong();
                        siparisOzel.isteme_tarihi = dt.Rows[i]["isteme_tarihi"].acekaToDateTime();
                        siparisOzel.tahmini_uretim_tarihi = dt.Rows[i]["tahmini_uretim_tarihi"].acekaToDateTimeWithNullable();
                        siparisOzel.sezon_id = dt.Rows[i]["sezon_id"].acekaToShortWithNullable();
                        siparisOzel.tahmini_dikim_tarihi = dt.Rows[i]["tahmini_dikim_tarihi"].acekaToDateTimeWithNullable();
                        siparisOzel.ref_siparis_no = dt.Rows[i]["ref_siparis_no"].ToString();
                        siparisOzel.ref_siparis_no2 = dt.Rows[i]["ref_siparis_no2"].ToString();
                        siparisOzel.ref_sistem_name = dt.Rows[i]["ref_sistem_name"].ToString();
                        siparisOzel.ref_link_status = dt.Rows[i]["ref_link_status"].acekaToByteWithNullable();
                        siparisOzelList.Add(siparisOzel);
                        siparisOzel = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return siparisOzelList;
        }

        public short NotsiraidGetir(long siparis_id, ref string errorMessage)
        {
            //siparis_notlar siparisnot = null;
            short result = 0;
            #region Query
            string query = @"
                             SELECT 
                                 ISNULL(MAX(sira_id),0) as MaxSiraId
                                FROM siparis_notlar where siparis_id=@siparis_id
                            ";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id)
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
        public siparis_notlar NotGetir(long siparis_id, short sira_id, ref string errorMessage)
        {
            siparis_notlar siparisnot = null;

            #region Query
            string query = @"
                            SELECT siparis_id
                            ,sira_id
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            ,aciklama
                            FROM siparis_notlar where siparis_id=@siparis_id AND sira_id=@sira_id
                            ";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id),
                    new SqlParameter("@sira_id",sira_id)
            };
            #endregion
            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    siparisnot = new siparis_notlar();
                    siparisnot.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                    siparisnot.sira_id = dt.Rows[0]["sira_id"].acekaToShort();
                    siparisnot.aciklama = dt.Rows[0]["aciklama"].ToString();
                    siparisnot.degistiren_carikart_id = Tools.PersonelId;
                    siparisnot.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                }
            }
            catch (Exception Ex)
            {
                errorMessage = Ex.Message;
            }
            return siparisnot;
        }

        public string SiparisNoHarfGetir(long siparis_id)
        {
            string result = null;

            #region Query
            string query = @"
                           SELECT 
	                           harf
                        FROM siparis_sirano 
                        WHERE [max] >= @siparis_id order by [max] asc";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id),
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["harf"].ToString();
            }

            return result;
        }

        public int UpdateSiparisNo(long siparis_id, string siparis_no)
        {
            #region Query
            string query = @"
                            UPDATE siparis
	                            SET siparis_no = @siparis_no
                            FROM siparis 
                            WHERE siparis_id = @siparis_id";

            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id),
                    new SqlParameter("@siparis_no",siparis_no),
            };
            #endregion

            return SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameters);
        }

        //siparis_notlar liste Metodu.
        public List<siparis_notlar> SiparisNotGetir(long siparis_id, ref string errorMessage)
        {
            List<siparis_notlar> siparisnotlar = null;

            #region Query
            string query = @"
                            SELECT siparis_id
                            ,sira_id
                            ,degistiren_carikart_id
                            ,degistiren_tarih
                            ,aciklama
                            FROM siparis_notlar where siparis_id=@siparis_id";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion
            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    siparis_notlar siparisnot = null;
                    siparisnotlar = new List<siparis_notlar>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        siparisnot = new siparis_notlar();
                        siparisnot.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                        siparisnot.sira_id = dt.Rows[i]["sira_id"].acekaToShort();
                        siparisnot.aciklama = dt.Rows[i]["aciklama"].ToString();
                        siparisnot.degistiren_carikart_id = Tools.PersonelId;
                        siparisnot.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        siparisnotlar.Add(siparisnot);
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            return siparisnotlar;
        }

        public List<siparis_onay_log> SiparisOnayLoglari(long siparis_id, CustomEnums.OnayLogTipi OnayLogTipi, ref string errorMessage)
        {
            List<siparis_onay_log> loglar = null;
            try
            {

                #region Query
                string query = @"
                            SELECT 
	                            SKOnayLog.siparis_id,
	                            SKOnayLog.onay_carikart_id,
                                SKOnayLog.onay_alan_adi,
	                            OC.cari_unvan as 'onaylayan_cari',
	                            SKOnayLog.onay_tarihi,
	                            SKOnayLog.iptal_carikart_id,
	                            IC.cari_unvan as 'iptal_eden_cari',
	                            SKOnayLog.iptal_tarihi
                            FROM siparis_onay_log AS SKOnayLog
                            LEFT JOIN carikart AS OC ON OC.carikart_id = SKOnayLog.onay_carikart_id
                            LEFT JOIN carikart AS IC ON IC.carikart_id = SKOnayLog.iptal_carikart_id
                            WHERE siparis_id = @siparis_id AND onay_alan_adi = '" + OnayLogTipi.ToString() + "'";
                //  AND SKOnayLog.iptal_tarihi IS NULL
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    loglar = new List<siparis_onay_log>();

                    siparis_onay_log log = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        log = new siparis_onay_log();
                        log.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();

                        log.onay_carikart_id = dt.Rows[i]["onay_carikart_id"].acekaToLongWithNullable();
                        log.onay_tarihi = dt.Rows[i]["onay_tarihi"].acekaToDateTime();
                        log.onay_alan_adi = dt.Rows[i]["onay_alan_adi"].ToString();
                        log.onaylayan_carikart = new cari_kart();
                        log.onaylayan_carikart.cari_unvan = dt.Rows[i]["onaylayan_cari"].acekaToString();
                        log.iptal_carikart_id = dt.Rows[i]["iptal_carikart_id"].acekaToLongWithNullable();
                        log.iptal_tarihi = dt.Rows[i]["iptal_tarihi"].acekaToDateTimeWithNullable();
                        log.iptal_eden_carikart = new cari_kart();
                        log.iptal_eden_carikart.cari_unvan = dt.Rows[i]["iptal_eden_cari"].acekaToString();
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

        public siparis_onay SiparisOnay(long siparis_id)
        {
            siparis_onay onay = null;

            #region Query
            string query = @"
                        SELECT 
                                *
                        FROM siparis_onay
                        WHERE siparis_id = @siparis_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                onay = new siparis_onay();
                onay.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                onay.genel_onay = dt.Rows[0]["genel_onay"].acekaToBool();
                onay.malzeme_onay = dt.Rows[0]["malzeme_onay"].acekaToBool();
                onay.yukleme_onay = dt.Rows[0]["yukleme_onay"].acekaToBool();
                onay.uretim_onay = dt.Rows[0]["uretim_onay"].acekaToBool();
                onay.dikim_onay = dt.Rows[0]["dikim_onay"].acekaToBool();

            }
            return onay;
        }


        public void SiparisDataOsturStokkartan(long siparis_id, long stokkart_id, long degistiren_carikart_id)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>
            {
                new SqlParameter("@siparis_id", siparis_id),
                new SqlParameter("@stokkart_id", stokkart_id),
                new SqlParameter("@degistiren_carikart_id",degistiren_carikart_id)
            };

            SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.StoredProcedure, "SP_SiparisDetailCreateFromStokkart", parameterList.ToArray());
        }

        #region İlk Madde Kumaş, İlk Madde Aksesuar, İlk Madde İplik

        public List<siparis_model> SiparisModelListesiniGetir(long siparis_id, byte stokkart_tipi_id, ref string errorMessage)
        {
            List<siparis_model> modeller = null;
            try
            {
                #region Query
                string query = @"
                                SELECT 
                                     SM.siparis_id, 
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
                                 FROM siparis_model SM
                                 INNER JOIN stokkart SK ON SK.stokkart_id = SM.alt_stokkart_id AND SK.stokkart_tipi_id = @stokkart_tipi_id
                                 WHERE SM.siparis_id = @siparis_id
                            ";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id),
                    new  SqlParameter("@stokkart_tipi_id",stokkart_tipi_id)
                };
                #endregion

                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    modeller = new List<siparis_model>();

                    siparis_model model = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        model = new siparis_model();
                        model.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
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

        public siparis_model SiparisModelDetayiniGetir(long siparis_id, short sira_id, short beden_id)
        {
            siparis_model model = null;

            #region Query
            string query = @"
                                SELECT
	                                SM.siparis_id, 
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
                                FROM [dbo].[siparis_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                                WHERE SM.siparis_id = @siparis_id AND SM.beden_id = @beden_id AND SM.sira_id = @sira_id 
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id),
                    new  SqlParameter("@beden_id",beden_id),
                    new  SqlParameter("@sira_id",sira_id)
                };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                model = new siparis_model();
                model.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
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

        public short SiparisModelEnBuyukSiraNo(long siparis_id, ref string errorMessage)
        {
            short result = 0;
            #region Query
            string query = @"
                                SELECT 
	                            ISNULL(MAX(SM.sira_id),0) as MaxSiraId
                                FROM [dbo].[siparis_model] SM
                                INNER JOIN stokkart SK ON SM.alt_stokkart_id = SK.stokkart_id
                                WHERE SM.siparis_id = @siparis_id
                            ";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id)
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
        #endregion

        #region Genel Sekmesi 
        /// <summary>
        /// summary_field = adet ya da birimfiyat
        /// </summary>
        /// <param name="siparis_id"></param>
        /// <param name="summary_field">"adet" ya da "birimfiyat"</param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public DataTable SiparisAdetFiyatGetir(long siparis_id, CustomEnums.SiparisDetayPivotType siparisDetayPivotType)
        {

            #region Query
            string query = @"dbo.SP_GetSiparisDetayPivot";
            #endregion
            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@siparis_id",siparis_id),
               new SqlParameter("@summary_field",siparisDetayPivotType.ToString())
            };
            #endregion
            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.StoredProcedure, query, parameters).Tables[0];
            return dt;
        }

        public DataTable SiparisPivotIlkMaddeListesiniGetir(long siparis_id, long stokkart_id, byte stokkart_tipi_id, ref string errorMessage)
        {
            try
            {
                #region Query
                string query = @"SP_GetSiparisIlkmaddePivot";
                #endregion

                #region Parameters
                SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id),
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

        public siparis_detay SiparisDetayGetir(long siparis_id, long stokkart_id, int beden_id)
        {
            siparis_detay model = new siparis_detay();

            #region Query
            string query = @"
                                SELECT siparis_id, stokkart_id, beden_id, degistiren_carikart_id, degistiren_tarih, adet, birimfiyat
                                FROM [dbo].[siparis_detay] sd
                                WHERE sd.siparis_id = @siparis_id AND sd.stokkart_id = @stokkart_id and sd.beden_id = @beden_id 
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id),
                    new  SqlParameter("@stokkart_id",stokkart_id),
                    new  SqlParameter("@beden_id",beden_id)
                };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                    model.stokkart_id = dt.Rows[i]["stokkart_id"].acekaToLong();
                    model.beden_id = dt.Rows[i]["beden_id"].acekaToShort();
                    model.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                    model.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                    model.adet = dt.Rows[i]["adet"].acekaToInt();
                    model.birimfiyat = dt.Rows[i]["birimfiyat"].acekaToDecimal();
                }
            }

            return model;
        }


        #endregion

        #region talimatlar
        public short SiparisTalimatSiraID_Kontrol(long stokkart_id, short sira_id, ref string errorMessage)
        {
            short result = 0;

            #region Query
            string query = @"
                            SELECT 
	                            Count(*)
                            FROM siparis_talimat
                            WHERE siparis_id = @siparis_id AND sira_id = @sira_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",stokkart_id),
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
        public short SiparisTalimatEnBuyukSiraNo(long siparis_id, ref string errorMessage)
        {
            short result = 0;

            #region Query
            string query = @"
                            SELECT 
                             MAX(sira_id) as 'MaxSiraId'
                            FROM siparis_talimat
                            WHERE siparis_id = @siparis_id
                            ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
                    new  SqlParameter("@siparis_id",siparis_id)
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

        public List<siparis_talimat> SiparisTalimatGetir(long siparis_id, ref string errorMessage)
        {
            List<siparis_talimat> talimatlar = null;

            #region Query
            string query = @"
                        SELECT 
                                st.siparis_id,
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
                        FROM siparis_talimat st
                        LEFT JOIN carikart c ON c.carikart_id=st.fasoncu_carikart_id
						INNER JOIN talimat t ON t.talimatturu_id=st.talimatturu_id and t.kayit_silindi=0
                        WHERE siparis_id = @siparis_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@siparis_id",siparis_id)
            };
            #endregion

            try
            {
                dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    talimatlar = new List<siparis_talimat>();
                    siparis_talimat siptalimat = null;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        siptalimat = new siparis_talimat();
                        siptalimat.siparis_id = dt.Rows[i]["siparis_id"].acekaToLong();
                        siptalimat.sira_id = dt.Rows[i]["sira_id"].acekaToByte();
                        siptalimat.degistiren_carikart_id = dt.Rows[i]["degistiren_carikart_id"].acekaToLong();
                        siptalimat.degistiren_tarih = dt.Rows[i]["degistiren_tarih"].acekaToDateTime();
                        siptalimat.talimatturu_id = dt.Rows[i]["talimatturu_id"].acekaToByte();
                        siptalimat.talimat_adi = dt.Rows[i]["tanim"].ToString();
                        siptalimat.fasoncu_carikart_id = dt.Rows[i]["fasoncu_carikart_id"].acekaToLongWithNullable();
                        siptalimat.aciklama = dt.Rows[i]["aciklama"].acekaToString();
                        siptalimat.irstalimat = dt.Rows[i]["irstalimat"].acekaToString();
                        siptalimat.islem_sayisi = dt.Rows[i]["islem_sayisi"].acekaToShortWithNullable();
                        siptalimat.cari_kart = new cari_kart();
                        siptalimat.cari_kart.cari_unvan = dt.Rows[i]["cari_unvan"].ToString();
                        talimatlar.Add(siptalimat);
                        siptalimat = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


            return talimatlar;
        }
        public siparis_talimat SiparisTalimatDetay(long siparis_id, short sira_id)
        {
            siparis_talimat talimat = null;

            #region Query
            string query = @"
                        SELECT 
                                siparis_id, 
                                sira_id, 
                                degistiren_carikart_id, 
                                degistiren_tarih, 
                                talimatturu_id, 
                                fasoncu_carikart_id, 
                                aciklama, 
                                irstalimat, 
                                islem_sayisi
                        FROM siparis_talimat
                        WHERE siparis_id = @siparis_id AND sira_id = @sira_id
                        ";
            #endregion

            #region Parameters
            SqlParameter[] parameters = new SqlParameter[] {
               new SqlParameter("@siparis_id",siparis_id),
               new SqlParameter("@sira_id",sira_id)
            };
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query, parameters).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                talimat = new siparis_talimat();
                talimat.siparis_id = dt.Rows[0]["siparis_id"].acekaToLong();
                talimat.sira_id = dt.Rows[0]["sira_id"].acekaToByte();
                talimat.degistiren_carikart_id = dt.Rows[0]["degistiren_carikart_id"].acekaToLong();
                talimat.degistiren_tarih = dt.Rows[0]["degistiren_tarih"].acekaToDateTime();
                talimat.talimatturu_id = dt.Rows[0]["talimatturu_id"].acekaToByte();
                talimat.fasoncu_carikart_id = dt.Rows[0]["fasoncu_carikart_id"].acekaToLongWithNullable();
                talimat.aciklama = dt.Rows[0]["aciklama"].acekaToString();
                talimat.irstalimat = dt.Rows[0]["irstalimat"].acekaToString();
                talimat.islem_sayisi = dt.Rows[0]["islem_sayisi"].acekaToShortWithNullable();
            }
            return talimat;
        }

       

        #endregion
    }
}
