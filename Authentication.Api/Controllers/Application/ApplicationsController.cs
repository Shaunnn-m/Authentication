using Authentication.Api.Extentions.Results;
using Authentication.Application.Abstractions.Results;
using Authentication.Application.Features.Application.QueryApplicationsSettings;
using Authentication.Application.Features.Application.Register;
using Authentication.Application.Features.Application.RegisterApplicationUser;
using Authentication.Application.Features.Application.RegisterApplicationRole;
using Authentication.Application.Features.Application.QueryApplicationRoles;
using Authentication.Application.Features.Application.UpdateApplicationRole;
using Authentication.Application.Features.Application.DeleteApplicationRole;
using Authentication.Application.Features.Application.ForgotPassword;
using Authentication.Application.Features.Applications.AssignApplicationRole;
using Authentication.Application.Features.Applications.UpdateApplicationSettings;
using Authentication.Application.Features.Application.LoginApplicationUser;
using Authentication.Application.Features.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.Application.Features.Application;

namespace Authentication.Api.Controllers;

[ApiController]
[Route("api/applications")]
public sealed class ApplicationsController : ControllerBase
{
    private readonly ISender _sender;

    public ApplicationsController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginApplicationUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [AllowAnonymous]
    [HttpPost("users/register")]
    [ProducesResponseType(
        typeof(RegisterApplicationUserResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterApplicationUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>Creates a role for an application.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpPost("{applicationId:guid}/roles")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        typeof(RegisterApplicationRoleResponse),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        Guid applicationId,
        RegisterApplicationRoleCommand command,
        CancellationToken cancellationToken)
    {
        if (applicationId != command.ApplicationId)
        {
            return BadRequest(
                "Application ID in the route does not match the request.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

        /// <summary>Requests a password reset using an application's email settings.</summary>
    [AllowAnonymous]
    [HttpPost("forgot-password")]
    [ProducesResponseType(
        typeof(ApplicationForgotPasswordResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] ApplicationForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>Retrieves the roles defined for an application.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpGet("{applicationId:guid}/roles")]
    [ProducesResponseType(
        typeof(GetApplicationRolesResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoles(
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetApplicationRolesQuery(applicationId),
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>Renames an application role.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpPut("{applicationId:guid}/roles/{roleId:guid}")]
    [ProducesResponseType(
        typeof(ApplicationRoleResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateRole(
        Guid applicationId,
        Guid roleId,
        [FromBody] UpdateApplicationRoleCommand command,
        CancellationToken cancellationToken)
    {
        if (applicationId != command.ApplicationId ||
            roleId != command.RoleId)
        {
            return BadRequest(
                "Application ID and role ID in the route must match the request.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>Deactivates an application role.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpDelete("{applicationId:guid}/roles/{roleId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRole(
        Guid applicationId,
        Guid roleId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeleteApplicationRoleCommand(
                applicationId,
                roleId),
            cancellationToken);

        if (result.IsFailure)
        {
            return this.ToActionResult(result);
        }

        return NoContent();
    }

    [Authorize(Policy = "ApplicationAdmin")]
    [HttpPost("{applicationId:guid}/roles/{roleId:guid}/users/{userId:guid}")]
    [ProducesResponseType(
        typeof(AssignApplicationRoleResponse),
        StatusCodes.Status201Created)]
    public async Task<IActionResult> AssignRole(
        Guid applicationId,
        Guid roleId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new AssignApplicationRoleCommand(
            applicationId,
            userId,
            roleId);

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }

     /// <summary>Retrieves the settings for an application.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpGet("{applicationId:guid}/settings")]
    [ProducesResponseType(
        typeof(GetApplicationSettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ResultError),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSettings(
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetApplicationSettingsQuery(applicationId),
            cancellationToken);

        return this.ToActionResult(result);
    }

    /// <summary>Updates the settings for an application.</summary>
    [Authorize(Policy = "ApplicationAdmin")]
    [HttpPut("{applicationId:guid}/settings")]
    [ProducesResponseType(
        typeof(UpdateApplicationSettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ResultError),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSettings(
        Guid applicationId,
        [FromBody] UpdateApplicationSettingsCommand command,
        CancellationToken cancellationToken)
    {
        if (applicationId != command.ApplicationId)
        {
            return BadRequest(
                "Application ID in the route does not match the request.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return this.ToActionResult(result);
    }
}