using System;
using KY.Core;
using KY.Generator.Angular;
using KY.Generator.AspDotNet;
using KY.Generator.Csharp;
using KY.Generator.EntityFramework;
using KY.Generator.Json;
using KY.Generator.OData;
using KY.Generator.OpenApi;
using KY.Generator.Reflection;
using KY.Generator.Sqlite;
using KY.Generator.Tsql;
using KY.Generator.TypeScript;
using KY.Generator.Watchdog;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Generator.InitializeLogger(args);

            bool success = Generator.Create()
                                    .PreloadModule<AngularModule>()
                                    .PreloadModule<AspDotNetModule>()
                                    .PreloadModule<CsharpModule>()
                                    .PreloadModule<EntityFrameworkModule>()
                                    .PreloadModule<JsonModule>()
                                    .PreloadModule<ODataModule>()
                                    .PreloadModule<OpenApiModule>()
                                    .PreloadModule<ReflectionModule>()
                                    .PreloadModule<SqliteModule>()
                                    .PreloadModule<TsqlModule>()
                                    .PreloadModule<TypeScriptModule>()
                                    .PreloadModule<WatchdogModule>()
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
