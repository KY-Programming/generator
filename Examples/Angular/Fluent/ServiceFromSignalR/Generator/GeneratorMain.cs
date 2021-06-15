using KY.Generator;
using ServiceFromSignalRViaFluentApi.Hubs;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .AspDotNet(x => x.FromHub<WeatherForecastHub>())
                .Write()
                .Angular(x => x.Services(config => config.OutputPath("../Service/ClientApp/src/app/services"))
                               .Models(config => config.OutputPath("../Service/ClientApp/src/app/models"))
                );
        }
    }
}