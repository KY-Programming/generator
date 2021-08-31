using GenerateInterfacesInsteadClasses;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .FromType<TestModel>()
                .Write()
                .NoHeader()
                .Angular(angular => angular.Models(config => config.OutputPath("Output").PreferInterfaces()));
        }
    }
}
