
using aceka.infrastructure.Models;
using aceka.infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

namespace aceka.web_api.Models
{
    /// <summary>
    /// Custom Authentication Metod
    /// </summary>
    public class CustAuthFilter : ActionFilterAttribute
    {
        #region Variables
        private MemberRepository memberRepository = null;
        public string ApiUrl { get; set; }
        #endregion

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            //if (actionContext.Request.Method == HttpMethod.Get)
            //{

            //}

            IEnumerable<string> values = null;
            var result = actionContext.Request.Headers.TryGetValues("personel_id", out values);

            if (result == true)
            {
                var personel_id = Convert.ToInt64(values.First().ToString());
                var requestMethod = actionContext.Request.Method;
                var endPointArr = actionContext.Request.RequestUri.AbsolutePath.Split('/');

                /*
                 @carikart_id","100000000100"
                 "@url","api/stokkart"
                 */
                memberRepository = new MemberRepository();

                //Data Cache için kullanılacak
                //List<KullaniciYetki> kullaniciYetkileri = memberRepository.kullaniciYetkileriniGetir(100000000100);

                string errorMessage = "";

                KullaniciYetki kullaniciYetki = memberRepository.kullaniciYetkisiniGetir(personel_id, ApiUrl, ref errorMessage);

                if (kullaniciYetki != null && string.IsNullOrEmpty(errorMessage))
                {

                    bool authorized = false;
                    switch (requestMethod.Method.ToString())
                    {
                        case "GET"://Okuma
                            if (kullaniciYetki.okuma)
                                authorized = true;
                            break;
                        case "POST"://Yazma
                        case "PUT":
                            if (kullaniciYetki.yazma)
                                authorized = true;
                            break;
                        case "DELETE": //Silme
                            if (kullaniciYetki.silme)
                                authorized = true;
                            break;
                    }

                    if (!authorized)
                    {
                        // erişim yok ise
                        //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                    }
                }
                else
                {
                    //Eğer kullanıcıya ait herhangi bir yetkilendirme tanımlanmamış ise "Forbidden" uyarısı verilecek!

                    //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);

                }


            }
            else
            {
                //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }


            //else
            //{
            //    var arguments = actionContext.ActionArguments;


            //    Type itemType = arguments.First().Value.GetType();
            //    if (itemType.GetProperty("kullanici_adi") != null)
            //    {
            //        var val = itemType.GetProperty("kullanici_adi").GetValue(arguments.First().Value, null);
            //    }


            //    /*degistiren_carikart_id li kullanımlarda açılacak*/
            //    //Type itemType = arguments.First().Value.GetType();
            //    //if (itemType.GetProperty("degistiren_carikart_id") != null)
            //    //{
            //    //    var carikart_id = itemType.GetProperty("degistiren_carikart_id").GetValue(arguments.First().Value, null);
            //    //}
            //}


            base.OnActionExecuting(actionContext);
        }


    }
}