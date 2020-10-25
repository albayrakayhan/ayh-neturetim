using aceka.infrastructure.Core;
using aceka.infrastructure.Repositories;
using aceka.web_api.Models;
using aceka.web_api.Models.AnonymousModels;
using aceka.web_api.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace aceka.web_api.Controllers
{
    /// <summary>
    /// Üyelik işlemleri
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MemberController : ApiController
    {
        #region Degiskenler
        private MemberRepository memberRepository = null;
        #endregion

        /// <summary>
        /// Kullanıcı giriş metodu
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>        
        [HttpPost]
        [CustAuthFilter]
        [Route("api/member/giris")]
        public HttpResponseMessage Giris(Login login)
        {
            if (login != null)
            {
                memberRepository = new MemberRepository();
                var carikart = memberRepository.Giris(login.kullanici_adi, login.sifre);
                if (carikart != null)
                {
                    Member member = new Member
                    {
                        personel_id = carikart.carikart_id,
                        ad_soyad = carikart.cari_unvan
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, member);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new NotFound { message = "Authentication Failed!" });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new NotFound { message = "A problem has been occurred during the process." });
            }
        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [CustAuthFilter]
        [Route("api/member")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new long?[] { Tools.PersonelId });
        }

    }
}
