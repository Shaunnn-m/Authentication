using Authentication.Application.Interfaces.Email;

namespace Authentication.Infrastructure.Services.Email
{
    class EmailTemplateService : IEmailTemplateService
    {
        private const string RegistrationConfirmationTemplate =
                "RegistrationConfirmation.html";
        private const string PasswordResetTemplate =
                "PasswordReset.html";

        public string RenderRegistrationConfirmation(
            string firstName,
            string confirmationLink)
        {
            var template = LoadTemplate(RegistrationConfirmationTemplate);

            return template
                .Replace("{{FirstName}}", firstName)
                .Replace("{{ConfirmationLink}}", confirmationLink);
        }

        public string RenderPasswordReset(
            string firstName,
            string passwordResetLink)
        {
            var template = LoadTemplate(PasswordResetTemplate);

            return template
                .Replace("{{FirstName}}", firstName)
                .Replace("{{PasswordResetLink}}", passwordResetLink);
        }

        private static string LoadTemplate(string templateFileName)
        {
            var templatePath = Path.Combine(
                AppContext.BaseDirectory,
                "Email",
                "Templates",
                templateFileName);

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException(
                    $"The email template '{templateFileName}' could not be found.",
                    templatePath);
            }

            return File.ReadAllText(templatePath);
        }
    }
}
