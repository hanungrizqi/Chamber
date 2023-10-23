using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class EmpRecordController : Controller
    {
        CfmDataContext db = new CfmDataContext();
        // GET: EmpRecord
        public ActionResult Index(string startDate = null, string endDate = null, string status = null)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }

            if (status != null)
            {
                ViewBag.Status = status;
            }

            if (startDate != null)
            {
                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
            }

            if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 1)
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                ViewBag.Count = countss;
            }
            else if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 4)
            {
                int countss = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).Count();
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

        public ActionResult Detail_ERecord(int id)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }

            if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 1)
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).Count();
                ViewBag.Count = countss;
            }
            else if (Session["ID_Role"] != null && (int)Session["ID_Role"] == 4)
            {
                int countss = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).Count();
                ViewBag.Count = countss;
            }
            else
            {
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                int countss = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == Session["PositionID"].ToString()).Count();
                ViewBag.Count = countss;
            }
            ViewBag.id = id;
            return View();
        }
    }
}