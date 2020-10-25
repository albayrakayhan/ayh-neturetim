using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using aceka.infrastructure.Core;
using static aceka.infrastructure.Core.CustomEnums;
using aceka.infrastructure.Models;

namespace aceka.infrastructure.Repositories
{
    /// <summary>
    /// Generic Class. Class çağırılırken ilgili class attach edilerek kullanılır.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CrudRepository<T> where T : class
    {
        /// <summary>
        /// CommandType = Text. INSERT Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Insert(T className, string[] excepts = null)
        {
            //Adnan TÜRK 25.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string values = "";
            string fields = "";

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Insert cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;
                        fields += name + ",";
                        values += "@" + name + ",";

                        parameterList.Add(new SqlParameter("@" + name, value));
                    }
                }

                if (parameterList.Count > 0)
                {
                    //Insert query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    values = values.TrimEnd(',');
                    query = string.Format("INSERT INTO {0}({1}) values({2}) SELECT SCOPE_IDENTITY()", t.Name, fields, values);

                    acekaResult.RetVal = SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method public static AcekaResult Insert(T className, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = Text. INSERT Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="tableName">Tablo adı manuel belirtilmelidir.</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Insert(T className, string tableName, string[] excepts = null)
        {
            //Adnan TÜRK 25.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string values = "";
            string fields = "";
            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Insert cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;


                        fields += name + ",";
                        values += "@" + name + ",";

                        parameterList.Add(new SqlParameter("@" + name, value));
                    }
                }

                if (parameterList.Count > 0)
                {
                    //Insert query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    values = values.TrimEnd(',');
                    query = string.Format("INSERT INTO {0}({1}) values({2}) SELECT SCOPE_IDENTITY()", tableName, fields, values);

                    acekaResult.RetVal = SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method public static AcekaResult Insert(T className, string tableName, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = StoredProcedure. StoredProcedure için gerekli alanlar gönderilerek kayıt işlemi yapılır.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="spName">StoredProcedure adı</param>
        /// <param name="recortMethodType">İşlemin tipi. ExecuteScalar, executeNonQuery v.s.</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Insert(T className, string spName, CustomEnums.RecortMethodType recortMethodType, string[] excepts = null)
        {
            //Adnan TÜRK 25.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Insert cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;

                        parameterList.Add(new SqlParameter("@" + name, value));
                    }
                }

                if (parameterList.Count > 0)
                {
                    switch (recortMethodType)
                    {
                        case RecortMethodType.executeScalar:
                            acekaResult.RetVal = SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.StoredProcedure, spName, parameterList.ToArray());
                            break;
                        case RecortMethodType.executeNonQuery:
                            acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.StoredProcedure, spName, parameterList.ToArray());
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method public static AcekaResult Insert(T className, string spName, CustomEnums.RecortMethodType recortMethodType, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = Text. Update Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="keyForWhere">Where için kullanılacak field</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Update(T className, string keyForWhere, string[] excepts = null)
        {
            //Adnan TÜRK 27.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string fields = "";
            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);
                        //Eğer Update cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;
                        parameterList.Add(new SqlParameter("@" + name, value));
                        //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                        if (keyForWhere == name)
                            continue;
                        fields += name + "= @" + name + ",";
                    }
                }

                if (parameterList.Count > 0)
                {
                    //Update query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    query = String.Format("UPDATE {0} SET {1} WHERE {2} = @{3}", t.Name, fields, keyForWhere, keyForWhere);

                    acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Metod public static AcekaResult Update(T className, string keyForWhere, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = Text. Update Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="keysForWhere">Where için kullanılacak field lar Örn: new string[]{"carikart_id","yil"}</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Update(T className, string[] keysForWhere, string[] excepts = null)
        {
            //Adnan TÜRK 16.02.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string fields = "";

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Update cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && (Array.IndexOf(excepts, name) > -1 && Array.IndexOf(keysForWhere, name) <= -1 ))
                            continue;

                        parameterList.Add(new SqlParameter("@" + name, value));


                        //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                        if (Array.IndexOf(keysForWhere, name) > -1)
                            continue;

                        fields += name + "= @" + name + ",";
                    }
                }

                //Where için AND kriterler oluşturuluyor
                string keyForWhere = "";
                for (int i = 0; i < keysForWhere.Length; i++)
                {
                    keyForWhere += keysForWhere[i] + " = @" + keysForWhere[i] + " AND ";
                }

                if (!string.IsNullOrEmpty(keyForWhere))
                    keyForWhere = keyForWhere.Trim(new char[] { ' ', 'A', 'N', 'D', ' ' });

                if (parameterList.Count > 0)
                {
                    //Update query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    query = String.Format("UPDATE {0} SET {1} WHERE {2}", t.Name, fields, keyForWhere);
                    acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "public static AcekaResult Update(T className, string keyForWhere, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = Text. Update Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="tableName">Tablo adı manuel belirtilmelidir.</param>
        /// <param name="keyForWhere">Where için kullanılacak field</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Update(T className, string tableName, string keyForWhere, string[] excepts = null)
        {
            //Adnan TÜRK 27.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string fields = "";

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Update cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;

                        parameterList.Add(new SqlParameter("@" + name, value));


                        //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                        if (keyForWhere == name)
                            continue;

                        fields += name + "= @" + name + ",";
                    }
                }

                if (parameterList.Count > 0)
                {
                    //Update query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    query = String.Format("UPDATE {0} SET {1} WHERE {2} = @{3}", tableName, fields, keyForWhere, keyForWhere);

                    acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Metod public static AcekaResult Update(T className, string keyForWhere, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = Text. Update Metodu otomatik oluştururlur.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="tableName">Tablo adı manuel belirtilmelidir.</param>
        /// <param name="keysForWhere">Where için kullanılacak field lar Örn: new string[]{"carikart_id","yil"}</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Update(T className, string tableName, string[] keysForWhere, string[] excepts = null)
        {
            //Adnan TÜRK 16.02.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string fields = "";

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Update cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;

                        parameterList.Add(new SqlParameter("@" + name, value));


                        //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                        if (Array.IndexOf(keysForWhere, name) > -1)
                            continue;

                        fields += name + "= @" + name + ",";
                    }
                }

                //Where için AND kriterler oluşturuluyor
                string keyForWhere = "";
                for (int i = 0; i < keysForWhere.Length; i++)
                {
                    keyForWhere += keysForWhere[i] + " = @" + keysForWhere[i] + " AND ";
                }

                if (!string.IsNullOrEmpty(keyForWhere))
                    keyForWhere = keyForWhere.Trim(new char[] { ' ', 'A', 'N', 'D', ' ' });

                if (parameterList.Count > 0)
                {

                    //Update query oluşturuluyor
                    fields = fields.TrimEnd(',');
                    query = String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, fields, keyForWhere);

                    acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "public static AcekaResult Update(T className, string keyForWhere, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// CommandType = StoredProcedure. StoredProcedure için gerekli alanlar gönderilerek güncelleme işlemi yapılır.
        /// </summary>
        /// <param name="className">Generic T. Kayıt yapılacak tablo için gerekli class' dır</param>
        /// <param name="spName">StoredProcedure adı</param>
        /// <param name="recortMethodType">İşlemin tipi. ExecuteScalar, executeNonQuery v.s.</param>
        /// <param name="excepts">Tabloda kullanılmak istenilmeyen alanlar string array olarak belirtilebilir. (Opsiyonel)</param>
        /// <returns></returns>
        public static AcekaResult Update(T className, string spName, CustomEnums.RecortMethodType recortMethodType, string[] excepts = null)
        {
            //Adnan TÜRK 27.01.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();
            List<SqlParameter> parameterList = new List<SqlParameter>();

            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);

                        //Eğer Update cümlesine dahil edilmeyecek field lar varsa kontrol control ediliyor.
                        if (excepts != null && Array.IndexOf(excepts, name) > -1)
                            continue;

                        parameterList.Add(new SqlParameter("@" + name, value));
                    }
                }

