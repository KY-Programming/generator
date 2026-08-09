using KY.Generator;

namespace JsonWithReader;

/// <summary>
/// The JSON reader infers a model from the shape of an actual JSON document.
/// Nested objects and object arrays become models of their own, named after the property
/// they came from.
/// </summary>
internal class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // A C# model per object in the document, each with static Load(fileName) and Parse(json)
        // helpers built on Newtonsoft.Json - hence the Newtonsoft.Json package reference.
        this.Read(read => read
                .Json(json => json.FromFile("Source/complex.json")))
            .Write(write => write
                .Json(json => json.Model("Output", "Complex", "KY.Generator.Examples.Json")));

        // The same, but WithoutReader() leaves the Load/Parse helpers out - a plain data class for
        // callers that deserialize themselves.
        this.Read(read => read
                .Json(json => json.FromFile("Source/simple.json")))
            .Write(write => write
                .Json(json => json.Model("Output", "Simple", "KY.Generator.Examples.Json")
                                  .WithoutReader()
                ));

        // The same source read once more, written through a different writer: the model that went to
        // C# above comes out as TypeScript here.
        this.Read(read => read
                .Json(json => json.FromFile("Source/simple.json")))
            .Write(write => write
                .Angular(angular => angular.Models(config => config.OutputPath("Output"))));
    }
}
