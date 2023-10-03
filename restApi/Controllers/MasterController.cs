using restApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace restApi.Controllers
{
    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpGet]
        [Route("Get_Employee")]
        public IHttpActionResult Get_Employee()
        {
            try
            {
                var data = db.TBL_R_MASTER_KARYAWAN_ALLs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Employee/{id}")]
        public IHttpActionResult Get_Employee(string id)
        {
            try
            {
                var data = db.VW_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == id).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Roled/{nrp}")]
        public IHttpActionResult Get_Roled(string nrp)
        {
            try
            {
                string nrps;
                if (nrp.Count() > 7)
                {
                    nrps = nrp.Substring(nrp.Length - 7);
                }
                else
                {
                    nrps = nrp;
                }

                var data = db.VW_Users.Where(a => a.Username == nrps).FirstOrDefault();

                return Ok(new { Remarks = true, Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Keperluan Transaksi
        [HttpGet]
        [Route("getDistrict")]
        public IHttpActionResult getDistrict()
        {
            try
            {
                var data = db.VW_DISTRICTs.OrderBy(a => a.DSTRCT_CODE).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
