using Authentication.Api.Extentions.Results;
using Authentication.Application.Abstractions.Results.Application;
using Authentication.Application.Features.Application.Register;
using Authentication.Application.Features.Application.RegisterApplicationRole;
using Authentication.Application.Features.Applications.AssignApplicationRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers;

[ApiController]
[Route("api/applications")]
public sealed class ApplicationsController : ControllerBase
{
    private readonly ISender _sender;

    public ApplicationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{applicationId:guid}/roles")]
    [ProducesResponseType(
        typeof(RegisterApplicationRoleResult),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        Guid applicationId,
        RegisterApplicationRoleCommand command,
        CancellationToken cancellationToken)
    {
        if (applicationId != command.ApplicationId)
        {
            return BadRequest(
                "Application ID in the route does not match the request.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{applicationId:guid}/roles/{roleId:guid}/users/{userId:guid}")]
    [ProducesResponseType(
        typeof(AssignApplicationRoleResult),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> AssignRole(
        Guid applicationId,
        Guid roleId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new AssignApplicationRoleCommand(
            applicationId,
            userId,
            roleId);

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}