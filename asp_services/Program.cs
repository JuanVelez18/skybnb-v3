using application.Implementations;
using application.Interfaces;
using Microsoft.EntityFrameworkCore;
using repository.Conexions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ConexionString");

// Add services to the container.
builder.Services.AddControllers();

// Inyecta la aplicación de usuarios
builder.Services.AddScoped<IUsersApplication, UsersApplication>();

// Inyecta instancia de conexión a la base de datos
builder.Services.AddDbContext<DbConexion>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
