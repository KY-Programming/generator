# KY.Generator ![](https://img.shields.io/nuget/v/KY.Generator.Core.svg?style=flat)

## Setup for Visual Studio
### .net Core
Install nuget package [KY.Generator.CLI.Core.Standalone](https://www.nuget.org/packages/KY.Generator.CLI.Core.Standalone/) ![](https://img.shields.io/nuget/v/KY.Generator.CLI.Core.Standalone.svg?style=flat)

All dependencies are included. At least .net Core 2.0 is required

For lower framework version please [create an issue](https://github.com/KY-Programming/generator/issues/new) and we will try to support your version

Add an generator.json configuration file [(see Wiki)](https://github.com/KY-Programming/generator/wiki/v2:-Overview#modules)  
and add an Pre-build event
```
dotnet "%USERPROFILE%\.nuget\packages\ky.generator.cli\2.5.0\tools\KY.Generator.dll" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```

### .net Framework
Install nuget package [KY.Generator.CLI.Standalone](https://www.nuget.org/packages/KY.Generator.CLI.Standalone/) ![](https://img.shields.io/nuget/v/KY.Generator.CLI.Standalone.svg?style=flat)  

All dependencies are included. At least .net Framework 4.6 is required

For lower framework version please [create an issue](https://github.com/KY-Programming/generator/issues/new) and we will try to support your version

Add an generator.json configuration file [(see Wiki)](https://github.com/KY-Programming/generator/wiki/v2:-Overview#modules)  
and add an Pre-build event
```
"$(SolutionDir)packages\ky.generator.cli.standalone\2.5.0\tools\KY.Generator.exe" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```
```
"<path-to-KY.Generator.exe>" "<path-to-configuration-file>" "<output-path>"
```

## Setup for Console/Powershell
Download KY.Generator.exe ![](https://img.shields.io/nuget/v/KY.Generator.CLI.svg?style=flat) from [Releases](https://github.com/KY-Programming/generator/releases)

### Run with configuration file
Use an .json configuration file [(see Wiki)](https://github.com/KY-Programming/generator/wiki/v2:-Overview#modules)   
and run in cmd:
```
KY.Generator configuration.json Output
```
```
KY.Generator <path-to-configuration-file> <output-path>
```

### Run a command
Run a command [(see Wiki)](https://github.com/KY-Programming/generator/wiki/v2:-Overview#commands)
```
KY.Generator.exe reflection -assembly=KY.Generator.Examples.Reflection.dll -name=ExampleType -namespace=KY.Generator.Examples.Reflection -relativePath=Output -language=TypeScript
```

## More Informations
For complete overview [see our Wiki](https://github.com/KY-Programming/generator/wiki/v2:-Overview)
