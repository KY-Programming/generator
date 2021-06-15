using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Output;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Readers;

namespace KY.Generator.Tsql.Commands
{
    public class TsqlReadCommand : GeneratorCommand<TsqlReadCommandParameters>
    {
        private readonly IDependencyResolver resolver;
        public override string[] Names { get; } = { "tsql-read" };

        public TsqlReadCommand(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public override IGeneratorCommandResult Run(IOutput output)
        {
            TsqlReadConfiguration configuration = new TsqlReadConfiguration();
            configuration.Connection = this.Parameters.ConnectionString;
            if (!string.IsNullOrEmpty(this.Parameters.Table))
            {
                configuration.Entities.Add(new TsqlReadEntity
                                           {
                                               Schema = this.Parameters.Schema,
                                               Table = this.Parameters.Table,
                                               Namespace = this.Parameters.Namespace,
                                               Name = this.Parameters.Name
                                           });
            }
            if (!string.IsNullOrEmpty(this.Parameters.StoredProcedure))
            {
                configuration.StoredProcedures.Add(new TsqlReadStoredProcedure
                                                   {
                                                       Schema = this.Parameters.Schema,
                                                       Name = this.Parameters.StoredProcedure
                                                   });
            }

            this.resolver.Create<TsqlReader>().Read(configuration, this.TransferObjects);

            return this.Success();
        }
    }
}