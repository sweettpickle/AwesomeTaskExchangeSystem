using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Identity.Shared
{
    public static class AuthOptions
    {
        public static string SECRETKEY = "64c18189-e3e9-470e-8896-78d03f2a0ee5"; //todo move to settings
        public static string ISSUER = "64c18189-e3e9-470e-8896-78d03f2a0ee5"; //todo move to settings
        //private const string AUDIENCE = "64c18189-e3e9-470e-8896-78d03f2a0ee5"; //todo move to settings and add clients apps info
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRETKEY));
    }
}
