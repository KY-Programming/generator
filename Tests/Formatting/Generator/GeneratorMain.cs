using Formatting;
using KY.Generator;
using KY.Generator.Syntax;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                   .Reflection(reflection => reflection
                       .FromType<TwoWhitespaceTest>()))
               .Write(write => write
                   .SetTestDefaults()
                   .Formatting(config => config.UseWhitespaces(2))
                   .Angular(angular => angular.Models(config => config.OutputPath("Output"))));

            this.Read(read => read
                   .Reflection(reflection => reflection
                       .FromType<TabTest>()))
               .Write(write => write
                   .SetTestDefaults()
                   .Formatting(config => config.UseTab())
                   .Angular(angular => angular.Models(config => config.OutputPath("Output"))));

            this.Read(read => read
                   .Reflection(reflection => reflection
                       .FromType<IInterface>()
                       .FromType<Interface>()
                       .FromType<MyClassWithInterface>()
                       .FromType<MyClassWithIInterface>()))
               .Write(write => write
                   .SetTestDefaults()
                   .Formatting(config => config.InterfacePrefix("I").ClassPrefix("C"))
                   .FileName(config => config.Replace("^c-(.*)$", "$1"))
                   .Angular(angular => angular.Models(config => config.OutputPath("Output/WithPrefix"))));

            this.Read(read => read
                   .Reflection(reflection => reflection
                       .FromType<IInterface>()
                       .FromType<Interface>()
                       .FromType<MyClassWithInterface>()
                       .FromType<MyClassWithIInterface>()))
               .Write(write => write
                   .SetTestDefaults()
                   .Angular(angular => angular.Models(config => config.OutputPath("Output/WithoutPrefix"))));
        }
    }

    public static class TestExtension
    {
        public static IWriteFluentSyntax SetTestDefaults(this IWriteFluentSyntax syntax)
        {
            return syntax.NoHeader().NoIndex();
        }
    }
}
