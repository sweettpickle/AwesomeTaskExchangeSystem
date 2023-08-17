using MediatR;

namespace TaskManager.Application.UseCases.CreateParrot
{
    public class CreateParrotCommand : IRequest
    {
        public string PublicId { get; }
        public RoleEnum Role { get; }

        public CreateParrotCommand(string publicId, string role)
        {
            PublicId = publicId;
            Role = Enum.Parse<RoleEnum>(role);
        }
    }
}
