
using Authentication.Application.Abstractions.Email;
using Authentication.Application.Interfaces.Email;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure.Services.Email
{
    class LoggingEmailService : IEmailService
    {
        private readonly ILogger<LoggingEmailService> _logger;

        public LoggingEmailService(
            ILogger<LoggingEmailService> logger)
        {
            _logger = logger;
        }


        public Task SendAsync(
        EmailMessage email,
        CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                  """
            Email
            To: {Recipient}
            Subject: {Subject}
            Body:
            {Body}
            """,
            email.To,
            email.Subject,
            email.Body);

            return Task.CompletedTask;
        }
    }
}
