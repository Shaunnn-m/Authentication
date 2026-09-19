using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.ChangeEmail;
using Authentication.Application.Features.Authentication.ConfirmEmailChange;
using Authentication.Application.Features.Authentication.GetMyAccount;
using Authentication.Application.Features.Authentication.UpdateMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication;

[ApiController]
[Route("api/authentication/account")]

public sealed class AccountController : ControllerBase
{
    private readonly ISender _sender;

    public AccountController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMyAccount(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetMyAccountQuery(),
            cancellationToken);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateMyProfileCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail(
        ChangeEmailCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("confirm-email-change")]
    public async Task<IActionResult> ConfirmEmailChange(
        ConfirmEmailChangeCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}
