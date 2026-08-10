using KY.Generator;
using AspDotNetFluent.Controllers;

namespace AspDotNetFluent;

public class Generator : GeneratorFluentMain
{
    public override void Execute()
    {
        // A controller read from the fluent API instead of from an annotation, with two writer options
        // only this project covers switched on: every written file is piped through the prettier of the
        // client app, and ForceOverwrite rewrites files that are already there and unchanged.
        this.Read(read => read.AspDotNet(asp => asp.FromController<WeatherForecastController>()))
            .Write(write => write.NoHeader()
                                 .NoIndex()
                                 .Angular(angular => angular.Services(config => config.OutputPath("ClientApp/src/app/services"))
                                                            .Models(config => config.OutputPath("ClientApp/src/app/models")))
                                 .Formatter("\"$outputClientApp/node_modules/.bin/prettier\" --write \"$file\"")
                                 .ForceOverwrite());
    }
}
