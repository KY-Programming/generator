using System;
using System.Collections.Generic;
using KY.Core.Dependency;
using KY.Generator.Angular.Commands;
using KY.Generator.Angular.Models;
using KY.Generator.Command;
using KY.Generator.Syntax;

namespace KY.Generator.Angular.Fluent
{
    internal class AngularPackageSyntax : IAngularPackageSyntax, IExecutableSyntax
    {
        private readonly IDependencyResolver resolver;
        private readonly AngularPackageCommand command;

        public List<IGeneratorCommand> Commands => this.command.Commands;

        public AngularPackageSyntax(IDependencyResolver resolver, AngularPackageCommand command)
        {
            this.resolver = resolver;
            this.command = command;
        }

        public IAngularPackageSyntax Name(string packageName)
        {
            this.command.Parameters.Name = packageName;
            return this;
        }

        public IAngularPackageSyntax Version(string version)
        {
            this.command.Parameters.Version = version;
            return this;
        }

        public IAngularPackageSyntax VersionFromNpm()
        {
            this.command.Parameters.VersionFromNpm = true;
            return this;
        }

        public IAngularPackageSyntax IncrementMajorVersion()
        {
            this.command.Parameters.IncrementVersion = IncrementVersion.Major;
            return this;
        }

        public IAngularPackageSyntax IncrementMinorVersion()
        {
            this.command.Parameters.IncrementVersion = IncrementVersion.Minor;
            return this;
        }

        public IAngularPackageSyntax IncrementPatchVersion()
        {
            this.command.Parameters.IncrementVersion = IncrementVersion.Patch;
            return this;
        }

        public IAngularPackageSyntax CliVersion(string version)
        {
            this.command.Parameters.CliVersion = version;
            return this;
        }

        public IAngularPackageSyntax DependsOn(string packageName, string version)
        {
            this.command.Parameters.DependsOn.Add(new AngularPackageDependsOnParameter(packageName, version));
            return this;
        }

        public IAngularPackageSyntax OutputPath(string path)
        {
            this.command.Parameters.RelativePath = path;
            return this;
        }

        public IAngularPackageSyntax Build()
        {
            this.command.Parameters.Build = true;
            return this;
        }

        public IAngularPackageSyntax Publish()
        {
            this.command.Parameters.Publish = true;
            return this;
        }

        public IAngularPackageSyntax PublishLocal()
        {
            this.command.Parameters.PublishLocal = true;
            return this;
        }

        public IAngularPackageSyntax Models(Action<IAngularModelSyntax> action = null)
        {
            AngularModelCommand modelCommand = this.resolver.Create<AngularModelCommand>();
            this.Commands.Add(modelCommand);
            action?.Invoke(new AngularModelSyntax(this, modelCommand));
            return this;
        }

        public IAngularPackageSyntax Services(Action<IAngularServiceSyntax> action = null)
        {
            AngularServiceCommand serviceCommand = this.resolver.Create<AngularServiceCommand>();
            this.Commands.Add(serviceCommand);
            action?.Invoke(new AngularServiceSyntax(this, serviceCommand));
            return this;
        }
    }
}
