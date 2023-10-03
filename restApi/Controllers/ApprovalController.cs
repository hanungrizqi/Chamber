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
    [RoutePrefix("api/Approval")]
    public class ApprovalController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpGet]
        [Route("Get_ListApproval/{posid}")]
        public IHttpActionResult Get_ListApproval(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                //var data = db.VW_T_APPROVALs.Where(a => a.APPROVER == "" || a.APPROVER == null && a.ID_STATUS != 1).OrderBy(a => a.APPROVAL_ID).ToList();

                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                if (isAdminorNot.ID_Role == 1)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                else
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }

                //return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListApproval_Daterange/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult Get_ListApproval_Daterange(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                IQueryable<VW_T_APPROVAL> query = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    query = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && !excludedStatuses.Contains(a.ID_STATUS.Value));
                }
                else
                {
                    query = db.VW_T_APPROVALs.Where(a => a.APPROVER == null && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid);
                }

                // Parse startDate and endDate to DateTime
                DateTime parsedStartDate, parsedEndDate;
                if (DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                    DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                {
                    // Filter data by date range
                    query = query.Where(a => a.DATE_FROM_CFC >= parsedStartDate && a.DATE_FROM_CFC <= parsedEndDate);
                }

                var data = query.OrderBy(a => a.APPROVAL_ID).ToList();
                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("QuickApprove")]
        public IHttpActionResult QuickApprove(TBL_T_APPROVAL chambers)
        {
            try
            {
                var cek = db.TBL_T_APPROVALs.FirstOrDefault(a => a.APPROVAL_ID == chambers.APPROVAL_ID);

                if (cek != null)
                {
                    cek.ID_STATUS = chambers.ID_STATUS;
                    cek.APPROVER = chambers.APPROVER;
                    cek.WAKTU_APPROVAL = DateTime.UtcNow.ToLocalTime();

                    db.SubmitChanges();

                    return Ok(new { Remarks = true });
                }
                else
                {
                    return Ok(new { Remarks = false, Message = "No matching record found." });
                }
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Approve")]
        public IHttpActionResult Approve(TBL_T_APPROVAL chambers)
        {
            try
            {
                var cek = db.TBL_T_APPROVALs.FirstOrDefault(a => a.APPROVAL_ID == chambers.APPROVAL_ID);

                if (cek != null)
                {
                    cek.ID_STATUS = chambers.ID_STATUS;
                    cek.APPROVER = chambers.APPROVER;
                    cek.WAKTU_APPROVAL = DateTime.UtcNow.ToLocalTime();
                    cek.NOTED = chambers.NOTED;

                    db.SubmitChanges();

                    return Ok(new { Remarks = true });
                }
                else
                {
                    return Ok(new { Remarks = false, Message = "No matching record found." });
                }
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpGet]
        [Route("Detail/{id}")]
        public IHttpActionResult Get_PPEDetail(int id)
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_APPROVALs.Where(a => a.APPROVAL_ID == id).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
