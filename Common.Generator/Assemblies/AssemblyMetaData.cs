using System.Reflection;
using System.Runtime.InteropServices;
using KY.Core.DataAccess;

namespace KY.Generator;

public class AssemblyMetaData
{
    private static string[]? frameworkFiles;
    private readonly Assembly metaData;

    private AssemblyMetaData(Assembly metaData)
    {
        this.metaData = metaData;
    }

    public AssemblyName GetName()
    {
        return this.metaData.GetName();
    }

    public IList<CustomAttributeData> GetCustomAttributesData()
    {
        return this.metaData.GetCustomAttributesData();
    }

    public static AssemblyMetaData From(string path)
    {
        IEnumerable<string> loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                                        .Where(x => !x.IsDynamic)
                                                        .Select(x => x.Location)
                                                        .Where(x => !string.IsNullOrEmpty(x));
        PathAssemblyResolver resolver = new(loadedAssemblies.Concat(GetFrameworkFiles()));
        /*using */
        MetadataLoadContext metadataLoadContext = new(resolver);
        return new AssemblyMetaData(metadataLoadContext.LoadFromAssemblyPath(path));
    }

    public static AssemblyMetaData From(Assembly assembly)
    {
        return new AssemblyMetaData(assembly);
    }

    private static string[] GetFrameworkFiles()
    {
        return frameworkFiles ??= FileSystem.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
    }
}
