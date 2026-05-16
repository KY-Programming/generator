# Net10

Tests TypeScript generation with .NET 10 framework, covering various type annotations and ensuring compatibility with the latest .NET framework version for code generation.

## Parameters

readid -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\Net10\Net10.csproj msbuild set -output=C:\Projekte\C#\Generator\Tests\Net10\ load -assembly=C:\Projekte\C#\Generator\Tests\Net10\bin\Debug\net10.0\Net10.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #33e55207
    - sub-type.ts #4aed7519
    - types.ts #8a24f432

## Status

Last Build: 2026-05-16 10:47:47
Status: Warning
Info: 3 changed
Last Success: 2026-05-16 10:47:47
Generator: 10.0.0-preview.44
