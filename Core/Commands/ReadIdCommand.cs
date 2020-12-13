using System.Linq;
using KY.Core;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Commands
{
    internal class ReadIdCommand : GeneratorCommand<ReadIdCommandParameters>
    {
        public override string[] Names { get; }= { "readid" };

        public override IGeneratorCommandResult Run(IOutput output)
        {
            SolutionParser parser = new SolutionParser();
            VisualStudioSolution solution = parser.Parse(this.Parameters.Solution);
            VisualStudioSolutionProject project = solution?.Projects.FirstOrDefault(x => x.Path.EndsWith(this.Parameters.Project));
            if (project == null)
            {
                Logger.Warning($"Can not read project id. No solution for project '{this.Parameters.Project}' found. Automatic file cleanup deactivated!");
                return this.Success();
            }
            this.TransferObjects.Add(new OutputIdTransferObject(project.Id));

            return this.Success();
        }
    }
}