using GenerateInterfacesInsteadClasses;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                          .Reflection(reflection => reflection.FromType<TestModel>()))
                .Write(write => write
                                .NoHeader()
                                .Angular(angular => angular.Models(config => config.OutputPath("Output").PreferInterfaces())));
        }
    }
}
