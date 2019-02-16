# KY.Generator ![](https://img.shields.io/nuget/v/KY.Generator.Core.svg?style=flat)
We are not yet done, see the [dev-branch](https://github.com/KY-Programming/generator/tree/dev)

## Setup for Visual Studio
Install nuget package *KY.Generator.CLI* ![](https://img.shields.io/nuget/v/KY.Generator.CLI.svg?style=flat)  
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
Install nuget package *KY.Generator.CLI.Standalone* ![](https://img.shields.io/nuget/v/KY.Generator.CLI.Standalone.svg?style=flat)

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

# Languages
The generator has the possibility to support different languages. All commands and actions writes their output in a meta language, that can be converted in each language you want. 
## C# ![](https://img.shields.io/nuget/v/KY.Generator.Csharp.svg?style=flat)
Language name: *Csharp*  
## TypeScript ![](https://img.shields.io/nuget/v/KY.Generator.TypeScript.svg?style=flat)
Language name: *TypeScript*

# Reflection ![](https://img.shields.io/nuget/v/KY.Generator.Reflection.svg?style=flat)
Generate TypeScript classes from any .net type including all dependencies.  
Optionally convert all properties to public fields or vice versa.

How to configure [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Configuration#reflection)  
Commands are available [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Commands#reflection)  
Examples are found under [/Examples/Reflection](https://github.com/KY-Programming/generator/tree/master/Examples/Reflection)  

# Json ![](https://img.shields.io/nuget/v/KY.Generator.Json.svg?style=flat)
Create a class from a json file or request.  
Optionally append a reader method or class.  

How to configure [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Configuration#json)  
Examples are found under [/Examples/Json](https://github.com/KY-Programming/generator/tree/master/Examples/Json)

# Xml ![](https://img.shields.io/nuget/v/KY.Generator.Xml.svg?style=flat)
Create a class from a xml file or request.  
Optionally append a reader method or class.
_Currently this feature is not available_

# oData ![](https://img.shields.io/nuget/v/KY.Generator.Odata.svg?style=flat)
Creates a client for an oData service. Has the possibility to connect to an oData service, reads the $metadata and outputs models for all described types and a datacontex containing all datasets.  
_Currently this feature is not fully available_

# TSQL ![](https://img.shields.io/nuget/v/KY.Generator.Tsql.svg?style=flat)
Generate models from MS SQL-Server. Generate a Entity Framework compatible data context. Possibility to generate oData compatible ASP.net controllers.   
_Currently this feature is not fully available_

# ASP.net / ASP.net Core ![](https://img.shields.io/nuget/v/KY.Generator.AspDotNet.svg?style=flat)
Generate Angular client from an ASP.net controller.

# Watchdog ![](https://img.shields.io/nuget/v/KY.Generator.Watchdog.svg?style=flat)
Suspend generation until a website is available or a file or directory exists  

Command is available [(go to Wiki)](https://github.com/KY-Programming/generator/wiki/Commands#watchdog)
<!-- Examples are found under [/Examples/Json](https://github.com/KY-Programming/generator/tree/master/Examples/Json)  -->
