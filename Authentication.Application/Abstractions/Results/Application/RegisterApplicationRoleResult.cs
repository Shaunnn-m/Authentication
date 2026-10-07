namespace Authentication.Application.Abstractions.Results.Application
{
    public sealed record RegisterApplicationRoleResult(
    Guid RoleId,
    Guid ApplicationId,
    string Name);
}
