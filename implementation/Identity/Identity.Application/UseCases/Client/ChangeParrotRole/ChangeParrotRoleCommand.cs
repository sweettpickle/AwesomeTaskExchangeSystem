using MediatR;

namespace Identity.Application.UseCases.Client.ChangeParrotRole
{
    public class ChangeParrotRoleCommand : IRequest
    {
        public string PublicId { get; }
        public RoleEnum NewRole { get; }

        public ChangeParrotRoleCommand(string publicId, RoleEnum newRole)
        {
            NewRole = newRole;
            PublicId = publicId;
        }
    }
}
