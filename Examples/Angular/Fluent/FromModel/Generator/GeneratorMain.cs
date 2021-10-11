using FromModel;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                .Reflection(reflection => reflection.FromType<TypeToRead>())
            )
            .Write(write => write
                .Angular(angular => angular.Models(config => config.OutputPath("Output/Models")))
            );
        }
    }
}
