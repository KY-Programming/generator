using KY.Generator;

// What this project is about: the command has to run, and it has to run after the last file is written.
[assembly: RunAtSuccess("node ./run-at-success.js")]
[assembly: GenerateNoHeader]
[assembly: GenerateNoIndex]
