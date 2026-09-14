namespace Authentication.Application.Common.Authorization;

public sealed class InitialAdminOptions
{
    public const string SectionName =
        "Authentication:InitialAdmin";

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = "System";

    public string LastName { get; set; } = "Administrator";
}