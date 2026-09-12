using Authentication.Application.Interfaces.Email;

namespace Authentication.Infrastructure.Services.Email
{
    class EmailTemplateService : IEmailTemplateService
    {
        private const string RegistrationConfirmationTemplate =
                "RegistrationConfirmation.html";

        public string RenderRegistrationConfirmation(
            string firstName,
            string confirmationLink)
        {
            var templatePath = Path.Combine(
                AppContext.BaseDirectory,
                "Email",
                "Templates",
                RegistrationConfirmationTemplate);

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException(
                    "The registration confirmation email template could not be found.",
                    templatePath);
            }

            var template = File.ReadAllText(templatePath);

            return template
                .Replace("{{FirstName}}", firstName)
                .Replace("{{ConfirmationLink}}", confirmationLink);
        }
    }
}
