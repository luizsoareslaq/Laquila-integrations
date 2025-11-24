using System.Security.Cryptography;
namespace Laquila.Integrations.Core.Shared
{
    public class DomainHelpers
    {
        public static string GenerateSecureRefreshToken()
        {
            var bytes = new byte[256];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}