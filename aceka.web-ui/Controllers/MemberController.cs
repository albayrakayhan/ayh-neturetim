using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult login()
        {
            return View();
        }
    }
}