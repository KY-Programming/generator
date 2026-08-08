# Angular Fluent Examples

These examples configure generation with the **fluent API** instead of annotations. Each one has
an equivalent under [`../Annotation`](../Annotation) that produces the same output, so the two
folders can be compared side by side.

## Why every example has two projects

This is the whole point of the fluent API, and the main reason to choose it over annotations.

Annotations live *in* the project being generated from, so that project has to reference
`KY.Generator` and the annotation packages. The generator and its assemblies end up as
dependencies of your application.

The fluent API moves the generator out. Each example is therefore split in two:

| Project | Contains | References |
|---|---|---|
| `Service` / `Assembly` | The application: controllers, hubs, models, `ClientApp` | No generator packages |
| `Generator` | Only `GeneratorMain.cs`, the generation configuration | `KY.Generator.*`, plus a `ProjectReference` to the application |

The `Generator` project reads the application by reference and writes into its `ClientApp`. The
application itself stays clean: no generator packages in its dependency tree, nothing
generator-related shipped or published with it.

Use annotations when convenience matters more than isolation; use the fluent API when the
generated project must not carry the generator along.

## Choosing between them

- **Annotations** — configuration sits next to the type it applies to. Output paths are set once
  in `AssemblyInfo.cs`. One project.
- **Fluent** — configuration sits in one file, in a separate project. Nothing about generation
  leaks into the application.

Some capabilities are fluent-only, because they describe the generation run rather than a type:
`NpmPackage` (packaging generated code as an npm library), and `WithCustomHttpClient`
(substituting the HTTP client and renaming the generated methods).
