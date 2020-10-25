using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.StokkartModel
{
    /// <summary>
    /// Stok KArt Arama İşlemleri için gerekli olan POST parametreleri
    /// </summary>
    public class StokkartAramaParameter
    {
        /// <summary>
        /// Stok Kart Kimlik
        /// </summary>
        public long stokkart_id { get; set; }
        /// <summary>
        /// Stok Kart Adı
        /// </summary>
        public string stok_adi { get; set; }

        /// <summary>
        /// Stok Kart Tür Bilgisi
        /// </summary>
        public short stokkart_tur_id { get; set; }

        /// <summary>
        /// Stokkart Tip Bilgisi
        /// </summary>
        public int stokkart_tipi_id { get; set; }

        /// <summary>
        /// Stok Kart Kod Bilgisi
        /// </summary>
        public string stok_kodu { get; set; }


        /// <summary>
        /// Stok Kart Türü Bilgisi
        /// </summary>
        public byte stokkartturu { get; set; }


    }
}