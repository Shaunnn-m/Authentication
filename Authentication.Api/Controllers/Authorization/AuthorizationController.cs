using Authentication.Api.Extentions.Results;
using Authentication.Application.Common.Authorization;
using Authentication.Application.Features.Authorization.AssignRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authorization;

[ApiController]
[Route("api/authorization")]
public sealed class AuthorizationController : ControllerBase
{
    private readonly ISender _sender;

    public AuthorizationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("users/{userId:guid}/roles")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> AssignRole(
        Guid userId,
        [FromBody] AssignRoleCommand command,
        CancellationToken cancellationToken)
    {
        var request = command with
        {
            UserId = userId
        };

        var result = await _sender.Send(
            request,
            cancellationToken);

        return this.ToActionResult(result);
    }
}