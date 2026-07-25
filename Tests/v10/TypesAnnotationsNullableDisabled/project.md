# TypesAnnotationsNullableDisabled

Tests comprehensive TypeScript type generation with nullable reference types disabled, covering primitive types, nullable types, system types, collections, generics, arrays, dictionaries, and complex nested types.

## Parameters

read-project -solution=*Undefined* -project=$\Tests\v10\TypesAnnotationsNullableDisabled\TypesAnnotationsNullableDisabled.csproj ms-build options -output=$\Tests\v10\TypesAnnotationsNullableDisabled load -assembly=$\Tests\v10\TypesAnnotationsNullableDisabled\bin\Debug\net9.0\TypesAnnotationsNullableDisabled.dll fluent annotation

## Output

- Output/
    - generic-sub-type.ts #ea543311
    - sub-type.ts #be8084b8
    - types.ts #a6aeb1e7

## Status

Last Build: 2026-07-25 16:44:31
Duration: 4,7s
Status: Success
Info: All outputs match
Last Success: 2026-07-25 16:44:31
Generator: 10.0.0-preview.48
