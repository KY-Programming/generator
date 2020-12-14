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
                .AngularModels().OutputPath("Output/Models").SkipHeader()
                .AngularServices().OutputPath("Output/Services").SkipHeader();
        }
    }
}