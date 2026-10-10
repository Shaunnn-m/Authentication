using Authentication.Domain.Enums;

namespace Authentication.Application.Features.Application.QueryApplicationsSettings;

/// <summary>Settings configured for an application.</summary>
public sealed record GetApplicationSettingsResponse(
    Guid ApplicationId,
    EmailHandlingMode EmailHandlingMode,
    bool EnableEmailVerification,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
