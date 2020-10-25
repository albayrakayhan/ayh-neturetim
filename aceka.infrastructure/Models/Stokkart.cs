using System;
using System.Collections.Generic;

namespace aceka.infrastructure.Models
{
    public class stokkart
    {

        public long stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public int stokkart_tipi_id { get; set; }
        public short stokkart_tur_id { get; set; }
        public string stok_kodu { get; set; }
        public string stok_adi { get; set; }
        public Nullable<long> anastokkart_id { get; set; }
        public Nullable<long> anabarkod_id { get; set; }
        public Nullable<long> satici_carikart_id { get; set; }
        //public Nullable<int> stokalan_id_1 { get; set; }
        //public Nullable<int> stokalan_id_2 { get; set; }
        //public Nullable<int> stokalan_id_3 { get; set; }
        //public Nullable<int> stokalan_id_4 { get; set; }
        //public Nullable<int> stokalan_id_5 { get; set; }
        //public Nullable<int> stokalan_id_6 { get; set; }
        public Nullable<byte> kdv_alis_id { get; set; }
        public Nullable<byte> kdv_satis_id { get; set; }
        public byte birim_id_1 { get; set; }
        public Nullable<byte> birim_id_2 { get; set; }
        public Nullable<byte> birim_id_3 { get; set; }
        public bool birim_id_2_zorunlu { get; set; }
        public bool birim_id_3_zorunlu { get; set; }
        public Nullable<short> stokkart_sezon_id { get; set; }
        public Nullable<byte> tevkifat_alis { get; set; }
        public Nullable<byte> tevkifat_satis { get; set; }
        public Nullable<byte> maliyetlendirme_turu { get; set; }

        public stokkart_ozel stokkart_ozel { get; set; }
        public stokkart_talimat stokkart_talimat { get; set; }
        public talimat talimat { get; set; }
        public giz_sabit_stokkarttipi stokkarttipi { get; set; }
        public giz_sabit_stokkartturu stokkartturu { get; set; }
        public stokkart_onay_log stokkartonay_log { get; set; }
        public stokkart_onay stokkartonay { get; set; }
        public short uretimyeri_id { get; set; }

    }


    public class stokkart_ozel
    {
        public long stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public string stok_adi_uzun { get; set; }
        public string orjinal_stok_kodu { get; set; }
        public string orjinal_stok_adi { get; set; }
        public string orjinal_renk_kodu { get; set; }
        public string orjinal_renk_adi { get; set; }
        public Nullable<bool> tek_varyant { get; set; }
        public string gtipno { get; set; }
        public string urun_grubu { get; set; }
        public Nullable<double> birim1x { get; set; }
        public Nullable<double> birim2x { get; set; }
        public Nullable<double> birim3x { get; set; }
        public Nullable<double> M2_gram { get; set; }
        public Nullable<double> eni { get; set; }
        public Nullable<double> fyne { get; set; }
        public Nullable<double> fire_orani { get; set; }
        public Nullable<double> birim_gram { get; set; }
    }

    public class stokkart_talimat
    {

        public long stokkart_id { get; set; }
        public short sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int talimatturu_id { get; set; }
        public string talimat_adi { get; set; }
        public Nullable<long> fasoncu_carikart_id { get; set; }
        public string aciklama { get; set; }
        public string irstalimat { get; set; }
        public Nullable<short> islem_sayisi { get; set; }

        //Sadece GET için kullanılacak.      
        public string fasoncu_carikart_adi { get; set; }
        public cari_kart cari_kart { get; set; }
    }

    public class stokkart_rapor_parametre
    {

        public long stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }

        /// <summary>
        /// 01.03.2017
        /// CRUD işlemlerinde kullanılmayacak!
        /// </summary>
        public Nullable<long> satici_carikart_id { get; set; }

        /// <summary>
        /// 15.06.2017
        /// CRUD işlemlerinde kullanılmayacak!
        /// </summary>
        public string satici_cari_unvan { get; set; }


