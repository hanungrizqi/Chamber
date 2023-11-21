using restApi.Models;
using restApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace restApi.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        CFMDataContext db = new CFMDataContext();

        [HttpPost]
        [Route("Get_Login")]
        public IHttpActionResult Get_Login(ClsLogin param)
        {
            //bool remarks = false;
            try
            {

                var status = param.Login();

                //remarks = status.Status;

                return Ok(new { Remarks = status.Status, Message = status.Message });
            }
            catch (Exception ex)
            {

                return Ok(new { Remarks = false, Message = ex.Message });
            }

        }
    }
}
