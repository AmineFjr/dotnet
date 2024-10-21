using BlazorApp.Components.Pages;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static List<WeatherForecast> DataWeatherForecasts = new List<WeatherForecast>();

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        if (DataWeatherForecasts.Count > 0)
        {
            return DataWeatherForecasts;
        }
        
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
        
        DataWeatherForecasts.AddRange(forecasts);
        var countData = DataWeatherForecasts.Count;
        
        _logger.LogInformation($"GetWeatherForecast: {countData} data");
        
        return DataWeatherForecasts;
    }
    
    [HttpPost(Name = "PostWeatherForecast")]
    public ActionResult<IEnumerable<WeatherForecast>> Post([FromBody] WeatherForecast forecast)
    {
        if (DataWeatherForecasts.Any(x => x.Summary == forecast.Summary))
        {
            return BadRequest("This summary already exists");
        }
    
        DataWeatherForecasts.Add(forecast);
        return Ok(DataWeatherForecasts);
    }
}