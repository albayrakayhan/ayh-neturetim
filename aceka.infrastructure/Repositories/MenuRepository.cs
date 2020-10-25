using aceka.infrastructure.Models;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Repositories
{
    public class MenuRepository
    {
        #region Degiskenler
        private DataTable dt = null;
        private DataSet ds = null;
        private SqlConnection conn = null;
        private MenuItem menu = null;
        #endregion

        public List<MenuItem> Getir()
        {
            List<MenuItem> rootMenu = null;
            //List<MenuItem> menu = null;

            #region Query
            string query = @"SELECT system_name, dil_id, menu_level, parent_system_name, order_id, caption, tooltip_caption, url FROM [menu].[main]";
            #endregion

            dt = SqlHelper.ExecuteDataset(ConnectionStrings.SqlConn, CommandType.Text, query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                Dictionary<string, MenuItem> dict = dt.Rows.Cast<DataRow>()
                .Select(r => new MenuItem
                {
                    order_id = r.Field<int>("order_id"),
                    system_name = r.Field<string>("system_name"),
                    parent_system_name = r.Field<string>("parent_system_name"),
                    caption = r.Field<string>("caption"),
                    url = r.Field<string>("url"),
                    menu_level = Convert.ToByte(r["menu_level"])
                })
               .ToDictionary(m => m.system_name);
                rootMenu = new List<MenuItem>();

                rootMenu = dict.Values.Where(v => v.menu_level == 0).ToList<MenuItem>();

                foreach (var item in rootMenu)
                {
                    if (dict[item.system_name] != null)
                    {
                        var subMenues = dict.Values.Where(sb => sb.parent_system_name == item.system_name).OrderBy(sb => sb.order_id).ToList();
                        if (subMenues != null && subMenues.Count > 0)
                        {
                            item.SubMenu.AddRange(subMenues);
                        }
                    }
                }

            }

            return rootMenu;
        }
    }
}
