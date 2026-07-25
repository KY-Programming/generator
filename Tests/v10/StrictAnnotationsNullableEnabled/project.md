# StrictAnnotationsNullableEnabled

Tests [GenerateStrict] annotation with nullable reference types enabled, controlling whether to generate only public properties or include internal/protected members in TypeScript output.

## Parameters

read-project -solution=*Undefined* -project=$\Tests\v10\StrictAnnotationsNullableEnabled\StrictAnnotationsNullableEnabled.csproj ms-build options -output=$\Tests\v10\StrictAnnotationsNullableEnabled load -assembly=$\Tests\v10\StrictAnnotationsNullableEnabled\bin\Debug\net9.0\StrictAnnotationsNullableEnabled.dll fluent annotation

## Output

- Output/
    - not-strict-class.ts #d8cb529b
    - not-strict-interface.ts #3734e088
    - strict-class.ts #351493ee
    - strict-interface.ts #67d8a61c

## Status

Last Build: 2026-07-25 16:44:28
Duration: 4,4s
Status: Success
Info: All outputs match
Last Success: 2026-07-25 16:44:28
Generator: 10.0.0-preview.48
