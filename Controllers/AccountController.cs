using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Test_WebApplication.Models.ViewModel;
using Test_WebApplication.Services;

namespace Test_WebApplication.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Uri baseAddress = new Uri("https://sandboxapi.azurewebsites.net/api/users");
        HttpClient client;
        public AccountController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.IsLoginOrRegister = true;
            return View();
        }
        public ActionResult SaveLogin(LoginModel login)
        {
            ViewBag.IsLoginOrRegister = true;
            var service = new CustomerService();
            var getUserLogged = service.Login(login);
            if (getUserLogged != null && !string.IsNullOrEmpty(getUserLogged.Email))
            {
                HttpContext.Session.Add("Name", getUserLogged.Name);
                HttpContext.Session.Add("Email", getUserLogged.Email);
                return RedirectToAction("Index", "Home");
            }
            TempData["InvalidMessage"] = "These credentials do not match our records.";
            return RedirectToAction("Login");
        }
        public ActionResult Registration()
        {
            ViewBag.IsLoginOrRegister = true;
            return View();
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public ActionResult SaveRegistration(RegistrationModel registration)
        {
            var service = new CustomerService();
            var resp = service.SaveCustomer(registration);
            if (resp == 1)
                return RedirectToAction("Login");
            else if (resp == 3)
                TempData["InvalidMessage"] = "An account with the same email already exists.";
            else
                TempData["InvalidMessage"] = "Something went wrong.please try again";
            return RedirectToAction("Registration");
        }
    }
}