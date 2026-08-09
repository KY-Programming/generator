# KY.Generator.Sdk

Build your own [KY.Generator](https://generator.ky-programming.de) module.

A module is two assemblies, packed into one NuGet package:

| Assembly | Package folder | Contains | Referenced |
|---|---|---|---|
| `MyModule` | `lib/netstandard2.0/` | annotations, command parameters, fluent interfaces | by the projects that use your module |
| `MyModule.Generator` | `generators/netstandard2.0/` | the module class, commands, readers, writers | by nobody - the generator loads it at run time |

This package is what the `.Generator` half compiles against. It carries the module, command, writer and
template API, plus the base assemblies of the Common, C#, TypeScript, Angular and Reflection modules, so
a module can build on the existing readers and writers.

```xml
<PackageReference Include="KY.Generator.Sdk" Version="10.0.1" />
```

## Development time only

The assemblies ship under `ref/`, not `lib/`. NuGet uses them to compile and never copies them to your
output folder, so:

- your module package never contains a copy of the engine,
- nothing has to be excluded with `PrivateAssets` or `ExcludeAssets`,
- the generator cannot pick up a stale engine assembly from your output folder and load it instead of the
  one the running tool already has.

The last point is not theoretical. When two versions of the engine meet in one process, generation fails
with `Could not load type ...` rather than with a version error.

Use the same SDK version as the `KY.Generator` version your module is meant to run with.

## Wiring

The base assembly points the generator at the other half:

```csharp
[assembly: GenerateWith("MyModule.Generator", UseSameVersion = true)]
```

The generator half declares the module:

```csharp
public class MyModule : GeneratorModule
{
    public MyModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependsOn<ReflectionModule>();
        this.Register<MyCommand>(MyCommandParameters.Names);
    }
}
```

And the base package packs the generator half into `generators/`:

```xml
<None Include="$(OutputPath)\publish\MyModule.Generator.dll"
      Pack="true" PackagePath="generators\netstandard2.0\" Visible="false" />
```

See the [CustomModule example](https://github.com/KY-Programming/generator/tree/master/Examples/Customization)
for the complete, working set of projects.
