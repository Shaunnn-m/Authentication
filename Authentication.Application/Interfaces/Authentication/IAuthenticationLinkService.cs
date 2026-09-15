namespace Authentication.Application.Interfaces.Authentication;

public interface IAuthenticationLinkService
{
    string CreateEmailConfirmationLink(
        Guid userId,
        string token);

    string CreatePasswordResetLink(
        Guid userId,
        string token);
}