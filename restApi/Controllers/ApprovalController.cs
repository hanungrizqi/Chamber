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

                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                var excludedStatuses2 = new[] { 1, 4, 5, 6, 7 };
                if (isAdminorNot.ID_Role == 1)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
                else
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses2.Contains(a.ID_STATUS.Value) /*&& a.ATASAN == posid*/).OrderBy(a => a.APPROVAL_ID).ToList();

                    return Ok(new { Data = data });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListApproval_Daterange")]
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
                    query = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value));
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    query = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && a.ID_STATUS == 4);
                }
                else
                {
                    query = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) /*&& a.ATASAN == posid*/);
                }

                if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate) && DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
                {
                    // Filter data by date range
                    query = query.Where(a => a.WAKTU_ABSEN >= parsedStartDate && a.WAKTU_ABSEN <= parsedEndDate);
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
                    var cekRole = db.VW_Users.Where(a => a.Username == chambers.APPROVER).FirstOrDefault();
                    if (cekRole.ID_Role == 4)
                    {
                        cek.ID_STATUS = chambers.ID_STATUS;
                        cek.WAKTU_APPROVAL = DateTime.Now;
                        cek.APPROVER_PARAMEDIC = chambers.APPROVER;
                        cek.FLAG = 1;

                        db.SubmitChanges();
                        Exec_NotifOperator(cek.NRP, cek.APPROVAL_ID, cek.ID_CFC);

                        return Ok(new { Remarks = true });
                    }
                    else
                    {
                        cek.ID_STATUS = chambers.ID_STATUS;
                        cek.APPROVER = chambers.APPROVER;
                        cek.WAKTU_APPROVAL = DateTime.Now;
                        cek.FLAG = 1;

                        db.SubmitChanges();
                        Exec_NotifOperator(cek.NRP, cek.APPROVAL_ID, cek.ID_CFC);

                        return Ok(new { Remarks = true });
                    }
                    
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
                    var cekRole = db.VW_Users.Where(a => a.Username == chambers.APPROVER).FirstOrDefault();
                    if (cekRole.ID_Role == 4)
                    {
                        cek.ID_STATUS = chambers.ID_STATUS;
                        cek.WAKTU_APPROVAL = DateTime.Now;
                        cek.APPROVER_PARAMEDIC = chambers.APPROVER;
                        cek.FLAG = 1;

                        db.SubmitChanges();
                        Exec_NotifOperator(cek.NRP, cek.APPROVAL_ID, cek.ID_CFC);

                        return Ok(new { Remarks = true });
                    }
                    else
                    {
                        cek.ID_STATUS = chambers.ID_STATUS;
                        cek.APPROVER = chambers.APPROVER;
                        cek.WAKTU_APPROVAL = DateTime.Now;
                        cek.FLAG = 1;

                        db.SubmitChanges();
                        Exec_NotifOperator(cek.NRP, cek.APPROVAL_ID, cek.ID_CFC);

                        return Ok(new { Remarks = true });
                    }
                    
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
        [Route("Exec_NotifOperator")]
        public IHttpActionResult Exec_NotifOperator(string nrp, int approval_id, int? id_cfc)
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.cusp_insertPushNotifMOK_Operator(nrp, approval_id);
                var data2 = db.cusp_insertDataForInAppNotificationCFM_Operator(nrp, id_cfc);

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
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

        [HttpPost]
        [Route("Insert_Dummy_Data")]
        public IHttpActionResult Insert_Dummy_Data(List<TBL_T_CFC> param)
        {
            try
            {
                foreach (TBL_T_CFC p in param)
                {
                    TBL_T_CFC tbl = new TBL_T_CFC();
                    tbl.WAKTU_ABSEN = p.WAKTU_ABSEN;
                    tbl.NRP = p.NRP;
                    tbl.ID_CHAMBER = p.ID_CHAMBER;
                    tbl.OXYGEN_SATURATION = p.OXYGEN_SATURATION;
                    tbl.HEART_RATE = p.HEART_RATE;
                    tbl.SYSTOLIC = p.SYSTOLIC;
                    tbl.DIASTOLIC = p.DIASTOLIC;
                    tbl.TEMPRATURE = p.TEMPRATURE;
                    tbl.FACE_PICTURE_URL = p.FACE_PICTURE_URL;
                    tbl.ID_STATUS = p.ID_STATUS;
                    tbl.NOTE = p.NOTE;
                    tbl.ATTENDANCE_NOTE = p.ATTENDANCE_NOTE;

                    db.TBL_T_CFCs.InsertOnSubmit(tbl);
                }

                db.SubmitChanges();

                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

    }
}
