using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.DAL;
using Taxi.Models;
namespace Taxi.BLL.managers
{
    public class Error_manager
    {

        #region System
        public  Repository db;
        private bool _disposed;

        public Error_manager()
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

          public  void LogError(Exception ex, string additional = "", object parameters = null)
            {



                if (HttpContext.Current == null || HttpContext.Current.Request.IsLocal)
                    return;

                if (ex.InnerException != null) ex = ex.InnerException;

                // get the current date and time
                string dateTime = DateTime.Now.ToLongDateString() + " "
                + DateTime.Now.ToShortTimeString();
                // stores the error message
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string errorMessage = "<br><b>Страница:</b> " + context.Request.Url.Host + context.Request.RawUrl;
                // build the error message
                errorMessage += "<br><b>Сообщение:</b> " + ex.Message + "<br /><hr /><br />";
                errorMessage += "<b>Время возникновения ошибки:</b> " + dateTime;
                // obtain the page that generated the error
                errorMessage += "<br><b>Источник:</b> " + ex.Source;
                errorMessage += "<br><b>Метод:</b> " + ex.TargetSite;
                errorMessage += "<br>Дополнительно: " + additional;

                errorMessage += "<br><b>Стек трассировки:</b><br>" + ex.StackTrace;

           
                db.SaveTrace(new tx_trace { code = "exception", Id = 0, created = DateTime.Now, header = ex.Message + " " + additional, itemID = 0, text = errorMessage + " " + context.Request.Url.Host + " " + context.Request.RawUrl });
              
            }

        }
}