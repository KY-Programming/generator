using System;
using System.Linq;
using KY.Generator.AspDotNet;
//using KY.Generator.OData;
using KY.Generator.Reflection;
//using KY.Generator.Tsql;

namespace KY.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            bool success = Generator.Initialize()
                                    .PreloadModule<AspDotNetModule>()
                                    .PreloadModule<CsharpModule>()
                                    .PreloadModule<TypeScriptModule>()
                                    //.PreloadModule<TsqlModule>()
                                    //.PreloadModule<ODataModule>()
                                    .PreloadModule<ReflectionModule>()
                                    .SetOutput(args.Skip(1).FirstOrDefault())
                                    .ReadConfiguration(args.FirstOrDefault())
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