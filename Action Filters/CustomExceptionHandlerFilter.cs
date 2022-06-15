using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_WebApplication.Models.DomainModel;

namespace Test_WebApplication.Action_Filters
{
    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {

            ExceptionLogger logger = new ExceptionLogger()
            {
                ExceptionMessage = filterContext.Exception.Message,
                ExceptionStackTrack = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                ExceptionLogTime = DateTime.Now
            };
            TestApplicationEntities dbContext = new TestApplicationEntities();
            dbContext.ExceptionLoggers.Add(logger);
            dbContext.SaveChanges();
            filterContext.Controller.ViewData.Add("TestValue", "test");

            //TempData["InvalidMessage"] = "An account with the same email already exists.",



            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            { //MasterName = filterContext.Exception.Message,
              //ViewBag= "An account with the same email already exists.",
              // TempData = filterContext.Controller.TempData,
              //TempData["InvalidMessage"] = "An account with the same email already exists.",

                ViewName = "SwallowedException"
            };
        }
    }
}