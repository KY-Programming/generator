﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Extensions;
using KY.Generator.Output;

namespace KY.Generator.Reflection.Commands
{
    public class AttributesCommand : GeneratorCommand<RunByAttributeCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "attributes" };

        public AttributesCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            CommandRunner commandRunner = this.resolver.Get<CommandRunner>();
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
            foreach (Type objectType in TypeHelper.GetTypes(result.Assembly))
            {
                List<Attribute> attributes = objectType.GetCustomAttributes().ToList();
                List<RawCommand> commands = new List<RawCommand>();
                foreach (IGeneratorCommandAttribute attribute in attributes.OfType<IGeneratorCommandAttribute>())
                {
                    commands.AddRange(attribute.Commands.Select(x =>
                    {
                        RawCommand command = new RawCommand(x.Command);
                        command.Parameters.AddRange(x.Parameters.Select(RawCommandParameter.Parse));
                        command.Parameters.Add(new RawCommandParameter("IsAsyncAssembly", isAssemblyAsync.ToString()));
                        foreach (RawCommandParameter parameter in command.Parameters)
                        {
                            parameter.Value = parameter.Value.Replace("$ASSEMBLY$", this.Parameters.Assembly)
                                                       .Replace("$NAMESPACE$", objectType.Namespace)
                                                       .Replace("$NAME$", objectType.FullName.TrimStart(objectType.Namespace + "."));
                        }
                        return command;
                    }));
                    foreach (IGeneratorCommandAdditionalParameterAttribute additionalParameterAttribute in attributes.OfType<IGeneratorCommandAdditionalParameterAttribute>())
                    {
                        foreach (AttributeCommandConfiguration additionalParameters in additionalParameterAttribute.Commands)
                        {
                            foreach (RawCommand command in commands.Where(x => x.Name == additionalParameters.Command || additionalParameters.Command == "*"))
                            {
                                command.Parameters.AddRange(additionalParameters.Parameters.Select(RawCommandParameter.Parse));
                            }
                        }
                    }
                    if (commands.Count > 0)
                    {
                        commandRunner.Run(commands, output, this.TransferObjects);
                        this.TransferObjects.Clear();
                    }
                }
            }
            return this.Success();
        }
    }
}