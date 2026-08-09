using KY.Generator;

// See AnnotationInBackground - the generation outlives the build, so the generator runs the validation.
[assembly: RunAtSuccess("node ./validate.js")]
[assembly: GenerateInBackground]
[assembly: GenerateNoHeader]
[assembly: GenerateNoIndex]
