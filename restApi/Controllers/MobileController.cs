using restApi.Models;
using System;
using System.Collections.Generic;
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
                var data = db.VW_T_CFCs.OrderByDescending(a => a.WAKTU_ABSEN)
                .Select(a => new
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
                }).Where(a => a.nrp == nrp).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
