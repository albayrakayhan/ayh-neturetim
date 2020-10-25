using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class FiyatTanimController : Controller
    {
        // GET: FiyatTanim
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TopluFiyatTanimlama()
        {
            return View();
        }
    }
}