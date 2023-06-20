using MudBlazorTemplates1.Shared.Models;

namespace ApiClient.Endpoints;

public static class Weather
{
    public static IEndpointRouteBuilder RegisterWeatherEndpoints(this RouteGroupBuilder group)
    {
        var summaries = new[]{
                                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                                };

        group.MapGet("/", () =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Id = index,
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            return forecast;
        })
        .Produces<List<WeatherForecast>>()
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        return group;
    }
}
