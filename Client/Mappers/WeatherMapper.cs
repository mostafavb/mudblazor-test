using Infrastructure.Shared.Models;
using Ui.WebAssembly.Models;

namespace Ui.WebAssembly.Mappers;

public static class WeatherMapper
{
    public static WeatherForecastDto ToDto(this WeatherForecast model) =>
        new WeatherForecastDto
        {
            Summary = model.Summary,
            Id = model.Id,
            Date = new DateTime(model.Date.Value.Year, model.Date.Value.Month, model.Date.Value.Day),
            TemperatureC = model.TemperatureC,
            TemperatureF = model.TemperatureF
        };

    public static List<WeatherForecastDto> ToListDto(this IList<WeatherForecast> dtos) =>
        dtos.Select(weatherForecast => new WeatherForecastDto
        {
            Summary = weatherForecast.Summary,
            Id = weatherForecast.Id,
            Date = new DateTime(weatherForecast.Date.Value.Year, weatherForecast.Date.Value.Month, weatherForecast.Date.Value.Day),
            TemperatureC = weatherForecast.TemperatureC,
            TemperatureF = weatherForecast.TemperatureF
        }).ToList();

    public static WeatherForecast ToModel(this WeatherForecastDto dto) =>
        new WeatherForecast
        {
            Id = dto.Id,
            Date = DateOnly.FromDateTime(dto.Date ?? DateTime.Now),
            Summary = dto.Summary,
            TemperatureC = dto.TemperatureC,
        };
}