using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication")]
public sealed class LoginController : ControllerBase
{
    private readonly ISender _sender;

    public LoginController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}