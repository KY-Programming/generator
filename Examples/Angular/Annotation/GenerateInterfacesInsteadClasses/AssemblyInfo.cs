using KY.Generator;

// The output folder is set here globally instead of on each [GenerateAngularModel] attribute.
// This is the place to change model and service paths for the whole project.
[assembly: GenerateModelOutput("Output")]

// Emit TypeScript interfaces instead of classes for every model in this project.
// It can also be applied to a single type instead of the whole assembly.
[assembly: GeneratePreferInterfaces]
