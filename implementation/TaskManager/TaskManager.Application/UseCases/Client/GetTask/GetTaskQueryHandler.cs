using MediatR;
using NHibernate;
using NHibernate.Linq;
using TaskManager.Application.Utils.Common.Models;

namespace TaskManager.Application.UseCases.Client.GetTask
{
    internal class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskResult>
    {
        private readonly ISessionFactory _sessionFactory;

        public GetTaskQueryHandler(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Task<TaskResult> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            using var session = _sessionFactory.OpenSession();

            return session.Query<Domain.Task>()
                .Select(task => new TaskResult
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
                })
                .FirstOrDefaultAsync(x => x.PublicId == request.PublicId, cancellationToken);
        }
    }
}
