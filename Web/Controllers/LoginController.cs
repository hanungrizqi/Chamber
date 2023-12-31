﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        CfmDataContext db = new CfmDataContext();
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

            var check_paramedis = db.VW_Users.Where(a => a.Username == nrp).FirstOrDefault();
            if (check_paramedis == null)
            {
                return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke CFM Web" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            if (check_paramedis.ID_Role == 4 && check_paramedis.POSITION_ID == null)
            {
                var dataUser = db.VW_Users.Where(a => a.Username == nrp).FirstOrDefault();
                var dataRole = db.TBL_M_USERs.Where(a => a.Username == nrp).FirstOrDefault();
                var lastAddedUser = db.TBL_M_USERs.OrderByDescending(u => u.DateAdd).FirstOrDefault();
                var lastCfc = db.VW_T_APPROVALs.OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();

                if (dataRole != null)
                {
                    var dataRoledakun = db.TBL_M_ROLEs.Where(a => a.ID == dataRole.ID_Role).FirstOrDefault();

                    Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
                    Session["Nrp"] = nrp;
                    Session["ID_Role"] = dataRole.ID_Role;

                    Session["Name"] = "Paramedis";
                    Session["Site"] = "";
                    Session["Role"] = dataRoledakun.RoleName;
                    Session["PositionID"] = "IN1J01112";

                    //Notifikasi
                    #region Notifkasi
                    Session["LastaddUser"] = lastAddedUser.Username;
                    string timeAgo = "Invalid Date";
                    if (lastAddedUser != null && DateTime.TryParse(lastAddedUser.DateAdd.ToString(), out DateTime dateAdd))
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - dateAdd;
                        timeAgo = FormatTimeAgo(timeDifference);
                    }
                    Session["LastaddUserDate"] = timeAgo;

                    if (dataRole.ID_Role == 1)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    else if (dataRole.ID_Role == 2)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == dataUser.POSITION_ID.Trim()).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    else if (dataRole.ID_Role == 4)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && a.ID_STATUS == 4).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && a.ID_STATUS == 4).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    else
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    #endregion

                    return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke CFM Web" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            if (check_paramedis.ID_Role == 4 && check_paramedis.POSITION_ID != null)
            {
                var dataUser = db.TBL_R_MASTER_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == nrp).FirstOrDefault();
                var dataRole = db.TBL_M_USERs.Where(a => a.Username == nrp).FirstOrDefault();
                var lastAddedUser = db.TBL_M_USERs.OrderByDescending(u => u.DateAdd).FirstOrDefault();
                var lastCfc = db.VW_T_APPROVALs.OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();

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
                    Session["PositionID"] = dataUser.POSITION_ID.Trim();
                    ViewBag.POSID = dataUser.POSITION_ID;

                    //Notifikasi
                    #region Notifkasi
                    Session["LastaddUser"] = lastAddedUser.Username;
                    string timeAgo = "Invalid Date";
                    if (lastAddedUser != null && DateTime.TryParse(lastAddedUser.DateAdd.ToString(), out DateTime dateAdd))
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - dateAdd;
                        timeAgo = FormatTimeAgo(timeDifference);
                    }
                    Session["LastaddUserDate"] = timeAgo;

                    if (dataRole.ID_Role == 1)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    else if (dataRole.ID_Role == 2)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == dataUser.POSITION_ID.Trim()).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    else if (dataRole.ID_Role == 4)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        //Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                        Session["LastCFC"] = lastCfc_Notif.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    }
                    else
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                    }
                    #endregion

                    return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke CFM Web" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                var dataUser = db.TBL_R_MASTER_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == nrp).FirstOrDefault();
                var dataRole = db.TBL_M_USERs.Where(a => a.Username == nrp).FirstOrDefault();
                var lastAddedUser = db.TBL_M_USERs.OrderByDescending(u => u.DateAdd).FirstOrDefault();
                var lastCfc = db.VW_T_APPROVALs.OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();

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
                    Session["PositionID"] = dataUser.POSITION_ID.Trim();
                    ViewBag.POSID = dataUser.POSITION_ID;

                    //Notifikasi
                    #region Notifkasi
                    Session["LastaddUser"] = lastAddedUser.Username;
                    string timeAgo = "Invalid Date";
                    if (lastAddedUser != null && DateTime.TryParse(lastAddedUser.DateAdd.ToString(), out DateTime dateAdd))
                    {
                        DateTime currentTime = DateTime.Now;
                        TimeSpan timeDifference = currentTime - dateAdd;
                        timeAgo = FormatTimeAgo(timeDifference);
                    }
                    //Session["LastaddUserDate"] = timeAgo;
                    var zoltrak = lastAddedUser.DateAdd.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["LastaddUserDate"] = zoltrak;

                    if (dataRole.ID_Role == 1)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        string timeAgos = "Invalid Date";
                        if (lastCfc_Notif != null && DateTime.TryParse(lastCfc_Notif.DATETIME_FROM_CFC.ToString(), out DateTime dateAdds))
                        {
                            DateTime currentTimes = DateTime.Now;
                            TimeSpan timeDifference = currentTimes - dateAdds;
                            timeAgos = FormatTimeAgo(timeDifference);
                        }
                        //Session["LastCFC"] = timeAgos;
                        Session["LastCFC"] = lastCfc_Notif.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    }
                    else if (dataRole.ID_Role == 2)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) /*&& a.ATASAN == dataUser.POSITION_ID.Trim()*/).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        //Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                        Session["LastCFC"] = lastCfc_Notif.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    }
                    else if (dataRole.ID_Role == 4)
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && a.ID_STATUS == 4).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && a.ID_STATUS == 4).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        //Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                        Session["LastCFC"] = lastCfc_Notif.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    }
                    else
                    {
                        var excludedStatuses = new[] { 1, 5, 6, 7 };
                        int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                        Session["Counted"] = countss;

                        var lastCfc_Notif = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(u => u.DATETIME_FROM_CFC).FirstOrDefault();
                        //Session["LastCFC"] = GetTimeAgo(lastCfc_Notif?.DATETIME_FROM_CFC);
                        Session["LastCFC"] = lastCfc_Notif.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    }
                    #endregion

                    return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { Remarks = false, Message = "Maaf anda tidak memiliki akses ke CFM Web" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
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

        public ActionResult LoginMokGem(string o, string i)
        {
            VW_MOK_LOGIN login = db.VW_MOK_LOGINs.Where(f => f.username == o && f.token == i).FirstOrDefault();
            string test = o + " " + i;
            if (o == null || i == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                string nrp = "";

                if (o.Count() > 7)
                {
                    nrp = o.Substring(o.Length - 7);
                }
                else
                {
                    nrp = o;
                }

                var checks = db.VW_Users.Where(a => a.Username == nrp).FirstOrDefault();
                if (checks != null)
                {

                    var result = MakeSession(o);
                    return RedirectToAction("Index", "Approval");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }

        }

        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Login");
        }
    }
}