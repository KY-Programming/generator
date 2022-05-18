using ChangeReturnType;
using ChangeReturnType.Controllers;
using KY.Generator;

namespace Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read.AspDotNet(asp => asp.FromController<WeatherForecastController>())
                                  .Reflection(reflection => reflection.FromType<WeatherForecast>())
                )
                .SetMember<WeatherForecastController>(x => x.Get(),
                    config => config.ReturnType("CustomWeatherForecast[]").ImportFile("custom-weather-forecast", "CustomWeatherForecast")
                )
                .Write(write => write.Angular(angular => angular.Services(config => config.OutputPath("../Assembly/ClientApp/src/app/services"))
                                                                .Models(config => config.OutputPath("../Assembly/ClientApp/src/app/models"))));
        }
    }
}
