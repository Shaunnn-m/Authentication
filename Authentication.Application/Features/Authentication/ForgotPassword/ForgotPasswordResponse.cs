namespace Authentication.Application.Features.Authentication.ForgotPassword;

public sealed record ForgotPasswordResponse(
    string Email,
    string? PasswordResetLink,
    string Message);