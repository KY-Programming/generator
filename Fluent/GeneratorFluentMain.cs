using System.Collections.Generic;
using KY.Generator.Helpers;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public abstract class GeneratorFluentMain
    {
        internal DependencyResolverReference ResolverReference { get; } = new DependencyResolverReference();
        internal List<FluentSyntax> Syntaxes { get; } = new List<FluentSyntax>();

        protected IReadFluentSyntax Read()
        {
            FluentSyntax syntax = new FluentSyntax(this.ResolverReference);
            this.Syntaxes.Add(syntax);
            return syntax;
        }

        protected IWriteFluentSyntax Write()
        {
            FluentSyntax syntax = new FluentSyntax(this.ResolverReference);
            this.Syntaxes.Add(syntax);
            return syntax;
        }

        public abstract void Execute();
    }
}