
using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ResendEmailConfirmation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class ResendEmailConfirmationController : ControllerBase
{
    private readonly ISender _sender;

    public ResendEmailConfirmationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation(
        [FromBody] ResendEmailConfirmationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}