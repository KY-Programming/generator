using HeaderAndLint;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            // NoHeaderVersion applies to the whole write: no file of this chain carries the version of
            // the generator in its header, so updating the generator does not change any of them. Nothing is
            // configured for the linter, so the file comes out with the comment of the language -
            // /* eslint-disable */ for TypeScript.
            this.Read(read => read
                    .Reflection(reflection => reflection.FromType<WeatherForecast>()))
                .Write(write => write
                    .NoHeaderVersion()
                    .TypeScriptModel(model => model.OutputPath("../HeaderAndLint/Output/Models")));

            // The same model for a project that does not run ESLint, written in both languages by one write.
            // SuppressLint replaces the comment of the language - pass an empty one to write none at all.
            // Without a language it would replace the comment of both files, so the language limits it to the
            // TypeScript one and the C# one keeps // ReSharper disable All.
            this.Read(read => read
                    .Reflection(reflection => reflection.FromType<WeatherForecast>()))
                .Write(write => write
                    .NoHeaderVersion()
                    .SuppressLint("// @ts-nocheck", nameof(OutputLanguage.TypeScript))
                    .TypeScriptModel(model => model.OutputPath("../HeaderAndLint/Output/Legacy"))
                    .Reflection(reflection => reflection.Models("../HeaderAndLint/Output/Legacy")));

            // The C# writer brings its own comment - // ReSharper disable All.
            this.Read(read => read
                    .Reflection(reflection => reflection.FromType<ContactDto>()))
                .SetType<ContactDto>(config => config.ReplaceName("Dto", string.Empty))
                .Write(write => write
                    .NoHeaderVersion()
                    .Reflection(reflection => reflection.Models("../HeaderAndLint/Output/Models")));
        }
    }
}
