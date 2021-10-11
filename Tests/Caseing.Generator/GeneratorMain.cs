using KY.Generator;

namespace Caseing.Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read.Reflection(reflection => reflection.FromType<MixedCaseing>()))
                .SetType<KeepMyCase>(config => config.FormatNames(false))
                .Write(write => write.Angular(angular => angular.Models(config => config.OutputPath("Output"))));
        }
    }
}
