using System.Reflection;
using KY.Generator.Command;
using KY.Generator.Transfer;

namespace KY.Generator.Models;

public interface IEnvironment
{
    Guid OutputId { get; set; }
    string Name { get; set; }
    List<ITransferObject> TransferObjects { get; }
    string OutputPath { get; set; }
    string ApplicationData { get; }
    string LocalApplicationData { get; }
    List<CliCommandParameter> Parameters { get; }
    bool IsBeforeBuild { get; set; }
    bool IsMsBuild { get; set; }
    bool Force { get; set; }
    List<Assembly> LoadedAssemblies { get; }
}
