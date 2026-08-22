using KY.Generator;

namespace HeaderAndLint.Source;

// A project that does not run ESLint needs a different comment. The one given here replaces the comment
// of the language - pass an empty one to write none at all. The model is written in both languages, and
// the given language limits the comment to the TypeScript file: the C# one keeps the comment of its own
// language. Leave the language away to reach every language the model is written in.
[GenerateTypeScriptModel]
[GenerateCsharpModel("Output/Models")]
[GenerateSuppressLint("// @ts-nocheck", nameof(OutputLanguage.TypeScript))]
public class LegacyForecast
{
    public string Station { get; set; } = string.Empty;
    public double Temperature { get; set; }
}
