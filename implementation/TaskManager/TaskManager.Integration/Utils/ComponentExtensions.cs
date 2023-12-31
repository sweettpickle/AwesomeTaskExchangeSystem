﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManager.Integration.UseCases.OnParrotChanged;
using TaskManager.Integration.UseCases.OnParrotCreated;
using TaskManager.Integration.Utils.Kafka;

namespace TaskManager.Integration.Utils
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
                    .AddHostedService<ParrotCreatedConsumer>()
                    .AddHostedService<ParrotChangedConsumer>()
                    .AddMediatR(cfg =>
                    {
                        cfg.RegisterServicesFromAssembly(executingAssembly);
                        cfg.Lifetime = ServiceLifetime.Singleton;
                    })
                ;
        }


    }
}