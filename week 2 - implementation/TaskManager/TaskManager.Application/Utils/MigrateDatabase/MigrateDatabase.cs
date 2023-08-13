using MediatR;

namespace TaskManager.Application.Utils.MigrateDatabase
{
    /// <summary>
    /// A request to migrate database.
    /// </summary>
    public class MigrateDatabaseCommand : IRequest<Unit>
    {

    }
}
