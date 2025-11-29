using System.Reflection;
using System.Text.RegularExpressions;
using KY.Core;
using KY.Core.Extension;

namespace KY.Generator;

public class AssemblyLocateInfo
{
    private static readonly Regex regex = new(@"^(?<name>[^,]+)(,\sVersion=(?<version>[\d.]+))?(,\sCulture=(?<culture>[\w-]+))?(,\sPublicKeyToken=(?<token>\w+))?(,\sContentType=(?<contentType>\w+))?$", RegexOptions.Compiled);

    public string Name { get; set; }
    public Version? Version { get; set; }
    public DotNetVersion? DotNetVersion { get; set; }
    public string? Hint { get; set; }
    public string FullName => $"{this.Name}, Version={this.Version}";

    public AssemblyLocateInfo(string name, Version? version = null, DotNetVersion? dotNetVersion = null, string? hint = null)
    {
        this.Name = name;
        this.Version = version;
        this.DotNetVersion = dotNetVersion;
        this.Hint = hint;
    }

    public static AssemblyLocateInfo From(ResolveEventArgs args)
    {
        // Version version = args.RequestingAssembly.GetName().Version;
        Assembly callingAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        DotNetVersion dotNetVersion = callingAssembly.GetDotNetVersion();
        Match match = regex.Match(args.Name);
        if (match.Success)
        {
            return new AssemblyLocateInfo(match.Groups["name"].Value, new Version(match.Groups["version"].Value), dotNetVersion);
        }
        return new AssemblyLocateInfo(args.Name, null, dotNetVersion);
    }

    public static AssemblyLocateInfo From(AssemblyName assemblyName)
    {
        Assembly callingAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
        DotNetVersion dotNetVersion = callingAssembly.GetDotNetVersion();
        return new AssemblyLocateInfo(assemblyName.Name, assemblyName.Version, dotNetVersion, assemblyName.CodeBase);
    }

    public static AssemblyLocateInfo From(AssemblyMetaData metaData)
    {
        return From(metaData.GetName());
    }

    public override string ToString()
    {
        return $"{this.Name}{(this.Version != null ? $"-{this.Version}" : "")}";
    }
}
