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

    /// <summary>
    /// The command lines collected from <see cref="RunAtSuccessAttribute"/>, run after the generation succeeded
    /// and every file is written.
    /// </summary>
    List<string> RunAtSuccess { get; }

    /// <summary>
    /// The command lines collected from <see cref="RunAtFailureAttribute"/>, run after the generation failed.
    /// </summary>
    List<string> RunAtFailure { get; }
}
