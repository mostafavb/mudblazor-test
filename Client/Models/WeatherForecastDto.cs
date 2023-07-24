namespace Ui.WebAssembly.Models;

public class WeatherForecastDto
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF { get; set; }
}

