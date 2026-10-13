namespace Authentication.Application.Interfaces.Application;

public interface IClientSecretGenerator
{
    string Generate();

    byte[] Hash(string secret);
}