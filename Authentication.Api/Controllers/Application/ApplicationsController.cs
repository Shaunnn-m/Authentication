using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Application.DeactivateApplication;
using Authentication.Application.Features.Application.DeleteApplication;
using Authentication.Application.Features.Application.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }

    [Authorize]
    [HttpPatch("{applicationId:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeactivateApplicationCommand(applicationId),
            cancellationToken);

        if (result.IsFailure)
        {
            return this.ToActionResult(result);
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{applicationId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new DeleteApplicationCommand(applicationId),
            cancellationToken);

        if (result.IsFailure)
        {
            return this.ToActionResult(result);
        }

        return NoContent();
    }
}
