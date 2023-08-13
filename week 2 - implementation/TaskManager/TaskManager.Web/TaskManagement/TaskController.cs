using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.UseCases.CompleteTask;
using TaskManager.Application.UseCases.CreateTask;
using TaskManager.Application.UseCases.GetTask;
using TaskManager.Application.UseCases.GetTasks;
using TaskManager.Application.Utils.Common.Exceptions;
using TaskManager.Application.Utils.Common.Models;
using TaskManager.Web.Utils.Web;

[Route("tasks")]
[ApiController]
//[Authorize] //todo authFilter
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController> _logger;
    private readonly IMediator _mediator;

    public TaskController(IMediator mediator, ILogger<TaskController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TaskResult>), 200)]
    public async Task<IActionResult> GetTasks(CancellationToken token)
    {
        var result = await _mediator.Send(new GetTasksQuery(), token);
        return Ok(result);
    }


    [HttpGet, Route("{publicId}")]
    [ProducesResponseType(typeof(TaskResult), 200)]
    public async Task<IActionResult> GetTask(string publicId, CancellationToken token)
    {
        try
        {
            var result = await _mediator.Send(new GetTaskQuery(publicId), token);
            return Ok(result);
        }
        catch (TaskNotFound)
        {
            return NotFound();
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(TaskResult), 200)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request, CancellationToken token)
    {
        var caller = HttpContext.GetCallerIdentity();
        if (caller.Role != RoleEnum.Admin || caller.Role != RoleEnum.Manager)
            return Forbid();

        try
        {
            var result = await _mediator.Send(new CreateTaskCommand(
            request.Name,
            request.Description,
            request.ParrotPid
            ), token);

            return Ok(result);
        }
        catch (ParrotNotFound)
        {
            return NotFound($"Parrot {request.ParrotPid} not found");
        }
    }


    [HttpPut, Route("{publicId}/complete")]
    public async Task<IActionResult> CompleteTask(string publicId, CancellationToken token)
    {
        try
        {
            await _mediator.Send(new CompleteTaskCommand(publicId), token);
        }
        catch (TaskNotFound)
        {
            return NotFound();
        }
        return Ok();
    }


    [HttpPut, Route("reassignTasks")]
    public async Task<IActionResult> ReassignTask(CancellationToken token)
    {
        var caller = HttpContext.GetCallerIdentity();
        if (caller.Role != RoleEnum.Admin || caller.Role != RoleEnum.Manager)
            return Forbid();

        //todo
        return Ok();
    }
}