namespace Authentication.Application.Interfaces.Email
{
    public interface IEmailTemplateService
    {
        string RenderRegistrationConfirmation(
            string firstName,
            string confirmationLink);

        string RenderPasswordReset(
            string firstName,
            string resetLink);
    }
}
