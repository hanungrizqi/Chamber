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
    [RoutePrefix("api/Emprecord")]
    public class EmpRecordController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpGet]
        [Route("Get_ListEmprecord/{posid}")]
        public IHttpActionResult Get_ListEmprecord(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };

                if (isAdminorNot.ID_Role == 1)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                else if (isAdminorNot.ID_Role == 2)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 && a.ID_STATUS == 4).ToList();

                    return Ok(new { Data = data });
                }
                else
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.POSITION_ID == posid).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListEmprecord_Daterange/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult Get_ListEmprecord_Daterange(string posid, string startDate = null, string endDate = null)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                IQueryable<VW_T_APPROVAL> query = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    query = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value));
                }
                else if (isAdminorNot.ID_Role == 2)
                {
                    query = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)));
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    query = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 && a.ID_STATUS == 4);
                }
                else
                {
                    query = db.VW_T_APPROVALs.Where(a => a.POSITION_ID == posid);
                }

                // Jika startDate dan endDate diberikan, tambahkan filter berdasarkan tanggal
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    // Parse startDate dan endDate menjadi DateTime
                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        // Filter data berdasarkan rentang tanggal
                        query = query.Where(a => a.WAKTU_ABSEN >= parsedStartDate && a.WAKTU_ABSEN <= parsedEndDate);
                    }
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
        [Route("QuickActions")]
        public IHttpActionResult QuickActions(TBL_T_APPROVAL chambers)
        {
            try
            {
                var cek = db.TBL_T_APPROVALs.FirstOrDefault(a => a.APPROVAL_ID == chambers.APPROVAL_ID);

                if (cek != null)
                {
                    cek.ID_STATUS = chambers.ID_STATUS;
                    cek.APPROVER = chambers.APPROVER;
                    cek.WAKTU_APPROVAL = DateTime.UtcNow.ToLocalTime();
                    cek.FLAG = 1;

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
        public IHttpActionResult Detail(int id)
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

        [HttpGet]
        [Route("RecordData")]
        public IHttpActionResult RecordData(string nrp, string datefromcfc, int jmlapprvlperhari)
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_APPROVALs.Where(a => a.NRP == nrp && a.DATE_FROM_CFC.ToString() == datefromcfc && a.JUMLAH_APPROVAL_PERHARI == jmlapprvlperhari).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();

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
                    cek.NOTED = chambers.NOTED;
                    cek.FLAG = 1;

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
