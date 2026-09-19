namespace Authentication.Application.Abstractions.Results.Email;
public sealed record EmailChangeResult(
    Guid UserId,
    string NewEmail,
    string ConfirmationLink);
