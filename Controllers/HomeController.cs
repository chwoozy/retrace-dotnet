using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test_WebApplication.Action_Filters;
using Test_WebApplication.Models.DomainModel;
using Test_WebApplication.Services;

namespace Test_WebApplication.Controllers
{
    [CustomActionFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var service = new CustomerService();
            ViewBag.LoadCustomer = service.LoadTODo();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SaveToDoc(ToDoc toDoc)
        {

            var service = new CustomerService();
            var resp = service.SaveToDoc(toDoc);

            if (!resp)
                TempData["InvalidMessage"] = "Something went wrong.please try again";

            return Json(new { Message = resp, JsonRequestBehavior.AllowGet });

        }
        public ActionResult RemoveToDoc(int id)
        {
            var service = new CustomerService();
            var resp = service.RemoveToDoc(id);

            if (!resp)
                TempData["InvalidMessage"] = "Something went wrong.please try again";

            return Json(new { Message = resp, JsonRequestBehavior.AllowGet });

        }

    }
}