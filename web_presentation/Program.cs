using presentations;
using presentations.Implementations;
using presentations.Interfaces;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddViteServices(options =>
{
    options.Server.AutoRun = true;
});

// Add presentation configuration
builder.Services.Configure<PresentationConfiguration>(builder.Configuration.GetSection("PresentationConfiguration"));

// Add application services
builder.Services.AddScoped<Comunication>();
builder.Services.AddScoped<IAuthPresentation, AuthPresentation>();
builder.Services.AddScoped<ICountryPresentation, CountryPresentation>();
builder.Services.AddScoped<ICitytPresentation, CityPresentation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    app.UseWebSockets();
    // Use Vite Dev Server as middleware.
    app.UseViteDevelopmentServer(true);
}

app.Run();
