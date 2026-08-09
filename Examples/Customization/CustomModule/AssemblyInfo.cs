using KY.Generator;

// This single line is what turns a library into a generator module: it names the other half. Without it
// CustomModule.Generator is never loaded and the write-message command does not exist.
//
// A published module ships that half in the "generators" folder of its NuGet package. Here the console
// project references it directly, so the generator finds it in the output folder instead.
[assembly: GenerateWith("CustomModule.Generator", UseSameVersion = true)]
