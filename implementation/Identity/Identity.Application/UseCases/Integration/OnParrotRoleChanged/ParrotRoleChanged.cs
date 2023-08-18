using MediatR;

namespace Identity.Application.UseCases.Internal.OnParrotRoleChanged
{
    public class ParrotRoleChanged : INotification
    {
        public string PublicId { get; }
        public string RolePid { get; }

        public ParrotRoleChanged(string publicId, string rolePid)
        {
            PublicId = publicId;
            RolePid = rolePid;
        }
    }
}
