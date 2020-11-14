# KY.Generator ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

[Documentation](https://generator.ky-programming.de) | [Getting Started](https://generator.ky-programming.de/start) | [Supported Platforms](https://generator.ky-programming.de/start/platforms) | [Need Help?](https://ky-programming.de/contact/mail)

## Setup for Visual Studio

Install nuget package [KY.Generator](https://www.nuget.org/packages/KY.Generator/) ![](https://img.shields.io/nuget/v/KY.Generator.svg?style=flat)

All dependencies are included. At least .net Framework 4.6.1, .net Standard 2.0 or .net Core 2.0 is required

For lower framework version please [create an issue](https://github.com/KY-Programming/generator/issues/new) and we will try to support your version

See [documentation](https://generator.ky-programming.de) for more details

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
For complete overview see our [documentation](https://generator.ky-programming.de)
