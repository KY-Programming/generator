# KY.Generator ![](https://img.shields.io/nuget/v/KY.Generator.CLI.svg?style=flat)
We are not yet done, see the [dev-branch](https://github.com/KY-Programming/generator/tree/dev)

## Setup for Visual Studio
Install nuget package *KY.Generator.CLI*  
All dependencies are automatically installed. We support .net Framework 4.6 or .net Core 2.0  
For lower framework version please [create an issue](https://github.com/KY-Programming/generator/issues/new) and we will try to support your version

Add an .json or .xml configuration file [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Configuration)  
and add an Pre-build event
### .net Core
```
"%USERPROFILE%\.nuget\packages\ky.generator.cli\0.8.0\tools\KY.Generator.exe" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```

### .net Framework
```
"$(SolutionDir)packages\ky.generator.cli\0.8.0\tools\KY.Generator.exe" "$(ProjectDir)generator.json" "$(ProjectDir)ClientApp\src\app"
```
```
"<path-to-KY.Generator.exe>" "<path-to-configuration-file>" "<output-path>"
```

## Setup for Console/Powershell
Install nuget package *KY.Generator.CLI.Standalone*

### Run with configuration file
Use an .json or .xml configuration file [(see here)](https://github.com/KY-Programming/generator/wiki/Configuration)  
and run in cmd:
```
KY.Generator configuration.json Output
```
```
KY.Generator <path-to-configuration-file> <output-path>
```

### Run a command
Run a command [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Commands)
```
KY.Generator.exe reflection -assembly=KY.Generator.Examples.Reflection.dll -name=ExampleType -namespace=KY.Generator.Examples.Reflection -relativePath=Output -language=TypeScript
```

# Reflection ![](https://img.shields.io/nuget/v/KY.Generator.Reflection.svg?style=flat)
Generate TypeScript classes from any .net type including all dependencies.  
Optionally convert all properties to public fields or vice versa.

How to configure [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Configuration#reflection)  
Commands are available [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Commands#reflection)  
Examples are found under [/Examples/KY.Generator.Examples.Reflection](https://github.com/KY-Programming/generator/tree/master/Examples/KY.Generator.Examples.Reflection)  
