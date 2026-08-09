using System.Reflection;
using KY.Core.DataAccess;
using KY.Generator.Command;
using KY.Generator.Transfer;

namespace KY.Generator.Models;

public class GeneratorEnvironment : IEnvironment
{
    public Guid OutputId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public List<ITransferObject> TransferObjects { get; } = [];
    public static string ApplicationDataPath { get; } = FileSystem.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KY-Programming", "KY-Generator");
    public static string LocalApplicationDataPath { get; } = FileSystem.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KY-Programming", "KY-Generator");
    public string ApplicationData => ApplicationDataPath;
    public string LocalApplicationData => LocalApplicationDataPath;
    public List<CliCommandParameter> Parameters { get; } = [];
    public bool IsBeforeBuild { get; set; }
    public bool IsMsBuild { get; set; }
    public bool Force { get; set; }
    public List<Assembly> LoadedAssemblies { get; } = [];
    public List<string> RunAtSuccess { get; } = [];
}
