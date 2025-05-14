using asp_services.Dtos;
using domain.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using repository.Conexions;
using System.Text;

if (string.IsNullOrEmpty(Configuration.SecretKey) || Encoding.UTF8.GetBytes(Configuration.SecretKey).Length < 32)
{
    // Manejar error crítico: la clave es esencial y debe ser segura.
    throw new InvalidOperationException("La clave secreta JWT no está configurada correctamente o es demasiado corta.");
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Inyecta el hasheador de constraseñas a los controladores
builder.Services.AddScoped<IPasswordHasher<UserDto>, PasswordHasher<UserDto>>();

// Inyecta instancia de conexión a la base de datos
builder.Services.AddSingleton<DbConexion, DbConexion>();


// 1. AGREGAR SERVICIOS DE AUTENTICACIÓN Y CONFIGURAR JWT BEARER
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // Opcional, para algunos escenarios
})
.AddJwtBearer(options =>
{
options.SaveToken = true; // Opcional: guarda el token en HttpContext después de la validación
options.RequireHttpsMetadata = builder.Environment.IsProduction(); // Requerir HTTPS en producción

    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateLifetime = true, // Validar expiración del token

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.SecretKey)),

        // ClockSkew permite una pequeña desviación de tiempo entre el servidor que emite
        // el token y el servidor que lo valida. El valor por defecto es 5 minutos.
        // Para mayor seguridad, podrías reducirlo o ponerlo a TimeSpan.Zero
        // si tus servidores están bien sincronizados.
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
