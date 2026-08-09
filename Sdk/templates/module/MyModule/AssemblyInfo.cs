using KY.Generator;

// This single line is what turns a library into a generator module. When a project references MyModule,
// the generator reads this attribute and loads MyModule.Generator - out of the "generators" folder of
// this package, or out of the output folder when both are project references.
//
// UseSameVersion keeps the two halves in lockstep: the generator half is always loaded at the version of
// this assembly, never a newer one that happens to be in the NuGet cache.
[assembly: GenerateWith("MyModule.Generator", UseSameVersion = true)]
