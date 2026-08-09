using System;

namespace FromDatabaseToTypeScript;

internal class Program
{
    private static void Main(string[] args)
    {
        // Nothing to run - the interesting part of this example happens at build time, see Generator.cs.
        // The generated TypeScript is type-checked afterwards by validate.js.
        Console.WriteLine("Models were generated into Output/ during the build.");
    }
}
