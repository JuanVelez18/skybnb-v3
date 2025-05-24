using System.Text;
using application.Core;
using application.DTOs;
using application.Implementations;
using application.Interfaces;
using asp_services.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using repository.Conexions;
using repository.Configuration;
using repository.Implementations;
using repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtOptions");
string? secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrEmpty(secretKey) || Encoding.UTF8.GetBytes(secretKey).Length < 32)
{
    throw new InvalidOperationException("Please ensure that the 'JwtOptions:SecretKey' setting is present in the configuration and is at least 32 bytes long.");
}

var connectionString = builder.Configuration.GetConnectionString("ConexionString");

// Add services to the container.
builder.Services.AddControllers();

// JWT Configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Dependecies Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher<UserCredentialsDto>, PasswordHasher<UserCredentialsDto>>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<ITokenHasher, TokenHasher>();
builder.Services.AddScoped<IUsersApplication, AuthApplication>();

// Initializer
builder.Services.AddScoped<IDataInitializer, DataInitializer>();

// Inyecta instancia de conexi�n a la base de datos
builder.Services.AddDbContext<DbConexion>(options => options.UseSqlServer(connectionString));

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = builder.Environment.IsProduction();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Disable issuer validation
        ValidateAudience = false, // Disable audience validation
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
    await dataInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Authentication Middleware
app.UseAuthorization();

// Custom Error Handling Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
