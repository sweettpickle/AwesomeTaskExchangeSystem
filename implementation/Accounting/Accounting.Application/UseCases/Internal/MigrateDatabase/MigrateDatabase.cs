using MediatR;

namespace Accounting.Application.UseCases.Internal.MigrateDatabase
{
    /// <summary>
    /// A request to migrate database.
    /// </summary>
    public class MigrateDatabaseCommand : IRequest<Unit>
    {

    }
}
