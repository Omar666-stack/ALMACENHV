using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar el puerto desde la variable de entorno PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://+:{port}");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "ALMACENHV API", 
        Version = "v1",
        Description = "API para el sistema de gestión de almacén"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configurar middleware
app.UseRouting();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALMACENHV API V1");
    c.RoutePrefix = string.Empty; // Hacer Swagger UI la página principal
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
