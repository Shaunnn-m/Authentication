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

    public static readonly ResultError InvalidCredentials =
        new(
            "User.InvalidCredentials",
            "The email or password is invalid.",
            ErrorType.Validation);

    public static readonly ResultError AuthenticationRequired =
        new(
            "User.AuthenticationRequired",
            "Authentication is required.",
            ErrorType.Unauthorized);

    public static readonly ResultError AccountInactive =
        new(
            "User.AccountInactive",
            "The user account is inactive.",
            ErrorType.Forbidden);

    public static readonly ResultError EmailNotConfirmed =
        new(
            "User.EmailNotConfirmed",
            "The email address has not been confirmed.",
            ErrorType.Forbidden);

    public static readonly ResultError RefreshTokenCreationFailed =
        new(
            "User.RefreshTokenCreationFailed",
            "A refresh token could not be created.",
            ErrorType.Failure);

    public static readonly ResultError InvalidRefreshToken =
        new(
            "User.InvalidRefreshToken",
            "The refresh token is invalid or expired.",
            ErrorType.Unauthorized);

    public static readonly ResultError PasswordResetRequested =
        new(
            "User.PasswordResetRequested",
            "If an account exists for this email, a password reset link has been sent.",
            ErrorType.Validation);

    public static readonly ResultError PasswordChangeFailed =
        new(
            "User.PasswordChangeFailed",
            "The current password is incorrect or the new password is invalid.",
            ErrorType.Validation);

    public static readonly ResultError PasswordResetFailed =
        new(
            "User.PasswordResetFailed",
            "The password reset token is invalid or has expired.",
            ErrorType.Validation);

    public static readonly ResultError ProfileUpdateFailed =
        new(
            "User.ProfileUpdateFailed",
            "The user profile could not be updated.",
            ErrorType.Failure);

    public static readonly ResultError DeactivationFailed =
        new(
            "User.DeactivationFailed",
            "Failed to deactivate the user account.",
            ErrorType.Failure);

    public static readonly ResultError EmailChangeFailed =
        new(
            "User.EmailChangeFailed",
            "The email address could not be changed.",
            ErrorType.Validation);

    public static readonly ResultError EmailAlreadyInUse =
        new(
            "User.EmailAlreadyInUse",
            "The specified email address is already in use.",
            ErrorType.Conflict);
}