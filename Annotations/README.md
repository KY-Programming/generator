# KY.Generator.Annotations ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

[Documentation](https://generator.ky-programming.de) | [Getting Started](https://generator.ky-programming.de/start) | [Supported Platforms](https://generator.ky-programming.de/start/platforms) | [Need Help?](https://generator.ky-programming.de/start/help)

## Generate via Attributes

Annotations are attributes on classes, methods or properties.

The annotations are found in the KY.Generator.Annotations package

In example decorate an ASP.NET Core controller with an GenerateAngularService attribute to generate a full Angular service with all http request to your controller

```
using KY.Generator;
...

namespace ServiceFromAspNetCoreAnnotation.Controllers
{
  [GenerateAngularService("\\ClientApp\\src\\app\\services", "\\ClientApp\\src\\app\\models")]
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    ...
  }
}
```

## This is a library package! 
This package does not contains any directly executable assemblies. You need at least the [KY.Generator](https://www.nuget.org/packages/KY.Generator/) ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat) package

## Read More
Continue reading with [Annotations Overview](https://generator.ky-programming.de/start/code/annotations)
