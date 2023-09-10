using Accounting.Integration.UseCases.ParrotCreated;
using Accounting.Integration.UseCases.TaskAssigned;
using Accounting.Integration.UseCases.TaskCompleted;
using Accounting.Integration.Utils.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Accounting.Integration.Utils
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
                    .AddSingleton<ParrotCreatedConsumer>()
                    .AddSingleton<TaskAssignedConsumer>()
                    .AddSingleton<TaskCompletedConsumer>()
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(executingAssembly);
                        cfg.Lifetime = ServiceLifetime.Singleton;
                    })
                ;
        }
    }
}