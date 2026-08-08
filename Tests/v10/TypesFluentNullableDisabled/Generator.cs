using KY.Generator;

namespace Types;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // The fluent counterpart of TypesAnnotationsNullableDisabled - same types, same output, configured
        // here instead of with annotations. Strict mode is not configured, so the default applies.
        this.Read(read => read.Reflection(reflection => reflection.FromType<Types>()))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .TypeScriptModel(model => model.PreferInterfaces()
                                                                .OutputPath("Output")));
    }
}
