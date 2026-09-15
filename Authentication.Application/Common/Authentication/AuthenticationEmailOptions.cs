public sealed class AuthenticationEmailOptions
{
    public const string SectionName =
        "Authentication:Email";

    public bool Enabled { get; set; } = true;

    public AuthenticationEmailMode Mode { get; set; } =
        AuthenticationEmailMode.AuthenticationService;
}

public enum AuthenticationEmailMode
{
    AuthenticationService,
    Application
}