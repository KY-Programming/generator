using System;
using KY.Generator;

namespace NonStrict;

/// <summary>
/// Generated for TypeScripts strict mode, which is the default. Members that can not be undefined get a
/// default value, so strictPropertyInitialization is satisfied:
/// <code>
/// public date: Date = new Date(0);
/// public temperatureC: number = 0;
/// </code>
/// </summary>
[GenerateAngularModel]
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
