# Net8

Tests TypeScript generation targeting .NET 8 framework with comprehensive type coverage including generic types and nested complex types.

## Parameters

read-project -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\Net8\Net8.csproj ms-build options -output=C:\Projekte\C#\Generator\Tests\Net8 load -assembly=C:\Projekte\C#\Generator\Tests\Net8\bin\Debug\net8.0\Net8.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #453e0353
    - sub-type.ts #136e2bdb
    - types.ts #2f64f083

## Status

Last Build: 2026-05-16 10:47:48
Status: Warning
Info: 3 changed
Last Success: 2026-05-16 10:47:48
Generator: 10.0.0-preview.44
