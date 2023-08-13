using FluentMigrator.Runner;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Utils.MigrateDatabase;

namespace TaskManager.Persistence.UseCases
{
    /// <summary>
    /// Migrates the database up to latest version.
    /// </summary>
    internal class MigrateDatabaseCommandHandler : IRequestHandler<MigrateDatabaseCommand, Unit>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MigrateDatabaseCommandHandler> _logger;

        public MigrateDatabaseCommandHandler(IServiceProvider serviceProvider, ILogger<MigrateDatabaseCommandHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<Unit> Handle(MigrateDatabaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // why so weird? see https://fluentmigrator.github.io/articles/guides/upgrades/guide-2.0-to-3.0.html?tabs=di
                using var scope = _serviceProvider.CreateScope();

                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error occured while migrating database.");
            }

            return Unit.Value;
        }
    }
}
