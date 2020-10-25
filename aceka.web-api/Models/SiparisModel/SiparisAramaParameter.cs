using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.SiparisModel
{
    /// <summary>
    /// Sipariş Arama İşlemleri için gerekli olan POST parametreleri
    /// </summary>
    public class SiparisAramaParameter
    {
        /// <summary>
        /// Sipariş Kimlik
        /// </summary>
        public long siparis_id { get; set; }
        /// <summary>
        /// Sipariş Numarası
        /// </summary>
        public string siparis_no { get; set; }

        /// <summary>
        /// Müşteri Bilgisi
        /// </summary>
        public long musteri_carikart_id { get; set; }

        /// <summary>
        /// Stok Yeri Bigisi
        /// </summary>
        public long stokyeri_carikart_id { get; set; }

        /// <summary>
        /// Sipariş Türü Bilgisi
        /// </summary>
        public byte siparisturu_id { get; set; }

        public short sezon_id { get; set; }
        

    }
}