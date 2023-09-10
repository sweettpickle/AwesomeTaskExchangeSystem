using Identity.Integration.Utils.Kafka;
using Identity.Integration.Utils.Kafka.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Identity.Integration.Utils
{
    public static class ComponentExtensions
    {
        public static IServiceCollection AddIntegration(this IServiceCollection services, IConfiguration configuration)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            var kafkaConfig = configuration
                .GetSection(nameof(KafkaConfig))
                .Get<KafkaConfig>();

            return services
                    .AddSingleton(kafkaConfig)
                    .AddSingleton<KafkaProducer<string, ParrotCreatedData>>()
                    .AddSingleton<KafkaProducer<string, ParrotRoleChangedData>>()
                    .AddSingleton<KafkaProducer<string, ParrotChangedData>>()
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(executingAssembly);
                        cfg.Lifetime = ServiceLifetime.Singleton;
                    })
                ;
        }


    }
}