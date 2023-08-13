using MediatR;

namespace TaskManager.Application.UseCases.CompleteTask
{
    public class CompleteTaskCommand : IRequest
    {
        public string PublicId { get; }

        public CompleteTaskCommand(string publicId)
        {
            PublicId = publicId;
        }
    }
}
