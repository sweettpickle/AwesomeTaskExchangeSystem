using MediatR;

namespace Identity.Application.UseCases.RemoveParrot
{
    public class RemoveParrotCommand : IRequest
    {
        public string PublicId { get; }

        public RemoveParrotCommand(string publicId)
        {
            PublicId = publicId;
        }
    }
}
