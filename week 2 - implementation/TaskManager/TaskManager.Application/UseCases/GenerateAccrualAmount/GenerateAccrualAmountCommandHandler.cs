using MediatR;

namespace TaskManager.Application.UseCases.GenerateAccrualAmount
{
    internal class GenerateAccrualAmountCommandHandler : IRequestHandler<GenerateAccrualAmountCommand, decimal>
    {
        public Task<decimal> Handle(GenerateAccrualAmountCommand request, CancellationToken cancellationToken)
        {
            var rand = new Random();
            return Task.FromResult(new decimal(rand.Next(20, 40)));
        }
    }
}
