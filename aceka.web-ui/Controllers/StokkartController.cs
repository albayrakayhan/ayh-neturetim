using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class StokkartController : Controller
    {
        // GET: Stokkart
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("search");
            }
            return View(id);
        }
        public ActionResult New()
        {
            return View();
        }
    }
}