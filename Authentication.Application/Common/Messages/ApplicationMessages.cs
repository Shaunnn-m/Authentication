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

    public static readonly ResultError RoleNotFound =
        new(
            "Application.RoleNotFound",
            "The role was not found for this application.",
            ErrorType.NotFound);

    public static readonly ResultError RoleInactive =
        new(
            "Application.RoleInactive",
            "The role is inactive.",
            ErrorType.Forbidden);

    public static readonly ResultError UserAccessRequired =
        new(
            "Application.UserAccessRequired",
            "The user does not have access to the application.",
            ErrorType.Forbidden);

    public static readonly ResultError RoleAlreadyAssigned =
        new(
            "Application.RoleAlreadyAssigned",
            "The role is already assigned to the user.",
            ErrorType.Conflict);
}
