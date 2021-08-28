using System;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Sqlite.Transfer;
using KY.Generator.Sqlite.Transfer.Readers;
using KY.Generator.Sqlite.Writers;

namespace KY.Generator.Sqlite.Commands
{
    public class SqliteWriteRepositoryCommand : GeneratorCommand<SqliteWriteRepositoryCommandParameters>
    {
        private readonly IDependencyResolver resolver;

        public override string[] Names { get; } = { "sqlite-repository" };

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
            IOptions options = this.resolver.Get<Options>().Current;
            options.Language = this.resolver.Get<CsharpLanguage>();
            SqliteModelTransferObject model = this.resolver.Create<SqliteModelReader>().Read(type);
            this.resolver.Create<SqliteRepositoryWriter>().Write(model, this.Parameters);
            return this.Success();
        }
    }
}
