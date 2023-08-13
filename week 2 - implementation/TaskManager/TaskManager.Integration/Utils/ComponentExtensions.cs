using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Integration.Utils.Kafka;

namespace TaskManager.Integration.Utils
{
    public static class ComponentExtensions
    {
        public static IServiceCollection AddIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            //var rmqConfig = configuration
            //    .GetSection(nameof(RmqConfig))
            //    .Get<RmqConfig>();

            return services
                    .AddSingleton<KafkaProducer>()
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(executingAssembly);
                        cfg.Lifetime = ServiceLifetime.Singleton;
                    })
                ;
        }


    }
}