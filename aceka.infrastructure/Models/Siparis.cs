using System;
using System.Collections.Generic;

namespace aceka.infrastructure.Models
{
    public class siparis
    {
        public long siparis_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public bool kayit_silindi { get; set; }
        public byte statu { get; set; }
        public string siparis_no { get; set; }
        public Nullable<short> sirket_id { get; set; }
        public long musteri_carikart_id { get; set; }
        public Nullable<long> stokyeri_carikart_id { get; set; }
        public System.DateTime siparis_tarihi { get; set; }
        public byte siparisturu_id { get; set; }
        public byte zorlukgrubu_id { get; set; }
        public Nullable<short> musterifazla { get; set; }
        public string siparis_not { get; set; }
        public Nullable<short> uretimyeri_id { get; set; }
        public Nullable<short> mense_uretimyeri_id { get; set; }
        public Nullable<byte> pb { get; set; }

        public List<siparis_ozel> siparis_ozel { get; set; }
        public parametre_siparisturu parametre_siparisturu { get; set; }
        public parametre_zorlukgrubu parametre_zorlukgrubu { get; set; }
        public cari_kart musteri_carikart { get; set; }
        public cari_kart stokyeri_carikart { get; set; }
        public siparis_detay siparis_detay { get; set; }
        public siparis_ozel siparisozel { get; set; }
        public stokkart stokkart { get; set; }
        public siparis_onay siparisonay { get; set; }
    }

    public class siparis_detay
    {
        public long siparis_id { get; set; }
        public long stokkart_id { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public int? adet { get; set; }
        public Nullable<decimal> birimfiyat { get; set; }
    }

    public class siparis_model
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public byte talimatturu_id { get; set; }
        public string modelyeri { get; set; }
        public long alt_stokkart_id { get; set; }
        public int renk_id { get; set; }
        public Nullable<byte> sira { get; set; }
        public byte ana_kayit { get; set; }
        public string aciklama { get; set; }
        public byte birim_id { get; set; }
        public Nullable<byte> birim_id3 { get; set; }
        public Nullable<float> miktar { get; set; }
        public Nullable<float> miktar3 { get; set; }
    }

    public class siparis_notlar
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public string aciklama { get; set; }
    }

    public class siparis_talimat
    {
        public long siparis_id { get; set; }
        public byte sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public byte talimatturu_id { get; set; }
        public Nullable<long> fasoncu_carikart_id { get; set; }
        public string aciklama { get; set; }
        public string irstalimat { get; set; }
        public Nullable<short> islem_sayisi { get; set; }
        //Sadece GET için kullanılacak.
        public string talimat_adi { get; set; }
        public string fasoncu_carikart_adi { get; set; }
        public cari_kart cari_kart { get; set; }
    }

    public class siparis_ozel_detay
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public long stokkart_id { get; set; }
        public short beden_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public System.DateTime degistiren_tarih { get; set; }
        public int adet { get; set; }
    }

    public class siparis_ozel
    {
        public long siparis_id { get; set; }
        public short sira_id { get; set; }
        public long degistiren_carikart_id { get; set; }
        public DateTime degistiren_tarih { get; set; }
        public Nullable<long> stokkart_id { get; set; }
        public Nullable<long> bayi_carikart_id { get; set; }
        public DateTime isteme_tarihi { get; set; }
        public Nullable<System.DateTime> tahmini_uretim_tarihi { get; set; }
        public Nullable<short> sezon_id { get; set; }
        public Nullable<System.DateTime> tahmini_dikim_tarihi { get; set; }
        public string ref_siparis_no { get; set; }
        public string ref_siparis_no2 { get; set; }
        public string ref_sistem_name { get; set; }
        public Nullable<byte> ref_link_status { get; set; }

        public stokkart stokkart { get; set; }
    }

    public class siparis_onay
    {
        public long siparis_id { get; set; }
        public bool genel_onay { get; set; }
        public bool malzeme_onay { get; set; }
        public bool yukleme_onay { get; set; }
        public bool uretim_onay { get; set; }
        public bool dikim_onay { get; set; }
    }

    public class siparis_onay_log
    {
        public long siparis_id { get; set; }
        public string onay_alan_adi { get; set; }
        public System.DateTime onay_tarihi { get; set; }
        public Nullable<long> onay_carikart_id { get; set; }
        public Nullable<System.DateTime> iptal_tarihi { get; set; }
        public Nullable<long> iptal_carikart_id { get; set; }

        public cari_kart onaylayan_carikart { get; set; }
        public cari_kart iptal_eden_carikart { get; set; }
    }
}
