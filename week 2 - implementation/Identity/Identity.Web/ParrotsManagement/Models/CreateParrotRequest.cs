namespace Identity.Web.ParrotsManagement.Models
{
    public class CreateParrotRequest
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public RoleEnum Role { get; set; }
        public string NumberAccount { get; set; }
        public string AccountNickname { get; set; }
        public string FavoriteTreat { get; set; }
    }
}
