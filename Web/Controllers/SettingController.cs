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
            int countss = db.VW_T_APPROVALs.Count(a => a.APPROVER == null);
            ViewBag.Emp = db.TBL_R_MASTER_KARYAWAN_ALLs.ToList();
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            ViewBag.Count = countss;
            return View();
        }

        public ActionResult Menu()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            int countss = db.VW_T_APPROVALs.Count(a => a.APPROVER == null);
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            ViewBag.Count = countss;
            return View();
        }
    }
}