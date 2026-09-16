using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class ResetPasswordController : ControllerBase
{
    private readonly ISender _sender;

    public ResetPasswordController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}