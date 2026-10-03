namespace Authentication.Infrastructure.Services.Email;

public sealed class SmtpOptions
{
    public const string SectionName = "Authentication:Smtp";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1025;
    public string? Username { get; set; }
    public string? Password { get; set; }

    public bool UseStartTls { get; set; }

    public string FromEmail { get; set; } = "no-reply@authentication.local";
    public string FromName { get; set; } = "Authentication Service";
}