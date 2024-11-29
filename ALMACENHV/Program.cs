using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ALMACENHV.Data;
using ALMACENHV.Middleware;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ALMACENHV.Models;
using ALMACENHV.Services;
using ALMACENHV.Filters;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("logs/almacen-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

// Configurar el puerto desde la variable de entorno PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://+:{port}");

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Añadir filtro de validación global
    options.Filters.Add(new ValidateModelStateAttribute());
    options.Filters.Add<ValidationExceptionMiddleware>();
    options.ReturnHttpNotAcceptable = true;  
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
});

// Configurar CORS para Render
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowRender", builder =>
    {
        builder.WithOrigins("https://almacenhv.onrender.com")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configurar la conexión a la base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AlmacenContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(30);
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    
    // Habilitar el seguimiento de consultas LazyLoading
    options.UseLazyLoadingProxies();
    
    // Configurar el comportamiento de cambio de seguimiento
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging()
              .EnableDetailedErrors()
              .LogTo(Console.WriteLine);
    }
});

builder.Services.AddEndpointsApiExplorer();

// Configurar CORS con políticas específicas
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination");
    });
});

// Agregar caché distribuida
builder.Services.AddDistributedMemoryCache();

// Agregar caché en memoria para resultados de consultas
builder.Services.AddMemoryCache();

// Configurar compresión de respuesta
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
});

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JWT");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Token"] ?? 
                    throw new InvalidOperationException("JWT Token not configured"))),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidAudience = jwtSettings["ValidAudience"],
            ClockSkew = TimeSpan.Zero
        };
    });

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "ALMACENHV API", 
        Version = "v1",
        Description = @"API RESTful para el sistema de gestión de almacén ALMACENHV.
        
Características principales:
- Gestión completa de productos e inventario
- Control de ubicaciones y secciones
- Gestión de proveedores
- Sistema de autenticación JWT
- Registro de movimientos y apuntes",
        Contact = new OpenApiContact
        {
            Name = "Omar",
            Email = "omar@ejemplo.com",
            Url = new Uri("https://github.com/yourusername")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Agregar comentarios XML de documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configurar autenticación
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. 
                      Ingrese 'Bearer' [espacio] y luego su token.
                      Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Configurar ejemplos de respuesta
    c.UseInlineDefinitionsForEnums();
    
    // Agrupar endpoints por tag
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        if (api.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    c.DocInclusionPredicate((name, api) => true);

    // Ordenar acciones por nombre de tag
    c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");

    // Configurar esquemas personalizados
    c.SchemaFilter<EnumSchemaFilter>();
});

// Configurar HttpClient con reintentos
builder.Services.AddHttpClient("DefaultClient")
    .AddTransientHttpErrorPolicy(p => 
        p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));

var app = builder.Build();

// Middleware de manejo de errores
app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ALMACENHV API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseErrorHandling(); 
app.UseCors("AllowRender");

app.UseAuthentication();
app.UseAuthorization();

// Configurar manejo de excepciones no controladas
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error no controlado en la aplicación");
        throw;
    }
});

app.MapControllers();

// Aplicar las migraciones automáticamente con reintentos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    retryPolicy.Execute(() =>
    {
        try
        {
            var context = services.GetRequiredService<AlmacenContext>();
            context.Database.Migrate();
            Log.Information("Migraciones aplicadas correctamente");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error al aplicar las migraciones");
            throw;
        }
    });
}

try
{
    Log.Information("Iniciando la aplicación...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar");
}
finally
{
    Log.CloseAndFlush();
}
