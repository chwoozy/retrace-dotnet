using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Test_WebApplication.Action_Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        private const string LogOnSession = "UserId";
        private const string ErrorController = "Error";
        private const string LogOnController = "Account";
        private const string LogOnAction = "LogIn";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (ctx.Session[LogOnSession] == null && Convert.ToString(HttpContext.Current.Session["Email"]) == "")
            {
                ctx.Response.Redirect("~/Account/Login", false);
            }
            base.OnActionExecuting(filterContext);
            Log("OnActionExecuting", filterContext.RouteData);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
        }
    }
}