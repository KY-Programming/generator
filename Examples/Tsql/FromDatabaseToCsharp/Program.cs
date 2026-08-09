using System;

namespace FromDatabaseToCsharp;

internal class Program
{
    private static void Main(string[] args)
    {
        // Nothing to run - the interesting part of this example happens at build time, see Generator.cs.
        // The generated models are compiled with this project, so a build that succeeds also proves the
        // generated C# is valid.
        Console.WriteLine("Models were generated into Output/ during the build.");
    }
}
