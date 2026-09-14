namespace Authentication.Application.Common.Authentication;

public sealed record AccessTokenResult(
    string Token,
    DateTime ExpiresAt);