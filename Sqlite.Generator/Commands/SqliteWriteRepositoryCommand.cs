using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Sqlite.Transfer;
using KY.Generator.Sqlite.Transfer.Readers;
using KY.Generator.Sqlite.Writers;

namespace KY.Generator.Sqlite.Commands;

internal class SqliteWriteRepositoryCommand : GeneratorCommand<SqliteWriteRepositoryCommandParameters>
{
    private readonly IDependencyResolver resolver;
    private readonly GeneratorTypeLoader typeLoader;

    public SqliteWriteRepositoryCommand(IDependencyResolver resolver, GeneratorTypeLoader typeLoader)
    {
        this.resolver = resolver;
        this.typeLoader = typeLoader;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        Type? type = this.typeLoader.Get(this.Parameters.Namespace, this.Parameters.Name);
        if (type == null)
        {
            Logger.Trace($"Class {this.Parameters.Namespace}.{this.Parameters.Name} not found");
            return this.ErrorAsync();
        }
        GeneratorOptions options = this.resolver.Get<Options>().Get<GeneratorOptions>();
        options.Language = this.resolver.Get<CsharpLanguage>();
        SqliteModelTransferObject model = this.resolver.Create<SqliteModelReader>().Read(type);
        this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        this.resolver.Create<SqliteRepositoryWriter>().Write(model, this.Parameters);
        return this.SuccessAsync();
    }
}
