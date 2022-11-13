using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using MyApp.Core;
using MyApp.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ComicsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Comics")));
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IPowerRepository, PowerRepository>();

builder.Services.AddValidatorsFromAssembly(Assembly.Load("MyApp.Core"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

var powers = app.MapGroup("/powers").WithOpenApi();

powers.MapGet("/", async (IPowerRepository repository) => await repository.ReadAsync());
powers.MapGet("/{id}", async (int id, IPowerRepository repository) => await repository.FindAsync(id));
powers.MapPost("/", async (Power power, IPowerRepository repository) => await repository.CreateAsync(power));
powers.MapPut("/{id}", async (int id, Power power, IPowerRepository repository) => await repository.UpdateAsync(id, power));
powers.MapDelete("/{id}", async (int id, IPowerRepository repository) => await repository.DeleteAsync(id));

var cities = app.MapGroup("/cities").WithOpenApi();

cities.MapGet("/", async (ICityRepository repository) => await repository.ReadAsync());
cities.MapGet("/{id}", async (int id, ICityRepository repository) => await repository.FindAsync(id));
cities.MapPost("/", async (City city, ICityRepository repository) => await repository.CreateAsync(city));
cities.MapPut("/{id}", async (int id, City city, ICityRepository repository) => await repository.UpdateAsync(id, city));
cities.MapDelete("/{id}", async (int id, ICityRepository repository) => await repository.DeleteAsync(id));

app.Run();
