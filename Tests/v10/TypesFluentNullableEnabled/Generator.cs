using KY.Generator;

namespace Types;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // this.Read(read => read.Reflection(reflection => reflection.FromType<Types>()))
        //     .Write(write => write.NoHeader()
        //                          .Strict()
        //                          .PreferInterfaces()
        //                          .Output("Output")
        //                          .TypeScriptModel());
    }
}
