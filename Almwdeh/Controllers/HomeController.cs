using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Almwdeh.Common;
using Almwdeh.Models;

namespace Almwdeh.Controllers
{
    public class HomeController : Controller
    {
        public AlmawadehDbEntities db = new AlmawadehDbEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult Sidebar()
        {

            return PartialView("_Sidebar");
        }

        public PartialViewResult TopNavigation()
        {
            var student = db.StudentsTbls.Where(s => s.UserAccessIDs == MySession.Current.UserID).FirstOrDefault();

            return PartialView("_TopNavigation", student);
        }
    }
}