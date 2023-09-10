using Accounting.Application.UseCases.Integration.TaskCompleted;
using MediatR;

namespace Accounting.Application.UseCases.Internal.AddMoneyToAccount
{
    internal class AccrualMoneyToAccountCommandHander : INotificationHandler<TaskCompletedInExternalSystem>
    {
        public Task Handle(TaskCompletedInExternalSystem notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
