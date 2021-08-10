using KY.Generator;
using WithCustomHttpClient.Controllers;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read()
                .AspDotNet(asp => asp.FromController<WeatherForecastController>())
                .Write()
                .Angular(angular => angular.Services(config => config.OutputPath("../Service/ClientApp/src/app/services")
                                                                     .HttpClient("CustomHttpClient", "../base/custom-http-client")
                                                                     .GetMethod("MyGet", options => options.NoHttpOptions())
                                                                     .PostMethod("myPost")
                                                                     .PutMethod("myPut", options => options.ParameterGeneric())
                                                                     .DeleteMethod("myDelete", options => options.NoHttpOptions().UseParameters().NotGeneric()))
                                           .Models(config => config.OutputPath("../Service/ClientApp/src/app/models"))
                );
        }
    }
}
