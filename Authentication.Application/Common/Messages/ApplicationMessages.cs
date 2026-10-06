using Authentication.Application.Abstractions.Results;
using Authentication.Application.Common.Results;
using System.Diagnostics;

namespace Authentication.Application.Common.Messages;

public static class ApplicationMessages
{
    public static readonly ResultError NotFound =
        new(
            "Application.NotFound",
            "The application was not found.",
            ErrorType.NotFound);

    public static readonly ResultError Inactive =
        new(
            "Application.Inactive",
            "The application is inactive.",
            ErrorType.Forbidden);
}
