using KY.Core.Dependency;
using KY.Generator.Languages;
using KY.Generator.Models;
using KY.Generator.TypeScript.Commands;
using KY.Generator.TypeScript.Fluent;
using KY.Generator.TypeScript.Languages;
using KY.Generator.TypeScript.Transfer;
using KY.Generator.TypeScript.Transfer.Readers;
using KY.Generator.TypeScript.Transfer.Writers;

namespace KY.Generator.TypeScript;

public class TypeScriptModule : GeneratorModule
{
    public TypeScriptModule(IDependencyResolver dependencyResolver)
        : base(dependencyResolver)
    {
        this.DependencyResolver.Bind<ILanguage>().To<TypeScriptLanguage>();
        this.DependencyResolver.Bind<TypeScriptModelWriter>().ToSelf();
        this.DependencyResolver.Bind<TypeScriptIndexReader>().ToSelf();
        this.DependencyResolver.Bind<TypeScriptIndexWriter>().ToSelf();
        this.DependencyResolver.Bind<TypeScriptIndexHelper>().ToSelf();
        this.DependencyResolver.Bind<IOptionsFactory>().ToSingleton<TypeScriptOptionsFactory>();
        this.Register<TypeScriptModelCommand>(TypeScriptModelCommandParameters.Names);
        this.Register<ITypeScriptSyntax, TypeScriptSyntax>();
    }
}
