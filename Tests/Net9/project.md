# Net9

Tests TypeScript generation targeting .NET 9 framework with comprehensive type coverage including generic types and nested complex types.

## Parameters

read-project -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\Net9\Net9.csproj ms-build options -output=C:\Projekte\C#\Generator\Tests\Net9 load -assembly=C:\Projekte\C#\Generator\Tests\Net9\bin\Debug\net9.0\Net9.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #12700901
    - sub-type.ts #9ff70d0b
    - types.ts #e835f698

## Status

Last Build: 2026-05-16 10:47:43
Status: Warning
Info: 3 changed
Last Success: 2026-05-16 10:47:43
Generator: 10.0.0-preview.44
