using KY.Core.Dependency;
using KY.Generator.Command;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Loaders;
using KY.Generator.Tsql.Readers;

namespace KY.Generator.Tsql.Commands;

internal class TsqlReadCommand : GeneratorCommand<TsqlReadCommandParameters>
{
    private readonly IDependencyResolver resolver;

    public TsqlReadCommand(IDependencyResolver resolver)
    {
        this.resolver = resolver;
    }

    public override Task<IGeneratorCommandResult> Run()
    {
        // Before anything that references SqlClient is touched - see SqlClientLoader
        this.resolver.Create<SqlClientLoader>().Load();

        TsqlReadConfiguration configuration = new()
                                              {
                                                  Connection = this.Parameters.ConnectionString,
                                                  Schema = this.Parameters.Schema,
                                                  Namespace = this.Parameters.Namespace,
                                                  ReadAll = this.Parameters.ReadAll
                                              };
        // The single -table parameter of the CLI and the fluent list end up in the same place
        List<string> tables = this.Parameters.Tables.Concat([this.Parameters.Table])
                                  .Where(x => !string.IsNullOrEmpty(x))
                                  .Select(x => x!)
                                  .ToList();
        foreach (string table in tables)
        {
            // A custom name can only mean one model, so it is dropped as soon as more than one table is read
            configuration.Entities.Add(CreateEntity(table, this.Parameters, tables.Count == 1 ? this.Parameters.Name : null));
        }
        if (!string.IsNullOrEmpty(this.Parameters.StoredProcedure))
        {
            configuration.StoredProcedures.Add(new TsqlReadStoredProcedure
                                               {
                                                   Schema = this.Parameters.Schema,
                                                   Name = this.Parameters.StoredProcedure!
                                               });
        }

        this.resolver.Create<TsqlReader>().Read(configuration);

        return this.SuccessAsync();
    }

    /// <summary>
    /// Splits a qualified "schema.table" entry. Only the entry's own schema is set here - an unqualified name
    /// falls back to the configuration's schema while reading, so a later change still reaches it.
    /// </summary>
    private static TsqlReadEntity CreateEntity(string table, TsqlReadCommandParameters parameters, string? name)
    {
        int separator = table.LastIndexOf('.');
        return new TsqlReadEntity
               {
                   Schema = separator < 0 ? null : table.Substring(0, separator),
                   Table = separator < 0 ? table : table.Substring(separator + 1),
                   Namespace = parameters.Namespace,
                   Name = name
               };
    }
}
