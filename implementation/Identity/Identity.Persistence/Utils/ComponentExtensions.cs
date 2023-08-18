using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System.Data;
using System.Reflection;


namespace Identity.Persistence.Utils
{
    public static class ComponentExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration
                         .GetSection(nameof(PersistenceConfig))
                         .Get<PersistenceConfig>();


            return services
                   .AddSingleton(config)
                   .AddFluentMigrator(config)
                   .AddMediatR(cfg =>
                   {
                       cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                       cfg.Lifetime = ServiceLifetime.Singleton;
                   })
                   .AddSingleton(ConfigureSessionFactory)
                   .AddSingleton(ConfigureReadOnlyConnection)
                ;
        }

        private static Func<IDbConnection> ConfigureReadOnlyConnection(IServiceProvider services)
        {
            var config = services.GetService<PersistenceConfig>();
            return () => new SqlConnection(config.ConnectionString);
        }

        private static IServiceCollection AddFluentMigrator(this IServiceCollection services, PersistenceConfig config)
        {
            return services.AddFluentMigratorCore()
                           .ConfigureRunner(
                               rb => rb
                                     .AddSqlServer2016()
                                     .WithGlobalConnectionString(config.ConnectionString)
                                     .ScanIn(Assembly.GetExecutingAssembly())
                                     .For.Migrations());
        }

        private static ISessionFactory ConfigureSessionFactory(IServiceProvider services)
        {
            var config = services.GetService<PersistenceConfig>();

            var factory = Fluently.Configure()
                                  .Database(
                                      MsSqlConfiguration.MsSql2012.ConnectionString(config.ConnectionString)
                                                        .ShowSql()
                                  )
                                  .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                                  .BuildSessionFactory();
            return factory;
        }
    }
}
