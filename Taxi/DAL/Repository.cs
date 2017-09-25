using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using Taxi.Models;
using Taxi.BLL.managers;
namespace Taxi.DAL
{

    public class Repository: IDisposable
    {

        #region System
        public DB_taxiEntities db;
        private bool _disposed;

        public Repository()
        {
            db = new DB_taxiEntities();
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

        #region Trace
        public int SaveTrace(tx_trace element)
        {
            try
            {
                if (element.Id == 0)
                {
                    db.tx_trace.Add(element);
                    db.SaveChanges();
                }
                else
                {
                    
                        db.Entry(element).State = EntityState.Modified;
                        db.SaveChanges();
                  
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
            return element.Id;
        }
        #endregion


        #region CRUD tx_orders
        public int addOrder(tx_orders newItem)
        {
            int res = 0;

            try
            {
                db.tx_orders.Add(newItem);
                db.SaveChanges();
                res = 1;
            }
            catch(Exception ex)
            {

                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                res = -1;
                return res;
            }


            return res;
 }



        public tx_orders getOrder(int orderID)
        {

            tx_orders res = null;

            try
            {
                res = db.tx_orders.Where(x => x.ID == orderID).ToList().FirstOrDefault();
            }
            catch(Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            return res;

        }

        public IQueryable<tx_orders> getOrders()
        {
            IQueryable<tx_orders> res = null;

            try
            {
                res = db.tx_orders;
            }
            catch(Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }


            return res;

        }


        public int updateOrder(tx_orders element)
        {

            int res = -1;

            try
            {
                db.Entry(element).State = EntityState.Modified;
                db.SaveChanges();
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

        public int deleteOrder(int orderID)
        {

            int res = -1;

            try
            {
                var element = db.tx_orders.SingleOrDefault(x => x.ID == orderID);
                if (element != null)
                {
                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

            
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



        #endregion

        #region CRUD tx_settings
        
        public int addSettings(tx_settings newItem)
        {
            int res = 0;

            try
            {
                db.tx_settings.Add(newItem);
                db.SaveChanges();
                res = 1;
            }
            catch (Exception ex)
            {

                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                res = -1;
                return res;
            }


            return res;
        }



        public tx_settings getSetting(string name)
        {

            tx_settings res = null;

            try
            {
                res = db.tx_settings.Where(x => x.name == name).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            return res;

        }

        public IQueryable<tx_settings> getSettings()
        {
            IQueryable<tx_settings> res = null;

            try
            {
                res = db.tx_settings;
            }
            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }


            return res;

        }


        public int updateSetting(tx_settings element)
        {

            int res = -1;

            try
            {
                db.Entry(element).State = EntityState.Modified;
                db.SaveChanges();
                res = 1;
            }
            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            return res;
        }

        public int deleteSetting(string name)
        {

            int res = -1;

            try
            {
                var element = db.tx_settings.SingleOrDefault(x => x.name == name);
                if (element != null)
                {
                    db.Entry(element).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }


                res = 1;
            }
            catch (Exception ex)
            {
                Error_manager mng = new Error_manager();
                mng.LogError(ex);
                return res;
            }

            return res;

        }



        #endregion


    }
}