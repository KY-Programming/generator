using KY.Generator;
using WebApiFluent.Controllers;

namespace WebApiFluent.Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read.AspDotNet(asp => asp.FromController<WeatherForecastController>()))
                .Write(write => write.Angular(angular => angular.Services(config => config.OutputPath("../WebApiFluent/ClientApp/src/app/services"))
                                                                .Models(config => config.OutputPath("../WebApiFluent/ClientApp/src/app/models"))
                                     )
                                     .Formatter("\"$output../WebApiFluent/ClientApp/node_modules/.bin/prettier\" --write \"$file\"")
                                     .ForceOverwrite()
                );
        }
    }
}
