using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Test_WebApplication.Models.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}