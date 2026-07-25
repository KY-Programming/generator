# SignalsServiceAnnotations

Tests the generated Angular service for models that are generated with [GenerateWithSignals]. The service gets a public
`wrapMyModel` / `unwrapMyModel` pair per model, wraps every value that is read from the backend into signals and unwraps
it again before it is written back. Models without signals stay untouched.

The plain shape that is sent by and to the backend is described by the generated `Unwrapped<T>` helper type, which is
written next to the models.

## Parameters

readid -solution=*Undefined* -project=$\Tests\v10\SignalsServiceAnnotations\SignalsServiceAnnotations.csproj msbuild set -output=$\Tests\v10\SignalsServiceAnnotations\ load -assembly=$\Tests\v10\SignalsServiceAnnotations\bin\Debug\net10.0\SignalsServiceAnnotations.dll fluent annotation

## Output

- Output/
    - plain-model.ts #deb340c5
    - signal-model.ts #5cbafc80
    - signals.service.ts #31a926fc
    - sub-model.ts #eeba8e6f
    - unwrapped.ts #8bd28e12

## Status

Last Build: 2026-07-25 16:44:39
Duration: 6,0s
Status: Success
Info: All outputs match
Last Success: 2026-07-25 16:44:39
Generator: 10.0.0-preview.48
