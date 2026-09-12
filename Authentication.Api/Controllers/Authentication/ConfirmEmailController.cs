using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class ConfirmEmailController : ControllerBase
{
    private readonly ISender _sender;

    public ConfirmEmailController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] Guid userId,
        [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(
            userId,
            token);

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}