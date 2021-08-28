using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Helpers;
using KY.Generator.Output;
using KY.Generator.Syntax;
using KY.Generator.Templates;
using KY.Generator.Transfer;
using Environment = KY.Generator.Models.Environment;

namespace KY.Generator.Fluent
{
    internal class FluentCommand : GeneratorCommand<FluentCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "fluent" };

        public FluentCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run()
        {
            if (string.IsNullOrEmpty(this.Parameters.Assembly))
            {
                Logger.Error("Run from attributes can not be run without assembly parameter");
                return this.Error();
            }
            LocateAssemblyResult result = GeneratorAssemblyLocator.Locate(this.Parameters.Assembly, this.Parameters.IsBeforeBuild);
            if (result.SwitchContext)
            {
                return result;
            }
            if (this.Parameters.IsBeforeBuild && !result.Success)
            {
                return this.Success();
            }
            bool isAssemblyAsync = result.Assembly.IsAsync();
            if (!this.Parameters.IsOnlyAsync && isAssemblyAsync)
            {
                return this.SwitchAsync();
            }
            IEnumerable<Type> types = TypeHelper.GetTypes(result.Assembly).Where(type => typeof(GeneratorFluentMain).IsAssignableFrom(type));
            foreach (Type objectType in types)
            {
                GeneratorFluentMain main = (GeneratorFluentMain)this.resolver.Create(objectType);
                main.Resolver = this.resolver;
                if (this.Parameters.IsBeforeBuild)
                {
                    main.ExecuteBeforeBuild();
                }
                else
                {
                    main.Execute();
                }
                List<ITransferObject> transferObjects = this.resolver.Get<Environment>().TransferObjects;
                foreach (IFluentInternalSyntax syntax in main.Syntaxes)
                {
                    IGeneratorCommandResult commandResult = syntax.Run();
                    if (!commandResult.Success)
                    {
                        return commandResult;
                    }
                    transferObjects.AddIfNotExists(syntax.Resolver.Get<List<ITransferObject>>());
                }
            }
            return this.Success();
        }
    }
}
