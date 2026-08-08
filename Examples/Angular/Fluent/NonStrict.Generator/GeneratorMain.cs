using KY.Generator;
using NonStrict;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            // Strict mode is the default, so nothing has to be configured for it: members that can not be
            // undefined get a default value and nullable members become a union with undefined.
            this.Read(read => read
                    .Reflection(reflection => reflection.FromType<WeatherForecast>()))
                .Write(write => write
                    .Angular(angular => angular.Models(config => config.OutputPath("../NonStrict/Output/Strict"))));

            // The same model, written for a TypeScript project that does not run with "strict": true.
            // NonStrict() applies to the whole write, so every model of this chain opts out.
            this.Read(read => read
                    .Reflection(reflection => reflection.FromType<WeatherForecast>()))
                .Write(write => write
                    .NonStrict()
                    .Angular(angular => angular.Models(config => config.OutputPath("../NonStrict/Output/NonStrict"))));
        }
    }
}
