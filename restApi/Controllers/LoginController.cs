﻿using restApi.Models;
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
            bool remarks = false;
            try
            {

                bool status = param.Login();

                remarks = status;

                return Ok(new { Remarks = remarks });
            }
            catch (Exception)
            {

                return Ok(remarks);
            }

        }
    }
}
