using KY.Generator;
using ServiceFromAspNetCoreViaFluentApi.Controllers;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .FromController<WeatherForecastController>()
                .Write()
                .AngularServices().OutputPath("../Service/ClientApp/src/app/services")
                .AngularModel().OutputPath("../Service/ClientApp/src/app/models");
        }
    }
}