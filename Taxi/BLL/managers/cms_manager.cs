using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.BLL.managers;
using Taxi.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Taxi.DAL;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;


namespace Taxi.BLL
{
    public class cms_manager
    {

        #region System
        public Repository db;
        private bool _disposed;
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        public cms_manager()
        {
            db = new Repository();
            _disposed = false;
            //SERIALIZE WILL FAIL WITH PROXIED ENTITIES
            //dbContext.Configuration.ProxyCreationEnabled = false;
            //ENABLING COULD CAUSE ENDLESS LOOPS AND PERFORMANCE PROBLEMS
            //dbContext.Configuration.LazyLoadingEnabled = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (db != null)
                        db.Dispose();
                }
                db = null;
                _disposed = true;
            }
        }
        #endregion
        
        public string getUserPhone()
        {
            string res = "";

            try
            {
                using (var context = new ApplicationContext())
                {


                    string userID = HttpContext.Current.User.Identity.GetUserId();


                        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    ApplicationUser user = manager.Users.SingleOrDefault(x => x.Id == userID);
                    res = user.PhoneNumber;



                }

                    

            }
            catch(Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }


            return res;
        }


        public int isUserExist (string phoneNumber)
        {
            int res = -1;


            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationContext()));
                var userView = manager.Users.Where(x => x.PhoneNumber == phoneNumber).ToList();

                if (userView.Count() > 0)
                {
                    res = 1;
                }




            }
            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }


            return res;

        }


        public async Task<int> InvisibleLogin(ApplicationUser user)
        {

            int res = -1;

        try
            {

                ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                         DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, claim);
                res = 1;


            }
            catch(Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }




            return res;



        }



        public async Task<int> createNewUser(string phoneNumber)
        {

            int res = -1;

            try
            {
                using (var context = new ApplicationContext())
                {
                   



                    var newUser = new ApplicationUser() { UserName = Regex.Replace(phoneNumber, @"[^\d]", String.Empty), Email = "customer@mail.ru", PhoneNumber=phoneNumber };

                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var result = await userManager.CreateAsync(newUser, "1234567");

                    if (!result.Succeeded)
                    {
                        
                    }

                    

                     if (newUser.Id != null)
                     {
                        userManager.AddToRole(newUser.Id, "Customer");
                      int isLogedIn = await InvisibleLogin(newUser);

                        if (isLogedIn == -1)
                        {
                            return res;
                        }


                     }

                    

                    res = 1;
                }



                return res;

            }
            catch(Exception ex)
            {

                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;

            }





        }

        public List<driverModel> getListOfDrivers()
        {
            List<driverModel> res = new List<driverModel>();
            var user_manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationContext()));
          var role_manager =  new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationContext()));


            try {
                string roleName = "Driver";
                var role = role_manager.Roles.Single(r => r.Name == roleName);

                // Find the users in that role
                var roleUsers = user_manager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id.ToString())).ToList();
                
                foreach (var item in roleUsers)
                {
                    driverModel element = new driverModel();

                    element.userID = item.Id;
                    element.userName = item.UserName;

                    res.Add(element);

                }

            }

            catch(Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            return res;

        }


        public string getCurrentUserRoles(ApplicationUser userObject = null)
        {
            string res = "";
            List<string> listOfUserRoles = null;
            try
            {

              //  listOfUserRoles = Roles.GetRolesForUser().ToList();



                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationContext()));

                ApplicationUser user = null;

                if (userObject == null)
                {
                    user = manager.FindByName(HttpContext.Current.User.Identity.Name);
                }
                else
                {
                    user = userObject;
                }

                


                listOfUserRoles = manager.GetRoles(user.Id).ToList();



            }

            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            res = listOfUserRoles.FirstOrDefault();
            return res;

        }






    }
}