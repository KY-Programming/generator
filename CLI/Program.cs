using System;
using KY.Core;
using KY.Generator.Angular;
using KY.Generator.AspDotNet;
using KY.Generator.Csharp;
using KY.Generator.Json;
using KY.Generator.Reflection;
using KY.Generator.TypeScript;
using KY.Generator.Watchdog;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Logger.AllTargets.Add(Logger.VisualStudioOutput);

            bool success = Generator.Initialize()
                                    .PreloadModule<AspDotNetModule>()
                                    .PreloadModule<CsharpModule>()
                                    .PreloadModule<TypeScriptModule>()
                                    //.PreloadModule<TsqlModule>()
                                    //.PreloadModule<ODataModule>()
                                    .PreloadModule<ReflectionModule>()
                                    .PreloadModule<AngularModule>()
                                    .PreloadModule<JsonModule>()
                                    .PreloadModule<WatchdogModule>()
                                    .SetParameters(args)
                                    .Run();
            if (!success)
            {
                Environment.ExitCode = 1;
            }

#if DEBUG
            Console.WriteLine("Press key to EXIT...");
            Console.ReadKey();
#endif
        }
    }
}