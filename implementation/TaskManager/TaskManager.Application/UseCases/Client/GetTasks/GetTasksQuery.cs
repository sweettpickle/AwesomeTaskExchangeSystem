using MediatR;
using TaskManager.Application.Utils.Common.Models;

namespace TaskManager.Application.UseCases.Client.GetTasks
{
    public class GetTasksQuery : IRequest<List<TaskResult>>
    {
        public GetTasksQuery() { }
    }
}
