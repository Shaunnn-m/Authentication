using Authentication.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Extentions.Results
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(
            this ControllerBase controller,
            Result<T> result)
        {
            if (result.IsSuccess)
            {
                return controller.Ok(result.Value);
            }

            return result.Error!.Type switch
            {
                ErrorType.Validation =>
                    controller.BadRequest(result.Error),

                ErrorType.NotFound =>
                    controller.NotFound(result.Error),

                ErrorType.Conflict =>
                    controller.Conflict(result.Error),

                ErrorType.Unauthorized =>
                    controller.Unauthorized(result.Error),

                ErrorType.Forbidden =>
                    controller.StatusCode(
                        StatusCodes.Status403Forbidden,
                        result.Error),

                _ =>
                    controller.StatusCode(
                        StatusCodes.Status500InternalServerError,
                        result.Error)
            };
        }
    }
}
