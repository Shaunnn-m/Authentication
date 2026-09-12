namespace Authentication.Application.Common.Authentication;

public sealed class EmailConfirmationOptions
{
    public const string SectionName = "Authentication:EmailConfirmation";

    public bool Enabled { get; set; } = true;

    public EmailConfirmationMode Mode { get; set; } =
        EmailConfirmationMode.AuthenticationService;
}

public enum EmailConfirmationMode
{
    AuthenticationService,
    Application
}