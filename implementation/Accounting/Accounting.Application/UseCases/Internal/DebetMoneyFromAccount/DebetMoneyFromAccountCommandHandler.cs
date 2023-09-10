using Accounting.Application.UseCases.Integration.TaskAssigned;
using MediatR;

namespace Accounting.Application.UseCases.Internal.DebetMoneyFromAccount
{
    public class DebetMoneyFromAccountCommandHandler : INotificationHandler<TaskAssignedInExternalSystem>
    {
        public Task Handle(TaskAssignedInExternalSystem notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
