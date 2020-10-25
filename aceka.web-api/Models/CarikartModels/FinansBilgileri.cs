using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace aceka.web_api.Models.CarikartModels
{
    public class FinansBilgileri
    {

        public Int64 carikart_id { get; set; }
        public Int64 degistiren_carikart_id { get; set; }

        [JsonIgnore]
        [ScriptIgnore]
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
        public byte kur_farki { get; set; }
        public byte odeme_listesinde_cikmasin { get; set; }
        public byte alacak_listesinde_cikmasin { get; set; }
        public byte ticari_islem_grubu { get; set; }
        public long ilgili_sube_carikart_id { get; set; }
        public long finans_sorumlu_carikart_id { get; set; }
        public string swift_kodu { get; set; }
        public int tedarik_gunu { get; set; }
        public bool cari_hesapta_ciksin { get; set; }
        public int odeme_plani_id { get; set; }
        //Önemli ! 
        //parametre_cari_odeme_sekli tablosunda "cari_odeme_sekli_id" olan field bu tabloda "odeme_tipi" olarak tutuluyor
        public byte odeme_sekli_id { get; set; }


        //Tablo
        //carikart_fiyat_tipi tablosu
        public string fiyattipi { get; set; }

        //Tablo
        //carikart_muhasebe tablosu
        public byte sirket_id { get; set; }
        public int sene { get; set; }
        public string muh_kod { get; set; }
        public Nullable<short> masraf_merkezi_id { get; set; }
    }
}