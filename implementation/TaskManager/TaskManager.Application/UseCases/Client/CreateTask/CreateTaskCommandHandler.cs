using MediatR;
using NHibernate;
using NHibernate.Linq;
using TaskManager.Application.UseCases.Client.GenerateAccrualAmount;
using TaskManager.Application.UseCases.Client.GenerateWriteOffAmount;
using TaskManager.Application.Utils.Common.Exceptions;
using TaskManager.Application.Utils.Common.Models;

namespace TaskManager.Application.UseCases.Client.CreateTask
{
    internal class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResult>
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IMediator _mediator;

        public CreateTaskCommandHandler(ISessionFactory sessionFactory, IMediator mediator)
        {
            _sessionFactory = sessionFactory;
            _mediator = mediator;
        }

        public async Task<TaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var parrot = await session.Query<Domain.Parrot>()
                .FirstOrDefaultAsync(x => x.PublicId == request.ParrotPid, cancellationToken);

            if (parrot is null) throw new ParrotNotFound();

            var writeOffAmount = await _mediator.Send(new GenerateWriteOffAmountCommand(), cancellationToken);
            var accrualAmount = await _mediator.Send(new GenerateAccrualAmountCommand(), cancellationToken);

            var task = new Domain.Task(request.Name, request.Description, writeOffAmount, accrualAmount, parrot);

            await session.SaveAsync(task);

            return new TaskResult
            {
                PublicId = task.PublicId,
                Name = task.Name,
                Description = task.Description,
                WriteOffAmount = task.WriteOffAmount,
                AccrualAmount = task.AccrualAmount,
                Status = task.Status.ToString(),
                Parrot = new ParrotResult
                {
                    PublicId = task.Parrot.PublicId,
                    Role = task.Parrot.Role.ToString(),
                }
            };
        }
    }
}
