# KY.Generator.Annotations ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

[Documentation](https://generator.ky-programming.de) | [Getting Started](https://generator.ky-programming.de/start) | [Supported Platforms](https://generator.ky-programming.de/start/platforms) | [Need Help?](https://generator.ky-programming.de/start/help)

## Generate via Chained Methods

The Fluent API is a chained method based pattern, to provide an easy and powerful way to specify your generator actions.

The methods are found in the KY.Generator.Fluent package

You have to create a class that inherits from GeneratorFluentMain (e.g. in a own generator project)

```
public class GeneratorMain : GeneratorFluentMain
{
  public override void Execute()
  {
    this.Read()
        .AspDotNet(x => x.FromController<WeatherForecastController>())
        .Write()
        .Angular(x => x
          .Services(config => config.OutputPath("/ClientApp/src/app/services"))
          .Models(config => config.OutputPath("/ClientApp/src/app/models"))
        );
  }
}
```

## Read More
Continue reading with [Fluent API Overview](https://generator.ky-programming.de/start/code/fluent-api)
