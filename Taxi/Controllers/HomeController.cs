using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taxi.ActionFilters;
using Taxi.Models;
using Taxi.BLL;
using System.Threading.Tasks;
namespace Taxi.Controllers
{
    public class HomeController : Controller
    {

        private HttpContext _context = System.Web.HttpContext.Current; 

        public bool IsAuthenticated
        {
            get
            {
                return _context.User != null &&
                       _context.User.Identity != null &&
                       _context.User.Identity.IsAuthenticated;
            }
        }


        [LayoutManager("_MainPageLayout")]
        public ActionResult Index()
        {

            ViewModelHome model = new ViewModelHome();
            cms_manager mng = new cms_manager();
            model.isAuthenticated = IsAuthenticated;

            if (IsAuthenticated)
            {
                model.userPhone = mng.getUserPhone();
            }


            return View(model);
        }



        [HttpPost]
        public async Task<string> addNewOrder(tx_orders element)
        {
            int res = -1;


            orders_manager mng = new orders_manager();

            res = mng.createNewOrder(element);
            cms_manager cms_mng = new cms_manager();

            int isUserExsist = cms_mng.isUserExist(element.phone);

            if (isUserExsist == -1)
            {
              int result = await cms_mng.createNewUser(element.phone);

                if (result == -1)
                {
                    res = -1;
                }
                else
                {
                   
                }

            }






            return "{\"result\": " + res + "}";


        } 
            



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}