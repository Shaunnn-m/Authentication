using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class LogoutController : ControllerBase
{
    private readonly ISender _sender;

    public LogoutController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}