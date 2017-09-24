using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.Interfaces;
namespace Taxi.Models
{
    public class ViewModelHome: ILayoutLogin
    {
        public bool isAuthenticated { get; set; }
        public string userPhone { get; set; }
        public string controllerLink { get; set; }
        public string actionLink { get; set; }
    }
}