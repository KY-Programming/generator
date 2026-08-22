using KY.Generator;

namespace HeaderAndLint.Source;

// Nothing is configured for the linter, so the file comes out with the comment of the language:
// /* eslint-disable */ for TypeScript.
[GenerateTypeScriptModel]
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string Summary { get; set; } = string.Empty;
}
