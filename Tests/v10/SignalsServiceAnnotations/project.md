# SignalsServiceAnnotations

Tests the generated Angular service for models that are generated with [GenerateWithSignals]. The service gets a public
`wrapMyModel` / `unwrapMyModel` pair per model, wraps every value that is read from the backend into signals and unwraps
it again before it is written back. Models without signals stay untouched.

The plain shape that is sent by and to the backend is described by the generated `Unwrapped<T>` helper type, which is
written next to the models.

## Output

- Output/
    - plain-model.ts
    - signal-model.ts
    - signals.service.ts
    - sub-model.ts
    - unwrapped.ts
