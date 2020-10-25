using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class PersonelAramaParameters
    {
        public string tanim { get; set; }
        public long personel_no { get; set; }
        public string personel_kodu { get; set; }
        public int personel_tipi { get; set; }
        public Nullable<bool> statu { get; set; }
    }
    public class Personel
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public Int64 kayit_yeri { get; set; }
        public Nullable<bool> statu { get; set; }
        public string statu_adi { get; set; }

        #region carikart tip ve türleri
        public int carikart_turu_id { get; set; }
        public string carikart_turu_adi { get; set; }
        public int carikart_tipi_id { get; set; }
        public string carikart_tipi_adi { get; set; }
        #endregion  carikart tip ve türleri
        public string cari_unvan { get; set; }
        public string ozel_kod { get; set; }
        public string fiyattipi { get; set; }
        public Int16 giz_yazilim_kodu { get; set; }


        public Int64 sube_carikart_id { get; set; }
        public Int64 transfer_depo_id { get; set; }
        public string giz_kullanici_adi { get; set; }
        public string giz_kullanici_sifre { get; set; }
        public int cari_parametre_1 { get; set; }
        public int cari_parametre_2 { get; set; }
        public int cari_parametre_3 { get; set; }
        public int cari_parametre_4 { get; set; }
        public int cari_parametre_5 { get; set; }
        public int cari_parametre_6 { get; set; }
        public int cari_parametre_7 { get; set; }
        public carikart_muhasebe_personel muh_masraf { get; set; }
        public carikart_genel_kimlik personel_kimlik { get; set; }
        
    }

    public class Personel_Parametreler
    {
        public long carikart_id { get; set; }
        public int cari_parametre_1 { get; set; }
        public int cari_parametre_2 { get; set; }
        public int cari_parametre_3 { get; set; }
        public int cari_parametre_4 { get; set; }
        public int cari_parametre_5 { get; set; }
        public int cari_parametre_6 { get; set; }
        public int cari_parametre_7 { get; set; }
        public string cari_parametre_1_tanim { get; set; }
        public string cari_parametre_2_tanim { get; set; }
        public string cari_parametre_3_tanim { get; set; }
        public string cari_parametre_4_tanim { get; set; }
        public string cari_parametre_5_tanim { get; set; }
        public string cari_parametre_6_tanim { get; set; }
        public string cari_parametre_7_tanim { get; set; }
        
    }
    public class carikart_personel
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public string cep_telefonu { get; set; }
        public string email { get; set; }
        public string resim_path { get; set; }

        #region personel sube
        public Int64 personel_sube_id { get; set; }
        public string personel_sube_adi { get; set; }
        #endregion personel sube

        public int personel_departman_id { get; set; }
        public int personel_gorev_id { get; set; }
        public Int64 personel_bagli_yonetici_carikart_id { get; set; }
        public int ogrenim_durumu_id { get; set; }
        public int askerlik_durumu_id { get; set; }
        public int meslek_id { get; set; }
        public int ehliyet_sinifi_id { get; set; }
    }

    public class carikart_muhasebe_personel
    {
        public Int64 carikart_id_m { get; set; }

        #region şirket
        public int sirket_id { get; set; }
        public string sirket_adi { get; set; }
        #endregion şirket
        public int sene { get; set; }

        #region muhasebe kodu 
        public string muh_kod { get; set; }
        public string muh_kod_adi { get; set; }
        #endregion muhasebe kodu 

        #region masraf merkezi
        public int masraf_merkezi_id { get; set; }
        public string masraf_merkezi_adi { get; set; }
        #endregion masraf merkezi
    }

    public class carikart_personel_calisma_yerleri
    {
        public long carikart_id { get; set; }
        public long stokyeri_carikart_id { get; set; }
        public string stokyeri_carikart_adi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int gorev_id { get; set; }
        public string gorev_adi { get; set; }
        public int departman_id { get; set; }
        public string departman_adi { get; set; }
        public List<parametre_genel> departmanlar { get; set; }
        public List<parametre_genel> gorevler { get; set; }
    }

    public class carikart_genel_kimlik
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int kimlik_tipi { get; set; }
        public string tc_kimlik_no { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public string cinsiyet { get; set; }
        public DateTime dogum_tarihi { get; set; }
        public string dogum_yeri { get; set; }
        public string kan_grubu { get; set; }
        public string kimlik_seri { get; set; }
        public string seri_no { get; set; }
        public string sira_no { get; set; }
        public string baba_adi { get; set; }
        public string anne_adi { get; set; }
        public string anne_kizlik_soyadi { get; set; }
        public string kizlik_soyadi { get; set; }
        public string dini { get; set; }
        public string medeni_durum { get; set; }
        public Nullable<short> uyruk { get; set; }
        public Nullable<short> ulke { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public string mahalle { get; set; }
        public string cilt_no { get; set; }
        public string aile_sira_no { get; set; }
        public string cilt_sira_no { get; set; }
        public string verilis_yeri { get; set; }
        public DateTime verilis_tarihi { get; set; }
        public string verilis_nedeni { get; set; }
        public DateTime evlilik_tarihi { get; set; }
    }
}
