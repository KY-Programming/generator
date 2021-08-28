using Formatting;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .FromType<TwoWhitespaceTest>()
                .Write()
                .Formatting(config => config.UseWhitespaces(2))
                .Angular(angular => angular.Models(config => config.OutputPath("Output").SkipHeader()));

            this.Read()
                .FromType<TabTest>()
                .Write()
                .Formatting(config => config.UseTab())
                .Angular(angular => angular.Models(config => config.OutputPath("Output").SkipHeader()));
        }
    }
}
