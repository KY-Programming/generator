using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Sqlite.Readers;

namespace KY.Generator.Sqlite.Commands;

internal class SqliteReadDatabaseCommand : GeneratorCommand<SqliteReadDatabaseCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public SqliteReadDatabaseCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        string outputPath = this.resolver.Get<IOutput>() is FileOutput fileOutput ? fileOutput.BasePath : string.Empty;
        this.resolver.Create<SqliteTableReader>().Read(this.Parameters, outputPath);
        return this.SuccessAsync();
    }
}
