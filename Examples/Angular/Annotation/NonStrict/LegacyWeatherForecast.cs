using System;
using KY.Generator;

namespace NonStrict;

/// <summary>
/// The same model, but opted out of strict mode with [GenerateNonStrict]. Without the strict rules the
/// members are left definitely-unassigned:
/// <code>
/// public date: Date;
/// public temperatureC: number;
/// </code>
/// Use it for TypeScript projects that do not run with "strict": true. The attribute also works on the
/// whole assembly - see AssemblyInfo.cs - and takes a parameter, so a single type of a non strict
/// assembly can switch back with [GenerateNonStrict(false)].
/// </summary>
[GenerateAngularModel]
[GenerateNonStrict]
public class LegacyWeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}
