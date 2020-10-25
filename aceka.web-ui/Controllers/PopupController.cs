using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aceka.web_ui.Controllers
{
    public class PopupController : Controller
    {
        // GET: Popup
        public PartialViewResult OnayGecmisi()
        {
            return PartialView("_OnayGecmisi");
        }

        public PartialViewResult Varyant()
        {
            return PartialView("_varyant");
        }
        public PartialViewResult IlkMadde()
        {
            return PartialView("_ilkMadde");
        }
        public PartialViewResult StokkartVaryant()
        {
            return PartialView("_stokkartvaryant");
        }
        public PartialViewResult TalimatPopup()
        {
            return PartialView("_talimatPopup");
        }

        public PartialViewResult siparisNotPopup()
        {
            return PartialView("_siparisNotPopup");
        }

        public PartialViewResult FileUpload(long id=0)
        {
            return PartialView("_FileUpload",id);
        }
        public PartialViewResult BenzerPopup(long id = 0)
        {
            return PartialView("_benzerPopup", id);
        }
        public PartialViewResult StokkartFiyatPopUp(long id = 0)
        {
            return PartialView("_stokkartFiyatPopUp", id);
        }
        public PartialViewResult finansBankaHesap(long id = 0)
        {
            return PartialView("_finansBankaHesap", id);
        }
        public PartialViewResult finansIletisim(long id=0)
        {
            return PartialView("_finansIletisim", id);
        }
        public PartialViewResult finansNotlar(long id = 0)
        {
            return PartialView("_finansNotlar", id);
        }
        public PartialViewResult finansSube(long id = 0)
        {
            return PartialView("_finansSube", id);
        }

        public PartialViewResult personelCalismaYeri()
        {
            return PartialView("_personelCalismaYeri");
        }

        public PartialViewResult modelKartSecimPopup(long id = 0)
        {
            return PartialView("_modelKartSecimPopup", id);
        }

        public PartialViewResult depokartFiyatPopup(long id = 0)
        {
            return PartialView("_depokartFiyatPopup", id);
        }

        public PartialViewResult OtomatikAksesuarPopup()
        {
            return PartialView("_otomatikAksesuarPopup");
        }

        public PartialViewResult SearchAksesuarPopup()
        {
            return PartialView("_aksesuarArama");
        }
        
    }
}