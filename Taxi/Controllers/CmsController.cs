using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Taxi.BLL;
using Taxi.Models;

using Taxi.BLL.managers;
using System.Web.Script.Serialization;
using Taxi.ActionFilters;
namespace Taxi.Controllers
{
    

    public class CmsController : Controller
    {

        private static readonly JavaScriptSerializer js = new JavaScriptSerializer();


        [Authorize (Roles = "Director")]
        [LayoutManager("_MainPageLayout")]
        public ActionResult Index()
        {

            cms_manager mng = new cms_manager();


          string currentUserRole =  mng.getCurrentUserRoles();
            ViewModelCms localModel = new ViewModelCms();
            localModel.position = currentUserRole;

            return View(localModel);
        }
        [Authorize(Roles = "Driver")]
        [LayoutManager("_MainPageLayout")]
        public ActionResult driver()
        {

            cms_manager mng = new cms_manager();


            string currentUserRole = mng.getCurrentUserRoles();
            ViewModelCms localModel = new ViewModelCms();
            localModel.position = currentUserRole;

            localModel.driverID = HttpContext.User.Identity.GetUserId();

            return View(localModel);
        }


        [HttpPost]
        public string getOrdersForDriver(string driverID, int pageSize, int pageNumber)
        {
            int res = -1;
            string jsObj = "";
            orders_manager mng = new orders_manager();
            int total = 0;
            try
            {
                jsObj = js.Serialize(mng.getOrdersForDriver(out total, driverID , pageSize, pageNumber));
                res = 1;
            }
            catch (Exception ex)
            {

                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return "{\"result\": " + res + " }";
            }







            string myString = "{\"result\": " + res + ", \"items\":" + jsObj + ", \"total\":" + total + " }";

            return myString;



        }







        [Authorize(Roles = "Dispatcher")]
        [LayoutManager("_MainPageLayout")]
        public ActionResult dispatcher()
        {

            cms_manager mng = new cms_manager();


            string currentUserRole = mng.getCurrentUserRoles();
            ViewModelCms localModel = new ViewModelCms();
            localModel.position = currentUserRole;

            return View(localModel);
        }


        [HttpPost]
        public int setDriver(int orderID, string driverID)
        {
            int res = -1;

            try
            {
                orders_manager mng = new orders_manager();

                tx_orders element = mng.getOrder(orderID);

                element.driver = driverID;

                res = mng.updateOrder(element);
            }
            catch(Exception ex)
            {
                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return res;
            }



            return res;

        }


        [HttpPost]
        public string getListOfDrivers()
        {
            
            int res = -1;
            string jsObj = "";
            cms_manager mng = new cms_manager();
            try
            {
                
                jsObj = js.Serialize(mng.getListOfDrivers());
                res = 1;


            }
            catch(Exception ex)
            {
                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return "{\"result\": " + res + " }";
            }

            string myString = "{\"result\": " + res + ", \"items\":" + jsObj + " }";

            return myString;

        }



        [HttpPost]
        public int cancelOrder(int orderID)
        {

            int res = -1;

            try
            {

                orders_manager mng = new orders_manager();

                tx_orders element = mng.getOrder(orderID);

                element.status = "canceled";
                element.driver = null;
                res = mng.updateOrder(element);



            }
            catch(Exception ex)
            {
                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return res;
            }


            return res;
        }

        [HttpPost]
        public int doneOrder(int orderID)
        {

            int res = -1;

            try
            {

                orders_manager mng = new orders_manager();

                tx_orders element = mng.getOrder(orderID);

                element.status = "done";
                
                res = mng.updateOrder(element);



            }
            catch (Exception ex)
            {
                Error_manager errorMng = new Error_manager();
                errorMng.LogError(ex);
                return res;
            }


            return res;
        }

        [HttpPost]
        public string getOrders(string Status, int pageSize, int pageNumber)
        {
            int res = -1;
            string jsObj = "";
            orders_manager mng = new orders_manager();
            int total = 0;
            try
            {
                jsObj = js.Serialize(mng.getOrdersByStatus(out total, Status, pageSize, pageNumber));
                res = 1;
            }
            catch (Exception ex)
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