using MediatR;
using Microsoft.AspNetCore.Mvc;
using Authentication.Api.Application.Features.Authentication.Register;
using Authentication.Api.Extensions;

namespace Authentication.Api.Controllers;

[ApiController]
[Route("api/authentication")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    [ProducesResponseType(
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(
        RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return result.ToActionResult(
            this,
            StatusCodes.Status201Created);
    }
}