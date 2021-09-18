using KY.Generator;
using WithCustomHttpClient.Controllers;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read
                    .AspDotNet(asp => asp.FromController<WeatherForecastController>()))
                .Write(write => write
                    .Angular(angular => angular.Services(config => config.OutputPath("../Service/ClientApp/src/app/services")
                                                                         .HttpClient("CustomHttpClient", "../base/custom-http-client")
                                                                         .GetMethod("MyGet", options => options.NoHttpOptions())
                                                                         .PostMethod("myPost")
                                                                         .PutMethod("myPut", options => options.ParameterGeneric())
                                                                         .DeleteMethod("myDelete", options => options.NoHttpOptions().NotGeneric()))
                                               .Models(config => config.OutputPath("../Service/ClientApp/src/app/models"))
                    ));
        }
    }
}
