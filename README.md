# KY.Generator&nbsp;![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

[Documentation](https://generator.ky-programming.de) | [Getting Started](https://generator.ky-programming.de/start) | [Supported Platforms](https://generator.ky-programming.de/start/platforms) | [Need Help?](https://generator.ky-programming.de/start/help)

## Setup for Visual Studio

### via Annotations

*Pros:* Easy to use, rapid to implement

*Cons:* Attributes are part of the build, annotation assemblies have to be published and loaded when reflection is used

Install nuget package [KY.Generator](https://www.nuget.org/packages/KY.Generator/)&nbsp;![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

Decorate a class with one of our `Generate` attributes

```
using KY.Generator;
...
[Generate("Output")]
internal class TypeToRead
{
  public string Property { get; set; }
}
```

See the [complete showcase](https://generator.ky-programming.de/start/showcases/annotations)

See [documentation](https://generator.ky-programming.de/start/annotations/overview) for more details

### via Fluent API

*Pros:* generator code is completely separated and is not published, more actions are available than via annotations

*Cons:* the initial setup is not so easy as with annotations

Create a new class library project

Install nuget package [KY.Generator.Fluent](https://www.nuget.org/packages/KY.Generator.Fluent/)&nbsp;![](https://img.shields.io/nuget/v/KY.Generator.Fluent.svg?style=flat)

Derive a class from GeneratorFluentMain, override the execute method and use the Read method

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

See the [complete showcase](https://generator.ky-programming.de/start/fluent-api/annotations)

See [documentation](https://generator.ky-programming.de/start/fluent/overview) for more details

## Setup for Console/PowerShell

Install the .NET global tool&nbsp;![](https://img.shields.io/nuget/v/KY.Generator.CLI.svg?style=flat)

```
dotnet tool install -g KY.Generator.CLI
```

Run a command

```
ky-generator reflection -assembly=KY.Generator.Examples.Reflection.dll -name=ExampleType -namespace=KY.Generator.Examples.Reflection -relativePath=Output -language=TypeScript
```

The tool ships every built-in module, so nothing has to be installed next to it. It requires the .NET 8, 9 or
10 runtime; an assembly built for one of those is read by a matching process, whichever of them the tool itself
was started on.

See [documentation](https://generator.ky-programming.de/start/commands/overview) for more details

## Read More

For a complete overview see our [documentation](https://generator.ky-programming.de)
