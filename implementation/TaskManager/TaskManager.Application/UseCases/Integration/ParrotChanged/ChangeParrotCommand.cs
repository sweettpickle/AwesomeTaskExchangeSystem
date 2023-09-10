using MediatR;

namespace TaskManager.Application.UseCases.Integration.ParrotRoleChanged
{
    public class ChangeParrotCommand : IRequest
    {
        public string PublicId { get; }
        public RoleEnum Role { get; }

        public ChangeParrotCommand(string publicId, string role)
        {
            PublicId = publicId;
            Role = Enum.Parse<RoleEnum>(role);
        }
    }
}
