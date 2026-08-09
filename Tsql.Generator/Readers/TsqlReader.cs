using KY.Core;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Language;
using KY.Generator.Tsql.Transfers;
using KY.Generator.Tsql.Type;

namespace KY.Generator.Tsql.Readers;

public class TsqlReader : ITransferReader
{
    private readonly List<ITransferObject> transferObjects;

    public TsqlReader(List<ITransferObject> transferObjects)
    {
        this.transferObjects = transferObjects;
    }

    public void Read(TsqlReadConfiguration configuration)
    {
        this.Validate(configuration);
        TsqlTypeReader typeReader = new(configuration.Connection);
        if (configuration.ReadAll)
        {
            // Discovered tables are appended, so an explicitly listed one is not read twice
            List<TsqlTable> tables = typeReader.GetTables(configuration.Schema);
            Logger.Trace($"Found {tables.Count} table(s) to read");
            foreach (TsqlTable table in tables)
            {
                if (configuration.Entities.Any(x => IsSame(x, table, configuration)))
                {
                    continue;
                }
                configuration.Entities.Add(new TsqlReadEntity
                                           {
                                               Schema = table.Schema,
                                               Table = table.Name,
                                               Namespace = configuration.Namespace
                                           });
            }
        }
        foreach (TsqlReadEntity readEntity in configuration.Entities)
        {
            string schema = ResolveSchema(readEntity.Schema, configuration);
            ModelTransferObject model;
            if (!string.IsNullOrEmpty(readEntity.Table))
            {
                List<TsqlColumn> columns = typeReader.GetColumns(schema, readEntity.Table);
                model = new ModelTransferObject
                        {
                            Name = readEntity.Name ?? readEntity.Table,
                            Namespace = readEntity.Namespace ?? configuration.Namespace,
                            Language = TsqlLanguage.Instance
                        };
                foreach (TsqlColumn column in columns)
                {
                    model.Properties.Add(new PropertyTransferObject
                                         {
                                             Name = column.Name,
                                             Type = new TypeTransferObject { Name = column.Type, IsNullable = column.IsNullable },
                                             IsNullable = column.IsNullable,
                                             IsOptional = column.IsNullable,
                                             DeclaringType = model
                                         });
                }
                this.transferObjects.Add(model);
            }
            else
            {
                //TODO: Implement for StoredProcedure
                model = new ModelTransferObject
                        {
                            Name = readEntity.Name ?? readEntity.StoredProcedure,
                            Namespace = readEntity.Namespace ?? configuration.Namespace,
                            Language = TsqlLanguage.Instance
                        };
            }
            EntityTransferObject entity = new()
                                          {
                                              Name = model.Name,
                                              Model = model,
                                              Table = readEntity.Table,
                                              Schema = schema
                                          };
            if (!string.IsNullOrEmpty(readEntity.Table))
            {
                typeReader.GetPrimaryKeys(schema, readEntity.Table)
                          .Select(x => new EntityKeyTransferObject { Name = x.Name })
                          .ForEach(entity.Keys.Add);
            }
            foreach (TsqlReadEntityKeyAction action in readEntity.KeyActions)
            {
                switch (action.Action.ToLowerInvariant())
                {
                    case "remove":
                    case "delete":
                        if (action.All)
                        {
                            entity.Keys.Clear();
                        }
                        else
                        {
                            entity.Keys.Remove(entity.Keys.FirstOrDefault(x => x.Name.Equals(action.Name, StringComparison.InvariantCultureIgnoreCase)));
                        }
                        break;
                    case "add":
                    case "insert":
                        entity.Keys.Add(new EntityKeyTransferObject { Name = action.Name });
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown entity key action {action.Action} found");
                }
            }
            foreach (EntityKeyTransferObject key in entity.Keys)
            {
                key.Property = entity.Model.Properties.FirstOrDefault(x => x.Name == key.Name).AssertIsNotNull(key.Name, $"Key {key.Name} has no matching property");
                key.Type = key.Property.Type;
            }
            this.transferObjects.Add(entity);
        }
        foreach (TsqlReadStoredProcedure readStoredProcedure in configuration.StoredProcedures)
        {
            string schema = ResolveSchema(readStoredProcedure.Schema, configuration);
            //List<TsqlColumn> columns = typeReader.GetColumnsFromStoredProcedure(schema, readStoredProcedure.Name);
            StoredProcedureTransferObject storedProcedure = new() { Schema = schema, Name = readStoredProcedure.Name };
            storedProcedure.ReturnType = new TypeTransferObject { Name = "void", FromSystem = true };
            this.transferObjects.Add(storedProcedure);
        }
    }

    /// <summary>
    /// The schema a single entity is read from: its own, the one of the whole read, or T-SQL's default. Only a
    /// discovery read (UseAll) works without one - a named table has to be looked up somewhere.
    /// </summary>
    private const string DefaultSchema = "dbo";

    private static string ResolveSchema(string? entitySchema, TsqlReadConfiguration configuration)
    {
        return entitySchema ?? configuration.Schema ?? DefaultSchema;
    }

    private static bool IsSame(TsqlReadEntity entity, TsqlTable table, TsqlReadConfiguration configuration)
    {
        return string.Equals(entity.Table, table.Name, StringComparison.InvariantCultureIgnoreCase)
               && string.Equals(ResolveSchema(entity.Schema, configuration), table.Schema, StringComparison.InvariantCultureIgnoreCase);
    }

    private void Validate(TsqlReadConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.Connection))
        {
            throw new InvalidOperationException("Tsql setting without connection found. Connection can not be null or empty");
        }
        if (!configuration.ReadAll && configuration.Entities.Count == 0 && configuration.StoredProcedures.Count == 0)
        {
            throw new InvalidOperationException($"Tsql setting without anything to read found. Set at least one table or use {nameof(ITsqlFromDatabaseOrReadSyntax.UseAll)}()");
        }
        foreach (TsqlReadEntity entity in configuration.Entities)
        {
            if (string.IsNullOrEmpty(entity.Table) && string.IsNullOrEmpty(entity.StoredProcedure))
            {
                throw new InvalidOperationException($"Tsql entity '{entity.Name ?? "without name"}' have to has at least a {nameof(entity.Table)} or {nameof(entity.StoredProcedure)} filled");
            }
        }
    }
}
