using KY.Generator;
using NpmPackage.Controllers;

namespace NpmPackage.Generator
{
    public class GeneratorMain : GeneratorFluentMain
    {
        public override void Execute()
        {
            this.Read(read => read.AspDotNet(asp => asp.FromController<WeatherForecastController>()))
                .Write(write => write.Angular(angular => angular.Package(package =>
                    package.Name("@ky/test")
                           .Version("11.0.0-preview.0")
                           .IncrementPatchVersion()
                           .VersionFromNpm()
                           .DependsOn("rxjs", "^6.6.0")
                           .OutputPath("../NpmPackage")
                           .Models(model => model.OutputPath("./models"))
                           .Services(service => service.OutputPath("./services"))
                           .Build()
                           .PublishLocal()
                )));
        }
    }
}
