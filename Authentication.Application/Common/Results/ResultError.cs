namespace Authentication.Application.Common.Results;

public sealed record ResultError(
    string Code,
    string Message);