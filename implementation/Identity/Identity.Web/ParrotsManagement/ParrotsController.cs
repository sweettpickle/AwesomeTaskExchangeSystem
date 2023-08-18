using Identity.Application.UseCases.Client.ChangeParrotRole;
using Identity.Application.UseCases.Client.CreateParrot;
using Identity.Application.UseCases.Client.GetParrot;
using Identity.Application.UseCases.Client.GetParrots;
using Identity.Application.UseCases.Client.RemoveParrot;
using Identity.Application.Utils.Common.Models;
using Identity.Web.ParrotsManagement.Models;
using Identity.Web.Utils.Web;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("parrots")]
[ApiController]
[Authorize]
public class ParrotsController : ControllerBase
{
    private readonly ILogger<ParrotsController> _logger;
    private readonly IMediator _mediator;


    public ParrotsController(IMediator mediator, ILogger<ParrotsController> logger) 
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ParrotResult>), 200)]
    public async Task<IActionResult> GetParrots(CancellationToken token)
    {
        var result = await _mediator.Send(new GetParrotsQuery(), token);
        return Ok(result);
    }


    [HttpGet, Route("me")]
    [ProducesResponseType(typeof(ParrotResult), 200)]
    public async Task<IActionResult> GetParrot(CancellationToken token)
    {
        var callerIdentity = HttpContext.GetCallerIdentity();
        var result = await _mediator.Send(new GetParrotQuery(callerIdentity.PublicId), token);
        return Ok(result);
    }


    [HttpGet, Route("{publicId}")]
    [ProducesResponseType(typeof(ParrotResult), 200)]
    public async Task<IActionResult> GetParrot(string publicId, CancellationToken token)
    {
        var result = await _mediator.Send(new GetParrotQuery(publicId), token);
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(ParrotResult), 200)]
    public async Task<IActionResult> CreateParrot([FromBody] CreateParrotRequest request, CancellationToken token)
    {
        var caller = HttpContext.GetCallerIdentity();
        if (caller.Role != RoleEnum.Admin && caller.Role != RoleEnum.Manager)
            return Forbid();

        var result = await _mediator.Send(new CreateParrotCommand(
            request.Nickname,
            request.Email,
            request.Address,
            request.Role,
            request.NumberAccount,
            request.AccountNickname,
            request.FavoriteTreat
            ), token);

        if (result is null)
            return BadRequest("Parrot already exists");

        return Ok(result);
    }


    [HttpPut, Route("{publicId}/changeRole")]
    public async Task<IActionResult> ChangeParrotRole(string publicId, [FromBody]ChangeParrotRoleRequest request, CancellationToken token)
    {
        var caller = HttpContext.GetCallerIdentity(); //todo вынести в атрибут
        if (caller.Role != RoleEnum.Admin && caller.Role != RoleEnum.Manager)
            return Forbid();

        await _mediator.Send(new ChangeParrotRoleCommand(publicId, request.NewRole), token);
        return Ok();
    }


    [HttpDelete, Route("{publicId}")]
    public async Task<IActionResult> DeleteParrot(string publicId, CancellationToken token)
    {
        var caller = HttpContext.GetCallerIdentity();
        if (caller.Role != RoleEnum.Admin && caller.Role != RoleEnum.Manager)
            return Forbid();

        await _mediator.Send(new RemoveParrotCommand(publicId), token);
        return Ok();
    }


    [HttpGet, Route("roles")]
    public async Task<ActionResult> Roles(CancellationToken token)
    {
        return null;
    }
}