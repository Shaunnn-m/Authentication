namespace Authentication.Application.Common.Results.Applications;

public sealed record RegisterApplicationResult(
    Guid ApplicationId,
    string Name,
    string ClientId);