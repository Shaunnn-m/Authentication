using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ForgotPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class ForgotPasswordController : ControllerBase
{
    private readonly ISender _sender;

    public ForgotPasswordController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}