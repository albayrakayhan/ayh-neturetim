using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.MemberModels
{
    /// <summary>
    /// Giriş için gerekli bilgiler
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Kullanıcı Adı
        /// </summary>
        public string kullanici_adi { get; set; }

        /// <summary>
        /// Şifre
        /// </summary>
        public string sifre { get; set; }
    }
}