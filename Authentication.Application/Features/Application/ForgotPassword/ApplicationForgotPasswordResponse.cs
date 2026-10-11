namespace Authentication.Application.Features.Application.ForgotPassword;

public sealed record ApplicationForgotPasswordResponse(
    string Email,
    string? PasswordResetLink,
    string Message);
