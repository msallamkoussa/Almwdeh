using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Almwdeh.Models;
using Almwdeh.Common;
using Almwdeh.Resources;
using System.Net.Mail;
using System.Net;
using Almwdeh.Functions;
using System.Web.Security;

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
       
        public PartialViewResult Login()
        {
            
            return PartialView("_Login");
        }
        public PartialViewResult Signup()
        {

            return PartialView("_Signup");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Account");
        }

        //  POST: /Account/Login
        [HttpPost]  
        public ActionResult Validate(UsersAccessTbl admin,string ReturnUrl,string ControllerName,string ActionName)
        {

            var _admin = db.UsersAccessTbls.Where(s => s.Email == admin.Email || s.Username==admin.Username ).FirstOrDefault();
            if (_admin != null)
            {
                if (string.Compare(_admin.Password, Crypto.Hash(admin.Password)) == 0)
                {
                    MySession.Current.UserID = _admin.Id;                   
                    Session["id"] = _admin.Id;
                    FormsAuthentication.SetAuthCookie(_admin.Email, false);
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        // return RedirectToAction(ActionName,ControllerName);
                        return Json(new
                        {
                            redirectUrl = Url.Action(ActionName, ControllerName),
                            isRedirect = true,
                            status = true
                        });
                    
                    }

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
                    admin.Password = Crypto.Hash(admin.Password);
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




        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "ResetPassword")
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var GeneralInfo = db.GeneralInformationTbls.Where(s => s.Id == 1).FirstOrDefault();
             var fromEmail = new MailAddress("musekousa@gmail.com", "Almawadeh");
            //var fromEmail = new MailAddress(GeneralInfo.GmailEmail, GeneralInfo.GmailName);
            var toEmail = new MailAddress(emailID);
             var fromEmailPassword = "b2EBa9HkDtMQrdhMPZg2rg=="; // Replace with actual password
            //var fromEmailPassword = GeneralInfo.GmailPassword;

            string subject = "";
            string body = "";
            //if (emailFor == "VerifyAccount")
            //{
            //    subject = "Your account is successfully created!";
            //    body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
            //        " successfully created. Please click on the below link to verify your account" +
            //        " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            //}
            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/><br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, Crypto.Decrypt(fromEmailPassword))
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }


        public PartialViewResult ForgotPassword()
        {

            return PartialView("_ForgotPassword");
        }
       

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            var account = db.UsersAccessTbls.Where(a => a.Email == EmailID).FirstOrDefault();
            if (account != null)
            {
                //Send email for reset password
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                account.ResetPasswordCode = resetCode;
                //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                //in our model class in part 1
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Reset password link has been sent to your email id.";
                status = true;
            }
            else
            {
                message = "Account not found";
            }

            ViewBag.Message = message;
            return Json(new { status , message });

        }



        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }


            var user = db.UsersAccessTbls.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPasswrodModel model = new ResetPasswrodModel();
                model.ResetCode = id;
                return View(model);
            }
            else
            {
                return HttpNotFound();
            }

        }

        //**********************************  2
        [HttpPost]
        
        public ActionResult ResetPassword(ResetPasswrodModel model)
        {
            bool status = false;
            var message = "";
            if (ModelState.IsValid)
            {

                var user = db.UsersAccessTbls.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = Crypto.Hash(model.NewPassword);
                    user.ResetPasswordCode = "";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    message = "New password updated successfully";
                    status = true;
                }

            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return Json(new { status, message });
        }


    }
}