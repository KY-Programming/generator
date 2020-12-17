# KY.Generator ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

[Documentation](https://generator.ky-programming.de) | [Getting Started](https://generator.ky-programming.de/start) | [Supported Platforms](https://generator.ky-programming.de/start/platforms) | [Need Help?](https://generator.ky-programming.de/start/help)

## Setup for Visual Studio

### via Annotations
*Pros:* Easy to use, very quick to implement

*Cons:* Attributes are part of the build, annotations assembly has to be published and loaded

Install nuget package [KY.Generator](https://www.nuget.org/packages/KY.Generator/) ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

Decorate a class with one of our generate attributes
```
using KY.Generator;
...
[Generate("Output")]
internal class TypeToRead
{
  public string Property { get; set; }
}
```

See [documentation](https://generator.ky-programming.de/start/annotations/overview) for more details

### via Fluent API
*Pros:* generator code is completely separated and is not published, more actions available than via annotations

*Cons:* the initial setup is not so easy as with annotations

Create a new class library project

Install nuget package [KY.Generator.Fluent](https://www.nuget.org/packages/KY.Generator.Fluent/) ![](https://img.shields.io/nuget/v/KY.Generator.Fluent.svg?style=flat)

Derrive a class from GeneratorFluentMain, override the execute method and use the Read method
```
public class GeneratorMain : GeneratorFluentMain
{
    public override void Execute()
    {
        this.Read()
            .FromType<Types>()
            .Write()
            .AngularModels().OutputPath("Output/Models").SkipHeader()
            .AngularServices().OutputPath("Output/Services").SkipHeader();
    }
}
```

See [documentation](https://generator.ky-programming.de/start/fluent/overview) for more details

## Setup for Console/Powershell
Download KY.Generator.exe ![](https://img.shields.io/nuget/v/KY.Generator.CLI.svg?style=flat) from [Releases](https://github.com/KY-Programming/generator/releases)

Run a command
```
KY.Generator.exe reflection -assembly=KY.Generator.Examples.Reflection.dll -name=ExampleType -namespace=KY.Generator.Examples.Reflection -relativePath=Output -language=TypeScript
```
See [documentation](https://generator.ky-programming.de/start/commands/overview) for more details

## More Informations
For complete overview see our [documentation](https://generator.ky-programming.de)
