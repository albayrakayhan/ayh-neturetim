using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class GenelAyarlarController : Controller
    {
        // GET: GenelAyarlar
        public ActionResult calismaTakvimi()
        {
            return View();
        }
        public ActionResult gtipTanimlamalari()
        {
            return View();
        }

        public ActionResult sistemAyarlari()
        {
            return View();
        }
        public ActionResult toplufiyatTanimlama()
        {
            return View();
        }

        public ActionResult kurBilgileri()
        {
            return View();
        }

        public ActionResult talimatturuTanimlama()
        {
            return View();
        }
    }
}