using restApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace restApi.Controllers
{
    [RoutePrefix("api/CfmManagement")]
    public class CfmManagementController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpGet]
        [Route("Get_ListChambers")]
        public IHttpActionResult Get_ListChambers()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_R_CFM_MANAGEMENTs.OrderBy(a => a.ID_CHAMBER).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListChambers_Daterange/{startDate}/{endDate}")]
        public IHttpActionResult Get_ListChambers_Daterange(string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                List<cufn_filterCFMManagementResult> data = null;

                // Parse startDate and endDate to DateTime
                DateTime parsedStartDate, parsedEndDate;
                if (DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                    DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                {
                    // Call the stored function with parameters
                    data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();
                }

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("Create_Chamber")]
        public IHttpActionResult Create_Chamber(TBL_M_CHAMBER param)
        {
            try
            {
                TBL_M_CHAMBER tbl = new TBL_M_CHAMBER();
                tbl.ID_CHAMBER = param.ID_CHAMBER;
                tbl.LOKASI = param.LOKASI;

                db.TBL_M_CHAMBERs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpPost]
        [Route("Update_Chamber")]
        public IHttpActionResult Update_Chamber(TBL_M_CHAMBER param)
        {
            try
            {
                var data = db.TBL_M_CHAMBERs.Where(a => a.ID == param.ID).FirstOrDefault();
                data.ID_CHAMBER = param.ID_CHAMBER;
                data.LOKASI = param.LOKASI;

                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
    }
}
