using MediatR;

namespace TaskManager.Application.UseCases.Client.GenerateWriteOffAmount
{
    internal class GenerateWriteOffAmountCommandHandler : IRequestHandler<GenerateWriteOffAmountCommand, decimal>
    {
        public Task<decimal> Handle(GenerateWriteOffAmountCommand request, CancellationToken cancellationToken)
        {
            var rand = new Random();
            return Task.FromResult(new decimal(rand.Next(-20, -10)));
        }
    }
}
