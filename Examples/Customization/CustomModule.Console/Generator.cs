using KY.Generator;

namespace CustomModule.Console;

/// <summary>
/// The configuration side of the example. HelloWorld and Message are not part of KY.Generator - they come
/// from the CustomModule project next door, which is what this example is about: a module adds its own
/// action to the fluent syntax and it reads like any built-in one.
/// </summary>
public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // The fluent pipeline is always read then write. This module has nothing to read - it generates
        // from its parameters alone - so the read step stays empty.
        this.Read(read => { })
            .Write(write => write
                       .HelloWorld(hello => hello
                                       .Message("Hello World!", "Greeter", "CustomModule.Console.Generated", "Output")));
    }
}
