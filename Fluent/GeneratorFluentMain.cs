using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Extensions;
using KY.Generator.Syntax;

namespace KY.Generator
{
    public interface IDoReadFluentSyntax : IFluentSyntax<IDoReadFluentSyntax>
    {
        IReadFluentSyntax Read();
    }

    /// <summary>
    /// Entry point for fluent language generation.
    /// Override the <see cref="Execute"/> method to add some generation actions
    /// </summary>
    public abstract class GeneratorFluentMain : IFluentSyntax<IDoReadFluentSyntax>
    {
        public IDependencyResolver Resolver { get; set; }
        public List<IFluentInternalSyntax> Syntaxes { get; } = new();

        /// <summary>
        /// This method does not do anything. Use at least one extension method from one of the other generator packages e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        protected IReadFluentSyntax Read()
        {
            return this.Create();
        }

        /// <summary>
        /// This method does not do anything. Use at least one extension method from one of the other generator packages e.g. <code>KY.Generator.Angular</code> or <code>KY.Generator.Reflection</code>
        /// </summary>
        protected IWriteFluentSyntax Write()
        {
            return this.Create();
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

        /// <summary>
        /// Runs before the assembly is build. Use the <see cref="Read"/> method to add generation actions like this:
        /// <code>this.Read().FromType&lt;Types&gt;().Write().AngularModels().OutputPath("Output/Models").AngularServices().OutputPath("Output/Services");</code>
        /// </summary>
        /// <example>
        /// this.Read()
        ///   .FromType&lt;Types&gt;()
        ///   .Write()
        ///   .AngularModels().OutputPath("Output/Models")
        ///   .AngularServices().OutputPath("Output/Services");
        /// </example>
        public virtual void ExecuteBeforeBuild()
        { }

        /// <inheritdoc />
        public IDoReadFluentSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action)
        {
            return this.Create().CastTo<IDoReadFluentSyntax>().SetGlobal(assembly, action);
        }

        /// <inheritdoc />
        public IDoReadFluentSyntax SetType<T>(Action<ISetFluentSyntax> action)
        {
            return this.Create().CastTo<IDoReadFluentSyntax>().SetType<T>(action);
        }

        /// <inheritdoc />
        public IDoReadFluentSyntax SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetFluentSyntax> action)
        {
            return this.Create().CastTo<IDoReadFluentSyntax>().SetMember(memberExpression, action);
        }

        /// <inheritdoc />
        public IDoReadFluentSyntax SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetFluentSyntax> action)
        {
            return this.Create().CastTo<IDoReadFluentSyntax>().SetMember(memberExpression, action);
        }

        /// <inheritdoc />
        public IDoReadFluentSyntax SetMember<T>(string name, Action<ISetFluentSyntax> action)
        {
            return this.Create().CastTo<IDoReadFluentSyntax>().SetMember<T>(name, action);
        }

        private FluentSyntax Create()
        {
            IDependencyResolver resolver = this.Resolver.CloneForCommands();
            FluentSyntax syntax = resolver.Create<FluentSyntax>();
            this.Syntaxes.Add(syntax);
            return syntax;
        }
    }
}
