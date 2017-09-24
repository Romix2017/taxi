using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.Interfaces;
namespace Taxi.Models
{
    public class ViewModelUserCabinet: ILayoutLogin
    {

        public string UserPhone { get; set; }
        public string actionLink { get; set; }
        public string controllerLink { get; set; }



    }
}