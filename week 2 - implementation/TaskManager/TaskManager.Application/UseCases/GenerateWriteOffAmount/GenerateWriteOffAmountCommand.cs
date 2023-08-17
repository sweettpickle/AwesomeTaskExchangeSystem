using MediatR;

namespace TaskManager.Application.UseCases.GenerateWriteOffAmount
{
    internal class GenerateWriteOffAmountCommand : IRequest<decimal>
    {
    }
}
