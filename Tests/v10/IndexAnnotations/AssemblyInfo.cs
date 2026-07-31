using KY.Generator;

// No GenerateNoIndex here - this project exists to cover the index.ts barrel file, which every
// other project switches off. GenerateForceIndex additionally requests it explicitly.
[assembly:GenerateNoHeader]
[assembly:GenerateModelOutput("Output")]
[assembly:GenerateStrict]
[assembly:GenerateForceIndex]
