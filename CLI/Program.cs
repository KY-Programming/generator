using System;
using KY.Core;
using KY.Generator.Angular;
using KY.Generator.AspDotNet;
using KY.Generator.Csharp;
using KY.Generator.Reflection;
using KY.Generator.TypeScript;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Generator.InitializeLogger(args);

            bool success = Generator.Initialize()
                                    .PreloadModule<AngularModule>()
                                    .PreloadModule<AspDotNetModule>()
                                    .PreloadModule<CsharpModule>()
                                    //.PreloadModule<EntityFrameworkModule>()
                                    //.PreloadModule<JsonModule>()
                                    //.PreloadModule<ODataModule>()
                                    //.PreloadModule<OpenApiModule>()
                                    .PreloadModule<ReflectionModule>()
                                    //.PreloadModule<TsqlModule>()
                                    .PreloadModule<TypeScriptModule>()
                                    //.PreloadModule<WatchdogModule>()
                                    .SetParameters(args)
                                    .Run();
            if (!success)
            {
                Environment.ExitCode = 1;
            }

#if DEBUG
            if (Logger.Console.IsConsoleAvailable)
            {
                Console.WriteLine("Press key to EXIT...");
                Console.ReadKey();
            }
#endif
        }
    }
}