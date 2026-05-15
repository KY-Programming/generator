# AnnotationsWithMultipleOutputs

Tests generating multiple classes with [GenerateAngularModel] annotation targeting different output directories, demonstrating support for shared dependencies (SubType) across multiple output locations.

## Output

- Output/First/
    - first-type.ts #440afe31
    - index.ts #cb47a2b5
    - sub-type.ts #b2ac0b4b
- Output/Second/
    - index.ts #a83914f6
    - second-type.ts #972656ea
    - sub-type.ts #b2ac0b4b
- Output/Third/
    - third-type.ts #13c08a30

## Status

Last Build: 2026-05-09 06:12:32
Status: Verify Missing
Info: 7 missing
Generator: 10.0.0-preview.39
