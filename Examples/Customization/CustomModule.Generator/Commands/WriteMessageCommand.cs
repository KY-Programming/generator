using CustomModule.Commands;
using CustomModule.Generator.Writers;
using KY.Core.Dependency;
using KY.Generator;
using KY.Generator.Command;
using KY.Generator.Command.Extensions;
using KY.Generator.Csharp.Languages;

namespace CustomModule.Generator.Commands;

/// <summary>
/// A command runs in two steps. Prepare() sets up the options every writer of this run sees - above all
/// the output language. Run() does the work.
/// <para>
/// Nothing here touches the file system: writers add <c>FileTemplate</c>s to a shared list and the engine
/// writes them all at the end of the run, which is what makes a generation run atomic.
/// </para>
/// </summary>
internal class WriteMessageCommand(IDependencyResolver resolver) : GeneratorCommand<WriteMessageCommandParameters>
{
    public override void Prepare()
    {
        GeneratorOptions options = resolver.Get<Options>().Get<GeneratorOptions>();
        options.SetFromParameter(this.Parameters);
        options.Language = resolver.Get<CsharpLanguage>();
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        resolver.Create<MessageWriter>().Write(this.Parameters);
        return this.SuccessAsync();
    }
}
