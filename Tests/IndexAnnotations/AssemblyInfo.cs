using KY.Generator;

// This project exists to cover the index.ts barrel file, which the ky-generator.json of the test
// folder switches off for everything below it. GenerateForceIndex requests it back.
[assembly:GenerateModelOutput("Output")]
[assembly:GenerateForceIndex]
