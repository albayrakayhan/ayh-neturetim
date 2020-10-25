using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Models
{
    public class MenuItem
    {
        public string system_name { get; set; }
        public string parent_system_name { get; set; }
        public string caption { get; set; }
        public string url { get; set; }
        public Int16 menu_level { get; set; }
        public int order_id { get; set; }
        public List<MenuItem> SubMenu { get; set; }
        //public string tooltip_caption { get; set; }
        //public string dil_id { get; set; }

        public MenuItem()
        {
            SubMenu = new List<MenuItem>();
        }
    }
}
