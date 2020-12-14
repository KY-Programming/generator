using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Helpers;
using KY.Generator.Output;
using KY.Generator.Transfer;

namespace KY.Generator.Fluent
{
    internal class FluentCommand : GeneratorCommand<FluentCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; }= { "fluent" };

        public FluentCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            if (string.IsNullOrEmpty(this.Parameters.Assembly))
            {
                Logger.Error("Run from attributes can not be run without assembly parameter");
                return this.Error();
            }
            LocateAssemblyResult result = GeneratorAssemblyLocator.Locate(this.Parameters.Assembly, this.Parameters.IsBeforeBuild);
            if (!result.Success)
            {
                return result;
            }
            bool isAssemblyAsync = result.Assembly.IsAsync();
            if (!this.Parameters.IsOnlyAsync && isAssemblyAsync)
            {
                return this.SwitchAsync();
            }
            CommandRunner commandRunner = this.resolver.Get<CommandRunner>();
            IEnumerable<Type> types = TypeHelper.GetTypes(result.Assembly).Where(type => typeof(GeneratorFluentMain).IsAssignableFrom(type));
            foreach (Type objectType in types)
            {
                GeneratorFluentMain main = (GeneratorFluentMain)Activator.CreateInstance(objectType);
                main.ResolverReference.Resolver = this.resolver;
                main.Execute();
                foreach (List<IGeneratorCommand> commands in main.Syntaxes.Select(x => x.Commands).Where(x => x.Count > 0))
                {
                    List<ITransferObject> transferObjects = this.TransferObjects.ToList();
                    commands.ForEach(x => x.TransferObjects = transferObjects);
                    commandRunner.Run(commands, output);
                }
            }
            return this.Success();
        }
    }
}