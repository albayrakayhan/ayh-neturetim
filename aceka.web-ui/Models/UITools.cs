using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aceka.web_ui.Models
{
    public static class UITools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">Gönderen</param>
        /// <param name="to">Alan</param>
        /// <param name="subject">Mail Başlığı</param>
        /// <param name="body">Mail Mesajı</param>
        /// <param name="errorMessage">Geri dönen hata mesajı</param>
        /// <returns></returns>
        public static bool SendMail(string from, string to, string subject, string body, ref string errorMessage)
        {
            bool retVal = false;

            try
            {
                ///Bu Bilgiler web.config den geliyor!
                string acekaSMTP = System.Configuration.ConfigurationManager.AppSettings["acekaSMTP"];
                string acekaSenderAccount = System.Configuration.ConfigurationManager.AppSettings["acekaSenderAccount"];
                string acekaSenderPassword = System.Configuration.ConfigurationManager.AppSettings["acekaSenderPassword"];
                ///Bu Bilgiler web.config den geliyor!

                System.Net.Mail.MailMessage mesaj = new System.Net.Mail.MailMessage(from, to, subject, body);
                mesaj.IsBodyHtml = true;

                System.Net.Mail.SmtpClient emailClient = new System.Net.Mail.SmtpClient(acekaSMTP, 587);
                emailClient.EnableSsl = false;
                emailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = new System.Net.NetworkCredential(acekaSenderAccount, acekaSenderPassword);
                emailClient.Send(mesaj);

                retVal = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return retVal;
        }

        public static string ReadToHtml(string htmlPath, System.Collections.Hashtable htReplace)
        {
            try
            {
                System.IO.TextReader reader = System.IO.File.OpenText(AppDomain.CurrentDomain.BaseDirectory + htmlPath);
                string text = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();

                System.Collections.IDictionaryEnumerator ie = htReplace.GetEnumerator();
                while (ie.MoveNext())
                {
                    text = text.Replace(ie.Key.ToString(), ie.Value.ToString());
                }
                return text;
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}