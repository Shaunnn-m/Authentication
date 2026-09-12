using Authentication.Application.Common.Results;

namespace Authentication.Application.Abstractions.Results;

public sealed record ResultError(
    string Code,
    string Message,
    ErrorType Type);