using Accounting.Application.Utils.MediatR.Pipelines.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Accounting.Application.Utils
{
    public static class ComponentExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                   .AddMediatR(cfg =>
                   {
                       cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                       cfg.Lifetime = ServiceLifetime.Singleton;
                       cfg.AddOpenBehavior(typeof(PipelineValidationBehavior<,>), ServiceLifetime.Singleton);
                   })
                   .AddValidatorsFromAssemblyContaining(typeof(PipelineValidationBehavior<,>), ServiceLifetime.Singleton, includeInternalTypes: true)
                   .AddHostedService<StartupService>()
                ;
        }
    }
}