using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using Test_WebApplication.Action_Filters;
using Test_WebApplication.Models.DomainModel;

namespace Test_WebApplication.Controllers
{
    public class ValueController : Controller
    {
        // GET: Value
        TestApplicationEntities db = new TestApplicationEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SlowRequest() => View();
        public ActionResult LoadSlowRequest(FormCollection form, string apicall = "")
        {
            string rate = form["execute"].ToString();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://sandboxapi.azurewebsites.net/api/values/");
                //HTTP GET
                var responseTask = client.GetAsync(apicall);
                responseTask.Wait();

                var result = responseTask.Result;
                var result1 = responseTask.Result.Content.ReadAsStringAsync().Result; 
                if (result.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine(result);
                    System.Diagnostics.Debug.WriteLine(result1);
                    System.Diagnostics.Debug.WriteLine("SlowRequest API call successful!");
                    TempData["ValidMessage"] = "SlowRequest API call successful!";
                }
                else //web api sent error response 
                {
                    System.Diagnostics.Debug.WriteLine(result);
                    System.Diagnostics.Debug.WriteLine("SlowRequest API call failed");
                    TempData["InvalidMessage"] = "SlowRequest API call failed";
                }
            }
            return RedirectToAction("SlowRequest");
        }
        public ActionResult SlowDB() => View();
        public ActionResult LoadSlowDB()
        {
            try
            {
                string connString = @"Server=DESKTOP-G0BP2H2\SQLEXPRESS;initial catalog=TestApplication;integrated security=True;Trusted_Connection = True;";
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //set stored procedure name
                    string spName = @"dbo.[SlowDB]";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(spName, conn);
                    //open connection
                    conn.Open();
                    //set the SqlCommand type to stored procedure and execute
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var myString = rdr["Name"].ToString();
                            System.Diagnostics.Debug.WriteLine(rdr["Id"].ToString());
                            System.Diagnostics.Debug.WriteLine(rdr["Email"].ToString());
                            System.Diagnostics.Debug.WriteLine(rdr["Name"].ToString());
                            System.Diagnostics.Debug.WriteLine(rdr["CreatedDate"].ToString());
                            System.Diagnostics.Debug.WriteLine("");
                            System.Diagnostics.Debug.WriteLine("");
                        }
                    }
                    //var result = cmd.ExecuteScalar();
                    //System.Diagnostics.Debug.WriteLine(result);
                    System.Diagnostics.Debug.WriteLine("SQL stored procedure successfully executed!");
                    TempData["validMessage"] = "SQL stored procedure successfully executed!";
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("SQL stored procedure not executed!");
                TempData["InvalidMessage"] = "SQL stored procedure not executed!";
            }
            //return result;

            return RedirectToAction("SlowDB");
        }
        public ActionResult Untracked() => View();
        public ActionResult LoadUntracked()
        {
            try
            {

                var getquery = db.Customers.ToList();
                System.Diagnostics.Debug.WriteLine("Untracked call successfully executed!");
                TempData["ValidMessage"] = "Untracked call successfully executed!";
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Untracked SQL query failed");
                TempData["InvalidMessage"] = "Untracked SQL query failed";
            }
            return View("Untracked");
        }
        public ActionResult ORM() => View();

        public ActionResult LoadORM()
        {
            try
            {
                var getquery = db.Customers.ToList();
                foreach (var item in getquery)
                {
                    System.Diagnostics.Debug.WriteLine(item.Id);
                    System.Diagnostics.Debug.WriteLine(item.Email);
                    System.Diagnostics.Debug.WriteLine(item.Name);
                    System.Diagnostics.Debug.WriteLine(item.CreatedDate.Value.Date);
                    System.Diagnostics.Debug.WriteLine("");
                    System.Diagnostics.Debug.WriteLine("");

                }

                System.Diagnostics.Debug.WriteLine("ORM SQL query executed succesfully");
                TempData["ValidMessage"] = "ORM SQL query executed succesfully";
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ORM SQL query failed");
                TempData["InvalidMessage"] = "ORM SQL query failed";
            }
            return View("ORM");
        }
        public ActionResult SwallowedException() => View();

        [CustomExceptionHandlerFilter]
        public ActionResult LoadSwallowedException()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://sandboxapi.azurewebsites.net/api/values/");
                    //HTTP GET
                    var responseTask = client.GetAsync("BADWEBREQUEST");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                }

            }
            catch (Exception E) { }
            finally
            {
                System.Diagnostics.Debug.WriteLine("SwallowedException successfully");
                TempData["ValidMessage"] = "SwallowedException successfully";
            }

            return RedirectToAction("SwallowedException");
        }


    }
}