using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Npgsql;
using FluentNHibernate.Cfg;
using FluentNHibernate.Mapping;
using FluentNHibernate.Cfg.Db;
using rpbd_lw2.Model;


namespace rpbd_lw2
{
    namespace Logic
    {
        public class NHibHelper
        {
            private static ISessionFactory sessionFactory;
            public static ISessionFactory SessionFactory
            {
                get
                {
                    if (sessionFactory == null)
                        InitializeSessionFactory(); return sessionFactory;
                }
            }

            private static void InitializeSessionFactory()
            {
                FluentConfiguration FG = Fluently.Configure();
                FG = FG.Database(PostgreSQLConfiguration.Standard.ConnectionString(c => c.Host("localhost").Port(5432).Password("1").Database("rpbd").Username("postgres")));
                FG = FG.Mappings(m => m.FluentMappings.Add<Maps.HolderMap>()).Mappings(m => m.FluentMappings.Add<Maps.ParkedMap>()).Mappings(m => m.FluentMappings.Add<Maps.SpecialsMap>());
                sessionFactory = FG.BuildSessionFactory();
            }

            public static ISession OpenSession()
            {
                return SessionFactory.OpenSession();
            }
            
        }
    }
}
