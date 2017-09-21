using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taxi.ActionFilters;
using Microsoft.AspNet.Identity;
using Taxi.Models;
using Taxi.BLL;
using Taxi.BLL.managers;
using System.Web.Script.Serialization;

namespace Taxi.Controllers
{
    public class UserCabinetController : Controller
    {

        public string UserPhone { get; set; }
        private static readonly JavaScriptSerializer js = new JavaScriptSerializer();
        [LayoutManager("_MainPageLayout")]
        [Authorize (Roles = "Customer")]
        public ActionResult Index()
        {
            cms_manager mng = new cms_manager();


            UserPhone = mng.getUserPhone();

            ViewModelUserCabinet model = new ViewModelUserCabinet();
            model.UserPhone = UserPhone;

            return View(model);
        }


        [HttpPost]
        public string getOrders(string UserPhone, int pageSize, int pageNumber)
        {
            int res = -1;
            string jsObj = "";
            orders_manager mng = new orders_manager();
            int total = 0;
            try
            {
                jsObj = js.Serialize(mng.getOrdersByPhone(out total, UserPhone, pageSize, pageNumber));
                res = 1;
            }
            catch(Exception ex)
            {

                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return "{\"result\": " + res + " }";
            }







            string myString = "{\"result\": " + res + ", \"items\":" + jsObj + ", \"total\":" + total + " }";

            return myString;



        }


    }
}