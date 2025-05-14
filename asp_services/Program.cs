using asp_services.Dtos;
using domain.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using repository.Conexions;
using System.Text;

if (string.IsNullOrEmpty(Configuration.SecretKey) || Encoding.UTF8.GetBytes(Configuration.SecretKey).Length < 32)
{
    // Manejar error cr�tico: la clave es esencial y debe ser segura.
    throw new InvalidOperationException("La clave secreta JWT no est� configurada correctamente o es demasiado corta.");
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Inyecta el hasheador de constrase�as a los controladores
builder.Services.AddScoped<IPasswordHasher<UserDto>, PasswordHasher<UserDto>>();

// Inyecta instancia de conexi�n a la base de datos
builder.Services.AddSingleton<DbConexion, DbConexion>();


// 1. AGREGAR SERVICIOS DE AUTENTICACI�N Y CONFIGURAR JWT BEARER
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Opcional, para algunos escenarios
})
.AddJwtBearer(options =>
{
options.SaveToken = true; // Opcional: guarda el token en HttpContext despu�s de la validaci�n
options.RequireHttpsMetadata = builder.Environment.IsProduction(); // Requerir HTTPS en producci�n

    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateLifetime = true, // Validar expiraci�n del token

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SecretKey)),

        // ClockSkew permite una peque�a desviaci�n de tiempo entre el servidor que emite
        // el token y el servidor que lo valida. El valor por defecto es 5 minutos.
        // Para mayor seguridad, podr�as reducirlo o ponerlo a TimeSpan.Zero
        // si tus servidores est�n bien sincronizados.
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
