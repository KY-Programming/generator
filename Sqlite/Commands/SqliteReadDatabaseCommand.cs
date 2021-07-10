using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Sqlite.Readers;

namespace KY.Generator.Sqlite.Commands
{
    public class SqliteReadDatabaseCommand : GeneratorCommand<SqliteReadDatabaseCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "sqlite-read" };

        public SqliteReadDatabaseCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            string outputPath = output is FileOutput fileOutput ? fileOutput.BasePath : string.Empty;
            this.resolver.Create<SqliteTableReader>().Read(this.Parameters, this.TransferObjects, outputPath);
            return this.Success();
        }
    }
}
