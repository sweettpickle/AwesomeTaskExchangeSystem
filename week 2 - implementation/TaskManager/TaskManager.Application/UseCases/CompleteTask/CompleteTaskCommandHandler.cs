using MediatR;
using NHibernate;
using NHibernate.Linq;
using TaskManager.Application.Utils.Common.Exceptions;

namespace TaskManager.Application.UseCases.CompleteTask
{
    internal class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand>
    {
        private readonly ISessionFactory _sessionFactory;

        public CompleteTaskCommandHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public async Task Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            var task = await session.Query<Domain.Task>()
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId);

            if (task is null) throw new TaskNotFound();

            using var tran = session.BeginTransaction();
            task.Complete();
            await session.SaveAsync(task);
            await tran.CommitAsync();
        }
    }
}
