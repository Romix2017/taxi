using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Taxi.DAL;
using Taxi.Models;
using Taxi.BLL.managers;
namespace Taxi.BLL
{
    public class orders_manager
    {
        #region System
        public Repository db;
        private bool _disposed;

        public orders_manager()
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

        #region CRUD orders
        public int createNewOrder(tx_orders element)
        {
            int res = -1;
            Repository mng = new Repository();

            try
            {
              res =   mng.addOrder(element);
            }
            catch(Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                return res;
            }


            return res;
         }

        //
        public List<tx_orders> getOrdersForDriver(out int total, string driverID = "", int pageSize = 10, int pageNumber = 1)
        {
            List<tx_orders> res = new List<tx_orders>();
            try
            {

                Repository mng = new Repository();
                IQueryable<tx_orders> myQuery = null;
                
                myQuery = mng.getOrders().Where(x => x.driver == driverID).OrderByDescending(y => y.date);
                
                total = myQuery.Count();
                res = myQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();





            }
            catch (Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                total = 0;
                return res;
            }



            return res;

        }
        //

        public tx_orders getOrder(int orderID)
        {
            tx_orders res = null;


            try
            {
                Repository mng = new Repository();

                res = mng.getOrder(orderID);

            }
            catch(Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                return res;
            }


            return res;


        }
        //
        public List<tx_orders> getOrdersByStatus(out int total,  string Status = "all", int pageSize = 10, int pageNumber = 1)
        {
            List<tx_orders> res = new List<tx_orders>();
            try
            {

                Repository mng = new Repository();
                IQueryable < tx_orders > myQuery = null;
                if (Status == "all")
                {
                    myQuery = mng.getOrders().OrderByDescending(y => y.date);
                }
                else
                {
                    myQuery = mng.getOrders().Where(x => x.status == Status).OrderByDescending(y => y.date);
                }

                
            

                




                total = myQuery.Count();
                res = myQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();





            }
            catch (Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                total = 0;
                return res;
            }



            return res;

        }
        //

        public List<tx_orders> getOrdersByPhone(out int total, string UserPhone, int pageSize = 10, int pageNumber = 1)
        {
            List<tx_orders> res = new List<tx_orders>();
            try
            {

                Repository mng = new Repository();

                IQueryable<tx_orders> myQuery = mng.getOrders().Where(x => x.phone == UserPhone).OrderByDescending(y => y.date);
                total = myQuery.Count();
                res = myQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();





            }
            catch (Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                total = 0;
                return res;
            }



            return res;

        }


        public int updateOrder(tx_orders element)
        {
            int res = -1;

            try
            {
                Repository mng = new Repository();
                res = mng.updateOrder(element);



            }
            catch(Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                return res;
            }

            return res;


        }


        public int deleteOrder(int orderID)
        {
            int res = -1;

            try
            {
                Repository mng = new Repository();

                res = mng.deleteOrder(orderID);

            }
            catch (Exception ex)
            {
                Error_manager ErrorMng = new Error_manager();
                ErrorMng.LogError(ex);
                return res;
            }

            return res;

        }

        #endregion




    }
}