                if (parameterList.Count > 0)
                {
                    switch (recortMethodType)
                    {
                        case RecortMethodType.executeScalar:
                            acekaResult.RetVal = SqlHelper.ExecuteScalar(ConnectionStrings.SqlConn, CommandType.StoredProcedure, spName, parameterList.ToArray());
                            break;
                        case RecortMethodType.executeNonQuery:
                            acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.StoredProcedure, spName, parameterList.ToArray());
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method public static AcekaResult Update(T className, string spName, CustomEnums.RecortMethodType recortMethodType, string[] excepts = null)"
                };
            }
            return acekaResult;
        }

        /// <summary>
        /// Delete metod
        /// </summary>
        /// <param name="className">Object</param>
        /// <param name="tableName">Tablo adı</param>
        /// <param name="keysForWhere">Where için gerekli parametreler</param>
        /// <param name="including">Delete cümlesine dahil edilmeyecek field lar kontrol ediliyor ve ekleniyor.</param>
        /// <returns></returns>
        public static AcekaResult Delete(T className, string tableName, string[] keysForWhere, string[] including)
        {
            //Adnan TÜRK 22.02.2017
            AcekaResult acekaResult = new AcekaResult();

            Type t = className.GetType();
            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            try
            {
                foreach (var per in t.GetProperties())
                {
                    //Class içerisinde tanımlı primitive (int, string, bool v.s) değişken tipleri ile Insert parametreleri oluşturuluyor!
                    if (per.PropertyType.FullName.Contains("System") && !per.PropertyType.FullName.Contains("System.Collections"))
                    {
                        var name = per.Name;
                        var value = per.GetValue(className, null);
                        //Delete cümlesine dahil edilmeyecek field lar kontrol ediliyor ve ekleniyor.
                        if (including != null && Array.IndexOf(including, name) > -1)
                            parameterList.Add(new SqlParameter("@" + name, value));
                        else
                            continue;
                    }
                }
                //Where için AND kriterler oluşturuluyor
                string keyForWhere = "";
                for (int i = 0; i < keysForWhere.Length; i++)
                {
                    keyForWhere += keysForWhere[i] + " = @" + keysForWhere[i] + " AND ";
                }

                if (!string.IsNullOrEmpty(keyForWhere))
                    keyForWhere = keyForWhere.Trim(new char[] { ' ', 'A', 'N', 'D', ' ' });

                query = String.Format("DELETE FROM {0} WHERE {1}", tableName, keyForWhere);

                acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method  public static AcekaResult Delete(T className, string tableName, string[] keysForWhere, string[] including)"
                };
            }

            return acekaResult;
        }
    }

    /// <summary>
    /// Text tabanlı class. Tablo ve sorgular generic değildir. Tetx olarak belirtilir.
    /// </summary>
    public static class CrudRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">Text olarak tablo adı</param>
        /// <param name="keyForWhere">Where için kullanılacak field Örn: "carikart_id"</param>      
        /// <param name="fields">Dictionary tipinde update e dahil olacak alanları belirtilir</param>
        /// <param name="overrideWhereCriteria">True olarak gönderilirse where kriteri elle yazabiliyoruz</param>
        /// <returns></returns>
        public static AcekaResult Update(string tableName, string keyForWhere, Dictionary<string, object> fields, bool overrideWhereCriteria = false)
        {
            //Adnan TÜRK 27.01.2017
            AcekaResult acekaResult = new AcekaResult();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string strFields = "";

            try
            {
                foreach (var item in fields)
                {

                    parameterList.Add(new SqlParameter("@" + item.Key, item.Value));

                    //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                    if (overrideWhereCriteria == false && keyForWhere == item.Key)
                        continue;
                    strFields += item.Key + " = @" + item.Key + ", ";

                }


                strFields = strFields.TrimEnd(new char[] { ',', ' ' });

                if (!overrideWhereCriteria)
                    query = String.Format("UPDATE {0} SET {1} WHERE {2} = @{3}", tableName, strFields, keyForWhere, keyForWhere);
                else
                    query = String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, strFields, keyForWhere);

                acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Metod public static AcekaResult Update(string tableName, string keyForWhere, Dictionary<string, object> fields)"
                };
            }

            return acekaResult;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="keysForWhere">>Where için kullanılacak field lar Örn: new string[]{"carikart_id","yil"}</param>
        /// <param name="fields">Dictionary tipinde update e dahil olacak alanları belirtilir</param>
        /// <returns></returns>
        public static AcekaResult Update(string tableName, string[] keysForWhere, Dictionary<string, object> fields)
        {
            //Adnan TÜRK 27.01.2017
            AcekaResult acekaResult = new AcekaResult();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string strFields = "";

            try
            {
                foreach (var item in fields)
                {

                    parameterList.Add(new SqlParameter("@" + item.Key, item.Value));

                    //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                    if (Array.IndexOf(keysForWhere, item.Key) > -1)
                        continue;
                    strFields += item.Key + " = @" + item.Key + ", ";

                }
                strFields = strFields.TrimEnd(new char[] { ',', ' ' });

                //Where için AND kriterler oluşturuluyor
                string keyForWhere = "";
                for (int i = 0; i < keysForWhere.Length; i++)
                {
                    keyForWhere += keysForWhere[i] + " = @" + keysForWhere[i] + " AND ";
                }

                if (!string.IsNullOrEmpty(keyForWhere))
                    keyForWhere = keyForWhere.Trim(new char[] { ' ', 'A', 'N', 'D', ' ' });

                query = String.Format("UPDATE {0} SET {1} WHERE {2}", tableName, strFields, keyForWhere);

                acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Metod public static AcekaResult Update(string tableName, string[] keysForWhere, Dictionary<string, object> fields)"
                };
            }



            return acekaResult;
        }

        public static AcekaResult Delete(string tableName, string[] keysForWhere, Dictionary<string, object> fields)
        {
            AcekaResult acekaResult = new AcekaResult();

            List<SqlParameter> parameterList = new List<SqlParameter>();
            string query = "";
            string strFields = "";

            try
            {
                foreach (var item in fields)
                {
                    parameterList.Add(new SqlParameter("@" + item.Key, item.Value));

                    //Where kriterinde kullanılacak alan SET statement da dahil edilmiyor
                    if (Array.IndexOf(keysForWhere, item.Key) > -1)
                        continue;
                    strFields += item.Key + " = @" + item.Key + ", ";

                }

                strFields = strFields.TrimEnd(new char[] { ',', ' ' });

                //Where için AND kriterler oluşturuluyor
                string keyForWhere = "";
                for (int i = 0; i < keysForWhere.Length; i++)
                {
                    keyForWhere += keysForWhere[i] + " = @" + keysForWhere[i] + " AND ";
                }

                if (!string.IsNullOrEmpty(keyForWhere))
                    keyForWhere = keyForWhere.Trim(new char[] { ' ', 'A', 'N', 'D', ' ' });

                query = String.Format("DELETE {0}  WHERE {1}", tableName, keyForWhere);

                acekaResult.RetVal = SqlHelper.ExecuteNonQuery(ConnectionStrings.SqlConn, CommandType.Text, query, parameterList.ToArray());
            }
            catch (Exception ex)
            {
                acekaResult.ErrorInfo = new ErrorInfo
                {
                    Message = ex.Message,
                    Source = ex.Source,
                    ErrorCode = ex.HResult.ToString(),
                    Location = "namespace aceka.infrastructure.Repositories, Method public static AcekaResult Update(string tableName, KeyValuePair<string, object> where, Dictionary<string, object> fields))"
                };
            }
            return acekaResult;
        }
    }
}
