using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace aceka.infrastructure.Core
{
    public static class Tools
    {

        /// <summary>
        /// DD.MM.YYYY date format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string DateFormat(DateTime date)
        {
            return string.Format("{0:dd.MM.yyyy}", date);
        }

        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file

            string key = "aceka#holding+-";
            //System.Windows.Forms.MessageBox.Show(key);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            string key = "aceka#holding+-";

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        public static string ReplaceTitle(string Title)
        {
            string temp = Title.ToLower();
            //temp = temp.Replace("-", "");
            temp = temp.Replace("_", "");
            temp = temp.Replace(".", "-");
            temp = temp.Replace("  ", " ");
            temp = temp.Replace(" / ", "-");
            temp = temp.Replace(" ", "-");
            temp = temp.Replace("ç", "c");
            temp = temp.Replace("ğ", "g");
            temp = temp.Replace("ı", "i");
            temp = temp.Replace("ö", "o");
            temp = temp.Replace("ş", "s");
            temp = temp.Replace("ü", "u");
            temp = temp.Replace("'", "");
            temp = temp.Replace("!", "");
            temp = temp.Replace("?", "");
            temp = temp.Replace(":", "");
            temp = temp.Replace(";", "");
            temp = temp.Replace("~", "");
            temp = temp.Replace(",", "");
            temp = temp.Replace("&", "and");
            temp = temp.Replace("%", "");
            temp = temp.Replace("(", "");
            temp = temp.Replace(")", "");
            temp = temp.Replace("[", "");
            temp = temp.Replace("]", "");
            temp = temp.Replace("=", "");
            temp = temp.Replace("<", "");
            temp = temp.Replace(">", "");
            temp = temp.Replace("^", "");
            temp = temp.Replace("+", "");
            temp = temp.Replace("{", "");
            temp = temp.Replace("}", "");
            temp = temp.Replace("$", "");
            temp = temp.Replace("#", "");
            temp = temp.Replace("/", "-");
            temp = temp.Replace("|", "");
            temp = temp.Replace("\"", "");
            temp = temp.Replace("‘", "");
            temp = temp.Replace("’", "");
            temp = temp.Replace("“", "");
            temp = temp.Replace("”", "");
            temp = temp.Replace("á", "a");
            temp = temp.Replace("ê", "e");
            temp = temp.Replace("--", "");
            temp = temp.Replace("---", "");
            temp = temp.Replace("..", "");
            temp = temp.Replace("...", "");
            temp = temp.Replace("*", "");
            temp = temp.Replace("®", "");
            temp = temp.Replace("--", "-");
            temp = temp.Replace("---", "-");
            temp = temp.Replace("™", "");
            temp = temp.Replace("<sup>&reg;</sup>", "");
            temp = temp.Replace("supandreg-sup", "");

            return temp;
        }

        public static string HtmlTemizle150Length(string Html)
        {
            string temp = Html;
            if (temp != null)
            {
                temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
                temp = temp.Replace("\r\n", " ");
                temp = temp.Replace("\r", "").Replace("\n", "");
                temp = temp.Replace("&nbsp;&nbsp;", "");
                temp = temp.Replace("&quot;", "");
                temp = temp.Replace("&amp;", "");
                if (temp.Length > 150)
                {
                    temp = temp.Substring(0, 150) + "...";
                }
            }
            else
            {
                temp = string.Empty;
            }

            return temp;
        }

        public static string HtmlTemizle(string Html, int Uzunluk)
        {
            string temp = Html;
            if (temp != null)
            {
                temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
                temp = temp.Replace("\r\n", " ");
                temp = temp.Replace("\r", "").Replace("\n", "");
                temp = temp.Replace("&nbsp;&nbsp;", "");
                temp = temp.Replace("&quot;", "");
                temp = temp.Replace("&amp;", "");
                if (temp.Length > Uzunluk)
                {
                    temp = temp.Substring(0, Uzunluk) + "...";
                }
            }
            else
            {
                temp = string.Empty;
            }

            return temp;
        }

        public static string HtmlTemizle(string Html)
        {
            string temp = Html;
            temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
            temp = temp.Replace("\r", "").Replace("\n", "");
            temp = temp.Replace("&nbsp;&nbsp;", "");
            temp = temp.Replace("&quot;", "");
            temp = temp.Replace("&amp;", "");
            return temp;
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static bool EmailValidation(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }

        public static bool IsInteger(string str)
        {
            int outInt;
            return int.TryParse(str, out outInt);
        }

        public static bool IsBool(string str)
        {
            bool outBool;
            return bool.TryParse(str, out outBool);
        }

        public static bool IsDate(string str)
        {
            DateTime outDate;
            return DateTime.TryParse(str, out outDate);
        }

        public static bool IsNumeric(string str)
        {
            double outDouble;
            return double.TryParse(str, out outDouble);
        }

        public static bool IsDecimal(string str)
        {
            decimal outDecimal;
            return decimal.TryParse(str, out outDecimal);
        }

        public static bool IsEmail(string str)
        {
            Regex rg = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return rg.IsMatch(str);
        }

        public static int? ParseNullableInt(this string value)
        {
            if (value == null || value.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                try
                {
                    return int.Parse(value);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string GetDataByForm(DateTime date)
        {
            return String.Format("{0:dd.MMMM.yyyy}", date.Date);
        }

        public static long PersonelId
        {
            get
            {
                //long calisanId = 0;
                //var result = HttpContext.Current.Request.Headers.GetValues("personel_id");

                //long b2;
                //bool success = long.TryParse(result[0].ToString(), out b2);
                //if (success)
                //    calisanId = b2;

                //return calisanId;

                return 100000000100;

            }
        }

        public static string Substring(string text, int length)
        {
            if (text.Length > length)
                text = text.Substring(0, length);
            return text;
        }

        public static string CalculateSiparisNo(long siparis_id, string harf, byte length = 5)
        {
            var sounc = (1000000000001 + siparis_id).ToString();
            string sub = sounc.Substring(sounc.Length - length, length);
            return $"{harf}{sub}";
        }

        /// <summary>
        /// Mail Gönderen Metod
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="errorMessage"></param>
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

        public static List<string> CreateJson(DataSet ds)
        {
            var Olist = new List<string>();
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483644;
            foreach (DataTable dt in ds.Tables)
            {
                var rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col].ToString());
                    }
                    rows.Add(row);
                    row = null;
                }
                Olist.Add(serializer.Serialize(rows));
                rows = null;
            }
            serializer = null;
            return Olist;
        }
    }
}
