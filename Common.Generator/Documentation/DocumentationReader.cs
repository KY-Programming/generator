using System.Reflection;
using System.Xml.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Extension;

namespace KY.Generator.Documentation;

public static class DocumentationReader
{
    private static readonly Dictionary<Assembly, XDocument> cache = new();
    private static readonly NugetAssemblyLocator nugetLocator = new();

    public static string Get(Assembly assembly)
    {
        return string.Empty;
    }

    public static string Get(Type type)
    {
        XElement? typeElement = GetType(type);
        string comment = typeElement?.Element("summary")?.Value.Trim() ?? string.Empty;
        return string.Join(Environment.NewLine, comment.Split('\n').Select(x => x.Trim()));
    }

    public static string Get(MemberInfo member)
    {
        if (member is Type type)
        {
            return Get(type);
        }
        if (member.DeclaringType != null)
        {
            Get(member.DeclaringType);
        }
        return string.Empty;
    }

    public static string Get(ParameterInfo parameter)
    {
        return string.Empty;
    }

    private static IEnumerable<XElement> GetTypes(Assembly assembly)
    {
        if (!cache.TryGetValue(assembly, out XDocument? documentation))
        {
            string? filePath = LocateDocumentation(assembly);
            cache[assembly] = documentation = filePath == null ? new XDocument() : XDocument.Load(filePath);
        }
        return documentation.Root?.Element("members")?.Elements() ?? [];
    }

    /// <summary>
    /// The documentation file only sits next to the assembly for locally built projects. An assembly coming from a
    /// package reference is resolved out of the build output, and the SDK does not copy the package's xml file there,
    /// so the package cache is asked for the same assembly as a fallback. Without it every documentation based option
    /// (e.g. "Generator ignore") would be silently dropped for all types from referenced packages.
    /// </summary>
    private static string? LocateDocumentation(Assembly assembly)
    {
        if (string.IsNullOrEmpty(assembly.Location))
        {
            return null;
        }
        string? filePath = ToDocumentationPath(assembly.Location);
        if (filePath != null && FileSystem.FileExists(filePath))
        {
            return filePath;
        }
        AssemblyLocation? packageLocation = nugetLocator.Locate(AssemblyLocateInfo.From(assembly.GetName()));
        filePath = packageLocation == null ? null : ToDocumentationPath(packageLocation.Path);
        if (filePath != null && FileSystem.FileExists(filePath))
        {
            Logger.Trace($"Documentation of {assembly.GetName().Name} read from package cache ({filePath})");
            return filePath;
        }
        return null;
    }

    private static string? ToDocumentationPath(string assemblyPath)
    {
        string fileName = FileSystem.GetFileName(assemblyPath);
        return fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                   ? FileSystem.Combine(FileSystem.Parent(assemblyPath), fileName.TrimEnd(".dll") + ".xml")
                   : null;
    }

    private static XElement? GetType(Type type)
    {
        return GetTypes(type.Assembly).FirstOrDefault(x => x.Attribute("name")?.Value == $"T:{type.FullName}");
    }
}
