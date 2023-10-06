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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
        [Route("TotalKaryawanMasuk_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult TotalKaryawanMasuk_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    var data_1 = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) &&
                                    a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    var data_2 = db.VW_T_APPROVALs
                        .Where(a => (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) &&
                                    a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    data_2.AddRange(data_1);
                    data = data_2;
                }
                else
                {
                    var data_1 = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) &&
                                    a.ATASAN == posid && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime)
                        .OrderBy(a => a.APPROVAL_ID)
                        .ToList();

                    var data_2 = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) &&
                                    a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime)
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.FLAG == 0).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanMasuk_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult UpdateterakhirKaryawanMasuk_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.FLAG == 0 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
        [Route("TotalKaryawanFit_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult TotalKaryawanFit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 1 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 1 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 1).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanFit_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult UpdateterakhirKaryawanFit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 1 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.ID_STATUS == 1 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 2).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 2).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("TotalKaryawanUnfit_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult TotalKaryawanUnfit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 2 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && a.ID_STATUS == 2 && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
                }

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("UpdateterakhirKaryawanUnfit_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult UpdateterakhirKaryawanUnfit_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 2 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && a.ID_STATUS == 2 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value))).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("sudahapproved_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult sudahapproved_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
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
        [Route("sudahApprovedDate_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult sudahApprovedDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ATASAN == posid && (a.FLAG == 1 || excludedStatuses.Contains(a.ID_STATUS.Value)) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).ToList();
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value)).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("butuhApproval_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult butuhApproval_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateDateTime && a.WAKTU_ABSEN <= endDateDateTime).ToList();
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
        [Route("butuhApprovalDate_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult butuhApprovalDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                var excludedStatuses = new[] { 1, 5, 6, 7 };
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.FLAG == 0 && !excludedStatuses.Contains(a.ID_STATUS.Value) && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 5 && a.ATASAN == posid).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("retest_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult retest_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
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
        [Route("retestDate_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult retestDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 5 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 5 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

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
                List<VW_T_APPROVAL> data = null;

                if (isAdminorNot.ID_Role == 1)
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
                VW_T_APPROVAL data = null;

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs.Where(a => a.ID_STATUS == 7 && a.ATASAN == posid).OrderByDescending(a => a.WAKTU_ABSEN).FirstOrDefault();
                }

                var formattedDate = data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                return Ok(new { Data = data, Tanggal = formattedDate });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("tdkdptbekerja_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult tdkdptbekerja_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                List<VW_T_APPROVAL> data = null;

                // Konversi string tanggal awal dan tanggal akhir menjadi DateTime
                DateTime startDateDateTime = DateTime.Parse(startDate);
                DateTime endDateDateTime = DateTime.Parse(endDate);

                if (isAdminorNot.ID_Role == 1)
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
        [Route("tdkdptbekerjaDate_Datepicker/{posid}/{startDate}/{endDate}")]
        public IHttpActionResult tdkdptbekerjaDate_Datepicker(string posid, string startDate, string endDate)
        {
            try
            {
                db.CommandTimeout = 120;
                var isAdminorNot = db.VW_Users.Where(c => c.POSITION_ID == posid).FirstOrDefault();
                VW_T_APPROVAL data = null;

                DateTime startDateTime = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime endDateTime = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                if (isAdminorNot.ID_Role == 1)
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 7 && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }
                else
                {
                    data = db.VW_T_APPROVALs
                        .Where(a => a.ID_STATUS == 7 && a.ATASAN == posid && a.WAKTU_ABSEN >= startDateTime && a.WAKTU_ABSEN <= endDateTime)
                        .OrderByDescending(a => a.WAKTU_ABSEN)
                        .FirstOrDefault();
                }

                var formattedDate = data != null ? data.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")) : "-";

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
        public IHttpActionResult cham001()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 1);

                if (data != null)
                {
                    return Ok(new { USDTDY = data.USDTDY });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001Date")]
        public IHttpActionResult cham001Date()
        {
            try
            {
                db.CommandTimeout = 120;
                VW_R_CFM_MANAGEMENT data = null;

                data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 1);

                if (data != null)
                {
                    var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham001_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
                    data = data.Where(d => d.ID_CHAMBER == "CHAM001").ToList();
                }

                // Mengambil nilai USDTDY dari data
                int? USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                if (USDTDY == null)
                {
                    USDTDY = 0;
                }

                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham001Date_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham001Date_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
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

                return NotFound();
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
        public IHttpActionResult cham002()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 2);

                if (data != null)
                {
                    return Ok(new { USDTDY = data.USDTDY });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002Date")]
        public IHttpActionResult cham002Date()
        {
            try
            {
                db.CommandTimeout = 120;
                VW_R_CFM_MANAGEMENT data = null;

                data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 2);

                if (data != null)
                {
                    var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham002_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
                    data = data.Where(d => d.ID_CHAMBER == "CHAM002").ToList();
                }

                // Mengambil nilai USDTDY dari data
                int? USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                if (USDTDY == null)
                {
                    USDTDY = 0;
                }

                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham002Date_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham002Date_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
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

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion

        #region CHam 003
        [HttpGet]
        [Route("cham003")]
        public IHttpActionResult cham003()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 3);

                if (data != null)
                {
                    return Ok(new { USDTDY = data.USDTDY });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003Date")]
        public IHttpActionResult cham003Date()
        {
            try
            {
                db.CommandTimeout = 120;
                VW_R_CFM_MANAGEMENT data = null;

                data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 3);

                if (data != null)
                {
                    var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham003_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
                    data = data.Where(d => d.ID_CHAMBER == "CHAM003").ToList();
                }

                // Mengambil nilai USDTDY dari data
                int? USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                if (USDTDY == null)
                {
                    USDTDY = 0;
                }

                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham003Date_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham003Date_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
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

                return NotFound();
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
        public IHttpActionResult cham004()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 4);

                if (data != null)
                {
                    return Ok(new { USDTDY = data.USDTDY });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004Date")]
        public IHttpActionResult cham004Date()
        {
            try
            {
                db.CommandTimeout = 120;
                VW_R_CFM_MANAGEMENT data = null;

                data = db.VW_R_CFM_MANAGEMENTs.FirstOrDefault(a => a.ID == 4);

                if (data != null)
                {
                    var formattedDate = data.LSTUSD.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID"));

                    return Ok(new { Data = data, Tanggal = formattedDate });
                }
                else
                {
                    // Handle jika data tidak ditemukan
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham004_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
                    data = data.Where(d => d.ID_CHAMBER == "CHAM004").ToList();
                }

                // Mengambil nilai USDTDY dari data
                int? USDTDY = data.Select(d => d.USDTDY).FirstOrDefault();
                if (USDTDY == null)
                {
                    USDTDY = 0;
                }

                return Ok(new { USDTDY });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("cham004Date_Datepicker/{startDate}/{endDate}")]
        public IHttpActionResult cham004Date_Datepicker(string startDate, string endDate)
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

                    // Filter the data to keep only rows where ID_CHAMBER is "CHAM001"
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

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
