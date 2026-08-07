using Authentication.Api.Application.Common;

namespace Authentication.Api.Application.Errors
{
    public static class UserErrors
{
    public static readonly Error EmailAlreadyExists =
        new(
            "User.EmailAlreadyExists",
            "A user with this email already exists.");

    public static readonly Error UserNotFound =
        new(
            "User.NotFound",
            "The requested user could not be found.");

    public static readonly Error UserInactive =
        new(
            "User.Inactive",
            "The user account has not been activated.");
}
}
