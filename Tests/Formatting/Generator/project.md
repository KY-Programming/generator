# Generator

Tests custom formatting and code indentation configuration via fluent API, supporting configurable whitespace styles (tabs, spaces) and interface naming prefixes in TypeScript output.

## Output

- Output/
    - tab-test.ts #f5fb7b1e
    - two-whitespace-test.ts #1eea0bce
- Output/WithPrefix/
    - c-my-class-with-i-interface.ts #945c40e9
    - c-my-class-with-interface.ts #811c56df
    - interface.interface.ts #d8a20f6c
    - interface.ts #6553b5da
    - my-class-with-i-interface.ts #ba6e7cfa
    - my-class-with-interface.ts #66f62c76
- Output/WithoutPrefix/
    - interface.interface.ts #d8a20f6c
    - interface.ts #6553b5da
    - my-class-with-i-interface.ts #ba6e7cfa
    - my-class-with-interface.ts #66f62c76

## Status

Last Build: 2026-05-09 06:12:41
Status: Failure
Info: Build failed
Generator: 10.0.0-preview.39
