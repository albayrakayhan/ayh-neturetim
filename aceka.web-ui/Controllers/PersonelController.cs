using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class PersonelController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult Detail(long id)
        {
            return View(id);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}