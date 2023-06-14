using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Interview_Tracker.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Please Enter valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}