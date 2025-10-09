using System.Diagnostics;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Models;

namespace KY.Generator.Commands;

internal class ReadProjectCommand(IDependencyResolver resolver) : GeneratorCommand<ReadProjectCommandParameters>, IPrepareCommand
{
    public static string[] Names { get; } = [ToCommand(nameof(ReadProjectCommand)), "read-project", "readProject", "readid"];

    public override IGeneratorCommandResult Run()
    {
        string projectFileName = FileSystem.GetFileName(this.Parameters.Project);
        VisualStudioParser parser = new();
        VisualStudioSolutionProject project = parser.ParseProject(this.Parameters.Project);
        if (project == null || project.Id == Guid.Empty)
        {
            VisualStudioSolution solution = parser.ParseSolution(this.Parameters.Solution) ?? this.FindSolution(parser);
            VisualStudioSolutionProject solutionProject = solution?.Projects.FirstOrDefault(x => x.Path.EndsWith(projectFileName));
            if (project == null)
            {
                project = solutionProject;
            }
            else
            {
                project.Id = solutionProject?.Id ?? Guid.Empty;
            }
        }
        if (project != null && project.Id == Guid.Empty)
        {
            project.Id = Guid.NewGuid();
            Logger.Warning("Project has no id and solution could not be found. A new id was generated and set to project.");
            parser.SetProjectGuid(this.Parameters.Project, project.Id);
        }
        if (project != null && project.Name == null)
        {
            project.Name = projectFileName.Replace(Path.GetExtension(projectFileName), string.Empty);
        }
        if (project == null || project.Id == Guid.Empty)
        {
            Logger.Warning($"Can not read project id. No solution for project '{this.Parameters.Project}' found. Automatic file cleanup deactivated!");
            return this.Success();
        }
        IEnvironment environment = resolver.Get<IEnvironment>();
        environment.OutputId = project.Id;
        environment.Name = project.Name;
        Options.GetGlobal<GeneratorOptions>().Nullable = project.Nullable ?? false;
        AssemblyCache assemblyCache = resolver.Get<AssemblyCache>();
        assemblyCache.LoadLocal(this.Parameters.Project);

        return this.Success().ForceRerunOnAsync();
    }

    private VisualStudioSolution FindSolution(VisualStudioParser parser, int levelToGoUp = 3)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        try
        {
            string projectFileName = FileSystem.GetFileName(this.Parameters.Project);
            string solutionDirectory = this.Parameters.Project;
            while (levelToGoUp > 0)
            {
                solutionDirectory = FileSystem.Parent(solutionDirectory);
                string[] solutionFiles = FileSystem.GetFiles(solutionDirectory, "*.sln");
                foreach (string solutionFile in solutionFiles)
                {
                    VisualStudioSolution solution = parser.ParseSolution(solutionFile);
                    if (solution.Projects.Any(x => x.Path.EndsWith(projectFileName)))
                    {
                        return solution;
                    }
                }
                levelToGoUp--;
            }
        }
        catch (Exception exception)
        {
            Logger.Warning($"Can not find solution for project '{this.Parameters.Project}'. {exception.Message}");
        }
        finally
        {
            stopwatch.Stop();
            Logger.Trace($"Searching for solution in {(stopwatch.ElapsedMilliseconds >= 1 ? stopwatch.ElapsedMilliseconds.ToString() : "<1")} ms");
            Logger.Trace("To skip the previous step, build the solution instead the project or set a <ProjectGuid> in the project file.");
        }
        return null;
    }
}
