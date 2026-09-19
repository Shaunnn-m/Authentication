namespace Authentication.Application.Features.Authentication.ConfirmEmailChange;

public sealed record ConfirmEmailChangeResponse(
    string Email,
    string Message);
