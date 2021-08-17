using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Csharp.Languages;
using KY.Generator.Output;
using KY.Generator.Sqlite.Transfer;
using KY.Generator.Sqlite.Transfer.Readers;
using KY.Generator.Sqlite.Writers;
using KY.Generator.Templates;
using KY.Generator.Transfer;

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

        public override IGeneratorCommandResult Run(IOutput output)
        {
            Type type = GeneratorTypeLoader.Get(this.Parameters.Assembly, this.Parameters.Namespace, this.Parameters.Name);
            if (type == null)
            {
                Logger.Trace($"Class {this.Parameters.Namespace}.{this.Parameters.Name} not found");
                return this.Error();
            }
            this.Parameters.OutputId = this.TransferObjects.OfType<OutputIdTransferObject>().FirstOrDefault()?.Value;
            SqliteModelTransferObject model = this.resolver.Create<SqliteModelReader>().Read(type, this.TransferObjects);
            List<FileTemplate> files = this.resolver.Create<SqliteRepositoryWriter>().Write(model, this.Parameters);
            files.ForEach(file => CsharpLanguage.Instance.Write(file, output));
            return this.Success();
        }
    }
}
