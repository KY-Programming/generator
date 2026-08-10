using KY.Generator;

// The generation outlives the build, so nothing that runs after the build can see its result - the
// generator runs the validation itself once the last file is written.
[assembly: RunAtSuccess("node ./validate.js")]
[assembly: GenerateInBackground]
[assembly: GenerateNoHeader]
[assembly: GenerateNoIndex]
