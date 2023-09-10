using Accounting.Application.UseCases.Integration.ParrotCreated;
using MediatR;

namespace Accounting.Application.UseCases.Internal.CreateAccount
{
    internal class CreateAccountCommandHandler : INotificationHandler<ParrotCreatedInExternalSystem>
    {
        public Task Handle(ParrotCreatedInExternalSystem notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
