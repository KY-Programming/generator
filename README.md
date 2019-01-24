# KY.Generator
We are not yet done, see the [dev-branch](https://github.com/KY-Programming/generator/tree/dev)

## Setup for Visual Studio
Install nuget package KY.Generator.CLI  
All dependencies are automatically installed. We support .net Framework 4.6 or .net Core 2.0  
For lower framework version please [create an issue](https://github.com/KY-Programming/generator/issues/new) and we will try to support your version

Add an .json or .xml configuration file [(see here)](https://github.com/KY-Programming/generator/wiki/Configuration)  
and add an Pre-build event
### .net Core
```
"%USERPROFILE%\.nuget\packages\ky.generator.cli\0.8.0\tools\KY.Generator.exe" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```

### .net Framework
```
"$(SolutionDir)packages\ky.generator.cli\0.8.0\tools\KY.Generator.exe" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```

pathToKY.Generator.exe pathToConfigurationFile outputPath

## Setup for Console/Powershell
Install nuget package KY.Generator.CLI.Standalone

### Run with configuration file
Use an .json or .xml configuration file [(see here)](https://github.com/KY-Programming/generator/wiki/Configuration)  
and run in cmd:
```
KY.Generator configuration.json Output
```
KY.Generator pathToConfigurationFile outputPath

### Run a command
Run a command [(see here)](https://github.com/KY-Programming/generator/wiki/Commands)
```
KY.Generator.exe reflection -assembly=KY.Generator.Examples.Reflection.dll -name=ExampleType -namespace=KY.Generator.Examples.Reflection -relativePath=Output -language=TypeScript
```

## KY.Generator.Reflection
Generate TypeScript classes from any .net assembly

Examples are found under [/Examples/KY.Generator.Examples.Reflection](https://github.com/KY-Programming/generator/tree/master/Examples/KY.Generator.Examples.Reflection)
