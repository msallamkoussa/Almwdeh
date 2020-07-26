using Almwdeh.Common;
using Almwdeh.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Almwdeh.Controllers
{
    public class MessageController : Controller
    {
        public AlmawadehDbEntities db = new AlmawadehDbEntities();



        // GET: Message
        public ActionResult Index()
        {

            return View();
        }


        public PartialViewResult MailSidebar()
        {
            return PartialView("_MailSidebar");
        }
        public PartialViewResult MailInbox()
        {

            int userAcceseID = 5;//MySession.Current.UserID;
            var UserMessages = db.MessagesTbls.Where(s => s.ReceiverUserAccessIDs == userAcceseID).ToList();
            //DateTime t = DateTime.Now;

            //// t = (DateTime)StudentInfo.BirthDate;

            //ViewBag.BirthDate = t.ToString("d");

            //ViewBag.CitySyria = new SelectList(db.ComboCitySyriaTbls, "Id", "Name");
            //ViewBag.CityIran = new SelectList(db.ComboCityIranTbls, "Id", "Name");
            return PartialView("_MailInbox", UserMessages);
        }

        public PartialViewResult CreateMessage()
        {
            //LIST OF USERS 
            return PartialView("_CreateMessage");
        }
        public PartialViewResult ReadMessage(int msgId)
        {
            var Message = db.MessagesTbls.Where(s => s.Id == msgId).FirstOrDefault();
            if (Message.ReadDate == null)
            {
                Message.ReadDate = DateTime.Now;
                Message.HasBeenRead = true;
                db.SaveChanges();
            }

            var Attachments = db.MessageAttachmentsTbls.Where(s => s.MessageIDs == msgId).ToList();

            MessageAndAttachments MSGAttachments = new MessageAndAttachments();
            MSGAttachments.msg = Message;
            MSGAttachments.attachments = Attachments;
            return PartialView("_ReadMessage", MSGAttachments);
        }

        [HttpPost]
        //[Authorize]

        public ActionResult SendMessage(MessagesTbl Msg)
        {
            var NewTransaction = db.Database.BeginTransaction(); // we have to commit the save the changes on the database
            //NewTransaction.Commit();
            //NewTransaction.Rollback();

            Msg.SenderUserAccessIDs = 5; // need to be modified later 

            // prepear the message to be added to table
            DateTime TimeNow = DateTime.Now;
            Msg.SendDate = TimeNow;
            Msg.HasAttachments = false;
            Msg.HasBeenRead = false;

            if (Msg.Subject != null && Msg.ReceiverUserAccessIDs != null)
            {
                try
                {
                    db.MessagesTbls.Add(Msg);
                    db.SaveChanges();//now we have the message id so if there were attachments we can inserted

                }
                catch (Exception e)
                {
                    NewTransaction.Rollback();
                    return Json(new { status = false, message = "There were a problem in Creating the Message: " + e.Message });
                }

            }
            else
            {
                return Json(new { status = false, message = "Supject and reciver cannot be empty " });
            }

            // Checking numbers of files injected in Request object  
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                try
                {
                    Msg.HasAttachments = true;

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string fileNameOnServer = string.Empty;
                        string MessagesfilesPathOnServer = "/Uploaded/Messages/";
                        var file = System.Web.HttpContext.Current.Request.Files["FileNum[" + i + "]"];
                        if (file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var ext = Path.GetExtension(file.FileName);
                            var size = Request.Files[i].ContentLength;

                            fileNameOnServer = TimeNow.ToString("yyyyMMddHHmmssfff") + "_" + fileName;

                            var comPath = Server.MapPath(MessagesfilesPathOnServer) + fileNameOnServer;
                            file.SaveAs(comPath);

                            MessageAttachmentsTbl attachment = new MessageAttachmentsTbl();
                            attachment.FileName = fileName;
                            attachment.FilePath = MessagesfilesPathOnServer + fileNameOnServer;
                            attachment.MessageIDs = Msg.Id;
                            attachment.FileType = ext;
                            attachment.FileSize = (size / 1024).ToString() + "KB";
                            db.MessageAttachmentsTbls.Add(attachment);
                            //std.Photo = url;
                        }
                        db.SaveChanges();

                    }
                }
                catch (Exception e)
                {
                    NewTransaction.Rollback();
                    return Json(new { status = false, message = "There were a problem in uploading the attachments: " + e.Message });
                }


            }

            NewTransaction.Commit();
            return Json(new { status = true, message = "Your message has been sent successfully." });
        }



        //reply to the message owner          - using email template

        //var messageOwner = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
        //var users = from user in dbContext.Users
        //            orderby user.FirstName
        //            select new
        //            {
        //                FullName = user.FirstName + " " + user.LastName,
        //                UserEmail = user.Email
        //            };

        //var uemail = users.Where(x => x.FullName == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
        //SendGridMessage replyMessage = new SendGridMessage();
        //replyMessage.From = new MailAddress(username);
        //replyMessage.Subject = "Reply for your message :" + dbContext.Messages.Where(i => i.Id == messageId).Select(s => s.Subject).FirstOrDefault();
        //replyMessage.Text = vm.Reply.ReplyMessage;


        //replyMessage.AddTo(uemail);


        //var credentials = new NetworkCredential("YOUR SENDGRID USERNAME", "PASSWORD");
        //var transportweb = new Web(credentials);
        //transportweb.DeliverAsync(replyMessage);
        //return RedirectToAction("Index", "Home", new { Id = messageId });


        [HttpPost]
        [Authorize]

        // will never be used.
        public ActionResult DeleteMessage(int messageId)
        {
            MessagesTbl _messageToDelete = db.MessagesTbls.Find(messageId);
            db.MessagesTbls.Remove(_messageToDelete);
            db.SaveChanges();

            // also delete the replies related to the message
            var _repliesToDelete = db.MessagesTbls.Where(i => i.ReplyToMessageIDs == messageId).ToList();
            if (_repliesToDelete != null)
            {
                foreach (var rep in _repliesToDelete)
                {
                    db.MessagesTbls.Remove(rep);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Index", "Message");
        }
    }
}