        public Nullable<int> stokalan_id_1 { get; set; }
        public Nullable<int> stokalan_id_2 { get; set; }
        public Nullable<int> stokalan_id_3 { get; set; }
        public Nullable<int> stokalan_id_4 { get; set; }
        public Nullable<int> stokalan_id_5 { get; set; }
        public Nullable<int> stokalan_id_6 { get; set; }
        public Nullable<int> stokalan_id_7 { get; set; }
        public Nullable<int> stokalan_id_8 { get; set; }
        public Nullable<int> stokalan_id_9 { get; set; }
        public Nullable<int> stokalan_id_10 { get; set; }
        public Nullable<int> stokalan_id_11 { get; set; }
        public Nullable<int> stokalan_id_12 { get; set; }
        public Nullable<int> stokalan_id_13 { get; set; }
        public Nullable<int> stokalan_id_14 { get; set; }
        public Nullable<int> stokalan_id_15 { get; set; }
        public Nullable<int> stokalan_id_16 { get; set; }
        public Nullable<int> stokalan_id_17 { get; set; }
        public Nullable<int> stokalan_id_18 { get; set; }
        public Nullable<int> stokalan_id_19 { get; set; }
        public Nullable<int> stokalan_id_20 { get; set; }

        public stokkart stokkart { get; set; }

    }

    public class stokkart_ekler
    {
        public long stokkart_id { get; set; }
        public int ek_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public List<ekler> ek_bilgiler { get; set; }
    }

    public class stokkart_fiyat
    {
        public long stokkart_id { get; set; }
        public string fiyattipi { get; set; }
        public DateTime tarih { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public decimal fiyat { get; set; }
        //Fiyat objesini yaratabilmek için alis_fiyati ve satis_fiyati eklendi.
        /// <summary>
        /// 13.04.2017
        /// alis_fiyati ve satis_fiyati CRUD işlemlerinde kullanılmayacak!
        /// </summary>
        public decimal alis_fiyati { get; set; }
        public decimal satis_fiyati { get; set; }

        public int pb { get; set; }
        //parametre_fiyattipi tablosundan fiyattipi_adını getirmek için kullanıldı.
        public string fiyattipi_adi { get; set; }
        //parametre_parabirimi tablosundan para birim adını getirmek için kullanıldı.
        public string pb_adi { get; set; }
        //parametre_parabirimi tablosundan para birim kodu getirmek için kullanıldı.
        public string pb_kodu { get; set; }
        public bool kayit_silindi { get; set; }
    }

    public partial class stokkart_fiyat_talimat
    {
        public long stokkart_id { get; set; }
        public System.DateTime tarih { get; set; }
        public long carikart_id { get; set; }
        public int talimatturu_id_1 { get; set; }
        public int talimatturu_id_2 { get; set; }
        public int talimatturu_id_3 { get; set; }
        public int talimatturu_id_4 { get; set; }
        public int talimatturu_id_5 { get; set; }
        public int talimatturu_id_6 { get; set; }
        public int talimatturu_id_7 { get; set; }
        public int talimatturu_id_8 { get; set; }
        public int talimatturu_id_9 { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public decimal fiyat { get; set; }
        public int pb { get; set; }

        public talimat talimat { get; set; }
        public parametre_parabirimi parabirimi { get; set; }
    }

    public class stokkart_sku
    {
        public long sku_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public bool statu { get; set; }
        public string sku_no { get; set; }
        public long stokkart_id { get; set; }
        public long renk_id { get; set; }
        public int beden_id { get; set; }
        public Nullable<bool> asorti { get; set; }
        public Nullable<double> asorti_miktar { get; set; }

        public parametre_renk renk { get; set; }
        public parametre_beden parametre_beden { get; set; }
        public List<parametre_beden> parametre_bedenler { get; set; }
    }

    public class giz_setup_varyant_oto
    {
        public short sku_oto_field_id { get; set; }
        public bool statu { get; set; }
        public string table_name { get; set; }
        public string field_name { get; set; }
        public string tanim { get; set; }
    }

    public class stokkart_kontrol
    {
        public long stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool tedarik_edilemez { get; set; }
        public bool musteri_siparisi_icin_acik { get; set; }
        public bool eksi_stok_izin { get; set; }
        public bool eksi_stok_uyari { get; set; }
        public bool min_stok_uyari { get; set; }
        public bool satin_alma_testi_gerekli_uyari { get; set; }
        public bool her_sezon_onay_gerekli { get; set; }
        public bool beden_bazli_kullanim { get; set; }
        public bool sezon_onayi_yok_uyarisi { get; set; }
    }

    public class stokkart_sezon
    {
        public long stokkart_id { get; set; }
        public short sezon_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public bool statu { get; set; }

        public parametre_sezon sezon { get; set; }

    }

    public class stokkart_muadil
    {
        public long stokkart_id { get; set; }
        public long muadil_stokkart_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool statu { get; set; }

        /// <summary>
        /// 22.02.2017 Adnan TÜRK. Post ve Put işlemlerinde kullanılmayacak.
        /// </summary>
        public string tanim { get; set; }
    }

    public class stokkart_onay
    {
        public long stokkart_id { get; set; }
        public bool genel_onay { get; set; }
        public bool malzeme_onay { get; set; }
        public bool yukleme_onay { get; set; }
        public bool uretim_onay { get; set; }
    }

    public class stokkart_onay_log
    {
        public long stokkart_id { get; set; }
        public string onay_alan_adi { get; set; }
        public DateTime onay_tarihi { get; set; }
        public Nullable<long> onay_carikart_id { get; set; }
        public Nullable<DateTime> iptal_tarihi { get; set; }
        public Nullable<long> iptal_carikart_id { get; set; }

        public cari_kart onaylayan_carikart { get; set; }
        public cari_kart iptal_eden_carikart { get; set; }
    }

    public class stokkart_sku_oto
    {
        public long stokkart_id { get; set; }
        public short sku_oto_field_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public byte sira_id { get; set; }

        /// <summary>
        /// 03.03.2017 AA. Aşağıdaki üç field GET için eklendi Post ve Put işlemlerinde kullanılmayacak.
        /// </summary>
        public stokkart_ozel stokkart_ozel { get; set; }
        public giz_setup_varyant_oto giz_setup_varyant_oto { get; set; }

        public bool secili { get; set; }
    }

    public class stokkart_model
    {
        public long stokkart_id { get; set; }
        public short sira_id { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public byte talimatturu_id { get; set; }
        public string modelyeri { get; set; }
        public long? alt_stokkart_id { get; set; }
        public int? renk_id { get; set; }
        public Nullable<byte> sira { get; set; }
        public byte ana_kayit { get; set; }
        public string aciklama { get; set; }
        public byte? birim_id { get; set; }
        public Nullable<byte> birim_id3 { get; set; }
        public Nullable<float> miktar { get; set; }
        public Nullable<float> miktar3 { get; set; }
    }

    public class stokkart_olcu
    {
        public int olcu_id { get; set; }
        public long stokkart_id { get; set; }
        public string olcuyeri { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public float deger { get; set; }
        public byte birim_id { get; set; }
        public parametre_beden parametrebeden { get; set; }
    }
    //Parametrelere alındı. AA.29.04.2017
    //public class parametre_uretimyeri
    //{
    //    public short uretimyeri_id { get; set; }
    //    public long degistiren_carikart_id { get; set; }
    //    public DateTime degistiren_tarih { get; set; }
    //    public bool kayit_silindi { get; set; }
    //    public string uretimyeri_kod { get; set; }
    //    public string uretimyeri_tanim { get; set; }
    //    public int? uretimyeri_rgb { get; set; }
    //}

}
