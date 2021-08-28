﻿using System;
using System.Collections.Generic;
using System.Linq;
using KY.Core;
using KY.Generator.Transfer;
using KY.Generator.Transfer.Readers;
using KY.Generator.Tsql.Configurations;
using KY.Generator.Tsql.Language;
using KY.Generator.Tsql.Transfers;
using KY.Generator.Tsql.Type;

namespace KY.Generator.Tsql.Readers
{
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
            TsqlTypeReader typeReader = new TsqlTypeReader(configuration.Connection);
            foreach (TsqlReadEntity readEntity in configuration.Entities)
            {
                ModelTransferObject model;
                if (!string.IsNullOrEmpty(readEntity.Table))
                {
                    List<TsqlColumn> columns = typeReader.GetColumns(readEntity.Schema ?? configuration.Schema, readEntity.Table);
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
                                                 Type = new TypeTransferObject { Name = column.Type, IsNullable = column.IsNullable }
                                             });
                    }
                    transferObjects.Add(model);
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
                EntityTransferObject entity = new EntityTransferObject
                                              {
                                                  Name = model.Name,
                                                  Model = model,
                                                  Table = readEntity.Table,
                                                  Schema = readEntity.Schema ?? configuration.Schema
                                              };
                if (!string.IsNullOrEmpty(readEntity.Table))
                {
                    typeReader.GetPrimaryKeys(readEntity.Schema ?? configuration.Schema, readEntity.Table)
                              .Select(x => new EntityKeyTransferObject { Name = x.Name })
                              .ForEach(entity.Keys.Add);
                    List<TsqlNavigationProperty> navigationProperties = typeReader.GetNavigationProperties(readEntity.Schema ?? configuration.Schema, readEntity.Table);
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
                transferObjects.Add(entity);
            }
            foreach (TsqlReadStoredProcedure readStoredProcedure in configuration.StoredProcedures)
            {
                string schema = readStoredProcedure.Schema ?? configuration.Schema;
                //List<TsqlColumn> columns = typeReader.GetColumnsFromStoredProcedure(schema, readStoredProcedure.Name);
                StoredProcedureTransferObject storedProcedure = new StoredProcedureTransferObject { Schema = schema, Name = readStoredProcedure.Name };
                storedProcedure.ReturnType = new TypeTransferObject { Name = "void", FromSystem = true };
                transferObjects.Add(storedProcedure);
            }
        }

        private void Validate(TsqlReadConfiguration configuration)
        {
            //if (string.IsNullOrEmpty(configuration.Connection))
            //{
            //    throw new InvalidConfigurationException("Tsql setting without connection found. Connection can not be null or empty");
            //}
            //foreach (TsqlReadEntity entity in configuration.Entities)
            //{
            //    if (string.IsNullOrEmpty(entity.Schema ?? configuration.Schema))
            //    {
            //        throw new InvalidConfigurationException($"Tsql entity '{entity.Name ?? "without name"}' {nameof(entity.Schema)} can not be null or empty");
            //    }
            //    if (string.IsNullOrEmpty(entity.Table) && string.IsNullOrEmpty(entity.StoredProcedure))
            //    {
            //        throw new InvalidConfigurationException($"Tsql entity '{entity.Name ?? "without name"}' have to has at leas a {nameof(entity.Table)} or {nameof(entity.StoredProcedure)} filled");
            //    }
            //}
        }
    }
}
