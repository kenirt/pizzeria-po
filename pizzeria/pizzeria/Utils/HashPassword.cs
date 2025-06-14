using System.Security.Cryptography;
using System.Text;
namespace pizzeria.Utils
{
    public static class HashPassword
    {
        public static string Hash(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = SHA256.HashData(bytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}