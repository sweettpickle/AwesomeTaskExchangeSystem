using Identity.Integration.Utils.Kafka;
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
                    .AddSingleton<KafkaProducer>()
                    //.AddHostedService<TestConsumer>()
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(executingAssembly);
                        cfg.Lifetime = ServiceLifetime.Singleton;
                    })
                ;
        }


    }
}