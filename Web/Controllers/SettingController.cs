using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class SettingController : Controller
    {
        CfmDataContext db = new CfmDataContext();
        // GET: EmpRecord
        public ActionResult Users()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Emp = db.TBL_R_MASTER_KARYAWAN_ALLs.ToList();
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 1)
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                ViewBag.Count = countss;
            }
            else
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == Session["PositionID"].ToString()).Count();
                ViewBag.Count = countss;
            }
            return View();
        }

        public ActionResult Menu()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 1)
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                ViewBag.Count = countss;
            }
            else
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == Session["PositionID"].ToString()).Count();
                ViewBag.Count = countss;
            }
            return View();
        }
    }
}