using System.Reflection;
using System.Xml.Linq;
using KY.Core.DataAccess;

namespace KY.Generator.Documentation;

public static class DocumentationReader
{
    private static readonly Dictionary<Assembly, XDocument> cache = new();

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
            string filePath = assembly.Location.Replace(".dll", ".xml");
            if (FileSystem.FileExists(filePath))
            {
                cache[assembly] = documentation = XDocument.Load(filePath);
            }
            else
            {
                cache[assembly] = documentation = new XDocument();
            }
        }
        return documentation.Root?.Element("members")?.Elements() ?? [];
    }

    private static XElement? GetType(Type type)
    {
        return GetTypes(type.Assembly).FirstOrDefault(x => x.Attribute("name")?.Value == $"T:{type.FullName}");
    }
}
