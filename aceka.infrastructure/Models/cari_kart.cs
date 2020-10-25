using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class cari_kart
    {
        public Int64 carikart_id { get; set; }
        public Int64 degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public Int64 kayit_yeri { get; set; }
        public bool statu { get; set; }
        public byte carikart_turu_id { get; set; }
        public byte carikart_tipi_id { get; set; }
        public string cari_unvan { get; set; }
        public string ozel_kod { get; set; }
        public string fiyattipi { get; set; }
        public string fiyattipi_adi { get; set; }
        public Int16 giz_yazilim_kodu { get; set; }
        public Int64 sube_carikart_id { get; set; }
        //sube_carikart_id Adını getirmek için eklendi.
        public string sube_cari_unvan { get; set; }

        public Int64 ana_carikart_id { get; set; }
        /// Adnan TÜRK 20.01.2017
        /// <summary>
        /// "ana_cari_unvan" alanı Tabloya dahil değil! Sorgu için eklendi. 
        /// CrudRepository işlemlerinde dahil edilmemeli.
        /// </summary>
        public string ana_cari_unvan { get; set; }

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

        public List<carikart_genel_adres> carikart_genel_adres { get; set; }
        public carikart_finans carikart_finans { get; set; }
        public carikart_firma_ozel carikart_firma_ozel { get; set; }

        /// <summary>
        /// Parametreler
        /// </summary>
        public giz_sabit_carikart_tipi giz_sabit_carikart_tipi { get; set; }
        public giz_sabit_carikart_turu giz_sabit_carikart_turu { get; set; }
        public carikart_earsiv carikart_earsiv { get; set; }
        public carikart_efatura carikart_efatura { get; set; }
        public carikart_muhasebe carikart_muhasebe { get; set; }
        public muhasebe_tanim_masrafmerkezleri muhasebe_tanim_masrafmerkezleri { get; set; }
        public carikart_stokyeri carikart_stokyeri { get; set; }
        public giz_sirket giz_sirket { get; set; }
        public parametre_parabirimi parametre_parabirimi { get; set; }
    }

    public class carikart_genel_adres
    {
        public long carikart_adres_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string adres_tipi_id { get; set; }
        public long carikart_id { get; set; }
        public string adrestanim { get; set; }
        public string adresunvan { get; set; }
        public string adres { get; set; }
        public string postakodu { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public Nullable<int> semt_id { get; set; }
        public string vergidairesi { get; set; }
        public string vergino { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string websitesi { get; set; }
        public string yetkili_ad_soyad { get; set; }
        public string yetkili_tel { get; set; }
        public bool faturaadresi { get; set; }

        /// <summary>
        /// Adnan TÜRK. 23.01.2017
        /// Model dönebilmek için eklendi.
        /// </summary>
        public parametre_ulke ulke { get; set; }
        public parametre_ulke_sehir ulke_sehir { get; set; }
        public parametre_ulke_sehir_ilce ulke_sehir_ilce { get; set; }
        public parametre_ulke_sehir_ilce_semt ulke_sehir_ilce_semt { get; set; }
    }

    public class carikart_elektronik_bilgilendirme
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public string irsaliye_eposta { get; set; }
        public string perakende_fatura_eposta { get; set; }
        public string toptan_fatura_eposta { get; set; }
        public string siparis_formu_eposta { get; set; }
        public string babs_formu_eposta { get; set; }
        public string cari_mutabakat_formu_eposta { get; set; }
        public string odeme_hatirlatma_eposta { get; set; }
    }

    public class carikart_finans
    {
        public Int64 carikart_id { get; set; }
        public Int64 degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public string tckimlikno { get; set; }
        public string vergi_no { get; set; }
        public string vergi_dairesi { get; set; }
        public bool yabanci_uyruklu { get; set; }
        public string diger_kod { get; set; }
        public string pb { get; set; }

        public Nullable<decimal> vade_alis { get; set; }
        public Nullable<decimal> vade_satis { get; set; }
        public Nullable<decimal> iskonto_alis { get; set; }
        public Nullable<decimal> iskonto_satis { get; set; }

        public byte odeme_tipi { get; set; }
        public byte kur_farki { get; set; }
        public byte odeme_listesinde_cikmasin { get; set; }
        public byte alacak_listesinde_cikmasin { get; set; }
        public byte ticari_islem_grubu { get; set; }
        public long ilgili_sube_carikart_id { get; set; }
        public int odeme_plani_id { get; set; }
        /// <summary>
        /// ilgili_sube_cari_unvan
        /// Ekleyen Adnan TÜRK. 22.01.2017
        /// Not: Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string ilgili_sube_cari_unvan { get; set; }

        public long finans_sorumlu_carikart_id { get; set; }
        /// <summary>
        /// finans_sorumlu_cari_unvan
        /// Ekleyen Adnan TÜRK. 20.01.2017
        /// Not: Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string finans_sorumlu_cari_unvan { get; set; }
        public string swift_kodu { get; set; }
        public int tedarik_gunu { get; set; }
        public bool cari_hesapta_ciksin { get; set; }

        public carikart_muhasebe carikart_muhasebe { get; set; }
        public finans_tanim_odemeplani odeme_plani { get; set; }
        public carikart_fiyat_tipi carikart_fiyat_tipi { get; set; }
        public cari_kart carikart { get; set; }
    }

    public class carikart_firma_ozel
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public long? satin_alma_sorumlu_carikart_id { get; set; }

        /// <summary>
        /// satin_alma_sorumlu_unvan
        /// Ekleyen Adnan TÜRK. 20.01.2017
        /// Not:  Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string satin_alma_sorumlu_cari_unvan { get; set; }

        public long? satis_sorumlu_carikart_id { get; set; }

        /// <summary>
        /// satis_sorumlu_unvan
        /// Ekleyen Adnan TÜRK. 20.01.2017
        /// Not: Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string satis_sorumlu_cari_unvan { get; set; }
        public DateTime baslamatarihi { get; set; }
        public string ozel { get; set; }
    }

    public class carikart_finans_banka_hesaplari
    {
        public long carikart_banka_id { get; set; }
        public bool kayit_silindi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public long carikart_id { get; set; }
        public Nullable<short> banka_id { get; set; }
        public Nullable<short> banka_sube_id { get; set; }
        public string ibanno { get; set; }
        public string pb { get; set; }
        public Nullable<bool> ebanka { get; set; }
        public Nullable<bool> odemehesabi { get; set; }
        public Nullable<decimal> kredi_limiti_dbs { get; set; }

        public parametre_banka banka { get; set; }
    }

    public class carikart_genel_notlar
    {
        public long carikart_not_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public long carikart_id { get; set; }
        public string aciklama { get; set; }
        public string nereden { get; set; }
    }

    public class carikart_muhasebe
    {
        public long carikart_id { get; set; }
        public byte sirket_id { get; set; }
        public int sene { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public string muh_kod { get; set; }
        public Nullable<short> masraf_merkezi_id { get; set; }
        /// <summary>
        /// masraf_merkezi_adi
        /// Ekleyen Adnan TÜRK. 22.01.2017
        /// Not: Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string masraf_merkezi_adi { get; set; }

    }

    public class carikart_earsiv
    {
        public long carikart_id { get; set; }
        public int sene { get; set; }
        public string earsiv_seri { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
    }

    public class carikart_efatura
    {
        public long carikart_id { get; set; }
        public string efatura_seri { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
    }

    public class carikart_stokyeri
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<System.DateTime> acilis_tarihi { get; set; }
        public Nullable<System.DateTime> kapanis_tarihi { get; set; }
        public Nullable<bool> kapali { get; set; }
        public Nullable<bool> transfer_depo_kullan { get; set; }
        
    }

    public class muhasebe_tanim_masrafmerkezleri
    {
        public int masraf_merkezi_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public byte sirket_id { get; set; }
        public Nullable<int> ana_masraf_merkezi_id { get; set; }
        public string masraf_merkezi_kodu { get; set; }
        public string masraf_merkezi_adi { get; set; }
        public bool statu { get; set; }
        public string muhkod_ek { get; set; }
        public string grup_kodu { get; set; }
        public string grup_adi { get; set; }
        public bool kayit_silindi { get; set; }
        public Nullable<long> personel_carikart_id_1 { get; set; }
        public Nullable<long> stokyeri_carikart_id { get; set; }
    }

    public class carikart_fiyat_tipi
    {
        public long carikart_id { get; set; }
        public string fiyattipi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public bool varsayilan { get; set; }

        /// <summary>
        /// parametre_fiyattipi tablosundaki fiyat tipi için ekledik
        /// Ekleyen Ayhan ALBAYRAK. 08.02.2017
        /// Not:  Sorgu sonucunda gerekli bir alan.
        /// </summary>
        public string fiyattipi_adi { get; set; }
    }

    public partial class giz_sirket
    {
        public byte sirket_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string sirket_adi { get; set; }
        public string sirket_unvan { get; set; }
        public Nullable<int> sirket_tipi { get; set; }
        public string vergi_dairesi { get; set; }
        public string vergi_no { get; set; }
        public string ticaret_sicil_no { get; set; }
        public string sgk_no { get; set; }
        public string mersis_no { get; set; }
        public string adres { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public Nullable<int> semt_id { get; set; }
        public string posta_kodu { get; set; }
        public string web_sitesi { get; set; }
        public string email { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string tel3 { get; set; }
        public string ceptel { get; set; }
        public string fax { get; set; }
        public string ozel1 { get; set; }
        public string ozel2 { get; set; }
        public string ozel3 { get; set; }
        public string ozel4 { get; set; }
        public string ozel5 { get; set; }
        public byte[] logo_image { get; set; }
        public string logo_path { get; set; }
    }

    public class carikart_rapor_parametre
    {
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public Nullable<int> cari_parametre_1 { get; set; }
        public Nullable<int> cari_parametre_2 { get; set; }
        public Nullable<int> cari_parametre_3 { get; set; }
        public Nullable<int> cari_parametre_4 { get; set; }
        public Nullable<int> cari_parametre_5 { get; set; }
        public Nullable<int> cari_parametre_6 { get; set; }
        public Nullable<int> cari_parametre_7 { get; set; }
    }

    public class carikart_denetim_aksesuar
    {
        public long carikart_id { get; set; }
        public byte tip { get; set; }
        public short sira { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public long aksesuarkart_id { get; set; }
        public int renk_id { get; set; }
        public Nullable<double> miktar { get; set; }
        public string kosul { get; set; }
        
        public stokkart stokkart { get; set; }
        public stokkart_ozel stokkart_ozel { get; set; }
        public parametre_birim parametre_birim { get; set; }
        public parametre_renk parametre_renk { get; set; }
    }

    public partial class carikart_denetim_aksesuar_kosullar
    {
        public byte tip { get; set; }
        public string grup_adi { get; set; }
        public byte sira { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public string param_tanim { get; set; }
        public string param_field_name { get; set; }
        public Nullable<bool> direkt_kosul { get; set; }
        public string operator_liste { get; set; }
        public string cevap_liste_sql { get; set; }
    }
    public partial class carikart_denetim_yukleme
    {
        public long carikart_id { get; set; }
        public byte tip { get; set; }
        public short sira { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<bool> kalite1fazla_kabul { get; set; }
        public Nullable<bool> kalite1eksik_kabul { get; set; }
        public Nullable<bool> kalite2_kabul { get; set; }
        public string uyarinot { get; set; }
        public string kosul { get; set; }
    }
}





























