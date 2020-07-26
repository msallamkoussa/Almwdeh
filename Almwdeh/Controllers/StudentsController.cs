using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Almwdeh.Common;
using Almwdeh.Models;
namespace Almwdeh.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private AlmawadehDbEntities db = new AlmawadehDbEntities();
        private int userAcceseID = MySession.Current.UserID;
        // GET: Students
        public ActionResult Index()
        {
            var Student = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            if (Student==null)
            {
                return View("CreateStudent");
            }
            else if(Student.FirstName == null || Student.PassportNumber == null || Student.NeedMonthlyAid == null ||)
            {
                return View("CreateStudent");
            }
            return View();
        }

        public ActionResult CreateStudent()
        {
            return View("CreateStudent");
        }

        public PartialViewResult StudentGeneralInfo()
        {

            //int userAcceseID = MySession.Current.UserID;
            //var StudentInfo = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            //DateTime t = DateTime.Now;//(DateTime)StudentInfo.BirthDate;
            //ViewBag.BirthDate = t.ToString("d");
            ViewBag.CitySyria = new SelectList(db.ComboCitySyriaTbls, "Name", "Name");
            ViewBag.CityIran = new SelectList(db.ComboCityIranTbls, "Name", "Name");
            return PartialView("_StudentGeneralInfo");
        }


        [HttpPost]
        public ActionResult AddStudent(StudentsTbl std)
        {

            var NewStudent = db.StudentsTbls.Where(s => s.UserAccessIDs == std.UserAccessIDs).FirstOrDefault();

            if (NewStudent == null)
            {
                StudentsTbl student = new StudentsTbl();
                student.UserAccessIDs = std.UserAccessIDs;
                db.StudentsTbls.Add(student);
                db.SaveChanges();
                NewStudent = db.StudentsTbls.Where(s => s.UserAccessIDs == std.UserAccessIDs).FirstOrDefault();
            }
            //***************************************
            try
            {
                //*************************************** Uploaded Files
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    try
                    {
                        string fileNameOnServer = string.Empty;
                        string PassportfilesPathOnServer = "/Documents/PassportFiles/";
                        string ResidentfilesPathOnServer = "/Documents/ResidentFiles/";
                        string ProfileImagePathOnServer = "/Documents/ProfileImages/";
                        var Passport = System.Web.HttpContext.Current.Request.Files["PassportFile"];
                        var Resident = System.Web.HttpContext.Current.Request.Files["ResidentFile"];
                        var ProfileImage = System.Web.HttpContext.Current.Request.Files["file"];

                        if (Passport != null)
                            if (Passport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(Passport.FileName);
                                var ext = Path.GetExtension(Passport.FileName);
                                DateTime TimeNow = DateTime.Now;
                                fileNameOnServer = TimeNow.ToString("yyyyMMddHHmmssfff") + "_" + fileName;
                                var comPath = Server.MapPath(PassportfilesPathOnServer) + fileNameOnServer;
                                Passport.SaveAs(comPath);
                                NewStudent.PassportFile = PassportfilesPathOnServer + fileNameOnServer;
                                //std.Photo = url;
                            }
                        if (Resident != null)
                            if (Resident.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(Resident.FileName);
                                var ext = Path.GetExtension(Resident.FileName);
                                DateTime TimeNow = DateTime.Now;
                                fileNameOnServer = TimeNow.ToString("yyyyMMddHHmmssfff") + "_" + fileName;
                                var comPath = Server.MapPath(ResidentfilesPathOnServer) + fileNameOnServer;
                                Resident.SaveAs(comPath);
                                NewStudent.ResidentFile = ResidentfilesPathOnServer + fileNameOnServer;
                                //std.Photo = url;
                            }
                        if (ProfileImage != null)
                            if (ProfileImage.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(ProfileImage.FileName);
                                var ext = Path.GetExtension(ProfileImage.FileName);
                                DateTime TimeNow = DateTime.Now;
                                fileNameOnServer = TimeNow.ToString("yyyyMMddHHmmssfff") + "_" + fileName;
                                var comPath = Server.MapPath(ProfileImagePathOnServer) + fileNameOnServer;
                                ProfileImage.SaveAs(comPath);
                                NewStudent.Photo = ProfileImagePathOnServer + fileNameOnServer;
                            }

                    }
                    catch (Exception e)
                    {
                        //NewTransaction.Rollback();
                        return Json(new { status = false, message = "There were a problem in uploading Passport or Resident or profile image" + e.Message });
                    }


                }

                //*************************************** General student Info
                if (std.FirstName != null)
                    NewStudent.FirstName = std.FirstName;
                if (std.LastName != null)
                    NewStudent.LastName = std.LastName;
                if (std.Sex != null)
                    NewStudent.Sex = std.Sex;
                if (std.BirthDate != null)
                    NewStudent.BirthDate = std.BirthDate;
                if (std.BirthPlace != null)
                    NewStudent.BirthPlace = std.BirthPlace;
                if (std.FatherName != null)
                    NewStudent.FatherName = std.FatherName;
                if (std.MotherName != null)
                    NewStudent.MotherName = std.MotherName;
                if (std.MobileSyria != null)
                    NewStudent.MobileSyria = std.MobileSyria;
                if (std.MobileIran != null)
                    NewStudent.MobileIran = std.MobileIran;
                if (std.WhatsappNumber != null)
                    NewStudent.WhatsappNumber = std.WhatsappNumber;
                if (std.Photo != null)
                    NewStudent.Photo = std.Photo;
                if (std.Doctrine != null)
                    NewStudent.Doctrine = std.Doctrine;
                if (std.CitySyria != null)
                    NewStudent.CitySyria = std.CitySyria;
                if (std.AddressSyria != null)
                    NewStudent.AddressSyria = std.AddressSyria;
                if (std.CityIran != null)
                    NewStudent.CityIran = std.CityIran;
                if (std.AddressIran != null)
                    NewStudent.AddressIran = std.AddressIran;
                //*************************************** 
                if (std.MaritalStatus != null)
                    NewStudent.MaritalStatus = std.MaritalStatus;
                if (std.WifeName != null)
                    NewStudent.WifeName = std.WifeName;
                if (std.ChildernNumber != null)
                    NewStudent.ChildernNumber = std.ChildernNumber;
                if (std.WifeDetails != null)
                    NewStudent.WifeDetails = std.WifeDetails;
                ////*************************************** معلومات جواز السفر
                if (std.PassportNumber != null)
                    NewStudent.PassportNumber = std.PassportNumber;
                if (std.PassportExpireDate != null)
                    NewStudent.PassportExpireDate = std.PassportExpireDate;
                if (std.ResidentExpireDate != null)
                    NewStudent.ResidentExpireDate = std.ResidentExpireDate;
                //***************************************المعلومات المالیة 
                if (std.FinancialStatus != null)
                    NewStudent.FinancialStatus = std.FinancialStatus;
                if (std.NeedMonthlyAid != null)
                    NewStudent.NeedMonthlyAid = std.NeedMonthlyAid;
                if (std.BankAccountNumber != null)
                    NewStudent.BankAccountNumber = std.BankAccountNumber;
                if (std.BankCardNumber != null)
                    NewStudent.BankCardNumber = std.BankCardNumber;
                if (std.BankShibaNumber != null)
                    NewStudent.BankShibaNumber = std.BankShibaNumber;
                //***************************************
                if (std.Working != null)
                    NewStudent.Working = std.Working;
                if (std.WorkDetails != null)
                    NewStudent.WorkDetails = std.WorkDetails;
                //*************************************** معلومات اضافیة
                if (std.Skills != null)
                    NewStudent.Skills = std.Skills; ;
                if (std.PreviousJobs != null)
                    NewStudent.PreviousJobs = std.PreviousJobs;
                if (std.ShahidFamily != null)
                    NewStudent.ShahidFamily = std.ShahidFamily;
                if (std.ShahidRelationship != null)
                    NewStudent.ShahidRelationship = std.ShahidRelationship;
                if (std.ReligiousCommitment != null)
                    NewStudent.ReligiousCommitment = std.ReligiousCommitment;
                if (std.GeneralBehavior != null)
                    NewStudent.GeneralBehavior = std.GeneralBehavior;
                if (std.TheIdentifier != null)
                    NewStudent.TheIdentifier = std.TheIdentifier;
                if (std.EffectivenessAndActivity != null)
                    NewStudent.EffectivenessAndActivity = std.EffectivenessAndActivity;
                if (std.DisciplineSituation != null)
                    NewStudent.DisciplineSituation = std.DisciplineSituation;
                if (std.AdditionalInformation != null)
                    NewStudent.AdditionalInformation = std.AdditionalInformation;
                //***************************************
                NewStudent.LastEditDate = DateTime.Now; ;
                NewStudent.EditVerified = false;
                //***************************************
                db.SaveChanges();
                return Json(new { status = true, message = "تم حفظ المعلومات بنجاح" });


            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "لم يتم الحفظ " + e.Message.ToString() });

            }
        }

        [HttpPost]
        public ActionResult AddStudentUniversity(StudentsUniversitiesTbl std)
        {
            var NewStudent = db.StudentsUniversitiesTbls.Where(s => s.UserAccessIDs == std.UserAccessIDs).FirstOrDefault();
            if (NewStudent == null)
            {
                
                
                db.StudentsUniversitiesTbls.Add(std);
                db.SaveChanges();
                NewStudent = db.StudentsUniversitiesTbls.Where(s => s.UserAccessIDs == std.UserAccessIDs).FirstOrDefault();
            }
            //***************************************
            try
            {
                //*************************************** Uploaded Files
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    try
                    {
                        string fileNameOnServer = string.Empty;
                        string CertificatefilesPathOnServer = "/Documents/Certificates/";
                        var Certificate = System.Web.HttpContext.Current.Request.Files["CertificateFile"];
                        if (Certificate != null)
                            if (Certificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(Certificate.FileName);
                                var ext = Path.GetExtension(Certificate.FileName);
                                DateTime TimeNow = DateTime.Now;
                                fileNameOnServer = TimeNow.ToString("yyyyMMddHHmmssfff") + "_" + fileName;
                                var comPath = Server.MapPath(CertificatefilesPathOnServer) + fileNameOnServer;
                                Certificate.SaveAs(comPath);
                                NewStudent.CertificateFile = CertificatefilesPathOnServer + fileNameOnServer;
                                //std.Photo = url;
                            }
                    }
                    catch (Exception e)
                    {
                        //NewTransaction.Rollback();
                        return Json(new { status = false, message = "There were a problem in uploading The certificate" + e.Message });
                    }
                }

                //***************************************  Student University Info
                if (std.AcademicDegree != null)
                    NewStudent.AcademicDegree = std.AcademicDegree;
                if (std.UniversityName != null)
                    NewStudent.UniversityName = std.UniversityName;
                if (std.FieldOfStudy != null)
                    NewStudent.FieldOfStudy = std.FieldOfStudy;
                if (std.StudentUniversityNumber != null)
                    NewStudent.StudentUniversityNumber = std.StudentUniversityNumber;
                if (std.StartAcademicYearDate != null)
                    NewStudent.StartAcademicYearDate = std.StartAcademicYearDate;
                if (std.GraduationStatues != null)
                    NewStudent.GraduationStatues = std.GraduationStatues;
                if (std.GraduationYear != null)
                    NewStudent.GraduationYear = std.GraduationYear;
                if (std.AverageScore != null)
                    NewStudent.AverageScore = std.AverageScore;

                //***************************************
                NewStudent.LastEditDate = DateTime.Now; ;
                NewStudent.EditVerified = false;
                //***************************************
                db.SaveChanges();
                return Json(new { status = true, message = "تم حفظ المعلومات بنجاح" });


            }
            catch (Exception e)
            {
                return Json(new { status = false, message = "لم يتم الحفظ " + e.Message.ToString() });

            }
        }

        public PartialViewResult PassportInfo()
        {
            return PartialView("_PassportInfo");
        }

        public PartialViewResult FinanceInfo()
        {
            return PartialView("_FinanceInfo");
        }
        public PartialViewResult StudentExtralInfo()
        {
            ViewBag.Skills = new SelectList(db.ComboSkilles, "Name", "Name");
            ViewBag.TheIdentifier = new SelectList(db.ComboTheIdentifiers, "Name", "Name");
            return PartialView("_StudentExtralInfo");
        }


        public PartialViewResult SemesterInfo()
        {
            return PartialView("_SemesterInfo");
        }
        public PartialViewResult EditStudentGeneralInfo()
        {

            var StudentInfo = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            DateTime t = DateTime.Now;//(DateTime)StudentInfo.BirthDate;
            ViewBag.BirthDate = StudentInfo.BirthDate.Value.ToString("d");

            ViewBag.CitySyria = new SelectList(db.ComboCitySyriaTbls, "Name", "Name", StudentInfo.CitySyria);
            ViewBag.CityIran = new SelectList(db.ComboCityIranTbls, "Name", "Name", StudentInfo.CityIran);
            return PartialView("_EditStudentGeneralInfo", StudentInfo);
        }
        public PartialViewResult profileStudent()
        {
            var student = db.StudentsTbls.Where(s => s.UserAccessIDs == MySession.Current.UserID).FirstOrDefault();
            return PartialView("_profileStudent", student);
        }

        public PartialViewResult EditPassportInfo()
        {
            var StudentInfo = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            DateTime t = DateTime.Now;//(DateTime)StudentInfo.BirthDate;
            ViewBag.PassportExpireDate = StudentInfo.PassportExpireDate.Value.ToString("d");
            ViewBag.ResidentExpireDate = StudentInfo.ResidentExpireDate.Value.ToString("d");
            return PartialView("_EditPassportInfo", StudentInfo);
        }

        public PartialViewResult EditFinanceInfo()
        {
            var StudentInfo = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();

            return PartialView("_EditFinanceInfo", StudentInfo);
        }
        public PartialViewResult EditStudentExtralInfo()
        {
            var StudentInfo = db.StudentsTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            ViewBag.Skills = new SelectList(db.ComboSkilles, "Name", "Name");
            ViewBag.TheIdentifier = new SelectList(db.ComboTheIdentifiers, "Name", "Name");
            

            ViewData["SelectedSkills"] = StudentInfo.Skills.Split(',').ToList();
            return PartialView("_EditStudentExtralInfo", StudentInfo);
        }


        public ActionResult UniversityInfo()
        {
            var Student = db.StudentsUniversitiesTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();
            ViewBag.AcademicDegree = new SelectList(db.ComboAcademicDegrees, "Name", "Name");
            ViewBag.FieldOfStudy = new SelectList(db.ComboFieldOfStudies, "Name", "Name");
            ViewBag.GraduationStatues = new SelectList(db.ComboGraduationStatues, "Name", "Name");
            DateTime t = DateTime.Now;//(DateTime)StudentInfo.BirthDate;
            ViewBag.StartAcademicYearDate = Student.StartAcademicYearDate.Value.ToString("d");
            ViewBag.GraduationYear = Student.GraduationYear.Value.ToString("d");


            if (Student == null)
            {
               
                return View("_UniversityInfo");
            }
            return View("_EditUniversityInfo", Student);
        }
        public ActionResult EditUniversityInfo()
        {
            var StudentUniver = db.StudentsUniversitiesTbls.Where(s => s.UserAccessIDs == userAcceseID).FirstOrDefault();

            ViewBag.AcademicDegree = new SelectList(db.ComboAcademicDegrees, "Name", "Name");
            ViewBag.FieldOfStudy = new SelectList(db.ComboFieldOfStudies, "Name", "Name");
            ViewBag.GraduationStatues = new SelectList(db.ComboGraduationStatues, "Name", "Name");
            DateTime t = DateTime.Now;//(DateTime)StudentInfo.BirthDate;
            ViewBag.StartAcademicYearDate = StudentUniver.StartAcademicYearDate.Value.ToString("d");
            ViewBag.GraduationYear = StudentUniver.GraduationYear.Value.ToString("d");
            return View("_EditUniversityInfo", StudentUniver);
        }
    }
}
 