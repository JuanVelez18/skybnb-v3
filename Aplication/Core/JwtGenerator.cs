using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace application.Core
{
    public class JwtGenerator
    {
        private readonly JwtOptions _options;

        public JwtGenerator(JwtOptions options)
        {
            if (options.SecretKey == null)
                throw new ArgumentNullException(nameof(options.SecretKey), "Secret key cannot be null.");

            if (Encoding.UTF8.GetBytes(options.SecretKey).Length < 32) // 256 bits for HS256
            {
                throw new ArgumentException("The secret key is too short for HS256. It should be at least 32 bytes.");
            }

            if (options.TokenExpirationInMinutes <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.TokenExpirationInMinutes), "Expiration time must be greater than zero.");
            }
        }

        public string GenerateAccessToken(Guid userId, int roleId)
        {
            // 1. Create the symmetric security key from the secret key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            // 2. Create the signing credentials using the key and the HS256 algorithm
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            // 3. Define the claims for the payload
            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, userId.ToString()), // "sub" (Subject)
            new (JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // "iat" (Issued At)
            new ("currentRole", roleId.ToString(), ClaimValueTypes.UInteger32)
        };

            // 4. Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.Add(TimeSpan.FromMinutes(_options.TokenExpirationInMinutes)), // Set the expiration time
                SigningCredentials = signingCredentials
            };

            // 5. Create an instance of the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 6. Create the security token using the token descriptor
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // 7. Write the token as a string (serialize to compact JWS format)
            string jwtTokenString = tokenHandler.WriteToken(securityToken);

            return jwtTokenString;
        }
    }
}
