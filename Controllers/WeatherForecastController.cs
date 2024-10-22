using System.Net.Mime;
using BlazorApp.Components.Pages;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly List<WeatherForecast> DataWeatherForecasts = [];

    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherForecast>> Get()
    {
        var countData = DataWeatherForecasts.Count;
        
        if (countData > 0)
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
        
        return DataWeatherForecasts;
    }
    
    [HttpGet("{summary}", Name = "GetWeatherForecastBySummary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<WeatherForecast> GetBySummary(string summary)
    {
        var forecast = DataWeatherForecasts.FirstOrDefault(x => x.Summary == summary);
    
        if (forecast == null)
        {
            return NotFound("Summary not found");
        }
    
        return Ok(forecast);
    }
    
    [HttpPost(Name = "PostWeatherForecast")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<WeatherForecast>> Post([FromBody] WeatherForecast forecast)
    {
        if (DataWeatherForecasts.Any(x => x.Summary == forecast.Summary))
        {
            return BadRequest("This summary already exists");
        }
    
        DataWeatherForecasts.Add(forecast);
        return Ok(DataWeatherForecasts);
    }
    
    [HttpDelete("{summary}", Name = "DeleteWeatherForecast")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherForecast>> Delete(string summary)
    {
        var forecast = DataWeatherForecasts.FirstOrDefault(x => x.Summary == summary);
        
        if (forecast == null)
        {
            return NotFound("Summary not found");
        }
        
        DataWeatherForecasts.Remove(forecast);
        return Ok(DataWeatherForecasts);
    }
    
    [HttpPut("{summary}", Name = "PutWeatherForecast")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherForecast>> Put(string summary, [FromBody] WeatherForecast forecast)
    {
        var forecastOld = DataWeatherForecasts.FirstOrDefault(x => x.Summary == summary);
        
        if (forecastOld == null)
        {
            return NotFound("Summary not found");
        }
        
        DataWeatherForecasts.Remove(forecastOld);
        DataWeatherForecasts.Add(forecast);
        return Ok(DataWeatherForecasts);
    }
    
    [HttpPatch("{summary}", Name = "PatchWeatherForecast")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<WeatherForecast>> Patch(string summary, [FromBody] WeatherForecast forecast)
    {
        var forecastOld = DataWeatherForecasts.FirstOrDefault(x => x.Summary == summary);
        
        if (forecastOld == null)
        {
            return NotFound("Summary not found");
        }
        
        forecastOld.Date = forecast.Date;
        forecastOld.TemperatureC = forecast.TemperatureC;
        forecastOld.Summary = forecast.Summary;
        
        return Ok(DataWeatherForecasts);
    }
}