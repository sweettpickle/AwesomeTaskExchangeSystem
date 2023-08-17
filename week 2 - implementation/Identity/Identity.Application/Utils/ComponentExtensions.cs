using FluentValidation;
using Identity.Application.Utils.MediatR.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.Utils
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