using System.Web.Mvc;
using System.Web.Routing;

namespace aceka.web_api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "Arama",
               url: "api/modelkart/arama",
               defaults: new { controller = "ModelKart", action = "StokkartAra",
                   stokkart_id = 0,
                   stok_adi = "",
                   stokkart_tur_id = 0,
                   stokkart_tipi_id = 1,
                   stok_kodu = "",
                   stokkartturu = 0,
                   orjinal_stok_kodu = ""
               }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
