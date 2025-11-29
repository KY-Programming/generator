using KY.Core;

namespace KY.Generator;

public class AssemblyLocation
{
    public string Path { get; }
    public SemanticVersion Version { get; }
    public DotNetVersion DotNetVersion { get; set; }

    public AssemblyLocation(string path, SemanticVersion version, DotNetVersion dotNetVersion)
    {
        this.Path = path;
        this.Version = version;
        this.DotNetVersion = dotNetVersion;
    }
}
