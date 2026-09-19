using Authentication.Api.Extentions.Results;
using Authentication.Application.Common.Authorization;
using Authentication.Application.Features.Administration.Users.ActivateUser;
using Authentication.Application.Features.Administration.Users.DeactivateUser;
using Authentication.Application.Features.Administration.Users.GetUser;
using Authentication.Application.Features.Administration.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace Authentication.Api.Controllers.Admin;

[Authorize(Roles = AppRoles.Admin)]
[ApiController]
[Route("api/admin")]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser(
        [FromQuery] GetUserQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{userId:guid}/activate")]
    public async Task<IActionResult> ActivateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new ActivateUserCommand(userId),
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{userId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeactivateUserCommand(userId),
            cancellationToken);

        return this.ToActionResult(result);
    }

}
