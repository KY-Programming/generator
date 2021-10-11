using KY.Generator;
using ServiceFromAspNetCoreViaFluentApi.Controllers;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                .AspDotNet(asp => asp.FromController<WeatherForecastController>())
            )
            .Write(write => write
                .Angular(angular => angular
                    .Services(config => config.OutputPath("../Service/ClientApp/src/app/services"))
                    .Models(config => config.OutputPath("../Service/ClientApp/src/app/models"))
                )
            );
        }
    }
}
