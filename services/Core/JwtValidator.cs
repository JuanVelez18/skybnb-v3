using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace services.Core
{
    public class JwtValidator
    {
        private readonly string _secretKey;

        public JwtValidator(string secretKey)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token es nulo o vacío.");
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_secretKey);

            // Configurar los parámetros de validación
            var tokenValidationParameters = new TokenValidationParameters
            {
                // Validar la firma usando la clave secreta
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

                // Validar el tiempo de vida del token (expiración)
                ValidateLifetime = true,

                // Permitir una pequeña desviación de reloj (clock skew) entre servidores.
                // El valor por defecto es 5 minutos. Puedes ajustarlo o ponerlo a TimeSpan.Zero.
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            try
            {
                // Validar el token. Si la validación falla, lanzará una excepción (ej. SecurityTokenExpiredException, SecurityTokenInvalidSignatureException)
                // El resultado es un ClaimsPrincipal que representa al usuario autenticado.
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // El token es válido. 'validatedToken' es del tipo JwtSecurityToken.
                // 'claimsPrincipal' contiene todas las claims del token.
                Console.WriteLine("JWT es válido (con biblioteca).");
                return claimsPrincipal;
            }
            catch (SecurityTokenValidationException stvex)
            {
                Console.WriteLine($"Error de validación del token: {stvex.Message}");
                // Puedes inspeccionar stvex para más detalles del error
                return null;
            }
            catch (ArgumentException argex) // Por ejemplo, si el token está malformado
            {
                Console.WriteLine($"Argumento inválido durante la validación del token: {argex.Message}");
                return null;
            }
            catch (Exception ex) // Otras excepciones inesperadas
            {
                Console.WriteLine($"Ocurrió un error inesperado al validar el token: {ex.Message}");
                return null;
            }
        }
    }
}
