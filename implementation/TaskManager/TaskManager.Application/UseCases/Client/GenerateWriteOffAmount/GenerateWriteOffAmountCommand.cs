using MediatR;

namespace TaskManager.Application.UseCases.Client.GenerateWriteOffAmount
{
    internal class GenerateWriteOffAmountCommand : IRequest<decimal>
    {
    }
}
