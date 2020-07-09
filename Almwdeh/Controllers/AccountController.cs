using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Almwdeh.Models;
using Almwdeh.Common;
using Almwdeh.Resources;
namespace Almwdeh.Controllers
{
    
    public class AccountController : Controller
    {
  
        public AlmawadehDbEntities db = new AlmawadehDbEntities();

      
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Login()
        {
            
            return View();
        }

      //  POST: /Account/Login
       [HttpPost]  
        public ActionResult Validate(UsersAccessTbl admin)
        {

            var _admin = db.UsersAccessTbls.Where(s => s.Email == admin.Email || s.Username==admin.Username ).FirstOrDefault();
            if (_admin != null)
            {
                if (string.Compare(_admin.Password, admin.Password) == 0)
                {
                    MySession.Current.UserID = _admin.Id;                   
                    Session["id"] = _admin.Id;
           
           

                    return Json(new { status = true, message = Resource.LoginSuccessfull });
                }
                else
                {
                    return Json(new { status = false, message = Resource.ErrorMessageLoginWrongEmailOrPassword });
                }
            }
            else
            {
                return Json(new { status = false, message = Resource.ErrorMessageLoginWrongEmailOrPassword });
            }
        }
        [HttpPost]

        public ActionResult CreateAccount(UsersAccessTbl admin)
        {
            var _admin = db.UsersAccessTbls.Where(s => s.Email == admin.Email).FirstOrDefault();

            if (_admin == null) {
                try
                {
                    db.UsersAccessTbls.Add(admin);
                    db.SaveChanges();
                    return Json(new { status = true, message = Resource.CreateAccountSuccessfull });
                }
                catch(Exception e)
                {
                    return Json(new { status = false, message = Resource.CreateAccountdbSaveChange });

                }
            }         
           return Json(new { status = false, message = Resource.CreateAccountEmailAlreadyExist });
           

        }

    }
}