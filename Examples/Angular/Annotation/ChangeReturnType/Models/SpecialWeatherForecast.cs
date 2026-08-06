namespace ChangeReturnType.Models;

/// <summary>
/// Returned by the API, but never generated.
/// [GenerateImport] on the controller points the generated service at the "@my-lib/models" package instead.
/// </summary>
public class SpecialWeatherForecast : WeatherForecast
{
    public string? Warning { get; set; }
}
