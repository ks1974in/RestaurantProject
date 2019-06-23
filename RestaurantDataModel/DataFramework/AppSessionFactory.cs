using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Extensions.Configuration;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.DataModel.DataFramework
{

    
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool IsId(Member member)
        {
            
            bool result =  member.Name.Equals("Id");
            return result;
        }
        public override bool ShouldMap(Type type)
        {
            return type.Namespace.Equals("RealEstate.DataModel.Entites") ||
                type.Namespace.Equals("DataModel.RealEstate.DataModel.Export");

        }

    }
    public class AppSessionFactory
    {
        private readonly ISessionFactory sessionFactory;

      
        public ISessionFactory SessionFactory { get => sessionFactory;}

        public AppSessionFactory(IConfiguration config)
        {
            IConfigurationSection section = config.GetSection("DatabaseConfig");
            string server = section["Server"];
            string db = section["Database"];
            string userId = section["UserId"];
            string pwd = section["Password"];
            sessionFactory =CreateSessionFactory(server,db,userId,pwd);   
        }
        private ISessionFactory CreateSessionFactory(string server, string db, string userId, string pwd)
        {
            return Fluently.Configure()
              .Database(
             MsSqlConfiguration.MsSql2012
  .ConnectionString(c => c
    .Server(server)
    .Database(db)
    .Username(userId)
    .Password(pwd)))
    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Dao>().Conventions.Add(DefaultCascade.All(), DefaultLazy.Never()))

    .ProxyFactoryFactory<NHibernate.PropertyChanged.PropertyChangedProxyFactoryFactory>().BuildSessionFactory();
        }
        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
//Changed DefaultCascade.Merge() to DefaultCascade.All()



