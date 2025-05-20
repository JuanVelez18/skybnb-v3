using application.Core;
using application.DTOs;
using application.Implementations;
using application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;
using repository.Implementations;
using repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConexionString");

// Add services to the container.
builder.Services.AddControllers();

// JWT Configuration
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Dependecie Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher<UserCredentialsDto>, PasswordHasher<UserCredentialsDto>>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IUsersApplication, UsersApplication>();

// Inyecta instancia de conexión a la base de datos
builder.Services.AddDbContext<DbConexion>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
