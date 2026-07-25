# SignalsAnnotations

Tests the [GenerateWithSignals] annotation. Every member of an annotated model is generated as `WritableSignal<T>`
instead of a plain member. Optional members keep their optionality in the value type (`WritableSignal<T | undefined>`),
the member itself is always present. Models without the annotation stay untouched.

## Output

- Output/
    - plain-model.ts
    - signal-model.ts
    - sub-model.ts
    - without-signals-model.ts
