# StrictAnnotationsNullableDisabled

Tests [GenerateStrict] annotation with nullable reference types disabled, controlling whether to generate only public properties or include internal/protected members in TypeScript output.

## Parameters

read-project -solution=*Undefined* -project=$\Tests\v10\StrictAnnotationsNullableDisabled\StrictAnnotationsNullableDisabled.csproj ms-build options -output=$\Tests\v10\StrictAnnotationsNullableDisabled load -assembly=$\Tests\v10\StrictAnnotationsNullableDisabled\bin\Debug\net9.0\StrictAnnotationsNullableDisabled.dll fluent annotation

## Output

- Output/
    - not-strict-class.ts #97b64102
    - not-strict-interface.ts #5b19adcb
    - strict-class.ts #8e73f95b
    - strict-interface.ts #47866a48

## Status

Last Build: 2026-07-25 16:44:31
Duration: 4,4s
Status: Success
Info: All outputs match
Last Success: 2026-07-25 16:44:31
Generator: 10.0.0-preview.48
