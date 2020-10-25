using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class ModelController : Controller
    {
        public ActionResult index()
        {
            return RedirectToAction("search");
        }

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