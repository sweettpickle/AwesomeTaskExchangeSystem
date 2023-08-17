using TaskManager.Domain.Utils;

namespace TaskManager.Domain
{
    public class Parrot : Entity
    {
        public virtual RoleEnum Role { get; protected set; }

        protected Parrot() { }

        public Parrot(string publicId, RoleEnum role)
        {
            PublicId = publicId;
            Role = role;
        }
    }
}
