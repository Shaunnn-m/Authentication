namespace Authentication.Application.Interfaces.Authentication;

public interface IConfirmationLinkService
{
    string CreateConfirmationLink(
        Guid userId,
        string token);
}