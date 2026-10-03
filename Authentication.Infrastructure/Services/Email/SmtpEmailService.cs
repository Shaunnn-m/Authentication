using Authentication.Application.Abstractions.Email;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Authentication.Infrastructure.Services.Email
{
    public sealed class SmtpEmailService
    {
        private readonly SmtpOptions _smtpOptions;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(
            IOptions<SmtpOptions> options,
            ILogger<SmtpEmailService> logger)
        {
            _smtpOptions = options.Value;
            _logger = logger;
        }

        public async Task SendAsync(
            EmailMessage email,
            CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _smtpOptions.FromName,
                    _smtpOptions.FromEmail));

            message.To.Add(
                MailboxAddress.Parse(email.To));

            message.Subject = email.Subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = email.Body
            }.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                var secureSocketOptions = _smtpOptions.UseStartTls
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.None;

                await client.ConnectAsync(
                    _smtpOptions.Host,
                    _smtpOptions.Port,
                    secureSocketOptions,
                    cancellationToken);

                if (!string.IsNullOrWhiteSpace(_smtpOptions.Username))
                {
                    await client.AuthenticateAsync(
                        _smtpOptions.Username,
                        _smtpOptions.Password,
                        cancellationToken);
                }

                await client.SendAsync(message, cancellationToken);

                _logger.LogInformation(
                    "Email delivered to SMTP server. Subject: {Subject}",
                    email.Subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Email delivery failed. Subject: {Subject}",
                    email.Subject);

                throw;
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(
                        true,
                        cancellationToken);
                }
            }
        }
    }
}