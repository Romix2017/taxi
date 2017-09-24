using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.Interfaces;
namespace Taxi.Models
{
    public class ViewModelCms: ILayoutLogin
    {

        public string position { get; set; }

        public string driverID { get; set; }

        public string actionLink { get; set; }

        public string controllerLink { get; set; }

    }
}