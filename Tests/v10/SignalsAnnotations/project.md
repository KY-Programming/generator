# SignalsAnnotations

Tests the [GenerateWithSignals] annotation. Every member of an annotated model is generated as `WritableSignal<T>`
instead of a plain member. Optional members keep their optionality in the value type (`WritableSignal<T | undefined>`),
the member itself is always present. Models without the annotation stay untouched.

## Parameters

readid -solution=*Undefined* -project=C:\Projekte\C#\Generator\Tests\v10\SignalsAnnotations\SignalsAnnotations.csproj msbuild set -output=C:\Projekte\C#\Generator\Tests\v10\SignalsAnnotations\ load -assembly=C:\Projekte\C#\Generator\Tests\v10\SignalsAnnotations\bin\Debug\net10.0\SignalsAnnotations.dll fluent annotation

## Output

- Output/
    - plain-model.ts #0b473596
    - signal-model.ts #77f875b6
    - sub-model.ts #a5180187
    - without-signals-model.ts #1268a5fd

## Status

Last Build: 2026-07-25 12:25:01
Status: Warning
Info: 4 changed
Last Success: 2026-07-25 12:25:01
Generator: 10.0.0-preview.47
