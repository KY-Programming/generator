﻿using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.DataAccess;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Models;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Commands
{
    internal class ReadIdCommand : GeneratorCommand<ReadIdCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; }= { "readid" };

        public ReadIdCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run()
        {
            string projectFileName = FileSystem.GetFileName(this.Parameters.Project);
            VisualStudioParser parser = new VisualStudioParser();
            VisualStudioSolutionProject project = parser.ParseProject(this.Parameters.Project);
            if (project == null || project.Id == Guid.Empty)
            {
                VisualStudioSolution solution = parser.ParseSolution(this.Parameters.Solution);
                project = solution?.Projects.FirstOrDefault(x => x.Path.EndsWith(projectFileName)) ?? project;
            }
            if (project != null && project.Id == Guid.Empty)
            {
                project.Id = Guid.NewGuid();
                parser.SetProjectGuid(this.Parameters.Project, project.Id);
            }

            if (project == null || project.Id == Guid.Empty)
            {
                Logger.Warning($"Can not read project id. No solution for project '{this.Parameters.Project}' found. Automatic file cleanup deactivated!");
                return this.Success();
            }
            this.resolver.Get<IEnvironment>().OutputId = project.Id;

            return this.Success().ForceRerunOnAsync();
        }
    }
}
