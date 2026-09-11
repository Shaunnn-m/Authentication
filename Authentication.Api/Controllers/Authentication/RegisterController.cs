using Authentication.Api.Extentions.Results;
using Authentication.Application.Features.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers.Authentication
{

    [ApiController]
    [Route("api/authentication")]
    public class RegisterController : ControllerBase
    {
        private readonly ISender _sender;

        public RegisterController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                command,
                cancellationToken);

            return this.ToActionResult(result);
        }
    }
}
