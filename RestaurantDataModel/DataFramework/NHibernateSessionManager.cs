using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Cache;



namespace Restaurant.DataModel.DataFramework
{
    
    public sealed  class NHibernateSessionManager
    {
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";
        private ISessionFactory sessionFactory;
        private readonly AsyncLocal<Dictionary<string, object>> asyncLocal = new AsyncLocal<Dictionary<string, object>>(); 
       
       public static NHibernateSessionManager Instance
        {
            get { return Nested.nHibernateNHibernateSessionManager; }
        }

        private NHibernateSessionManager(){}

 
       private static class Nested
        {
            internal static readonly NHibernateSessionManager nHibernateNHibernateSessionManager = new NHibernateSessionManager();
        }

        
        public void RegisterInterceptor(IInterceptor interceptor)
        {
            {
                ISession session = ThreadSession;
                if (session != null && session.IsOpen)
                {
                    throw new CacheException("You cannot register an interceptor once a session has already been opened");
                }
                GetSession(interceptor);
            }
        }
       public ISession GetSession()
        {
            {
                return GetSession(null);
            }
        }


        private ISession GetSession(IInterceptor interceptor)
        {
            {
                ISession session = ThreadSession;
                if (session == null)
                {
                    if (interceptor != null)
                    {
                        session = sessionFactory.WithOptions().Interceptor(interceptor).OpenSession();
                    }
                    else
                    {
                        session = sessionFactory.OpenSession();
                    }
                    ThreadSession = session;
                }
                return session;
            }
        }
        public void CloseSession()
        {
            {
                ISession session = ThreadSession;
                ThreadSession = null;
                if (session != null && session.IsOpen)
                {
                    session.Close();
                }
            }
        }
        public void BeginTransaction()
        {
            _ = ThreadTransaction;
        }
        public void CommitTransaction()
        {
            {
                ITransaction transaction = ThreadTransaction;
                try
                {
                    if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                    {
                        transaction.Commit();
                        ThreadTransaction = null;
                    }
                }
                catch (HibernateException ex)
                {
                    LogException(ex);
                    RollbackTransaction();
                    throw;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    RollbackTransaction();
                    throw;
                }
            }
        }

        public void RollbackTransaction()
        {
            {
                ITransaction transaction = ThreadTransaction;
                try
                {
                    ThreadTransaction = null;
                    if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                    {
                        transaction.Rollback();
                    }

                }
                catch (HibernateException ex)
                {
                    LogException(ex);
                    throw;
                }

               finally
                {
                    CloseSession();
                }
            }
        }

 
        private void InitializeAsyncLocal()
        {
            Debug.WriteLine("Current Thread:" + Thread.CurrentThread.ManagedThreadId);
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary[SESSION_KEY] = null;
            dictionary[TRANSACTION_KEY] = null;
            this.asyncLocal.Value = dictionary;
        }
        
        private ITransaction ThreadTransaction
        {
            get
            {
                
                if (asyncLocal.Value == null) { InitializeAsyncLocal();}


                Dictionary<string, object> dictionary =asyncLocal.Value;
                ITransaction transaction = (ITransaction)(dictionary)[TRANSACTION_KEY];
                if (transaction == null)
                {
                    ThreadTransaction=transaction = GetSession().BeginTransaction();
                }
                return transaction;
                
            }

            set
            {
               asyncLocal.Value[TRANSACTION_KEY] = value;
            }
        }

        private ISession ThreadSession
        {
        get
            {
                if (asyncLocal.Value == null) { InitializeAsyncLocal(); }
                Dictionary<string, object> dictionary =(asyncLocal.Value);
                ISession session= (ISession)(dictionary)[SESSION_KEY];
                if (session == null)
                {
                    ThreadSession=session = sessionFactory.OpenSession();
                }
                return session;
            }
        set
            {
                asyncLocal.Value[SESSION_KEY] = value;
            }
        }

        public ISessionFactory SessionFactory { get {  { return sessionFactory; } } set {  { sessionFactory = value; } } }
        private void LogException(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            Debug.WriteLine(JsonConvert.SerializeObject(ex));

        }

    }

}
