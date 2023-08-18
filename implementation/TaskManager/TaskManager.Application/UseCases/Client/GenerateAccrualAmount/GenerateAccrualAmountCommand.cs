using MediatR;

namespace TaskManager.Application.UseCases.Client.GenerateAccrualAmount
{
    internal class GenerateAccrualAmountCommand : IRequest<decimal>
    {
    }
}
