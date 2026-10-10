using Authentication.Application.Abstractions.Results;
using Authentication.Application.Common.Results;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.Application.Common.Messages;

public static class ApplicationMessages
{
    public static readonly ResultError NotFound =
        new(
            "Application.NotFound",
            "The application was not found.",
            ErrorType.NotFound);

    public static readonly ResultError SettingsNotFound =
        new(
            "Application.SettingsNotFound",
            "Application settings were not found.",
            ErrorType.NotFound);

    public static readonly ResultError Inactive =
        new(
            "Application.Inactive",
            "The application is inactive.",
            ErrorType.Forbidden);

    public static readonly ResultError RegistrationDisabled =
        new(
        "Application.RegistrationDisabled",
        "Registration is disabled for this application.",
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

    public static readonly ResultError RoleNameAlreadyExists =
        new(
            "Application.RoleNameAlreadyExists",
            "A role with this name already exists for the application.",
            ErrorType.Conflict);
}
