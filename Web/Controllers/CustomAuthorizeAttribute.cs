using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            CfmDataContext db = new CfmDataContext();

            int userRole = Convert.ToInt32(httpContext.Session["ID_Role"]);

            var routeData = httpContext.Request.RequestContext.RouteData;
            var actionName = routeData.GetRequiredString("action");
            var controllerName = routeData.GetRequiredString("controller");

            string URL = $"/{controllerName}/{actionName}";

            var allowedRoles = db.VW_AUTORIZE_MENUs.Where(a => a.Link_Menu == URL && a.ID == userRole).Select(a => a.ID).ToList();

            var isAuthorized = allowedRoles.Contains(userRole);

            return isAuthorized;
        }
    }
}