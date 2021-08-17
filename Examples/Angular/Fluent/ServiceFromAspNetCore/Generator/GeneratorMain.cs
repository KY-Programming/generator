using KY.Generator;
using ServiceFromAspNetCoreViaFluentApi;
using ServiceFromAspNetCoreViaFluentApi.Controllers;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .AspDotNet(asp => asp.FromController<WeatherForecastController>())
                .Write()
                .Angular(angular => angular.Services(config => config.OutputPath("../Service/ClientApp/src/app/services"))
                                           .Models(config => config.OutputPath("../Service/ClientApp/src/app/models"))
                );
        }
    }
}
