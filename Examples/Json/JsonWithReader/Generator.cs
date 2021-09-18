using KY.Generator;

namespace JsonWithReader
{
    internal class Generator : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                    .Json(json => json.FromFile("Source/complex.json")))
                .Write(write => write
                    .Json(json => json.Model("Output", "Complex", "KY.Generator.Examples.Json")));

            this.Read(read => read
                    .Json(json => json.FromFile("Source/simple.json")))
                .Write(write => write
                    .Json(json => json.Model("Output", "Simple", "KY.Generator.Examples.Json")
                                      .WithoutReader()
                    ));

            this.Read(read => read
                    .Json(json => json.FromFile("Source/simple.json")))
                .Write(write => write
                    .Angular(angular => angular.Models(config => config.OutputPath("Output"))));
        }
    }
}
