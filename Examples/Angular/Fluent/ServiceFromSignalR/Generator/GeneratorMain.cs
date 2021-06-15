using KY.Generator;
using ServiceFromSignalRViaFluentApi.Hubs;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .FromHub<WeatherForecastHub>()
                .Write()
                .AngularServices().OutputPath("../Service/ClientApp/src/app/services")
                .AngularModel().OutputPath("../Service/ClientApp/src/app/models");
        }
    }
}