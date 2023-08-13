using Identity.Application.Utils.Common;
using Identity.Web.Utils.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.EndpointIsDecoratedWith<AuthorizeAttribute>() == false)
        {
            await _next(context);
            return;
        }

        var claims = context.User.Identity as ClaimsIdentity;

        var publicId = claims.Claims.FirstOrDefault(x => x.Type == "publicId").Value;
        var role = claims.Claims.FirstOrDefault(x => x.Type == claims.RoleClaimType).Value;

        var callerIdentity = new CallerIdentity(publicId, Enum.Parse<RoleEnum>(role));
        context.SetCallerIdentity(callerIdentity);
        await _next(context);
    }
}