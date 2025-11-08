using System.Reflection;
using System.Runtime.Loader;

namespace KY.Generator;

internal class Program
{
    private static string SharedPath { get; } = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)), "netstandard2.0");

    private static async Task Main(string[] args)
    {
        bool success = LoadShared("KY.Core.Common")
                       && LoadShared("KY.Generator.Common")
                       && LoadShared("KY.Generator.Common.Generator")
                       && await Run(args);
        if (!success)
        {
            Environment.ExitCode = 1;
        }
    }

    private static bool LoadShared(string assemblyName)
    {
        string coreFileName = Path.Combine(SharedPath, assemblyName + ".dll");
        if (!File.Exists(coreFileName))
        {
            Console.WriteLine($"Error: {assemblyName} not found in {SharedPath}");
            return false;
        }
        AssemblyLoadContext.Default.LoadFromAssemblyPath(coreFileName);
        return true;
    }

    private static async Task<bool> Run(string[] args)
    {
        Assembly core = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.FullName?.StartsWith("KY.Generator.Common.Generator,") ?? false);
        Type? type = core.GetType("KY.Generator.Main");
        if (type == null)
        {
            Console.WriteLine("Error: KY.Generator.Main not found");
            return false;
        }
        MethodInfo? runMethod = type.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
        if (runMethod == null)
        {
            Console.WriteLine("Error: KY.Generator.Main.Run not found");
            return false;
        }
        object[] parameter = new object[1];
        parameter[0] = args;
        return await (Task<bool>)runMethod.Invoke(null, parameter)!;
    }
}
