using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace aceka.web_api.Models.SiparisModel
{
    /// <summary>
    /// Siparis -> tab -> Genel -> Adetler ve Fiyatlar için kullanılacak
    /// </summary>
    public class SiparisPivotModel
    {
        [Required(ErrorMessage ="Sipariş Id gerekli")]
        public long? siparis_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Beden Id gerekli")]
        public short beden_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string beden { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float toplam { get; set; }

        /// <summary>
        /// Model Array
        /// </summary>
        public List<SiparisPivotArray> siparisPivotArray { get; set; }
    }

    /// <summary>
    /// Dinamik oluşacak model için gerekli
    /// </summary>
    public class SiparisPivotArray
    {
        /// <summary>
        /// Dinamik gelen modele ait adet
        /// </summary>
        public int? adet { get; set; }

        /// <summary>
        /// Zorunlu. Model = Stokkart ı temsil ediyor. POST ve PUT işlemleri için gerekli olacak
        /// </summary>
        [Required(ErrorMessage = "Beden Id gerekli")]
        public long stokkart_id { get; set; }

        /// <summary>
        /// Stokkart a ait stok kodu bilgisi
        /// </summary>
        public string stok_kodu { get; set; }

        /// <summary>
        /// Dinamik gelen modele ait birim fiyat
        /// </summary>
        public decimal birimfiyat { get; set; }
    }
}