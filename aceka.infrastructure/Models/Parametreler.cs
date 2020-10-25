using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace aceka.infrastructure.Models
{
    public class giz_sabit_carikart_tipi
    {
        public byte carikart_tipi_id { get; set; }
        public byte carikart_turu_id { get; set; }
        public string carikart_tipi_adi { get; set; }
        public string aciklama { get; set; }
    }

    public class giz_sabit_carikart_turu
    {
        public byte carikart_turu_id { get; set; }
        public string carikart_turu_adi { get; set; }
        public string aciklama { get; set; }
    }

    public class parametre_banka
    {
        public short banka_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string banka_adi { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public string banka_eft_kodu { get; set; }
        public string banka_swift_kodu { get; set; }
        public List<parametre_banka_sube> subeler { get; set; }
    }

    public class parametre_banka_sube
    {
        public short banka_sube_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public short banka_id { get; set; }
        public string banka_sube_kodu { get; set; }
        public string banka_sube_adi { get; set; }
        public short ulke_id { get; set; }
        public Nullable<short> sehir_id { get; set; }
        public Nullable<short> ilce_id { get; set; }
        public Nullable<short> semt_id { get; set; }
        public string sube_dunya_kodu { get; set; }
        public string adres { get; set; }
        public string telefon1 { get; set; }
        public string telefon2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string yetkili_1_adi { get; set; }
        public string yetkili_1_tel { get; set; }
        public string yetkili_1_email { get; set; }
        public string yetkili_2_adi { get; set; }
        public string yetkili_2_tel { get; set; }
        public string yetkili_2_email { get; set; }
    }

    public class parametre_ulke
    {
        public short ulke_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string ulke_adi { get; set; }
        public bool statu { get; set; }
        public string ulke_dunya_kodu { get; set; }
        public string ulke_plaka_kodu { get; set; }
        public string ulke_telefon_kodu { get; set; }
        public string ulke_tipi { get; set; }
        public string ulke_adi_dil_1 { get; set; }
        public string ulke_adi_dil_2 { get; set; }
        public string ulke_adi_dil_3 { get; set; }
        public string ulke_adi_dil_4 { get; set; }
        public string ulke_adi_dil_5 { get; set; }
        public Nullable<short> sira { get; set; }
    }

    public class parametre_ulke_sehir
    {
        public int sehir_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public short ulke_id { get; set; }
        public string sehir_adi { get; set; }
        public string sehir_dunya_kodu { get; set; }
        public string sehir_telefon_kodu { get; set; }
        public string sehir_plaka_kodu { get; set; }
        public string sehir_adi_dil_1 { get; set; }
        public string sehir_adi_dil_2 { get; set; }
        public string sehir_adi_dil_3 { get; set; }
        public string sehir_adi_dil_4 { get; set; }
        public string sehir_adi_dil_5 { get; set; }
        public Nullable<short> sira { get; set; }
        public Nullable<double> ups_id { get; set; }
    }

    public class parametre_ulke_sehir_ilce
    {
        public int ilce_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public short ulke_id { get; set; }
        public short sehir_id { get; set; }
        public string ilce_adi { get; set; }
        public string ilce_adi_dil_1 { get; set; }
        public string ilce_adi_dil_2 { get; set; }
        public string ilce_adi_dil_3 { get; set; }
        public string ilce_adi_dil_4 { get; set; }
        public string ilce_adi_dil_5 { get; set; }
        public Nullable<short> sira { get; set; }
        public Nullable<double> ups_id { get; set; }
    }

    public class parametre_ulke_sehir_ilce_semt
    {
        public int semt_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public byte kayit_silindi { get; set; }
        public short ulke_id { get; set; }
        public short sehir_id { get; set; }
        public short ilce_id { get; set; }
        public string semt_adi { get; set; }
        public string posta_kodu { get; set; }
        public string semt_adi_dil_1 { get; set; }
        public string semt_adi_dil_2 { get; set; }
        public string semt_adi_dil_3 { get; set; }
        public string semt_adi_dil_4 { get; set; }
        public string semt_adi_dil_5 { get; set; }
        public Nullable<short> sira { get; set; }
    }

    public class parametre_uretimyeri
    {
        public short uretimyeri_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string uretimyeri_kod { get; set; }
        public string uretimyeri_tanim { get; set; }
        public Nullable<int> uretimyeri_rgb { get; set; }
    }

    public partial class parametre_uretimyeri_carikart
    {
        public short uretimyeri_id { get; set; }
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public Nullable<byte> oncelik_sira { get; set; }
        public bool varsayilan { get; set; }
        public string made_in { get; set; }
        public string kod1 { get; set; }
        public string kod2 { get; set; }
        public string kod3 { get; set; }
    }

    public class parametre_kalite2_tur
    {
        public int kalite2tur_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public byte sira { get; set; }
        public byte numune { get; set; }
    }

    public class parametre_birim
    {
        public int birim_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string birim_adi { get; set; }
        public string birim_kod { get; set; }
        public byte ondalik { get; set; }
        public string birim_adi_dil_1 { get; set; }
        public string birim_adi_dil_2 { get; set; }
        public string birim_adi_dil_3 { get; set; }
        public string birim_adi_dil_4 { get; set; }
        public string birim_adi_dil_5 { get; set; }
    }

    public class parametre_genel
    {
        public int parametre_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string parametre_grup_id { get; set; }
        public string parametre_kodu { get; set; }
        public string parametre_adi { get; set; }
        public string deger_metin1 { get; set; }
        public string deger_metin2 { get; set; }
        public string deger_metin3 { get; set; }
        public Nullable<long> deger_sayi1 { get; set; }
        public Nullable<long> deger_sayi2 { get; set; }
        public Nullable<long> deger_sayi3 { get; set; }
        public string parametre_adi_dil1 { get; set; }
        public string parametre_adi_dil2 { get; set; }
        public string parametre_adi_dil3 { get; set; }
        public string parametre_adi_dil4 { get; set; }
        public string parametre_adi_dil5 { get; set; }
    }

    public class parametre_carikart_rapor
    {
        public int parametre_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public byte parametre { get; set; }
        public int kaynak_1_parametre_id { get; set; }
        public int kaynak_2_parametre_id { get; set; }
        public int kaynak_3_parametre_id { get; set; }
        public int kaynak_4_parametre_id { get; set; }
        public string kod { get; set; }
        public string tanim { get; set; }
        public string grup1 { get; set; }
        public string grup2 { get; set; }
        public int? sira { get; set; }
        public byte parametre_grubu { get; set; }
        public string dil_1_tanim { get; set; }
        public string dil_2_tanim { get; set; }
        public string dil_3_tanim { get; set; }
        public string dil_4_tanim { get; set; }
        public string dil_5_tanim { get; set; }
    }

    public class parametre_cari_odeme_sekli
    {
        public byte cari_odeme_sekli_id { get; set; }
        public Nullable<long> degistiren_carikart_id { get; set; }
        public Nullable<System.DateTime> degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string cari_odeme_sekli { get; set; }
    }

    public class parametre_parabirimi
    {
        public byte pb { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string pb_kodu { get; set; }
        public string pb_adi { get; set; }
        public Nullable<short> ulke_id { get; set; }
        public string merkezbankasi_kodu { get; set; }
        public Nullable<byte> sira { get; set; }
        public string kusurat_tanimi { get; set; }
        public string pr_kodu { get; set; }
    }

    public class parametre_fiyattipi
    {
        public string fiyattipi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string fiyattipi_adi { get; set; }
        public string fiyattipi_turu { get; set; }
        public bool kdv_dahil { get; set; }
        public Nullable<byte> sira { get; set; }
        public bool kullanici_giris { get; set; }

        //carikart_fiyat_tipi deki değerleri getirmek için kullanılıdı.
        public bool varsayilan { get; set; }
        public bool statu { get; set; }
    }

    public class parametre_ozel_alan
    {
        public long ozel_alan_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string tablo_adi { get; set; }
        public string tablo_kolon_adi { get; set; }
        public string kod { get; set; }
        public string tanim { get; set; }
    }

    public class parametre_renk
    {
        public long renk_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string renk_kodu { get; set; }
        public string renk_adi { get; set; }
        //public Nullable<int> renk_rgb { get; set; } Eski hali.
        public Nullable<int> renk_rgb { get; set; }
        public string renk_kodu2 { get; set; }
        public Nullable<bool> siparis_ozel { get; set; }
        public Nullable<byte> stokkart_parametre_grubu { get; set; }
        public Nullable<byte> stokkart_tipi_id { get; set; }
        public Nullable<int> stokalan_id_1 { get; set; }
        public string renk_kodu_dil_1 { get; set; }
        public string renk_adi_dil_1 { get; set; }
        public string renk_kodu_dil_2 { get; set; }
        public string renk_adi_dil_2 { get; set; }
        public string renk_kodu_dil_3 { get; set; }
        public string renk_adi_dil_3 { get; set; }
        public string renk_kodu_dil_4 { get; set; }
        public string renk_adi_dil_4 { get; set; }
        public string renk_kodu_dil_5 { get; set; }
        public string renk_adi_dil_5 { get; set; }
    }

    public class parametre_sezon
    {
        public short sezon_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string sezon_kodu { get; set; }
        public string sezon_adi { get; set; }
        public Nullable<bool> mal_kabul { get; set; }
        public Nullable<DateTime> mal_kabul_baslama { get; set; }
        public Nullable<DateTime> mal_kabul_bitis { get; set; }
        public Nullable<bool> satis { get; set; }
        public Nullable<DateTime> satis_baslama { get; set; }
        public Nullable<DateTime> satis_bitis { get; set; }
    }

    public class parametre_ulke_vergi_daireleri
    {
        public short ulke_id { get; set; }
        public string vergi_daire_no { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string vergi_daire_adi { get; set; }
    }

    public class parametre_siparisturu
    {
        public byte siparisturu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string siparisturu_tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
        public bool genel { get; set; }
        public bool kalite2 { get; set; }
        public bool numune { get; set; }
    }

    public partial class parametre_tedariksekli
    {
        public int tedariksekli_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
    }

    public partial class parametre_tedariktipi
    {
        public int tedariktipi_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
        public bool satinalmagecikme_eposta { get; set; }
    }

    public class parametre_zorlukgrubu
    {
        public int zorlukgrubu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string tanim { get; set; }
        public bool varsayilan { get; set; }
        public int sira { get; set; }
    }

    public class planlama_zorlukgrubu
    {
        public byte zorlukgrubu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<short> kesimfire { get; set; }
        public Nullable<short> kesimfazla { get; set; }
        public Nullable<short> musterifazla { get; set; }
        public byte bedenbazinda { get; set; }
    }
    public class planlama_zorlukgrubu_oranlari
    {
        public byte zorlukgrubu_id { get; set; }
        public short sira { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public int altseviye { get; set; }
        public int ustseviye { get; set; }
        public short kesimfire { get; set; }
        public short kesimfazla { get; set; }
    }
    public class kalite_kontrol_oranlari
    {
        public string kalite_kontrol_kod { get; set; }
        public byte stokkart_tipi_id { get; set; }
        public List<giz_sabit_stokkarttipi> stokkart_tipleri { get; set; }
        public double adet { get; set; }
        public int sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public Nullable<double> kontrol_miktar { get; set; }
        public Nullable<double> red_miktar { get; set; }
        public bool miktarlar_oran_mi { get; set; }
    }
    public partial class giz_setup
    {
        public string ayaradi { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public string ayar { get; set; }
        public string ayaraciklama { get; set; }
        public string ayar_grubu { get; set; }
        public string veri_turu { get; set; }
        public string combo_sql { get; set; }
        public List<giz_setup_combo> combo_degeri { get; set; }

    }
    public class giz_setup_combo
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    public class parametre_stokkart_rapor
    {
        public int parametre_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public byte parametre { get; set; }
        public byte parametre_grubu { get; set; }
        public int kaynak_1_parametre_id { get; set; }
        public int kaynak_2_parametre_id { get; set; }
        public int kaynak_3_parametre_id { get; set; }
        public int kaynak_4_parametre_id { get; set; }
        public string kod { get; set; }
        public string tanim { get; set; }
        public string dil_1_tanim { get; set; }
        public string dil_2_tanim { get; set; }
        public string dil_3_tanim { get; set; }
        public string dil_4_tanim { get; set; }
        public string dil_5_tanim { get; set; }
        public Nullable<int> sira { get; set; }
        public Nullable<int> renk_rgb { get; set; }
        public string kod1 { get; set; }
        public string kod2 { get; set; }
        public string kod3 { get; set; }
        public string kod4 { get; set; }
        public string kod5 { get; set; }
        public string kod6 { get; set; }
        public Nullable<double> deger1 { get; set; }
        public Nullable<double> deger2 { get; set; }

        //giz_setup_stokkart_parametre tablosundan parametre_adi getirmek için kullanıldı.
        //Adnan TÜRK 01.03.2017
        public string parametre_adi { get; set; }
    }

    public class parametre_kdv
    {
        public byte kod { get; set; }
        public System.DateTime tarih { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public long oran { get; set; }
        //public string giz_yazilim_kodu { get; set; }
    }

    public class giz_sabit_stokkarttipi
    {
        public byte stokkarttipi { get; set; }
        public string tanim { get; set; }
        public string otostokkodu { get; set; }
        public byte parametre_grubu { get; set; }
        public byte stokkartturu { get; set; }
    }

    public class giz_sabit_stokkartturu
    {
        public byte stokkartturu { get; set; }
        public string tanim { get; set; }
    }

    public class talimat
    {
        public int talimatturu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string kod { get; set; }
        public bool varsayilan { get; set; }
        public string tanim { get; set; }
        public string tanim_dil1 { get; set; }
        public string tanim_dil2 { get; set; }
        public string tanim_dil3 { get; set; }
        public string tanim_dil4 { get; set; }
        public string tanim_dil5 { get; set; }
        public int sira { get; set; }
        public Nullable<int> renk_rgb { get; set; }
        public bool kesim { get; set; }
        public bool dikim { get; set; }
        public bool parca { get; set; }
        public bool model { get; set; }
        public byte stokkart_tipi_id { get; set; }
        public bool onayoto { get; set; }
        public bool parcamodel_giris { get; set; }
        public bool parcamodel_cikis { get; set; }
        public bool model_zorunlu { get; set; }
        public Nullable<long> varsayilan_fasoncu { get; set; }
        public Nullable<byte> kdv_tevkifat { get; set; }
    }

    public class ekler
    {
        public int ek_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public short ekturu_id { get; set; }
        public string ekadi { get; set; }
        public string aciklama { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public giz_setup_ekturu ekturu_bilgileri { get; set; }
    }

    public class giz_setup_ekturu
    {
        public short ekturu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool statu { get; set; }
        public byte stokkart_tipi_id { get; set; }
        public string tanim { get; set; }
        public string file_ext { get; set; }
        public string file_types { get; set; }
        public bool preview { get; set; }
    }
    public partial class parametre_beden
    {
        public int beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string bedengrubu { get; set; }
        public string beden { get; set; }
        public string beden_tanimi { get; set; }
        public int sira { get; set; }
    }
    public partial class parametre_beden_carikart
    {
        public byte sira_id { get; set; }
        public int beden_id { get; set; }
        public long carikart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public string bedenkodu { get; set; }
    }

    public class giz_setup_carikart_parametre
    {
        public byte parametre_grubu { get; set; }
        public byte parametre { get; set; }
        public Nullable<long> degistiren_carikart_id { get; set; }
        public Nullable<System.DateTime> degistiren_tarih { get; set; }
        public string parametre_adi { get; set; }
        public Nullable<byte> ust_parametre { get; set; }
        public Nullable<bool> zorunlu { get; set; }
        public Nullable<byte> ust_1 { get; set; }
        public Nullable<byte> ust_2 { get; set; }
        public Nullable<byte> ust_3 { get; set; }
        public string parametre_adi1 { get; set; }
        public Nullable<byte> ust_parametre1 { get; set; }
        public Nullable<byte> ust_11 { get; set; }
        public Nullable<byte> ust_21 { get; set; }
        public Nullable<byte> ust_31 { get; set; }
        public string parametre_adi2 { get; set; }
        public Nullable<byte> ust_parametre2 { get; set; }
        public Nullable<byte> ust_12 { get; set; }
        public Nullable<byte> ust_22 { get; set; }
        public Nullable<byte> ust_32 { get; set; }
        public string parametre_adi3 { get; set; }
        public Nullable<byte> ust_parametre3 { get; set; }
        public Nullable<byte> ust_13 { get; set; }
        public Nullable<byte> ust_23 { get; set; }
        public Nullable<byte> ust_33 { get; set; }
    }

    public class cari_parametreler
    {
        public int parametre_grubu { get; set; }
        public int parametre { get; set; }
        public string label { get; set; }
        public int parametre_id { get; set; }
        public string tanim { get; set; }
        public int kaynak1_parametre_id { get; set; }

    }


    public class talimattanim
    {
        public int talimatturu_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string kod { get; set; }
        public bool varsayilan { get; set; }
        public string tanim { get; set; }
        public string tanim_dil1 { get; set; }
        public string tanim_dil2 { get; set; }
        public string tanim_dil3 { get; set; }
        public string tanim_dil4 { get; set; }
        public string tanim_dil5 { get; set; }
        public int sira { get; set; }
        public string renk_rgb { get; set; }
        public bool kesim { get; set; }
        public bool dikim { get; set; }
        public bool parca { get; set; }
        public bool model { get; set; }
        public byte stokkart_tipi_id { get; set; }
        public List<giz_sabit_stokkarttipi> stokkart_tipleri { get; set; }
        public bool onayoto { get; set; }
        public bool parcamodel_giris { get; set; }
        public bool parcamodel_cikis { get; set; }
        public bool model_zorunlu { get; set; }
        public string cari_unvan { get; set; }
        public Nullable<long> varsayilan_fasoncu { get; set; }
        public Nullable<byte> kdv_tevkifat { get; set; }
    }

}
