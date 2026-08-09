# MyModule

A [KY.Generator](https://generator.ky-programming.de) module, created with `dotnet new ky-generator`.

## The three projects

| Project | Ships as | Contains |
|---|---|---|
| `MyModule` | `lib/netstandard2.0/` | the annotation, the command parameters and the fluent API - everything a user references |
| `MyModule.Generator` | `generators/netstandard2.0/` | the module, the command and the writer - loaded by the generator at run time, referenced by nobody |
| `MyModule.Example` | not published | uses the module, so `dotnet build` proves the whole loop works |

The split is not cosmetic. A user of your module compiles against the first assembly only; the second one
is loaded by the generator tool in its own process. That is why `MyModule.Generator` references
`KY.Generator.Sdk`, a package whose assemblies live under `ref/` and are never copied anywhere: two
copies of the engine in one process fail with `Could not load type ...`.

`AssemblyInfo.cs` is the link between the halves. Without `[assembly: GenerateWith(...)]` nothing happens.

## Try it

```bash
dotnet build
```

`MyModule.Example/Output/GreeterGreeter.cs` appears, generated from the `[GenerateHello]` attribute in
`Greeter.cs`, and is compiled into the example project.

## Publish it

```bash
dotnet pack MyModule/MyModule.csproj -c Release
```

One package with both halves in the right folders. The generator half is published automatically first -
see the `PublishGeneratorHalf` target in `MyModule.csproj`.

## Make it yours

1. Rename `SayHello` throughout - the command, its parameters, the writer.
2. Rewrite `HelloWriter` to emit what you actually need. The `Code` helper builds a language independent
   template tree, so the same writer can emit C# or TypeScript depending on the language set in
   `SayHelloCommand.Prepare()`.
3. To read something first - an assembly, a file, a database - add a reader and register a read command in
   `SampleModuleModule`, then `DependsOn<ReflectionModule>()` to reuse the existing type reader.

Both entry points are already wired: the `[GenerateHello]` annotation and the fluent `.SampleModule(...)`
action. Delete the one you do not want.
