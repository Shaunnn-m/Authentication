using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ConfirmEmail;
using Authentication.Application.Features.Authentication.ForgotPassword;
using Authentication.Application.Features.Authentication.Login;
using Authentication.Application.Features.Authentication.Logout;
using Authentication.Application.Features.Authentication.RefreshToken;
using Authentication.Application.Features.Authentication.Register;
using Authentication.Application.Features.Authentication.ResendEmailConfirmation;
using Authentication.Application.Features.Authentication.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

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
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
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

    [Authorize]
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
