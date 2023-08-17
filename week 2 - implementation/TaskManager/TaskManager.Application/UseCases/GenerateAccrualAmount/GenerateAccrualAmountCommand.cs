using MediatR;

namespace TaskManager.Application.UseCases.GenerateAccrualAmount
{
    internal class GenerateAccrualAmountCommand : IRequest<decimal>
    {
    }
}
