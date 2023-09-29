using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        CfmDataContext db = new CfmDataContext();
        // GET: Login
        public ActionResult Index()
        {
            Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
            return View();
        }

        public JsonResult MakeSession(string NRP)
        {
            string nrp = "";

            if (NRP.Count() > 7)
            {
                nrp = NRP.Substring(NRP.Length - 7);
            }
            else
            {
                nrp = NRP;
            }
            var dataUser = db.TBL_R_MASTER_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == nrp).FirstOrDefault();
            var dataRole = db.TBL_M_USERs.Where(a => a.Username == nrp).FirstOrDefault();
            var lastAddedUser = db.TBL_M_USERs.OrderByDescending(u => u.DateAdd).FirstOrDefault();
            var lastCfc = db.VW_T_APPROVALs.OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

            if (dataRole != null)
            {
                var dataRoledakun = db.TBL_M_ROLEs.Where(a => a.ID == dataRole.ID_Role).FirstOrDefault();

                Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
                Session["Nrp"] = nrp;
                Session["ID_Role"] = dataRole.ID_Role;
                string[] nameWords = dataUser.NAME.Split(' ');
                string firstName = "";

                if (nameWords.Length >= 2)
                {
                    string combinedName = nameWords[0] + " " + nameWords[1];
                    firstName = combinedName.Length > 14 ? combinedName.Substring(0, 14) : combinedName;
                }
                else
                {
                    firstName = dataUser.NAME.Length > 14 ? dataUser.NAME.Substring(0, 14) : dataUser.NAME;
                }

                Session["Name"] = firstName;
                Session["Site"] = dataUser.DSTRCT_CODE;
                Session["Role"] = dataRoledakun.RoleName;
                Session["PositionID"] = dataUser.POSITION_ID;
                ViewBag.POSID = dataUser.POSITION_ID;
                Session["LastaddUser"] = lastAddedUser.Username;
                //Session["LastaddUserDate"] = 
                string timeAgo = "Invalid Date";
                if (lastAddedUser != null && DateTime.TryParse(lastAddedUser.DateAdd.ToString(), out DateTime dateAdd))
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeDifference = currentTime - dateAdd;
                    timeAgo = FormatTimeAgo(timeDifference);
                }
                Session["LastaddUserDate"] = timeAgo;
                int countss = db.VW_T_APPROVALs.Count(a => a.APPROVER == null);
                Session["Counted"] = countss;
                Session["LastCFC"] = GetTimeAgo(lastCfc?.WAKTU_ABSEN);
                return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke CFM Web" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }
        private string GetTimeAgo(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan timeDifference = currentTime - dateTime.Value;
                return FormatTimeAgo(timeDifference);
            }
            return "Invalid Date";
        }
        private string FormatTimeAgo(TimeSpan timeDifference)
        {
            if (timeDifference.TotalDays >= 1)
            {
                int days = (int)timeDifference.TotalDays;
                int hours = (int)timeDifference.TotalHours % 24;
                return $"{days} day{(days != 1 ? "s" : "")} {hours} hour{(hours != 1 ? "s" : "")} ago";
            }
            else if (timeDifference.TotalHours >= 1)
            {
                int hours = (int)timeDifference.TotalHours;
                int minutes = (int)timeDifference.TotalMinutes % 60;
                return $"{hours} hour{(hours != 1 ? "s" : "")} {minutes} minute{(minutes != 1 ? "s" : "")} ago";
            }
            else
            {
                int minutes = (int)timeDifference.TotalMinutes;
                return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
            }
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Login");
        }
    }
}