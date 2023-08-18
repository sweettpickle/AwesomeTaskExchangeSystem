using MediatR;
using TaskManager.Application.Utils.Common.Models;

namespace TaskManager.Application.UseCases.Client.GetTask
{
    public class GetTaskQuery : IRequest<TaskResult>
    {
        public string PublicId { get; }

        public GetTaskQuery(string publicId)
        {
            PublicId = publicId;
        }
    }
}
