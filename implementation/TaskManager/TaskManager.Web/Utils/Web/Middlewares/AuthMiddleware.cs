public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        //todo call IdentityService and set context

        //var claims = context.User.Identity as ClaimsIdentity;

        //var publicId = claims.Claims.FirstOrDefault(x => x.Type == "publicId").Value;
        //var role = claims.Claims.FirstOrDefault(x => x.Type == claims.RoleClaimType).Value;

        //var callerIdentity = new CallerIdentity(publicId, Enum.Parse<RoleEnum>(role));
        //context.SetCallerIdentity(callerIdentity);
        await _next(context);
    }
}