using Authentication.Application.Abstractions.Results;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Common.Messages;

public static class ApplicationMessages
{
    public static readonly ResultError NotFound =
        new(
            "Application.NotFound",
            "The application was not found.",
            ErrorType.NotFound);

    public static readonly ResultError CreatorOnly =
        new(
            "Application.CreatorOnly",
            "Only the user who created the application can manage it.",
            ErrorType.Forbidden);

}
