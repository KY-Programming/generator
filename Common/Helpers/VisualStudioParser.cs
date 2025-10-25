using System.Text.RegularExpressions;
using System.Xml.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Xml;

namespace KY.Generator;

public class VisualStudioParser
{
    private static readonly Regex projectRegex = new(@"^Project\(""(?<typeId>{[a-fA-F0-9-]+})""\)\s*=\s*""(?<name>[^""]+)""\s*,\s*""(?<path>[^""]*)""\s*,\s*""(?<id>{[a-fA-F0-9-]+})""\s*$");

    public VisualStudioSolution? ParseSolution(string path)
    {
        if (!FileSystem.FileExists(path))
        {
            return null;
        }
        VisualStudioSolution solution = new();
        string[] lines = FileSystem.ReadAllLines(path);
        foreach (string line in lines)
        {
            Match match = projectRegex.Match(line);
            if (!match.Success)
            {
                continue;
            }
            solution.Projects.Add(new VisualStudioSolutionProject
                {
                    Id = new Guid(match.Groups["id"].Value),
                    Name = match.Groups["name"].Value,
                    Path = match.Groups["path"].Value,
                    TypeId = new Guid(match.Groups["typeId"].Value)
                }
            );
        }
        return solution;
    }

    public VisualStudioSolutionProject? ParseProject(string path)
    {
        if (!FileSystem.FileExists(path))
        {
            return null;
        }

        VisualStudioSolutionProject project = new();

        XElement element = FileSystem.ReadXml(path);
        foreach (XElement propertyGroup in element.GetElementsIgnoreNamespace("PropertyGroup"))
        {
            try
            {
                XElement projectGuid = propertyGroup.GetElementIgnoreNamespace("ProjectGuid");
                if (projectGuid != null)
                {
                    project.Id = new Guid(projectGuid.Value);
                }
                XElement nullable = propertyGroup.GetElementIgnoreNamespace("Nullable");
                if (nullable?.Value == "enable")
                {
                    project.Nullable = true;
                }
                else if (nullable?.Value == "disable")
                {
                    project.Nullable = false;
                }
            }
            catch (Exception exception)
            {
                Logger.Warning($"Can not read from .csproj. {exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }
        return project;
    }

    public void SetProjectGuid(string path, Guid id)
    {
        if (!FileSystem.FileExists(path))
        {
            return;
        }

        string content = FileSystem.ReadAllText(path);
        if (content.Contains("</PropertyGroup>"))
        {
            int index = content.IndexOf("</PropertyGroup>", StringComparison.CurrentCultureIgnoreCase);
            content = content.Insert(index, $"  <ProjectGuid>{id:B}</ProjectGuid>{Environment.NewLine}  ");
        }
        else
        {
            int index = content.IndexOf("</Project>", StringComparison.CurrentCultureIgnoreCase);
            content = content.Insert(index, $"<PropertyGroup>{Environment.NewLine}    <ProjectGuid>{id:B}</ProjectGuid>{Environment.NewLine}  </PropertyGroup>{Environment.NewLine}  ");
        }
        FileSystem.WriteAllText(path, content);
    }
}

public class VisualStudioSolution
{
    public List<VisualStudioSolutionProject> Projects { get; } = new();
}

public class VisualStudioSolutionProject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public Guid TypeId { get; set; }
    public bool? Nullable { get; set; }
}
