using Identity.Domain.Utils;
using Identity.Shared;

namespace Identity.Domain
{
    public class AuthInfo : Entity
    {
        public virtual Parrot Parent { get; protected set; }
        public virtual string Login { get; protected set; }
        /// <summary>
        /// типа пароль
        /// </summary>
        public virtual byte[] FavoriteTreatHash { get; protected set; }

        protected AuthInfo() { }

        public AuthInfo(Parrot parent, string login, string favoriteTreat)
        {
            Parent = parent;
            PublicId = parent.PublicId;
            Login = login;

            PasswordHash hash = new PasswordHash(favoriteTreat);
            FavoriteTreatHash = hash.ToArray();
        }

        public virtual void ChangeFavoriteTreatHash(string newFavoriteTreat)
        {
            PasswordHash hash = new PasswordHash(newFavoriteTreat);
            var newFavoriteTreatHash = hash.ToArray();

            if (FavoriteTreatHash == newFavoriteTreatHash)
                return;

            FavoriteTreatHash = newFavoriteTreatHash;
        }
    }
}
