using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Readers;

namespace KY.Generator.Tsql.Commands;

public class TsqlReadCommand : GeneratorCommand<TsqlReadCommandParameters>
{
    private readonly IDependencyResolver resolver;
    public static string[] Names { get; } = [ToCommand(nameof(TsqlReadCommand)), "tsql-read"];

    public TsqlReadCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override IGeneratorCommandResult Run()
    {
        TsqlReadConfiguration configuration = new();
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

        this.resolver.Create<TsqlReader>().Read(configuration);

        return this.Success();
    }
}
