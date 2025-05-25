using presentations;
using presentations.Implementations;
using presentations.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add presentation configuration
builder.Services.Configure<PresentationConfiguration>(builder.Configuration.GetSection("PresentationConfiguration"));

// Add application services
builder.Services.AddSingleton<Comunication>();
builder.Services.AddSingleton<IAuthPresentation, AuthPresentation>();
builder.Services.AddSingleton<ICountryPresentation, CountryPresentation>();

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

app.Run();
