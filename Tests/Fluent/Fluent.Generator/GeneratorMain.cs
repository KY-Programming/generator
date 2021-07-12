using KY.Generator;

namespace Fluent.Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .FromType<Types.Types>()
                .Write()
                .Angular(angular => angular.Models(config => config.OutputPath("Output/Models").SkipHeader())
                                           .Services(config => config.OutputPath("Output/Services").SkipHeader())
                );
        }
    }
}
