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
    [RoutePrefix("api/Mobile")]
    public class MobileController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpGet]
        [Route("Dashboard")]
        public IHttpActionResult Dashboard()
        {
            try
            {
                var data = db.VW_T_CFCs.Select(a => new
                {
                    nama = a.NAME,
                    umur = a.AGE,
                    tanggal_check = a.WAKTU_ABSEN,
                    temperatur_badan = a.TEMPRATURE,
                    sys = a.SYSTOLIC,
                    dia = a.DIASTOLIC,
                    spo = a.OXYGEN_SATURATION,
                    heart = a.HEART_RATE,
                    nrp = a.NRP
                }).ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Dashboards")]
        public IHttpActionResult Dashboards(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs.OrderByDescending(a => a.WAKTU_ABSEN)
                .Select(a => new
                {
                    nrp = a.NRP,
                    nama = a.NAME,
                    umur = a.AGE,
                    update_terakhir = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    status = a.STATUS,
                    temperatur_badan = $"{a.TEMPRATURE} C",
                    systolic = a.SYSTOLIC,
                    diastolic = a.DIASTOLIC,
                    oxygen = $"{a.OXYGEN_SATURATION}%",
                    heart_rate = $"{a.HEART_RATE} Bpm",
                }).Where(a => a.nrp == nrp).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Detail_Temperature")]
        public IHttpActionResult Detail_Temperature(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs
                .Select(a => new
                {
                    nrp = a.NRP,
                    history_tanggal = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    idchamber = a.ID_CHAMBER,
                    temperatur_badan = $"{a.TEMPRATURE} C"
                }).Where(a => a.nrp == nrp).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Detail_Tekanan_Darah")]
        public IHttpActionResult Detail_Tekanan_Darah(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs
                .Select(a => new
                {
                    nrp = a.NRP,
                    history_tanggal = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    idchamber = a.ID_CHAMBER,
                    tekanan_darah = $"SYS ({a.SYSTOLIC}) / DIA ({a.DIASTOLIC})"
                }).Where(a => a.nrp == nrp).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Detail_Oxygen")]
        public IHttpActionResult Detail_Oxygen(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs
                .Select(a => new
                {
                    nrp = a.NRP,
                    history_tanggal = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    idchamber = a.ID_CHAMBER,
                    spo02 = $"{a.OXYGEN_SATURATION}%",
                }).Where(a => a.nrp == nrp).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Detail_Heart_Rate")]
        public IHttpActionResult Detail_Heart_Rate(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs
                .Select(a => new
                {
                    nrp = a.NRP,
                    history_tanggal = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    idchamber = a.ID_CHAMBER,
                    heart_rate = $"{a.HEART_RATE} Bpm"
                }).Where(a => a.nrp == nrp).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Detail_Data_Kesehatan")]
        public IHttpActionResult Detail_Data_Kesehatan(string nrp)
        {
            try
            {
                var data = db.VW_R_MOBILEs
                .Select(a => new
                {
                    nrp = a.NRP,
                    history_tanggal = a.WAKTU_ABSEN.Value.ToString("d MMMM yyyy | HH:mm", new CultureInfo("id-ID")),
                    idchamber = a.ID_CHAMBER,
                    temperatur_badan = $"{a.TEMPRATURE} C",
                    tekanan_darah = $"SYS ({a.SYSTOLIC}) / DIA ({a.DIASTOLIC})",
                    spo02 = $"{a.OXYGEN_SATURATION}%",
                    heart_rate = $"{a.HEART_RATE} Bpm"
                }).Where(a => a.nrp == nrp).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
