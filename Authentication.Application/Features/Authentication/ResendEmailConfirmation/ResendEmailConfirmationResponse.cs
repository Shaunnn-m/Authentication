namespace Authentication.Application.Features.Authentication.ResendEmailConfirmation;

public sealed record ResendEmailConfirmationResponse(
    string Email,
    string Message);
