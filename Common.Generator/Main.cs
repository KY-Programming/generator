using System.Diagnostics;

namespace KY.Generator;

public static class Main
{
    public static async Task<bool> Run(string[] args)
    {
#if DEBUG
        if (args.Length > 0 && args[0] != "statistics")
        {
            Debugger.Launch();
        }
#endif
        Generator.InitializeLogger(args);
        try
        {
            return await Generator.Create()
                                  .SetParameters(args)
                                  .Run();
        }
        catch (EngineVersionMismatchException)
        {
            return false;
        }
    }
}
