using restApi.Models;
using System;
using System.Collections.Generic;
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
        [Route("Get_ListApproval")]
        public IHttpActionResult Get_ListApproval()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_APPROVALs.Where(a => a.APPROVER == "" || a.APPROVER == null).OrderBy(a => a.APPROVAL_ID).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
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
    }
}
