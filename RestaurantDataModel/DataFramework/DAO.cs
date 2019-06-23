
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.PropertyChanged;
using Restaurant.DataModel.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Restaurant.DataModel.DataFramework
{
    public class Dao
    {

    
        private readonly NHibernateSessionManager manager = NHibernateSessionManager.Instance;
        
        public void Evict(IEntity entity)
        {
            ISession session = manager.GetSession();
            session.Evict(entity);
            session.Flush();
        }
       


        public Dao(ISessionFactory sf)
        {
            manager.SessionFactory = sf;
        }
        public IEntity Add(IEntity model)
        {
          
            var session = manager.GetSession();
            manager.BeginTransaction();
            session.Save(model);
            manager.CommitTransaction();
            return model;
            
        }
        public void Clear()
        {

            var session = manager.GetSession();
            manager.BeginTransaction();
            session.Clear();
            manager.CommitTransaction();
            session.Flush();

        }
        public T Refresh<T>(IEntity o)
        {
            var session = manager.GetSession();
            session.Refresh(o);
            return (T) o;
        }
        public T Load<T>(Guid id)
        {
            var session = manager.GetSession();
            T o = session.Load<T>(id);
            return o;
        }
     
        public T Get<T>(Guid id)
        {
            var session = manager.GetSession();
            T o = session.Get<T>(id);
            return o;


        }
        public void Flush()
        {
            manager.BeginTransaction();
            manager.GetSession().Flush();
            manager.CommitTransaction();
        }
        public IEntity Delete(IEntity model)
        {
            var session = manager.GetSession();
            manager.BeginTransaction();
            session.Delete(model);
            
            manager.CommitTransaction();
            return model;
        }
        public IEntity SaveOrUpdate(IEntity model)
        {


            var session = manager.GetSession();
            manager.BeginTransaction();
            session.SaveOrUpdate(model);
            manager.CommitTransaction();
            return model;

        }
       
        public T Merge<T>(IEntity model)
        {
            var session = manager.GetSession();
            manager.BeginTransaction();
            T merged=(T)session.Merge(model);
            manager.CommitTransaction();
            return  merged;
            
        }
        public T Save<T>(IEntity entity)
        {
            try
            {
                T entityDb = Get<T>(entity.Id);
                if (entityDb != null)
                {


                    return (T) Merge<T>(entity);
                }
                else
                {
                    return (T)Add(entity);
                }
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(JsonConvert.SerializeObject(e));
                throw;
            }
        }
        public void Execute(string sql)
        {
            var session = manager.GetSession();
            manager.BeginTransaction();
            session.CreateQuery(sql).ExecuteUpdate();
            manager.CommitTransaction();

        }
        public void DeleteAll(Type type)
        {

            var session = manager.GetSession();
            manager.BeginTransaction();
            session.Clear();  // to get rId associations of the objects in memory
            string sql = "delete from " + type.Name;
            session.CreateQuery(sql).ExecuteUpdate();
            manager.CommitTransaction();
            
        }

        public void DeleteAll(Type type,string whereClause)
        {

            var session = manager.GetSession();
            manager.BeginTransaction();
          
            string sql = "delete from " + type.Name + " where " + whereClause;
            session.CreateQuery(sql).ExecuteUpdate();
            manager.CommitTransaction();

        }

        public T Query<T>(string sql)
        {

            T o = default(T);
            try
            {
                var session = manager.GetSession();
                manager.BeginTransaction();
                o = session.CreateSQLQuery(sql).AddEntity(typeof(T)).UniqueResult<T>();
                manager.CommitTransaction();
                
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(JsonConvert.SerializeObject(e));
                manager.RollbackTransaction();
                throw;
            }
            return o;


        }
        public IList<T> List<T>()
        {
                var session = manager.GetSession();
                manager.BeginTransaction();
                IList<T> list = session.CreateCriteria(typeof(T)).List<T>();
                manager.CommitTransaction();
                if (list == null)
                {
                    list = new List<T>();
                }
                return list;
           
        }
        public IList<T> List<T>(string sql)
        {
            var session = manager.GetSession();
            manager.BeginTransaction();
            IList<T> list = session.CreateSQLQuery(sql).AddEntity(typeof(T)).List<T>();
            manager.CommitTransaction();  
            return list;

           
        }
    }
}
