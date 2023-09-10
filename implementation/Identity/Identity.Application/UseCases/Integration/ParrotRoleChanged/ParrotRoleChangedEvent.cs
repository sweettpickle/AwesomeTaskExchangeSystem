using MediatR;

namespace Identity.Application.UseCases.Internal.OnParrotRoleChanged
{
    public class ParrotRoleChangedEvent : INotification
    {
        public string PublicId { get; }
        public string RolePid { get; }

        public ParrotRoleChangedEvent(string publicId, string rolePid)
        {
            PublicId = publicId;
            RolePid = rolePid;
        }
    }
}
