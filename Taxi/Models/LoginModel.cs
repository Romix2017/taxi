using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Taxi.Interfaces;
namespace Taxi.Models
{
    public class LoginModel:ILayoutLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string actionLink { get; set; }
        public string controllerLink { get; set; }

    }
}