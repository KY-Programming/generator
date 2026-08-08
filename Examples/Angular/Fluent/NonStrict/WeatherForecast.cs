using System;

namespace NonStrict;

/// <summary>
/// A plain model - no generator packages and no annotations in this project. What is generated from it,
/// and whether it is generated for TypeScripts strict mode, is decided in NonStrict.Generator.
/// </summary>
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
