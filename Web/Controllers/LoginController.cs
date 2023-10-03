using System;
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
                Session["PositionID"] = dataUser.POSITION_ID.Trim();
                ViewBag.POSID = dataUser.POSITION_ID;

                //Notifikasi
                Session["LastaddUser"] = lastAddedUser.Username;
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

                //untuk dashboard
                //Data Karyawan Fit
                if (dataRole.ID_Role == 1)
                {
                    var lastCfc_Admin = db.VW_T_APPROVALs.OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Fit_Admin = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 1).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Unfit_Admin = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 2).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int kar_masuk = db.VW_T_APPROVALs.Count();
                    int kar_fit = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 1);
                    int kar_unfit = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 2);
                    Session["KaryawanMasuk"] = kar_masuk;
                    Session["KaryawanFit"] = kar_fit;
                    Session["KaryawanUnfit"] = kar_unfit;
                    string updateTerakhirMasuk = lastCfc_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirMasuk"] = updateTerakhirMasuk;
                    string updateTerakhirFit = lastCfc_Fit_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirFit"] = updateTerakhirFit;
                    string updateTerakhirUnfit = lastCfc_Unfit_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirUnfit"] = updateTerakhirUnfit;
                }
                else if (dataRole.ID_Role == 2)
                {
                    var lastCfc_GL = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Fit_GL = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 1 && u.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Unfit_GL = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 2 && u.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int kar_masuk = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).Count();
                    int kar_fit = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).Count(a => a.ID_STATUS == 1);
                    int kar_unfit = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).Count(a => a.ID_STATUS == 2);
                    Session["KaryawanMasuk"] = kar_masuk;
                    Session["KaryawanFit"] = kar_fit;
                    Session["KaryawanUnfit"] = kar_unfit;

                    string updateTerakhirMasuk = lastCfc_GL.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirMasuk"] = updateTerakhirMasuk;
                    string updateTerakhirFit = lastCfc_Fit_GL.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirFit"] = updateTerakhirFit;
                    string updateTerakhirUnfit = lastCfc_Unfit_GL.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirUnfit"] = updateTerakhirUnfit;
                }
                else
                {
                    var lastCfc_Admin = db.VW_T_APPROVALs.OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Fit_Admin = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 1).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();
                    var lastCfc_Unfit_Admin = db.VW_T_APPROVALs.Where(u => u.ID_STATUS == 2).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int kar_masuk = db.VW_T_APPROVALs.Count();
                    int kar_fit = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 1);
                    int kar_unfit = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 2);
                    Session["KaryawanMasuk"] = kar_masuk;
                    Session["KaryawanFit"] = kar_fit;
                    Session["KaryawanUnfit"] = kar_unfit;
                    string updateTerakhirMasuk = lastCfc_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirMasuk"] = updateTerakhirMasuk;
                    string updateTerakhirFit = lastCfc_Fit_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirFit"] = updateTerakhirFit;
                    string updateTerakhirUnfit = lastCfc_Unfit_Admin.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                    Session["UpdateTerakhirUnfit"] = updateTerakhirUnfit;
                }

                //CFC Utilization
                var cham1 = db.VW_R_CFM_MANAGEMENTs.Where(cham => cham.ID == 1).FirstOrDefault();
                var cham2 = db.VW_R_CFM_MANAGEMENTs.Where(cham => cham.ID == 2).FirstOrDefault();
                var cham3 = db.VW_R_CFM_MANAGEMENTs.Where(cham => cham.ID == 3).FirstOrDefault();
                var cham4 = db.VW_R_CFM_MANAGEMENTs.Where(cham => cham.ID == 4).FirstOrDefault();
                Session["Cham001"] = cham1.USDTDY;
                Session["Cham002"] = cham2.USDTDY;
                Session["Cham003"] = cham3.USDTDY;
                Session["Cham004"] = cham4.USDTDY;
                string updateTerakhircham1 = cham1.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                Session["UpdateTerakhirCham1"] = updateTerakhircham1;
                string updateTerakhircham2 = cham2.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                Session["UpdateTerakhirCham2"] = updateTerakhircham2;
                string updateTerakhircham3 = cham3.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                Session["UpdateTerakhirCham3"] = updateTerakhircham3;
                string updateTerakhircham4 = cham4.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                Session["UpdateTerakhirCham4"] = updateTerakhircham4;

                //progress fatigue check//
                #region tidak dapat bekerja
                if (dataRole.ID_Role == 1)
                {
                    var lasttdkdptbkrj = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int tdkdptbekerja = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 7);
                    Session["TidakDptBekerja"] = tdkdptbekerja;
                    if (lasttdkdptbkrj != null)
                    {
                        string updateTerakhirtdkdptBkerja = lasttdkdptbkrj.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirTdkdptBekerja"] = updateTerakhirtdkdptBkerja;
                    }
                    else
                    {
                        Session["UpdateTerakhirTdkdptBekerja"] = "";
                    }
                }
                else if (dataRole.ID_Role == 2)
                {
                    var lasttdkdptbkrj = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int tdkdptbekerja = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).Count(a => a.ID_STATUS == 7);
                    Session["TidakDptBekerja"] = tdkdptbekerja;
                    if (lasttdkdptbkrj != null)
                    {
                        string updateTerakhirtdkdptBkerja = lasttdkdptbkrj.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirTdkdptBekerja"] = updateTerakhirtdkdptBkerja;
                    }
                    else
                    {
                        Session["UpdateTerakhirTdkdptBekerja"] = "";
                    }
                }
                else
                {
                    var lasttdkdptbkrj = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int tdkdptbekerja = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 7);
                    Session["TidakDptBekerja"] = tdkdptbekerja;
                    if (lasttdkdptbkrj != null)
                    {
                        string updateTerakhirtdkdptBkerja = lasttdkdptbkrj.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirTdkdptBekerja"] = updateTerakhirtdkdptBkerja;
                    }
                    else
                    {
                        Session["UpdateTerakhirTdkdptBekerja"] = "";
                    }
                }
                #endregion

                #region akan retest
                if (dataRole.ID_Role == 1)
                {
                    var lastakanretest = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int retest = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 5);
                    Session["AkanRetest"] = retest;
                    if (lastakanretest != null)
                    {
                        string updateTerakhirretest = lastakanretest.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirRetest"] = updateTerakhirretest;
                    }
                    else
                    {
                        Session["UpdateTerakhirRetest"] = "";
                    }
                }
                else if (dataRole.ID_Role == 2)
                {
                    var lastakanretest = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int retest = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim()).Count(a => a.ID_STATUS == 5);
                    Session["AkanRetest"] = retest;
                    if (lastakanretest != null)
                    {
                        string updateTerakhirretest = lastakanretest.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirRetest"] = updateTerakhirretest;
                    }
                    else
                    {
                        Session["UpdateTerakhirRetest"] = "";
                    }
                }
                else
                {
                    var lastakanretest = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5).OrderByDescending(u => u.WAKTU_ABSEN).FirstOrDefault();

                    int retest = db.VW_T_APPROVALs.Count(a => a.ID_STATUS == 5);
                    Session["AkanRetest"] = retest;
                    if (lastakanretest != null)
                    {
                        string updateTerakhirretest = lastakanretest.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirRetest"] = updateTerakhirretest;
                    }
                    else
                    {
                        Session["UpdateTerakhirRetest"] = "";
                    }
                }
                #endregion

                #region butuh approval
                if (dataRole.ID_Role == 1)
                {
                    var butuhapproval = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int bthapprvl = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1).Count();
                    Session["ButuhApproval"] = bthapprvl;
                    if (butuhapproval != null)
                    {
                        string updateterakhirbutuhapprvl = butuhapproval.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirButuhApproval"] = updateterakhirbutuhapprvl;
                    }
                    else
                    {
                        Session["UpdateTerakhirButuhApproval"] = "";
                    }
                }
                else if (dataRole.ID_Role == 2)
                {
                    var butuhapproval = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1 && a.ATASAN == dataUser.POSITION_ID.Trim()).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int bthapprvl = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1 && a.ATASAN == dataUser.POSITION_ID.Trim()).Count();
                    Session["ButuhApproval"] = bthapprvl;
                    if (butuhapproval != null)
                    {
                        string updateterakhirbutuhapprvl = butuhapproval.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirButuhApproval"] = updateterakhirbutuhapprvl;
                    }
                    else
                    {
                        Session["UpdateTerakhirButuhApproval"] = "";
                    }
                }
                else
                {
                    var butuhapproval = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int bthapprvl = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && a.ID_STATUS != 1).Count();
                    Session["ButuhApproval"] = bthapprvl;
                    if (butuhapproval != null)
                    {
                        string updateterakhirbutuhapprvl = butuhapproval.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirButuhApproval"] = updateterakhirbutuhapprvl;
                    }
                    else
                    {
                        Session["UpdateTerakhirButuhApproval"] = "";
                    }
                }
                #endregion

                #region sudah approved
                if (dataRole.ID_Role == 1)
                {
                    var sudahapproved = db.VW_T_APPROVALs.Where(a => a.APPROVER != null || a.ID_STATUS == 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int sdhapprvd = db.VW_T_APPROVALs.Where(a => a.APPROVER != null || a.ID_STATUS == 1).Count();
                    Session["SudahApproved"] = sdhapprvd;
                    if (sudahapproved != null)
                    {
                        string updateterakhirsudahapproved = sudahapproved.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirSudahApproved"] = updateterakhirsudahapproved;
                    }
                    else
                    {
                        Session["UpdateTerakhirSudahApproved"] = "";
                    }
                }
                else if (dataRole.ID_Role == 2)
                {
                    var sudahapproved = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim() && (a.APPROVER != null || a.ID_STATUS == 1)).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int sdhapprvd = db.VW_T_APPROVALs.Where(a => a.ATASAN == dataUser.POSITION_ID.Trim() && (a.APPROVER != null || a.ID_STATUS == 1)).Count();
                    Session["SudahApproved"] = sdhapprvd;
                    if (sudahapproved != null)
                    {
                        string updateterakhirsudahapproved = sudahapproved.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirSudahApproved"] = updateterakhirsudahapproved;
                    }
                    else
                    {
                        Session["UpdateTerakhirSudahApproved"] = "";
                    }
                }
                else
                {
                    var sudahapproved = db.VW_T_APPROVALs.Where(a => a.APPROVER != null || a.ID_STATUS == 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

                    int sdhapprvd = db.VW_T_APPROVALs.Where(a => a.APPROVER != null || a.ID_STATUS == 1).Count();
                    Session["SudahApproved"] = sdhapprvd;
                    if (sudahapproved != null)
                    {
                        string updateterakhirsudahapproved = sudahapproved.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                        Session["UpdateTerakhirSudahApproved"] = updateterakhirsudahapproved;
                    }
                    else
                    {
                        Session["UpdateTerakhirSudahApproved"] = "";
                    }
                }
                #endregion

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