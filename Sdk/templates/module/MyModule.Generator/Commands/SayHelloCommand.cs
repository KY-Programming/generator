using KY.Core.Dependency;
using KY.Generator;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;
using MyModule.Commands;
using MyModule.Generator.Writers;

namespace MyModule.Generator.Commands;

/// <summary>
/// A command runs in two steps. Prepare() sets up the options every writer of this run sees - most
/// importantly the output language. Run() does the work.
/// <para>
/// Note that nothing here touches the file system: writers add <c>FileTemplate</c>s to a shared list and
/// the engine writes them all at the end of the run. That is what makes a generation run atomic.
/// </para>
/// </summary>
internal class SayHelloCommand(IDependencyResolver resolver) : GeneratorCommand<SayHelloCommandParameters>
{
    public override void Prepare()
    {
        GeneratorOptions options = resolver.Get<Options>().Get<GeneratorOptions>();
        options.SetFromParameter(this.Parameters);
        options.Language = resolver.Get<CsharpLanguage>();
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        resolver.Create<HelloWriter>().Write(this.Parameters);
        return this.SuccessAsync();
    }
}
