using KY.Generator;

// The output folder is set here globally instead of on each [GenerateAngularModel] attribute.
// This is the place to change model and service paths for the whole project.
[assembly: GenerateModelOutput("Output")]

// Nothing switches strict mode on - it is the default. To opt out for every model of this project at
// once, uncomment the line below instead of annotating the single types:
// [assembly: GenerateNonStrict]
