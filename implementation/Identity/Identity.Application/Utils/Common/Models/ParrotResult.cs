namespace Identity.Application.Utils.Common.Models
{
    public class ParrotResult
    {
        public string PublicId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public PersonalAccountInfo PersonalAccount { get; set; }
    }
}
