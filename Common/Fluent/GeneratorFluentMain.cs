using System.Linq.Expressions;
using System.Reflection;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Syntax;

namespace KY.Generator;

/// <summary>
/// Entry point for fluent language generation.
/// Override the <see cref="Execute"/> method to add some generation actions
/// </summary>
public abstract class GeneratorFluentMain : ISwitchToReadFluentSyntax
{
    public IDependencyResolver Resolver { get; set; }
    public List<IFluentInternalSyntax> Syntaxes { get; } = new();

    /// <inheritdoc cref="ISwitchToReadFluentSyntax.Read"/>
    public ISwitchToWriteFluentSyntax Read(Action<IReadFluentSyntax> action)
    {
        return this.Create().Read(action);
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
    /// <footer>see <a href="https://generator.ky-programming.de/core/documentation/fluent-api#Execute">`.Execute()` on generator.ky-programming.de</a></footer>
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
    /// <footer>see <a href="https://generator.ky-programming.de/core/documentation/fluent-api#ExecuteBeforeBuild">`.ExecuteBeforeBuild()` on generator.ky-programming.de</a></footer>
    public virtual void ExecuteBeforeBuild()
    { }

    /// <inheritdoc cref="IFluentSyntax{TSyntax}.SetGlobal"/>
    public ISwitchToReadFluentSyntax SetGlobal(Assembly assembly, Action<ISetFluentSyntax> action)
    {
        return this.Create().CastTo<ISwitchToReadFluentSyntax>().SetGlobal(assembly, action);
    }

    /// <inheritdoc cref="IFluentSyntax{TSyntax}.SetType{T}"/>
    public ISwitchToReadFluentSyntax SetType<T>(Action<ISetFluentSyntax> action)
    {
        return this.Create().CastTo<ISwitchToReadFluentSyntax>().SetType<T>(action);
    }

    /// <inheritdoc cref="IFluentSyntax{TSyntax}.SetMember{T}(System.Linq.Expressions.Expression{System.Func{T,object}},System.Action{KY.Generator.Syntax.ISetMemberFluentSyntax})"/>
    public ISwitchToReadFluentSyntax SetMember<T>(Expression<Func<T, object>> memberExpression, Action<ISetMemberFluentSyntax> action)
    {
        return this.Create().CastTo<ISwitchToReadFluentSyntax>().SetMember(memberExpression, action);
    }

    /// <inheritdoc cref="IFluentSyntax{TSyntax}.SetMember{T}(System.Linq.Expressions.Expression{System.Func{T,object}},System.Action{KY.Generator.Syntax.ISetMemberFluentSyntax})"/>
    public ISwitchToReadFluentSyntax SetMember<T>(Expression<Action<T>> memberExpression, Action<ISetMemberFluentSyntax> action)
    {
        return this.Create().CastTo<ISwitchToReadFluentSyntax>().SetMember(memberExpression, action);
    }

    /// <inheritdoc cref="IFluentSyntax{TSyntax}.SetMember{T}(string,System.Action{KY.Generator.Syntax.ISetMemberFluentSyntax})"/>
    public ISwitchToReadFluentSyntax SetMember<T>(string name, Action<ISetMemberFluentSyntax> action)
    {
        return this.Create().CastTo<ISwitchToReadFluentSyntax>().SetMember<T>(name, action);
    }

    private FluentSyntax Create()
    {
        IDependencyResolver resolver = this.Resolver.CloneForCommand();
        FluentSyntax syntax = resolver.Create<FluentSyntax>();
        this.Syntaxes.Add(syntax);
        return syntax;
    }
}
