namespace Authentication.Application.Features.Authentication.ChangeEmail;

public sealed record ChangeEmailResponse(
    string Email,
    string Message,
    string? ConfirmationLink);
