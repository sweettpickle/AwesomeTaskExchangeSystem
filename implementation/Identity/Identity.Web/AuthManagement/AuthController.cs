using Identity.Application.UseCases.Client.ChangeFavoriteTeat;
using Identity.Application.UseCases.Client.Login;
using Identity.Shared;
using Identity.Web.Utils.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<ParrotsController> _logger;
    private readonly IMediator _mediator;

    
    public AuthController(ILogger<ParrotsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        var parrot = await _mediator.Send(new LoginCommand(request.Login, request.FavoriteTeat), token);
        if (parrot is null)
            return Unauthorized();

        var signInCred = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AuthOptions.ISSUER,
            Subject = new ClaimsIdentity(new[]
            {
                new  Claim("publicId", parrot.PublicId),
                new  Claim("role", parrot.Role.ToString()),
            }),
            Expires = DateTime.Now.AddMinutes(60),
            SigningCredentials = signInCred
        };

        var jwt = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return Ok(
            new { 
                token = jwt,
                expires = tokenDescriptor.Expires
            });
    }

    //создают акки админы, а воркеры заход€т потом и мен€ют свой пароль
    //public async Task<IActionResult> Register()
    //{
    //    return Ok();
    //}

    [HttpPut, Route("changeFavoriteTeat"), Authorize]
    public async Task<IActionResult> ChangeFavoriteTeat([FromBody] ChangeFavoriteTreatRequest request,
        CancellationToken token)
    {
        var callerIdentity = HttpContext.GetCallerIdentity();
        await _mediator.Send(new ChangeFavoriteTeatCommand(callerIdentity.PublicId, request.NewFavoriteTeat), token);
        return Ok();
    }
}