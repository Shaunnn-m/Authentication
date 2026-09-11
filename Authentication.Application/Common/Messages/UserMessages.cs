using Authentication.Application.Common.Results;

namespace Authentication.Application.Common.Messages;

public static class UserMessages
{
    public static readonly ResultError AlreadyExists =
        new(
            "User.AlreadyExists",
            "A user with this email already exists.",
            ErrorType.Conflict);

    public static readonly ResultError CreationFailed =
        new(
            "User.CreationFailed",
            "The user could not be created.",
            ErrorType.Failure);
}