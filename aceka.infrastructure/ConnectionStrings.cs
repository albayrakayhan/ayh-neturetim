using aceka.infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace aceka.infrastructure
{
    public static class ConnectionStrings
    {
        public static string SqlConn
        {
            get
            {
                var connString =
                      String.Format("Data Source={0};Initial Catalog={1};User Id={2};password={3}"
                      , "92.45.23.86,2544"
                      , "NetUretim"
                      , "sa"
                      //, Tools.Decrypt("7IFxf2GPP8dOEV/QTzZpYw==")
                      , Tools.Decrypt("izYnoTUWm2aSiIemnuiWvw==")
                      );
                return connString;
            }
        }
    }
}
