# Examples

This folder contains small, self-contained sample projects — one per feature. Each of them shows a single aspect of
KY.Generator: which reader is used, which writer produces the output, and how the generator is triggered.

See the [documentation](https://generator.ky-programming.de) for the full reference.

## Angular

### via Annotations

Generation is configured with attributes directly on the C# types.

| Example | What it shows |
|---|---|
| [FromModel](Angular/Annotation/FromModel) | Generating TypeScript models from C# classes with `[GenerateAngularModel]` — [TypeToRead.cs](Angular/Annotation/FromModel/TypeToRead.cs). |
| [ModelFromAssembly](Angular/Annotation/ModelFromAssembly) | Pulling in a whole set of types through an index type marked with `[GenerateOnlySubTypes]` — [Index.cs](Angular/Annotation/ModelFromAssembly/Index.cs). |
| [GenerateInterfacesInsteadClasses](Angular/Annotation/GenerateInterfacesInsteadClasses) | The assembly-wide `[GeneratePreferInterfaces]` annotation, which makes every model come out as a TypeScript interface instead of a class — [AssemblyInfo.cs](Angular/Annotation/GenerateInterfacesInsteadClasses/AssemblyInfo.cs). |
| [NonStrict](Angular/Annotation/NonStrict) | Opting a model out of TypeScript's strict mode with `[GenerateNonStrict]`, so members are not initialized with a default value — [LegacyWeatherForecast.cs](Angular/Annotation/NonStrict/LegacyWeatherForecast.cs). |
| [ServiceFromAspNetCore](Angular/Annotation/ServiceFromAspNetCore) | Generating Angular services and models from an ASP.NET Core WebAPI controller with `[GenerateAngularService]` — [WeatherForecastController.cs](Angular/Annotation/ServiceFromAspNetCore/Controllers/WeatherForecastController.cs). |
| [ServiceFromAspNetCoreSignalRHub](Angular/Annotation/ServiceFromAspNetCoreSignalRHub) | Generating an Angular service from a SignalR hub with `[GenerateAngularHub]`, including the retry configuration — [WeatherHub.cs](Angular/Annotation/ServiceFromAspNetCoreSignalRHub/Hubs/WeatherHub.cs). |
| [ChangeReturnType](Angular/Annotation/ChangeReturnType) | Overriding method return types and wiring in external TypeScript types with `[GenerateImport]` — [WeatherForecastController.cs](Angular/Annotation/ChangeReturnType/Controllers/WeatherForecastController.cs). |

### via Fluent API

Each example consists of the demo application and a separate `*.Generator` project that contains the fluent
configuration — that `GeneratorMain.cs` is the interesting file.

| Example | What it shows |
|---|---|
| [FromModel](Angular/Fluent/FromModel) | The fluent counterpart to the annotation example: TypeScript models from C# classes — [GeneratorMain.cs](Angular/Fluent/FromModel.Generator/GeneratorMain.cs). |
| [GenerateInterfacesInsteadClasses](Angular/Fluent/GenerateInterfacesInsteadClasses) | Configuring the writer to emit interfaces instead of classes — [GeneratorMain.cs](Angular/Fluent/GenerateInterfacesInsteadClasses.Generator/GeneratorMain.cs). |
| [NonStrict](Angular/Fluent/NonStrict) | Writing the same model once for strict and once for non-strict TypeScript projects with `NonStrict()` — [GeneratorMain.cs](Angular/Fluent/NonStrict.Generator/GeneratorMain.cs). |
| [ServiceFromAspNetCore](Angular/Fluent/ServiceFromAspNetCore) | Angular services and models from ASP.NET Core controllers — [GeneratorMain.cs](Angular/Fluent/ServiceFromAspNetCore.Generator/GeneratorMain.cs). |
| [ServiceFromSignalR](Angular/Fluent/ServiceFromSignalR) | Angular services from SignalR hubs — [GeneratorMain.cs](Angular/Fluent/ServiceFromSignalR.Generator/GeneratorMain.cs). |
| [ChangeReturnType](Angular/Fluent/ChangeReturnType) | Overriding service method return types with custom TypeScript models — [GeneratorMain.cs](Angular/Fluent/ChangeReturnType.Generator/GeneratorMain.cs). |
| [WithCustomHttpClient](Angular/Fluent/WithCustomHttpClient) | Making the generated services use your own HTTP client instead of Angular's `HttpClient` — [GeneratorMain.cs](Angular/Fluent/WithCustomHttpClient.Generator/GeneratorMain.cs). |
| [NpmPackage](Angular/Fluent/NpmPackage) | Generating into an Angular library structure so the result can be published as an npm package — [GeneratorMain.cs](Angular/Fluent/NpmPackage.Generator/GeneratorMain.cs). |

## AspDotNet

| Example | What it shows |
|---|---|
| [WebApi.Core](AspDotNet/WebApi.Core) | Reading an ASP.NET Core WebAPI controller and writing an Angular service, configured in a [generator.json](AspDotNet/WebApi.Core/generator.json). |
| [WebApi.Attributes.Core](AspDotNet/WebApi.Attributes.Core) | The same for ASP.NET Core, but configured with attributes on the controller — [ValuesController.cs](AspDotNet/WebApi.Attributes.Core/Controllers/ValuesController.cs). |
| [WebApi](AspDotNet/WebApi) | The classic ASP.NET Web API (.NET Framework) variant — [generator.json](AspDotNet/WebApi/generator.json). |
| [WebApi.Attributes](AspDotNet/WebApi.Attributes) | The classic ASP.NET Web API variant, configured with attributes — [ValuesController.cs](AspDotNet/WebApi.Attributes/Controllers/ValuesController.cs). |

## Reflection

Reading types from compiled assemblies instead of source code.

| Example | What it shows |
|---|---|
| [ReflectionFromAttributes](Reflection/ReflectionFromAttributes) | Discovering the types to generate through `[GenerateTypeScriptModel]` in the loaded assembly — [TypeToRead.cs](Reflection/ReflectionFromAttributes/TypeToRead.cs). |
| [ReflectionFromConstant](Reflection/ReflectionFromConstant) | How constants and static fields are carried over into the generated model — [Class1.cs](Reflection/ReflectionFromConstant/Class1.cs). |
| [ReflectionFromIndex](Reflection/ReflectionFromIndex) | Forcing an index file for the generated types with `[assembly: GenerateForceIndex]` — [AssemblyInfo.cs](Reflection/ReflectionFromIndex/AssemblyInfo.cs). |
| [ReflectionFromExecutable](Reflection/ReflectionFromExecutable) | Loading the types from a compiled executable instead of a library — [TypeToRead.cs](Reflection/ReflectionFromExecutable/TypeToRead.cs). |
| [ReflectionFromMultipleAssemblies](Reflection/ReflectionFromMultipleAssemblies) | A type referencing a type from another assembly, which is resolved and generated as well — [TypeToRead.cs](Reflection/ReflectionFromMultipleAssemblies/MainAssembly/TypeToRead.cs). |
| [ReflectionIgnoreAttribute](Reflection/ReflectionIgnoreAttribute) | Excluding single types, properties and enum entries with `[GenerateIgnore]` — [TypeToRead.cs](Reflection/ReflectionIgnoreAttribute/TypeToRead.cs). |
| [ReflectionReturnTypeAttribute](Reflection/ReflectionReturnTypeAttribute) | Replacing the type of a property with `[GenerateProperty(Type = ...)]` — [Types.cs](Reflection/ReflectionReturnTypeAttribute/Types.cs). |

## Other readers

| Example | What it shows |
|---|---|
| [Json/JsonWithReader](Json/JsonWithReader) | Reading JSON files and writing C# models with and without reader, plus Angular models — [Generator.cs](Json/JsonWithReader/Generator.cs). |
| [Sqlite/FromDatabase](Sqlite/FromDatabase) | Reading an existing SQLite database and generating C# models from its schema — [Generator.cs](Sqlite/FromDatabase/Generator.cs). |
| [Sqlite/ToDatabase](Sqlite/ToDatabase) | Describing a table with `[GenerateSqliteRepository]` and column attributes and generating the repository — [Person.cs](Sqlite/ToDatabase/Person.cs). |
| [Tsql/Tsql](Tsql/Tsql) | Reading a table from a T-SQL server and generating a C# model — [Generator.cs](Tsql/Tsql/Generator.cs). |

## Customization

| Example | What it shows |
|---|---|
| [CustomModule](Customization/CustomModule) | Building your own generator module — the module registers the command and the fluent syntax extension — [Module.cs](Customization/CustomModule/Module.cs). |
| [CustomModule.Console](Customization/CustomModule.Console) | Using that custom module from a fluent generator project — [Generator.cs](Customization/CustomModule.Console/Generator.cs). |
