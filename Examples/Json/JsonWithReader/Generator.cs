using KY.Generator;

namespace JsonWithReader
{
    internal class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .JsonFromFile("Source/complex.json")
                .Write()
                .JsonModel("Output", "Complex", "KY.Generator.Examples.Json")
                .WithReader();

            this.Read()
                .JsonFromFile("Source/simple.json")
                .Write()
                .JsonModel("Output", "Simple", "KY.Generator.Examples.Json");

            this.Read()
                .JsonFromFile("Source/simple.json")
                .Write()
                .AngularModels()
                .OutputPath("Output")
                ;
        }
    }
}