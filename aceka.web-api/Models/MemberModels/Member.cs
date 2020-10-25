using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_api.Models.MemberModels
{
    /// <summary>
    /// Member model
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Member Id
        /// </summary>
        public long personel_id { get; set; }
        /// <summary>
        /// Ad Soyad bilgisi
        /// </summary>
        public string ad_soyad { get; set; }

    }
}