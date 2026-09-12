namespace Authentication.Application.Features.Authentication.ConfirmEmail;

public sealed record ConfirmEmailResponse(
    Guid UserId,
    string Email,
    string Message);