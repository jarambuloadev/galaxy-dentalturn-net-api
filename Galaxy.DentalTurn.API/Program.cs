using Galaxy.DentalTurn.API.Endpoints;
using Galaxy.DentalTurn.Business;
using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.NameTranslation;

// Establecer el switch antes de que Npgsql se inicialice para evitar que
// el NpgsqlConnectionStringBuilder intente parsear la clave EnableLegacyTimestampBehavior
System.AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// appsettings.Local.json is gitignored — use it to point this API at Supabase
// (or any other Postgres) without touching the docker-compose defaults in appsettings.Development.json.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:Default. Set it in appsettings.Development.json (local docker) or via env var for Supabase.");

var nullNameTranslator = new NpgsqlNullNameTranslator();
builder.Services.AddDbContext<DentalTurnDbContext>(options =>
    options.UseNpgsql(connectionString, o =>
        o.MapEnum<AppointmentStatus>("AppointmentStatus", nameTranslator: nullNameTranslator)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Obtener la clave secreta y validar que exista antes de convertirla a bytes
        var secret = builder.Configuration["Jwt:SecretKey"]
                     ?? throw new InvalidOperationException("Falta configuración: Jwt:SecretKey (usar appsettings, User Secrets o variable de entorno Jwt__SecretKey).");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret))
        };
    });

builder.Services.AddAuthorization();

builder.Services
    .AddBusiness()
    .AddRepositories();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("/api/auth").MapAuthEndpoints().WithTags("Auth");
app.MapGroup("/api/patients").MapPatientEndpoints().WithTags("patients");
app.MapGroup("/api/adminusers").MapAdminUserEndpoints().WithTags("adminusers");
app.MapGroup("/api/business-hours").MapBusinessHoursEndpoints().WithTags("business-hours");
app.MapGroup("/api/practitioners").MapPractitionerEndpoints().WithTags("practitioners");
app.MapGroup("/api/appointments").MapAppointmentEndpoints().WithTags("appointments");

var servicesGroup = app.MapGroup("/api/services")
    .MapServiceEndpoints()
    .GetAwaiter().GetResult();
servicesGroup.WithTags("services");


app.MapGet("/api/hola", () =>
{
    return Results.Ok("Hello World! 100");
}); 

app.Run();
