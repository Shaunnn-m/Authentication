using Authentication.Api.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        int successStatusCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess)
        {
            return new ObjectResult(result.Value)
            {
                StatusCode = successStatusCode
            };
        }

        return result.Error!.Code switch
        {
            "User.EmailAlreadyExists" =>
                controller.Conflict(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status409Conflict,
                        Title = "User already exists",
                        Detail = result.Error.Message
                    }),

            "User.NotFound" =>
                controller.NotFound(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "User not found",
                        Detail = result.Error.Message
                    }),

            "User.Inactive" =>
                controller.StatusCode(
                    StatusCodes.Status403Forbidden,
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Title = "User inactive",
                        Detail = result.Error.Message
                    }),

            _ =>
                controller.BadRequest(
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Request failed",
                        Detail = result.Error.Message
                    })
        };
    }
}