using Authentication.Application.Abstractions.Results;
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

    public static readonly ResultError NotFound =
        new(
            "User.NotFound",
            "The user was not found.",
            ErrorType.NotFound);

    public static readonly ResultError EmailConfirmationFailed =
        new(
            "User.EmailConfirmationFailed",
            "The email confirmation token is invalid or has expired.",
            ErrorType.Validation);

    public static readonly ResultError EmailAlreadyConfirmed =
        new(
            "User.EmailAlreadyConfirmed",
            "The email address has already been confirmed.",
            ErrorType.Validation);
}