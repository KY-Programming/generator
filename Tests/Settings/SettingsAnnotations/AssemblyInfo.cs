using KY.Generator;

// The ky-generator.json above this project names another output path. An attribute sits closer to the generated
// type than any configuration file, so this one has to win.
[assembly: GenerateModelOutput("Output")]
