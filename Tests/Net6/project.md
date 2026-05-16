# Net6

Tests TypeScript generation targeting .NET 6 framework with comprehensive type coverage including generic types and nested complex types.

## Parameters

read-project -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\Net6\Net6.csproj ms-build options -output=C:\Projekte\C#\Generator\Tests\Net6 load -assembly=C:\Projekte\C#\Generator\Tests\Net6\bin\Debug\net6.0\Net6.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #687e05c8
    - sub-type.ts #2b412e4c
    - types.ts #f24f4056

## Status

Last Build: 2026-05-16 10:47:43
Status: Warning
Info: 3 changed
Last Success: 2026-05-16 10:47:43
Generator: 10.0.0-preview.44
