﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class CfmManagementController : Controller
    {
        CfmDataContext db = new CfmDataContext();
        // GET: CfmManagement
        public ActionResult Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            int countss = db.VW_T_APPROVALs.Count(a => a.APPROVER == null);

            ViewBag.Count = countss;
            return View();
        }
    }
}