using ChangeReturnType.Controllers;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read.AspDotNet(asp => asp.FromController<WeatherForecastController>()))
                .SetMember<WeatherForecastController>(x => x.Get(), config => config.ReturnType("CustomWeatherForecast", "", "../models/custom-weather-forecast"))
                .Write(write => write.Angular(angular => angular.Services(config => config.OutputPath("../Assembly/ClientApp/src/app/services"))
                                                                .Models(config => config.OutputPath("../Assembly/ClientApp/src/app/models"))));
        }
    }
}
