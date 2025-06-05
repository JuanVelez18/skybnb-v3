using System.Text;
using application.Core;
using application.DTOs;
using application.Implementations;
using application.Interfaces;
using asp_services.Core;
using asp_services.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<ITokenHasher, TokenHasher>();
builder.Services.AddScoped<IUsersApplication, AuthApplication>();
builder.Services.AddScoped<ICountryApplication, CountryApplication>();
builder.Services.AddScoped<ICitytApplication, CityApplication>();
builder.Services.AddScoped<IPropertiesApplication, PropertiesApplication>();
builder.Services.AddScoped<IBookingsApplication, BookingsApplication>();
builder.Services.AddScoped<IReviewsApplication, ReviewsApplication>();
builder.Services.AddScoped<IUserApplication, UserApplication>();
builder.Services.AddScoped<IPropertyTypesApplication, PropertyTypesApplication>();
builder.Services.AddScoped<IPaymentApplication, PaymentApplication>();

// Initializer
builder.Services.AddScoped<IDataInitializer, DataInitializer>();

// Add instance of DbContext
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

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var problemDetails = new ProblemDetails
            {
                Title = "Unauthorized",
                Detail = context.ErrorDescription
            };

            return context.Response.WriteAsJsonAsync(problemDetails);
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            var problemDetails = new ProblemDetails
            {
                Title = "Forbidden",
                Detail = "User does not have permission to perform this action"
            };

            return context.Response.WriteAsJsonAsync(problemDetails);
        }
    };
});

// Add authorization
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    // Initialize the database for development environment
    using var scope = app.Services.CreateScope();
    var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
    await dataInitializer.InitializeAsync();
}

// Authentication Middleware
app.UseAuthorization();

// Custom Error Handling Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
