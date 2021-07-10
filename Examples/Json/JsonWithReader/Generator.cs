using KY.Generator;

namespace JsonWithReader
{
    internal class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .Json(json => json.FromFile("Source/complex.json"))
                .Write()
                .Json(json => json.Model("Output", "Complex", "KY.Generator.Examples.Json"));

            this.Read()
                .Json(json => json.FromFile("Source/simple.json"))
                .Write()
                .Json(json => json.Model("Output", "Simple", "KY.Generator.Examples.Json")
                                  .WithoutReader()
                );

            this.Read()
                .Json(json => json.FromFile("Source/simple.json"))
                .Write()
                .Angular(angular => angular.Models(config => config.OutputPath("Output")));
        }
    }
}