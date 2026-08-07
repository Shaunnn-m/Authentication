using Authentication.Api.Application.Common;

namespace Authentication.Api.Application.Abstractions.Authentication;

public sealed record PasswordValidationResult
{
    public bool IsValid { get; init; }

    public Error ErrorMessage { get; init; } = new Error(
        "", "");

    private PasswordValidationResult()
    {
    }

    public static PasswordValidationResult Success()
    {
        return new PasswordValidationResult
        {
            IsValid = true,
        };
    }

    public static PasswordValidationResult Failure(string message)
    {
        return new PasswordValidationResult
        {
            IsValid = false,
            ErrorMessage = new Error(
                "400", message)
        };
    }
}