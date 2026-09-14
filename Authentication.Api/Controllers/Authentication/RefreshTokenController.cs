using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class RefreshTokenController : ControllerBase
{
    private readonly ISender _sender;

    public RefreshTokenController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}