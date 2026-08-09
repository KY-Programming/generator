using KY.Generator;

// The generation of this project runs in the background, so the build is over before a single file is written.
// Nothing that runs after the build can see the result, so the validation is run by the generator itself.
[assembly: RunAtSuccess("node ./validate.js")]
[assembly: GenerateNoHeader]
[assembly: GenerateNoIndex]
