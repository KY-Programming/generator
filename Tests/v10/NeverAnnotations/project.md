# NeverAnnotations

Covers GenerateNever: a type marked with it must never end up in the output. ReferencingDto is the type that
should be generated, but it exposes NeverGeneratedModel, so the generator would write that model too. Instead
the generation is aborted with an error that contains the path of the file the forbidden type would have been
written to (Output/never-generated-model.ts), so the class that drags it into the output is easy to find.

This project is verified by a script instead of by output hashes - it must NOT build. Validate/validate.cmd
returns 200 when the build failed for exactly that reason, and produced no output file.

## Output

- Validate/
    - validate.cmd
