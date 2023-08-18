using Identity.Application.Utils.MigrateDatabase;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace Identity.Application.Utils
{
    /// <summary>
    /// Executes startup activities.
    /// </summary>
    public class StartupService : IHostedService
    {
        private readonly IMediator _mediator;

        public StartupService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mediator.Send(new MigrateDatabaseCommand(), cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}