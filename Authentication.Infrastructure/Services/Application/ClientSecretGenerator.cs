using System.Security.Cryptography;
using Authentication.Application.Interfaces.Application;

namespace Authentication.Infrastructure.Services.Application
{
    public sealed class ClientSecretGenerator : IClientSecretGenerator
    {
        public string Generate()
        {
            var secretBytes = RandomNumberGenerator.GetBytes(32);

            return Convert.ToBase64String(secretBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        public byte[] Hash(string secret)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(secret);

            return SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(secret));
        }

        public bool Verify(string secret, byte[] storedHash)
        {
            if (string.IsNullOrWhiteSpace(secret) ||
                storedHash is null ||
                storedHash.Length != 32)
            {
                return false;
            }

            var suppliedHash = Hash(secret);

            return CryptographicOperations.FixedTimeEquals(
                suppliedHash,
                storedHash);
        }
    }
}