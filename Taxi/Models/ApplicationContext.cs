using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
namespace Taxi.Models
{
    public class ApplicationContext:IdentityDbContext<ApplicationUser>
    {

        public ApplicationContext() : base("Db_taxi") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }


    }
}