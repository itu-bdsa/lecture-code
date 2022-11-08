using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MyApp.Core;
using MyApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ComicsContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Comics")));
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IPowerRepository, PowerRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapGet("/powers", async (IPowerRepository repository) => await repository.ReadAsync());
app.MapGet("/powers/{id}", async (IPowerRepository repository, int id) => await repository.FindAsync(id));
app.MapPost("/powers", async (IPowerRepository repository, int id, PowerCreateDto power) => await repository.CreateAsync(power));
app.MapPut("/powers/{id}", async (IPowerRepository repository, int id, PowerDto power) => await repository.UpdateAsync(power));
app.MapDelete("/powers/{id}", async (IPowerRepository repository, int id) => await repository.DeleteAsync(id));

app.Run();

public partial class Program { }