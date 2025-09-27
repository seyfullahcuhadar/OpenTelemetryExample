using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetryExample.Models;
using OpenTelemetryExample.Data;
using System.Diagnostics;

namespace OpenTelemetryExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationDbContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        _logger.LogInformation("Weather forecast requested at {Time}", DateTime.UtcNow);
        try 
        {
            var existingForecasts = await _context.WeatherForecasts.ToListAsync();
            
            if (!existingForecasts.Any())
            {
                _logger.LogInformation("No existing forecasts found, generating new ones");
                
                var newForecasts = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    }).ToList();

                _context.WeatherForecasts.AddRange(newForecasts);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Generated and saved {Count} new weather forecasts", newForecasts.Count);
                return newForecasts;
            }
            
            _logger.LogInformation("Retrieved {Count} weather forecasts from database", existingForecasts.Count);
            return existingForecasts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving weather forecast from database");
            throw;
        }
    }
}
