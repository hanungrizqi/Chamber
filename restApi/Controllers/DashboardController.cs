using restApi.Models;
using restApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace restApi.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        #region Data Karyawan Masuk Chamber
        [HttpGet]
        [Route("TotalKaryawanMasuk/{posid}")]
        public IHttpActionResult TotalKaryawanMasuk(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    var data_1 = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).ToList();

                    var data_2 = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).OrderBy(a => a.APPROVAL_ID).OrderBy(a => a.APPROVAL_ID).ToList();

                    data_2.AddRange(data_1);
                    data = data_2;
                    //data = db.VW_T_APPROVALs.ToList();
                }
                else
                {
                    var data_1 = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid).OrderBy(a => a.APPROVAL_ID).ToList();

                    var data_2 = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).OrderBy(a => a.APPROVAL_ID).ToList();

                    data_2.AddRange(data_1);
                    data = data_2;
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("TotalKaryawanMasuk_Datepicker")]
        public IHttpActionResult TotalKaryawanMasuk_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    var data_1 = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) &&
                                    a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    var data_2 = db.VW_T_APPROVALs
                        .Where(a => (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) &&
                                    a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    data_2.AddRange(data_1);
                    data = data_2;
                }
                else
                {
                    var data_1 = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) &&
                                    a.ATASAN == posid && a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    var data_2 = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) &&
                                    a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    data_2.AddRange(data_1);
                    data = data_2;
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanMasuk/{posid}")]
        public IHttpActionResult UpdateterakhirKaryawanMasuk(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                var formattedDate = data.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanMasuk_Datepicker")]
        public IHttpActionResult UpdateterakhirKaryawanMasuk_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Data Karyawan Fit
        [HttpGet]
        [Route("TotalKaryawanFit/{posid}")]
        public IHttpActionResult TotalKaryawanFit(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 1).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 1).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("TotalKaryawanFit_Datepicker")]
        public IHttpActionResult TotalKaryawanFit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 1 && a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 1 && a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanFit/{posid}")]
        public IHttpActionResult UpdateterakhirKaryawanFit(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 1).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 1).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanFit_Datepicker")]
        public IHttpActionResult UpdateterakhirKaryawanFit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 1 && a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.ID_STATUS == 1 && a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Data Karyawan Unfit
        [HttpGet]
        [Route("TotalKaryawanUnfit/{posid}")]
        public IHttpActionResult TotalKaryawanUnfit(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 2).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 2).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanUnfit/{posid}")]
        public IHttpActionResult UpdateterakhirKaryawanUnfit(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 2).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 2).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[HttpGet]
        //[Route("UpdateterakhirKaryawanUnfit/{posid}")]
        //public IHttpActionResult UpdateterakhirKaryawanUnfit(string posid)
        //{
        //    try
        //    {
        //        db.CommandTimeout = 120;
        //        var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
        //        VW_T_APPROVAL data = null;

        //        if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
        //        {
        //            data = db.VW_T_APPROVALs
        //                .Where(a => a.ID_STATUS == 2)
        //                .OrderByDescending(a => a.DATETIME_FROM_CFC)
        //                .ThenByDescending(a => a.WAKTU_APPROVAL)
        //                .FirstOrDefault();
        //        }
        //        else
        //        {
        //            data = db.VW_T_APPROVALs
        //                .Where(a => a.ATASAN == posid && a.ID_STATUS == 2)
        //                .OrderByDescending(a => a.WAKTU_APPROVAL)
        //                .ThenByDescending(a => a.DATETIME_FROM_CFC)
        //                .FirstOrDefault();
        //        }

        //        if (data != null)
        //        {
        //            var formattedDate = data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC;
        //            var formattedDateString = formattedDate.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
        //            return Ok(new { Data = data, Tanggal = formattedDateString });
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpGet]
        [Route("TotalKaryawanUnfit_Datepicker")]
        public IHttpActionResult TotalKaryawanUnfit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 2 && a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 2 && a.DATETIME_FROM_CFC >= startDateDateTime && a.DATETIME_FROM_CFC <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanUnfit_Datepicker")]
        public IHttpActionResult UpdateterakhirKaryawanUnfit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 2 && a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.ID_STATUS == 2 && a.DATETIME_FROM_CFC >= startDateTime && a.DATETIME_FROM_CFC <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Sudah Approved
        [HttpGet]
        [Route("sudahapproved/{posid}")]
        public IHttpActionResult sudahapproved(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("sudahapprovedDate/{posid}")]
        public IHttpActionResult sudahapprovedDate(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                //var formattedDate = data.WAKTU_APPROVAL.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("sudahapproved_Datepicker")]
        public IHttpActionResult sudahapproved_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("sudahApprovedDate_Datepicker")]
        public IHttpActionResult sudahApprovedDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Butuh Approve
        [HttpGet]
        [Route("butuhapproval/{posid}")]
        public IHttpActionResult butuhapproval(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).ToList();
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("butuhapprovalDate/{posid}")]
        public IHttpActionResult butuhapprovalDate(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                //var formattedDate = data.DATETIME_FROM_CFC.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("butuhApproval_Datepicker")]
        public IHttpActionResult butuhApproval_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("butuhApprovalDate_Datepicker")]
        public IHttpActionResult butuhApprovalDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else if (isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 4 && a.APPROVER_PARAMEDIC == null && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Retest
        [HttpGet]
        [Route("retest/{posid}")]
        public IHttpActionResult retest(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.ATASAN == posid).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("retestDate/{posid}")]
        public IHttpActionResult retestDate(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.ATASAN == posid).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("retest_Datepicker")]
        public IHttpActionResult retest_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("retestDate_Datepicker")]
        public IHttpActionResult retestDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 5 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 5 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Tidak Dapat Bekerja
        [HttpGet]
        [Route("tdkdptbekerja/{posid}")]
        public IHttpActionResult tdkdptbekerja(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.ATASAN == posid).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tdkdptbekerjaDate/{posid}")]
        public IHttpActionResult tdkdptbekerjaDate(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.ATASAN == posid).OrderByDescending(a => a.APPROVAL_ID).FirstOrDefault();
                }

                var formattedDate = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                //var gmtPlus7Time = (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.AddHours(7);
                //var formattedDate = gmtPlus7Time.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));


                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tdkdptbekerja_Datepicker")]
        public IHttpActionResult tdkdptbekerja_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //List<VW_T_APPROVAL> data = null;
                List<VW_T_APPROVAL> data = new List<VW_T_APPROVAL>();

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tdkdptbekerjaDate_Datepicker")]
        public IHttpActionResult tdkdptbekerjaDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                //VW_T_APPROVAL data = null;
                VW_T_APPROVAL data = new VW_T_APPROVAL();

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1 || isAdminorNot.ID_Role == 4)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 7 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 7 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.APPROVAL_ID)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? (data.WAKTU_APPROVAL ?? data.DATETIME_FROM_CFC).Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Cham 001
        [HttpGet]
        [Route("cham001")]
        public IHttpActionResult cham001(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 1);

                    if (data != null)
                    {
                        return Ok(new { USDTDY = data.USDTDY });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var filteredData = data.Where(a => a.ID == 1).ToList();
                    var USDTDY = filteredData.Select(d => d.USDTDY).FirstOrDefault();
                    return Ok(new { USDTDY });
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001Date")]
        public IHttpActionResult cham001Date(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    VW_R_CFM_MANAGEMENT data = new VW_R_CFM_MANAGEMENT();

                    data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 1);

                    if (data != null)
                    {
                        var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                        return Ok(new { Data = data, Tanggal = formattedDate });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var date = data.Where(a => a.ID == 1).ToList();
                    var datee = date.Select(d => d.LSTUSD).FirstOrDefault();
                    var formattedDate = datee.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001_Datepicker")]
        public IHttpActionResult cham001_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                int? USDTDY = 0;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM001").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM001").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }

                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001Date_Datepicker")]
        public IHttpActionResult cham001Date_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM001").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM001").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Cham 002
        [HttpGet]
        [Route("cham002")]
        public IHttpActionResult cham002(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 2);

                    if (data != null)
                    {
                        return Ok(new { USDTDY = data.USDTDY });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var filteredData = data.Where(a => a.ID == 2).ToList();
                    var USDTDY = filteredData.Select(d => d.USDTDY).FirstOrDefault();
                    return Ok(new { USDTDY });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002Date")]
        public IHttpActionResult cham002Date(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    VW_R_CFM_MANAGEMENT data = new VW_R_CFM_MANAGEMENT();

                    data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 2);

                    if (data != null)
                    {
                        var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                        return Ok(new { Data = data, Tanggal = formattedDate });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var date = data.Where(a => a.ID == 2).ToList();
                    var datee = date.Select(d => d.LSTUSD).FirstOrDefault();
                    var formattedDate = datee.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002_Datepicker")]
        public IHttpActionResult cham002_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                int? USDTDY = 0;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM002").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM002").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                
                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002Date_Datepicker")]
        public IHttpActionResult cham002Date_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM002").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM002").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Cham 003
        [HttpGet]
        [Route("cham003")]
        public IHttpActionResult cham003(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 3);

                    if (data != null)
                    {
                        return Ok(new { USDTDY = data.USDTDY });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var filteredData = data.Where(a => a.ID == 3).ToList();
                    var USDTDY = filteredData.Select(d => d.USDTDY).FirstOrDefault();
                    return Ok(new { USDTDY });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003Date")]
        public IHttpActionResult cham003Date(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    VW_R_CFM_MANAGEMENT data = new VW_R_CFM_MANAGEMENT();

                    data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 3);

                    if (data != null)
                    {
                        var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                        return Ok(new { Data = data, Tanggal = formattedDate });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var date = data.Where(a => a.ID == 3).ToList();
                    var datee = date.Select(d => d.LSTUSD).FirstOrDefault();
                    var formattedDate = datee.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003_Datepicker")]
        public IHttpActionResult cham003_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                int? USDTDY = 0;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM003").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM003").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                
                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003Date_Datepicker")]
        public IHttpActionResult cham003Date_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM003").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM003").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region Cham 004
        [HttpGet]
        [Route("cham004")]
        public IHttpActionResult cham004(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 4);

                    if (data != null)
                    {
                        return Ok(new { USDTDY = data.USDTDY });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var filteredData = data.Where(a => a.ID == 4).ToList();
                    var USDTDY = filteredData.Select(d => d.USDTDY).FirstOrDefault();
                    return Ok(new { USDTDY });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004Date")]
        public IHttpActionResult cham004Date(string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    VW_R_CFM_MANAGEMENT data = new VW_R_CFM_MANAGEMENT();

                    data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 4);

                    if (data != null)
                    {
                        var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                        return Ok(new { Data = data, Tanggal = formattedDate });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var data = db.cufn_getCFMManagement_forGL(posid).ToList();

                    var date = data.Where(a => a.ID == 4).ToList();
                    var datee = date.Select(d => d.LSTUSD).FirstOrDefault();
                    var formattedDate = datee.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004_Datepicker")]
        public IHttpActionResult cham004_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                int? USDTDY = 0;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM004").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        data = data.Where(d => d.ID_CHAMBER == "CHAM004").ToList();
                    }

                    USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                    if (USDTDY == null)
                    {
                        USDTDY = 0;
                    }
                }
                
                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004Date_Datepicker")]
        public IHttpActionResult cham004Date_Datepicker(string startDate, string endDate, string posid)
        {
            try
            {
                db.CommandTimeout = 120;
                var checkRole = db.VW_Users.Where(a => a.POSITION_ID == posid).FirstOrDefault();
                if (checkRole.ID_Role == 1 || checkRole.ID_Role == 4)
                {
                    List<cufn_filterCFMManagementResult> data = new List<cufn_filterCFMManagementResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement(parsedStartDate, parsedEndDate).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM004").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }
                else
                {
                    List<cufn_filterCFMManagement_forGLResult> data = new List<cufn_filterCFMManagement_forGLResult>();

                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(startDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(endDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        data = db.cufn_filterCFMManagement_forGL(parsedStartDate, parsedEndDate, posid).ToList();

                        var filteredData = data.Where(d => d.ID_CHAMBER == "CHAM004").ToList();

                        if (filteredData.Any())
                        {
                            var mostRecentItem = filteredData.OrderByDescending(d => d.LSTUSD).First();
                            var formattedDate = mostRecentItem.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));
                            return Ok(new { Data = mostRecentItem, Tanggal = formattedDate });
                        }
                        else
                        {
                            return Ok(new { Tanggal = "-" });
                        }
                    }
                }

                return Ok(new { Remarks = true });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
