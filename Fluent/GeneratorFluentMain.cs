using System.Collections.Generic;
using KY.Generator.Helpers;
using KY.Generator.Syntax;

namespace KY.Generator
{
    /// <summary>
    /// Entry point for fluent language generation.
    /// Override the <see cref="Execute"/> method to add some generation actions
    /// </summary>
    public abstract class GeneratorFluentMain
    {
        public DependencyResolverReference ResolverReference { get; } = new DependencyResolverReference();
        public List<IFluentSyntax> Syntaxes { get; } = new List<IFluentSyntax>();

        /// <summary>
        /// This method does not do anything. Use at least one extension method from one of the other generator packages e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        protected IReadFluentSyntax Read()
        {
            FluentSyntax syntax = new FluentSyntax(this.ResolverReference);
            this.Syntaxes.Add(syntax);
            return syntax;
        }

        /// <summary>
        /// This method does not do anything. Use at least one extension method from one of the other generator packages e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        protected IWriteFluentSyntax Write()
        {
            FluentSyntax syntax = new FluentSyntax(this.ResolverReference);
            this.Syntaxes.Add(syntax);
            return syntax;
        }

        /// <summary>
        /// Use the <see cref="Read"/> method to add generation actions like this:
        /// <code>this.Read().FromType&lt;Types&gt;().Write().AngularModels().OutputPath("Output/Models").AngularServices().OutputPath("Output/Services");</code>
        /// </summary>
        /// <example>
        /// this.Read()
        ///   .FromType&lt;Types&gt;()
        ///   .Write()
        ///   .AngularModels().OutputPath("Output/Models")
        ///   .AngularServices().OutputPath("Output/Services");
        /// </example>
        public abstract void Execute();
    }
}