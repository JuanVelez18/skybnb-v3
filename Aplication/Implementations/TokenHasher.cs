using System.Security.Cryptography;
using application.Interfaces;

namespace application.Implementations
{
    public class TokenHasher : ITokenHasher
    {
        public string HashToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(token));
            }

            // Simple hash implementation using SHA256
            using (var sha256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(token);
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
