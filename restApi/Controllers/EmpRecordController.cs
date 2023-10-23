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

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).ToList();

                    //mendapatkan descending NRP dengan status retest dan jumlah approval perhari 2,3,4
                    var listUser = data.Where(x => x.ID_STATUS == 5 && x.JUMLAH_APPROVAL_PERHARI >= 2).Select(x => x.NRP).Distinct().ToList();

                    List<VwLatest> filteredData = new List<VwLatest>();

                    foreach (var j in data)
                    {
                        VW_T_APPROVAL latestDataByUser = new VW_T_APPROVAL();

                        foreach (var i in listUser)
                        {
                            if(j.NRP == i)
                            {
                                latestDataByUser = data.Where(x => x.NRP == i).OrderByDescending(x => x.DATETIME_FROM_CFC).FirstOrDefault();
                                break;
                            }
                        }

                        if (j == latestDataByUser)
                        {
                            filteredData.Add(new VwLatest
                            {
                                NRP = j.NRP,
                                DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                                APPROVAL_ID = j.APPROVAL_ID,
                                NAME = j.NAME,
                                POS_TITLE = j.POS_TITLE,
                                ID_STATUS = j.ID_STATUS,
                                STATUS = j.STATUS,
                                WAKTU_ABSEN = j.WAKTU_ABSEN,
                                ID_CHAMBER = j.ID_CHAMBER,
                                JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                                OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                                HEART_RATE = (int)j.HEART_RATE,
                                SYSTOLIC = (int)j.SYSTOLIC,
                                DIASTOLIC = (int)j.DIASTOLIC,
                                TEMPRATURE = (int)j.TEMPRATURE,
                                NOTE = j.NOTE,
                                IS_LATEST = true
                            });
                        }
                        else
                        {
                            filteredData.Add(new VwLatest
                            {
                                NRP = j.NRP,
                                DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                                APPROVAL_ID = j.APPROVAL_ID,
                                NAME = j.NAME,
                                POS_TITLE = j.POS_TITLE,
                                ID_STATUS = j.ID_STATUS,
                                STATUS = j.STATUS,
                                WAKTU_ABSEN = j.WAKTU_ABSEN,
                                ID_CHAMBER = j.ID_CHAMBER,
                                JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                                OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                                HEART_RATE = (int)j.HEART_RATE,
                                SYSTOLIC = (int)j.SYSTOLIC,
                                DIASTOLIC = (int)j.DIASTOLIC,
                                TEMPRATURE = (int)j.TEMPRATURE,
                                NOTE = j.NOTE,
                                IS_LATEST = false
                            });
                        }

                    }

                    return Ok(new { Data = filteredData });
                }
                else if (isAdminorNot.ID_Role == 2)
                {
                    var data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).OrderBy(a => a.APPROVAL_ID).ToList();

                    //mendapatkan descending NRP dengan status retest dan jumlah approval perhari 2,3,4
                    var listUser = data.Where(x => x.ID_STATUS == 5 && x.JUMLAH_APPROVAL_PERHARI >= 2).Select(x => x.NRP).Distinct().ToList();

                    List<VwLatest> filteredData = new List<VwLatest>();

                    foreach (var j in data)
                    {
                        VW_T_APPROVAL latestDataByUser = new VW_T_APPROVAL();

                        foreach (var i in listUser)
                        {
                            if (j.NRP == i)
                            {
                                latestDataByUser = data.Where(x => x.NRP == i).OrderByDescending(x => x.DATETIME_FROM_CFC).FirstOrDefault();
                                break;
                            }
                        }

                        if (j == latestDataByUser)
                        {
                            filteredData.Add(new VwLatest
                            {
                                NRP = j.NRP,
                                DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                                APPROVAL_ID = j.APPROVAL_ID,
                                NAME = j.NAME,
                                POS_TITLE = j.POS_TITLE,
                                ID_STATUS = j.ID_STATUS,
                                STATUS = j.STATUS,
                                WAKTU_ABSEN = j.WAKTU_ABSEN,
                                ID_CHAMBER = j.ID_CHAMBER,
                                JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                                OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                                HEART_RATE = (int)j.HEART_RATE,
                                SYSTOLIC = (int)j.SYSTOLIC,
                                DIASTOLIC = (int)j.DIASTOLIC,
                                TEMPRATURE = (int)j.TEMPRATURE,
                                NOTE = j.NOTE,
                                IS_LATEST = true
                            });
                        }
                        else
                        {
                            filteredData.Add(new VwLatest
                            {
                                NRP = j.NRP,
                                DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                                APPROVAL_ID = j.APPROVAL_ID,
                                NAME = j.NAME,
                                POS_TITLE = j.POS_TITLE,
                                ID_STATUS = j.ID_STATUS,
                                STATUS = j.STATUS,
                                WAKTU_ABSEN = j.WAKTU_ABSEN,
                                ID_CHAMBER = j.ID_CHAMBER,
                                JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                                OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                                HEART_RATE = (int)j.HEART_RATE,
                                SYSTOLIC = (int)j.SYSTOLIC,
                                DIASTOLIC = (int)j.DIASTOLIC,
                                TEMPRATURE = (int)j.TEMPRATURE,
                                NOTE = j.NOTE,
                                IS_LATEST = false
                            });
                        }

                    }

                    return Ok(new { Data = filteredData });
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
        [Route("Get_ListEmprecord_Daterange")]
        public IHttpActionResult Get_ListEmprecord_Daterange(string posid, string startDate, string endDate)
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

                if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate) && DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate))
                {
                    query = query.Where(a => a.WAKTU_ABSEN >= parsedStartDate && a.WAKTU_ABSEN <= parsedEndDate);
                }

                var data = query.OrderBy(a => a.APPROVAL_ID).ToList();

                List<VwLatest> filteredData = GetFilteredData(data);
                return Ok(new { Data = filteredData });

                //return Ok(new { Data = data });
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
                var data = db.VW_T_APPROVALs.Where(a => a.NRP == nrp && a.DATE_FROM_CFC.ToString() == datefromcfc && a.JUMLAH_APPROVAL_PERHARI == jmlapprvlperhari).OrderByDescending(a => a.DATETIME_FROM_CFC).FirstOrDefault();

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

        public class VwLatest
        {
            public string NRP { get; set; }
            public DateTime? DATETIME_FROM_CFC { get; set; }
            public bool IS_LATEST { get; set; }
            public int APPROVAL_ID { get; set; }
            public string NAME { get; set; }
            public string POS_TITLE { get; set; }
            public int? ID_STATUS { get; set; }
            public string STATUS { get; set; }
            public DateTime? WAKTU_ABSEN { get; set; }
            public string ID_CHAMBER { get; set; }
            public int? JUMLAH_APPROVAL_PERHARI { get; set; }
            public int OXYGEN_SATURATION { get; set; }
            public int HEART_RATE { get; set; }
            public int SYSTOLIC { get; set; }
            public int DIASTOLIC { set; get; }
            public int TEMPRATURE { get; set; }
            public string NOTE { get; set; }
        }

        public List<VwLatest> GetFilteredData(IEnumerable<VW_T_APPROVAL> data)
        {
            var listUser = data
                .Where(x => x.ID_STATUS == 5 && x.JUMLAH_APPROVAL_PERHARI >= 2)
                .Select(x => x.NRP)
                .Distinct()
                .ToList();

            List<VwLatest> filteredData = new List<VwLatest>();

            foreach (var j in data)
            {
                VW_T_APPROVAL latestDataByUser = new VW_T_APPROVAL();

                foreach (var i in listUser)
                {
                    if (j.NRP == i)
                    {
                        latestDataByUser = data.Where(x => x.NRP == i).OrderByDescending(x => x.DATETIME_FROM_CFC).FirstOrDefault();
                        break;
                    }
                }

                if (j == latestDataByUser)
                {
                    filteredData.Add(new VwLatest
                    {
                        NRP = j.NRP,
                        DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                        APPROVAL_ID = j.APPROVAL_ID,
                        NAME = j.NAME,
                        POS_TITLE = j.POS_TITLE,
                        ID_STATUS = j.ID_STATUS,
                        STATUS = j.STATUS,
                        WAKTU_ABSEN = j.WAKTU_ABSEN,
                        ID_CHAMBER = j.ID_CHAMBER,
                        JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                        OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                        HEART_RATE = (int)j.HEART_RATE,
                        SYSTOLIC = (int)j.SYSTOLIC,
                        DIASTOLIC = (int)j.DIASTOLIC,
                        TEMPRATURE = (int)j.TEMPRATURE,
                        NOTE = j.NOTE,
                        IS_LATEST = true
                    });
                }
                else
                {
                    filteredData.Add(new VwLatest
                    {
                        NRP = j.NRP,
                        DATETIME_FROM_CFC = j.DATETIME_FROM_CFC,
                        APPROVAL_ID = j.APPROVAL_ID,
                        NAME = j.NAME,
                        POS_TITLE = j.POS_TITLE,
                        ID_STATUS = j.ID_STATUS,
                        STATUS = j.STATUS,
                        WAKTU_ABSEN = j.WAKTU_ABSEN,
                        ID_CHAMBER = j.ID_CHAMBER,
                        JUMLAH_APPROVAL_PERHARI = j.JUMLAH_APPROVAL_PERHARI,
                        OXYGEN_SATURATION = (int)j.OXYGEN_SATURATION,
                        HEART_RATE = (int)j.HEART_RATE,
                        SYSTOLIC = (int)j.SYSTOLIC,
                        DIASTOLIC = (int)j.DIASTOLIC,
                        TEMPRATURE = (int)j.TEMPRATURE,
                        NOTE = j.NOTE,
                        IS_LATEST = false
                    });
                }
            }

            return filteredData;
        }

    }
}
