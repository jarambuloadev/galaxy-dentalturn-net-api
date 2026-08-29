using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Npgsql.NameTranslation;

namespace Galaxy.DentalTurn.DataAccess.Context;

/// <summary>
/// Used only by the `dotnet ef` CLI (design time). Runtime resolution happens in Program.cs.
/// Connection string resolution order: ConnectionStrings__Default env var,
/// ../Galaxy.DentalTurn.API/appsettings.Development.json, then the local docker-compose default.
/// </summary>
public class DentalTurnDbContextFactory : IDesignTimeDbContextFactory<DentalTurnDbContext>
{
    public DentalTurnDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Galaxy.DentalTurn.API");

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.Exists(apiProjectPath) ? apiProjectPath : Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("Default")
            ?? "Host=localhost;Port=5432;Database=dentalturn;Username=postgres;Password=postgres;EnableLegacyTimestampBehavior=true";

        var optionsBuilder = new DbContextOptionsBuilder<DentalTurnDbContext>();
        optionsBuilder.UseNpgsql(connectionString, o =>
            o.MapEnum<Entities.AppointmentStatus>("AppointmentStatus", nameTranslator: new NpgsqlNullNameTranslator()));

        return new DentalTurnDbContext(optionsBuilder.Options);
    }
}
