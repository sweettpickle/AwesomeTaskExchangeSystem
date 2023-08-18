using Identity.Domain.Utils;

namespace Identity.Domain
{
    public class Parrot : Entity
    {
        /// <summary>
        /// как ФИО у людей
        /// </summary>
        public virtual string Nickname { get; protected set; }
        /// <summary>
        /// логин
        /// </summary>
        public virtual string Email { get; protected set; }
        public virtual string Address { get; protected set; }
        public virtual RoleEnum Role { get; protected set; } 
        public virtual PersonalAccountInfo PersonalAccountInfo { get; protected set;}
        public virtual AuthInfo AuthInfo { get; protected set; }

        protected Parrot() { }

        public Parrot(string nickname, 
            string email, 
            string address, 
            RoleEnum role,
            string accountNumber,
            string accountNickname,
            string favoriteTreat)
        {
            PublicId = $"{nickname}_{email}";
            Nickname = nickname;
            Email = email;
            Address = address;
            Role = role;
            PersonalAccountInfo = new PersonalAccountInfo(this, accountNumber, accountNickname);
            AuthInfo = new AuthInfo(this, email, favoriteTreat);
        }

        public virtual void ChangeRole(RoleEnum newRole)
        {
            if (Role == newRole) return;
            Role = newRole;
        }

        public virtual void ChangeFavoriteTreat(string newFavoriteTreat)
        {
            AuthInfo.ChangeFavoriteTreatHash(newFavoriteTreat);
        }
    }
}
