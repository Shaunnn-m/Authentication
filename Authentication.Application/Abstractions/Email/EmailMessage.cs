namespace Authentication.Application.Abstractions.Email;

public sealed record EmailMessage(
    string To,
    string Subject,
    string Body);