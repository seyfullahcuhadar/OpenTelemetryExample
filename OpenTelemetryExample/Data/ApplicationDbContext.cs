using Microsoft.EntityFrameworkCore;
using OpenTelemetryExample.Models;

namespace OpenTelemetryExample.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // WeatherForecast için seed data
        modelBuilder.Entity<WeatherForecast>().HasData(
            new WeatherForecast
            {
                Id = 1,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                TemperatureC = 25,
                Summary = "Sıcak"
            },
            new WeatherForecast
            {
                Id = 2,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                TemperatureC = 20,
                Summary = "Ilıman"
            },
            new WeatherForecast
            {
                Id = 3,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
                TemperatureC = 15,
                Summary = "Serin"
            }
        );
    }
}
