# Net7

Tests TypeScript generation targeting .NET 7 framework with comprehensive type coverage including generic types and nested complex types.

## Parameters

read-project -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\Net7\Net7.csproj ms-build options -output=C:\Projekte\C#\Generator\Tests\Net7 load -assembly=C:\Projekte\C#\Generator\Tests\Net7\bin\Debug\net7.0\Net7.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #2b7a83c8
    - sub-type.ts #b77658c0
    - types.ts #3ad403be

## Status

Last Build: 2026-05-16 10:47:43
Status: Warning
Info: 3 changed
Last Success: 2026-05-16 10:47:43
Generator: 10.0.0-preview.44
