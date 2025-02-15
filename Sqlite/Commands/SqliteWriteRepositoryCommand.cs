using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Sqlite.Transfer;
using KY.Generator.Sqlite.Transfer.Readers;
using KY.Generator.Sqlite.Writers;

namespace KY.Generator.Sqlite.Commands;

public class SqliteWriteRepositoryCommand : GeneratorCommand<SqliteWriteRepositoryCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public static string[] Names { get; } = [ToCommand(nameof(SqliteWriteRepositoryCommand)), "sqlite-repository"];

    public SqliteWriteRepositoryCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        Type type = GeneratorTypeLoader.Get(this.Parameters.Assembly, this.Parameters.Namespace, this.Parameters.Name);
        if (type == null)
        {
            Logger.Trace($"Class {this.Parameters.Namespace}.{this.Parameters.Name} not found");
            return this.Error();
        }
        GeneratorOptions options = this.resolver.Get<Options>().Get<GeneratorOptions>();
        options.Language = this.resolver.Get<CsharpLanguage>();
        SqliteModelTransferObject model = this.resolver.Create<SqliteModelReader>().Read(type);
        this.resolver.Get<IOutput>().DeleteAllRelatedFiles(this.Parameters.RelativePath);
        this.resolver.Create<SqliteRepositoryWriter>().Write(model, this.Parameters);
        return this.Success();
    }
}
