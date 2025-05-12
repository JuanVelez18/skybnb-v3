using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace asp_services.Core
{
    public class JwtGenerator
    {
        private readonly string _secretKey;

        public JwtGenerator(string secretKey)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));

            if (Encoding.UTF8.GetBytes(_secretKey).Length < 32) // 256 bits for HS256
            {
                throw new ArgumentException("La clave secreta es demasiado corta para HS256. Debería tener al menos 32 bytes.");
            }
        }

        public string GenerateToken(string userId, TimeSpan expiresIn)
        {
            // 1. Crear la clave de seguridad simétrica a partir de la clave secreta
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            // 2. Crear las credenciales de firma usando la clave y el algoritmo HS256
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 3. Definir las claims para el payload
            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, userId), // "sub" (Subject)
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // "iat" (Issued At)
        };

            // 4. Crear el descriptor del token (o directamente el JwtSecurityToken)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiresIn),
                SigningCredentials = signingCredentials
            };

            // 5. Crear una instancia del manejador de tokens
            var tokenHandler = new JwtSecurityTokenHandler();

            // 6. Crear el token de seguridad
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // 7. Escribir el token como una cadena (serializar a formato JWS compacto)
            string jwtTokenString = tokenHandler.WriteToken(securityToken);

            return jwtTokenString;
        }
    }
}
