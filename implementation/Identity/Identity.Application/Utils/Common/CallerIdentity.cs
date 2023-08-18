namespace Identity.Application.Utils.Common
{
    public class CallerIdentity
    {
        public string PublicId { get; }
        public RoleEnum Role{ get; }

        public CallerIdentity(string publicId, RoleEnum role)
        {
            PublicId = publicId;
            Role = role;
        }
    }
}
