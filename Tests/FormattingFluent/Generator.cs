using KY.Generator;

namespace FormattingFluent;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // Four runs in one build, each writing with a different formatting configuration. The indent runs
        // write into the same folder, the prefix runs into two folders, so the same four types can be
        // compared with and without the prefixes.
        this.Read(read => read.Reflection(reflection => reflection.FromType<TwoWhitespaceTest>()))
            .Write(write => write.TestDefaults()
                                 .Formatting(config => config.UseWhitespaces(2))
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output"))));

        this.Read(read => read.Reflection(reflection => reflection.FromType<TabTest>()))
            .Write(write => write.TestDefaults()
                                 .Formatting(config => config.UseTab())
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output"))));

        // The class prefix is added and then stripped from the file name again, so the prefix ends up in
        // the type name only - that the two are configured independently is the point of the FileName call.
        this.Read(read => read.Reflection(reflection => reflection.FromType<IInterface>()
                                                                  .FromType<Interface>()
                                                                  .FromType<MyClassWithInterface>()
                                                                  .FromType<MyClassWithIInterface>()))
            .Write(write => write.TestDefaults()
                                 .Formatting(config => config.InterfacePrefix("I").ClassPrefix("C"))
                                 .FileName(config => config.Replace("^c-(.*)$", "$1"))
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output/WithPrefix"))));

        this.Read(read => read.Reflection(reflection => reflection.FromType<IInterface>()
                                                                  .FromType<Interface>()
                                                                  .FromType<MyClassWithInterface>()
                                                                  .FromType<MyClassWithIInterface>()))
            .Write(write => write.TestDefaults()
                                 .Angular(angular => angular.Models(config => config.OutputPath("Output/WithoutPrefix"))));
    }
}

file static class WriteSyntaxExtension
{
    /// <summary>What every run of this project shares - only the formatting is supposed to differ.</summary>
    public static IWriteFluentSyntax TestDefaults(this IWriteFluentSyntax syntax) => syntax.NoHeader().NoIndex();
}
